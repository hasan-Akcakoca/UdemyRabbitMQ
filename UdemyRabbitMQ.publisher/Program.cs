using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace UdemyRabbitMQ.publisher
{
    public enum LogNames
    {
        Critical=1,
        Error=2,
        Warning=3,
        Info=4
    }


    class Program
    {
        static void Main(string[] args)
        {
            // RabbitMQ bağlantı ayarlarını yapılandırma
            /*
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://uhshoatb:4tRfDsemduk6BCrsZaIvfQgOhLsMtf-t@fish.rmq.cloudamqp.com/uhshoatb");
            */
            // RabbitMQ sunucusuna bağlanmak için ConnectionFactory oluşturulur.
            var factory = new ConnectionFactory
            {
                HostName = "192.168.1.109",
                UserName = "my",
                Password = "1",
                Port = 5672
            };



            // RabbitMQ'ya bağlantı açma
            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            // Exchange oluşturma (direct tipinde ve kalıcı)
            channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

            // LogNames enumundaki her bir log tipi için kuyruk oluşturma ve bind işlemi yapma
            Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
            {
                var routeKey = $"route-{x}";
                var queueName = $"direct-queue-{x}";

                //kuyruğu oluşturur veya var olan bir kuyruğa erişim sağlar
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
                // channel.QueueDeclare(queueName, true, false, false);




                // Oluşturulan kuyruğu ve exchange'i routeKey ile bind etme
                // channel.QueueBind(queueName, "logs-direct", routeKey, null);
                channel.QueueBind(queue: queueName,exchange: "logs-direct",routingKey: routeKey,arguments: null);


            });

            // Rastgele log mesajları üreterek ilgili kuyruklara gönderme
            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                LogNames log = (LogNames)new Random().Next(1, 5);
                string message = $"log-type: {log}";
                var messageBody = Encoding.UTF8.GetBytes(message);
                var routeKey = $"route-{log}";

                // Mesajı ilgili exchange'e ve routeKey'e gönderme
                channel.BasicPublish("logs-direct", routeKey, null, messageBody);
                Console.WriteLine($"Log gönderilmiştir : {message}");
            });

            Console.ReadLine();
        }
    }

}
