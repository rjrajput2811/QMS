using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMS.Core.DatabaseContext.Shared;

namespace QMS.Core.DatabaseContext
{
    [Table("tbl_SPM_Buy")]
    public class SPM_Buy : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("SpmBuy_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------
        public string? Supp_Name { get; set; }
        public string? Quater { get; set; }
        public string? Fy { get; set; }
        public string? Month { get; set; }
        public string? Pc { get; set; }
        public string? Location { get; set; }
        public string? Sqa { get; set; }
        public double? Ppm { get; set; }
        public double? Delivery { get; set; }
        public double? Capa { get; set; }
        public double? Audit { get; set; }
        public double? Cost { get; set; }
        public double? Npi_Resp { get; set; }
        public double? Rep_Lead_Time { get; set; }
        public double? Ppm_Rating { get; set; }
        public double? Delivery_Rating { get; set; }
        public double? Capa_Rating { get; set; }
        public double? Audit_Rating { get; set; }
        public double? Cost_Rating { get; set; }
        public double? Npi_Resp_Rating { get; set; }
        public double? Rep_Lead_Time_Rating { get; set; }
        public double? Total { get; set; }
        public double? Star_Rating { get; set; }
        public string? Performance { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
