//using Producer.Application;
//using RabbitMQ.Client;
//using System.Text;

//namespace Producer.Infrastructure
//{
//    public class MessagePublisher : IMessagePublisher
//    {
//        private readonly IConnection _connection;
//        private readonly IModel _channel;

//        public MessagePublisher()
//        {
//            var factory = new ConnectionFactory()
//            {
//                HostName = "localhost"
//            };

//            _connection = factory.CreateConnection();
//            _channel = _connection.CreateModel();

//            _channel.QueueDeclare(
//                queue: "file-processing-queue",
//                durable: false,
//                exclusive: false,
//                autoDelete: false,
//                arguments: null);
//        }

//        public void Publish(string message)
//        {
//            var body = Encoding.UTF8.GetBytes(message);

//            _channel.BasicPublish(
//                exchange: "",
//                routingKey: "file-processing-queue",
//                basicProperties: null,
//                body: body);

//            Console.WriteLine("Message sent to RabbitMQ");
//        }
//    }
//}