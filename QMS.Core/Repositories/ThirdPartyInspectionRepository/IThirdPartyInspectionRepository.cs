using QMS.Core.Models;
using QMS.Core.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.ThirdPartyInspectionRepository
{
    public interface IThirdPartyInspectionRepository
    {
        Task<List<ThirdPartyInspectionViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<OperationResult> CreateAsync(ThirdPartyInspection inspection, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(ThirdPartyInspection inspection, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int Id);
        Task<ThirdPartyInspection> GetByIdAsync(int inspectionId);
        Task<bool> CheckDuplicate(string projectName, int inspectionId);
        Task<bool> UpdateAttachmentAsync(int id, string fileName);
        Task<BulkTPICreateResult> BulkTPICreateAsync(List<ThirdPartyInspectionViewModel> listOfData, string fileName, string uploadedBy, string recordType);

    }
}
