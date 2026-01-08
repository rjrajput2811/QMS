using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.SPMMakeRepository
{
    public interface ISPMMakeRepository
    {
        Task<List<SPM_MakeViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<OperationResult> CreateAsync(SPM_Make spmMake, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(SPM_Make spmMake, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int Id);
        Task<SPM_Make> GetByIdAsync(int spmMakeId);
        Task<bool> CheckDuplicate(string sup_Name, string qtr, int spmId);
    }
}
