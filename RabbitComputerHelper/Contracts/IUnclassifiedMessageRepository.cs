using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts
{
    public interface IUnclassifiedMessageRepository
    {
        public Task AddAsync<T>(T entity) where T : class;

        Task<List<UnclassifiedMessage>> ListAsync();

        Task SaveChangesAsync();
    }
}
