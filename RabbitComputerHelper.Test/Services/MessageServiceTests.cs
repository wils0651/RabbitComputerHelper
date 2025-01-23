using NSubstitute;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;
using RabbitComputerHelper.Services;

namespace RabbitComputerHelper.Test.Services
{
    public class MessageServiceTests
    {
        IComputerRepository _computerRepository = Substitute.For<IComputerRepository>();
        IComputerTaskRepository _computerTaskRepository = Substitute.For<IComputerTaskRepository>();
        IMessageRepository _messageRepository = Substitute.For<IMessageRepository>();

        IMessageService _messageService;

        public MessageServiceTests()
        {
            _messageService = new MessageService(_computerRepository, _computerTaskRepository, _messageRepository);
        }

        [Fact]
        public async Task ParseAndSaveMessageAsync_ValidData_Success()
        {
            // Arrange
            const string messagePhrase = "computerName | 2025-01-22T10:31:01 | taskPhrase";

            var computer = new Computer();
            var computerTask = new ComputerTask();

            _computerRepository.GetByNameAsync("computerName").Returns(computer);

            _computerTaskRepository.GetByNameAsync("taskPhrase").Returns(computerTask);

            // Act
            await _messageService.ParseAndSaveMessageAsync(messagePhrase);

            // Assert
            await _messageRepository.Received().SaveChangesAsync();
        }
    }
}
