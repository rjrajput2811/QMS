using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Models;

public class MergeTrackerViewModel
{
    public ConsolidatedSummaryModel? Summary { get; set; }
    public List<FifoTrackerModel>? FifoDetails { get; set; }
    public List<CsatSummaryModel>? CsatDetails { get; set; }
}

public class ConsolidatedSummaryModel
{
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public string? FromMonth { get; set; }
    public string? ToMonth { get; set; }
    public int CATotalComplaintRegistered { get; set; }
    public int CATillDateOpenComplaint { get; set; }
    public int CATillDateClosedComplaint { get; set; }
    public int RCATotalComplaintRegistered { get; set; }
    public int RCATillDateOpenComplaint { get; set; }
    public int RCATillDateClosedComplaint { get; set; }
    public int TotalDeviationRaised { get; set; }
    public decimal TotalPayment { get; set; }
    public int TotalProject { get; set; }
    public int ModelInclusion { get; set; }
    public int SeriesAddition { get; set; }
    public int CCLUpdation { get; set; }
    public int CCLSeries { get; set; }
    public int OthersProject { get; set; }
    public int TotalTestingInitiated { get; set; }
    public int TestingCompleted { get; set; }
    public int TestingPendingToComplete { get; set; }
    public int TotalNumberOfTPI { get; set; }
    public decimal ProjectValue { get; set; }
    public int TotalChangeNoteRaised { get; set; }
    public int TotalValidation { get; set; }
    public int AsOnOpenValidation { get; set; }
    public int AsOnCloseValidation { get; set; }
}

public class FifoTrackerModel
{
    public string? Test { get; set; }
    public int Qty { get; set; }
    public int TestCompleted { get; set; }
    public int TestPending { get; set; }
}

public class CsatSummaryModel
{
    public string? FY { get; set; }
    public string? Quarter { get; set; }
    public float RequestSent { get; set; }
    public float ResponsesReceived { get; set; }
    public float Promoters { get; set; }
    public float Collection { get; set; }
    public float Detractors { get; set; }
    public float NPS { get; set; }
}
