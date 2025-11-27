using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QMS.Core.Repositories.VendorRepository.VendorRepository;

namespace QMS.Core.Repositories.VendorRepository
{
    public interface IVendorRepository
    {
        Task<List<VendorViewModel>> GetListAsync();
        Task<OperationResult> CreateAsync(Vendor vendor, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(Vendor vendor, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int userId);
        Task<VendorViewModel?> GetByIdAsync(int userId);
        Task<bool> CheckDuplicate(string searchText, string username, int Id);
        Task<List<string>> CheckDuplicateUserName(string userName, int Id);
        Task<CertificationDetail> CertGetByIdAsync(int id);
        Task<OperationResult> CertCreateAsync(CertificationDetail cert);
        Task<OperationResult> CertUpdateAsync(CertificationDetail cert);
        Task<OperationResult> CertDeleteAsync(int id, string updatedBy);
        Task<List<CertificationDetailViewModel?>> CertGetAllAsync(int? venId);
        Task<BulkCertiCreateResult> BulkCertiCreateAsync(List<CertificationDetailViewModel> listOfData, string fileName, string uploadedBy, string recordType);

        //Task<List<ProductCodeDetailDto>> GetProductCodesAsync(string term);
        Task<List<ThirdPartyTestReportViewModel>> ReportGetAllAsync();
        Task<OperationResult> ReportCreateAsync(ThirdPartyTestReport model);
        Task<ThirdPartyTestReport> ReportGetByIdAsync(int id);
        Task<OperationResult> ReportUpdateAsync(ThirdPartyTestReport cert);
        Task<OperationResult> ReportDeleteAsync(int id, string updatedBy);
        Task<List<ProductCodeDetailViewModel>> GetCodeSearchAsync(string search = "");

        Task<List<DropdownOptionViewModel>> GetVendorDropdownAsync();
        Task<List<VenBISCertificateViewModel>> GetAllBISCertificatesAsync(int? venId);
        Task<VenBISCertificate?> GetBISCertificateByIdAsync(int id);
        Task<bool> CreateOrUpdateBISCertificateAsync(VenBISCertificate cert);
        Task<bool> DeleteBISCertificateAsync(int id, string updatedBy);
    }
}
