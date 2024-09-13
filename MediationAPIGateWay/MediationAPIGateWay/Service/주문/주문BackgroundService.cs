using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using MediatR;
using 仁주문자.For주문자;
using 仁Common.Command.Queue;
using Polly;

namespace MediationAPIGateWay.Service.주문
{
    public class 주문BackgroundService : BackgroundService
    {
        private readonly ILogger<주문BackgroundService> _logger;
        private readonly IMediator _mediator;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;

        public 주문BackgroundService(ILogger<주문BackgroundService> logger, IMediator mediator, IConfiguration configuration)
        {
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"],
                Port = int.Parse(_configuration["RabbitMQ:Port"]),
                UserName = _configuration["RabbitMQ:Username"],
                Password = _configuration["RabbitMQ:Password"]
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // 각 Queue 설정을 설정 파일에서 가져와 선언
            DeclareQueue<BeforeOrderCommand>();
            DeclareQueue<MainOrderCommand>();
            DeclareQueue<AfterOrderCommand>();
        }

        private void DeclareQueue<T>() where T : IQueueableCommand, new()
        {
            var queueCommand = new T();
            var queueName = queueCommand.GetQueueName();

            // 설정 파일에서 Queue 설정을 가져옴
            var durableSetting = _configuration[$"RabbitMQ:Queues:{queueName}:Durable"];
            var autoDeleteSetting = _configuration[$"RabbitMQ:Queues:{queueName}:AutoDelete"];

            if (string.IsNullOrWhiteSpace(durableSetting) || string.IsNullOrWhiteSpace(autoDeleteSetting))
            {
                throw new ArgumentException($"Queue configuration for {queueName} is invalid or missing.");
            }

            var durable = bool.Parse(durableSetting);
            var autoDelete = bool.Parse(autoDeleteSetting);

            // Queue 선언
            _channel.QueueDeclare(queue: queueName, durable: durable, exclusive: false, autoDelete: autoDelete, arguments: null);

            _logger.LogInformation($"Declared queue {queueName} with durable={durable}, autoDelete={autoDelete}");
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 각 Queue에 대해 메시지 소비자를 등록
            ConsumeQueue<BeforeOrderCommand>(stoppingToken);
            ConsumeQueue<MainOrderCommand>(stoppingToken);
            ConsumeQueue<AfterOrderCommand>(stoppingToken);

            return Task.CompletedTask;
        }

        private void ConsumeQueue<T>(CancellationToken stoppingToken) where T : IQueueableCommand, new()
        {
            var queueCommand = new T();
            var queueName = queueCommand.GetQueueName();

            var retryPolicy = Policy.Handle<Exception>()
                                    .RetryAsync(3, onRetry: (exception, retryCount, context) =>
                                    {
                                        _logger.LogWarning($"Retry {retryCount} for {queueName} due to {exception.Message}");
                                    });

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    await retryPolicy.ExecuteAsync(async () =>
                    {
                        // 메시지를 Command로 변환
                        var command = JsonSerializer.Deserialize(message, typeof(T)) as IRequest;

                        // Command를 Mediator를 통해 처리
                        if (command != null)
                        {
                            await _mediator.Send(command);
                        }

                        _channel.BasicAck(ea.DeliveryTag, multiple: false);  // 메시지 처리 완료 시 Ack 전송
                        _logger.LogInformation($"Processed message from {queueName}");
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to process message from {queueName}: {message} after retries");
                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);  // 실패한 메시지 재시도
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            _logger.LogInformation($"Consuming messages from {queueName}");
        }


        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
