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
    [Table("tbl_ContractorDetail_Service")]
    public class ContractorDetail_Service : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("Cont_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

        public string? Cont_Firm_Name { get; set; }
        public string? Cont_Name { get; set; }
        public string? Cont_Ven_Code { get; set; }
        public string? Pan_No { get; set; }
        public string? Gst { get; set; }
        public DateTime? Cont_Valid_Date { get; set; }
        public string? Location { get; set; }
        public string? No_Tech { get; set; }
        public string? Moblie { get; set; }
        public double? Monthly_Salary { get; set; }
        public double? Conv_Bike_User { get; set; }
        public string? Daily_Wages_Local { get; set; }
        public string? Conv_Fixed_Actual { get; set; }
        public string? Daily_Wages_Outstation { get; set; }
        public string? Dailywages_Ext_Manpower { get; set; }
        public double? OT_Charge_Full_Night { get; set; }
        public double? OT_Charge_Till_10 { get; set; }
        public double? OT_Outstation_night_Travel { get; set; }
        public bool Con_Cont_ESIC_Tech { get; set; }
        public bool Con_Cont_PF_Tech { get; set; }
        public string? Attchment { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
