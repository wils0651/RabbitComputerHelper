﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Jobs;
using RabbitComputerHelper.Repositories;
using RabbitComputerHelper.Repositories.DatabaseContexts;
using RabbitComputerHelper.Services;

var services = new ServiceCollection();

string environment = Environment.GetEnvironmentVariable("APP_ENV") ?? "Development";

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

IConfiguration configuration = builder.Build();

// Jobs
services.AddSingleton<IJob, EventLogJob>();
services.AddSingleton<IJob, TemperatureProbeJob>();
services.AddSingleton<IJob, GarageSensorJob>();

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

// Database Contexts
services.AddDbContext<EventLogDatabaseContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

services.AddDbContext<ProbeDatabaseContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

services.AddDbContext<GarageSensorContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

var serviceProvider = services.BuildServiceProvider();

// Run Jobs
var jobNameAndShouldBeRun = configuration
    .GetSection("Jobs")
    .GetChildren()
    .ToDictionary(x => x.Key, x => x.Value);

var jobDictionary = serviceProvider
    .GetServices<IJob>()
    .ToDictionary(x => x.Name, x => x);

var jobsToRun = new List<IJob>();

foreach (var jobName in jobNameAndShouldBeRun.Keys)
{
    if (jobNameAndShouldBeRun.TryGetValue(jobName, out var shouldRunBool) && bool.TryParse(shouldRunBool, out var shouldRunJob) && shouldRunJob)
    {
        if(!jobDictionary.TryGetValue(jobName, out var job))
        {
            throw new InvalidOperationException($"Failed to get job {jobName}");
        }

        Console.WriteLine($"Adding Job: {jobName}.");

        jobsToRun.Add(job);
    }
    else
    {
        Console.WriteLine($"Not running job: {jobName}.");
    }
}

if (jobsToRun.Count == 0)
{
    Console.WriteLine("No Jobs to run.");
    return;
}

var jobTasks = new List<Task>();
foreach (var job in jobsToRun)
{
    jobTasks.Add(job.RunAsync());
}

await Task.WhenAll(jobTasks);
