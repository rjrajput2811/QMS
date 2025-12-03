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
    [Table("tbl_Ca_Report")]
    public class CAReport : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("Ca_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

        public string? Complaint_No { get; set; }
        public DateTime? Report_Date { get; set; }

        public bool Cust_Complaints { get; set; }
        public bool NPI_Validations { get; set; }
        public bool PDI_Obser { get; set; }
        public bool System { get; set; }

        public string? Cust_Name_Location { get; set; }
        public string? Source_Complaint { get; set; }
        public string? Prod_Code_Desc { get; set; }
        public string? Desc_Complaint { get; set; }

        public string? Batch_Code { get; set; }
        public string? Pkd { get; set; }

        public int? Supp_Qty { get; set; }
        public int? Failure_Qty { get; set; }
        public int? Failure { get; set; }

        public string? Age_Install { get; set; }
        public string? Description { get; set; }
        public string? Problem_State { get; set; }

        public string? Problem_Visual_ImgA { get; set; }
        public string? Problem_Visual_ImgB { get; set; }
        public string? Problem_Visual_ImgC { get; set; }

        public string? Initial_Observ { get; set; }

        public bool Man_Issue_Prob { get; set; }
        public bool Design_Prob { get; set; }
        public bool Site_Issue_Prob { get; set; }
        public bool Com_Gap_Prob { get; set; }
        public bool Install_Issues_Prov { get; set; }
        public bool Wrong_App_Prob { get; set; }

        public string? Interim_Corrective { get; set; }
        public string? Root_Cause_Anal { get; set; }
        public string? Corrective_Action { get; set; }

        public string? Before_Photo { get; set; }
        public string? After_Photo { get; set; }

        public string? Rca_Prepared_By { get; set; }
        public string? Name_Designation { get; set; }

        public DateTime? Date { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public List<CAReportDetail>? Details { get; set; }

        public DateTime? Interim_Cor_Date { get; set; }
        public DateTime? Root_Cause_Date { get; set; }
        public DateTime? Corrective_Action_Date { get; set; }





    }

    [Table("tbl_Moni_Plan_Ca")]
    public class CAReportDetail : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        public int Moni_Plan_Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        public int Ca_Id { get; set; }
        public string? Action_Plan { get; set; }
        public DateTime? Target_Date { get; set; }
        public string? Resp { get; set; }
        public string? Status { get; set; }

        [ForeignKey("Ca_Id")]
        public CAReport? CAReport { get; set; }
    }
}
