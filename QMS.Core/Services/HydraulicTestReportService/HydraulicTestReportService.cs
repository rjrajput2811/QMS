using QMS.Core.Models;
using QMS.Core.Repositories.HydraulicTestObservationRepository;
using QMS.Core.Repositories.HydraulicTestRepository;

namespace QMS.Core.Services.HydraulicTestReportService;

public class HydraulicTestReportService : IHydraulicTestReportService
{
    private readonly IHydraulicTestRepository _hydraulicTestRepository;
    private readonly IHydraulicTestObservationRepository _hydraulicTestObservationRepository;

    public HydraulicTestReportService(IHydraulicTestRepository hydraulicTestRepository,
                                      IHydraulicTestObservationRepository hydraulicTestObservationRepository)
    {
        _hydraulicTestRepository = hydraulicTestRepository;
        _hydraulicTestObservationRepository = hydraulicTestObservationRepository;
    }

    public async Task<List<HydraulicTestReportViewModel>> GetHydraulicTestReportListAsync()
    {
        return await _hydraulicTestRepository.GetHydraulicTestReportAsync();
    }

    public async Task<HydraulicTestReportViewModel> GetHydraulicTestReportDetailsAsync(int Id)
    {
        var hydraulicTestReport = await _hydraulicTestRepository.GetHydraulicTestReportDetailsAsync(Id);
        hydraulicTestReport.Observations = await _hydraulicTestObservationRepository.GetHydraulicTestObservationDetailsAsync(hydraulicTestReport.Id);
        return hydraulicTestReport;
    }

    public async Task<OperationResult> InsertHydraulicTestReportAsync(HydraulicTestReportViewModel model)
    {
        var result = new OperationResult();
        var newhydraulicTestReportId = await _hydraulicTestRepository.InsertHydraulicTestReportAsync(model);
        if (newhydraulicTestReportId == 0)
        {
            result.Success = false;
            result.Message = "An unexpected error occured. please refresh the page and try again. If this error persist. Please Contact administrator.";
            return result;
        }

        foreach (var ob in model.Observations)
        {
            ob.HydraulicTestReport_Id = newhydraulicTestReportId;
            result = await _hydraulicTestObservationRepository.InsertHydraulicTestObservationAsync(ob);
            if (!result.Success) { return result; }
        }

        return result;
    }

    public async Task<OperationResult> UpdateHydraulicTestReportAsync(HydraulicTestReportViewModel model)
    {
        var result = await _hydraulicTestRepository.UpdateHydraulicTestReportAsync(model);
        if (!result.Success) { return result; }

        result = await _hydraulicTestObservationRepository.DeleteHydraulicTestObservationAsync(model.Id);
        if (!result.Success) { return result; }

        foreach (var ob in model.Observations)
        {
            ob.HydraulicTestReport_Id = model.Id;
            result = await _hydraulicTestObservationRepository.InsertHydraulicTestObservationAsync(ob);
            if (!result.Success) { return result; }
        }

        return result;
    }

    public async Task<OperationResult> DeleteHydraulicTestReportAsync(int Id)
    {
        var result = await _hydraulicTestRepository.DeleteHydraulicTestReportAsync(Id);
        if (!result.Success) { return result; }

        result = await _hydraulicTestObservationRepository.DeleteHydraulicTestObservationAsync(Id);
        if (!result.Success) { return result; }

        return result;
    }
}
