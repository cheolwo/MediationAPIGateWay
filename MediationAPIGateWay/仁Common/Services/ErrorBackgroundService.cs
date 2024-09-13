using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using 仁Common.Command;
using System.Text.Json;
using Microsoft.Extensions.Hosting;

namespace 仁Common.Services
{
    public class ErrorBackgroundService : BackgroundService
    {
        private readonly ILogger<ErrorBackgroundService> _logger;
        private readonly IMediator _mediator;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public ErrorBackgroundService(ILogger<ErrorBackgroundService> logger, IMediator mediator, IConfiguration configuration)
        {
            _logger = logger;
            _mediator = mediator;

            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                Port = int.Parse(configuration["RabbitMQ:Port"]),
                UserName = configuration["RabbitMQ:Username"],
                Password = configuration["RabbitMQ:Password"]
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // 오류 처리 전용 Queue 선언
            _channel.QueueDeclare(queue: "ErrorQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConsumeQueue(stoppingToken);
            return Task.CompletedTask;
        }

        private void ConsumeQueue(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // 오류 Command를 처리
                var errorCommand = JsonSerializer.Deserialize<ErrorCommand>(message);

                // Mediator를 통해 ErrorCommandHandler로 전송
                if (errorCommand != null)
                {
                    await _mediator.Send(errorCommand, stoppingToken);
                }

                _channel.BasicAck(ea.DeliveryTag, multiple: false);  // 메시지 처리 완료 시 Ack 전송
            };

            _channel.BasicConsume(queue: "ErrorQueue", autoAck: false, consumer: consumer);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
