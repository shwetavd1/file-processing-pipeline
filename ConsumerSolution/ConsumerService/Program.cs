using Consumer.Application;
using Consumer.Infrastructure;
using ConsumerService;

var builder = Host.CreateApplicationBuilder(args); // Host.CreateApplicationBuilder created application builder which is used to configure services and build the host for the application.

builder.Services.AddHostedService<Worker>(); //run worker automatically when application starts

builder.Services.AddSingleton<IMessageConsumer, MessageConsumer>();// AddSingleton registers a service as a singleton, meaning that only one instance of the service will be created and shared throughout the application's lifetime. In this case, it registers the MessageConsumer class as the implementation for the IMessageConsumer interface. Whenever a component requests an IMessageConsumer, it will receive the same instance of MessageConsumer.
builder.Services.AddSingleton<ICsvToJsonConverter, CsvToJsonConverter>();
builder.Services.AddSingleton<IStatusTracker, StatusTracker>();

var host = builder.Build(); //creates app with all dependencies
host.Run(); // start application
