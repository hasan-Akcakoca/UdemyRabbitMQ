using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace UdemyRabbitMQ.publisher
{
    class Program
    {
        static void Main(string[] args)
        {
         //   var factory = new ConnectionFactory();
           // factory.Uri = new Uri("amqps://uhshoatb:4tRfDsemduk6BCrsZaIvfQgOhLsMtf-t@fish.rmq.cloudamqp.com/uhshoatb");



        
            var factory = new ConnectionFactory
            {
                HostName = "192.168.1.109",
                UserName = "my",
                Password = "1",
                Port= 5672
            };
          

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {

                string message = $"log {x}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("logs-fanout","", null, messageBody);
               
                Console.WriteLine($"Mesaj gönderilmiştir : {message}");
              //  Thread.Sleep(1500);
            });



            Console.ReadLine();


        }
    }
}
