using Consumer.Application;
using Consumer.Infrastructure;
using ConsumerService;

var builder = Host.CreateApplicationBuilder(args); 

builder.Services.AddHostedService(provider => provider.GetRequiredService<Worker>());

builder.Services.AddSingleton<Worker>();
builder.Services.AddSingleton<RabbitMQConnection>(); //only one connection for entire application
builder.Services.AddTransient<IMessageConsumer<string>, RabbitMQConsumer>(); // multiple consumers
builder.Services.AddTransient<ICsvToJsonConverter<Dictionary<string,object>>, CsvToJsonConverter>();
builder.Services.AddTransient<IStatusTracker, StatusTracker>();

var host = builder.Build(); 
host.Run(); 
