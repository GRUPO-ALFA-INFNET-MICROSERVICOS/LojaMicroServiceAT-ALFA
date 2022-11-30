﻿using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using StoreService_AT.Repository;
using StoreService_AT.Model;
using System.Text;
using System.Text.Json;
using StoreService_AT.Model.VOs;

namespace StoreService_AT.RabbitMQ.Consumer
{
    public class RabbitMQMessageConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQMessageConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
            var factory = new ConnectionFactory
            {
                HostName = hostname,
                UserName = username,
                Password = password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "createStoreQueue", false, false, false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                StoreVO store = JsonSerializer.Deserialize<StoreVO>(content);
                ProcessStore(store).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("createStoreQueue", false, consumer);
            return Task.CompletedTask;
        }


        private async Task ProcessStore(StoreVO storevo)
        {
            Store store = new()
            {
                Id = storevo.Id,
                StoreName = storevo.StoreName,
                Telephone = storevo.Telephone,
                Products =  storevo.Products,
                StoreAdress = storevo.StoreAdress,
            };
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IStoreRepository scopedProcessingService =
                    scope.ServiceProvider.GetRequiredService<IStoreRepository>();

                await scopedProcessingService.CreateStore(store);
            };
        }
    }
}
