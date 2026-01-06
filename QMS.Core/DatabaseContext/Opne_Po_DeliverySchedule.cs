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
    [Table("tbl_OpenPo_DeliverySchedule")]
    public class Opne_Po_DeliverySchedule :SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("DeliverySchu_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

        public int Ven_PoId { get; set; }
        public string? Vendor { get; set; }
        public string? PO_No { get; set; }
        public DateTime? PO_Date { get; set; }
        public int? PO_Qty { get; set; }
        public int? Balance_Qty { get; set; }
        public DateTime? Delivery_Date { get; set; }
        public int? Delivery_Qty { get; set; }
        public string? Delivery_Remark { get; set; }
        public string? Date_PC_Week { get; set; }
        public DateTime? Planner_Date { get; set; }
        public string? Planner_PC_Date { get; set; }
        public int? Buffer_Day { get; set; }
        public DateTime? Comit_Date { get; set; }
        public int? Comit_Qty { get; set; }
        public DateTime? Comit_Date1 { get; set; }
        public int? Comit_Qty1 { get; set; }
        public DateTime? Comit_Final_Date { get; set; }
        public int? Comit_Final_Qty { get; set; }
        public string? Key { get; set; }
        public string? Key1 { get; set; }
        public string? Comit_DateStr { get; set; }
        public string? Comit_QtyStr { get; set; }
        public string? Comit_Date1Str { get; set; }
        public string? Comit_Qty1Str { get; set; }
        public string? Comit_Final_DateStr { get; set; }
        public string? Comit_Final_QtyStr { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public  bool Status { get; set; }

    }
}
