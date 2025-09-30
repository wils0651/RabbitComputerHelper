using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitComputerHelper.Models
{
    [Table("garageeventtype")]
    public class GarageEventType
    {
        [Key]
        [Column("garageeventtypeid")]
        public int GarageEventTypeId { get; set; }

        [Column("garageeventtypename")]
        public string GarageEventTypeName { get; set; } = string.Empty;

        [Column("previousgaragestatusid")]
        public int PreviousGarageStatusId { get; set; }

        [ForeignKey(nameof(PreviousGarageStatusId))]
        public GarageStatus PreviousGarageStatus { get; set; }

        [Column("currentgaragestatusid")]
        public int CurrentGarageStatusId { get; set; }

        [ForeignKey(nameof(CurrentGarageStatusId))]
        public GarageStatus CurrentGarageStatus { get; set; }
    }
}
