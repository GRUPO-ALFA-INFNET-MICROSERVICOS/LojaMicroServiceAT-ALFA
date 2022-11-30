using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using StoreService_AT.Model;
using StoreService_AT.MessageBuss;

namespace StoreService_AT.RabbitMQ.Sender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection _connection;

        public RabbitMQMessageSender()
        {
            var hostname = Environment.GetEnvironmentVariable("AMQP_HOSTNAME");
            var password = Environment.GetEnvironmentVariable("AMQP_PASSWORD");
            var username = Environment.GetEnvironmentVariable("AMQP_USERNAME");
            if (hostname == null || password == null || username == null)
            {
                hostname = "rabbitmq";
                password = "guest";
                username = "guest";
                Console.WriteLine("Nao foi encontrado as variaveis de ambiente para o rabbitmq. Usando o padrao");
            }
            _hostName = hostname;
            _password = password;
            _userName = username;
        }
        public void SendMessage(BaseMessage message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };
            _connection = factory.CreateConnection();

            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);
            byte[] body = GetMessageAsByteArray(message);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }

        private byte[] GetMessageAsByteArray(object message)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize<Store>((Store)message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }
    }
}
