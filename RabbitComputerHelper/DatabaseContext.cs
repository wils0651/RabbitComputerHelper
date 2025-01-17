﻿using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Computer> Computer { get; set; }
        public DbSet<Models.ComputerTask> ComputerTask { get; set; }
        public DbSet<Message> Message { get; set; }
    }
}
