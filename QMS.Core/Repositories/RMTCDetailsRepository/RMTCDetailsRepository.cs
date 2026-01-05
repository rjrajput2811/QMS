using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.Shared;
using QMS.Core.Services.SystemLogs;

namespace QMS.Core.Repositories.RMTCDetailsRepository
{
    public class RMTCDetailsRepository : SqlTableRepository, IRMTCDetailsRepository
    {
        private readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public RMTCDetailsRepository(QMSDbContext dbContext, ISystemLogService systemLogService)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<List<RM_TCViewModel>> GetListAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var result = await _dbContext.RM_TC_Tracker
                    .FromSqlRaw("EXEC sp_Get_RM_TC_Tracker")
                    .ToListAsync();

                // Apply optional date filtering based on RMTCDate
                if (startDate.HasValue && endDate.HasValue)
                {
                    result = result
                        .Where(x => x.Date.HasValue &&
                                    x.Date.Value.Date >= startDate.Value.Date &&
                                    x.Date.Value.Date <= endDate.Value.Date)
                        .ToList();
                }

                return result.Select(data => new RM_TCViewModel
                {
                    Id = data.Id,
                    Vendor = data.Vendor,
                    Product_No = data.Product_No,
                    ProdDesc = data.ProdDesc,
                    Date = data.Date,
                    Housing_Body = data.Housing_Body,
                    Wires_Cable = data.Wires_Cable,
                    Diffuser_Lens = data.Diffuser_Lens,
                    Pcb = data.Pcb,
                    Connectors = data.Connectors,
                    Powder_Coat = data.Powder_Coat,
                    Led_LM80 = data.Led_LM80,
                    Led_Purchase_Proof = data.Led_Purchase_Proof,
                    Driver = data.Driver,
                    Pre_Treatment = data.Pre_Treatment,
                    Hardware = data.Hardware,
                    Other_Critical_Items = data.Other_Critical_Items,
                    Attchment = data.Attchment,
                    Remarks = data.Remarks,
                    Housing_Body_Attch = data.Housing_Body_Attch,
                    Wires_Cable_Attch = data.Wires_Cable_Attch,
                    Diffuser_Lens_Attch = data.Diffuser_Lens_Attch,
                    Pcb_Attch = data.Pcb_Attch,
                    Connectors_Attch = data.Connectors_Attch,
                    Powder_Coat_Attch = data.Powder_Coat_Attch,
                    Led_LM80_Attch = data.Led_LM80_Attch,
                    Led_Purchase_Proof_Attch = data.Led_Purchase_Proof_Attch,
                    Driver_Attch = data.Driver_Attch,
                    Pre_Treatment_Attch = data.Pre_Treatment_Attch,
                    Hardware_Attch = data.Hardware_Attch,
                    CreatedDate = data.CreatedDate,
                    UpdatedDate = data.UpdatedDate,
                    CreatedBy = data.CreatedBy,
                    UpdatedBy = data.UpdatedBy
                }).ToList();
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<RM_TCViewModel?> GetByIdAsync(int id)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Rm_Tc_Id", id),
                };

                var sql = @"EXEC sp_Get_RM_TC_Tracker_ById @Rm_Tc_Id";

                var result = await _dbContext.RM_TC_Tracker.FromSqlRaw(sql, parameters).ToListAsync();

                var viewModelList = result.Select(data => new RM_TCViewModel
                {
                    Id = data.Id,
                    Vendor = data.Vendor,
                    Product_No = data.Product_No,
                    ProdDesc = data.ProdDesc,
                    Date = data.Date,
                    Housing_Body = data.Housing_Body,
                    Wires_Cable = data.Wires_Cable,
                    Diffuser_Lens = data.Diffuser_Lens,
                    Pcb = data.Pcb,
                    Connectors = data.Connectors,
                    Powder_Coat = data.Powder_Coat,
                    Led_LM80 = data.Led_LM80,
                    Led_Purchase_Proof = data.Led_Purchase_Proof,
                    Driver = data.Driver,
                    Pre_Treatment = data.Pre_Treatment,
                    Hardware = data.Hardware,
                    Other_Critical_Items = data.Other_Critical_Items,
                    Attchment = data.Attchment,
                    Remarks = data.Remarks,
                    Housing_Body_Attch = data.Housing_Body_Attch,
                    Wires_Cable_Attch = data.Wires_Cable_Attch,
                    Diffuser_Lens_Attch = data.Diffuser_Lens_Attch,
                    Pcb_Attch = data.Pcb_Attch,
                    Connectors_Attch = data.Connectors_Attch,
                    Powder_Coat_Attch = data.Powder_Coat_Attch,
                    Led_LM80_Attch = data.Led_LM80_Attch,
                    Led_Purchase_Proof_Attch = data.Led_Purchase_Proof_Attch,
                    Driver_Attch = data.Driver_Attch,
                    Pre_Treatment_Attch = data.Pre_Treatment_Attch,
                    Hardware_Attch = data.Hardware_Attch,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    UpdatedBy = data.UpdatedBy,
                    UpdatedDate = data.UpdatedDate

                }).ToList();

