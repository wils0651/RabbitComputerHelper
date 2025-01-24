using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts
{
    public interface IUnclassifiedMessageRepository
    {
        Task AddMessageAsync(UnclassifiedMessage unclassifiedMessage);
        Task<List<UnclassifiedMessage>> ListAsync();
        Task SaveChangesAsync();
    }
}
