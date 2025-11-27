using QMS.Core.DatabaseContext.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.DatabaseContext
{
    [Table("tbl_CSAT_Summary")]
    public class CSAT_Summary :SqlTable
    {
        //-----------------------------------
        // SqlTable override
        //-----------------------------------
        [Key]
        [Column("CsatSum_Id")]
        public override int Id { get; set; }

        [Column("IsDeleted")]
        public override bool Deleted { get; set; }
        //------------ END overrides --------

        public double? Q1Pc1_ReqSent { get; set; }
        public double? Q1Pc1_ResRece { get; set; }
        public double? Q1Pc1_Promoter { get; set; }
        public double? Q1Pc1_Collection { get; set; }
        public double? Q1Pc1_Detractor { get; set; }
        public double? Q1Pc1_Nps { get; set; }
        public string? Q1Pc1_Detractor_Detail { get; set; }

        public double? Q1Pc2_ReqSent { get; set; }
        public double? Q1Pc2_ResRece { get; set; }
        public double? Q1Pc2_Promoter { get; set; }
        public double? Q1Pc2_Collection { get; set; }
        public double? Q1Pc2_Detractor { get; set; }
        public double? Q1Pc2_Nps { get; set; }
        public string? Q1Pc2_Detractor_Detail { get; set; }

        public double? Q1Pc3_ReqSent { get; set; }
        public double? Q1Pc3_ResRece { get; set; }
        public double? Q1Pc3_Promoter { get; set; }
        public double? Q1Pc3_Collection { get; set; }
        public double? Q1Pc3_Detractor { get; set; }
        public double? Q1Pc3_Nps { get; set; }
        public string? Q1Pc3_Detractor_Detail { get; set; }

        public double? Q1Q1_ReqSent { get; set; }
        public double? Q1Q1_ResRece { get; set; }
        public double? Q1Q1_Promoter { get; set; }
        public double? Q1Q1_Collection { get; set; }
        public double? Q1Q1_Detractor { get; set; }
        public double? Q1Q1_Nps { get; set; }
        public string? Q1Q1_Detractor_Detail { get; set; }

        public double? Q2Pc4_ReqSent { get; set; }
        public double? Q2Pc4_ResRece { get; set; }
        public double? Q2Pc4_Promoter { get; set; }
        public double? Q2Pc4_Collection { get; set; }
        public double? Q2Pc4_Detractor { get; set; }
        public double? Q2Pc4_Nps { get; set; }
        public string? Q2Pc4_Detractor_Detail { get; set; }

        public double? Q2Pc5_ReqSent { get; set; }
        public double? Q2Pc5_ResRece { get; set; }
        public double? Q2Pc5_Promoter { get; set; }
        public double? Q2Pc5_Collection { get; set; }
        public double? Q2Pc5_Detractor { get; set; }
        public double? Q2Pc5_Nps { get; set; }
        public string? Q2Pc5_Detractor_Detail { get; set; }

        public double? Q2Pc6_ReqSent { get; set; }
        public double? Q2Pc6_ResRece { get; set; }
        public double? Q2Pc6_Promoter { get; set; }
        public double? Q2Pc6_Collection { get; set; }
        public double? Q2Pc6_Detractor { get; set; }
        public double? Q2Pc6_Nps { get; set; }
        public string? Q2Pc6_Detractor_Detail { get; set; }

        public double? Q2Q2_ReqSent { get; set; }
        public double? Q2Q2_ResRece { get; set; }
        public double? Q2Q2_Promoter { get; set; }
        public double? Q2Q2_Collection { get; set; }
        public double? Q2Q2_Detractor { get; set; }
        public double? Q2Q2_Nps { get; set; }
        public string? Q2Q2_Detractor_Detail { get; set; }

        public double? Q3Pc7_ReqSent { get; set; }
        public double? Q3Pc7_ResRece { get; set; }
        public double? Q3Pc7_Promoter { get; set; }
        public double? Q3Pc7_Collection { get; set; }
        public double? Q3Pc7_Detractor { get; set; }
        public double? Q3Pc7_Nps { get; set; }
        public string? Q3Pc7_Detractor_Detail { get; set; }

        public double? Q3Pc8_ReqSent { get; set; }
        public double? Q3Pc8_ResRece { get; set; }
        public double? Q3Pc8_Promoter { get; set; }
        public double? Q3Pc8_Collection { get; set; }
        public double? Q3Pc8_Detractor { get; set; }
        public double? Q3Pc8_Nps { get; set; }
        public string? Q3Pc8_Detractor_Detail { get; set; }

        public double? Q3Pc9_ReqSent { get; set; }
        public double? Q3Pc9_ResRece { get; set; }
        public double? Q3Pc9_Promoter { get; set; }
        public double? Q3Pc9_Collection { get; set; }
        public double? Q3Pc9_Detractor { get; set; }
        public double? Q3Pc9_Nps { get; set; }
        public string? Q3Pc9_Detractor_Detail { get; set; }

        public double? Q3Q3_ReqSent { get; set; }
        public double? Q3Q3_ResRece { get; set; }
        public double? Q3Q3_Promoter { get; set; }
        public double? Q3Q3_Collection { get; set; }
        public double? Q3Q3_Detractor { get; set; }
        public double? Q3Q3_Nps { get; set; }
        public string? Q3Q3_Detractor_Detail { get; set; }

        public double? Q4Pc10_ReqSent { get; set; }
        public double? Q4Pc10_ResRece { get; set; }
        public double? Q4Pc10_Promoter { get; set; }
        public double? Q4Pc10_Collection { get; set; }
        public double? Q4Pc10_Detractor { get; set; }
        public double? Q4Pc10_Nps { get; set; }
        public string? Q4Pc10_Detractor_Detail { get; set; }

        public double? Q4Pc11_ReqSent1 { get; set; }
        public double? Q4Pc11_ResRece1 { get; set; }
        public double? Q4Pc11_Promoter1 { get; set; }
        public double? Q4Pc11_Collection1 { get; set; }
        public double? Q4Pc11_Detractor1 { get; set; }
        public double? Q4Pc11_Nps1 { get; set; }
        public string? Q4Pc11_Detractor_Detail1 { get; set; }

        public double? Q4Pc12_ReqSent1 { get; set; }
        public double? Q4Pc12_ResRece1 { get; set; }
        public double? Q4Pc12_Promoter1 { get; set; }
        public double? Q4Pc12_Collection1 { get; set; }
        public double? Q4Pc12_Detractor1 { get; set; }
        public double? Q4Pc12_Nps1 { get; set; }
        public string? Q4Pc12_Detractor_Detail1 { get; set; }

        public double? Q4Q4_ReqSent1 { get; set; }
        public double? Q4Q4_ResRece1 { get; set; }
        public double? Q4Q4_Promoter1 { get; set; }
        public double? Q4Q4_Collection1 { get; set; }
        public double? Q4Q4_Detractor1 { get; set; }
        public double? Q4Q4_Nps1 { get; set; }
        public string? Q4Q4_Detractor_Detail1 { get; set; }

        public double? Ytd_ReqSent11 { get; set; }
        public double? Ytd_ResRece11 { get; set; }
        public double? Ytd_Promoter11 { get; set; }
        public double? Ytd_Collection11 { get; set; }
        public double? Ytd_Detractor11 { get; set; }
        public double? Ytd_Nps11 { get; set; }
        public string? Ytd_Detractor_Detail11 { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

    }
}
