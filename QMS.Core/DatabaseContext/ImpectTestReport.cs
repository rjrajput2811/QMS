using QMS.Core.DatabaseContext.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.DatabaseContext;

[Table("tbl_ImpectTestReport")]
public class ImpectTestReport : SqlTable
{
    public string? CustomerProjectName { get; set; }
    public string? ReportNo { get; set; }
    public DateTime? ReportDate { get; set; }
    public string? ProductCatRef { get; set; }
    public string? ProductDescription { get; set; }
    public string? BatchCode { get; set; }
    public int Quantity { get; set; }
    public string? Weight_kg_IK01 { get; set; }
    public string? Weight_kg_IK02 { get; set; }
    public string? Weight_kg_IK03 { get; set; }
    public string? Weight_kg_IK04 { get; set; }
    public string? Weight_kg_IK05 { get; set; }
    public string? Weight_kg_IK06 { get; set; }
    public string? Weight_kg_IK07 { get; set; }
    public string? Weight_kg_IK08 { get; set; }
    public string? Weight_kg_IK09 { get; set; }
    public string? Weight_kg_IK10 { get; set; }
    public string? Weight_Material_IK01 { get; set; }
    public string? Weight_Material_IK02 { get; set; }
    public string? Weight_Material_IK03 { get; set; }
    public string? Weight_Material_IK04 { get; set; }
    public string? Weight_Material_IK05 { get; set; }
    public string? Weight_Material_IK06 { get; set; }
    public string? Weight_Material_IK07 { get; set; }
    public string? Weight_Material_IK08 { get; set; }
    public string? Weight_Material_IK09 { get; set; }
    public string? Weight_Material_IK10 { get; set; }
    public string? Distance_cm_IK01 { get; set; }
    public string? Distance_cm_IK02 { get; set; }
    public string? Distance_cm_IK03 { get; set; }
    public string? Distance_cm_IK04 { get; set; }
    public string? Distance_cm_IK05 { get; set; }
    public string? Distance_cm_IK06 { get; set; }
    public string? Distance_cm_IK07 { get; set; }
    public string? Distance_cm_IK08 { get; set; }
    public string? Distance_cm_IK09 { get; set; }
    public string? Distance_cm_IK10 { get; set; }
    public string? ImpactEnergy_joules_IK01 { get; set; }
    public string? ImpactEnergy_joules_IK02 { get; set; }
    public string? ImpactEnergy_joules_IK03 { get; set; }
    public string? ImpactEnergy_joules_IK04 { get; set; }
    public string? ImpactEnergy_joules_IK05 { get; set; }
    public string? ImpactEnergy_joules_IK06 { get; set; }
    public string? ImpactEnergy_joules_IK07 { get; set; }
    public string? ImpactEnergy_joules_IK08 { get; set; }
    public string? ImpactEnergy_joules_IK09 { get; set; }
    public string? ImpactEnergy_joules_IK10 { get; set; }
    public string? ApplicableTest_IK01 { get; set; }
    public string? ApplicableTest_IK02 { get; set; }
    public string? ApplicableTest_IK03 { get; set; }
    public string? ApplicableTest_IK04 { get; set; }
    public string? ApplicableTest_IK05 { get; set; }
    public string? ApplicableTest_IK06 { get; set; }
    public string? ApplicableTest_IK07 { get; set; }
    public string? ApplicableTest_IK08 { get; set; }
    public string? Applicable_IK09 { get; set; }
    public string? ApplicableTest_IK10 { get; set; }
    public string? Observation_DamageObserved { get; set; }
    public string? Observation_LivePartsAccessibility { get; set; }
    public string? Observation_Photo { get; set; }
    public string? TestResult { get; set; }
    public string? TestedBy { get; set; }
    public string? VerifiedBy { get; set; }
    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
