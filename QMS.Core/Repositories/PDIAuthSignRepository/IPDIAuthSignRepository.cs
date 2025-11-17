using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.PDIAuthSignRepository
{
    public interface IPDIAuthSignRepository
    {
        Task<List<PDI_Auth_SignViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<PDI_Auth_SignViewModel?> GetByIdAsync(int id);
        Task<OperationResult> CreateAsync(PDI_Auth_Signatory entity, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(PDI_Auth_Signatory entity, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int id);
        Task<bool> UpdateAttachmentAsync(int id, string fileName,string type);
        Task<List<DropdownOptionViewModel>> GetVendorDropdownAsync();
    }
}
