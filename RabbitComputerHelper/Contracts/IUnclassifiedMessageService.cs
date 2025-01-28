namespace RabbitComputerHelper.Contracts;

public interface IUnclassifiedMessageService
{
    Task CreateAndSaveUnclassifiedMessageAsync(string messageContent);
}