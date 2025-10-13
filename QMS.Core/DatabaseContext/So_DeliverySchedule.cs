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
    [Table("tbl_So_DeliverySchedule")]
    public class So_DeliverySchedule : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("SoDelSchu_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

        public int? SaleOrder_Id { get; set; }
        public string? Vendor { get; set; }
        public string? SO_No { get; set; }
        public DateTime? SO_Date { get; set; }
        public int? SO_Qty { get; set; }
        public DateTime? Delivery_Date { get; set; }
        public int? Delivery_Qty { get; set; }
        public string? Delivery_Remark { get; set; }
        public string? Date_PC_Week { get; set; }
        public string? Key { get; set; }
        public string? Key1 { get; set; }
        public string? Material { get; set; }
        public string? Old_Material_No { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }

    }
}
