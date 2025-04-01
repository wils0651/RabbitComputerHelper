using RabbitComputerHelper.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitComputerHelper.Jobs
{
    internal class GarageSensorJob
    {
        private const string QueueName = "garage_sensor_log";
        private const string HostName = "192.168.1.2";
        private const string UserName = "test";
        private const string Password = "test";

        private readonly IGarageDistanceService _garageDistanceService;

        public GarageSensorJob(IGarageDistanceService garageDistanceService)
        {
            this._garageDistanceService = garageDistanceService;
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
                queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine("Waiting for Garage Sesnor messages.");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received Garage Distance: {message}");

                await _garageDistanceService.ParseAndSaveDistanceMessageAsync(message);
            };

            while (true)
            {
                await channel.BasicConsumeAsync(QueueName, autoAck: true, consumer: consumer);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}
