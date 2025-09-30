using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitComputerHelper.Models;

[Table("garagestatus")]
public class GarageStatus
{
    [Key]
    [Column("garagestatusid")]
    public int GarageStatusId { get; set; }

    [Column("garagestatusname")]
    public string GarageStatusName { get; set; } = string.Empty;

    [Column("minimumdistance")]
    public decimal MinimumDistance { get; set; }

    [Column("maximumdistance")]
    public decimal MaximumDistance { get; set; }
}