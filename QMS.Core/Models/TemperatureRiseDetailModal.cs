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
      

        public decimal? MaxVal_T1 { get; set; }
        public decimal? MaxVal_T2 { get; set; }
        public decimal? MaxVal_T3 { get; set; }
        public decimal? MaxVal_T4 { get; set; }
        public decimal? MaxVal_T5 { get; set; }
        public decimal? MaxVal_T6 { get; set; }
        public decimal? MaxVal_T7 { get; set; }
        public decimal? MaxVal_T8 { get; set; }
        public decimal? MaxVal_T9 { get; set; }
        public decimal? MaxVal_T10 { get; set; }
        public decimal? MaxVal_T11 { get; set; }
        public decimal? MaxVal_T12 { get; set; }
        public decimal? MaxVal_T13 { get; set; }
        public decimal? MaxVal_T14 { get; set; }
        public decimal? MaxVal_T15 { get; set; }
        public decimal? MaxVal_T16 { get; set; }
        public decimal? MaxVal_TJ { get; set; }

        public List<TemperatureRiseDetailModal> Details { get; set; } = new();
    }
}
