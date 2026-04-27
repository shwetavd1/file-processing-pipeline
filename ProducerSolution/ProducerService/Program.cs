using Producer.Application.Interfaces;
using Producer.Application.Services;
using Producer.Domain;
using Producer.Infrastructure;
using ProducerService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IFileFetcher, FileFetcher>();
builder.Services.AddSingleton<IFileConversion, XmlToCsvConverter>();
builder.Services.AddSingleton<IConverterFactory, ConverterFactory>();
builder.Services.AddSingleton<IFileProcessor<FileData, string>, FileProcessor>();
builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();
builder.Services.Configure<FileSettings>(builder.Configuration.GetSection("FileSettings"));
builder.Services.AddSingleton<IPollingService>(new IntervalPollingService(TimeSpan.FromSeconds(10)));
builder.Services.AddSingleton<IFileProcessingOrchestrator, FileProcessingOrchestrator>();
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));

var host = builder.Build();
host.Run();
