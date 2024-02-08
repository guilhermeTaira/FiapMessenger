using ConsumerWorker.Listener;
using MassTransit;

namespace ConsumerWorker.Configuration;

public static class MassTransitConfig
{
    public static void AddMassTransitConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var server = configuration.GetSection("MassTransit")["Server"];
        var queue = configuration.GetSection("MassTransit")["QueueName"];
        var user = configuration.GetSection("MassTransit")["User"];
        var password = configuration.GetSection("MassTransit")["Password"];

        services.AddMassTransit((x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(server, "/", h =>
                {
                    h.Username(user);
                    h.Password(password);
                });

                cfg.ReceiveEndpoint(queue, e =>
                {
                    e.Consumer<OrderListener>();
                });

                cfg.ConfigureEndpoints(context);
            });

            x.AddConsumer<OrderListener>();
        }));
    }
}
