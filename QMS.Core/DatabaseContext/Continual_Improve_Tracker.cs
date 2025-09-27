using QMS.Core.DatabaseContext.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.DatabaseContext
{
    [Table("tbl_Continual_Improve_Tracker")]
    public class Continual_Improve_Tracker : SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("Conti_Improve_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

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
