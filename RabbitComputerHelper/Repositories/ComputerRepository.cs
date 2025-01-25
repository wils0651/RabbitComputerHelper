using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Repositories
{
    public class ComputerRepository : AbstractRepository, IComputerRepository
    {
        private readonly DatabaseContext _context;

        public ComputerRepository(DatabaseContext context) : base(context)
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

        public async Task AddAsync(Computer computer)
        {
            await _context.AddAsync(computer);
        }
    }
}
