using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts
{
    public interface IGarageEventTypeRepository
    {
        Task<GarageEventType?> GetEventTypeByStatusIds(int previousStatusId, int currentStatusId);
    }
}
