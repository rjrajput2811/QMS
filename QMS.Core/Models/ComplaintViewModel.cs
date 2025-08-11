using System;

namespace QMS.Core.Models
{
    public class ComplaintViewModel
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }

        public string? CCN_No { get; set; }
        public DateTime? CCCNDate { get; set; }
        public string? ReportedBy { get; set; }
        public string? CLocation { get; set; }
        public string? CustName { get; set; }
        public string? DealerName { get; set; }
        public string? CDescription { get; set; }
        public string? CStatus { get; set; }
        public DateTime? Completion { get; set; }
        public string? Remarks { get; set; }

        public int? TotalDays_Close { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class FinalMergeServiceViewModel
    {
        public string? CCN_No { get; set; }
        public DateTime? CCCNDate { get; set; }
        public string? ReportedBy { get; set; }
        public string? CLocation { get; set; }
        public string? CustName { get; set; }
        public string? DealerName { get; set; }
        public string? CDescription { get; set; }
        public string? CStatus { get; set; }
        public DateTime? Completion { get; set; }
        public string? Remarks { get; set; }
        public int? TotalDays_Close { get; set; }
        public string? Final_Status { get; set; }
        public DateTime? Custome { get; set; }
        public int? Open_Lead_Time { get; set; }
        public int? Final_Lead_Time { get; set; }
        public string? Range { get; set; }
        public string? Indent_No { get; set; }
        public DateTime? Indent_Date { get; set; }
        public string? Ind_CCN_No { get; set; }
        public string? Material_No { get; set; }
        public string? Item_Description { get; set; }
        public string? WiproCatelog_No { get; set; }
        public int? Quantity { get; set; }
        public string? Key { get; set; }
        public int? Inv_Qty { get; set; }
        public int? Bal_Qty { get; set; }
        public string? Pc { get; set; }
        public string? Fy { get; set; }

        public string? Vendor { get; set; }
        public string? PONo { get; set; }
        public DateTime? PODate { get; set; }
        public string? BalanceQty { get; set; }
        public string? BalanceValue { get; set; }
        public string? Closure_Range { get; set; }
        public string? Region { get; set; }
        public string? Closure_Type { get; set; }
        public int? Indent_Lead_Time { get; set; }
        public int? Inv_Lead_Time { get; set; }
        public DateTime? Last_Inv_Date { get; set; }

    }
}
