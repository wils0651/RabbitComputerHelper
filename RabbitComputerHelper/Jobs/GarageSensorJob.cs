using RabbitComputerHelper.Constants;
using RabbitComputerHelper.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitComputerHelper.Jobs
{
    internal class GarageSensorJob : IJob
    {
        private readonly IGarageDistanceService _garageDistanceService;

        public GarageSensorJob(IGarageDistanceService garageDistanceService)
        {
            this._garageDistanceService = garageDistanceService;
        }

        public string Name => "GarageSensor";

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
                queue: RabbitMqConstants.GarageSensorQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine("Waiting for Garage Sensor messages.");

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
                await channel.BasicConsumeAsync(RabbitMqConstants.GarageSensorQueueName, autoAck: true, consumer: consumer);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}
