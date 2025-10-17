using QMS.Core.DatabaseContext;
using QMS.Core.Models;

namespace QMS.Core.Repositories.PDITrackerRepository
{
    public interface IPDITrackerRepository
    {
        Task<List<PDITrackerViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<PDITracker?> GetByIdAsync(int id);
        Task<OperationResult> CreateAsync(PDITracker entity, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(PDITracker entity, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int id);
        Task<List<ProductCodeDetailViewModel>> GetCodeSearchAsync(string search = "");
        Task<List<DropdownOptionViewModel>> GetCodeSelect2OptionsAsync();
        Task<List<DropdownOptionViewModel>> GetVendorDropdownAsync();
        Task<bool> UpdateAttachmentAsync(int id, string fileName);

        Task<List<BatchCodePDIViewModel>> GetBatchCodePDIAsync();
        Task<BatchCode_PDI?> GetBatchCodePDIByIdAsync(int Id);
        Task<OperationResult> CreateBatchCodePDIAsync(BatchCodePDIViewModel newNatProjectRecord, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateBatchCodePDIAsync(BatchCodePDIViewModel updateNatProjectRecord, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteBatchCodePDIAsync(int Id);
        Task<bool> CheckBatchCodePDIDuplicate(string searchText, int Id);
        Task<List<DropdownOptionViewModel>> GetBatchCodeDropdownAsync();

    }
}
