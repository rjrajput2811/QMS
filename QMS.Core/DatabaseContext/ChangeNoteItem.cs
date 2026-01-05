using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext;

[Table("tbl_Change_Note_Items")]
public class ChangeNoteItem : SqlTable
{
    public int ChangeNoteId { get; set; }
    public string? ChangeFrom { get; set; }
    public string? ChangeTo { get; set; }
    public string? Category { get; set; }

    [ForeignKey("ChangeNoteId")]
    public virtual ChangeNote ChangeNote { get; set; }
}
