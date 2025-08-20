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
        Task<List<PaymentTracViewModel>> GetListAsync(DateTime? startDate, DateTime? endDate);
        Task<OperationResult> CreateAsync(Payment_Tracker payment, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(Payment_Tracker payment, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int Id);
        Task<PaymentTracViewModel?> GetByIdAsync(int id);
        Task<bool> CheckDuplicate(string searchText, int id);

        Task<List<LabPaymentViewModel>> GetLabPaymentAsync();
        Task<Lab_Payment?> GetLabPaymentByIdAsync(int Id);
        Task<OperationResult> CreateLabPaymentAsync(LabPaymentViewModel newNatProjectRecord, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateLabPaymentAsync(LabPaymentViewModel updateNatProjectRecord, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteLabPaymentAsync(int Id);
        Task<bool> CheckLabPaymentDuplicate(string searchText, int Id);
        Task<List<DropdownOptionViewModel>> GetLabDropdownAsync();

        Task<bool> UpdateAttachmentAsync(int id, string fileName);
    }
}
