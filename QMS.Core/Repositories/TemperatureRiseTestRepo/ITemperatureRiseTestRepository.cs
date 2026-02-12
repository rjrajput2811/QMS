using QMS.Core.Models;

namespace QMS.Core.Repositories.TemperatureRiseTestRepo;

public interface ITemperatureRiseTestRepository
{
    Task<List<TemperatureRiseTestViewModel>> GetTemperatureRiseTestAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<TemperatureRiseTestViewModel> GetTemperatureRiseTestByIdAsync(int Id);
    Task<OperationResult> InsertTemperatureRiseTestAsync(TemperatureRiseTestViewModel model);
    Task<OperationResult> UpdateTemperatureRiseTestAsync(TemperatureRiseTestViewModel model);
    Task<OperationResult> DeleteTemperatureRiseTestAsync(int Id);
}
