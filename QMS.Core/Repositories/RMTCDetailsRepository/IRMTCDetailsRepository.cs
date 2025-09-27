using QMS.Core.DatabaseContext;
using QMS.Core.Models;

namespace QMS.Core.Repositories.RMTCDetailsRepository
{
    public interface IRMTCDetailsRepository
    {
        Task<List<RM_TCViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<RM_TCViewModel?> GetByIdAsync(int id);
        Task<OperationResult> CreateAsync(RM_TC_Tracker entity, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(RM_TC_Tracker entity, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int id);
        Task<bool> UpdateAttachmentAsync(int id, string fileName);
    }
}
