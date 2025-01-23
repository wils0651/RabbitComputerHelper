using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts
{
    public interface IComputerRepository
    {
        public Task<Computer> GetByIdAsync(int computerId);

        public Task<Computer> GetByNameAsync(string name);
    }
}
