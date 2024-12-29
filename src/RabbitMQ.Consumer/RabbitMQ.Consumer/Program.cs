using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new();
factory.Uri = new("your-amqp-uri");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.QueueDeclare(queue: "example-queue", exclusive: false);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example-queue", autoAck: false, consumer: consumer);

consumer.Received += async (sender, eventArgs) =>
{
    byte[] body = eventArgs.Body.ToArray(); 
    string message = Encoding.UTF8.GetString(body);
    Console.WriteLine(message);
    await Task.Delay(200);
    channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
};
    
Console.Read();