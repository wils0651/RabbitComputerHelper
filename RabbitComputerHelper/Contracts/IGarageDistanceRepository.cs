using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts
{
    public interface IGarageDistanceRepository
    {
        public Task AddAsync<T>(T entity) where T : class;
        Task<GarageDistance?> GetLastWithStatusAsync();
        public Task SaveChangesAsync();
    }
}
