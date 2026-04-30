using Producer.Application.Configurations;
using Producer.Application.Conversion;
using Producer.Application.Interfaces;
using Producer.Application.Messaging;
using Producer.Application.Processing;
using Producer.Application.Services;
using Producer.Domain.Entities;
using Producer.Infrastructure.Conversion;
using Producer.Infrastructure.Messaging;
using Producer.Infrastructure.Processing;
using Producer.Infrastructure.Storage;
using ProducerService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IFileFetcher, FileFetcher>();
builder.Services.AddSingleton<IFileConverter, XmlToCsvConverter>();
builder.Services.AddSingleton<IConverterFactory, ConverterFactory>();
builder.Services.AddSingleton<IFileProcessor<FileData, string>, FileProcessor>();
builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();
builder.Services.Configure<FileSettings>(builder.Configuration.GetSection("FileSettings"));
builder.Services.AddSingleton<IPollingService>(new IntervalPollingService(TimeSpan.FromSeconds(10)));
builder.Services.AddSingleton<IFileProcessingOrchestrator, FileProcessingOrchestrator>();
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));
builder.Services.AddSingleton<IFilePipelineExecutor, FilePipelineExecutor>();
builder.Services.AddSingleton<IFileValidator, FileValidator>();
builder.Services.AddSingleton<IProcessedFileStore, InMemoryProcessedFileStore>();

var host = builder.Build();
host.Run();
