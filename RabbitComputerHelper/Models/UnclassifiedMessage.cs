using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitComputerHelper.Models
{
    [Table("unclassifiedmessage")]
    public class UnclassifiedMessage(string messageContent)
    {
        [Key]
        [Column("unclassifiedmessageid")]
        public long UnclassifiedMessageId { get; set; }

        [Column("messagecontent")]
        public string? MessageContent { get; set; } = messageContent;

        [Column("createddate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
