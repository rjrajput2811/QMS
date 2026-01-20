namespace QMS.Core.Models;

public class DeviationNoteViewModel
{
    public int Id { get; set; }
    public bool Deleted { get; set; }
    public string? DocumentNo { get; set; }
    public string? CatRefProduct { get; set; }
    public string? DeviationDetails { get; set; }
    public int? VendorId { get; set; }
    public string? VendorName { get; set; }
    public string? Remarks { get; set; }
    public string? DeviationNoteCategory { get; set; }
    public string? CopyTo { get; set; }
    public DateTime? SignatureDate { get; set; }
    public string? DeviationNoteGroup { get; set; }
    public string? DeviationNoteRefNo { get; set; }
    public DateTime? DateOfIssue { get; set; }
    public int? VendorQAInChargeId { get; set; }
    public string? VendorQAInChargeName { get; set; }
    public string? FinalQARemarks { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public string? User { get; set; }
    public virtual ICollection<DeviationNoteItemsViewModel> Items { get; set; } = new List<DeviationNoteItemsViewModel>();
    public virtual ICollection<DeviationNoteImplementationItemViewModel> ImplementationItems { get; set; } = new List<DeviationNoteImplementationItemViewModel>();
}
