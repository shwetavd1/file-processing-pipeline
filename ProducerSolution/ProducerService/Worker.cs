using Producer.Application;
using System.Collections.Generic;

namespace ProducerService
{
    public class Worker : BackgroundService //to run code continuously in background
    {
        // dependencies
        private readonly IFileFetcher _fileFetcher;
        private readonly IFileProcessor _fileProcessor;
        private readonly IConverterStrategy _converterStrategy;
        private readonly ProducerEvents _events;
        private readonly IMessagePublisher _publisher;

        //inject dependencies
        public Worker(IFileFetcher fileFetcher, IFileProcessor fileProcessor, IConverterStrategy converterStrategy, ProducerEvents events, IMessagePublisher publisher)
        {
            _fileFetcher = fileFetcher;
            _fileProcessor = fileProcessor;
            _converterStrategy = converterStrategy;
            _events = events;
            _publisher = publisher;

            // subscribed the event
            _events.OnFileProcessed += (fileName) =>
            {
                Console.WriteLine($"File Processed successfully {fileName}");
            };
        }

        //batch processor
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var folderPath = "D:\\Files";
            
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine("fetching files");
                    //fetching xml files
                    var files = await _fileFetcher.GetFilesAsync(folderPath);
                    foreach(var file in files)
                    {
                        try
                        {
                            //process files
                            var processedFile = await _fileProcessor.ProcessAsync(file);

                            //convert xml to csv
                            var csvData = await _converterStrategy.ConvertAsync(processedFile.Content);

                            _events.RaiseFileProcessed(processedFile.FileName);

                            Console.WriteLine($"CSV generated file for: {processedFile.FileName}");

                            //output
                            Console.WriteLine(csvData);

                            _publisher.Publish(csvData);

                            Console.WriteLine("Message sent to RabbitMQ");
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine($"Error processing file {file.FileName}: {ex.Message}");
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error in execution : {ex.Message}");
                }
                await Task.Delay(10000, stoppingToken); // wait 10sec, time interval
            }
        }
    }
}
