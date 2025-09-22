using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.FIFOTrackerRepository
{
    public interface IFIFOTrackerRepository
    {
        Task<List<FIFOTrackerViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<OperationResult> CreateAsync(FIFOTracker fIFOTracker, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(FIFOTracker fIFOTracker, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int Id);
        Task<FIFOTrackerViewModel?> GetByIdAsync(int id);
        Task<bool> CheckDuplicate(string searchText, int id);
        //Task<bool> UpdateAttachmentAsync(int id, string fileName);
    }
}
