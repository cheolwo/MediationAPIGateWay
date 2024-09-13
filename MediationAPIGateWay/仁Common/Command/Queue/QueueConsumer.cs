using MediatR;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace 仁Common.Command.Queue
{
    public class QueueConsumer
    {
        private readonly IMediator _mediator;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public QueueConsumer(IMediator mediator)
        {
            _mediator = mediator;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void StartListening(string queueName, Func<string, IRequest> commandFactory)
        {
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message} from {queueName}");

                // 메시지를 핸들러로 전달
                var command = commandFactory(message);
                await _mediator.Send(command);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            Console.WriteLine($" [*] Waiting for messages on {queueName}");
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
