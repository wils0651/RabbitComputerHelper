using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts
{
    public interface IComputerTaskRepository
    {
        public Task AddAsync<T>(T entity) where T : class;

        public Task<ComputerTask?> GetByIdAsync(int computerTaskId);

        public Task<ComputerTask?> GetByNameAsync(string name);
        Task SaveChangesAsync();
    }
}
