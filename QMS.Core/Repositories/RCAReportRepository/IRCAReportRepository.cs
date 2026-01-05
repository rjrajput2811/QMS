using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.RCAReportRepository
{
    public interface IRCAReportRepository
    {
        Task<List<RCAReportViewModel>> GetRCAReportAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<RCAReportViewModel> GetRCAReportByIdAsync(int rcaId);
        Task<OperationResult> InsertRCAReportAsync(RCAReportViewModel model);
        Task<OperationResult> UpdateRCAReportAsync(RCAReportViewModel model);
        Task<OperationResult> DeleteAsync(int id);
        Task<bool> CheckDuplicate(string searchText, int Id);
        Task<List<RCAReportViewModel>> GetRCATrackingAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}
