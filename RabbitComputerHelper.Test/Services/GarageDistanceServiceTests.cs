using NSubstitute;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;
using RabbitComputerHelper.Services;

namespace RabbitComputerHelper.Test.Services;

public class GarageDistanceServiceTests
{
    private readonly IGarageDistanceRepository _garageDistanceRepository = Substitute.For<IGarageDistanceRepository>();
    private readonly IUnclassifiedMessageService _unclassifiedMessageService = Substitute.For<IUnclassifiedMessageService>();
    private readonly IGarageStatusRepository _garageStatusRepository = Substitute.For<IGarageStatusRepository>();
    private readonly IGarageEventTypeRepository _garageEventTypeRepository = Substitute.For<IGarageEventTypeRepository>();
    private readonly IGarageEventLogRepository _garageEventLogRepository = Substitute.For<IGarageEventLogRepository>();

    private readonly GarageDistanceService _garageDistanceService;

    public GarageDistanceServiceTests()
    {
        _garageDistanceService = new GarageDistanceService(
            _garageDistanceRepository,
            _unclassifiedMessageService,
            _garageStatusRepository,
            _garageEventTypeRepository,
            _garageEventLogRepository);
    }

    [Fact]
    public async Task ParseAndSaveDistanceMessageAsync_ValidData_Success()
    {
        // Arrange
        const string messagePhrase = "123.4";

        var garageStatus = new GarageStatus
        {
            GarageStatusId = 1,
            GarageStatusName = "Test Status",
            MaximumDistance = 130.00M,
            MinimumDistance = 120.00M,
        };

        _garageStatusRepository.GetStatusForDistance(Arg.Any<decimal>()).Returns(garageStatus);

        // Act
        await _garageDistanceService.ParseAndSaveDistanceMessageAsync(messagePhrase);

        // Assert
        await _garageDistanceRepository.Received().AddAsync(Arg.Is<GarageDistance>(x => x.GarageStatus.GarageStatusId == 1));
    }

    [Fact]
    public async Task ParseAndSaveDistanceMessageAsync_NotDistancce_Unclassified()
    {
        // Arrange
        const string messagePhrase = "ThisIsNotDistance";

        // Act
        await _garageDistanceService.ParseAndSaveDistanceMessageAsync(messagePhrase);

        // Assert
        await _garageDistanceRepository.DidNotReceive().AddAsync(Arg.Any<GarageDistance>());

        await _unclassifiedMessageService.Received()
            .CreateAndSaveUnclassifiedMessageAsync(Arg.Is<string>(x => x.Contains(messagePhrase)));
    }

    [Fact]
    public async Task ParseAndSaveDistanceMessageAsync_ValidData_Success2()
    {
        // Arrange
        const string messagePhrase = "123.4";

        var garageStatus = new GarageStatus
        {
            GarageStatusId = 1,
            GarageStatusName = "Test Status",
            MaximumDistance = 130.00M,
            MinimumDistance = 120.00M,
        };

        var lastDistanceWithStatus = new GarageDistance
        {
            GarageStatusId = 2
        };

        var garageEventType = new GarageEventType { GarageEventTypeId = 2, GarageEventTypeName = "Event Occured"};

        _garageStatusRepository.GetStatusForDistance(Arg.Any<decimal>()).Returns(garageStatus);

        _garageDistanceRepository.GetLastWithStatusAsync().Returns(lastDistanceWithStatus);

        _garageEventTypeRepository.GetEventTypeByStatusIds(2, 1).Returns(garageEventType);

        // Act
        await _garageDistanceService.ParseAndSaveDistanceMessageAsync(messagePhrase);

        // Assert
        await _garageDistanceRepository.Received().AddAsync(Arg.Is<GarageDistance>(x => x.GarageStatus.GarageStatusId == 1));
        await _garageEventLogRepository.Received().AddAsync(Arg.Is<GarageEventLog>(x => x.GarageEventTypeId == 2));
    }
}