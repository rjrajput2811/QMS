using QMS.Core.Models;

namespace QMS.Core.Repositories.HydraulicTestRepository;

public interface IHydraulicTestRepository
{
    Task<List<HydraulicTestReportViewModel>> GetHydraulicTestReportAsync();
    Task<HydraulicTestReportViewModel> GetHydraulicTestReportDetailsAsync(int Id);
    Task<int> InsertHydraulicTestReportAsync(HydraulicTestReportViewModel model);
    Task<OperationResult> UpdateHydraulicTestReportAsync(HydraulicTestReportViewModel model);
    Task<OperationResult> DeleteHydraulicTestReportAsync(int Id);
}
