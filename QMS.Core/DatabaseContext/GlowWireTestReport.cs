using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_GlowWireTestReport")]
public class GlowWireTestReport : SqlTable
{
    public string? ReportNo { get; set; }
    public string? CustomerProjectName { get; set; }
    public DateTime? ReportDate { get; set; }
    public string? ProductCatRef { get; set; }
    public string? ProductDescription { get; set; }
    public string? BatchCode { get; set; }
    public int Quantity { get; set; }
    public string? PartDescription { get; set; }
    public string? PKD { get; set; }
    public string? ResistanceToFlame_Observation { get; set; }
    public string? ResistanceToFlame_Result { get; set; }
    public string? ResistanceToFlame_PhotoBeforeTest { get; set; }
    public string? ResistanceToFlame_PhotoAfterTest { get; set; }
    public string? TestResult { get; set; }
    public string? TestedBy { get; set; }
    public string? VerifiedBy { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
