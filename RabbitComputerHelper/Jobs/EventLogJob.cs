using RabbitComputerHelper.Constants;
using RabbitComputerHelper.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RabbitComputerHelper.Jobs
{
    internal class EventLogJob : IJob, IDisposable
    {
        private readonly IMessageService _messageService;

        private IConnection _connection;
        private IChannel _channel;
        private AsyncEventingBasicConsumer _consumer;
        private static readonly CancellationTokenSource _cts = new();

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

            _connection =  await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(
                queue: RabbitMqConstants.EventLogQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.ReceivedAsync += OnMessageReceived;

            await _channel.BasicConsumeAsync(queue: RabbitMqConstants.EventLogQueueName,
                                  autoAck: false,
                                  consumer: _consumer);

            Console.WriteLine("Consumer started.");
        }

        private async Task OnMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"Received: {message}");

            await _messageService.ParseAndSaveMessageAsync(message);

            await _channel.BasicAckAsync(deliveryTag: e.DeliveryTag, multiple: false);
        }

        public static void Stop()
        {
            _cts.Cancel();

            Console.WriteLine("Consumer stopping...");
        }

        public void Dispose()
        {
            _consumer.ReceivedAsync -= OnMessageReceived;
            _channel?.CloseAsync();
            _connection?.CloseAsync();
            _channel?.Dispose();
            _connection?.Dispose();
            _cts?.Dispose();
            Console.WriteLine("Consumer disposed.");
        }
    }
}