                return viewModelList.FirstOrDefault();

            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> CreateAsync(RM_TC_Tracker entity, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Vendor", entity.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Product_No", entity.Product_No ?? (object)DBNull.Value),
                    new SqlParameter("@ProdDesc", entity.ProdDesc ?? (object)DBNull.Value),
                    new SqlParameter("@Date", entity.Date ?? (object)DBNull.Value),
                    new SqlParameter("@Housing_Body", entity.Housing_Body ?? (object)DBNull.Value),
                    new SqlParameter("@Wires_Cable", entity.Wires_Cable ?? (object)DBNull.Value),
                    new SqlParameter("@Diffuser_Lens", entity.Diffuser_Lens ?? (object)DBNull.Value),
                    new SqlParameter("@Pcb", entity.Pcb ?? (object)DBNull.Value),
                    new SqlParameter("@Connectors", entity.Connectors ?? (object)DBNull.Value),
                    new SqlParameter("@Powder_Coat", entity.Powder_Coat ?? (object)DBNull.Value),
                    new SqlParameter("@Led_LM80", entity.Led_LM80 ?? (object)DBNull.Value),
                    new SqlParameter("@Led_Purchase_Proof", entity.Led_Purchase_Proof ?? (object)DBNull.Value),
                    new SqlParameter("@Driver", entity.Driver ?? (object)DBNull.Value),
                    new SqlParameter("@Pre_Treatment", entity.Pre_Treatment ?? (object)DBNull.Value),
                    new SqlParameter("@Hardware", entity.Hardware ?? (object)DBNull.Value),
                    new SqlParameter("@Other_Critical_Items", entity.Other_Critical_Items ?? (object)DBNull.Value),
                    new SqlParameter("@Attchment", entity.Attchment ?? (object)DBNull.Value),
                    new SqlParameter("@Remarks", entity.Remarks ?? (object)DBNull.Value),
                    new SqlParameter("@Housing_Body_Attch", entity.Housing_Body_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Wires_Cable_Attch", entity.Wires_Cable_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Diffuser_Lens_Attch", entity.Diffuser_Lens_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Pcb_Attch", entity.Pcb_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Connectors_Attch", entity.Connectors_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Powder_Coat_Attch", entity.Powder_Coat_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Led_LM80_Attch", entity.Led_LM80_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Led_Purchase_Proof_Attch", entity.Led_Purchase_Proof_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Driver_Attch", entity.Driver_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Pre_Treatment_Attch", entity.Pre_Treatment_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Hardware_Attch", entity.Hardware_Attch?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", entity.CreatedBy ?? (object)DBNull.Value)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Insert_RM_TCTracker " +
                    "@Vendor, @Product_No, @ProdDesc, @Date, @Housing_Body, @Wires_Cable, " +
                    "@Diffuser_Lens, @Pcb, @Connectors, @Powder_Coat, @Led_LM80, @Led_Purchase_Proof, " +
                    "@Driver, @Pre_Treatment, @Hardware, @Other_Critical_Items,@Attchment, @Remarks,@Housing_Body_Attch,@Wires_Cable_Attch, @Diffuser_Lens_Attch," +
                    "@Pcb_Attch, @Connectors_Attch, @Powder_Coat_Attch, @Led_LM80_Attch, @Led_Purchase_Proof_Attch, @Driver_Attch, @Pre_Treatment_Attch,@Hardware_Attch, @CreatedBy",
                    parameters
                );

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateAsync(RM_TC_Tracker entity, bool returnCreatedRecord = false)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@RMTCId", entity.Id),
                    new SqlParameter("@Vendor", entity.Vendor ?? (object)DBNull.Value),
                    new SqlParameter("@Product_No", entity.Product_No ?? (object)DBNull.Value),
                    new SqlParameter("@ProdDesc", entity.ProdDesc ?? (object)DBNull.Value),
                    new SqlParameter("@Date", entity.Date ?? (object)DBNull.Value),
                    new SqlParameter("@Housing_Body", entity.Housing_Body ?? (object)DBNull.Value),
                    new SqlParameter("@Wires_Cable", entity.Wires_Cable ?? (object)DBNull.Value),
                    new SqlParameter("@Diffuser_Lens", entity.Diffuser_Lens ?? (object)DBNull.Value),
                    new SqlParameter("@Pcb", entity.Pcb ?? (object)DBNull.Value),
                    new SqlParameter("@Connectors", entity.Connectors ?? (object)DBNull.Value),
                    new SqlParameter("@Powder_Coat", entity.Powder_Coat ?? (object)DBNull.Value),
                    new SqlParameter("@Led_LM80", entity.Led_LM80 ?? (object)DBNull.Value),
                    new SqlParameter("@Led_Purchase_Proof", entity.Led_Purchase_Proof ?? (object)DBNull.Value),
                    new SqlParameter("@Driver", entity.Driver ?? (object)DBNull.Value),
                    new SqlParameter("@Pre_Treatment", entity.Pre_Treatment ?? (object)DBNull.Value),
                    new SqlParameter("@Hardware", entity.Hardware ?? (object)DBNull.Value),
                    new SqlParameter("@Other_Critical_Items", entity.Other_Critical_Items ?? (object)DBNull.Value),
                    new SqlParameter("@Attchment", entity.Attchment ?? (object)DBNull.Value),
                    new SqlParameter("@Remarks", entity.Remarks ?? (object)DBNull.Value),
                    new SqlParameter("@Housing_Body_Attch", entity.Housing_Body_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Wires_Cable_Attch", entity.Wires_Cable_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Diffuser_Lens_Attch", entity.Diffuser_Lens_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Pcb_Attch", entity.Pcb_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Connectors_Attch", entity.Connectors_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Powder_Coat_Attch", entity.Powder_Coat_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Led_LM80_Attch", entity.Led_LM80_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Led_Purchase_Proof_Attch", entity.Led_Purchase_Proof_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Driver_Attch", entity.Driver_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Pre_Treatment_Attch", entity.Pre_Treatment_Attch?? (object)DBNull.Value),
                    new SqlParameter("@Hardware_Attch", entity.Hardware_Attch?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", entity.UpdatedBy ?? (object)DBNull.Value)
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_Update_RM_TCTracker " +
                    "@RMTCId, @Vendor, @Product_No, @ProdDesc, @Date, @Housing_Body, @Wires_Cable, " +
                    "@Diffuser_Lens, @Pcb, @Connectors, @Powder_Coat, @Led_LM80, @Led_Purchase_Proof, " +
                    "@Driver, @Pre_Treatment, @Hardware, @Other_Critical_Items,@Attchment, @Remarks,@Housing_Body_Attch,@Wires_Cable_Attch,@Diffuser_Lens_Attch," +
                    "@Pcb_Attch, @Connectors_Attch, @Powder_Coat_Attch, @Led_LM80_Attch, @Led_Purchase_Proof_Attch, @Driver_Attch, @Pre_Treatment_Attch,@Hardware_Attch,@UpdatedBy",
                    parameters
                );

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                return await base.DeleteAsync<RM_TC_Tracker>(id);
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateAttachmentFieldAsync(int id, string field, string fileName, string updatedBy)
        {
            var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Housing_Body_Attch","Wires_Cable_Attch","Diffuser_Lens_Attch","Pcb_Attch","Connectors_Attch",
                "Powder_Coat_Attch","Led_LM80_Attch","Led_Purchase_Proof_Attch","Driver_Attch","Pre_Treatment_Attch",
                "Hardware_Attch","Attchment"
            };

            if (!allowed.Contains(field)) return false;

            var entity = await _dbContext.RM_TC_Tracker.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return false;

            var prop = entity.GetType().GetProperty(field);
            if (prop == null) return false;

            prop.SetValue(entity, fileName);
            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedBy = updatedBy;

            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}
