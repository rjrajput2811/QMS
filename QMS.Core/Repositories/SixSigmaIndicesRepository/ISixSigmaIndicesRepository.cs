using QMS.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.SixSigmaIndicesRepo
{
    public interface ISixSigmaIndicesRepository
    {
        Task<List<SixSigmaIndicesViewModel>> GetSixSigmaIndicesAsync();
        Task<SixSigmaIndicesViewModel?> GetSixSigmaIndicesByIdAsync(int id);
        Task<OperationResult> InsertSixSigmaIndicesAsync(SixSigmaIndicesViewModel model);
        Task<OperationResult> UpdateSixSigmaIndicesAsync(SixSigmaIndicesViewModel model);
        Task<OperationResult> DeleteSixSigmaIndicesAsync(int id);
        byte[] Build(string templatePath, SixSigmaIndicesViewModel m, out string fileName);
    }
}
