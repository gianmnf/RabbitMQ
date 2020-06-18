using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace projetoExemploConsumer3
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
					channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

							var queueName = channel.QueueDeclare().QueueName;
							channel.QueueBind(queue: queueName,
                              exchange: "logs",
                              routingKey: "");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.Span;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(message);
                    };
                    channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);
                    
                    Console.WriteLine("Consumer 3 Funcionando");
                    Console.ReadLine();
                }
            }
    }
    }
}