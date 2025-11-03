using QMS.Core.Models;

namespace QMS.Core.Repositories.ElectricalPerformanceRepo;

public interface IElectricalPerformanceRepository
{
    Task<List<ElectricalPerformanceViewModel>> GetElectricalPerformancesAsync();
    Task<ElectricalPerformanceViewModel> GetElectricalPerformancesByIdAsync(int Id);
    Task<OperationResult> InsertElectricalPerformancesAsync(ElectricalPerformanceViewModel model);
    Task<OperationResult> UpdateElectricalPerformancesAsync(ElectricalPerformanceViewModel model);
    Task<OperationResult> DeleteElectricalPerformancesAsync(int Id);
}
