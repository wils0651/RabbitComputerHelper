using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Services
{
    public class GarageDistanceService : IGarageDistanceService
    {
        private readonly IGarageDistanceRepository _garageDistanceRepository;
        private readonly IUnclassifiedMessageService _unclassifiedMessageService;
        private readonly IGarageStatusRepository _garageStatusRepository;

        public GarageDistanceService(
            IGarageDistanceRepository garageDistanceRepository,
            IUnclassifiedMessageService unclassifiedMessageService,
            IGarageStatusRepository garageStatusRepository)
        {
            _garageDistanceRepository = garageDistanceRepository;
            _unclassifiedMessageService = unclassifiedMessageService;
            _garageStatusRepository = garageStatusRepository;
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

            var garageStatus = await _garageStatusRepository.GetStatusForDistance(distance);
            
            var garageDistance = new GarageDistance
            {
                CreatedDate = createdDate.ToUniversalTime(),
                Distance = distance,
                GarageStatus = garageStatus
            };

            await _garageDistanceRepository.AddAsync(garageDistance);
            await _garageDistanceRepository.SaveChangesAsync();
        }
    }
}
