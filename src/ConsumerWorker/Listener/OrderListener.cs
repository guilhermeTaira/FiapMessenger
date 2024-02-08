using Core;
using MassTransit;

namespace ConsumerWorker.Listener;

public class OrderListener : IConsumer<Order>
{
    public Task Consume(ConsumeContext<Order> context)
    {
        Console.WriteLine($"MassTransit - {context.Message}");
        return Task.CompletedTask;
    }
}
