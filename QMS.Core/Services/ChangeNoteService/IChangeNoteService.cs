using QMS.Core.Models;

namespace QMS.Core.Services.ChangeNoteService;

public interface IChangeNoteService
{
    Task<List<ChangeNoteViewModel>> GetChangeNotesListAsync();
    Task<ChangeNoteViewModel> GetChangeNotesDetailsAsync(int Id);
    Task<OperationResult> InsertChangeNoteAsync(ChangeNoteViewModel model);
    Task<OperationResult> UpdateChangeNoteAsync(ChangeNoteViewModel model);
    Task<OperationResult> DeleteChangeNoteAsync(int Id);
}
