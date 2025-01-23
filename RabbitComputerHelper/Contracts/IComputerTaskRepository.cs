using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts
{
    public interface IComputerTaskRepository
    {
        public Task<ComputerTask> GetByIdAsync(int computerTaskId);

        public Task<ComputerTask> GetByNameAsync(string name);
    }
}
