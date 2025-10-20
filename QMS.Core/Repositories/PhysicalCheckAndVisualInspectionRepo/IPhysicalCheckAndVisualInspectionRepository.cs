using QMS.Core.Models;

namespace QMS.Core.Repositories.ProductValidationRepo;

public interface IPhysicalCheckAndVisualInspectionRepository
{
    Task<List<PhysicalCheckAndVisualInspectionViewModel>> GetPhysicalCheckAndVisualInspectionsAsync();
    Task<PhysicalCheckAndVisualInspectionViewModel> GetPhysicalCheckAndVisualInspectionsByIdAsync(int Id);
    Task<OperationResult> InsertPhysicalCheckAndVisualInspectionsAsync(PhysicalCheckAndVisualInspectionViewModel model);
    Task<OperationResult> UpdatePhysicalCheckAndVisualInspectionsAsync(PhysicalCheckAndVisualInspectionViewModel model);
    Task<OperationResult> DeletePhysicalCheckAndVisualInspectionsAsync(int Id);
}
