using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitComputerHelper.Models
{
    [Table("message")]
    public class Message
    {
        [Key]
        [Column("messageid")]
        public long MessageId { get; set; }

        [Column("computerid")]
        public int ComputerId { get; set; }

        [ForeignKey(nameof(ComputerId))]
        public Computer Computer { get; set; }

        [Column("computertaskid")]
        public int ComputerTaskId { get; set; }

        [ForeignKey(nameof(ComputerTaskId))]
        public ComputerTask ComputerTask { get; set; }

        [Column("createddate")]
        public DateTime CreatedDate { get; set; }

        [Column("note")]
        [MaxLength(255)]
        public string? Note { get; set; }

        public Message(Computer computer, ComputerTask computerTask, DateTime sentDate, string note)
        {
            Computer = computer;
            ComputerTask = computerTask;
            Note = note;
            CreatedDate = sentDate;
        }

        private Message()
        {
            Computer = null!;
            ComputerTask = null!;
        }
    }
}
