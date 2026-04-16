using Consumer.Application;
using Consumer.Domain;
using System.Text.Json;

namespace ConsumerService
{
    /*
     worker subscribes to message events and processed them asynchronously by converting CSV data into structured objects, 
    handling errors and updating status accordingly.
     */
    public class Worker : BackgroundService
    {
        //private readonly ILogger<Worker> _logger;
        private readonly IMessageConsumer _consumer;
        private readonly ICsvToJsonConverter _converter;
        private readonly IStatusTracker _statusTracker;


        public Worker(IMessageConsumer consumer, ICsvToJsonConverter converter, IStatusTracker statusTracker) // DI injects dependencies
        {
            //stores object
            _consumer = consumer;
            _converter = converter;
            _statusTracker = statusTracker;

            //subscribe to event
            _consumer.OnMessageReceived += HandleMessageReceived; // this tells us like when event happen call this method
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            /*while (!stoppingToken.IsCancellationRequested)
            {
                //trigger message consumption
                await _consumer.ConsumeAsync();
                await Task.Delay(5000, stoppingToken); //execute loop for every 5 seconds
            }*/
            try
            {
                await _consumer.ConsumeAsync(); // this will start consuming messages and will call HandleMessageReceived when message is received
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private async void HandleMessageReceived(MessageData message)
        {
            try 
            {
                Console.WriteLine($"Processing message {message.Id}");
                _statusTracker.MarkAsPending(message.Id); // mark as pending
                var result = await _converter.ConvertAsync(message.CsvContent);// convert csv-> json
                //foreach(var item in result)//process data
                //{
                //    Console.WriteLine($"Processed: {item.Id}, {item.Name}, {item.Age}");
                //}
                /*await Parallel.ForEachAsync(result, async (item, token) =>       // instead of one by one processing it will process multiple message together
                {
                    Console.WriteLine($"Processed: {item.Id}, {item.Name}, {item.Age}");
                    await Task.Delay(100);//simulate processing
                });*/
                var json = JsonSerializer.Serialize(result, new JsonSerializerOptions //converts object to JSON string
                {
                    WriteIndented = true //this makes JSON formatted
                });
                Console.WriteLine("JSON OUTPUT:");
                Console.WriteLine(json); //prints actual json data
                _statusTracker.MarkAsCompleted(message.Id);//mark as complete
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                _statusTracker.MarkAsFailed(1, ex.Message);// mark failed
            }
        }
    }
}
