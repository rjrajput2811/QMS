using QMS.Core.DatabaseContext.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext
{
    [Table("tbl_DN_Tracker")]
    public class DNTracker : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("DNoteId")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

        public string? DNoteNumber { get; set; }

        public string? DNoteCategory { get; set; }

        public string? ProductCode { get; set; }

        public string? ProdDesc { get; set; }

        public string? Wattage { get; set; }

        public int? DQty { get; set; }

        public string? DRequisitionBy { get; set; }

        public string? Vendor { get; set; }

        public string? Remark { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
