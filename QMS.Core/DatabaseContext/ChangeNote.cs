using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_Change_Note")]
public class ChangeNote : SqlTable
{
    public string? DocumentNo { get; set; }
    public string? RevisionNo { get; set; }
    public string? Description { get; set; }
    public int? VendorId { get; set; }
    public string? Remarks { get; set; }
    public string? ChangeNoteCategory { get; set; }
    public string? CopyTo { get; set; }
    public string? SignatureFilePath { get; set; }
    public DateTime? SignatureDate { get; set; }
    public string? ChangeNoteGroup { get; set; }
    public string? ChangeNoteRefNo { get; set; }
    public DateTime? DateOfIssue { get; set; }
    public int? VendorQAInChargeId { get; set; }
    public string? FinalQARemarks { get; set; }
    public string? FinalSignatureFilePath { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public virtual ICollection<ChangeNoteItem> Items { get; set; } = new List<ChangeNoteItem>();
    public virtual ICollection<ChangeNoteImplementationItem> ImplementationItems { get; set; } = new List<ChangeNoteImplementationItem>();
}
