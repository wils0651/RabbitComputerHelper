using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Services
{
    public class GarageDistanceService : IGarageDistanceService
    {
        private readonly IGarageDistanceRepository _garageDistanceRepository;
        private readonly IUnclassifiedMessageService _unclassifiedMessageService;
        private readonly IGarageStatusRepository _garageStatusRepository;
        private readonly IGarageEventTypeRepository _garageEventTypeRepository;

        public GarageDistanceService(
            IGarageDistanceRepository garageDistanceRepository,
            IUnclassifiedMessageService unclassifiedMessageService,
            IGarageStatusRepository garageStatusRepository,
            IGarageEventTypeRepository garageEventTypeRepository)
        {
            _garageDistanceRepository = garageDistanceRepository;
            _unclassifiedMessageService = unclassifiedMessageService;
            _garageStatusRepository = garageStatusRepository;
            _garageEventTypeRepository = garageEventTypeRepository;
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

            await CheckStatusAndAddEvent(garageStatus, createdDate);

            var garageDistance = new GarageDistance
            {
                CreatedDate = createdDate.ToUniversalTime(),
                Distance = distance,
                GarageStatus = garageStatus
            };

            await _garageDistanceRepository.AddAsync(garageDistance);
            await _garageDistanceRepository.SaveChangesAsync();
        }

        private async Task CheckStatusAndAddEvent(GarageStatus? garageStatus, DateTime createdDate)
        {
            if (garageStatus == null)
            {
                return;
            }

            var lastDistanceWithStatus = await _garageDistanceRepository.GetLastWithStatusAsync();

            if (lastDistanceWithStatus == null || lastDistanceWithStatus.GarageStatusId == garageStatus.GarageStatusId)
            {
                return;
            }

            if (!lastDistanceWithStatus.GarageStatusId.HasValue)
            {
                throw new InvalidDataException($"GarageDistance {lastDistanceWithStatus.GarageDistanceId} has no status");
            }

            var garageEventType = _garageEventTypeRepository.GetEventTypeByStatusIds(
                lastDistanceWithStatus.GarageStatusId.Value, garageStatus.GarageStatusId);

            // Create a garageEventLog and add to repository
        }
    }
}
