using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_SurgeTestReport")]
public class SurgeTestReport : SqlTable
{
    public int Id { get; set; }
    public string? ReportNo { get; set; }
    public DateTime? ReportDate { get; set; }
    public string? ProductCatRef { get; set; }
    public string? ProductDescription { get; set; }
    public string? DriverCode { get; set; }
    public string? SPDCode { get; set; }
    public string? LEDConfiguration { get; set; }
    public string? BatchCode { get; set; }
    public string? PKDCode { get; set; }
    public string? ReferenceStandard { get; set; }
    public string? AcceptanceNorm { get; set; }

    public string? Surge_Photo { get; set; }
    public string? CheckedBy { get; set; }
    public string? VerifiedBy { get; set; }

    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    //public string? User { get; set; }
    public bool Deleted { get; set; } = false;

    // Navigation property for child rows
    public virtual ICollection<SurgeTestReportDetails> Details { get; set; } = new List<SurgeTestReportDetails>();
}
