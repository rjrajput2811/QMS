using Microsoft.AspNetCore.Http;
using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_NeedleFlameTestReport")]
public class NeedleFlameTestReport : SqlTable
{
    public string? ReportNo { get; set; }
    public string? Ref_Stan { get; set; }
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
    public List<NeedleFlameTestReportDetail>? Details { get; set; }
}

[Table("tbl_NeedleFlameTest_Detail")]
public class NeedleFlameTestReportDetail : SqlTable
{
    public string? Photo_During_Test { get; set; }
    public string? After_During_Test { get; set; }
    public string? Test_Ref { get; set; }
    public string? Specified_Req { get; set; }
    public string? Observation { get; set; }
    public string? Result { get; set; }
    public int NeedleFlame_Id { get; set; }
    public  bool Delete { get; set; }
    [NotMapped] public IFormFile? Photo_During_TestFile { get; set; }
    [NotMapped] public IFormFile? After_During_TestFile { get; set; }
}

