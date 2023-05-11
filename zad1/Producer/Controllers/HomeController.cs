using Producer.Email;
using Producer.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace Producer.Controllers;

[ApiController]
[Route("api/email/send")]
public class HomeController : ControllerBase
{
    private readonly IMessageProducer _messagePublisher;

    public HomeController(IMessageProducer messagePublisher)
    {
        _messagePublisher = messagePublisher;
    }

    [HttpGet]
    public ActionResult Get()
    {
        EmailMessage email = new()
            {
                From = "example@example.com",
                To = "example2@example2.com",
                Title = "Title",
                Body = "Body",
            };

        try {
            _messagePublisher.SendMessage(email);
        } catch (Exception e) {
            return Ok(new { status = "error", message = e.ToString() });
        }

        return Ok(new { status = "success", message = "Email sended successfully!" });
    }
}