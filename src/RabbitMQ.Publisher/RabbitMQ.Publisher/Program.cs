using System.Text;
using RabbitMQ.Client;

ConnectionFactory factory = new();
factory.Uri = new("your-amqp-uri");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.QueueDeclare(queue: "example-queue", exclusive:false);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(50);
    byte[] message = Encoding.UTF8.GetBytes("hello world " + i);
    channel.BasicPublish(exchange: "", routingKey: "example-queue", basicProperties: null, body: message);
}

Console.Read();