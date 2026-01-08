using QMS.Core.Models;

namespace QMS.Core.Repositories.ChangeNoteItemsRepository;

public interface IChangeNoteItemsRepository
{
    Task<List<ChangeNoteItemsViewModel>> GetChangeNoteItemssDetailsAsync(int Id);
    Task<OperationResult> InsertChangeNoteItemsAsync(ChangeNoteItemsViewModel model);
    Task<OperationResult> UpdateChangeNoteItemsAsync(ChangeNoteItemsViewModel model);
    Task<OperationResult> DeleteChangeNoteItemsAsync(int changeNoteId);
}
