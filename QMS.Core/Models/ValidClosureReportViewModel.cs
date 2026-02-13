using Microsoft.AspNetCore.Http;
using QMS.Core.DatabaseContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class ValidClosureReportViewModel
    {
        public int Id { get; set; }
        public string? ProductCatRef { get; set; }
        public string? ReportNo { get; set; }
        public DateTime? ReportDate { get; set; }
        public string? ProductDescription { get; set; }
        public string? ValidationDoneBy { get; set; }
        public string? BatchCode { get; set; }
        public string? PKD { get; set; }
        public int QuantityOffered { get; set; }
        public DateTime? ClosureDate { get; set; }
        public string? VendorQA_FinalComments { get; set; }
        public string? WiproQA_FinalComments { get; set; }
        public string? VendorQA_Signature { get; set; }
        public string? WiproQA_Signature { get; set; }
        public string? ReportAttached { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? User { get; set; }
        public bool Deleted { get; set; }
        [NotMapped] public IFormFile? VendorQA_SignatureFile { get; set; }
        [NotMapped] public IFormFile? WiproQA_SignatureFile { get; set; }
        [NotMapped] public IFormFile? ReportAttachedFile { get; set; }
        public List<ValidationClosureDetailViewModel>? Details { get; set; }
    }

    public class ValidationClosureDetailViewModel
    {
        public int Id { get; set; }
        public string? Open_Point { get; set; }
        public string? Action_Taken { get; set; }
        public string? Stake_Holder { get; set; }
        public string? Status { get; set; }
        public int ValidClose_Id { get; set; }
        public string? Evidence { get; set; }
        public string? Attached { get; set; }
        public bool Delete { get; set; }
        [NotMapped] public IFormFile? EvidenceFile { get; set; }
        [NotMapped] public IFormFile? AttachedFile { get; set; }

    }
}
