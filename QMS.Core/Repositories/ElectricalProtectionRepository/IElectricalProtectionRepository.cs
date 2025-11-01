using QMS.Core.Models;

namespace QMS.Core.Repositories.ElectricalProtectionRepo
{
    public interface IElectricalProtectionRepository
    {
        Task<List<ElectricalProtectionViewModel>> GetElectricalProtectionsAsync();
        Task<ElectricalProtectionViewModel> GetElectricalProtectionByIdAsync(int Id);
        Task<OperationResult> InsertElectricalProtectionAsync(ElectricalProtectionViewModel model);
        Task<OperationResult> UpdateElectricalProtectionAsync(ElectricalProtectionViewModel model);
        Task<OperationResult> DeleteElectricalProtectionAsync(int Id);
    }
}
