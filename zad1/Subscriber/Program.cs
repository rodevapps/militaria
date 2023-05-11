#nullable disable
using System.Text;
using RabbitMQ.Client;
using System.Net.Mail;
using System.Text.Json;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory
{
    HostName = "localhost"
};

var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare("orders", exclusive: false);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Message received: {message}");

    Response response = Newtonsoft.Json.JsonConvert.DeserializeObject<Response>(message);

    Console.WriteLine($"Response type: {response.Type}");
    Console.WriteLine($"Response message: {JsonSerializer.Serialize(response.Message)}");

    if (response.Type == "EmailMessage") {
        SendEmailMessage(response.Message);
    } else if (response.Type == "SmsMessage") {
        SendSmsMessage(response.Message);
    } else if (response.Type == "LocalMessage") {
        SendLocalMessage(response.Message);
    }
};

channel.BasicConsume(queue: "orders", autoAck: true, consumer: consumer);

Console.ReadKey();


void SendEmailMessage(Message m)
{
    MailMessage message = new MailMessage(m.From, m.To);

    message.Subject = m.Title;
    message.Body = m.Body;

    SmtpClient client = new SmtpClient("localhost");
    client.UseDefaultCredentials = false;

    try
    {
        Console.WriteLine("Sending email message => From: \"{0}\", to: \"{1}\", subject: \"{2}\", body: \"{3}\"", m.From, m.To, m.Title, m.Body);
        //client.Send(message);
    } catch (Exception ex) {
        Console.WriteLine("Exception caught during email sending(): {0}", ex.ToString());
    }
}

void SendSmsMessage(Message m)
{
    Console.WriteLine("Sending sms message => From: \"{0}\", to: \"{1}\", body: \"{2}\"", m.From, m.To, m.Body);
}

void SendLocalMessage(Message m)
{
    Console.WriteLine("Sending local message: \"{0}\"", m.Body);
}