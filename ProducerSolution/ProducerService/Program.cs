using Microsoft.Extensions.DependencyInjection;
using Producer.Application;
using Producer.Infrastructure;
using ProducerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IFileFetcher, FileFetcher>();
builder.Services.AddSingleton<IFileProcessor, FileProcessor>();
builder.Services.AddSingleton<IConverterStrategy, XmlToCsvConverter>();
builder.Services.AddSingleton<ProducerEvents>();
//builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();

var host = builder.Build();
host.Run();
