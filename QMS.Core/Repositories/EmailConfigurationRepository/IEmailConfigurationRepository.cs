using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.EmailConfigurationRepository
{
    public interface IEmailConfigurationRepository
    {
        Task<EmailConfigurationViewModel> GetEmailConfiguration();
        Task<OperationResult> CreateEmailConfiguration(EmailConfigurationViewModel emailConfigurationModel);
        Task<OperationResult> UpdateEmailConfiguration(EmailConfigurationViewModel emailConfigurationModel);
        Task<bool> SendForgotPassword(string tempPassword, string userEmail);
        //(string, string) GenerateMailBody(string mailbody, string mailHeader, string mailFooter, string subject, int mailType, string CsoNo, string CsoDT, string Location, string ProdLine, string Description, string ProductCode, string BatchCode, string Qty, string CsoUrl, string status);
        Task<bool> SendEmailTrigger(string stakeHolderEmailIds, string userEmails, string createdUser, string mailSubject, string mailBody);
        Task<bool> SendOTPEmailAsync(string userEmail, string otp, string body);
        Task<string> GenerateOtpLoginEmailBody(string otp);
    }
}
