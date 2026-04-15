using Consumer.Application;
using Consumer.Infrastructure;
using ConsumerService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IMessageConsumer, MessageConsumer>();
builder.Services.AddSingleton<ICsvToJsonConverter, CsvToJsonConverter>();
builder.Services.AddSingleton<IStatusTracker, StatusTracker>();

var host = builder.Build();
host.Run();
