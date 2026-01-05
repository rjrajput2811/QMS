using Microsoft.Extensions.FileProviders;

namespace QMS.Core.Models;

public class ChangeNoteViewModel
{
    public int Id { get; set; }
    public bool Deleted { get; set; }
    public string? DocumentNo { get; set; }
    public string? RevisionNo { get; set; }
    public string? Description { get; set; }
    public int? VendorId { get; set; }
    public string? VendorName { get; set; }
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
    public string? User { get; set; }
    public virtual ICollection<ChangeNoteItemsViewModel> Items { get; set; } = new List<ChangeNoteItemsViewModel>();
    public virtual ICollection<ChangeNoteImplementationItemViewModel> ImplementationItems { get; set; } = new List<ChangeNoteImplementationItemViewModel>();
}