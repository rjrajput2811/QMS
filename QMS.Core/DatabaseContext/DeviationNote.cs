using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_Deviation_Note")]
public class DeviationNote : SqlTable
{
    public string? DocumentNo { get; set; }
    public string? CatRefProduct { get; set; }
    public string? DeviationDetails { get; set; }
    public int? VendorId { get; set; }
    public string? Remarks { get; set; }
    public string? CopyTo { get; set; }
    public DateTime? SignatureDate { get; set; }
    public string? DeviationNoteGroup { get; set; }
    public string? DeviationNoteRefNo { get; set; }
    public DateTime? DateOfIssue { get; set; }
    public int? VendorQAInChargeId { get; set; }
    public string? FinalQARemarks { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public virtual ICollection<DeviationNoteItem> Items { get; set; } = new List<DeviationNoteItem>();
    public virtual ICollection<DeviationNoteImplementationItem> ImplementationItems { get; set; } = new List<DeviationNoteImplementationItem>();
}
