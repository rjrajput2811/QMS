namespace QMS.Core.Models;

public class ChangeNoteImplementationItemViewModel
{
    public int Id { get; set; }
    public bool Deleted { get; set; }
    public int ChangeNoteId { get; set; }
    public string? ActionPlanned { get; set; }
    public string? WhoWillDo { get; set; }
    public DateTime? ProposedCutOffDate { get; set; }
    public DateTime? ActualDate { get; set; }
}
