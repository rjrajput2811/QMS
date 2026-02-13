using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.ValidationClosureRepository
{
    public interface IValidationClosureRepository
    {
        Task<List<ValidClosureReportViewModel>> GetValidationClosureAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<ValidClosureReportViewModel?> GetValidationClosureByIdAsync(int Id);
        Task<OperationResult> InsertValidationClosureAsync(ValidationClosureReport model);
        Task<OperationResult> UpdateValidationClosureAsync(ValidationClosureReport model);
        Task<OperationResult> DeleteValidationClosureAsync(int Id);
        Task<bool> CheckDuplicate(string searchText, int Id);
    }
}
