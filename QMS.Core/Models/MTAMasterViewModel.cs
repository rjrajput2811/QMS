using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class MTAMasterViewModel
    {
        public  int Id { get; set; }
        public  bool Deleted { get; set; }
        public string? Material_No { get; set; }
        public string? Ref_Code { get; set; }
        public string? Material_Desc { get; set; }
        public int? Tog { get; set; }
        public int? Tor { get; set; }
        public int? Toy { get; set; }
        public int? Spike_Threshold { get; set; }
        public string? Material_Category { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
