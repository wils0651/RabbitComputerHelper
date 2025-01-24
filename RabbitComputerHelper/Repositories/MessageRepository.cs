using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DatabaseContext _context;

        public MessageRepository(DatabaseContext context)
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

        public async Task AddMessageAsync(Message message)
        {
            await _context.AddAsync(message);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
