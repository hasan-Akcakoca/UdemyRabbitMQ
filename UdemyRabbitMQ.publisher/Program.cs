using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace UdemyRabbitMQ.publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://uhshoatb:4tRfDsemduk6BCrsZaIvfQgOhLsMtf-t@fish.rmq.cloudamqp.com/uhshoatb");
            */

            var factory = new ConnectionFactory
            {
                HostName = "192.168.1.109",
                UserName = "my",
                Password = "1",
                Port = 5672
            };


            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

           // channel.QueueDeclare("hello-queue", true, false, false);
           //yada
            channel.QueueDeclare(queue: "hello-queue", durable: true, exclusive: false, autoDelete: false);


            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {

                string message = $"Message {x}";

                var messageBody = Encoding.UTF8.GetBytes(message);

               // channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);
               //yada
                channel.BasicPublish( exchange: string.Empty,routingKey: "hello-queue", basicProperties: null,body: messageBody);

                Console.WriteLine($"Mesaj gönderilmiştir : {message}");

            });



            Console.ReadLine();


        }
    }
}
