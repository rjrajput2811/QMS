using QMS.Core.Models;

namespace QMS.Core.Repositories.DeviationNoteItemsRepository;

public interface IDeviationNoteItemsRepository
{
    Task<List<DeviationNoteItemsViewModel>> GetDeviationNoteItemssDetailsAsync(int Id);
    Task<OperationResult> InsertDeviationNoteItemsAsync(DeviationNoteItemsViewModel model);
    Task<OperationResult> UpdateDeviationNoteItemsAsync(DeviationNoteItemsViewModel model);
    Task<OperationResult> DeleteDeviationNoteItemsAsync(int DeviationNoteId);
}
