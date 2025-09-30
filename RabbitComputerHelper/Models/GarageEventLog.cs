using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitComputerHelper.Models
{
    [Table("garageeventlog")]
    public class GarageEventLog
    {
        [Key]
        [Column("garageeventlogid")]
        public long GarageEventLogId { get; set; }

        [Column("garageeventtypeid")]
        public int GarageEventTypeId { get; set; }

        [ForeignKey(nameof(GarageEventTypeId))]
        public GarageEventType GarageEventType { get; set; }

        [Column("distance")]
        public decimal Distance { get; set; }

        [Column("createddate")]
        public DateTime CreatedDate { get; set; }

        public GarageEventLog(GarageEventType garageEventType, decimal distance, DateTime createdDate)
        {
            GarageEventTypeId = garageEventType.GarageEventTypeId;
            this.GarageEventType = garageEventType;
            Distance = distance;
            CreatedDate = createdDate;
        }

        private GarageEventLog()
        {
        }
    }
}
