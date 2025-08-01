using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.PaymentTrackerRepository
{
    public interface IPaymentTracRepository
    {
        Task<List<PaymentTracViewModel>> GetListAsync();
        Task<OperationResult> CreateAsync(Payment_Tracker payment, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(Payment_Tracker payment, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int Id);
        Task<PaymentTracViewModel?> GetByIdAsync(int id);
        Task<bool> CheckDuplicate(string searchText, int id);
    }
}
