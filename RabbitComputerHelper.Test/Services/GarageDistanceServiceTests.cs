using NSubstitute;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;
using RabbitComputerHelper.Services;

namespace RabbitComputerHelper.Test.Services;

public class GarageDistanceServiceTests
{
    private readonly IGarageDistanceRepository _garageDistanceRepository  = Substitute.For<IGarageDistanceRepository>() ;
    private readonly IUnclassifiedMessageService _unclassifiedMessageService = Substitute.For<IUnclassifiedMessageService>();
    private readonly IGarageStatusRepository _garageStatusRepository = Substitute.For<IGarageStatusRepository>();

    private readonly GarageDistanceService _garageDistanceService;
    
    public GarageDistanceServiceTests()
    {
        _garageDistanceService = new GarageDistanceService(_garageDistanceRepository, _unclassifiedMessageService, _garageStatusRepository);
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
}