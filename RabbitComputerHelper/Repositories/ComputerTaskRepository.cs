using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Repositories
{
    public class ComputerTaskRepository : AbstractRepository, IComputerTaskRepository
    {
        private readonly DatabaseContext _context;

        public ComputerTaskRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ComputerTask?> GetByIdAsync(int computerTaskId)
        {
            return await _context.ComputerTask
                .FirstOrDefaultAsync(c => c.ComputerTaskId == computerTaskId);
        }

        public async Task<ComputerTask?> GetByNameAsync(string name)
        {
            return await _context.ComputerTask
                .FirstOrDefaultAsync(c => c.Name.Equals(name));
        }
    }
}
