﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitComputerHelper;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Repositories;
using RabbitComputerHelper.Services;

var services = new ServiceCollection();

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

IConfiguration configuration = builder.Build();

// Jobs
services.AddSingleton<EventLogJob>();
services.AddSingleton<TemperatureProbeJob>();

// Services
services.AddScoped<IMessageService, MessageService>();
services.AddScoped<IProbeService, ProbeService>();
services.AddScoped<IUnclassifiedMessageService, UnclassifiedMessageService>();

// Repositories
services.AddScoped<IComputerRepository, ComputerRepository>();
services.AddScoped<IComputerTaskRepository, ComputerTaskRepository>();
services.AddScoped<IMessageRepository, MessageRepository>();
services.AddScoped<IUnclassifiedMessageRepository, UnclassifiedMessageRepository>();
services.AddScoped<IProbeRepository, ProbeRepository>();
services.AddScoped<IProbeDataRepository, ProbeDataRepository>();

services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

var serviceProvider = services.BuildServiceProvider();

var eventLogJob = serviceProvider.GetService<EventLogJob>();
var temperatureProbeJob = serviceProvider.GetService<TemperatureProbeJob>();

//await eventLogJob.RunAsync();
//await temperatureProbeJob.RunAsync();

var eventLogTask = eventLogJob.RunAsync();
var temperatureProbeTask = temperatureProbeJob.RunAsync();

await Task.WhenAll(eventLogTask, temperatureProbeTask);
