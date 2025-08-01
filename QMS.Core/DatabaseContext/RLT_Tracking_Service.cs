using QMS.Core.DatabaseContext.Shared;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.DatabaseContext
{
    [Table("tbl_RLTTracker_Service")]
    public class RLT_Tracking_Service : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("RLT_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

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

    public class BulkCreateRLTResult
    {
        public OperationResult Result { get; set; } = new();
        public List<(RLT_TracViewModel Record, string Reason)> FailedRecords { get; set; } = new();
    }
}
