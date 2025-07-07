using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.COPQComplaintDumpRepository
{
    public interface IJobWorkTracRepository
    {
        Task<List<JobWork_TracViewModel>> GetJobListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<JobWork_TracViewModel?> GetJobByIdAsync(int id);
        Task<OperationResult> CreateJobAsync(JobWork_Tracking_Service record, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateJobAsync(JobWork_Tracking_Service record, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteJobAsync(int id);
        Task<BulkCreateJobResult> BulkCreateJobAsync(List<JobWork_TracViewModel> listOfData, string fileName, string uploadedBy, string recordType);
    }
}
