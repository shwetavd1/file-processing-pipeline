using Consumer.Application;
using Consumer.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Runtime.CompilerServices;
using System.Text;


namespace Consumer.Infrastructure
{
    public class RabbitMQConsumer: IMessageConsumer<string>
    {
        private readonly IConnection _connection;
        private IModel? _channel;
        private static int _counter = 1;
        public event EventHandler<MessageData<string>>? OnMessageReceived;  // rabbitMQ messages are transmitted as bytes not strings, so we need to convert bytes to string before invoking the event. // MessageData<string> means that the message data will be of type string, which is the CSV content in this case.

        public RabbitMQConsumer(RabbitMQConnection connectionProvider)
        {
            _connection = connectionProvider.GetConnection();
        }
        public async Task ConsumeAsync()
        {
            Console.WriteLine("Connecting to RabbitMQ...");

            _channel = _connection.CreateModel();

            string queueName = "file-processing-pipeline-20260418-2210";

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageString = Encoding.UTF8.GetString(body);

                Console.WriteLine("Message received from RabbitMQ");
                Console.WriteLine(messageString);

                var messageData = new MessageData<string>
                {
                    Id = _counter++,
                    Content = messageString
                };

                OnMessageReceived?.Invoke(this, messageData);
            };

            _channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer
            );

            await Task.CompletedTask;
        }
    }
}
