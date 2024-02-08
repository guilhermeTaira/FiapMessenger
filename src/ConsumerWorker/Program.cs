using ConsumerWorker;
using ConsumerWorker.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();

        // MassTransit
        services.AddMassTransitConfig(hostContext.Configuration);
    })
    .Build();

host.Run();
