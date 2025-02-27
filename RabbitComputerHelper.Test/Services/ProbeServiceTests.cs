using NSubstitute;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;
using RabbitComputerHelper.Services;

namespace RabbitComputerHelper.Test.Services
{
    public class ProbeServiceTests
    {
        private readonly IProbeRepository _probeRepository = Substitute.For<IProbeRepository>();
        private readonly IProbeDataRepository _probeDataRepository = Substitute.For<IProbeDataRepository>();
        private readonly IUnclassifiedMessageService _unclassifiedMessageService = Substitute.For<IUnclassifiedMessageService>();

        private readonly IProbeService _probeService;

        public ProbeServiceTests()
        {
            _probeService = new ProbeService(_probeRepository, _probeDataRepository, _unclassifiedMessageService);
        }

        [Fact]
        public async Task ParseAndSaveProbeDataAsync_ValidData_Success()
        {
            // Arrange
            const string messagePhrase = "probeName : 25.5";
            var probe = new Probe { ProbeId = 1, ProbeName = "probeName" };

            _probeRepository.GetByNameAsync("probeName").Returns(probe);

            // Act
            await _probeService.ParseAndSaveProbeDataAsync(messagePhrase);

            // Assert
            await _probeDataRepository.Received().AddAsync(Arg.Any<ProbeData>());
            await _probeDataRepository.Received().SaveChangesAsync();
        }

        [Fact]
        public async Task ParseAndSaveProbeDataAsync_InvalidData_SavesMessage()
        {
            // Arrange
            const string messagePhrase = "this is an invalid message";

            // Act
            await _probeService.ParseAndSaveProbeDataAsync(messagePhrase);

            // Assert
            await _unclassifiedMessageService.Received().CreateAndSaveUnclassifiedMessageAsync(Arg.Any<string>());
        }
    }
}
