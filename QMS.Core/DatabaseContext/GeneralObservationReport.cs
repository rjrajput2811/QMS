using Microsoft.AspNetCore.Http;
using QMS.Core.DatabaseContext.Shared;
using QMS.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_GeneralObservationReport")]
public class GeneralObservationReport : SqlTable
{
    public string? ProductCatRef { get; set; }
    public string? ProductDescription { get; set; }
    public string? ReportNo { get; set; }
    public DateTime? ReportDate { get; set; }
    public string? CheckedBy { get; set; }
    public string? VerifiedBy { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public List<GeneralObservationReportDetail>? Details { get; set; } = new();
}

[Table("tbl_GeneralObservation_Detail")]
public class GeneralObservationReportDetail : SqlTable
{
    public string? Req_Spec { get; set; }
    public string? Actual_find { get; set; }
    public string? Open_Close { get; set; }
    public string? Closure_Respons { get; set; }
    public string? Attachment { get; set; }
    public int GenObs_Id { get; set; }
    [NotMapped] public IFormFile? AttachmentFile { get; set; }
}
