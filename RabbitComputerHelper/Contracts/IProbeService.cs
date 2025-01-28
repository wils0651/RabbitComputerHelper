namespace RabbitComputerHelper.Contracts;

public interface IProbeService
{
    Task ParseAndSaveProbeDataAsync(string messagePhrase);
}