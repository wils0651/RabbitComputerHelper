using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts
{
    public interface IComputerRepository
    {
        public Task AddAsync<T>(T entity) where T : class;

        public Task<Computer?> GetByIdAsync(int computerId);

        public Task<Computer?> GetByNameAsync(string name);

        Task SaveChangesAsync();
    }
}
