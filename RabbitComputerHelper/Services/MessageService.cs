using Microsoft.EntityFrameworkCore;
using RabbitComputerHelper.Contracts;
using RabbitComputerHelper.Models;

namespace RabbitComputerHelper.Services
{
    public class MessageService : IMessageService
    {
        private readonly DatabaseContext _context;

        public MessageService(DatabaseContext context)
        {
            this._context = context;
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
            var computer = await _context.Computer
                .FirstOrDefaultAsync(c => c.Name == computerName);

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

            var computerTask = await _context.ComputerTask
                .FirstOrDefaultAsync(t => t.Name == taskPhrase);

            if (computerTask == null)
            {
                // todo: do something
                return;
            }

            // create the message object
            var message = new Message(computer, computerTask, sentDate.ToUniversalTime(), note);

            _context.Message.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
