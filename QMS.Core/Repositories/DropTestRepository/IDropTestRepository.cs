using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.DropTestRepository
{
    public interface IDropTestRepository 
    {
        Task<List<DropTestViewModel>> GetDropTestAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<DropTestViewModel?> GetDropTestByIdAsync(int Id);
        Task<OperationResult> InsertDropTestAsync(DropTestReport model);
        Task<OperationResult> UpdateDropTestAsync(DropTestReport model);
        Task<OperationResult> DeleteDropTestAsync(int Id);
        Task<bool> CheckDuplicate(string searchText, int Id);
    }
}
