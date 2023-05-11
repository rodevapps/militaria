using System.Text;
using RabbitMQ.Client;
using System.Net.Mail;
using RabbitMQ.Client.Events;

using Subscriber.Email;

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
};

channel.BasicConsume(queue: "orders", autoAck: true, consumer: consumer);

Console.ReadKey();


void SendMessage(EmailMessage m)
{
    MailMessage message = new MailMessage(m.From, m.To);

    message.Subject = m.Title;
    message.Body = m.Body;

    SmtpClient client = new SmtpClient("localhost");
    client.UseDefaultCredentials = false;

    try
    {
        client.Send(message);
    } catch (Exception ex) {
        Console.WriteLine("Exception caught during email sending(): {0}", ex.ToString());
    }
}