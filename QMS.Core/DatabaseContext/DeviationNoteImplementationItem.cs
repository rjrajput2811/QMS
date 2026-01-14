using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_Deviation_Note_Implementation_Item")]
public class DeviationNoteImplementationItem : SqlTable
{
    public int DeviationNoteId { get; set; }
    public string? ActionPlanned { get; set; }
    public string? WhoWillDo { get; set; }
    public DateTime? ProposedCutOffDate { get; set; }
    public DateTime? ActualDate { get; set; }

    [ForeignKey("DeviationNoteId")]
    public virtual DeviationNote DeviationNote { get; set; }
}
