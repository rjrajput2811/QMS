using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models
{
    public class TemperatureRiseDetailModal
    {
        public int? TRId { get; set; }
        public decimal?  TimeHrs { get; set; }
        public decimal? T1 { get; set; }
        public decimal? T2 { get; set; }
        public decimal? T3 { get; set; }
        public decimal? T4 { get; set; }
        public decimal? T5 { get; set; }
        public decimal? T6 { get; set; }
        public decimal? T7 { get; set; }
        public decimal? T8 { get; set; }
        public decimal? T9 { get; set; }
        public decimal? T10 { get; set; }
        public decimal? T11 { get; set; }
        public decimal? T12 { get; set; }
        public decimal? T13 { get; set; }
        public decimal? T14 { get; set; }
        public decimal? T15 { get; set; }
        public decimal? T16 { get; set; }
        public decimal? VIN { get; set; }
        public decimal? IIN { get; set; }
        public decimal? PIN { get; set; }
        public decimal? TJ { get; set; }
      


        public List<TemperatureRiseDetailModal> Details { get; set; } = new();
    }
}
