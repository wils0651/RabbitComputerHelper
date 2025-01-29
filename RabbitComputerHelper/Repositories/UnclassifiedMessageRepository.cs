using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Repositories
{
    public class UnclassifiedMessageRepository : AbstractRepository, IUnclassifiedMessageRepository
    {
        private readonly DatabaseContext _context;

        public UnclassifiedMessageRepository(DatabaseContext context) : base(context)
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
