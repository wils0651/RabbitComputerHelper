using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitComputerHelper.Models
{
    [Table("computertask")]
    public class ComputerTask
    {
        [Key]
        [Column("computertaskid")]
        public int ComputerTaskId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}
