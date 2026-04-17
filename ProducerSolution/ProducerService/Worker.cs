using Producer.Application;

namespace ProducerService
{
    /* BackgroundService - built in class
     * runs background tasks as long as the service is running
     * polls the folders
     * processes incoming xml files
     * converts to csv
     * publishes the result to message queue
     * runs continuously
     */
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

        //batch processor - entry point for background service
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // folder path to fetch the files
            var folderPath = "D:\\Files";
            
            //infinite loop to keep the service running
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine("fetching files");

                    /* fetching xml files
                     * reads all xml files
                     * loads content
                     * returns List<FileData>
                     */
                    var files = await _fileFetcher.GetFilesAsync(folderPath);

                    // per file processing loop
                    foreach(var file in files)
                    {
                        try
                        {
                            //process files - validation - checks content
                            var processedFile = await _fileProcessor.ProcessAsync(file);

                            //convert xml to csv
                            var csvData = await _converterStrategy.ConvertAsync(processedFile.Content);

                            //event raised - triggers subscribed event
                            _events.RaiseFileProcessed(processedFile.FileName);

                            //logging
                            Console.WriteLine($"CSV generated file for: {processedFile.FileName}");
                            Console.WriteLine(csvData);

                            /* publish to rabbitMQ
                             * sends csv data as message
                             */
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
                /* wait 10 sec, time interval
                 * polls files every 10 sec
                 */
                await Task.Delay(10000, stoppingToken); 
            }
        }
    }
}
