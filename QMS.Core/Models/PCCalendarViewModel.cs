using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class PCCalendarViewModel
    {
        public  int Id { get; set; }
        public  bool Deleted { get; set; }
        public int? PC { get; set; }
        public int? Week { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? Days { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
