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
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
