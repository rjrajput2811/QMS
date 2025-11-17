using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.CSATSummaryRepository
{
    public interface ICSATSummaryRepository
    {
        Task<List<Csat_SummaryViewModel>> GetSummaryAsync(DateTime? startDate = null, DateTime? endDate = null);

        Task<Csat_SummaryViewModel?> GetByIdAsync(int id);
        Task<OperationResult> CreateAsync(CSAT_Summary entity, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(CSAT_Summary entity, bool returnCreatedRecord = false);
    }
}
