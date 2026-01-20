using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace QMS.Core.Repositories.ImpactTestRepository
{
    public interface IImpactTestRepository
    {
        Task<OperationResult> InsertImpactTestReportAsync(ImpactTestViewModel model);
        Task<OperationResult> UpdateImpactTestReportAsync(ImpactTestViewModel model);
        Task<List<ImpactTestViewModel>> GetImpactTestReportAsync();
        Task<ImpactTestViewModel> GetImpactTestReportByIdAsync(int Id);

        Task<OperationResult> DeleteImpactTestAsync(int Id);
    }
}
