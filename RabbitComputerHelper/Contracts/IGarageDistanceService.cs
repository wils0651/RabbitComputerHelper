
namespace RabbitComputerHelper.Contracts
{
    public interface IGarageDistanceService
    {
        Task ParseAndSaveDistanceMessageAsync(string messagePhrase);
    }
}
