using RabbitComputerHelper.Constants;
using RabbitComputerHelper.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitComputerHelper.Jobs;

internal class TemperatureProbeJob : IJob, IDisposable
{
    private readonly IProbeService _probeService;

    private IConnection _connection;
    private IChannel _channel;
    private AsyncEventingBasicConsumer _consumer;
    private static readonly CancellationTokenSource _cts = new();

    public TemperatureProbeJob(IProbeService probeService)
    {
        _probeService = probeService;
    }

    public string Name => "TemperatureProbe";

    public async Task RunAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = RabbitMqConstants.HostName,
            UserName = RabbitMqConstants.UserName,
            Password = RabbitMqConstants.Password
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: RabbitMqConstants.TemperatureQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        Console.WriteLine("Waiting for Temperature Probe messages.");

        _consumer = new AsyncEventingBasicConsumer(_channel);

        _consumer.ReceivedAsync += OnMessageReceived;

        await _channel.BasicConsumeAsync(RabbitMqConstants.TemperatureQueueName, autoAck: false, consumer: _consumer);
    }

    private async Task OnMessageReceived(object sender, BasicDeliverEventArgs e)
    {
        var body = e.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        Console.WriteLine($"Received: {message}");

        await _probeService.ParseAndSaveProbeDataAsync(message);

        await _channel.BasicAckAsync(deliveryTag: e.DeliveryTag, multiple: false);
    }

    public static void Stop()
    {
        _cts.Cancel();

        Console.WriteLine("Temperature Probe consumer stopping...");
    }

    public void Dispose()
    {
        _consumer.ReceivedAsync -= OnMessageReceived;
        _channel?.CloseAsync();
        _connection?.CloseAsync();
        _channel?.Dispose();
        _connection?.Dispose();
        _cts?.Dispose();
        Console.WriteLine("Temperature Probe Consumer disposed.");
    }
}
