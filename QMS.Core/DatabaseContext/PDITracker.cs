using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QMS.Core.DatabaseContext.Shared;

namespace QMS.Core.DatabaseContext
{
    [Table("tbl_PDITracker")]
    public class PDITracker: SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

        public string? PC { get; set; }
        public DateTime? DispatchDate { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductDescription { get; set; }
        public string? BatchCodeVendor { get; set; }
        public string? PONo { get; set; }
        public DateTime? PDIDate { get; set; }
        public string? PDIRefNo { get; set; }
        public int? OfferedQty { get; set; }
        public int? ClearedQty { get; set; }
        public bool? BISCompliance { get; set; }
        public string? InspectedBy { get; set; }
        public string? Remark { get; set; }
        public string? Attahcment { get; set; }
        public string? Document_No { get; set; }
        public string? Revision_No { get; set; }
        public DateTime? Effective_Date { get; set; }
        public DateTime? Revision_Date { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
       
    }
}
