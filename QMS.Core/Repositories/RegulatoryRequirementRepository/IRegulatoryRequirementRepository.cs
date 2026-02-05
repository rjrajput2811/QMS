using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.RegulatoryRequirementRepository
{
    public interface IRegulatoryRequirementRepository
    {
        Task<OperationResult> InsertRegulatoryRequirementAsync(RegulatoryRequirementViewModel model);
        Task<OperationResult> UpdateRegulatoryRequirementAsync(RegulatoryRequirementViewModel model);
        
        Task<List<RegulatoryRequirementViewModel>> GetRegulatoryRequirementAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<RegulatoryRequirementViewModel> GetRegulatoryRequirementByIdAsync(int Id);
        Task<OperationResult> DeleteRegulatoryRequirementAsync(int Id);
        Task<bool> CheckDuplicate(string searchText, int Id);
    }
}
