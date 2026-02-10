using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.GeneralObservationRepository
{
    public interface IGeneralObservationRepository
    {
        Task<List<GeneralObservationViewModel>> GetGeneralObservationAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<GeneralObservationViewModel?> GetGeneralObservationByIdAsync(int Id);
        Task<OperationResult> InsertGeneralObservationAsync(GeneralObservationReport model);
        Task<OperationResult> UpdateGeneralObservationAsync(GeneralObservationReport model);
        Task<OperationResult> DeleteGeneralObservationAsync(int Id);
        Task<bool> CheckDuplicate(string searchText, int Id);
    }
}
