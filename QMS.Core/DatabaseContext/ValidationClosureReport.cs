using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_ValidationClosureReport")]
public class ValidationClosureReport : SqlTable
{
    public string? ProductCatRef { get; set; }
    public string? ReportNo { get; set; }
    public DateTime? ReportDate { get; set; }
    public string? ProductDescription { get; set; }
    public string? ValidationDoneBy { get; set; }
    public string? BatchCode { get; set; }
    public string? PKD { get; set; }
    public int QuantityOffered { get; set; }
    public DateTime? ClosureDate { get; set; }

    public string? Row1_OpenPoints { get; set; }
    public string? Row1_ActionTaken { get; set; }
    public string? Row1_Stakeholder { get; set; }
    public string? Row1_Status { get; set; }

    public string? Row2_OpenPoints { get; set; }
    public string? Row2_ActionTaken { get; set; }
    public string? Row2_Stakeholder { get; set; }
    public string? Row2_Status { get; set; }

    public string? Row3_OpenPoints { get; set; }
    public string? Row3_ActionTaken { get; set; }
    public string? Row3_Stakeholder { get; set; }
    public string? Row3_Status { get; set; }

    public string? Row4_OpenPoints { get; set; }
    public string? Row4_ActionTaken { get; set; }
    public string? Row4_Stakeholder { get; set; }
    public string? Row4_Status { get; set; }

    public string? Row5_OpenPoints { get; set; }
    public string? Row5_ActionTaken { get; set; }
    public string? Row5_Stakeholder { get; set; }
    public string? Row5_Status { get; set; }

    public string? Row6_OpenPoints { get; set; }
    public string? Row6_ActionTaken { get; set; }
    public string? Row6_Stakeholder { get; set; }
    public string? Row6_Status { get; set; }

    public string? Row7_OpenPoints { get; set; }
    public string? Row7_ActionTaken { get; set; }
    public string? Row7_Stakeholder { get; set; }
    public string? Row7_Status { get; set; }

    public string? Row8_OpenPoints { get; set; }
    public string? Row8_ActionTaken { get; set; }
    public string? Row8_Stakeholder { get; set; }
    public string? Row8_Status { get; set; }

    public string? Row9_OpenPoints { get; set; }
    public string? Row9_ActionTaken { get; set; }
    public string? Row9_Stakeholder { get; set; }
    public string? Row9_Status { get; set; }

    public string? EvidencePicture { get; set; }
    public string? AttachedAnnexure { get; set; }
    public string? VendorQA_FinalComments { get; set; }
    public string? WiproQA_FinalComments { get; set; }
    public string? VendorQA_Signature { get; set; }
    public string? WiproQA_Signature { get; set; }
    public string? ReportAttached { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
