﻿using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;
using RabbitComputerHelper.Repositories.DatabaseContexts;

namespace RabbitComputerHelper.Repositories
{
    public class UnclassifiedMessageRepository : AbstractRepository, IUnclassifiedMessageRepository
    {
        private readonly EventLogDatabaseContext _context;

        public UnclassifiedMessageRepository(EventLogDatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<UnclassifiedMessage>> ListAsync()
        {
            return await _context.UnclassifiedMessage
                .OrderByDescending(um => um.CreatedDate)
                .ToListAsync();
        }
    }
}
