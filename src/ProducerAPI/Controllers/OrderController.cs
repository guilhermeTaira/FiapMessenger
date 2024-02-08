using Core;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ProducerAPI.Controllers;

[ApiController]
[Route("/Order")]
public class OrderController : ControllerBase
{
    private readonly IBus _bus;
    private readonly IConfiguration _configuration;

    public OrderController(IBus bus, IConfiguration configuration)
    {
        _bus = bus;
        _configuration = configuration;
    }

    [HttpPost("rabbitmq")]
    public IActionResult PostOrderWithRabbitMq(Order order)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        using var connection = factory.CreateConnection();
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(
                queue: "queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message = JsonSerializer.Serialize(order);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: "queue",
                basicProperties: null,
                body: body);
        }

        return Ok(order);
    }

    [HttpPost("masstransit")]
    public async Task<IActionResult> PutOrderWithMassTransit(Order order)
    {
        var queueName = _configuration.GetSection("MassTransit")["QueueName"];
        
        var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{queueName}"));

        await endpoint.Send(order);

        return Ok(order);
    }
}
