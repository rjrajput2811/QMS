using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_RippleTestReport")]
public class RippleTestReport : SqlTable
{
    public string? ReportNo { get; set; }
    public DateTime? TestingDate { get; set; }
    public string? MeasuringInstrument { get; set; }
    public string? ProductCatRef { get; set; }
    public string? ProductDescription { get; set; }
    public string? BatchCode { get; set; }
    public string? PKD { get; set; }
    public string? LEDDetails { get; set; }
    public string? LEDDriver { get; set; }
    public string? LEDCombination { get; set; }
    public decimal? DeltaValue { get; set; }
    public decimal? RMSValue { get; set; }
    public string? Calculation { get; set; }
    public decimal? RipplePercentage { get; set; }
    public string? RippleTestFileAttachedPath { get; set; }
    public string? Result { get; set; }
    public string? TestedBy { get; set; }
    public string? VerifiedBy { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
