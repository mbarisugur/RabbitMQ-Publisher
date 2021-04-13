using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ_Publisher
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Info = 3,
        Warning = 4
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://klcrkeba:JnfCJH6Kdo4H2rNQ9iM9GO-8u71VjOHv@coyote.rmq.cloudamqp.com/klcrkeba")
            };

            using (var connection =  factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("DefaultQueue", durable: true, false,false,null);

                    Array log_name_array = Enum.GetValues(typeof(LogNames));

                    for (int i = 1; i < 11; i++)
                    {
                        Random rnd = new Random();

                        LogNames log1 = (LogNames)log_name_array.GetValue(rnd.Next(log_name_array.Length));
                        LogNames log2 = (LogNames)log_name_array.GetValue(rnd.Next(log_name_array.Length));
                        LogNames log3 = (LogNames)log_name_array.GetValue(rnd.Next(log_name_array.Length));

                        string RoutingKey = $"{log1}.{log2}.{log3}";
                        
                        var bodyByte = Encoding.UTF8.GetBytes($"log={log1}-{log2}-{log3}");
                        
                        var properties = channel.CreateBasicProperties();

                        properties.Persistent = true;

                        channel.BasicPublish("", routingKey: "DefaultQueue", properties, body: bodyByte);

                        Console.WriteLine($"log mesajı gönderilmiştir=> mesaj:{RoutingKey}");
                    }
                }

                Console.WriteLine("Çıkış yapmak tıklayınız..");
                Console.ReadLine();
            }
        }

        private static string GetMessage(string[] args)
        {
            return args[0].ToString();
        }
    }
}