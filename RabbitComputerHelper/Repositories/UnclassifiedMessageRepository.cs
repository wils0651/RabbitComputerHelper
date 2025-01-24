using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Repositories
{
    public class UnclassifiedMessageRepository : IUnclassifiedMessageRepository
    {
        private readonly DatabaseContext _context;

        public UnclassifiedMessageRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<UnclassifiedMessage>> ListAsync()
        {
            return await _context.UnclassifiedMessage
                .OrderByDescending(um => um.CreatedDate)
                .ToListAsync();
        }

        public async Task AddMessageAsync(UnclassifiedMessage unclassifiedMessage)
        {
            await _context.AddAsync(unclassifiedMessage);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
