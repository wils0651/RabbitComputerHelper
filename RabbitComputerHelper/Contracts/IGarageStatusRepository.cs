using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts;

public interface IGarageStatusRepository
{
    public Task<GarageStatus?> GetStatusForDistance(decimal distance);
}