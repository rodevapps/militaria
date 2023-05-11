using Producer.Message;
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
        MessageObj email = new()
            {
                From = "example@example.com",
                To = "example2@example2.com",
                Title = "Title",
                Body = "Body",
            };

        try {
            _messagePublisher.SendMessage(new {Type="EmailMessage", Message=email});
        } catch (Exception e) {
            return Ok(new { status = "error", message = e.ToString() });
        }

        MessageObj sms = new()
            {
                From = "123456789",
                To = "987654321",
                Body = "Body",
            };

        try {
            _messagePublisher.SendMessage(new {Type="SmsMessage", Message=sms});
        } catch (Exception e) {
            return Ok(new { status = "error", message = e.ToString() });
        }

        MessageObj local = new()
            {
                Body = "Body",
            };

        try {
            _messagePublisher.SendMessage(new {Type="LocalMessage", Message=local});
        } catch (Exception e) {
            return Ok(new { status = "error", message = e.ToString() });
        }

        return Ok(new { status = "success", message = "Email sended successfully!" });
    }
}