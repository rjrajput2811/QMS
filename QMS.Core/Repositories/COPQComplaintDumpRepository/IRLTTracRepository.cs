using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.COPQComplaintDumpRepository
{
    public interface IRLTTracRepository 
    {
        Task<List<RLT_TracViewModel>> GetRLTListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<RLT_TracViewModel?> GetRLTByIdAsync(int id);
        Task<OperationResult> CreateRLTAsync(RLT_Tracking_Service record, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateRLTAsync(RLT_Tracking_Service record, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteRLTAsync(int id);
        Task<BulkCreateRLTResult> BulkCreateRLTAsync(List<RLT_TracViewModel> listOfData, string fileName, string uploadedBy, string recordType);
        Task<List<FinalRLTOutput>> GetFinalRLTListAsync();
    }
}
