namespace QMS.Core.Models;

public class DeviationNoteItemsViewModel
{
    public int Id { get; set; }
    public bool Deleted { get; set; }
    public int DeviationNoteId { get; set; }
    public string? StandardPractice { get; set; }
    public string? Deviation { get; set; }
    public string? Category { get; set; }
}
