using RabbitComputerHelper.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitComputerHelper.Jobs;

internal class TemperatureProbeJob : IJob
{
    private readonly IProbeService _probeService;
    private const string QueueName = "esp8266_amqp";
    private const string HostName = "192.168.1.2";
    private const string UserName = "test";
    private const string Password = "test";

    public TemperatureProbeJob(IProbeService probeService)
    {
        _probeService = probeService;
    }

    public string Name => "TemperatureProbe";

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
            queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        Console.WriteLine("Waiting for Temperature Probe messages.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received: {message}");
            await _probeService.ParseAndSaveProbeDataAsync(message);
        };

        while (true)
        {
            await channel.BasicConsumeAsync(QueueName, autoAck: true, consumer: consumer);
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
