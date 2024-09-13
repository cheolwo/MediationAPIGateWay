using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using 仁Common.Command.Queue;

namespace 仁Common.Command
{
    public class CommandQueuePublisher : ICommandQueuePublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;
        private readonly Policy _retryPolicy;
        private readonly ILogger<CommandQueuePublisher> _logger;

        public CommandQueuePublisher(IConfiguration configuration, ILogger<CommandQueuePublisher> logger)
        {
            _configuration = configuration;
            _logger = logger;  // Logger 주입

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"],
                Port = int.Parse(_configuration["RabbitMQ:Port"]),
                UserName = _configuration["RabbitMQ:Username"],
                Password = _configuration["RabbitMQ:Password"]
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _retryPolicy = Policy.Handle<Exception>().Retry(3, (exception, retryCount) =>
            {
                _logger.LogWarning($"Retry {retryCount} due to: {exception.Message}");
            });
        }

        public void PublishToQueue(IQueueableCommand command)
        {
            var queueName = command.GetQueueName();
            var message = JsonSerializer.Serialize(command);
            var body = Encoding.UTF8.GetBytes(message);

            // 재시도 정책을 사용한 발행 로직
            _retryPolicy.Execute(() =>
            {
                try
                {
                    _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                    _logger.LogInformation($"Sent {message} to {queueName}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to publish message to {queueName} after retries. Error: {ex.Message}");
                    throw;
                }
            });
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
    // ICommandQueuePublisher 인터페이스 정의
    public interface ICommandQueuePublisher
    {
        void PublishToQueue(IQueueableCommand command);
    }
}
