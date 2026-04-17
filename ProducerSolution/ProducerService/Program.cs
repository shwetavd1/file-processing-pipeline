using Producer.Application;
using Producer.Infrastructure;
using ProducerService;

/*Host built in class
* provides dependency services, background services
*/
var builder = Host.CreateApplicationBuilder(args);

// register worker class to intiate the background service
builder.Services.AddHostedService<Worker>();

// register dependency services
builder.Services.AddSingleton<IFileFetcher, FileFetcher>();
builder.Services.AddSingleton<IFileProcessor, FileProcessor>();
builder.Services.AddSingleton<IConverterStrategy, XmlToCsvConverter>();
builder.Services.AddSingleton<ProducerEvents>();
builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();

// create application instance from the above configuration
var host = builder.Build();
// starts application and keeps backgorund services running
host.Run();
