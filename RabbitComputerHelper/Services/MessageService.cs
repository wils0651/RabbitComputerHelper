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

            if (messageParts.Length != 3)
            {
                await CreateAndSaveUnclassifiedMessageAsync(messagePhrase);
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

            if (computerTask == null || computer == null && string.IsNullOrEmpty(computerName))
            {
                await CreateAndSaveUnclassifiedMessageAsync(messagePhrase);
                return;
            }
            
            computer ??= await CreateAndSaveComputerAsync(computerName);
            
            CheckAndUpdateIpAddress(computer, note);

            var convertedDate = sentDate.ToUniversalTime();

            // create the message object
            var message = new Message(computer, computerTask, convertedDate, note);

            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveChangesAsync();
        }

        private async Task CreateAndSaveUnclassifiedMessageAsync(string messageContent)
        {
            var unclassifiedMessage = new UnclassifiedMessage(messageContent);

            await _unclassifiedMessageRepository.AddAsync(unclassifiedMessage);
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
