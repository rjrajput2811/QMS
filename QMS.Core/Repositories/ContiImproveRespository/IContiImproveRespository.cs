using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.ContiImproveRespository
{
    public interface IContiImproveRespository
    {
        Task<List<ContiImproveViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<OperationResult> CreateAsync(Continual_Improve_Tracker fIFOTracker, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(Continual_Improve_Tracker fIFOTracker, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int Id);
        Task<ContiImproveViewModel?> GetByIdAsync(int id);
        //Task<bool> CheckDuplicate(string searchText, int id);
    }
}
