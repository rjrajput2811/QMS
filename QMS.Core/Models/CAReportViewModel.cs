using QMS.Core.DatabaseContext.Shared;
using QMS.Core.DatabaseContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMS.Core.ViewModels;
using Microsoft.AspNetCore.Http;

namespace QMS.Core.Models
{
    public class CAReportViewModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
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

        public List<CAReportDetailViewModel> Details { get; set; } = new();

        public IFormFile? Problem_Visual_ImgAFile { get; set; }
        public IFormFile? Problem_Visual_ImgBFile { get; set; }
        public IFormFile? Problem_Visual_ImgCFile { get; set; }
        public IFormFile? Before_PhotoFile { get; set; }
        public IFormFile? After_PhotoFile { get; set; }

        public string? Issue_Problems { get; set; }   // or any name you like

        public DateTime? Interim_Cor_Date { get; set; }
        public DateTime? Root_Cause_Date { get; set; }
        public DateTime? Corrective_Action_Date { get; set; }
        public int? Closure_Day { get; set; }
        public int? Interim_Day { get; set; }
        public int? Root_Cause_Day { get; set; }
        public int? Corrective_Action_Day { get; set; }

    }

    public class CAReportDetailViewModel
    {
        public int Moni_Plan_Id { get; set; }
        public  bool Deleted { get; set; }
        public int Ca_Id { get; set; }
        public string? Action_Plan { get; set; }
        public DateTime? Target_Date { get; set; }
        public string? Resp { get; set; }
        public string? Status { get; set; }

    }
}
