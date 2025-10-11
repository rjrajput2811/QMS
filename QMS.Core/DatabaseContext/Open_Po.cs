using QMS.Core.DatabaseContext.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.DatabaseContext
{
    [Table("tbl_Open_PO")]
    public class Open_Po : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("Ven_PoId")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

        public string? Key { get; set; }
        public string? PR_Type { get; set; }
        public string? PR_Desc { get; set; }
        public string? Requisitioner { get; set; }
        public string? Tracking_No { get; set; }
        public string? PR_No { get; set; }
        public string? Batch_No { get; set; }
        public string? Reference_No { get; set; }
        public string? Vendor { get; set; }
        public string? PO_No { get; set; }
        public DateTime? PO_Date { get; set; }
        public int? PO_Qty { get; set; }
        public int? Balance_Qty { get; set; }
        public string? Destination { get; set; }
        public DateTime? Delivery_Date { get; set; }
        public Decimal? Balance_Value { get; set; }
        public string? Material { get; set; }
        public DateTime? Hold_Date { get; set; }
        public DateTime? Cleared_Date { get; set; }

        public string? Key1 { get; set; }
        public DateTime? Comit_Date { get; set; }
        public int? Comit_Qty { get; set; }
        public int? Comit_Planner_Qty { get; set; }
        public DateTime? Comit_Planner_date { get; set; }
        public string? PCWeekDate { get; set; }
        public DateTime? Comit_Vendor_Date { get; set; }
        public int? Comit_Vendor_Qty { get; set; }
        public string? Comit_Planner_Remark { get; set; }
        public DateTime? Comit_Date1 { get; set; }
        public int? Comit_Qty1 { get; set; }
        public DateTime? Comit_Final_Date { get; set; }
        public int? Comit_Final_Qty { get; set; }

        public int? Buffer_Day { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? LastUploadedAt { get; set; }
        public Guid? LastUploadBatchId { get; set; }
        public bool SuppressChildDelivery { get; set; }

    }
}
