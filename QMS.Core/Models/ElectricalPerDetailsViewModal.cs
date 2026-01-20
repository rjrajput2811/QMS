using QMS.Core.DatabaseContext;
using System.ComponentModel.DataAnnotations;

namespace QMS.Core.Models;

public class ElectricalPerDetailsViewModal
{
    public int? ElId { get; set; }
    public int? SampleNo { get; set; }
    public string? ConditionType { get; set; }
    public int? RowNo { get; set; }
    public string? Vac { get; set; }
    public string? IacA { get; set; }
    public string? Wac { get; set; }
    public string? PF { get; set; }
    public string? ATHD { get; set; }
    public string? Vdc { get; set; }
    public string? IdcA { get; set; }
    public string? Wdc { get; set; }
    public string? Eff { get; set; }
    public string? NoLoadV { get; set; }
    public string? StartV { get; set; }
    public string? Result { get; set; }

    public List<ElectricalPerDetailsViewModal> Details { get; set; } = new();
}
 
