using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class InstallationTrialViewModel
    {
        public int Id { get; set; }
        public string? ReportNo { get; set; }
        public string? ProductCatRef { get; set; }
        public string? BatchCode { get; set; }
        public DateTime? ReportDate { get; set; }
        public string? ProductDescription { get; set; }
        public string? PKD { get; set; }
        public int SampleQty { get; set; }
        public string? ProductCategory_SampleDetails { get; set; }
        public string? ProductCategory_Result { get; set; }
        public string? InstallationSheet_SampleDetails { get; set; }
        public string? InstallationSheet_Result { get; set; }
        public string? MountingMechanism_SampleDetails { get; set; }
        public string? MountingMechanism_Result { get; set; }
        public string? DurationOfTest_SampleDetails { get; set; }
        public string? DurationOfTest_Result { get; set; }
        public string? InstallationWith4xLoad_SampleDetails { get; set; }
        public string? InstallationWith4xLoad_Result { get; set; }
        public string? InstallationWithSandBagLoad_SampleDetails { get; set; }
        public string? InstallationWithSandBagLoad_Result { get; set; }
        public string? InstallationWithSandBagLoad2_SampleDetails { get; set; }
        public string? InstallationWithSandBagLoad2_Result { get; set; }
        public string? Photo_WithLoad { get; set; }
        public string? Photo_WithoutLoad { get; set; }
        public string? OverallResult { get; set; }
        public string? CheckedBy { get; set; }
        public string? VerifiedBy { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? User { get; set; }
        public bool Deleted { get; set; }

        public IFormFile? Photo_WithLoadFile { get; set; }
        public IFormFile? Photo_WithoutLoadFile { get; set; }
    }
}
