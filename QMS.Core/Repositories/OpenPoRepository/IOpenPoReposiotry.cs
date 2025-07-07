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
        Task<List<Open_PoViewModel>> GetVendorListAsync(string vendor);
        Task<BulkCreateLogResult> BulkCreateAsync(List<Open_PoViewModel> listOfData, string fileName, string uploadedBy);

        Task<Opne_Po_DeliverySchViewModel> GetByPOIdAsync(int poId);
        Task SaveDeliveryScheduleAsync(Opne_Po_DeliverySchViewModel schedules, string updatedBy);

        Task<(List<Open_Po> poHeaders, List<Opne_Po_DeliverySchedule> deliverySchedules)> GetOpenPOWithDeliveryScheduleAsync(string vendor);
    }
}
