using Microsoft.AspNetCore.Http;
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
    public string? TestResult { get; set; }
    public string? TestedBy { get; set; }
    public string? VerifiedBy { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public List<GlowWireTestReportDetail>? Details { get; set; }
}

[Table("tbl_GlowWireTest_Details")]
public class GlowWireTestReportDetail : SqlTable
{
    public string? Photo_During_Test { get; set; }
    public string? Photo_After_Test { get; set; }
    public string? Test_Ref { get; set; }
    public string? Specified_Req { get; set; }
    public string? Observation   { get; set; }
    public string? Result { get; set; }
    public int GlowTest_Id { get; set; }

    [Column("IsDeleted")]
    public override bool Deleted { get; set; }
    [NotMapped] public IFormFile? Photo_During_TestFile { get; set; }
    [NotMapped] public IFormFile? Photo_After_TestFile { get; set; }
}
