﻿using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;
using RabbitComputerHelper.Repositories.DatabaseContexts;

namespace RabbitComputerHelper.Repositories
{
    public class MessageRepository : AbstractRepository, IMessageRepository
    {
        private readonly EventLogDatabaseContext _context;

        public MessageRepository(EventLogDatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Message>> ListMessagesAsync()
        {
            return await _context.Message
                .Include(m => m.ComputerTask)
                .Include(m => m.Computer)
                .OrderByDescending(m => m.CreatedDate)
                .ToListAsync();
        }
    }
}
