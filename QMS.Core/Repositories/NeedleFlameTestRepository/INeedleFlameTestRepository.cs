using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.NeedleFlameTestRepository
{
    public interface INeedleFlameTestRepository
    {
        Task<List<NeedleFlameTestViewModel>> GetNeedleFlameTestAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<NeedleFlameTestViewModel?> GetNeedleFlameTestByIdAsync(int Id);
        Task<OperationResult> InsertNeedleFlameTestAsync(NeedleFlameTestReport model);
        Task<OperationResult> UpdateNeedleFlameTestAsync(NeedleFlameTestReport model);
        Task<OperationResult> DeleteNeedleFlameTestAsync(int Id);
        Task<bool> CheckDuplicate(string searchText, int Id);
    }
}
