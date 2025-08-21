using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.DocumentConfiRepository
{
    public class DocumentConfiRepository : SqlTableRepository, IDocumentConfiRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public DocumentConfiRepository(QMSDbContext dbContext,
                               ISystemLogService systemLogService) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<DocumentDetViewModel>> GetDocDetailAsync()
        {
            try
            {
                var result = await (from pr in _dbContext.Doc_Detail
                                    where pr.Deleted == false // Add this condition
                                    select new DocumentDetViewModel
                                    {
                                        Id = pr.Id,
                                        Document_No = pr.Document_No,
                                        Revision_No = pr.Revision_No,
                                        Effective_Date = pr.Effective_Date,
                                        Revision_Date = pr.Revision_Date,
                                        CreatedBy = pr.CreatedBy,
                                        CreatedDate = pr.CreatedDate,
                                        UpdatedBy = pr.UpdatedBy,
                                        UpdatedDate = pr.UpdatedDate
                                    }).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<DocumentDetail?> GetDocDetailByIdAsync(int Id)
        {
            try
            {
                var result = await base.GetByIdAsync<DocumentDetail>(Id);
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreateDocDetailAsync(DocumentDetViewModel newDocDetailRecord, bool returnCreatedRecord = false)
        {
            try
            {
                var docDetToCreate = new DocumentDetail();
                docDetToCreate.Document_No = newDocDetailRecord.Document_No;
                docDetToCreate.Revision_No = newDocDetailRecord.Revision_No;
                docDetToCreate.Effective_Date = newDocDetailRecord.Effective_Date;
                docDetToCreate.Revision_Date = newDocDetailRecord.Revision_Date;
                docDetToCreate.CreatedBy = newDocDetailRecord.CreatedBy;
                docDetToCreate.CreatedDate = DateTime.Now;
                return await base.CreateAsync<DocumentDetail>(docDetToCreate, returnCreatedRecord);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateDocDetailAsync(DocumentDetViewModel updateDocDetailRecord, bool returnUpdatedRecord = false)
        {
            try
            {
                var docDetToCreate = await base.GetByIdAsync<DocumentDetail>(updateDocDetailRecord.Id);
                docDetToCreate.Document_No = updateDocDetailRecord.Document_No;
                docDetToCreate.Revision_No = updateDocDetailRecord.Revision_No;
                docDetToCreate.Effective_Date = updateDocDetailRecord.Effective_Date;
                docDetToCreate.Revision_Date = updateDocDetailRecord.Revision_Date;
                docDetToCreate.UpdatedBy = updateDocDetailRecord.UpdatedBy;
                docDetToCreate.UpdatedDate = DateTime.Now;
                return await base.UpdateAsync<DocumentDetail>(docDetToCreate, returnUpdatedRecord);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> DeleteDocDetailAsync(int Id)
        {
            try
            {
                return await base.DeleteAsync<DocumentDetail>(Id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<bool> CheckDocDetailDuplicate(string searchText, string type, int Id)
        {
            try
            {
                bool existingflag = false;
                int? existingId = null;

                IQueryable<int> query = _dbContext.Doc_Detail
                    .Where(x => x.Deleted == false && x.Document_No == searchText && x.Type == type)
                    .Select(x => x.Id);

                // Add additional condition if Id is not 0
                if (Id != 0)
                {
                    query = _dbContext.Doc_Detail
                        .Where(x => x.Deleted == false && x.Document_No == searchText && x.Type == type && x.Id != Id)
                        .Select(x => x.Id);
                }


                existingId = await query.FirstOrDefaultAsync();

                if (existingId != null && existingId > 0)
                {
                    existingflag = true;
                }

                return existingflag;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }
    }
}
