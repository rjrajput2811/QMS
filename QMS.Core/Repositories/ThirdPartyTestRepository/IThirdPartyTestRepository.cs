using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.ThirdPartyTestRepository
{
    public interface IThirdPartyTestRepository
    {
        Task<List<ThirdPartyTestViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<OperationResult> CreateAsync(ThirdPartyTesting thirdPartyTesting, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(ThirdPartyTesting thirdPartyTesting, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int Id);
        Task<ThirdPartyTestViewModel?> GetByIdAsync(int id);
        Task<bool> CheckDuplicate(string searchText, int id);
    }
}
