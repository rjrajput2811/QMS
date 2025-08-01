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
    [Table("tbl_Payment_Tracker")]
    public class Payment_Tracker :SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("PayTrac_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------
        public string? Fin_Year { get; set; }
        public string? Month { get; set; }
        public string? Lab { get; set; }
        public string? Vendor { get; set; }
        public string? Type_Test { get; set; }
        public string? Description { get; set; }
        public string? Bis_Id { get; set; }
        public string? Invoice_No { get; set; }
        public DateTime? Invoice_Date { get; set; }
        public double Testing_Fee { get; set; }
        public string? Approval_By { get; set; }
        public string? Remark { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
