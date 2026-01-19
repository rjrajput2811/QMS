using QMS.Core.DatabaseContext.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.DatabaseContext
{
    [Table("tbl_SurgeTestReportDetail")]
    public class SurgeTestReportDetails : SqlTable
    {        
        public int DetailId { get; set; }
        public int ReportId { get; set; }
        public string? TestType { get; set; }
        public int RowNo { get; set; }
        public bool IsResult { get; set; }
        public string? Voltage_KV { get; set; }
        public string? Mode { get; set; }

        public string? L_N_DM_90 { get; set; }
        public string? L_N_DM_180 { get; set; }
        public string? L_N_DM_270 { get; set; }
        public string? L_N_DM_0 { get; set; }

        public string? L_E_CM_90 { get; set; }
        public string? L_E_CM_180 { get; set; }
        public string? L_E_CM_270 { get; set; }
        public string? L_E_CM_0 { get; set; }

        public string? N_E_CM_90 { get; set; }
        public string? N_E_CM_180 { get; set; }
        public string? N_E_CM_270 { get; set; }
        public string? N_E_CM_0 { get; set; }

        public string? Observation { get; set; }

        public string? PassFail { get; set; }
        public string? SPD_OK { get; set; }
        public string? Driver_LED_PCB_OK { get; set; }
        public bool Deleted { get; set; }

        [ForeignKey("ReportId")]
        public virtual SurgeTestReport? ParentReport { get; set; }
    }
}
