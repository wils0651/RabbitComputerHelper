using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts
{
    public interface IComputerTaskRepository
    {
        Task AddAsync(ComputerTask computerTask);
        public Task<ComputerTask?> GetByIdAsync(int computerTaskId);

        public Task<ComputerTask?> GetByNameAsync(string name);
        Task SaveChangesAsync();
    }
}
