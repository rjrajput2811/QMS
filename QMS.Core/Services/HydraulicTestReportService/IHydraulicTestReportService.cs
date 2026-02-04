using QMS.Core.Models;

namespace QMS.Core.Services.HydraulicTestReportService;

public interface IHydraulicTestReportService
{
    Task<List<HydraulicTestReportViewModel>> GetHydraulicTestReportListAsync();
    Task<HydraulicTestReportViewModel> GetHydraulicTestReportDetailsAsync(int Id);
    Task<OperationResult> InsertHydraulicTestReportAsync(HydraulicTestReportViewModel model);
    Task<OperationResult> UpdateHydraulicTestReportAsync(HydraulicTestReportViewModel model);
    Task<OperationResult> DeleteHydraulicTestReportAsync(int Id);
}
