using Consumer.Application;
using Consumer.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


namespace Consumer.Infrastructure
{
    public class RabbitMQConsumer: IMessageConsumer
    {
        public event Action<MessageData>? OnMessageReceived;
        public async Task ConsumeAsync()
        {
            Console.WriteLine("Connecting to RabbitMQ...");
            var factory = new ConnectionFactory() 
            { 
                HostName = "localhost" 
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            string queueName = "file-processing-queue";

            channel.QueueDeclare
            (
                queue: queueName, 
                durable: false, 
                exclusive: false, 
                autoDelete: false, 
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageString = Encoding.UTF8.GetString(body);

                Console.WriteLine("Message received from RabbitMQ");
                Console.WriteLine(messageString);

                var messageData = new MessageData
                {
                    Id = 1, // generate unique id for message
                    CsvContent = messageString
                };
                OnMessageReceived?.Invoke(messageData); // trigger event when message is received
                
            };

            channel.BasicConsume
            (
                queue: queueName, 
                autoAck: true, 
                consumer: consumer
            );

            await Task.CompletedTask; // this keeps the method async, but actual work is done in event handler
        }
    }
}
