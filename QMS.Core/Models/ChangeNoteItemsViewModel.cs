namespace QMS.Core.Models;

public class ChangeNoteItemsViewModel
{
    public int Id { get; set; }
    public bool Deleted { get; set; }
    public int ChangeNoteId { get; set; }
    public string? ChangeFrom { get; set; }
    public string? ChangeTo { get; set; }
    public string? Category { get; set; }
}
