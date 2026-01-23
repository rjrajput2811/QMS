using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.PhotometryRepository
{
    public interface IPhotometryTestRepository
    {
        Task<List<PhotometryTestReportViewModel>> GetPhotometryTestReportAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<OperationResult> InsertPhotometryTestReportAsync(PhotometryTestReportViewModel model);
        Task<OperationResult> UpdatePhotometryTestReportAsync(PhotometryTestReportViewModel model);
        Task<PhotometryTestReportViewModel> GetPhotometryTestReportByIdAsync(int Id);
        Task<OperationResult> DeletePhotometryTestAsync(int Id);
        Task<bool> CheckDuplicate(string searchText, int Id);
    }
}
