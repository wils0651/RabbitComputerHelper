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

services.AddSingleton<EventLogJob>();
services.AddSingleton<TemperatureProbeJob>();

services.AddScoped<IMessageService, MessageService>();
services.AddScoped<IProbeService, ProbeService>();
services.AddScoped<IUnclassifiedMessageService, UnclassifiedMessageService>();

services.AddScoped<IComputerRepository, ComputerRepository>();
services.AddScoped<IComputerTaskRepository, ComputerTaskRepository>();
services.AddScoped<IMessageRepository, MessageRepository>();
services.AddScoped<IProbeDataRepository, ProbeDataRepository>();
services.AddScoped<IProbeRepository, ProbeRepository>();
services.AddScoped<IUnclassifiedMessageRepository, UnclassifiedMessageRepository>();

services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

var serviceProvider = services.BuildServiceProvider();

var eventLogJob = serviceProvider.GetService<EventLogJob>();
var temperatureProbeJob = serviceProvider.GetService<TemperatureProbeJob>();

if (eventLogJob == null || temperatureProbeJob == null)
{
    Console.WriteLine("Failed to resolve dependencies.");
    return;
}

while (true)
{
    var eventLogTask = eventLogJob.RunAsync();
    var temperatureProbeTask = temperatureProbeJob.RunAsync();

    await Task.WhenAll(eventLogTask, temperatureProbeTask);

    // Optionally add a delay to prevent tight loop
    await Task.Delay(TimeSpan.FromMinutes(1));
}
