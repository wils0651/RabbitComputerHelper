using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitComputerHelper.Models
{
    [Table("computer")]
    public class Computer
    {
        [Key]
        [Column("computerid")]
        public int ComputerId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}
