using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;

namespace QMS.Core.Repositories.COPQComplaintDumpRepository
{
    public interface IComplaintIndentDumpRepository
    {
        //// ----------------- Complaint Dump ------------------- ////
        Task<List<ComplaintViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<OperationResult> CreateAsync(ComplaintDump_Service complaint, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(ComplaintDump_Service complaint, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int id);
        Task<ComplaintViewModel?> GetByIdAsync(int id);
        Task<BulkCreateResult> BulkCreateAsync(List<ComplaintViewModel> listOfData, string fileName, string uploadedBy, string recordType);
        //// ----------------- Complaint Dump ------------------- ////
        


        //// ----------------- Po List ------------------- ////
        Task<List<PendingPoViewModel>> GetPOListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<PendingPoViewModel?> GetPOByIdAsync(int id);
        Task<OperationResult> CreatePOAsync(PendingPo_Service podetail, bool returnCreatedRecord = false);
        Task<OperationResult> UpdatePOAsync(PendingPo_Service podetail, bool returnUpdatedRecord = false);
        Task<OperationResult> DeletePOAsync(int id);
        Task<BulkCreatePOResult> BulkCreatePoAsync(List<PendingPoViewModel> listOfData, string fileName, string uploadedBy, string recordType);

        //// ----------------- Po List ------------------- ////


        //// ----------------- Indent Dump ------------------- ////
        Task<List<IndentDumpViewModel>> GetIndentListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IndentDumpViewModel?> GetIndentByIdAsync(int id);
        Task<OperationResult> CreateIndentAsync(IndentDump_Service podetail, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateIndentAsync(IndentDump_Service podetail, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteIndentAsync(int id);
        Task<BulkCreateIndentResult> BulkCreateIndentAsync(List<IndentDumpViewModel> listOfData, string fileName, string uploadedBy, string recordType);

        //// ----------------- Indent Dump ------------------- ////


        //// ----------------- Invoice List ------------------- ////
        Task<List<InvoiceListViewModel>> GetInvoiceListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<InvoiceListViewModel?> GetInvoiceByIdAsync(int id);
        Task<OperationResult> CreateInvoiceAsync(Invoice_Service invdetail, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateInvoiceAsync(Invoice_Service invdetail, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteInvoiceAsync(int id);
        Task<BulkCreateInvoiceResult> BulkCreateInvoiceAsync(List<InvoiceListViewModel> listOfData, string fileName, string uploadedBy, string recordType);

        //// ----------------- Invoice List ------------------- ////

        
        //// ----------------- Pc Chart ------------------- ////
        Task<List<PcChartViewModel>> GetPcListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<PcChartViewModel?> GetPcByIdAsync(int id);
        Task<OperationResult> CreatePcAsync(PcChart_Service pcdetail, bool returnCreatedRecord = false);
        Task<OperationResult> UpdatePcAsync(PcChart_Service pcdetail, bool returnUpdatedRecord = false);
        Task<OperationResult> DeletePcAsync(int id);
        Task<BulkCreatePcResult> BulkCreatePcAsync(List<PcChartViewModel> listOfData, string fileName, string uploadedBy, string recordType);

        //// ----------------- Pc Chart ------------------- ////


        //// ----------------- Region ------------------- ////
        Task<List<RegionViewModel>> GetRegListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<RegionViewModel?> GetRegByIdAsync(int id);
        Task<OperationResult> CreateRegAsync(Region_Service regdetail, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateRegAsync(Region_Service regdetail, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteRegAsync(int id);
        Task<BulkCreateRegionResult> BulkCreateRegAsync(List<RegionViewModel> listOfData, string fileName, string uploadedBy, string recordType);

        //// ----------------- Region ------------------- ////


        Task<List<DropdownOptionViewModel>> GetVendorDropdownAsync();


        //// ----------------- Final Merge ------------------- ////
        Task<List<FinalMergeServiceViewModel>> GetFinalMergeServiceAsync(DateTime? startDate = null, DateTime? endDate = null);

        //// ----------------- Final Merge ------------------- ////

    }
}
