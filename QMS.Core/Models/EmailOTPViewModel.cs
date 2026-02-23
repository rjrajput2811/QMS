using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class EmailOTPViewModel
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string OTP { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
