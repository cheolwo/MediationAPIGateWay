using RabbitMQ.Client;
using System.Text;

namespace Common.Services.MessageQueue
{
    public interface IRabbitMQPublisher : IDisposable
    {
        Task PublishAsync(string message, string exchangeName, string routingKey);
    }

    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQPublisher(string hostname)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQ 연결 실패: {ex.Message}");
                throw; // 예외를 상위로 전달
            }
        }

        public async Task PublishAsync(string message, string exchangeName, string routingKey)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);
                await Task.Run(() => _channel.BasicPublish(
                    exchange: exchangeName,
                    routingKey: routingKey,
                    basicProperties: null,
                    body: body)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"메시지 전송 중 오류 발생: {ex.Message}");
                throw; // 예외를 상위로 전달
            }
        }

        // IDisposable 구현
        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
