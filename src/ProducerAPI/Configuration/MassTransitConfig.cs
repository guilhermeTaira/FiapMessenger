using MassTransit;

namespace ProducerAPI.Configuration;

public static class MassTransitConfig
{
    public static void AddMassTransitConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var server = configuration.GetSection("MassTransit")["Server"];
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

                cfg.ConfigureEndpoints(context);
            });
        }));
    }
}
