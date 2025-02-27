using NSubstitute;
using NSubstitute.ReturnsExtensions;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;
using RabbitComputerHelper.Services;

namespace RabbitComputerHelper.Test.Services
{
    public class MessageServiceTests
    {
        private readonly IComputerRepository _computerRepository = Substitute.For<IComputerRepository>();
        private readonly IComputerTaskRepository _computerTaskRepository = Substitute.For<IComputerTaskRepository>();
        private readonly IMessageRepository _messageRepository = Substitute.For<IMessageRepository>();
        private readonly IUnclassifiedMessageService _unclassifiedMessageService = Substitute.For<IUnclassifiedMessageService>();

        private readonly IMessageService _messageService;

        public MessageServiceTests()
        {
            _messageService = new MessageService(
                _computerRepository, _computerTaskRepository, _messageRepository, _unclassifiedMessageService);
        }

        [Fact]
        public async Task ParseAndSaveMessageAsync_ValidData_Success()
        {
            // Arrange
            const string messagePhrase = "computerName | 2025-01-22T10:31:01 | taskPhrase";

            var computer = new Computer { Name = "computerName", Description = "a test computer" };
            var computerTask = new ComputerTask { Name = "taskPhrase" };

            _computerRepository.GetByNameAsync("computerName").Returns(computer);
            _computerTaskRepository.GetByNameAsync("taskPhrase").Returns(computerTask);

            // Act
            await _messageService.ParseAndSaveMessageAsync(messagePhrase);

            // Assert
            await _messageRepository.Received().AddAsync(Arg.Any<Message>());
            await _messageRepository.Received().SaveChangesAsync();
        }

        [Fact]
        public async Task ParseAndSaveMessageAsync_InvalidMessage_SavesMessage()
        {
            // Arrange
            const string messagePhrase = "this is an invalid message";

            // Act
            await _messageService.ParseAndSaveMessageAsync(messagePhrase);

            // Assert
            await _messageRepository.DidNotReceive().AddAsync(Arg.Any<Message>());

            await _unclassifiedMessageService.Received().CreateAndSaveUnclassifiedMessageAsync(Arg.Any<string>());
        }

        [Fact]
        public async Task ParseAndSaveMessageAsync_NoComputer_CreatesComputer()
        {
            // Arrange
            const string messagePhrase = "computerName | 2025-01-22T10:31:01 | taskPhrase";

            var computerTask = new ComputerTask { Name = "taskPhrase" };

            _computerRepository.GetByNameAsync("computerName").ReturnsNull();
            _computerTaskRepository.GetByNameAsync("taskPhrase").Returns(computerTask);

            // Act
            await _messageService.ParseAndSaveMessageAsync(messagePhrase);

            // Assert
            await _computerRepository.Received().AddAsync(Arg.Any<Computer>());
            await _computerRepository.Received().SaveChangesAsync();

            await _messageRepository.Received().AddAsync(Arg.Any<Message>());
            await _messageRepository.Received().SaveChangesAsync();
        }

        [Fact]
        public async Task ParseAndSaveMessageAsync_NoComputerTask_CreatesUnclassifiedMessage()
        {
            // Arrange
            const string messagePhrase = "computerName | 2025-01-22T10:31:01 | taskPhrase";

            var computer = new Computer { Name = "computerName", Description = "a test computer" };

            _computerRepository.GetByNameAsync("computerName").Returns(computer);
            _computerTaskRepository.GetByNameAsync("taskPhrase").ReturnsNull();

            // Act
            await _messageService.ParseAndSaveMessageAsync(messagePhrase);

            // Assert
            await _unclassifiedMessageService.Received().CreateAndSaveUnclassifiedMessageAsync(Arg.Any<string>());

            await _messageRepository.DidNotReceive().AddAsync(Arg.Any<Message>());
            await _messageRepository.DidNotReceive().SaveChangesAsync();
        }
    }
}
