using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class ContiImproveViewModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public DateTime? Date { get; set; }
        public string? Conti_Improve_Plane { get; set; }
        public bool? Iso_9001 { get; set; }
        public string? Iso9001_Plane_Implement { get; set; }
        public bool? Iso_14001 { get; set; }
        public string? Iso14001_Plane_Implement { get; set; }
        public bool? Iso_45001 { get; set; }
        public string? Iso_45001_Plane_Implement { get; set; }
        public string? FY { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
