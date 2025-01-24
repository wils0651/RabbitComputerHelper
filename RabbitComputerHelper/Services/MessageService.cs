using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Services
{
    public class MessageService : IMessageService
    {
        private readonly IComputerRepository _computerRepository;
        private readonly IComputerTaskRepository _computerTaskRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUnclassifiedMessageRepository _unclassifiedMessageRepository;

        public MessageService(
            IComputerRepository computerRepository,
            IComputerTaskRepository computerTaskRepository,
            IMessageRepository messageRepository,
            IUnclassifiedMessageRepository unclassifiedMessageRepository)
        {
            _computerRepository = computerRepository;
            _computerTaskRepository = computerTaskRepository;
            _messageRepository = messageRepository;
            _unclassifiedMessageRepository = unclassifiedMessageRepository;
        }

        public async Task ParseAndSaveMessageAsync(string messagePhrase)
        {
            // parse the message
            var messageParts = messagePhrase.Split('|');

            if (messageParts.Length == 0)
            {
                // todo: do something
                return;
            }

            var computerName = messageParts[0].Trim();
            var sentDatePhrase = messageParts[1].Trim();
            var taskPhrase = messageParts[2].Trim();

            DateTime sentDate;
            if (!DateTime.TryParse(sentDatePhrase, out sentDate))
            {
                sentDate = DateTime.Now;
            }
            sentDate = DateTime.SpecifyKind(sentDate, DateTimeKind.Local);

            var note = string.Empty;
            if (taskPhrase.Contains("Reboot"))
            {
                note = taskPhrase.Replace("Reboot", "").Trim();

                taskPhrase = "Reboot";
            }

            // find the parts
            var computer = await _computerRepository.GetByNameAsync(computerName);
            var computerTask = await _computerTaskRepository.GetByNameAsync(taskPhrase);

            if (computer == null && computerTask == null)
            {
                await CreateAndSaveUnclassifiedMessageAsync(messagePhrase);
                return;
            }
            else if (computer == null && !string.IsNullOrEmpty(computerName))
            {
                computer = await CreateAndSaveComputerAsync(computerName);
            }
            else if (computerTask == null && !string.IsNullOrEmpty(taskPhrase))
            {
                computerTask = await CreateAndSaveComputerTaskAsync(taskPhrase);
            }

            CheckAndUpdateIpAddress(computer, note);

            var convertedDate = sentDate.ToUniversalTime();

            // create the message object
            var message = new Message(computer, computerTask, convertedDate, note);

            await _messageRepository.AddMessageAsync(message);
            await _messageRepository.SaveChangesAsync();
        }

        private async Task CreateAndSaveUnclassifiedMessageAsync(string messageContent)
        {
            var unclassifiedMessage = new UnclassifiedMessage(messageContent);

            await _unclassifiedMessageRepository.AddMessageAsync(unclassifiedMessage);
            await _unclassifiedMessageRepository.SaveChangesAsync();
        }

        private async Task<Computer> CreateAndSaveComputerAsync(string computerName)
        {
            var computer = new Computer
            {
                Name = computerName,
                Description = $"Created from message processed on {DateTime.Now:f}"
            };

            await _computerRepository.AddAsync(computer);
            await _computerRepository.SaveChangesAsync();
            return computer;
        }

        private async Task<ComputerTask> CreateAndSaveComputerTaskAsync(string taskPhrase)
        {
            var computerTask = new ComputerTask
            {
                Name = taskPhrase,
                Description = $"Created from message processed on {DateTime.Now:f}"
            };

            await _computerTaskRepository.AddAsync(computerTask);
            await _computerTaskRepository.SaveChangesAsync();
            return computerTask;
        }

        private static void CheckAndUpdateIpAddress(Computer computer, string note)
        {
            if (string.IsNullOrEmpty(note))
            {
                return;
            }

            // todo: check if note is an ipAddress

            if (computer.IpAddress != note)
            {
                computer.IpAddress = note;
            }
        }
    }
}