using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Services
{
    public class GarageDistanceService : IGarageDistanceService
    {
        private readonly IGarageDistanceRepository _garageDistanceRepository;
        private readonly IUnclassifiedMessageService _unclassifiedMessageService;

        public GarageDistanceService(
            IGarageDistanceRepository garageDistanceRepository,
            IUnclassifiedMessageService unclassifiedMessageService)
        {
            _garageDistanceRepository = garageDistanceRepository;
            _unclassifiedMessageService = unclassifiedMessageService;
        }

        public async Task ParseAndSaveDistanceMessageAsync(string messagePhrase)
        {
            if (messagePhrase == null || string.IsNullOrEmpty(messagePhrase))
            {
                return;
            }

            var createdDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

            if (!decimal.TryParse(messagePhrase, out var distance))
            {
                messagePhrase += $"| Received: {createdDate:g}";
                await _unclassifiedMessageService.CreateAndSaveUnclassifiedMessageAsync(messagePhrase);
                return;
            }

            var garageDistance = new GarageDistance { CreatedDate = createdDate, Distance = distance };

            await _garageDistanceRepository.AddAsync(garageDistance);
            await _garageDistanceRepository.SaveChangesAsync();
        }
    }
}
