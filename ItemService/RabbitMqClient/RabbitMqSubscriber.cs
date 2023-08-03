//Classe que ira receber a mensagem de restauranteService no RabbitMQ
using ItemService.EventProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ItemService.RabbitMqClient
{
    //Ficara rodando em segundo plano verificando se existe alguma mensagem a ser recebida
    public class RabbitMqSubscriber : BackgroundService
    {
        //Criando configurações para utilizar RabbitMQ
        private readonly IConfiguration _configuration;
        //Nome da fila que iremos consumir
        private readonly string _nomeDaFila;
        //Nome da fila que iremos consumir
        private readonly IConnection _connection;
        //Criando canal de trafego dentro do mensageiro
        private readonly IModel _channel;
        private IProcessaEvento _processaEvento;

        public RabbitMqSubscriber(IConfiguration configuration, IProcessaEvento processaEvento)
        {
            _configuration = configuration;
            _connection = new ConnectionFactory() { HostName = _configuration["RabbitMqHost"], Port = Int32.Parse(_configuration["RabbitMqPort"]) }.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _nomeDaFila = _channel.QueueDeclare().QueueName;
            //Fazendo o vinculo da fila com a classe que estamos instanciando. Nome da fila que vamos utlizar, exchange e routingKey
            _channel.QueueBind(queue: _nomeDaFila, exchange: "trigger", routingKey: "");
            _processaEvento = processaEvento;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Consumidor das mensagens do canal utilizado
            EventingBasicConsumer? consumidor = new EventingBasicConsumer(_channel);

            //No momento em que recebermos alguma coisa, teremos a concatenação 
            consumidor.Received += (ModuleHandle, EventArgs) =>
            {
                //Conteudo da mensagem
                ReadOnlyMemory<byte> body = EventArgs.Body;
                //Convertendo a mensagem em string
                string mensagem = Encoding.UTF8.GetString(body.ToArray());
                //Objeto que vai receber a mensagem
                _processaEvento.Processa(mensagem);
            };

            //Informando que consumimos o conteudo dessa fila, passando uma flag que ela foi consumida e quem consumiu
            _channel.BasicConsume(queue: _nomeDaFila, autoAck: true, consumer: consumidor);

            return Task.CompletedTask;
              
        }
    }
}
