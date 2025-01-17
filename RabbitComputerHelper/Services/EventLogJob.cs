using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using RabbitComputerHelper.Contracts;

namespace RabbitComputerHelper.Services
{
    internal class EventLogJob
    {
        const string queueName = "eventLog_queue";
        const string HostName = "192.168.1.2";
        const string UserName = "test";
        const string Password = "test";
        private readonly IMessageService _messageService;

        public EventLogJob(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task RunAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine("Waiting for messages.");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received: {message}");
                await _messageService.ParseAndSaveMessageAsync(message);

                //return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
