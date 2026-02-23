using QMS.Core.DatabaseContext.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.DatabaseContext
{
    [Table("tbl_EmailOTPs")]
    public class EmailOTP : SqlTable
    {
        public required string Email { get; set; }
        public required string OTP { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
