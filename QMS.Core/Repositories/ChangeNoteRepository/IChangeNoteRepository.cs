using QMS.Core.Models;

namespace QMS.Core.Repositories.ChangeNoteRepository;

public interface IChangeNoteRepository
{
    Task<List<ChangeNoteViewModel>> GetChangeNotesAsync();
    Task<ChangeNoteViewModel> GetChangeNotesDetailsAsync(int Id);
    Task<int> InsertChangeNoteAsync(ChangeNoteViewModel model);
    Task<OperationResult> UpdateChangeNoteAsync(ChangeNoteViewModel model);
    Task<OperationResult> DeleteChangeNoteAsync(int Id);
}
