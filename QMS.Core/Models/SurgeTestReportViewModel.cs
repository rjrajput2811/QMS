using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class SurgeTestReportViewModel
    {
        public SurgeTestReportViewModel()
        {
            DetailRows = new List<SurgeTestDetailVM>();

            // Initialize 6 data rows for WithoutSPD (indexes 0-5)
            for (int i = 0; i < 6; i++)
            {
                DetailRows.Add(new SurgeTestDetailVM
                {
                    RowNo = i + 1,
                    TestType = "WithoutSPD",
                    IsResult = false
                });
            }

            // Add 1 result row for WithoutSPD (index 6)
            DetailRows.Add(new SurgeTestDetailVM
            {
                RowNo = 7,
                TestType = "WithoutSPD",
                IsResult = true
            });

            // Initialize 6 data rows for WithSPD (indexes 7-12)
            for (int i = 7; i < 13; i++)
            {
                DetailRows.Add(new SurgeTestDetailVM
                {
                    RowNo = i + 1,
                    TestType = "WithSPD",
                    IsResult = false
                });
            }

            // Add 1 result row for WithSPD (index 13)
            DetailRows.Add(new SurgeTestDetailVM
            {
                RowNo = 14,
                TestType = "WithSPD",
                IsResult = true
            });
        }
        public int Id { get; set; }
        public DateTime? ReportDate { get; set; }
        public string? ReportNo { get; set; }
        public string? BatchCode { get; set; }
        public string? ProductCatRef { get; set; }
        public string? ProductDescription { get; set; }
        public string? DriverCode { get; set; }
        public string? SPDCode { get; set; }
        public string? LEDConfiguration { get; set; }
        public string? PKDCode { get; set; }
        public string? ReferenceStandard { get; set; }
        public string? AcceptanceNorm { get; set; }

        public string? Surge_Photo { get; set; }
        public string? CheckedBy { get; set; }
        public string? VerifiedBy { get; set; }
        public List<SurgeTestDetailVM> DetailRows { get; set; }
        public int AddedBy { get; set; }
        public DateTime? AddedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? User { get; set; }

        public IFormFile? Photo_SurgeFile { get; set; }
    }

    public class SurgeTestDetailVM
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
    }
}
