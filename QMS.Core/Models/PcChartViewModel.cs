using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class PcChartViewModel
    {
        public  int Id { get; set; }
        public  bool Deleted { get; set; }
        public DateTime? Date { get; set; }
        public string? PC { get; set; }
        public string? FY { get; set; }
        public string? Qtr { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
