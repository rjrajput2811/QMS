using QMS.Core.Models;

namespace QMS.Core.Repositories.ElectricalPerformanceRepo;

public interface IElectricalPerformanceRepository
{
    Task<List<ElectricalPerformanceViewModel>> GetElectricalPerformancesAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<ElectricalPerformanceViewModel> GetElectricalPerformancesByIdAsync(int Id);
    Task<OperationResult> InsertElectricalPerformancesAsync(ElectricalPerformanceViewModel model);
    Task<OperationResult> UpdateElectricalPerformancesAsync(ElectricalPerformanceViewModel model);
    Task<OperationResult> DeleteElectricalPerformancesAsync(int Id);
}
