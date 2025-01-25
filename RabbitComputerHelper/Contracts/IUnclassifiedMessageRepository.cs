using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts
{
    public interface IUnclassifiedMessageRepository
    {
        Task AddAsync(UnclassifiedMessage unclassifiedMessage);
        Task<List<UnclassifiedMessage>> ListAsync();
        Task SaveChangesAsync();
    }
}
