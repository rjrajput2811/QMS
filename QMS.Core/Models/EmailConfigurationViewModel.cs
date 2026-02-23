using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class EmailConfigurationViewModel
    {
        public int Id { get; set; }
        public string? From { get; set; }
        public string? SmtpServer { get; set; }
        public int Port { get; set; }
        public string? SMTPUserName { get; set; }
        public string? SMTPPassword { get; set; }
        public bool SslRequired { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool Deleted { get; set; }
    }
}
