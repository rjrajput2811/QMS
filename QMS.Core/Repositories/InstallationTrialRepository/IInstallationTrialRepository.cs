using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.InstallationTrialRepository
{
    public interface IInstallationTrialRepository
    {
        Task<List<InstallationTrialViewModel>> GetInstallationTrailAsync();
        Task<InstallationTrialViewModel> GetInstallationTrailByIdAsync(int Id);
        Task<OperationResult> InsertInstallationTrailAsync(InstallationTrialViewModel model);
        Task<OperationResult> UpdateInstallationTrailAsync(InstallationTrialViewModel model);
        Task<OperationResult> DeleteInstallationTrailAsync(int Id);
    }
}
