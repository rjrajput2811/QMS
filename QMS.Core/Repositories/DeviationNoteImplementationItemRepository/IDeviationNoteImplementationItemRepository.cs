using QMS.Core.Models;

namespace QMS.Core.Repositories.DeviationNoteImplementationItemRepository;

public interface IDeviationNoteImplementationItemRepository
{
    Task<List<DeviationNoteImplementationItemViewModel>> GetDeviationNoteImplementationItemsDetailsAsync(int Id);
    Task<OperationResult> InsertDeviationNoteImplementationItemAsync(DeviationNoteImplementationItemViewModel model);
    Task<OperationResult> UpdateDeviationNoteImplementationItemAsync(DeviationNoteImplementationItemViewModel model);
    Task<OperationResult> DeleteDeviationNoteImplementationItemAsync(int DeviationNoteId);
}
