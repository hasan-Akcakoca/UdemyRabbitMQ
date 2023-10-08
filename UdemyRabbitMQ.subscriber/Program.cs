using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace UdemyRabbitMQ.subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            // RabbitMQ bağlantı ayarlarını yapılandırma
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

            // Kuyruk ismi
            var queueName = "direct-queue-Critical";

            // Consumer'a sadece bir mesajı işleme yetkisi ver (prefetchCount: 1)
            channel.BasicQos(0, 1, false);

            // Belirtilen kuyruğu dinleyen EventingBasicConsumer oluşturma
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queueName, false, consumer);

            Console.WriteLine("Logları dinleniyor...");

            // Mesaj geldiğinde çalışacak event handler
            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                // Mesajı işleme simülasyonu (Thread.Sleep ile 1.5 saniye bekleme)
                Thread.Sleep(1500);
                Console.WriteLine("Gelen Mesaj:" + message);

                // Mesaj başarıyla işlendi, acknowledgment gönderme
                //Bu şekilde, mesajlar doğru bir şekilde işlenene kadar yeni mesajlar alınmaz, böylece işlem sırasında hataların oluşması engellenir.
                channel.BasicAck(e.DeliveryTag,multiple: false);
            };

            Console.ReadLine();
        }



    }
}
