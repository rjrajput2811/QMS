using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_Change_Note_Implementation_Items")]
public class ChangeNoteImplementationItem : SqlTable
{
    public int ChangeNoteId { get; set; }
    public string? ActionPlanned { get; set; }
    public string? WhoWillDo { get; set; }
    public DateTime? ProposedCutOffDate { get; set; }
    public DateTime? ActualDate { get; set; }

    [ForeignKey("ChangeNoteId")]
    public virtual ChangeNote ChangeNote { get; set; }
}
