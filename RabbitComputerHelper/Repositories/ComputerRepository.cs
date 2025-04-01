using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;
using RabbitComputerHelper.Repositories.DatabaseContexts;

namespace RabbitComputerHelper.Repositories
{
    public class ComputerRepository : AbstractRepository, IComputerRepository
    {
        private readonly EventLogDatabaseContext _context;

        public ComputerRepository(EventLogDatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Computer?> GetByIdAsync(int computerId)
        {
            return await _context.Computer
                .FirstOrDefaultAsync(c => c.ComputerId == computerId);
        }

        public async Task<Computer?> GetByNameAsync(string name)
        {
            return await _context.Computer
                .FirstOrDefaultAsync(c => c.Name.Equals(name));
        }
    }
}
