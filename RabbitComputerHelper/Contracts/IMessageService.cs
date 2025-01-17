
namespace RabbitComputerHelper.Contracts
{
    public interface IMessageService
    {
        Task ParseAndSaveMessageAsync(string messagePhrase);
    }
}
