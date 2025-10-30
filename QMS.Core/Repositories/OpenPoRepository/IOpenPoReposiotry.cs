using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.OpenPoRepository
{
    public interface IOpenPoReposiotry
    {
        Task<List<Open_PoViewModel>> GetListAsync();
        Task<List<Open_Po_LogViewModel>> GetPoLogListAsync();
        Task<List<Open_Po_LogViewModel>> GetSOLogListAsync();
        Task<List<Open_PoViewModel>> GetVendorListAsync(string vendor);
        //Task<BulkCreateLogResult> BulkCreateAsync(List<Open_PoViewModel> listOfData, string fileName, string uploadedBy);
        Task<BulkCreateLogResult> BulkCreateAsync_Dapper(IList<Open_PoViewModel> listOfData, string fileName, string uploadedBy);

        Task<Opne_Po_DeliverySchViewModel> GetByPOIdAsync(int poId);
        Task<int> SaveDeliveryScheduleAsync(Opne_Po_DeliverySchViewModel schedules, string updatedBy);

        Task<OperationResult> SaveBuffScheduleAsync(int id, int buff, string updatedBy, bool returnUpdatedRecord = false);

        Task<(List<Open_Po> poHeaders, List<Opne_Po_DeliverySchedule> deliverySchedules)> GetOpenPOWithDeliveryScheduleAsync(string vendor);

        Task<List<Sales_Order_ViewModel>> GetSalesOrderListAsync(string? type);

        Task<List<Sales_Order_ViewModel>> GetSalesOrdersQtyAsync(string? type);

        Task<BulkSalesCreateLogResult> BulkSalesCreateAsync(IList<Sales_Order_ViewModel> listOfData, string fileName, string uploadedBy);

        Task<(List<MatchedRecordViewModel> matched, List<MatchDeliverySchViewModel> deliverySch, MatchSummaryViewModel? summary)> GetPO_SO_MatchReportAsync(string? type);

        Task<OperationResult> UpdateAsync(Open_Po updatedRecord, bool returnUpdatedRecord = false);

        Task<List<PCCalendarViewModel>> GetPCListAsync();

        Task<BulkPCCreateLogResult> BulkPCCreateAsync(List<PCCalendarViewModel> listOfData, string fileName, string uploadedBy);
        Task<(List<OpenPODetailViewModel> openPo, List<So_DeliveryScheduleViewModel> deliverySch)> GetPOListByMaterialRefNoAsync(string? material, string? oldMaterialNo, int soId);
        Task SOSaveDeliverySchAsync(So_DeliveryScheduleViewModel model, string updatedBy);
        Task<(List<MatchedRecordViewModel> soHeaders, List<MatchSODeliverySchViewModel> deliverySchedules)> GetSOWithDeliveryScheduleAsync(string type);

        Task<List<MTAMasterViewModel>> GetMTAListAsync();
        Task<BulkMTACreateResult> BulkMTACreateAsync(List<MTAMasterViewModel> listOfData, string fileName, string uploadedBy);
    }
}
