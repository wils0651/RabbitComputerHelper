using RabbitComputerHelper.Constants;
using RabbitComputerHelper.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitComputerHelper.Jobs
{
    internal class EventLogJob : IJob
    {
        private readonly IMessageService _messageService;

        public EventLogJob(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public string Name => "EventLog";

        public async Task RunAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = RabbitMqConstants.HostName,
                UserName = RabbitMqConstants.UserName,
                Password = RabbitMqConstants.Password
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: RabbitMqConstants.EventLogQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine("Waiting for EventLog messages.");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received: {message}");
                await _messageService.ParseAndSaveMessageAsync(message);
            };

            while (true)
            {
                await channel.BasicConsumeAsync(RabbitMqConstants.EventLogQueueName, autoAck: true, consumer: consumer);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}
