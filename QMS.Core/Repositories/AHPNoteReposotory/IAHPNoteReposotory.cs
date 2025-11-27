using QMS.Core.Models;

namespace QMS.Core.Repositories.AHPNoteReposotory;

public interface IAHPNoteReposotory
{
    Task<List<AHPNoteViewModel>> GetAHPNotesAsync();
    Task<AHPNoteViewModel> GetAHPNotesByIdAsync(int Id);
    Task<OperationResult> InsertAHPNotesAsync(AHPNoteViewModel model);
    Task<OperationResult> UpdateAHPNotesAsync(AHPNoteViewModel model);
    Task<OperationResult> DeleteAHPNotesAsync(int Id);
}
