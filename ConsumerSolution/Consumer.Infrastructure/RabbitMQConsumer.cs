using Consumer.Application;
using Consumer.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;


namespace Consumer.Infrastructure
{
    public class RabbitMQConsumer : IMessageConsumer<string>
    {
        private readonly IConnection _connection;
        private IModel? _channel;

        //timer for time based batching
        private Timer _timer;

        //lock for thread safety
        private readonly object _lock = new();

        

        //Batch storage
        private readonly List<(MessageData<string> message, ulong deliveryTag)> _batch = new(); // store message+deliveryTag(needed for ACK)
        private const int BatchSize = 5;
        public event EventHandler<MessageData<string>>? OnMessageReceived;  // rabbitMQ messages are transmitted as bytes not strings, so we need to convert bytes to string before invoking the event. // MessageData<string> means that the message data will be of type string, which is the CSV content in this case.

        public RabbitMQConsumer(RabbitMQConnection connectionProvider)
        {
            _connection = connectionProvider.GetConnection();
            _timer = new Timer(async _ => await ProcessBatchSafe(), null, 10000, 10000); //try processing batch for every 10 sec if not full
        }
        public async Task ConsumeAsync()
        {
            Console.WriteLine("Connecting to RabbitMQ...");

            _channel = _connection.CreateModel();

            string queueName = "file-processing-pipeline-20260418-2210";

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageString = Encoding.UTF8.GetString(body);
                var messageData = new MessageData<string>
                {
                    Id = new Random().Next(1, 1000),
                    Content = messageString,
                };

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
        private async Task ProcessBatchSafe()
        {
            if (_batch.Count == 0) 
                return;
            await ProcessBatch();
        }
    }
}    
