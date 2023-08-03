//Classe responsavel por consumir a interface de ~publicação de restaurante via RabbitMQ
using RabbitMQ.Client;
using RestauranteService.Dtos;
using System.Text;
using System.Text.Json;

namespace RestauranteService.RabbitMqClient
{
    public class RabbitMqClient : IRabbitMqClient
    {
        //Criando configurações para utilizar RabbitMQ
        private readonly IConfiguration _configuration;
        //Criando conexão para utilizar RabbitMQ
        private readonly IConnection _connection;
        //Criando canal de trafego dentro do mensageiro
        private readonly IModel _channel;

        public RabbitMqClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new ConnectionFactory() { HostName = _configuration["RabbitMqHost"], Port = Int32.Parse(_configuration["RabbitMqPort"]) }.CreateConnection();
            _channel = _connection.CreateModel();
            //Como os dados serão trafegados dentro dos canais. Exchange sera criada quando for designada (trigger) utilizando AMQP (comunicação entre serviços do RabbitMQ) (Fanout)
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        }

        public void PublicaRestaurante(RestauranteReadDto restauranteReadDto)
        {
            //Serializando a mensagem
            string mensagem = JsonSerializer.Serialize(restauranteReadDto);
            //Convertendo a mensagem para bytes que possam ser enviados (não podemos enviar a string diretamente)
            var body = Encoding.UTF8.GetBytes(mensagem);

            //Enviando corpo da mensagem através do canal aberto na nossa conexão entre restaurante service e rabbitmq
            //Enviamos a exchange, chave de roteamento (chave para identificar onde vai trafegar a informação por dentro do RabbitMQ)
            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
        }
    }
}
