using QMS.Core.Models;

namespace QMS.Core.Repositories.ChangeNoteImplementationItemRepository;

public interface IChangeNoteImplementationItemRepository
{
    Task<List<ChangeNoteImplementationItemViewModel>> GetChangeNoteImplementationItemsDetailsAsync(int Id);
    Task<OperationResult> InsertChangeNoteImplementationItemAsync(ChangeNoteImplementationItemViewModel model);
    Task<OperationResult> UpdateChangeNoteImplementationItemAsync(ChangeNoteImplementationItemViewModel model);
    Task<OperationResult> DeleteChangeNoteImplementationItemAsync(int changeNoteId);
}
