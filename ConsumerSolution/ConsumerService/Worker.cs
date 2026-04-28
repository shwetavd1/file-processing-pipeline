using Consumer.Application;
using Consumer.Domain;
using System.Text.Json;

namespace ConsumerService
{
    public class Worker : BackgroundService
    {
        private readonly IMessageConsumer<string> _consumer;
        private readonly ICsvToJsonConverter<Dictionary<string,object>> _converter; 
        private readonly IStatusTracker _statusTracker;
        public Worker(IMessageConsumer<string> consumer, ICsvToJsonConverter<Dictionary<string,object>> converter, IStatusTracker statusTracker)
        {
            _consumer = consumer;
            _converter = converter;
            _statusTracker = statusTracker;

            _consumer.OnMessageReceived += async (sender, message) =>
            {
                await HandleMessageReceived(message); //subscribe to the event
            };
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Worker started. Waiting for messages...");
            await _consumer.ConsumeAsync(); //start consuming message from rabbitMQ
        }
        public async Task HandleMessageReceived(MessageData <string> message)
        {
            try
            {
                Console.WriteLine("Message received from RabbitMQ");
                Console.WriteLine($"Processing message {message.Id}");
                _statusTracker.MarkAsPending(message.Id);
                var result = await _converter.ConvertAsync(message.Content);

                var json = JsonSerializer.Serialize(result, new JsonSerializerOptions //converts object to JSON string
                {
                    WriteIndented = true
                });
                Console.WriteLine("JSON OUTPUT:");
                Console.WriteLine(json);
                _statusTracker.MarkAsCompleted(message.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                _statusTracker.MarkAsFailed(message.Id, ex.Message);
            }
        }
    }
}
