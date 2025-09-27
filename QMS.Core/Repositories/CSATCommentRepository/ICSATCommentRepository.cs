using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.CSATCommentRepository
{
    public interface ICSATCommentRepository
    {
        Task<List<Csat_CommentViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<Csat_CommentViewModel?> GetByIdAsync(int id);
        Task<OperationResult> CreateAsync(CSAT_Comment entity, bool returnCreatedRecord = false);
        Task<OperationResult> UpdateAsync(CSAT_Comment entity, bool returnUpdatedRecord = false);
        Task<OperationResult> DeleteAsync(int id);

        //Task<bool> UpdateAttachmentAsync(int id, string fileName);
    }
}
