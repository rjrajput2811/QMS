using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext
{
    [Table("tbl_ElectricalPerformance")]
    public class ElectricalPerformance : SqlTable
    {
        [Key]
        public int Id { get; set; }

        public string? ProductCatRef { get; set; }
        public string? ProductDescription { get; set; }
        public string? ReportNo { get; set; }
        public DateTime? ReportDate { get; set; }
        public string? LightSourceDetails { get; set; }
        public string? DriverDetails { get; set; }
        public string? PCBDetails { get; set; }
        public string? LEDCombinations { get; set; }
        public string? BatchCode { get; set; }
        public string? SensorDetails { get; set; }
        public string? LampDetails { get; set; }
        public string? PKD { get; set; }
        public string? OverallResult { get; set; }
        public string? TestedByName { get; set; }
        public string? VerifiedByName { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<ElectricalPerDetails> Details { get; set; } = new List<ElectricalPerDetails>();
    }
}
