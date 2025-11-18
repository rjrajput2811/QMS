using QMS.Core.DatabaseContext.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext
{
    [Table("tbl_InternalType_Test")]
    public class InternalTypeTest : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("Internal_TypeId")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------
        public string? Report_No { get; set; }
        public DateTime? Date { get; set; }
        public string? Cust_Name { get; set; }
        public string? Samp_Identi_Lab { get; set; }
        public string? Samp_Desc { get; set; }
        public string? Prod_Cat_Code { get; set; }
        public string? Input_Voltage { get; set; }
        public string? Ref_Standard { get; set; }
        public string? TestedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public List<InternalTypeTestDetail>? Details { get; set; }
    }

    [Table("tbl_Test_Detail_InternalTypeTest")]
    public class InternalTypeTestDetail : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("InternalType_DetId")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------
        public int Internal_TypeId { get; set; } 
        public int? SeqNo { get; set; }
        public string? Perticular_Test { get; set; }
        public string? Test_Method { get; set; }
        public string? Test_Requirement { get; set; }
        public string? Test_Result { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("Internal_TypeId")]
        public InternalTypeTest? InternalTypeTest { get; set; }
    }
}
