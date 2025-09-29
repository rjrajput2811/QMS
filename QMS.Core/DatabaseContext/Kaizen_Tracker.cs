using QMS.Core.DatabaseContext.Shared;
using QMS.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext
{
    [Table("tbl_Kaizen_Tracker")]
    public class Kaizen_Tracker : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("Kaizen_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

        public string? Vendor { get; set; }

        public string? Kaizen_Theme { get; set; }

        public string? Month { get; set; }

        public string? Team { get; set; }

        public string? Kaizen_Attch { get; set; }

        public string? Remark { get; set; }

        public string? FY { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }

    }

    public class BulkKaizenCreateResult
    {
        public OperationResult Result { get; set; } = new();
        public List<(KaizenTracViewModel Record, string Reason)> FailedRecords { get; set; } = new();
    }
}
