using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.SurgeTestReportRepository
{
    public interface ISurgeTestReportRepository
    {
        Task<List<SurgeTestReportViewModel>> GetSurgeTestReportAsync();
        Task<SurgeTestReportViewModel> GetSurgeTestReportByIdAsync(int Id);
        Task<OperationResult> InsertUpdateSurgeTestReportAsync(SurgeTestReportViewModel model);
        Task<OperationResult> DeleteSurgeTestReportAsync(int Id);
    }
}
