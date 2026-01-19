using QMS.Core.Models;

namespace QMS.Core.Repositories.DeviationNoteRepository;

public interface IDeviationNoteRepository
{
    Task<List<DeviationNoteViewModel>> GetDeviationNotesAsync();
    Task<DeviationNoteViewModel> GetDeviationNotesDetailsAsync(int Id);
    Task<int> InsertDeviationNoteAsync(DeviationNoteViewModel model);
    Task<OperationResult> UpdateDeviationNoteAsync(DeviationNoteViewModel model);
    Task<OperationResult> DeleteDeviationNoteAsync(int Id);
}
