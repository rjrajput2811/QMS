using QMS.Core.Models;

namespace QMS.Core.Repositories.MergeTrackerRepository;

public interface IMergeTrackerRepository
{
    Task<MergeTrackerViewModel> GetSummaryOfAllActivityAsync(DateTime fromDate, DateTime toDate);
}
