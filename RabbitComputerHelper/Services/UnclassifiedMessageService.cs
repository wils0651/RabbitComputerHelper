using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Services;

public class UnclassifiedMessageService : IUnclassifiedMessageService
{
    private readonly IUnclassifiedMessageRepository _unclassifiedMessageRepository;

    public UnclassifiedMessageService(IUnclassifiedMessageRepository unclassifiedMessageRepository)
    {
        _unclassifiedMessageRepository = unclassifiedMessageRepository;
    }

    public async Task CreateAndSaveUnclassifiedMessageAsync(string messageContent)
    {
        var unclassifiedMessage = new UnclassifiedMessage(messageContent);

        await _unclassifiedMessageRepository.AddAsync(unclassifiedMessage);
        await _unclassifiedMessageRepository.SaveChangesAsync();
    }
}