using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;

namespace QMS.Core.Repositories.KaizenTrackerRepository
{
    public interface IKaizenTrackerRepository
    {
        Task<List<KaizenTracViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<KaizenTracViewModel?> GetByIdAsync(int id);
        Task<OperationResult> CreateAsync(Kaizen_Tracker kaizen, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(Kaizen_Tracker kaizen, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int id);
        Task<bool> UpdateAttachmentAsync(int id, string fileName);
    }
}
