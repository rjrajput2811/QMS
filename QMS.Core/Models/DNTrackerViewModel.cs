using System;
using System.ComponentModel.DataAnnotations;

namespace QMS.Core.Models
{
    public class DNTrackerViewModel
    {
        public int Id { get; set; }

        public string? DNoteNumber { get; set; }

        public string? DNoteCategory { get; set; }

        public string? ProductCode { get; set; }

        public string? ProdDesc { get; set; }

        public string? Wattage { get; set; }

        public int? DQty { get; set; }

        public string? DRequisitionBy { get; set; }

        public string? Vendor { get; set; }

        public string? Remark { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
