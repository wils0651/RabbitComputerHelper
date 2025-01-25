using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Repositories
{
    public class MessageRepository : AbstractRepository, IMessageRepository
    {
        private readonly DatabaseContext _context;

        public MessageRepository(DatabaseContext context): base(context)
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

        public async Task AddAsync(Message message)
        {
            await _context.AddAsync(message);
        }
    }
}
