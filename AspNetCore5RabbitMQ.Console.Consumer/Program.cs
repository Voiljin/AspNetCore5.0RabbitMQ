using AspNetCore5RabbitMQ.Common.Models.ShoppingModels;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace AspNetCore5RabbitMQ.Console.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "TakeOrderQueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, mq) =>
            {
                var data = Encoding.UTF8.GetString(mq.Body.ToArray());
                var consumeData = JsonConvert.DeserializeObject<TakeOrderRequest>(data);

                System.Console.WriteLine(String.Format("{0} ID user has bought the product named {1}", consumeData.CustomerId, consumeData.ProductName));

            };

            channel.BasicConsume(queue: "TakeOrderQueue",
                                 autoAck: true,//true ise datayı kuyruktan siler
                                 consumer: consumer);

            System.Console.ReadKey();

        }
    }
}
