using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class PDI_Auth_SignViewModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public string? Vendor { get; set; }
        public string? Address { get; set; }
        public string? Pdi_Inspector { get; set; }
        public string? Designation { get; set; }
        public string? Photo_Inspector { get; set; }
        public string? Specimen_Sign { get; set; }
        public string? Remark { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
