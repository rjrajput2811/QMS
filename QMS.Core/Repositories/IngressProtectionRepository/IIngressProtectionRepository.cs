using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.IngressProtectionRepository
{
    public interface IIngressProtectionRepository
    {
        Task<List<IngressProtectionTestViewModel>> GetIngressProtectionAsync();
        Task<IngressProtectionTestViewModel?> GetIngressProtectionByIdAsync(int Id);
        Task<OperationResult> InsertIngressProtectionAsync(IngressProtectionTestReport model);
        Task<OperationResult> UpdateIngressProtectionAsync(IngressProtectionTestReport model);
        Task<OperationResult> DeleteIngressProtectionAsync(int Id);
        Task<bool> CheckDuplicate(string searchText, int Id);
    }
}
