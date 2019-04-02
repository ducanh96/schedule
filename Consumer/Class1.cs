using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    public class Class1
    {
        public static void Main(string[] args)
        {
            string exchangeName = "HelloWorld_RabbitMQ";
            string queueName = "HelloQueue";
            // send queue
            string UserName = "guest";
            string Password = "guest";
            string HostName = "localhost";
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory()
            {
                UserName = UserName,
                Password = Password,
                HostName = HostName
            };

            using (IConnection connection = connectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);

                    channel.QueueDeclare(queue: queueName,
                                                     durable: false,
                                                     exclusive: false,
                                                     autoDelete: false,
                                                     arguments: null);

                    channel.QueueBind(queueName, exchangeName, string.Empty);

                    Console.WriteLine("Waiting for messages");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("Received - {0}", message);
                    };
                    channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);

                    Console.ReadLine();

                }
            }
        }
    }
}
