using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts
{
    public interface IComputerRepository
    {
        Task AddAsync(Computer computer);
        public Task<Computer?> GetByIdAsync(int computerId);

        public Task<Computer?> GetByNameAsync(string name);
        Task SaveChangesAsync();
    }
}
