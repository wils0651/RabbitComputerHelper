using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using RabbitComputerHelper.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitComputerHelper.Contracts;
using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper;

var services= new ServiceCollection();

var builder = new ConfigurationBuilder();

//builder.Sources.Add(IConfigurationSource{ })
//builder.AddJsonFile("appsettings.json", false, true);
//IConfigurationSource source = new ConfigurationS

//builder.Add(new IConfigurationSource);

IConfigurationRoot root = builder.Build();

services.AddSingleton<EventLogJob>();
services.AddScoped<IMessageService, MessageService>();

services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql("Host=192.168.1.3;Database=postgres;Username=databaseuser;Password=databaseSpecialWord"));

var serviceProvider = services.BuildServiceProvider();

var eventLogJob = serviceProvider.GetService<EventLogJob>();

await eventLogJob.RunAsync();