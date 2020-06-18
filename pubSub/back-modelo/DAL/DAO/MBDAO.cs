using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;
using back_modelo.DAL.Models;
using System.Text;
using RabbitMQ.Client;

namespace back_modelo.DAL.DAO
{
    public class MBDAO : IMBDAO
    {
        public void EnviarConsumer(string tipo, string info)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            string mensagem = "";

            if(tipo == "I") {
                mensagem = "Pessoa " + info + " inserida com Sucesso as " + DateTime.Now;
            }
            if(tipo == "A") {
                mensagem = "Pessoa com o ID: " + info + " atualizada com Sucesso as " + DateTime.Now;
            }
            if(tipo == "D") {
                mensagem = "Uma Pessoa foi removida do banco de dados. ID: " + info + " as " + DateTime.Now;
            }
            using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
						channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
						
                        var body = Encoding.UTF8.GetBytes(mensagem);

                        channel.BasicPublish(exchange: "logs",
                                routingKey: "",
                                basicProperties: null,
                                body: body);
                    }

                }
        }
    }
}