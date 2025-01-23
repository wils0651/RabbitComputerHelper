using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Services
{
    public class MessageService : IMessageService
    {
        private readonly IComputerRepository _computerRepository;
        private readonly IComputerTaskRepository _computerTaskRepository;
        private readonly IMessageRepository _messageRepository;

        public MessageService(
            IComputerRepository computerRepository,
            IComputerTaskRepository computerTaskRepository,
            IMessageRepository messageRepository)
        {
            this._computerRepository = computerRepository;
            this._computerTaskRepository = computerTaskRepository;
            this._messageRepository = messageRepository;
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

            // find the parts
            var computer = await _computerRepository.GetByNameAsync(computerName);

            if (computer == null)
            {
                // todo: do something
                return;
            }

            var note = string.Empty;
            if (taskPhrase.Contains("Reboot"))
            {
                note = taskPhrase.Replace("Reboot", "").Trim();
                taskPhrase = "Reboot";
            }

            var computerTask = await _computerTaskRepository.GetByNameAsync(taskPhrase);

            if (computerTask == null)
            {
                // todo: do something
                return;
            }

            var convertedDate = sentDate.ToUniversalTime();

            // create the message object
            var message = new Message(computer, computerTask, convertedDate, note);

            await _messageRepository.AddMessageAsync(message);
            await _messageRepository.SaveChangesAsync();
        }
    }
}
