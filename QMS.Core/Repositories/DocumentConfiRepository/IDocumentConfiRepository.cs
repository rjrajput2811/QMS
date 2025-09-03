using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.DocumentConfiRepository
{
    public interface IDocumentConfiRepository
    {
        Task<List<DocumentDetViewModel>> GetDocDetailAsync();
        Task<DocumentDetail?> GetDocDetailByIdAsync(int Id);
        Task<DocumentDetail?> GetDocDetailByTypeAsync(string type);
        Task<OperationResult> CreateDocDetailAsync(DocumentDetViewModel newDocDetailRecord, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateDocDetailAsync(DocumentDetViewModel updateDocDetailRecord, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteDocDetailAsync(int Id);
        Task<bool> CheckDocDetailDuplicate(string searchText, string type, int Id);
    }
}
