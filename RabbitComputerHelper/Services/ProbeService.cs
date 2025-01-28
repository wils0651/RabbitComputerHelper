using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Services;

public class ProbeService : IProbeService
{
    private readonly IProbeRepository _probeRepository;
    private readonly IProbeDataRepository _probeDataRepository;
    private readonly IUnclassifiedMessageService _unclassifiedMessageService;

    public ProbeService(
        IProbeRepository probeRepository,
        IProbeDataRepository probeDataRepository,
        IUnclassifiedMessageService unclassifiedMessageService)
    {
        _probeRepository = probeRepository;
        _probeDataRepository = probeDataRepository;
        _unclassifiedMessageService = unclassifiedMessageService;
    }

    public async Task ParseAndSaveProbeDataAsync(string messagePhrase)
    {
        var messageParts = messagePhrase.Split(':');

        if (messageParts.Length != 2)
        {
            await _unclassifiedMessageService.CreateAndSaveUnclassifiedMessageAsync(messagePhrase);
            return;
        }
        
        var probeName = messageParts[0].Trim();
        var temperaturePhrase = messageParts[1].Trim();
        
        var createdDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
        
        var probe = await _probeRepository.GetByNameAsync(probeName);
        
        if (probe == null || !decimal.TryParse(temperaturePhrase, out var temperature))
        {
            messagePhrase += $"| Received: {createdDate:g}";
            await _unclassifiedMessageService.CreateAndSaveUnclassifiedMessageAsync(messagePhrase);
            return;
        }

        var probeData = new ProbeData
        {
            Temperature = temperature,
            CreatedDate = createdDate.ToUniversalTime(),
            Probe = probe
        };
        
        await _probeDataRepository.AddAsync(probeData);
        await _probeDataRepository.SaveChangesAsync();
        
        Console.Write("Successfully  probed");
    }
}
