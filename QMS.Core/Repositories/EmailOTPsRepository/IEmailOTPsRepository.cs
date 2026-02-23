using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.EmailOTPsRepository
{
    public interface IEmailOTPsRepository
    {
        Task<EmailOTPViewModel> GetExistingNotExpiredOTP(string email);
        Task<OperationResult> CreateOTPAsync(EmailOTPViewModel model);
        Task<OperationResult> DeleteOTPAsync(string email);
        Task<OperationResult> CheckEmailAndOTPAsync(string email, string otp, DateTime expirationTime);
        Task<OperationResult> DeleteExpiredOTPAsync(string email);
    }
}
