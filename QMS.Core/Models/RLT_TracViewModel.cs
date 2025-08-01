using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class RLT_TracViewModel
    {
        public int Id { get; set; }

        public bool Deleted { get; set; }

        public string? Vendor { get; set; }

        public string? Material { get; set; }

        public string? Ref_No { get; set; }

        public string? Po_No { get; set; }

        public DateTime? Po_Date { get; set; }

        public string? PR_No { get; set; }

        public string? Batch_No { get; set; }

        public int? Po_Qty { get; set; }

        public int? Balance_Qty { get; set; }

        public string? Destination { get; set; }

        public double? Balance_Value { get; set; }

        public int? Lead_Time { get; set; }

        public string? Lead_Time_Range { get; set; }

        public DateTime? Dispatch_Date { get; set; }

        public string? Remark { get; set; }

        public string? Wipro_Remark { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class FinalRLTOutput
    {
        public string? Vendor { get; set; }
        public string? Day0To5 { get; set; }
        public string? Day6To10 { get; set; }
        public string? Day11To15 { get; set; }
        public string? Gt15Days { get; set; }
        public string? Total { get; set; }
    }
}
