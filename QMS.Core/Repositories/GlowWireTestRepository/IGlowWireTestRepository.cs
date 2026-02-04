using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.GlowWireTestRepository
{
    public interface IGlowWireTestRepository
    {
        Task<List<GlowWireTestReportViewModel>> GetGlowWireTestAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<GlowWireTestReportViewModel?> GetGlowWireTestByIdAsync(int Id);
        Task<OperationResult> InsertGlowWireTestAsync(GlowWireTestReport model);
        Task<OperationResult> UpdateGlowWireTestAsync(GlowWireTestReport model);
        Task<OperationResult> DeleteGlowWireTestAsync(int Id);
        Task<bool> CheckDuplicate(string searchText, int Id);
    }
}
