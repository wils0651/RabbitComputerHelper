using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts
{
    public interface IMessageRepository
    {
        public Task<List<Message>> ListMessagesAsync();

        public Task AddAsync<T>(T entity) where T : class;

        public Task SaveChangesAsync();
    }
}
