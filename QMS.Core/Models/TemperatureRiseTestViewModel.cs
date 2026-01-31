namespace QMS.Core.Models;

public class TemperatureRiseTestViewModel
{
    public int Id { get; set; }
    public string? ReportNo { get; set; }
    public DateTime? ReportDate { get; set; }
    public string? TestingLocation { get; set; }
    public string? ProductCatRef { get; set; }
    public string? ProductDescription { get; set; }
    public string? BatchCode { get; set; }
    public string? PKD { get; set; }
    public string? HeatSinkMaterial { get; set; }
    public string? HeatSinkWeight { get; set; }
    public string? LensDetails { get; set; }
    public string? ThermalPasteDetails { get; set; }
    public decimal? NominalOperatingVoltage { get; set; }
    public string? DriverUsed { get; set; }
    public int? NoOfDrivers { get; set; }
    public decimal? DriverOutputVoltage { get; set; }
    public decimal? DriverOutputCurrent { get; set; }
    public decimal? DriverAllowableTc { get; set; }
    public decimal? DriverAllowableTa { get; set; }
    public string? PcbMaterialMake { get; set; }
    public string? PcbSizeQty { get; set; }
    public string? LedUsed { get; set; }
    public int? NoOfLeds { get; set; }
    public int? NoOfLedsInSeries { get; set; }
    public int? NoOfLedsInParallel { get; set; }
    public decimal? LedRjTH { get; set; }
    public decimal? LedVf { get; set; }
    public decimal? LedIf { get; set; }
    public decimal? LedWdc { get; set; }
    public string? ProbeT1_Desc { get; set; }
    public string? ProbeT2_Desc { get; set; }
    public string? ProbeT3_Desc { get; set; }
    public string? ProbeT4_Desc { get; set; }
    public string? ProbeT5_Desc { get; set; }
    public string? ProbeT6_Desc { get; set; }
    public string? ProbeT7_Desc { get; set; }
    public string? ProbeT8_Desc { get; set; }
    public string? ProbeT9_Desc { get; set; }
    public string? ProbeT10_Desc { get; set; }
    public string? ProbeT11_Desc { get; set; }
    public string? ProbeT12_Desc { get; set; }
    public string? ProbeT13_Desc { get; set; }
    public string? ProbeT14_Desc { get; set; }
    public string? ProbeT15_Desc { get; set; }
    public string? ProbeT16_Desc { get; set; }

    public decimal? Conclusion_MaxRecordedTJ { get; set; }
    public decimal? Conclusion_AllowableTJ { get; set; }
    public decimal? Conclusion_MaxRecordedDriverTc { get; set; }
    public decimal? Conclusion_AllowableDriverTc { get; set; }
    public decimal? Conclusion_MaxRecordedLensTemp { get; set; }
    public string? Conclusion_OverThermalCutoff { get; set; }
    public string? Conclusion_Result { get; set; }
    public string? ConductedBy { get; set; }
    public string? WitnessBy { get; set; }
    public string? ApprovedBy { get; set; }
    public string? ConductedBySecnd { get; set; }
    public string? WitnessBySecnd { get; set; }
    public string? ApprovedBySecnd { get; set; }

    public int AddedBy { get; set; }
    public DateTime AddedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public string? User { get; set; }

    public List<TemperatureRiseDetailModal> Details { get; set; } = new();
}
