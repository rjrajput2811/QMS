using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.FIFOTrackerRepository
{
    public interface IFIFOTrackerRepository
    {
        Task<List<FIFOTrackerViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<OperationResult> CreateAsync(FIFOTracker fIFOTracker, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(FIFOTracker fIFOTracker, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int Id);
        Task<FIFOTrackerViewModel?> GetByIdAsync(int id);
        Task<bool> CheckDuplicate(string searchText, int id);
        Task<bool> UpdateAttachmentAsync(int id, string fileName);

        Task<List<TestReqFIFOViewModel>> GetTestReqFIFOAsync();
        Task<BatchCode_PDI?> GetTestReqFIFOByIdAsync(int Id);
        Task<OperationResult> CreateTestReqFIFOAsync(TestReqFIFOViewModel newNatProjectRecord, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateTestReqFIFOAsync(TestReqFIFOViewModel updateNatProjectRecord, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteTestReqFIFOAsync(int Id);
        Task<bool> CheckTestReqFIFODuplicate(string searchText, int Id);
        Task<List<DropdownOptionViewModel>> GetTestReqDropdownAsync();
    }
}
