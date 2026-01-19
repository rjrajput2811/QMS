using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using QMS.Core.Models;
using QMS.Core.DatabaseContext;
using QMS.Core.Services.SystemLogs;
using QMS.Core.Repositories.Shared;
using QMS.Core.Repositories.SixSigmaIndicesRepo;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace QMS.Core.Repositories.SixSigmaIndicesRepository
{
    public class SixSigmaIndicesRepository : SqlTableRepository, ISixSigmaIndicesRepository
    {
        private new readonly QMSDbContext _dbContext;
        private readonly ISystemLogService _systemLogService;

        public SixSigmaIndicesRepository(QMSDbContext dbContext, ISystemLogService systemLogService)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _systemLogService = systemLogService;
        }

        public async Task<OperationResult> DeleteSixSigmaIndicesAsync(int id)
        {
            try
            {
                var result = await base.DeleteAsync<SixSigmaIndices>(id);
                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }


        public async Task<List<SixSigmaIndicesViewModel>> GetSixSigmaIndicesAsync()
        {
            try
            {

                var result = await _dbContext.SixSigmaIndices
                    .FromSqlRaw("EXEC sp_Get_Six_Sigma_Indices")
                    .ToListAsync();


                return result.Select(x => new SixSigmaIndicesViewModel
                {
                    Id = x.Id,
                    Deleted = x.Deleted,
                    Fy = x.Fy,
                    Pc = x.Pc,
                    Location = x.Location,

                    Lean_Data_Pc_No_People = x.Lean_Data_Pc_No_People,
                    Lean_Data_Pc_Total_People = x.Lean_Data_Pc_Total_People,
                    Lean_Target = x.Lean_Target,
                    Lean_Weightage = x.Lean_Weightage,
                    Lean_Formula = x.Lean_Formula,
                    Lean_Weighted_Score = x.Lean_Weighted_Score,

                    Six_Sigma_Data_Pc_Proj = x.Six_Sigma_Data_Pc_Proj,
                    Six_Sigma_Data_Pc_Ann = x.Six_Sigma_Data_Pc_Ann,
                    Six_Sigma_Target = x.Six_Sigma_Target,
                    Six_Sigma__Weightage = x.Six_Sigma__Weightage,
                    Six_Sigma_Formula = x.Six_Sigma_Formula,
                    Six_Sigma_Weighted_Score = x.Six_Sigma_Weighted_Score,

                    Review_Data_Pc_Proj_Review = x.Review_Data_Pc_Proj_Review,
                    Review_Data_Pc_Proj_Running = x.Review_Data_Pc_Proj_Running,
                    Review_Target = x.Review_Target,
                    Review_Weightage = x.Review_Weightage,
                    Review_Formula = x.Review_Formula,
                    Review_Weighted_Score = x.Review_Weighted_Score,

                    Market_Data_Pc_Cap_Cso = x.Market_Data_Pc_Cap_Cso,
                    Market_Data_Pc_Tar_Cso = x.Market_Data_Pc_Tar_Cso,
                    Market_Target = x.Market_Target,
                    Market_Weightage = x.Market_Weightage,
                    Market_Formula = x.Market_Formula,
                    Market_Weighted_Score = x.Market_Weighted_Score,

                    Bkpt_Data_Pc_Ytd = x.Bkpt_Data_Pc_Ytd,
                    Bkpt_Data_Pc_Ann_Ytd = x.Bkpt_Data_Pc_Ann_Ytd,
                    Bkpt_Data_Pc_Mfg = x.Bkpt_Data_Pc_Mfg,
                    Bkpt_Data_Pc_Ann_Mfg = x.Bkpt_Data_Pc_Ann_Mfg,
                    Bkpt_Target_Ytd = x.Bkpt_Target_Ytd,
                    Bkpt_Weightage_Ytd = x.Bkpt_Weightage_Ytd,
                    Bkpt_Formula_Ytd = x.Bkpt_Formula_Ytd,
                    Bkpt_Weighted_Score_Ytd = x.Bkpt_Weighted_Score_Ytd,
                    Bkpt_Target_Mfg = x.Bkpt_Target_Mfg,
                    Bkpt_Weightage_Mfg = x.Bkpt_Weightage_Mfg,
                    Bkpt_Formula_Mfg = x.Bkpt_Formula_Mfg,
                    Bkpt_Weighted_Score_Mfg = x.Bkpt_Weighted_Score_Mfg,

                    Eng_Total_Weighted_Score = x.Eng_Total_Weighted_Score,
                    Eng_Total_Sum = x.Eng_Total_Sum,

                    Cust_Data_Pc_Ot = x.Cust_Data_Pc_Ot,
                    Cust_Data_Pc_As = x.Cust_Data_Pc_As,
                    Cust_Data_Pc_Close = x.Cust_Data_Pc_Close,
                    Cust_Data_Pc_Total = x.Cust_Data_Pc_Total,
                    Cust_Target_Tgt = x.Cust_Target_Tgt,
                    Cust_Weightage_Tgt = x.Cust_Weightage_Tgt,
                    Cust_Formula_Tgt = x.Cust_Formula_Tgt,
                    Cust_Weighted_Score_Tgt = x.Cust_Weighted_Score_Tgt,
                    Cust_Target_Ytd = x.Cust_Target_Ytd,
                    Cust_Weightage_Ytd = x.Cust_Weightage_Ytd,
                    Cust_Formula_Ytd = x.Cust_Formula_Ytd,
                    Cust_Weighted_Score_Ytd = x.Cust_Weighted_Score_Ytd,

                    Proc_Data_Pc_Npd = x.Proc_Data_Pc_Npd,
                    Proc_Data_Pc_Ann = x.Proc_Data_Pc_Ann,
                    Proc_Target = x.Proc_Target,
                    Proc_Weightage = x.Proc_Weightage,
                    Proc_Formula = x.Proc_Formula,
                    Proc_Weighted_Score = x.Proc_Weighted_Score,

                    Key_Data_Pc_Cust = x.Key_Data_Pc_Cust,
                    Key_Data_Pc_Ann = x.Key_Data_Pc_Ann,
                    Key_Target = x.Key_Target,
                    Key_Weightage = x.Key_Weightage,
                    Key_Formula = x.Key_Formula,
                    Key_Weighted_Score = x.Key_Weighted_Score,

                    Save_Data_Pc_Actu = x.Save_Data_Pc_Actu,
                    Save_Data_Pc_Ann = x.Save_Data_Pc_Ann,
                    Save_Target = x.Save_Target,
                    Save_Weightage = x.Save_Weightage,
                    Save_Formula = x.Save_Formula,
                    Save_Weighted_Score = x.Save_Weighted_Score,

                    Out_Data_Pc_Sigma = x.Out_Data_Pc_Sigma,
                    Out_Data_Pc_As = x.Out_Data_Pc_As,
                    Out_Target = x.Out_Target,
                    Out_Weightage = x.Out_Weightage,
                    Out_Formula = x.Out_Formula,
                    Out_Weighted_Score = x.Out_Weighted_Score,

                    Spm_Data_Pc_Spm = x.Spm_Data_Pc_Spm,
                    Spm_Data_Pc_As = x.Spm_Data_Pc_As,
                    Spm_Target = x.Spm_Target,
                    Spm_Weightage = x.Spm_Weightage,
                    Spm_Formula = x.Spm_Formula,
                    Spm_Weighted_Score = x.Spm_Weighted_Score,

                    Proj_Data_Pc_Close = x.Proj_Data_Pc_Close,
                    Proj_Data_Pc_Dmaic = x.Proj_Data_Pc_Dmaic,
                    Proj_Data_Pc_Turbo = x.Proj_Data_Pc_Turbo,
                    Proj_Data_Pc_Planned = x.Proj_Data_Pc_Planned,

                    Proj_Num_Target = x.Proj_Num_Target,
                    Proj_Num_Weightage = x.Proj_Num_Weightage,
                    Proj_Num_Formula = x.Proj_Num_Formula,
                    Proj_Num_Weighted_Score = x.Proj_Num_Weighted_Score,
                    Proj_No_Weighted_Score = x.Proj_No_Weighted_Score,
                    Proj_No_Formula = x.Proj_No_Formula,
                    Proj_No_Weightage = x.Proj_No_Weightage,
                    Proj_No_Target = x.Proj_No_Target,


                    Eff_Total_Weighted_Score = x.Eff_Total_Weighted_Score,
                    Eff_Total_Sum = x.Eff_Total_Sum,

                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedDate = x.UpdatedDate
                }).ToList();
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }


        public async Task<SixSigmaIndicesViewModel?> GetSixSigmaIndicesByIdAsync(int id)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@Six_Sig_EngId", id)
        };

                var sql = @"sp_Get_Six_Sigma_Indices_ById @Six_Sig_EngId";

                var result = await Task.Run(() => _dbContext.SixSigmaIndices
                    .FromSqlRaw(sql, parameters)
                    .AsEnumerable()
                    .Select(x => new SixSigmaIndicesViewModel
                    {
                        Id = x.Id,
                        Deleted = x.Deleted,
                        Fy = x.Fy,
                        Pc = x.Pc,
                        Location = x.Location,

                        Lean_Data_Pc_No_People = x.Lean_Data_Pc_No_People,
                        Lean_Data_Pc_Total_People = x.Lean_Data_Pc_Total_People,
                        Lean_Target = x.Lean_Target,
                        Lean_Weightage = x.Lean_Weightage,
                        Lean_Formula = x.Lean_Formula,
                        Lean_Weighted_Score = x.Lean_Weighted_Score,

                        Six_Sigma_Data_Pc_Proj = x.Six_Sigma_Data_Pc_Proj,
                        Six_Sigma_Data_Pc_Ann = x.Six_Sigma_Data_Pc_Ann,
                        Six_Sigma_Target = x.Six_Sigma_Target,
                        Six_Sigma__Weightage = x.Six_Sigma__Weightage,
                        Six_Sigma_Formula = x.Six_Sigma_Formula,
                        Six_Sigma_Weighted_Score = x.Six_Sigma_Weighted_Score,

                        Review_Data_Pc_Proj_Review = x.Review_Data_Pc_Proj_Review,
                        Review_Data_Pc_Proj_Running = x.Review_Data_Pc_Proj_Running,
                        Review_Target = x.Review_Target,
                        Review_Weightage = x.Review_Weightage,
                        Review_Formula = x.Review_Formula,
                        Review_Weighted_Score = x.Review_Weighted_Score,

                        Market_Data_Pc_Cap_Cso = x.Market_Data_Pc_Cap_Cso,
                        Market_Data_Pc_Tar_Cso = x.Market_Data_Pc_Tar_Cso,
                        Market_Target = x.Market_Target,
                        Market_Weightage = x.Market_Weightage,
                        Market_Formula = x.Market_Formula,
                        Market_Weighted_Score = x.Market_Weighted_Score,

                        Bkpt_Data_Pc_Ytd = x.Bkpt_Data_Pc_Ytd,
                        Bkpt_Data_Pc_Ann_Ytd = x.Bkpt_Data_Pc_Ann_Ytd,
                        Bkpt_Data_Pc_Mfg = x.Bkpt_Data_Pc_Mfg,
                        Bkpt_Data_Pc_Ann_Mfg = x.Bkpt_Data_Pc_Ann_Mfg,
                        Bkpt_Target_Ytd = x.Bkpt_Target_Ytd,
                        Bkpt_Weightage_Ytd = x.Bkpt_Weightage_Ytd,
                        Bkpt_Formula_Ytd = x.Bkpt_Formula_Ytd,
                        Bkpt_Weighted_Score_Ytd = x.Bkpt_Weighted_Score_Ytd,
                        Bkpt_Target_Mfg = x.Bkpt_Target_Mfg,
                        Bkpt_Weightage_Mfg = x.Bkpt_Weightage_Mfg,
                        Bkpt_Formula_Mfg = x.Bkpt_Formula_Mfg,
                        Bkpt_Weighted_Score_Mfg = x.Bkpt_Weighted_Score_Mfg,

                        Eng_Total_Weighted_Score = x.Eng_Total_Weighted_Score,
                        Eng_Total_Sum = x.Eng_Total_Sum,

                        Cust_Data_Pc_Ot = x.Cust_Data_Pc_Ot,
                        Cust_Data_Pc_As = x.Cust_Data_Pc_As,
                        Cust_Data_Pc_Close = x.Cust_Data_Pc_Close,
                        Cust_Data_Pc_Total = x.Cust_Data_Pc_Total,

                        Cust_Target_Tgt = x.Cust_Target_Tgt,
                        Cust_Weightage_Tgt = x.Cust_Weightage_Tgt,
                        Cust_Formula_Tgt = x.Cust_Formula_Tgt,
                        Cust_Weighted_Score_Tgt = x.Cust_Weighted_Score_Tgt,

                        Cust_Target_Ytd = x.Cust_Target_Ytd,
                        Cust_Weightage_Ytd = x.Cust_Weightage_Ytd,
                        Cust_Formula_Ytd = x.Cust_Formula_Ytd,
                        Cust_Weighted_Score_Ytd = x.Cust_Weighted_Score_Ytd,

                        Proc_Data_Pc_Npd = x.Proc_Data_Pc_Npd,
                        Proc_Data_Pc_Ann = x.Proc_Data_Pc_Ann,
                        Proc_Target = x.Proc_Target,
                        Proc_Weightage = x.Proc_Weightage,
                        Proc_Formula = x.Proc_Formula,
                        Proc_Weighted_Score = x.Proc_Weighted_Score,

                        Key_Data_Pc_Cust = x.Key_Data_Pc_Cust,
                        Key_Data_Pc_Ann = x.Key_Data_Pc_Ann,
                        Key_Target = x.Key_Target,
                        Key_Weightage = x.Key_Weightage,
                        Key_Formula = x.Key_Formula,
                        Key_Weighted_Score = x.Key_Weighted_Score,

                        Save_Data_Pc_Actu = x.Save_Data_Pc_Actu,
                        Save_Data_Pc_Ann = x.Save_Data_Pc_Ann,
                        Save_Target = x.Save_Target,
                        Save_Weightage = x.Save_Weightage,
                        Save_Formula = x.Save_Formula,
                        Save_Weighted_Score = x.Save_Weighted_Score,

                        Out_Data_Pc_Sigma = x.Out_Data_Pc_Sigma,
                        Out_Data_Pc_As = x.Out_Data_Pc_As,
                        Out_Target = x.Out_Target,
                        Out_Weightage = x.Out_Weightage,
                        Out_Formula = x.Out_Formula,
                        Out_Weighted_Score = x.Out_Weighted_Score,

                        Spm_Data_Pc_Spm = x.Spm_Data_Pc_Spm,
                        Spm_Data_Pc_As = x.Spm_Data_Pc_As,
                        Spm_Target = x.Spm_Target,
                        Spm_Weightage = x.Spm_Weightage,
                        Spm_Formula = x.Spm_Formula,
                        Spm_Weighted_Score = x.Spm_Weighted_Score,

                        Proj_Data_Pc_Close = x.Proj_Data_Pc_Close,
                        Proj_Data_Pc_Dmaic = x.Proj_Data_Pc_Dmaic,
                        Proj_Data_Pc_Turbo = x.Proj_Data_Pc_Turbo,
                        Proj_Data_Pc_Planned = x.Proj_Data_Pc_Planned,

                        Proj_Num_Target = x.Proj_Num_Target,
                        Proj_Num_Weightage = x.Proj_Num_Weightage,
                        Proj_Num_Formula = x.Proj_Num_Formula,
                        Proj_Num_Weighted_Score = x.Proj_Num_Weighted_Score,
                        Proj_No_Weighted_Score = x.Proj_No_Weighted_Score,
                        Proj_No_Formula = x.Proj_No_Formula,
                        Proj_No_Weightage = x.Proj_No_Weightage,
                        Proj_No_Target = x.Proj_No_Target,

                        Eff_Total_Weighted_Score = x.Eff_Total_Weighted_Score,
                        Eff_Total_Sum = x.Eff_Total_Sum
                    })
                    .FirstOrDefault());

                return result;
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> InsertSixSigmaIndicesAsync(SixSigmaIndicesViewModel model)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = model.Deleted },

                    new SqlParameter("@Fy", (object?)model.Fy ?? DBNull.Value),
                    new SqlParameter("@Pc", (object?)model.Pc ?? DBNull.Value),
                    new SqlParameter("@Location", (object?)model.Location ?? DBNull.Value),

                    new SqlParameter("@Lean_Data_Pc_No_People", (object?)model.Lean_Data_Pc_No_People ?? DBNull.Value),
                    new SqlParameter("@Lean_Data_Pc_Total_People", (object?)model.Lean_Data_Pc_Total_People ?? DBNull.Value),
                    new SqlParameter("@Lean_Target", (object?)model.Lean_Target ?? DBNull.Value),
                    new SqlParameter("@Lean_Weightage", (object?)model.Lean_Weightage ?? DBNull.Value),
                    new SqlParameter("@Lean_Formula", (object?)model.Lean_Formula ?? DBNull.Value),
                    new SqlParameter("@Lean_Weighted_Score", (object?)model.Lean_Weighted_Score ?? DBNull.Value),

                    new SqlParameter("@Six_Sigma_Data_Pc_Proj", (object?)model.Six_Sigma_Data_Pc_Proj ?? DBNull.Value),
                    new SqlParameter("@Six_Sigma_Data_Pc_Ann", (object?)model.Six_Sigma_Data_Pc_Ann ?? DBNull.Value),
                    new SqlParameter("@Six_Sigma_Target", (object?)model.Six_Sigma_Target ?? DBNull.Value),
                    new SqlParameter("@Six_Sigma__Weightage", (object?)model.Six_Sigma__Weightage ?? DBNull.Value),
                    new SqlParameter("@Six_Sigma_Formula", (object?)model.Six_Sigma_Formula ?? DBNull.Value),
                    new SqlParameter("@Six_Sigma_Weighted_Score", (object?)model.Six_Sigma_Weighted_Score ?? DBNull.Value),


                    new SqlParameter("@Review_Data_Pc_Proj_Review", (object?)model.Review_Data_Pc_Proj_Review ?? DBNull.Value),
                    new SqlParameter("@Review_Data_Pc_Proj_Running", (object?)model.Review_Data_Pc_Proj_Running ?? DBNull.Value),
                    new SqlParameter("@Review_Target", (object?)model.Review_Target ?? DBNull.Value),
                    new SqlParameter("@Review_Weightage", (object?)model.Review_Weightage ?? DBNull.Value),
                    new SqlParameter("@Review_Formula", (object?)model.Review_Formula ?? DBNull.Value),
                    new SqlParameter("@Review_Weighted_Score", (object?)model.Review_Weighted_Score ?? DBNull.Value),


                    new SqlParameter("@Market_Data_Pc_Cap_Cso", (object?)model.Market_Data_Pc_Cap_Cso ?? DBNull.Value),
                    new SqlParameter("@Market_Data_Pc_Tar_Cso", (object?)model.Market_Data_Pc_Tar_Cso ?? DBNull.Value),
                    new SqlParameter("@Market_Target", (object?)model.Market_Target ?? DBNull.Value),
                    new SqlParameter("@Market_Weightage", (object?)model.Market_Weightage ?? DBNull.Value),
                    new SqlParameter("@Market_Formula", (object?)model.Market_Formula ?? DBNull.Value),
                    new SqlParameter("@Market_Weighted_Score", (object?)model.Market_Weighted_Score ?? DBNull.Value),

                    new SqlParameter("@Bkpt_Data_Pc_Ytd", (object?)model.Bkpt_Data_Pc_Ytd ?? DBNull.Value),
                    new SqlParameter("@Bkpt_Data_Pc_Ann_Ytd", (object?)model.Bkpt_Data_Pc_Ann_Ytd ?? DBNull.Value),
                    new SqlParameter("@Bkpt_Data_Pc_Mfg", (object?)model.Bkpt_Data_Pc_Mfg ?? DBNull.Value),
                    new SqlParameter("@Bkpt_Data_Pc_Ann_Mfg", (object?)model.Bkpt_Data_Pc_Ann_Mfg ?? DBNull.Value),
                    new SqlParameter("@Bkpt_Target_Ytd", (object?)model.Bkpt_Target_Ytd ?? DBNull.Value),
                    new SqlParameter("@Bkpt_Weightage_Ytd", (object?)model.Bkpt_Weightage_Ytd ?? DBNull.Value),
                    new SqlParameter("@Bkpt_Formula_Ytd", (object?)model.Bkpt_Formula_Ytd ?? DBNull.Value),
                    new SqlParameter("@Bkpt_Weighted_Score_Ytd", (object?)model.Bkpt_Weighted_Score_Ytd ?? DBNull.Value),
                    new SqlParameter("@Bkpt_Target_Mfg", (object?)model.Bkpt_Target_Mfg ?? DBNull.Value),
                    new SqlParameter("@Bkpt_Weightage_Mfg", (object?)model.Bkpt_Weightage_Mfg ?? DBNull.Value),
                    new SqlParameter("@Bkpt_Formula_Mfg", (object?)model.Bkpt_Formula_Mfg ?? DBNull.Value),
                    new SqlParameter("@Bkpt_Weighted_Score_Mfg", (object?)model.Bkpt_Weighted_Score_Mfg ?? DBNull.Value),

                    new SqlParameter("@Eng_Total_Weighted_Score", (object?)model.Eng_Total_Weighted_Score ?? DBNull.Value),
                    new SqlParameter("@Eng_Total_Sum", (object?)model.Eng_Total_Sum ?? DBNull.Value),


                    new SqlParameter("@Cust_Data_Pc_Ot", (object?)model.Cust_Data_Pc_Ot ?? DBNull.Value),
                    new SqlParameter("@Cust_Data_Pc_As", (object?)model.Cust_Data_Pc_As ?? DBNull.Value),
                    new SqlParameter("@Cust_Data_Pc_Close", (object?)model.Cust_Data_Pc_Close ?? DBNull.Value),
                    new SqlParameter("@Cust_Data_Pc_Total", (object?)model.Cust_Data_Pc_Total ?? DBNull.Value),

                    new SqlParameter("@Cust_Target_Tgt", (object?)model.Cust_Target_Tgt ?? DBNull.Value),
                    new SqlParameter("@Cust_Weightage_Tgt", (object?)model.Cust_Weightage_Tgt ?? DBNull.Value),
                    new SqlParameter("@Cust_Formula_Tgt", (object?)model.Cust_Formula_Tgt ?? DBNull.Value),
                    new SqlParameter("@Cust_Weighted_Score_Tgt", (object?)model.Cust_Weighted_Score_Tgt ?? DBNull.Value),

                    new SqlParameter("@Cust_Target_Ytd", (object?)model.Cust_Target_Ytd ?? DBNull.Value),
                    new SqlParameter("@Cust_Weightage_Ytd", (object?)model.Cust_Weightage_Ytd ?? DBNull.Value),
                    new SqlParameter("@Cust_Formula_Ytd", (object?)model.Cust_Formula_Ytd ?? DBNull.Value),
                    new SqlParameter("@Cust_Weighted_Score_Ytd", (object?)model.Cust_Weighted_Score_Ytd ?? DBNull.Value),


                    new SqlParameter("@Proc_Data_Pc_Npd", (object?)model.Proc_Data_Pc_Npd ?? DBNull.Value),
                    new SqlParameter("@Proc_Data_Pc_Ann", (object?)model.Proc_Data_Pc_Ann ?? DBNull.Value),
                    new SqlParameter("@Proc_Target", (object?)model.Proc_Target ?? DBNull.Value),
                    new SqlParameter("@Proc_Weightage", (object?)model.Proc_Weightage ?? DBNull.Value),
                    new SqlParameter("@Proc_Formula", (object?)model.Proc_Formula ?? DBNull.Value),
                    new SqlParameter("@Proc_Weighted_Score", (object?)model.Proc_Weighted_Score ?? DBNull.Value),


                    new SqlParameter("@Key_Data_Pc_Cust", (object?)model.Key_Data_Pc_Cust ?? DBNull.Value),
                    new SqlParameter("@Key_Data_Pc_Ann", (object?)model.Key_Data_Pc_Ann ?? DBNull.Value),
                    new SqlParameter("@Key_Target", (object?)model.Key_Target ?? DBNull.Value),
                    new SqlParameter("@Key_Weightage", (object?)model.Key_Weightage ?? DBNull.Value),
                    new SqlParameter("@Key_Formula", (object?)model.Key_Formula ?? DBNull.Value),
                    new SqlParameter("@Key_Weighted_Score", (object?)model.Key_Weighted_Score ?? DBNull.Value),

                    new SqlParameter("@Save_Data_Pc_Actu", (object?)model.Save_Data_Pc_Actu ?? DBNull.Value),
                    new SqlParameter("@Save_Data_Pc_Ann", (object?)model.Save_Data_Pc_Ann ?? DBNull.Value),
                    new SqlParameter("@Save_Target", (object?)model.Save_Target ?? DBNull.Value),
                    new SqlParameter("@Save_Weightage", (object?)model.Save_Weightage ?? DBNull.Value),
                    new SqlParameter("@Save_Formula", (object?)model.Save_Formula ?? DBNull.Value),
                    new SqlParameter("@Save_Weighted_Score", (object?)model.Save_Weighted_Score ?? DBNull.Value),


                    new SqlParameter("@Out_Data_Pc_Sigma", (object?)model.Out_Data_Pc_Sigma ?? DBNull.Value),
                    new SqlParameter("@Out_Data_Pc_As", (object?)model.Out_Data_Pc_As ?? DBNull.Value),
                    new SqlParameter("@Out_Target", (object?)model.Out_Target ?? DBNull.Value),
                    new SqlParameter("@Out_Weightage", (object?)model.Out_Weightage ?? DBNull.Value),
                    new SqlParameter("@Out_Formula", (object?)model.Out_Formula ?? DBNull.Value),
                    new SqlParameter("@Out_Weighted_Score", (object?)model.Out_Weighted_Score ?? DBNull.Value),


                    new SqlParameter("@Spm_Data_Pc_Spm", (object?)model.Spm_Data_Pc_Spm ?? DBNull.Value),
                    new SqlParameter("@Spm_Data_Pc_As", (object?)model.Spm_Data_Pc_As ?? DBNull.Value),
                    new SqlParameter("@Spm_Target", (object?)model.Spm_Target ?? DBNull.Value),
                    new SqlParameter("@Spm_Weightage", (object?)model.Spm_Weightage ?? DBNull.Value),
                    new SqlParameter("@Spm_Formula", (object?)model.Spm_Formula ?? DBNull.Value),
                    new SqlParameter("@Spm_Weighted_Score", (object?)model.Spm_Weighted_Score ?? DBNull.Value),

                    new SqlParameter("@Proj_Data_Pc_Close", (object?)model.Proj_Data_Pc_Close ?? DBNull.Value),
                    new SqlParameter("@Proj_Data_Pc_Dmaic", (object?)model.Proj_Data_Pc_Dmaic ?? DBNull.Value),
                    new SqlParameter("@Proj_Data_Pc_Turbo", (object?)model.Proj_Data_Pc_Turbo ?? DBNull.Value),
                    new SqlParameter("@Proj_Data_Pc_Planned", (object?)model.Proj_Data_Pc_Planned ?? DBNull.Value),

                    new SqlParameter("@Eff_Total_Weighted_Score", (object?)model.Eff_Total_Weighted_Score ?? DBNull.Value),
                    new SqlParameter("@Eff_Total_Sum", (object?)model.Eff_Total_Sum ?? DBNull.Value),

                    new SqlParameter("@Proj_Num_Target", (object?)model.Proj_Data_Pc_Close ?? DBNull.Value),
                    new SqlParameter("@Proj_Num_Weightage", (object?)model.Proj_Data_Pc_Dmaic ?? DBNull.Value),
                    new SqlParameter("@Proj_Num_Formula", (object?)model.Proj_Data_Pc_Turbo ?? DBNull.Value),
                    new SqlParameter("@Proj_Num_Weighted_Score",(object?)model.Proj_Data_Pc_Planned ?? DBNull.Value),

                    new SqlParameter("@Proj_No_Weighted_Score", (object?)model.Proj_Data_Pc_Close ?? DBNull.Value),
                    new SqlParameter("@Proj_No_Formula", (object?)model.Proj_Data_Pc_Dmaic ?? DBNull.Value),
                    new SqlParameter("@Proj_No_Weightage", (object?)model.Proj_Data_Pc_Turbo ?? DBNull.Value),
                    new SqlParameter("@Proj_No_Target", (object?)model.Proj_Data_Pc_Planned ?? DBNull.Value),


                    new SqlParameter("@CreatedBy", (object?)model.CreatedBy ?? DBNull.Value),

                    // OUTPUT PARAM
                    new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC [dbo].[sp_Insert_Six_Sigma_Indices] " +
                    "@IsDeleted, @Fy, @Pc, @Location, " +
                    "@Lean_Data_Pc_No_People, @Lean_Data_Pc_Total_People, @Lean_Target, @Lean_Weightage, @Lean_Formula, @Lean_Weighted_Score, " +
                    "@Six_Sigma_Data_Pc_Proj, @Six_Sigma_Data_Pc_Ann, @Six_Sigma_Target, @Six_Sigma__Weightage, @Six_Sigma_Formula, @Six_Sigma_Weighted_Score, " +
                    "@Review_Data_Pc_Proj_Review, @Review_Data_Pc_Proj_Running, @Review_Target, @Review_Weightage, @Review_Formula, @Review_Weighted_Score, " +
                    "@Market_Data_Pc_Cap_Cso, @Market_Data_Pc_Tar_Cso, @Market_Target, @Market_Weightage, @Market_Formula, @Market_Weighted_Score, " +
                    "@Bkpt_Data_Pc_Ytd, @Bkpt_Data_Pc_Ann_Ytd, @Bkpt_Data_Pc_Mfg, @Bkpt_Data_Pc_Ann_Mfg, @Bkpt_Target_Ytd, @Bkpt_Weightage_Ytd, @Bkpt_Formula_Ytd, @Bkpt_Weighted_Score_Ytd, @Bkpt_Target_Mfg, @Bkpt_Weightage_Mfg, @Bkpt_Formula_Mfg, @Bkpt_Weighted_Score_Mfg, " +
                    "@Eng_Total_Weighted_Score, @Eng_Total_Sum, " +
                    "@Cust_Data_Pc_Ot, @Cust_Data_Pc_As, @Cust_Data_Pc_Close, @Cust_Data_Pc_Total, " +
                    "@Cust_Target_Tgt, @Cust_Weightage_Tgt, @Cust_Formula_Tgt, @Cust_Weighted_Score_Tgt, " +
                    "@Cust_Target_Ytd, @Cust_Weightage_Ytd, @Cust_Formula_Ytd, @Cust_Weighted_Score_Ytd, " +
                    "@Proc_Data_Pc_Npd, @Proc_Data_Pc_Ann, @Proc_Target, @Proc_Weightage, @Proc_Formula, @Proc_Weighted_Score, " +
                    "@Key_Data_Pc_Cust, @Key_Data_Pc_Ann, @Key_Target, @Key_Weightage, @Key_Formula, @Key_Weighted_Score, " +
                    "@Save_Data_Pc_Actu, @Save_Data_Pc_Ann, @Save_Target, @Save_Weightage, @Save_Formula, @Save_Weighted_Score, " +
                    "@Out_Data_Pc_Sigma, @Out_Data_Pc_As, @Out_Target, @Out_Weightage, @Out_Formula, @Out_Weighted_Score, " +
                    "@Spm_Data_Pc_Spm, @Spm_Data_Pc_As, @Spm_Target, @Spm_Weightage, @Spm_Formula, @Spm_Weighted_Score, " +
                    "@Proj_Data_Pc_Close, @Proj_Data_Pc_Dmaic, @Proj_Data_Pc_Turbo, @Proj_Data_Pc_Planned, " +
                    "@Eff_Total_Weighted_Score, @Eff_Total_Sum,@Proj_Num_Target,@Proj_Num_Weightage,@Proj_Num_Formula,@Proj_Num_Weighted_Score," +
                    "@Proj_No_Weighted_Score,@Proj_No_Formula,@Proj_No_Weightage,@Proj_No_Target," +
                    "@CreatedBy, @NewId OUTPUT",
                    parameters
                );


                return new OperationResult
                {
                    Success = true,
                    Message = "Six Sigma Indices record inserted successfully."
                };
            }
            catch (Exception ex)
            {
                _systemLogService.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<OperationResult> UpdateSixSigmaIndicesAsync(SixSigmaIndicesViewModel model)
        {
            try
            {
                // small helper to convert null → DBNull.Value
                object DbValue(object? value) => value ?? DBNull.Value;

                var parameters = new[]
                {
            new SqlParameter("@Six_Sig_EngId", model.Id),
            new SqlParameter("@IsDeleted", model.Deleted),

            new SqlParameter("@Fy", DbValue(model.Fy)),
            new SqlParameter("@Pc", DbValue(model.Pc)),
            new SqlParameter("@Location", DbValue(model.Location)),

            new SqlParameter("@Lean_Data_Pc_No_People", DbValue(model.Lean_Data_Pc_No_People)),
            new SqlParameter("@Lean_Data_Pc_Total_People", DbValue(model.Lean_Data_Pc_Total_People)),
            new SqlParameter("@Lean_Target", DbValue(model.Lean_Target)),
            new SqlParameter("@Lean_Weightage", DbValue(model.Lean_Weightage)),
            new SqlParameter("@Lean_Formula", DbValue(model.Lean_Formula)),
            new SqlParameter("@Lean_Weighted_Score", DbValue(model.Lean_Weighted_Score)),

            new SqlParameter("@Six_Sigma_Data_Pc_Proj", DbValue(model.Six_Sigma_Data_Pc_Proj)),
            new SqlParameter("@Six_Sigma_Data_Pc_Ann", DbValue(model.Six_Sigma_Data_Pc_Ann)),
            new SqlParameter("@Six_Sigma_Target", DbValue(model.Six_Sigma_Target)),
            new SqlParameter("@Six_Sigma__Weightage", DbValue(model.Six_Sigma__Weightage)),
            new SqlParameter("@Six_Sigma_Formula", DbValue(model.Six_Sigma_Formula)),
            new SqlParameter("@Six_Sigma_Weighted_Score", DbValue(model.Six_Sigma_Weighted_Score)),

            new SqlParameter("@Review_Data_Pc_Proj_Review", DbValue(model.Review_Data_Pc_Proj_Review)),
            new SqlParameter("@Review_Data_Pc_Proj_Running", DbValue(model.Review_Data_Pc_Proj_Running)),
            new SqlParameter("@Review_Target", DbValue(model.Review_Target)),
            new SqlParameter("@Review_Weightage", DbValue(model.Review_Weightage)),
            new SqlParameter("@Review_Formula", DbValue(model.Review_Formula)),
            new SqlParameter("@Review_Weighted_Score", DbValue(model.Review_Weighted_Score)),

            new SqlParameter("@Market_Data_Pc_Cap_Cso", DbValue(model.Market_Data_Pc_Cap_Cso)),
            new SqlParameter("@Market_Data_Pc_Tar_Cso", DbValue(model.Market_Data_Pc_Tar_Cso)),
            new SqlParameter("@Market_Target", DbValue(model.Market_Target)),
            new SqlParameter("@Market_Weightage", DbValue(model.Market_Weightage)),
            new SqlParameter("@Market_Formula", DbValue(model.Market_Formula)),
            new SqlParameter("@Market_Weighted_Score", DbValue(model.Market_Weighted_Score)),

            new SqlParameter("@Bkpt_Data_Pc_Ytd", DbValue(model.Bkpt_Data_Pc_Ytd)),
            new SqlParameter("@Bkpt_Data_Pc_Ann_Ytd", DbValue(model.Bkpt_Data_Pc_Ann_Ytd)),
            new SqlParameter("@Bkpt_Data_Pc_Mfg", DbValue(model.Bkpt_Data_Pc_Mfg)),
            new SqlParameter("@Bkpt_Data_Pc_Ann_Mfg", DbValue(model.Bkpt_Data_Pc_Ann_Mfg)),

            new SqlParameter("@Bkpt_Target_Ytd", DbValue(model.Bkpt_Target_Ytd)),
            new SqlParameter("@Bkpt_Weightage_Ytd", DbValue(model.Bkpt_Weightage_Ytd)),
            new SqlParameter("@Bkpt_Formula_Ytd", DbValue(model.Bkpt_Formula_Ytd)),
            new SqlParameter("@Bkpt_Weighted_Score_Ytd", DbValue(model.Bkpt_Weighted_Score_Ytd)),

            new SqlParameter("@Bkpt_Target_Mfg", DbValue(model.Bkpt_Target_Mfg)),
            new SqlParameter("@Bkpt_Weightage_Mfg", DbValue(model.Bkpt_Weightage_Mfg)),
            new SqlParameter("@Bkpt_Formula_Mfg", DbValue(model.Bkpt_Formula_Mfg)),
            new SqlParameter("@Bkpt_Weighted_Score_Mfg", DbValue(model.Bkpt_Weighted_Score_Mfg)),

            new SqlParameter("@Eng_Total_Weighted_Score", DbValue(model.Eng_Total_Weighted_Score)),
            new SqlParameter("@Eng_Total_Sum", DbValue(model.Eng_Total_Sum)),

            new SqlParameter("@Cust_Data_Pc_Ot", DbValue(model.Cust_Data_Pc_Ot)),
            new SqlParameter("@Cust_Data_Pc_As", DbValue(model.Cust_Data_Pc_As)),
            new SqlParameter("@Cust_Data_Pc_Close", DbValue(model.Cust_Data_Pc_Close)),
            new SqlParameter("@Cust_Data_Pc_Total", DbValue(model.Cust_Data_Pc_Total)),

            new SqlParameter("@Cust_Target_Tgt", DbValue(model.Cust_Target_Tgt)),
            new SqlParameter("@Cust_Weightage_Tgt", DbValue(model.Cust_Weightage_Tgt)),
            new SqlParameter("@Cust_Formula_Tgt", DbValue(model.Cust_Formula_Tgt)),
            new SqlParameter("@Cust_Weighted_Score_Tgt", DbValue(model.Cust_Weighted_Score_Tgt)),

            new SqlParameter("@Cust_Target_Ytd", DbValue(model.Cust_Target_Ytd)),
            new SqlParameter("@Cust_Weightage_Ytd", DbValue(model.Cust_Weightage_Ytd)),
            new SqlParameter("@Cust_Formula_Ytd", DbValue(model.Cust_Formula_Ytd)),
            new SqlParameter("@Cust_Weighted_Score_Ytd", DbValue(model.Cust_Weighted_Score_Ytd)),

            new SqlParameter("@Proc_Data_Pc_Npd", DbValue(model.Proc_Data_Pc_Npd)),
            new SqlParameter("@Proc_Data_Pc_Ann", DbValue(model.Proc_Data_Pc_Ann)),
            new SqlParameter("@Proc_Target", DbValue(model.Proc_Target)),
            new SqlParameter("@Proc_Weightage", DbValue(model.Proc_Weightage)),
            new SqlParameter("@Proc_Formula", DbValue(model.Proc_Formula)),
            new SqlParameter("@Proc_Weighted_Score", DbValue(model.Proc_Weighted_Score)),

            new SqlParameter("@Key_Data_Pc_Cust", DbValue(model.Key_Data_Pc_Cust)),
            new SqlParameter("@Key_Data_Pc_Ann", DbValue(model.Key_Data_Pc_Ann)),
            new SqlParameter("@Key_Target", DbValue(model.Key_Target)),
            new SqlParameter("@Key_Weightage", DbValue(model.Key_Weightage)),
            new SqlParameter("@Key_Formula", DbValue(model.Key_Formula)),
            new SqlParameter("@Key_Weighted_Score", DbValue(model.Key_Weighted_Score)),

            new SqlParameter("@Save_Data_Pc_Actu", DbValue(model.Save_Data_Pc_Actu)),
            new SqlParameter("@Save_Data_Pc_Ann", DbValue(model.Save_Data_Pc_Ann)),
            new SqlParameter("@Save_Target", DbValue(model.Save_Target)),
            new SqlParameter("@Save_Weightage", DbValue(model.Save_Weightage)),
            new SqlParameter("@Save_Formula", DbValue(model.Save_Formula)),
            new SqlParameter("@Save_Weighted_Score", DbValue(model.Save_Weighted_Score)),

            new SqlParameter("@Out_Data_Pc_Sigma", DbValue(model.Out_Data_Pc_Sigma)),
            new SqlParameter("@Out_Data_Pc_As", DbValue(model.Out_Data_Pc_As)),
            new SqlParameter("@Out_Target", DbValue(model.Out_Target)),
            new SqlParameter("@Out_Weightage", DbValue(model.Out_Weightage)),
            new SqlParameter("@Out_Formula", DbValue(model.Out_Formula)),
            new SqlParameter("@Out_Weighted_Score", DbValue(model.Out_Weighted_Score)),

            new SqlParameter("@Spm_Data_Pc_Spm", DbValue(model.Spm_Data_Pc_Spm)),
            new SqlParameter("@Spm_Data_Pc_As", DbValue(model.Spm_Data_Pc_As)),
            new SqlParameter("@Spm_Target", DbValue(model.Spm_Target)),
            new SqlParameter("@Spm_Weightage", DbValue(model.Spm_Weightage)),
            new SqlParameter("@Spm_Formula", DbValue(model.Spm_Formula)),
            new SqlParameter("@Spm_Weighted_Score", DbValue(model.Spm_Weighted_Score)),

            new SqlParameter("@Proj_Data_Pc_Close", DbValue(model.Proj_Data_Pc_Close)),
            new SqlParameter("@Proj_Data_Pc_Dmaic", DbValue(model.Proj_Data_Pc_Dmaic)),
            new SqlParameter("@Proj_Data_Pc_Turbo", DbValue(model.Proj_Data_Pc_Turbo)),
            new SqlParameter("@Proj_Data_Pc_Planned", DbValue(model.Proj_Data_Pc_Planned)),

            new SqlParameter("@Eff_Total_Weighted_Score", DbValue(model.Eff_Total_Weighted_Score)),
            new SqlParameter("@Eff_Total_Sum", DbValue(model.Eff_Total_Sum)),

            new SqlParameter("@Proj_Num_Target", (object?)model.Proj_Data_Pc_Close ?? DBNull.Value),
            new SqlParameter("@Proj_Num_Weightage", (object?)model.Proj_Data_Pc_Dmaic ?? DBNull.Value),
            new SqlParameter("@Proj_Num_Formula", (object?)model.Proj_Data_Pc_Turbo ?? DBNull.Value),
            new SqlParameter("@Proj_Num_Weighted_Score",(object?)model.Proj_Data_Pc_Planned ?? DBNull.Value),

            new SqlParameter("@Proj_No_Weighted_Score", (object?)model.Proj_Data_Pc_Close ?? DBNull.Value),
            new SqlParameter("@Proj_No_Formula", (object?)model.Proj_Data_Pc_Dmaic ?? DBNull.Value),
            new SqlParameter("@Proj_No_Weightage", (object?)model.Proj_Data_Pc_Turbo ?? DBNull.Value),
            new SqlParameter("@Proj_No_Target", (object?)model.Proj_Data_Pc_Planned ?? DBNull.Value),

            // fixed: this should be model.UpdatedBy, not model
            new SqlParameter("@UpdatedBy", DbValue(model.UpdatedBy))
        };

                var sql = "EXEC sp_Update_Six_Sigma_Indices " +
                          string.Join(", ", parameters.Select(p => p.ParameterName));

                await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

                return new OperationResult { Success = true, Message = "Record updated successfully." };
            }
            catch (Exception ex)
            {
                // you can also log ex.ToString() if you want stack trace
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public byte[] Build(string templatePath, SixSigmaIndicesViewModel m, out string fileName)
        {
            using var wb = new XLWorkbook(templatePath);
            var wsEng = wb.Worksheet("Eng");
            var wsEff = wb.Worksheet("Eff");

            wsEng.Cell("A1").Value =
                $"Six Sigma Indices Report - Segment : Engagement , FY : {m.Fy} , PC : {m.Pc} , Location : {m.Location}";

            wsEff.Cell("A1").Value =
                $"Six Sigma Indices Report - Segment : Effectiveness , FY : {m.Fy} , PC : {m.Pc} , Location : {m.Location}";

            // TODO: FillEng/FillEff mapping (same as before)
            FillEng(wsEng, m);
            FillEff(wsEff, m);

            using var ms = new MemoryStream();
            wb.SaveAs(ms);

            fileName = $"SixSigmaIndices_{m.Fy}_{m.Pc}_Id_{m.Id}.xlsx";
            return ms.ToArray();
        }

        private static void SetNum(IXLWorksheet ws, int row, int col, double? val)
        {
            if (val.HasValue) ws.Cell(row, col).Value = val.Value;
            else ws.Cell(row, col).Clear(XLClearOptions.Contents);
        }

        private static void SetText(IXLWorksheet ws, int row, int col, string? val)
        {
            if (!string.IsNullOrWhiteSpace(val)) ws.Cell(row, col).Value = val;
            else ws.Cell(row, col).Clear(XLClearOptions.Contents);
        }

        // IMPORTANT: Adjust row numbers to match your template exactly.
        // Columns assumption: C=Data, D=Target, E=Weightage, F=FormulaText
        private static void FillEng(IXLWorksheet ws, SixSigmaIndicesViewModel m)
        {
            const int ROW_LEAN = 5;
            const int ROW_LEAN_TWO = 6;
            const int ROW_SIXSIGMA = 8;
            const int ROW_SIXSIGMA_TWO = 9;
            const int ROW_REVIEW = 14;
            const int ROW_REVIEW_TWO = 15;
            const int ROW_MARKET = 17;
            const int ROW_MARKET_TWO = 18;
            const int ROW_BKPT_YTD = 20;
            const int ROW_BKPT_YTD_TWO = 21;
            const int ROW_BKPT_MFG = 22;
            const int ROW_BKPT_MFG_TWO = 23;

            SetNum(ws, ROW_LEAN, 3, m.Lean_Data_Pc_No_People);
            SetNum(ws, ROW_LEAN_TWO, 3, m.Lean_Data_Pc_Total_People);
            SetNum(ws, ROW_LEAN, 4, m.Lean_Target);
            SetNum(ws, ROW_LEAN, 5, m.Lean_Weightage);
            SetText(ws, ROW_LEAN, 6, m.Lean_Formula);
            SetText(ws, ROW_LEAN, 7, m.Lean_Weighted_Score.ToString());

            SetNum(ws, ROW_SIXSIGMA, 3, m.Six_Sigma_Data_Pc_Proj);
            SetNum(ws, ROW_SIXSIGMA_TWO, 3, m.Six_Sigma_Data_Pc_Ann);
            SetNum(ws, ROW_SIXSIGMA, 4, m.Six_Sigma_Target);
            SetNum(ws, ROW_SIXSIGMA, 5, m.Six_Sigma__Weightage);
            SetText(ws, ROW_SIXSIGMA, 6, m.Six_Sigma_Formula);

            SetNum(ws, ROW_REVIEW, 3, m.Review_Data_Pc_Proj_Review);
            SetNum(ws, ROW_REVIEW_TWO, 3, m.Review_Data_Pc_Proj_Running);
            SetNum(ws, ROW_REVIEW, 4, m.Review_Target);
            SetNum(ws, ROW_REVIEW, 5, m.Review_Weightage);
            SetText(ws, ROW_REVIEW, 6, m.Review_Formula);

            SetNum(ws, ROW_MARKET, 3, m.Market_Data_Pc_Cap_Cso);
            SetNum(ws, ROW_MARKET_TWO, 3, m.Market_Data_Pc_Tar_Cso);
            SetNum(ws, ROW_MARKET, 4, m.Market_Target);
            SetNum(ws, ROW_MARKET, 5, m.Market_Weightage);
            SetText(ws, ROW_MARKET, 6, m.Market_Formula);

            SetNum(ws, ROW_BKPT_YTD, 3, m.Bkpt_Data_Pc_Ytd);
            SetNum(ws, ROW_BKPT_YTD_TWO, 3, m.Bkpt_Data_Pc_Ann_Ytd);
            SetNum(ws, ROW_BKPT_YTD, 4, m.Bkpt_Target_Ytd);
            SetNum(ws, ROW_BKPT_YTD, 5, m.Bkpt_Weightage_Ytd);
            SetText(ws, ROW_BKPT_YTD, 6, m.Bkpt_Formula_Ytd);

            SetNum(ws, ROW_BKPT_MFG, 3, m.Bkpt_Data_Pc_Mfg);
            SetNum(ws, ROW_BKPT_MFG_TWO, 3, m.Bkpt_Data_Pc_Ann_Mfg);
            SetNum(ws, ROW_BKPT_MFG, 4, m.Bkpt_Target_Mfg);
            SetNum(ws, ROW_BKPT_MFG, 5, m.Bkpt_Weightage_Mfg);
            SetText(ws, ROW_BKPT_MFG, 6, m.Bkpt_Formula_Mfg);
        }

        private static void FillEff(IXLWorksheet ws, SixSigmaIndicesViewModel m)
        {
            const int ROW_CUST_TGT = 5;
            const int ROW_CUST_TGT_TWO = 6;
            const int ROW_CUST_YTD = 7;
            const int ROW_CUST_YTD_TWO = 8;
            const int ROW_PROC = 10;
            const int ROW_PROC_TWO = 11;
            const int ROW_KEY = 13;
            const int ROW_KEY_TWO = 14;
            const int ROW_SAVE = 16;
            const int ROW_SAVE_TWO = 17;
            const int ROW_OUT = 19;
            const int ROW_OUT_TWO = 20;
            const int ROW_SPM = 22;
            const int ROW_SPM_TWO = 23;
            const int ROW_PROJ = 28;
            const int ROW_PROJ_TWO = 29;
            const int ROW_PROJ_NO = 30;
            const int ROW_PROJ_NO_TWO = 31;

            SetNum(ws, ROW_CUST_TGT, 3, m.Cust_Data_Pc_Ot);
            SetNum(ws, ROW_CUST_TGT_TWO, 3, m.Cust_Data_Pc_As);
            SetNum(ws, ROW_CUST_TGT, 4, m.Cust_Target_Tgt);
            SetNum(ws, ROW_CUST_TGT, 5, m.Cust_Weightage_Tgt);
            SetText(ws, ROW_CUST_TGT, 6, m.Cust_Formula_Tgt);

            SetNum(ws, ROW_CUST_YTD, 3, m.Cust_Data_Pc_Close);
            SetNum(ws, ROW_CUST_YTD, 3, m.Cust_Data_Pc_Total);
            SetNum(ws, ROW_CUST_YTD, 4, m.Cust_Target_Ytd);
            SetNum(ws, ROW_CUST_YTD, 5, m.Cust_Weightage_Ytd);
            SetText(ws, ROW_CUST_YTD, 6, m.Cust_Formula_Ytd);

            SetNum(ws, ROW_PROC, 3, m.Proc_Data_Pc_Npd);
            SetNum(ws, ROW_PROC_TWO, 3, m.Proc_Data_Pc_Ann);
            SetNum(ws, ROW_PROC, 4, m.Proc_Target);
            SetNum(ws, ROW_PROC, 5, m.Proc_Weightage);
            SetText(ws, ROW_PROC, 6, m.Proc_Formula);

            SetNum(ws, ROW_KEY, 3, m.Key_Data_Pc_Cust);
            SetNum(ws, ROW_KEY_TWO, 3, m.Key_Data_Pc_Ann);
            SetNum(ws, ROW_KEY, 4, m.Key_Target);
            SetNum(ws, ROW_KEY, 5, m.Key_Weightage);
            SetText(ws, ROW_KEY, 6, m.Key_Formula);

            SetNum(ws, ROW_SAVE, 3, m.Save_Data_Pc_Actu);
            SetNum(ws, ROW_SAVE_TWO, 3, m.Save_Data_Pc_Ann);
            SetNum(ws, ROW_SAVE, 4, m.Save_Target);
            SetNum(ws, ROW_SAVE, 5, m.Save_Weightage);
            SetText(ws, ROW_SAVE, 6, m.Save_Formula);

            SetNum(ws, ROW_OUT, 3, m.Out_Data_Pc_Sigma);
            SetNum(ws, ROW_OUT_TWO, 3, m.Out_Data_Pc_As);
            SetNum(ws, ROW_OUT, 4, m.Out_Target);
            SetNum(ws, ROW_OUT, 5, m.Out_Weightage);
            SetText(ws, ROW_OUT, 6, m.Out_Formula);

            SetNum(ws, ROW_SPM, 3, m.Spm_Data_Pc_Spm);
            SetNum(ws, ROW_SPM_TWO, 3, m.Spm_Data_Pc_As);
            SetNum(ws, ROW_SPM, 4, m.Spm_Target);
            SetNum(ws, ROW_SPM, 5, m.Spm_Weightage);
            SetText(ws, ROW_SPM, 6, m.Spm_Formula);

            SetNum(ws, ROW_PROJ, 3, m.Proj_Data_Pc_Close);
            SetNum(ws, ROW_PROJ_TWO, 3, m.Proj_Data_Pc_Dmaic);
            SetNum(ws, ROW_PROJ, 4, m.proj);
            SetNum(ws, ROW_PROJ, 5, m.Proj_Data_Pc_Close);
            SetNum(ws, ROW_PROJ, 6, m.Proj_Data_Pc_Close);
        }


    }
}
