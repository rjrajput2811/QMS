using QMS.Core.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class IngressProtectionTestViewModel
    {
        public int Id { get; set; }
        public string? ReportNo { get; set; }
        public string? CustomerProjectName { get; set; }
        public DateTime? ReportDate { get; set; }
        public string? ProductCatRef { get; set; }
        public string? ProductDescription { get; set; }
        public string? BatchCode { get; set; }
        public int Quantity { get; set; }
        public string? IPRating { get; set; }
        public string? PKD { get; set; }
        public string? TestResult { get; set; }
        public string? TestedBy { get; set; }
        public string? VerifiedBy { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? User { get; set; }
        public bool Deleted { get; set; }

        public List<IngressProtectionTestDetailViewModel>? Details { get; set; } = new();
    }

    public class IngressProtectionTestDetailViewModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public string? Description { get; set; }
        public string? CustomerProjectName { get; set; }
        public string? Ip_Test { get; set; }
        public string? Photo_During_Test { get; set; }
        public string? Photo_After_Test { get; set; }
        public string? Observation { get; set; }
        public string? Result { get; set; }
        public int Ingr_Id { get; set; }

    }
}
