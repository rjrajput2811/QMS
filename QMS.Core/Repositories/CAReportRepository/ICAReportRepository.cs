using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.CAReportRepository
{
    public interface ICAReportRepository
    {
        Task<List<CAReportViewModel>> GetCAReportAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<CAReportViewModel> GetCAReportByIdAsync(int cAId);
        Task<OperationResult> InsertCAReportAsync(CAReportViewModel model);
        Task<OperationResult> UpdateCAReportAsync(CAReportViewModel model);
        Task<OperationResult> DeleteAsync(int id);
        Task<bool> CheckDuplicate(string searchText, int Id);
        Task<List<CAReportViewModel>> GetCSOTrackingAsync(DateTime? startDate = null, DateTime? endDate = null);

    }
}
