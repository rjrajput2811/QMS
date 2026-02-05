using QMS.Core.Models;

namespace QMS.Core.Repositories.HydraulicTestObservationRepository;

public interface IHydraulicTestObservationRepository
{
    Task<List<HydraulicTestObservationReportViewModel>> GetHydraulicTestObservationDetailsAsync(int HydraulicTestReportId);
    Task<OperationResult> InsertHydraulicTestObservationAsync(HydraulicTestObservationReportViewModel model);
    Task<OperationResult> DeleteHydraulicTestObservationAsync(int hydraulicTestReportId);
}
