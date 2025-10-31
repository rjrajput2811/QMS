using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_IngressProtectionTestReport")]
public class IngressProtectionTestReport : SqlTable
{
    public string? ReportNo { get; set; }
    public string? CustomerProjectName { get; set; }
    public DateTime? ReportDate { get; set; }
    public string? ProductCatRef { get; set; }
    public string? ProductDescription { get; set; }
    public string? BatchCode { get; set; }
    public int Quantity { get; set; }
    public string? IPRating { get; set; }
    public string? PKD { get; set; }
    public string? IPFirstDigit_PhotoDuringTest { get; set; }
    public string? IPFirstDigit_PhotoAfterTest { get; set; }
    public string? IPFirstDigit_Observation { get; set; }
    public string? IPFirstDigit_Result { get; set; }
    public string? IPSecondDigit_PhotoDuringTest { get; set; }
    public string? IPSecondDigit_PhotoAfterTest { get; set; }
    public string? IPSecondDigit_Observation { get; set; }
    public string? IPSecondDigit_Result { get; set; }
    public string? GlowTest_Observation { get; set; }
    public string? GlowTest_Result { get; set; }
    public string? HV_IR_Observation { get; set; }
    public string? HV_IR_Result { get; set; }
    public string? TestResult { get; set; }
    public string? TestedBy { get; set; }
    public string? VerifiedBy { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
