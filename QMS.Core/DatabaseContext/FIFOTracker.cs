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
    [Table("tbl_FIFOTrac")]
    public class FIFOTracker :SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("FifoTrac_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

        public DateTime? Sample_Recv_Date {get; set;}
        public string? Sample_Cat_Ref {get; set;}
        public string? Sample_Desc {get; set;}
        public string? Vendor {get; set;}
        public string? Sample_Qty {get; set;}
        public string? Test_Req {get; set;}
        public string? Test_Status {get; set;}
        public string? Responsbility {get; set;}
        public string? Current_Status { get; set; }
        public DateTime? Test_Completion_Date {get; set;}
        public DateTime? Report_Release_Date {get; set;}
        public DateTime? NABL_Released_Date {get; set;}
        public string? Final_Report {get; set;}
        public string? Remark {get; set;}
        public int? Delayed_Days { get; set;}
       
        public string? CreatedBy {get; set;}
        public DateTime? CreatedDate {get; set;}
        public string? UpdatedBy {get; set;}
        public DateTime? UpdatedDate {get; set;}
    }

    [Table("tbl_TestReq_FIFO")]
    public class TestReq_FIFO : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("TestReq_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

        public string? Test { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
