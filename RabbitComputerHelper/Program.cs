using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitComputerHelper;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Jobs;
using RabbitComputerHelper.Repositories;
using RabbitComputerHelper.Services;

var services = new ServiceCollection();

string environment = Environment.GetEnvironmentVariable("APP_ENV") ?? "Development";

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

IConfiguration configuration = builder.Build();

// Jobs
services.AddSingleton<EventLogJob>();
services.AddSingleton<TemperatureProbeJob>();
services.AddSingleton<GarageSensorJob>();

// Services
services.AddScoped<IMessageService, MessageService>();
services.AddScoped<IProbeService, ProbeService>();
services.AddScoped<IUnclassifiedMessageService, UnclassifiedMessageService>();
services.AddScoped<IGarageDistanceService, GarageDistanceService>();

// Repositories
services.AddScoped<IComputerRepository, ComputerRepository>();
services.AddScoped<IComputerTaskRepository, ComputerTaskRepository>();
services.AddScoped<IMessageRepository, MessageRepository>();
services.AddScoped<IProbeDataRepository, ProbeDataRepository>();
services.AddScoped<IProbeRepository, ProbeRepository>();
services.AddScoped<IUnclassifiedMessageRepository, UnclassifiedMessageRepository>();
services.AddScoped<IGarageDistanceRepository, GarageDistanceRepository>();

services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

var serviceProvider = services.BuildServiceProvider();

var eventLogJob = serviceProvider.GetService<EventLogJob>();
var temperatureProbeJob = serviceProvider.GetService<TemperatureProbeJob>();
var garageSensorJob = serviceProvider.GetService<GarageSensorJob>();

if (eventLogJob == null || temperatureProbeJob == null || garageSensorJob == null)
{
    Console.WriteLine("Failed to resolve dependencies.");
    return;
}

var eventLogTask = eventLogJob.RunAsync();
var temperatureProbeTask = temperatureProbeJob.RunAsync();
var garageSensorTask = garageSensorJob.RunAsync();

await Task.WhenAll(eventLogTask, temperatureProbeTask);
