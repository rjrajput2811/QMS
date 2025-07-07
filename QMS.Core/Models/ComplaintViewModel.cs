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
        public ComplaintViewModel Complaint { get; set; } = new ComplaintViewModel();
        public IndentDumpViewModel Indent { get; set; } = new IndentDumpViewModel();
        public PendingPoViewModel PO { get; set; } = new PendingPoViewModel();
        public InvoiceListViewModel Invoice { get; set; } = new InvoiceListViewModel();
    }
}
