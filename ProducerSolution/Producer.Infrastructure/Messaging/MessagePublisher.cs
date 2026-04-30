using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using Producer.Application.Configurations;
using Producer.Application.Interfaces;

namespace Producer.Infrastructure.Messaging
{
    public class MessagePublisher : IMessagePublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly RabbitMqSettings _settings;
        private readonly ILogger<MessagePublisher> _logger;
        private readonly object _lock = new object();
        public MessagePublisher(IOptions<RabbitMqSettings> options, ILogger<MessagePublisher> logger)
        {
            _logger =  logger;
            _settings = options.Value
                ?? throw new ArgumentNullException(nameof(options));
            _logger.LogInformation("Initializing RabbitMQ publisher");

            //connection
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName
            };
            _connection = factory.CreateConnection();
            //channel
            _channel = _connection.CreateModel();
            // DLQ
            _channel.QueueDeclare(
                queue: _settings.DeadLetter.Queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // main queue
            _channel.QueueDeclare(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: BuildQueueArguments()
               );
        }

        public Task Publish(string message, string fileName)
        {
            var body = Encoding.UTF8.GetBytes(message);
            lock (_lock)
            {
                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.MessageId = Guid.NewGuid().ToString();
                properties.Headers = new Dictionary<string, object>
            {
                {"file-name", fileName },
                {"source", "file-processing-producer" },
                {"created-at", DateTime.UtcNow.ToString("o") }
            };
                try
                {
                    _channel.BasicPublish(
                        exchange: "",
                        routingKey: _settings.QueueName,
                        basicProperties: properties,
                        body: body);
                    _logger.LogInformation("Message published successfully | File:{FileName}", fileName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to publish message for file : {fileName}", fileName);
                    throw;
                }

                Console.WriteLine($"Message sent | MessageId: {properties.MessageId} | File: {fileName}");
                Console.WriteLine(message);
                return Task.CompletedTask;
            }
        }
        private IDictionary<string, object> BuildQueueArguments()
        {
            var args = new Dictionary<string, object>();

            if (_settings.MessageTtlMs > 0)
                args["x-message-ttl"] = _settings.MessageTtlMs;

            if (_settings.MaxQueueLength > 0)
                args["x-max-length"] = _settings.MaxQueueLength;

            if (_settings.DeadLetter != null)
            {
                args["x-dead-letter-exchange"] =
                    _settings.DeadLetter.Exchange;

                args["x-dead-letter-routing-key"] =
                    _settings.DeadLetter.RoutingKey;
            }

            return args;
        }
        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
