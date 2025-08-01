using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.COPQComplaintDumpRepository
{
    public interface IContractorRepository
    {
        Task<List<ContractorDetailViewModel>> GetListAsync();
        Task<OperationResult> CreateAsync(ContractorDetail_Service contractor, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(ContractorDetail_Service contractor, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int userId);
        Task<ContractorDetailViewModel?> GetByIdAsync(int userId);
        Task<bool> CheckDuplicate(string searchText, int Id);

    }
}
