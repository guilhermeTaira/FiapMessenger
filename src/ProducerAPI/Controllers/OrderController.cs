using Core;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ProducerAPI.Controllers;

[ApiController]
[Route("/Order")]
public class OrderController : ControllerBase
{
    [HttpPost]
    public IActionResult PostOrder(Order order)
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
}
