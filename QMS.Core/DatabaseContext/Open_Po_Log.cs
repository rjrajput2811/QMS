using QMS.Core.DatabaseContext.Shared;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.DatabaseContext
{
    [Table("tbl_OpnPo_Log")]
    public class Open_Po_Log : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("OpnPoLog_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public int? TotalRecords { get; set; }
        public int? ImportedRecords { get; set; }
        public int? FailedRecords { get; set; }
        public string? UploadedBy { get; set; }
        public DateTime? UploadedAt { get; set; }

    }

    public class BulkCreateLogResult
    {
        public OperationResult Result { get; set; } = new();
        public List<(Open_PoViewModel Record, string Reason)> FailedRecords { get; set; } = new();
    }

    public class BulkSalesCreateLogResult
    {
        public OperationResult Result { get; set; } = new();
        public List<(Sales_Order_ViewModel Record, string Reason)> FailedRecords { get; set; } = new();
    }

    public class BulkPCCreateLogResult
    {
        public OperationResult Result { get; set; } = new();
        public List<(PCCalendarViewModel Record, string Reason)> FailedRecords { get; set; } = new();
    }

    public class BulkMTACreateResult
    {
        public OperationResult Result { get; set; } = new();
        public List<(MTAMasterViewModel Record, string Reason)> FailedRecords { get; set; } = new();
    }

}
