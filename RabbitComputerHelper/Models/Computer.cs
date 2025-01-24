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

        [MaxLength(128)]
        [Column("name")]
        public required string Name { get; set; }

        [MaxLength(256)]
        [Column("description")]
        public required string Description { get; set; }

        [MaxLength(128)]
        [Column("ipaddress")]
        public string? IpAddress { get; set; }
    }
}
