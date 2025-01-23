using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts
{
    public interface IMessageRepository
    {
        public Task<List<Message>> ListMessagesAsync();

        public Task AddMessageAsync(Message message);

        public Task SaveChangesAsync();
    }
}
