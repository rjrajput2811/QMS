using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_Deviation_Note_Items")]
public class DeviationNoteItem : SqlTable
{
    public int DeviationNoteId { get; set; }
    public string? StandardPractice  { get; set; }
    public string? Deviation { get; set; }
    public string? Category { get; set; }

    [ForeignKey("DeviationNoteId")]
    public virtual DeviationNote DeviationNote { get; set; }
}
