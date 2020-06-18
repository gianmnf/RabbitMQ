using System;
using System.Text;
using RabbitMQ.Client;

namespace projetoExemplo
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
                    channel.QueueDeclare(queue: "Prefeitura",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    string message = "Testando o RabbitMQ!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                             routingKey: "Prefeitura",
                             basicProperties: null,
                             body: body);
                             Console.WriteLine("Mensagem Enviada!");
                }

                Console.ReadLine();
            }
        }
    }
}
