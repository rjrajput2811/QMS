using QMS.Core.Models;

namespace QMS.Core.Services.DeviationNoteService;

public interface IDeviationNoteService
{
    Task<List<DeviationNoteViewModel>> GetDeviationNotesListAsync();
    Task<DeviationNoteViewModel> GetDeviationNotesDetailsAsync(int Id);
    Task<OperationResult> InsertDeviationNoteAsync(DeviationNoteViewModel model);
    Task<OperationResult> UpdateDeviationNoteAsync(DeviationNoteViewModel model);
    Task<OperationResult> DeleteDeviationNoteAsync(int Id);
}
