using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.ThirdPartyTestRepository
{
    public interface IThirdPartyTestRepository
    {
        Task<List<ThirdPartyTestViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<OperationResult> CreateAsync(ThirdPartyTesting thirdPartyTesting, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(ThirdPartyTesting thirdPartyTesting, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int Id);
        Task<ThirdPartyTestViewModel?> GetByIdAsync(int id);
        Task<bool> CheckDuplicate(string searchText, int id);
        Task<bool> UpdateAttachmentAsync(int id, string fileName);



        Task<List<PurposeTPTViewModel>> GetPurposeTPTAsync();
        Task<Purpose_TPT?> GetPurposeTPTByIdAsync(int Id);
        Task<OperationResult> CreatePurposeTPTAsync(PurposeTPTViewModel newPurposeTPTRecord, bool returnCreatedRecord = false);
        Task<OperationResult> UpdatePurposeTPTAsync(PurposeTPTViewModel updatePurposeTPTRecord, bool returnUpdatedRecord = false);
        Task<OperationResult> DeletePurposeTPTAsync(int Id);
        Task<bool> CheckPurposeTPTDuplicate(string searchText, int Id);
        Task<List<DropdownOptionViewModel>> GetPurposeDropdownAsync();



        Task<List<ProjectInitTPTViewModel>> GetProjectInitAsync();
        Task<ProjectInit_TPT?> GetProjectInitByIdAsync(int Id);
        Task<OperationResult> CreateProjectInitAsync(ProjectInitTPTViewModel newProjectInitRecord, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateProjectInitAsync(ProjectInitTPTViewModel updateProjectInitRecord, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteProjectInitAsync(int Id);
        Task<bool> CheckProjectInitDuplicate(string searchText, int Id);
        Task<List<DropdownOptionViewModel>> GetProjectInitDropdownAsync();



        Task<List<TestDetTPTViewModel>> GetTestDetTPTAsync();
        Task<TestDet_TPT?> GetTestDetTPTByIdAsync(int Id);
        Task<OperationResult> CreateTestDetTPTAsync(TestDetTPTViewModel newTestDetTPTRecord, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateTestDetTPTAsync(TestDetTPTViewModel updateTestDetTPTRecord, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteTestDetTPTAsync(int Id);
        Task<bool> CheckTestDetTPTDuplicate(string searchText, int Id);
        Task<List<DropdownOptionViewModel>> GetTestDetDropdownAsync();
    }
}
