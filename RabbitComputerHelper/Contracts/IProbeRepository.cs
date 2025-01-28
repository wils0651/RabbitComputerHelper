using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Contracts;

public interface IProbeRepository
{
    Task<Probe?> GetByNameAsync(string probeName);
}