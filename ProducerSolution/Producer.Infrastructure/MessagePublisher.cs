using Producer.Application;
using RabbitMQ.Client;
using System.Text;

namespace Producer.Infrastructure
{
    public class MessagePublisher : IMessagePublisher
    {
        // connection to rabbitMQ server
        private readonly IConnection _connection;
        // channel used to publish the messages
        private readonly IModel _channel;

        /* connects to rabbitMQ
         * creates communication channel
         * declares queue
         */
        public MessagePublisher()
        {
            // rabbitMQ connection configuration
            var factory = new ConnectionFactory()
            {
                // server running on same machine
                HostName = "localhost"
            };

            // opens tcp connection to rabbitMQ broker
            _connection = factory.CreateConnection();
            // creates channel - virtual connection
            _channel = _connection.CreateModel();

            // queue declaration
            _channel.QueueDeclare(
                // queue name
                queue: "file-processing-queue",
                // not persisted on disk
                durable: false,
                // accessible by other connections
                exclusive: false,
                // queue stays even if unused
                autoDelete: false,
                // no advanced setting
                arguments: null);
        }

        // takes csv string - sends to rabbitMQ
        public void Publish(string message)
        {
            // convert string to bytes - because rabbitMQ messages are transmitted as bytes not strings
            // UTF8 standard encoding
            var body = Encoding.UTF8.GetBytes(message);
            // send message
            _channel.BasicPublish(
                // default rabbitMQ exchange
                exchange: "",
                // queue name
                routingKey: "file-processing-queue",
                // no headers or metadata
                basicProperties: null,
                // message as bytes
                body: body);
            // logging
            Console.WriteLine("Message sent to RabbitMQ");
        }
    }
}