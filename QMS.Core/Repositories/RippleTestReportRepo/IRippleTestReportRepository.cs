using QMS.Core.Models;

namespace QMS.Core.Repositories.RippleTestReportRepo;

public interface IRippleTestReportRepository
{
    Task<List<RippleTestReportViewModel>> GetRippleTestReportsAsync();
    Task<RippleTestReportViewModel> GetRippleTestReportsByIdAsync(int Id);
    Task<OperationResult> InsertRippleTestReportsAsync(RippleTestReportViewModel model);
    Task<OperationResult> UpdateRippleTestReportsAsync(RippleTestReportViewModel model);
    Task<OperationResult> DeleteRippleTestReportsAsync(int Id);
}
