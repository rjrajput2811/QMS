using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;

using Microsoft.AspNetCore.Mvc;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.ElectricalProtectionRepo;
using QMS.Core.Repositories.ProductValidationRepo;
using QMS.Core.Repositories.ElectricalPerformanceRepo;
using System.Drawing;
using System.Threading.Tasks;

namespace QMS.Controllers;

public class ProductValidationController : Controller
{
    private readonly IPhysicalCheckAndVisualInspectionRepository _physicalCheckAndVisualInspectionRepository;
    private readonly IElectricalPerformanceRepository _electricalPerformanceRepository;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IElectricalProtectionRepository _electricalProtectionRepository;

    public ProductValidationController(
        IPhysicalCheckAndVisualInspectionRepository physicalCheckAndVisualInspectionRepository,
        IElectricalProtectionRepository electricalProtectionRepository,
        IWebHostEnvironment hostEnvironment,
        IElectricalProtectionRepository electricalProtectionRepository1)
    {
        _physicalCheckAndVisualInspectionRepository = physicalCheckAndVisualInspectionRepository;
        _electricalProtectionRepository = electricalProtectionRepository;
        _hostEnvironment = hostEnvironment;
        _electricalProtectionRepository = electricalProtectionRepository;
    }


    public IActionResult Index()
    {
        return View();
    }

    #region PhysicalCheckAndVisualInspection

    public IActionResult PhysicalCheckAndVisualInspection()
    {
        return View();
    }

    public async Task<ActionResult> GetPhysicalCheckAndVisualInspectionListAsync()
    {
        var result = await _physicalCheckAndVisualInspectionRepository.GetPhysicalCheckAndVisualInspectionsAsync();
        return Json(result);
    }

    public IActionResult Electricalprotection()
    {
        return View();
    }
    public async Task<IActionResult> PhysicalCheckAndVisualInspectionDetails(int Id)
    {
        var model = new PhysicalCheckAndVisualInspectionViewModel();
        if (Id > 0)
        {
            model = await _physicalCheckAndVisualInspectionRepository.GetPhysicalCheckAndVisualInspectionsByIdAsync(Id);
        }
        else
        {
            model.Report_Date = DateTime.Now;
        }
        return View(model);
    }
    public async Task<IActionResult> ElectricalProtectionDetails(int Id)
    {
        var model = new ElectricalProtectionViewModel();

        if (Id > 0)
        {
            model = await _electricalProtectionRepository.GetElectricalProtectionByIdAsync(Id);

        }
        else
        {
            model.ReportDate = DateTime.Now;
        }

        return View(model);
    }

    public IActionResult ElectricalPerformance()
    {
        return View();
    }

    public async Task<IActionResult> ElectricalPerformanceDetails(int Id)
    {
        var model = new ElectricalPerformanceViewModel();

        if (Id > 0)
        {
            model = await _electricalPerformanceRepository.GetElectricalPerformancesByIdAsync(Id);

        }
        else
        {
            model.ReportDate = DateTime.Now;
        }

        return View(model);
    }

    public IActionResult RippleTestReport()
    {
        return View();
    }

    public IActionResult Rippletestreportdetails(int Id)
    {
        return View();
    }

    public async Task<ActionResult> InsertUpdatePhysicalCheckAndVisualInspectionDetailsAsync(PhysicalCheckAndVisualInspectionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }

        if (model.Id > 0)
        {
            model.UpdatedBy = HttpContext.Session.GetInt32("UserId");
            model.UpdatedOn = DateTime.Now;
            var result = await _physicalCheckAndVisualInspectionRepository.UpdatePhysicalCheckAndVisualInspectionsAsync(model);
            return Json(result);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var result = await _physicalCheckAndVisualInspectionRepository.InsertPhysicalCheckAndVisualInspectionsAsync(model);
            return Json(result);
        }
    }

    public async Task<ActionResult> DeletePhysicalCheckAndVisualInspectionAsync(int Id)
    {
        var result = await _physicalCheckAndVisualInspectionRepository.DeletePhysicalCheckAndVisualInspectionsAsync(Id);
        return Json(result);
    }
    public async Task<ActionResult> GetElectricalProtectionListAsync()
    {
        var result = await _electricalProtectionRepository.GetElectricalProtectionsAsync();
        return Json(result);
    }

    public async Task<ActionResult> GetElectricalProtectionDetailsAsync(int Id)
    {
        var result = await _electricalProtectionRepository.GetElectricalProtectionByIdAsync(Id);
        return Json(result);
    }
    [HttpPost]
    public async Task<ActionResult> InsertUpdateElectricalProtectionAsync(ElectricalProtectionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
            return Json(new { Success = false, Errors = errors });
        }

        string[] allowedExtensions = { ".png", ".jpg", ".jpeg" };


        string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ElectricProt_Att", model.ReportNo.ToString());
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);


        if (model.TestedByFile != null)
        {
            string fileName = Path.GetFileName(model.TestedByFile.FileName);
            string ext = Path.GetExtension(fileName).ToLower();

            if (!allowedExtensions.Contains(ext))
            {
                return Json(new { Success = false, Message = "Only .png, .jpg, .jpeg" });
            }


            model.TestedBySignature = fileName;

            string savePath = Path.Combine(folder, fileName);
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await model.TestedByFile.CopyToAsync(stream);
            }
        }


        if (model.VerifiedByFile != null)
        {
            string fileName = Path.GetFileName(model.VerifiedByFile.FileName);
            string ext = Path.GetExtension(fileName).ToLower();

            if (!allowedExtensions.Contains(ext))
            {
                return Json(new { Success = false, Message = "Only .png, .jpg, .jpeg" });
            }

            model.VerifiedBySignature = fileName;

            string savePath = Path.Combine(folder, fileName);
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await model.VerifiedByFile.CopyToAsync(stream);
            }
        }


        if (model.Id > 0)
        {
            model.UpdatedBy = HttpContext.Session.GetInt32("UserId");
            model.UpdatedOn = DateTime.Now;
            var result = await _electricalProtectionRepository.UpdateElectricalProtectionAsync(model);
            return Json(result);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var result = await _electricalProtectionRepository.InsertElectricalProtectionAsync(model);
            return Json(result);
        }
    }




    public async Task<ActionResult> DeleteElectricalProtectionAsync(int Id)
    {
        var result = await _electricalProtectionRepository.DeleteElectricalProtectionAsync(Id);
        return Json(result);
    }
    public async Task<ActionResult> ExportElectricalProtectionToExcelAsync(int Id)
    {
        try
        {
            var data = await _electricalProtectionRepository.GetElectricalProtectionByIdAsync(Id);

            const int COL_SL = 1;
            const int COL_PARAM = 2;
            const int COL_SPEC = 3;
            const int COL_S1 = 4;
            const int COL_S5 = 8;
            const int COL_RESULT = 9;

            // 1. Setup Workbook and Worksheet (using ClosedXML)
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Electrical Protection Report");

            // --- General Report Formatting ---
            worksheet.SheetView.ZoomScale = 82;
            worksheet.Style.Font.FontName = "Aptos Narrow";
            worksheet.Style.Font.FontSize = 12;
            // Setting fixed column widths as requested
            worksheet.Columns("A").Width = 14.80;
            worksheet.Columns("B").Width = 39.10;
            worksheet.Columns("C").Width = 28.40;
            worksheet.Columns("D").Width = 19.80;
            worksheet.Columns("E").Width = 20.20;
            worksheet.Columns("F").Width = 21.30;
            worksheet.Columns("G").Width = 26.10;
            worksheet.Columns("H").Width = 26.10;
            worksheet.Columns("I").Width = 22.80;

            // --- Header Generation (Rows 1-6) ---
            int currentRow = 1;


            const int COL_IMAGE = 9;
            const int COL_TITLE_END = COL_RESULT - 1;


            worksheet.Row(currentRow).Height = 73.20;
            var titleRange = worksheet.Range(currentRow, 1, currentRow, COL_TITLE_END);
            titleRange.Merge();
            titleRange.Value = "Electrical Protection, Safety Test & Reliability Test";
            titleRange.Style.Font.Bold = true;
            titleRange.Style.Font.FontSize = 22;
            titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            titleRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            titleRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium);



            var webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, "images", "wipro-logo.png");
            if (System.IO.File.Exists(imagePath))
            { var picture = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell(currentRow, COL_IMAGE), 45, 12).WithPlacement(XLPicturePlacement.Move).Scale(0.9); }

            var imageRange = worksheet.Range(currentRow, 9, currentRow, 9);
            imageRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Medium);
            imageRange.Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            imageRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium);
            imageRange.Style.Border.SetLeftBorder(XLBorderStyleValues.Thin); currentRow++;


            worksheet.Range(currentRow, 1, currentRow, 3).Merge();
            worksheet.Range(currentRow, 1, currentRow, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 1).GetRichText().AddText("Product Cat Ref : ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.ProductCatRef);

            worksheet.Range(currentRow, 4, currentRow, 6).Merge();
            worksheet.Range(currentRow, 4, currentRow, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 4).GetRichText().AddText("Product Description : ").SetBold();
            worksheet.Cell(currentRow, 4).GetRichText().AddText(data.ProductDescription);

            worksheet.Range(currentRow, 7, currentRow, 8).Merge();
            worksheet.Range(currentRow, 7, currentRow, 8).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 7).GetRichText().AddText("Report No. : ").SetBold();
            worksheet.Cell(currentRow, 7).GetRichText().AddText(data.ReportNo);

            currentRow++;

            // Row 2: Light Source, Driver, Date
            worksheet.Range(currentRow, 1, currentRow, 2).Merge();
            worksheet.Range(currentRow, 1, currentRow, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 1).GetRichText().AddText("Light Source Details : ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.LightSourceDetails);

            worksheet.Range(currentRow, 3, currentRow, 4).Merge();
            worksheet.Range(currentRow, 3, currentRow, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 3).GetRichText().AddText("Driver Details & Qty : ").SetBold();
            worksheet.Cell(currentRow, 3).GetRichText().AddText(data.DriverDetailsQty);

            worksheet.Range(currentRow, 5, currentRow, 6).Merge();
            worksheet.Range(currentRow, 5, currentRow, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 5).GetRichText().AddText("Date : ").SetBold();
            worksheet.Cell(currentRow, 5).GetRichText().AddText(data.ReportDate?.ToShortDateString() ?? "Not Available");

            currentRow++;

            // Row 3: PCB, LED, Batch
            worksheet.Range(currentRow, 1, currentRow, 2).Merge();
            worksheet.Range(currentRow, 1, currentRow, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 1).GetRichText().AddText("PCB Details & Qty : ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.PCBDetailsQty);

            worksheet.Range(currentRow, 3, currentRow, 4).Merge();
            worksheet.Range(currentRow, 3, currentRow, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 3).GetRichText().AddText("LED Combinations : ").SetBold();
            worksheet.Cell(currentRow, 3).GetRichText().AddText(data.LEDCombinations);

            worksheet.Range(currentRow, 5, currentRow, 6).Merge();
            worksheet.Range(currentRow, 5, currentRow, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 5).GetRichText().AddText("Batch Code : ").SetBold();
            worksheet.Cell(currentRow, 5).GetRichText().AddText(data.BatchCode);

            currentRow++;

            // Row 4: Sensor, Lamp, PKD
            worksheet.Range(currentRow, 1, currentRow, 2).Merge();
            worksheet.Range(currentRow, 1, currentRow, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 1).GetRichText().AddText("Sensor Details & Qty : ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.SensorDetailsQty);

            worksheet.Range(currentRow, 3, currentRow, 4).Merge();
            worksheet.Range(currentRow, 3, currentRow, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 3).GetRichText().AddText("Lamp Details : ").SetBold();
            worksheet.Cell(currentRow, 3).GetRichText().AddText(data.LampDetails);

            worksheet.Range(currentRow, 5, currentRow, 6).Merge();
            worksheet.Range(currentRow, 5, currentRow, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 5).GetRichText().AddText("PKD : ").SetBold();
            worksheet.Cell(currentRow, 5).GetRichText().AddText(data.PKD);


            // --- 3. Main Table Headers (Rows 7-8) ---
            int headerRowStart = currentRow;

            // Row 7 (Merged Headers)
            worksheet.Range(currentRow, COL_SL, currentRow + 1, COL_SL).Merge();
            worksheet.Cell(currentRow, COL_SL).Value = "SL. No.";
            worksheet.Range(currentRow, COL_PARAM, currentRow + 1, COL_PARAM).Merge();
            worksheet.Cell(currentRow, COL_PARAM).Value = "Description Testing";
            worksheet.Range(currentRow, COL_SPEC, currentRow + 1, COL_SPEC).Merge();
            worksheet.Cell(currentRow, COL_SPEC).Value = "Minimum Technical Requirement";
            worksheet.Range(currentRow, COL_S1, currentRow, COL_S5).Merge();
            worksheet.Cell(currentRow, COL_RESULT).Value = "Result - Pass/ Fail";
            worksheet.Cell(currentRow, COL_RESULT).Style.Font.FontColor = XLColor.FromColor(Color.Red);

            currentRow++; // Row 8 (Sample Headers)

            worksheet.Cell(currentRow, COL_S1).Value = "Sample-1";
            worksheet.Cell(currentRow, COL_S1 + 1).Value = "Sample-2";
            worksheet.Cell(currentRow, COL_S1 + 2).Value = "Sample-3";
            worksheet.Cell(currentRow, COL_S1 + 3).Value = "Sample-4";
            worksheet.Cell(currentRow, COL_S1 + 4).Value = "Sample-5";

            // Apply header style to Rows 7 and 8
            var headerRange = worksheet.Range(headerRowStart, COL_SL, currentRow, COL_RESULT);
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                headerRange.Style.Alignment.WrapText = true;
                headerRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                headerRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            }

            currentRow++; // Starting data entry at Row 9

            // --- 4. Data Entry ---
            int slNo = 1;
            int dataStartRow = currentRow;

            // Helper to insert data columns for one row (SL.No/Parameter handled externally)
            Func<int, string, string, string?, string?, string?, string?, string?, string?, int> AddDataRow =
                (slNumber, paramText1, paramText2, s1, s2, s3, s4, s5, result) =>
                {
                    worksheet.Cell(currentRow, COL_SL).Value = slNumber;
                    // Set the Description Testing and Minimum Technical Requirement texts
                    worksheet.Cell(currentRow, COL_PARAM).Value = paramText1; // Description Testing
                    worksheet.Cell(currentRow, COL_SPEC).Value = paramText2; // Minimum Technical Requirement

                    // Set the sample data and result
                    worksheet.Cell(currentRow, COL_S1).Value = s1 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 1).Value = s2 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 2).Value = s3 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 3).Value = s4 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 4).Value = s5 ?? "";
                    worksheet.Cell(currentRow, COL_RESULT).Value = result ?? "";

                    worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Alignment.WrapText = true;

                    currentRow++;
                    return currentRow;
                };

            // Adding Under Voltage test data with the description as "Under Voltage Cut off & auto recovery" and "As per Design Documents"
            AddDataRow(slNo++, "Under Voltage Cut off & auto recovery", "As per Design Documents",
                       data.UnderVoltage_Sample1, data.UnderVoltage_Sample2,
                       data.UnderVoltage_Sample3, data.UnderVoltage_Sample4,
                       data.UnderVoltage_Sample5, data.UnderVoltage_Result);

            // Adding Over Voltage test data with the description as "Over Voltage Cut off & auto recovery" and "As per Design Documents"
            AddDataRow(slNo++, "Over Voltage Cut off & auto recovery", "As per Design Documents",
                       data.OverVoltage_Sample1, data.OverVoltage_Sample2,
                       data.OverVoltage_Sample3, data.OverVoltage_Sample4,
                       data.OverVoltage_Sample5, data.OverVoltage_Result);

            // Adding Open Circuit Protection test data with the description as "As per Design Documents"
            AddDataRow(slNo++, "Open Circuit Protection", "As per Design Documents",
                       data.OpenCircuit_Sample1, data.OpenCircuit_Sample2,
                       data.OpenCircuit_Sample3, data.OpenCircuit_Sample4,
                       data.OpenCircuit_Sample5, data.OpenCircuit_Result);

            // Adding Short Circuit Protection test data with the description as "Driver goes to shutdown mode"
            AddDataRow(slNo++, "Short Circuit Protection", "Driver goes to shutdown mode",
                       data.ShortCircuit_Sample1, data.ShortCircuit_Sample2,
                       data.ShortCircuit_Sample3, data.ShortCircuit_Sample4,
                       data.ShortCircuit_Sample5, data.ShortCircuit_Result);

            // Adding Reverse Polarity Protection test data with the description as "Driver goes to shutdown mode"
            AddDataRow(slNo++, "Reverse Polarity Protection", "Driver goes to shutdown mode",
                       data.ReversePolarity_Sample1, data.ReversePolarity_Sample2,
                       data.ReversePolarity_Sample3, data.ReversePolarity_Sample4,
                       data.ReversePolarity_Sample5, data.ReversePolarity_Result);

            // Adding Over Load Protection test data with the description as "Driver goes to shutdown mode"
            AddDataRow(slNo++, "Over Load Protection", "Driver goes to shutdown mode",
                       data.OverLoad_Sample1, data.OverLoad_Sample2,
                       data.OverLoad_Sample3, data.OverLoad_Sample4,
                       data.OverLoad_Sample5, data.OverLoad_Result);

            // Adding Over Thermal Cut Off test data with the description as "As per Design Documents"
            AddDataRow(slNo++, "Over Thermal Cut Off", "As per Design Documents",
                       data.OverThermal_Sample1, data.OverThermal_Sample2,
                       data.OverThermal_Sample3, data.OverThermal_Sample4,
                       data.OverThermal_Sample5, data.OverThermal_Result);

            // Adding Earth Fault Protection test data with the description as "Driver should not fail"
            AddDataRow(slNo++, "Earth Fault Protection", "Driver should not fail",
                       data.EarthFault_Sample1, data.EarthFault_Sample2,
                       data.EarthFault_Sample3, data.EarthFault_Sample4,
                       data.EarthFault_Sample5, data.EarthFault_Result);

            // Adding Driver Isolation - Input - Output test data with the description as "As per Design Documents"
            AddDataRow(slNo++, "Driver Isolation - Input - Output", "As per Design Documents",
                       data.DriverIsolation_Sample1, data.DriverIsolation_Sample2,
                       data.DriverIsolation_Sample3, data.DriverIsolation_Sample4,
                       data.DriverIsolation_Sample5, data.DriverIsolation_Result);

            // Adding High Voltage test data with the description as "Should pass as per Driver data sheet"
            AddDataRow(slNo++, "High Voltage -440 V / 320V / 350 V etc.", "Should pass as per Driver data sheet",
                       data.HighVoltage_Sample1, data.HighVoltage_Sample2,
                       data.HighVoltage_Sample3, data.HighVoltage_Sample4,
                       data.HighVoltage_Sample5, data.HighVoltage_Result);

            // Adding HV test data with the description "a) Class 1:- 1.5 kv for a period of 1 Min b) Class 2:- 3.72Kv for a period of 1 Min"
            AddDataRow(slNo++, "HV",
                       "a) Class 1:- 1.5 kv for a period of 1 Min b) Class 2:- 3.72Kv for a period of 1 Min",
                       data.HV_Sample1, data.HV_Sample2,
                       data.HV_Sample3, data.HV_Sample4,
                       data.HV_Sample5, data.HV_Result);

            // Adding Insulation Resistance test data with the description "> 2 MΩ"
            AddDataRow(slNo++, "Insulation Resistance", "> 2 MΩ",
                       data.InsulationResistance_Sample1, data.InsulationResistance_Sample2,
                       data.InsulationResistance_Sample3, data.InsulationResistance_Sample4,
                       data.InsulationResistance_Sample5, data.InsulationResistance_Result);

            // Adding Earth Continuity test data with the description "Continuity"
            AddDataRow(slNo++, "Earth Continuity", "Continuity",
                       data.EarthContinuity_Sample1, data.EarthContinuity_Sample2,
                       data.EarthContinuity_Sample3, data.EarthContinuity_Sample4,
                       data.EarthContinuity_Sample5, data.EarthContinuity_Result);

            // Adding SELV Protection test data with the description "As per Design Documents"
            AddDataRow(slNo++, "SELV Protection", "As per Design Documents",
                       data.SELVProtection_Sample1, data.SELVProtection_Sample2,
                       data.SELVProtection_Sample3, data.SELVProtection_Sample4,
                       data.SELVProtection_Sample5, data.SELVProtection_Result);

            // Adding Leakage Current test data with the description "< 1 Mili Amp"
            AddDataRow(slNo++, "Leakage Current", "< 1 Mili Amp",
                       data.LeakageCurrent_Sample1, data.LeakageCurrent_Sample2,
                       data.LeakageCurrent_Sample3, data.LeakageCurrent_Sample4,
                       data.LeakageCurrent_Sample5, data.LeakageCurrent_Result);

            // Adding Creepage & Clearance test data with the description "As per IS 10322"
            AddDataRow(slNo++, "Creepage & Clearance", "As per IS 10322",
                       data.CreepageClearance_Sample1, data.CreepageClearance_Sample2,
                       data.CreepageClearance_Sample3, data.CreepageClearance_Sample4,
                       data.CreepageClearance_Sample5, data.CreepageClearance_Result);

            // Adding Hi-pot on MCPCB test data with the description "Withstand specified voltages"
            AddDataRow(slNo++, "Hi-pot on MCPCB", "Withstand specified voltages",
                       data.HiPotMCPCB_Sample1, data.HiPotMCPCB_Sample2,
                       data.HiPotMCPCB_Sample3, data.HiPotMCPCB_Sample4,
                       data.HiPotMCPCB_Sample5, data.HiPotMCPCB_Result);

            // Adding On/off switching for 4hrs at 50deg C at 240V AC test data with the description "Driver / PCB should not failed"
            AddDataRow(slNo++, "On/off (5Sec On/5Sec Off) switching for 4hrs at 50deg C at 240V AC",
                       "Driver / PCB should not failed",
                       data.OnOffSwitching_Sample1, data.OnOffSwitching_Sample2,
                       data.OnOffSwitching_Sample3, data.OnOffSwitching_Sample4,
                       data.OnOffSwitching_Sample5, data.OnOffSwitching_Result);

            // Adding Soaking / Ageing at Room temperature test data with the description 
            // "At 240V AC, 60Min: Burning. At 240V AC, 30Min: 10Sec on/off. At 270V AC, 45Min: Burning. At 140V AC, 45Min: Burning."
            AddDataRow(slNo++, "Soaking / Ageing at Room temperature",
                       "At 240V AC, 60Min: Burning\nAt 240V AC, 30Min: 10Sec on/off\nAt 270V AC, 45Min: Burning\nAt 140V AC, 45Min: Burning",
                       data.SoakingAgeing_Sample1, data.SoakingAgeing_Sample2,
                       data.SoakingAgeing_Sample3, data.SoakingAgeing_Sample4,
                       data.SoakingAgeing_Sample5, data.SoakingAgeing_Result);

            // Adding Rolling Endurance Test (Continuous glow for 24x7 days) test data with the description "Daily observations to be taken."
            AddDataRow(slNo++, "Rolling Test/Endurance Test (Continuous glow at least 24x7 days)",
                       "Daily observations to be taken. (PCB & driver should not failed. Check burning of SPDs, LED lens, wires, plastic parts etc.)",
                       data.RollingEndurance_Sample1, data.RollingEndurance_Sample2,
                       data.RollingEndurance_Sample3, data.RollingEndurance_Sample4,
                       data.RollingEndurance_Sample5, data.RollingEndurance_Result);

            // Adding Glow Test test data with the description "Should Lit Up"
            AddDataRow(slNo++, "Glow Test", "Should Lit Up",
                       data.GlowTest_Sample1, data.GlowTest_Sample2,
                       data.GlowTest_Sample3, data.GlowTest_Sample4,
                       data.GlowTest_Sample5, data.GlowTest_Result);

            // Adding Lamp Accommodation test data with the description "No Looseness or Tight"
            AddDataRow(slNo++, "Lamp Accommodation (for Tube)", "No Looseness or Tight",
                       data.LampAccommodation_Sample1, data.LampAccommodation_Sample2,
                       data.LampAccommodation_Sample3, data.LampAccommodation_Sample4,
                       data.LampAccommodation_Sample5, data.LampAccommodation_Result);

            // Adding Dali Function test data with the description "As per Design Documents"
            AddDataRow(slNo++, "Dali Function", "As per Design Documents",
                       data.DaliFunction_Sample1, data.DaliFunction_Sample2,
                       data.DaliFunction_Sample3, data.DaliFunction_Sample4,
                       data.DaliFunction_Sample5, data.DaliFunction_Result);

            // Adding Tuneable CCT test data with the description "As per Design Documents"
            AddDataRow(slNo++, "Tuneable CCT", "As per Design Documents",
                       data.TuneableCCT_Sample1, data.TuneableCCT_Sample2,
                       data.TuneableCCT_Sample3, data.TuneableCCT_Sample4,
                       data.TuneableCCT_Sample5, data.TuneableCCT_Result);

            // Adding Battery Backup test data with the description "As per Design Documents"
            AddDataRow(slNo++, "Battery Back Up", "As per Design Documents",
                       data.BatteryBackup_Sample1, data.BatteryBackup_Sample2,
                       data.BatteryBackup_Sample3, data.BatteryBackup_Sample4,
                       data.BatteryBackup_Sample5, data.BatteryBackup_Result);

            // Adding Smart Lighting test data with the description "As per Design Documents"
            AddDataRow(slNo++, "Smart Lighting (POE, BLE, RF- NEMA etc.)", "As per Design Documents",
                       data.SmartLighting_Sample1, data.SmartLighting_Sample2,
                       data.SmartLighting_Sample3, data.SmartLighting_Sample4,
                       data.SmartLighting_Sample5, data.SmartLighting_Result);

            // Adding Sensor Function test data with the description "As per Design Documents"
            AddDataRow(slNo++, "Sensor Function", "As per Design Documents",
                       data.SensorFunction_Sample1, data.SensorFunction_Sample2,
                       data.SensorFunction_Sample3, data.SensorFunction_Sample4,
                       data.SensorFunction_Sample5, data.SensorFunction_Result);



            int dataEndRow = currentRow - 1; // The row where the last data item was placed

            // Final Result Row (Electronic Details Result - Pass/Fail)
            worksheet.Range(currentRow, COL_SL, currentRow, COL_RESULT).Merge();
            worksheet.Cell(currentRow, COL_SL).Value = "Electronic Details Result - Pass/ Fail : ";
            worksheet.Cell(currentRow, COL_SL).Style.Font.Bold = true;
            worksheet.Cell(currentRow, COL_SL).Style.Font.FontColor = XLColor.FromColor(Color.Red);
            worksheet.Cell(currentRow, COL_RESULT).Value = data.OverallReportResult;  // Your result field
            worksheet.Cell(currentRow, COL_RESULT).Style.Font.Bold = true;

            worksheet.Range(7, COL_SL, currentRow, COL_RESULT).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            currentRow++; // Move to next row

            // --- Apply All Borders and Styles to the Main Table Data ---
            var dataRange = worksheet.Range(dataStartRow, COL_SL, dataEndRow, COL_RESULT);
            {
                dataRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                dataRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                dataRange.Style.Alignment.WrapText = true;
                dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            }

            // Apply borders to the Overall Result row
            worksheet.Range(dataEndRow + 1, COL_SL, dataEndRow + 1, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(dataEndRow + 1, COL_SL, dataEndRow + 1, COL_RESULT).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

            // --- Signatures (Bottom of Report) ---
            currentRow++; // Move to next row

            // Tested By
            worksheet.Range(currentRow, 1, currentRow, 4).Merge();
            worksheet.Range(currentRow, 1, currentRow, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 1, currentRow, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range(currentRow, 1, currentRow, 4).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 1).GetRichText().AddText("Tested By: ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.TestedByName ?? "");  // Your field

            // Verified By
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Merge();
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 5).GetRichText().AddText("Verified By: ").SetBold();
            worksheet.Cell(currentRow, 5).GetRichText().AddText(data.VerifiedByName ?? "");  // Your field




            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            var filename = $"Electrical_Protection_Report_{Id}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }



    public async Task<ActionResult> ExportPhyCheckToExcelAsync(int Id)
    {
        try
        {
            var data = await _physicalCheckAndVisualInspectionRepository.GetPhysicalCheckAndVisualInspectionsByIdAsync(Id);
            const int COL_SL = 1;
            const int COL_PARAM = 2;
            const int COL_SPEC = 3;
            const int COL_S1 = 4;
            const int COL_S5 = 8;
            const int COL_RESULT = 9;

            // 1. Setup Workbook and Worksheet (using ClosedXML)
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Physical Check Report");

            // --- General Report Formatting ---
            worksheet.SheetView.ZoomScale = 82;
            worksheet.Style.Font.FontName = "Aptos Narrow";
            worksheet.Style.Font.FontSize = 12;
            // Setting fixed column widths as requested
            worksheet.Columns("A").Width = 14.80;
            worksheet.Columns("B").Width = 39.10;
            worksheet.Columns("C").Width = 28.40;
            worksheet.Columns("D").Width = 19.80;
            worksheet.Columns("E").Width = 20.20;
            worksheet.Columns("F").Width = 21.30;
            worksheet.Columns("G").Width = 26.10;
            worksheet.Columns("H").Width = 26.10;
            worksheet.Columns("I").Width = 22.80;

            // --- Header Generation (Rows 1-6) ---
            int currentRow = 1;

            // Defining constants for the title row with image offset
            const int COL_IMAGE = 9;
            const int COL_TITLE_END = COL_RESULT - 1; // Column H (8) for the end of the merged title

            // 1. Adjust the Main Title Merge Range: A1 to H1 (one column less)
            worksheet.Row(currentRow).Height = 73.20;
            var titleRange = worksheet.Range(currentRow, 1, currentRow, COL_TITLE_END);
            titleRange.Merge();
            titleRange.Value = "Physical Check/Visual Inspection Report";
            titleRange.Style.Font.Bold = true;
            titleRange.Style.Font.FontSize = 22;
            titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            titleRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            titleRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium);

            // Optional: Increase the height of the row to ensure the image fits well.
            worksheet.Row(currentRow).Height = 73.20;

            // 2. Add the Image to the I column (I1)
            var webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, "images", "wipro-logo.png");

            if (System.IO.File.Exists(imagePath))
            {
                var picture = worksheet.AddPicture(imagePath)
                    .MoveTo(worksheet.Cell(currentRow, COL_IMAGE), 45, 12)
                    .WithPlacement(XLPicturePlacement.Move)
                    .Scale(0.9);
            }

            var imageRange = worksheet.Range(currentRow, 9, currentRow, 9);
            imageRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Medium);
            imageRange.Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            imageRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Medium);
            imageRange.Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);

            currentRow++; // Row 2
                          // Report No.
            worksheet.Range(currentRow, 1, currentRow, 6).Merge();
            worksheet.Range(currentRow, 1, currentRow, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 1, currentRow, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(currentRow, 7).GetRichText().AddText("Report No.  ").SetBold();
            worksheet.Cell(currentRow, 7).GetRichText().AddText(data.Report_No);
            worksheet.Cell(currentRow, 7).Style.Font.FontName = "Arial";
            worksheet.Cell(currentRow, 7).Style.Font.FontSize = 12;
            worksheet.Range(currentRow, 7, currentRow, COL_RESULT).Merge();
            worksheet.Range(currentRow, 7, currentRow, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 7, currentRow, COL_RESULT).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 7, currentRow, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Row(currentRow).Height = 22.20;


            currentRow++; // Row 3, 4
                          // Customer/Project Name and Date
            worksheet.Range(currentRow, 1, currentRow + 1, 6).Merge();
            worksheet.Range(currentRow, 1, currentRow + 1, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 1, currentRow + 1, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(currentRow, 1).GetRichText().AddText("Customer/Project Name :  ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.Project_Name);
            worksheet.Range(currentRow, 7, currentRow + 1, COL_RESULT).Merge();
            worksheet.Cell(currentRow, 7).GetRichText().AddText("Date :  ").SetBold();
            worksheet.Cell(currentRow, 7).GetRichText().AddText(data.Report_Date.HasValue
                ? data.Report_Date.Value.ToShortDateString()
                : string.Empty);
            worksheet.Range(currentRow, 7, currentRow + 1, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 7, currentRow + 1, COL_RESULT).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 7, currentRow + 1, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range(currentRow, 1, currentRow + 1, COL_RESULT).Style.Font.FontName = "Arial";
            worksheet.Range(currentRow, 1, currentRow + 1, COL_RESULT).Style.Font.FontSize = 10;
            worksheet.Row(currentRow + 1).Height = 19.80;

            currentRow += 2; // Row 5
                             // Product Details
            worksheet.Range(currentRow, 1, currentRow, 2).Merge();
            worksheet.Range(currentRow, 1, currentRow, 2).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 1, currentRow, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(currentRow, 1).GetRichText().AddText("Product Cat Ref :  ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.Product_Cat_Ref);
            worksheet.Range(currentRow, 3, currentRow, 5).Merge();
            worksheet.Range(currentRow, 3, currentRow, 5).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 3, currentRow, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(currentRow, 3).GetRichText().AddText("Product Description :  ").SetBold();
            worksheet.Cell(currentRow, 3).GetRichText().AddText(data.Product_Description);
            worksheet.Cell(currentRow, 6).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(currentRow, 6).GetRichText().AddText("Batch Code :  ").SetBold();
            worksheet.Cell(currentRow, 6).GetRichText().AddText(data.Batch_Code);
            worksheet.Cell(currentRow, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(currentRow, 7).GetRichText().AddText("PKD :  ").SetBold();
            worksheet.Cell(currentRow, 7).GetRichText().AddText(data.PKD);
            worksheet.Range(currentRow, 8, currentRow, COL_RESULT).Merge();
            worksheet.Range(currentRow, 8, currentRow, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 8, currentRow, COL_RESULT).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 8, currentRow, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Cell(currentRow, 8).GetRichText().AddText("Quantity :  ").SetBold();
            worksheet.Cell(currentRow, 8).GetRichText().AddText((data.Quantity ?? 0).ToString());
            worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Font.FontName = "Arial";
            worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Font.FontSize = 10;
            worksheet.Row(currentRow).Height = 18.00;

            currentRow++; // Row 6

            worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Merge();
            worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Row(currentRow).Height = 13.80;


            currentRow++; // Starts main table at Row 7

            // --- 3. Main Table Headers (Rows 7-8) ---
            int headerRowStart = currentRow;

            // Row 7 (Merged Headers)
            worksheet.Range(currentRow, COL_SL, currentRow + 1, COL_SL).Merge();
            worksheet.Cell(currentRow, COL_SL).Value = "SL. No.";
            worksheet.Range(currentRow, COL_PARAM, currentRow + 1, COL_PARAM).Merge();
            worksheet.Cell(currentRow, COL_PARAM).Value = "Parameter";
            worksheet.Range(currentRow, COL_SPEC, currentRow + 1, COL_SPEC).Merge();
            worksheet.Cell(currentRow, COL_SPEC).Value = "Specification";
            worksheet.Range(currentRow, COL_S1, currentRow, COL_S5).Merge();
            worksheet.Cell(currentRow, COL_S1).Value = "Observations";
            worksheet.Range(currentRow, COL_RESULT, currentRow + 1, COL_RESULT).Merge();
            worksheet.Cell(currentRow, COL_RESULT).Value = "Result- Pass/ Fail";
            worksheet.Cell(currentRow, COL_RESULT).Style.Font.FontColor = XLColor.FromColor(Color.Red);

            currentRow++; // Row 8 (Sample Headers)

            worksheet.Cell(currentRow, COL_S1).Value = "Sample-1";
            worksheet.Cell(currentRow, COL_S1 + 1).Value = "Sample-2";
            worksheet.Cell(currentRow, COL_S1 + 2).Value = "Sample-3";
            worksheet.Cell(currentRow, COL_S1 + 3).Value = "Sample-4";
            worksheet.Cell(currentRow, COL_S1 + 4).Value = "Sample-5";

            // Apply header style to Rows 7 and 8
            var headerRange = worksheet.Range(headerRowStart, COL_SL, currentRow, COL_RESULT);
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                headerRange.Style.Alignment.WrapText = true;
                headerRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                headerRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            }

            currentRow++; // Starting data entry at Row 9

            // --- 4. Data Entry ---
            int slNo = 1;
            int dataStartRow = currentRow;

            // Helper to insert data columns for one row (SL.No/Parameter handled externally)
            Func<string, string?, string?, string?, string?, string?, string?, int> AddDataRow =
                (spec, s1, s2, s3, s4, s5, result) =>
                {
                    worksheet.Cell(currentRow, COL_SPEC).Value = spec;
                    worksheet.Cell(currentRow, COL_S1).Value = s1 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 1).Value = s2 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 2).Value = s3 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 3).Value = s4 ?? "";
                    worksheet.Cell(currentRow, COL_S1 + 4).Value = s5 ?? "";
                    worksheet.Cell(currentRow, COL_RESULT).Value = result ?? "";
                    worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Range(currentRow, 1, currentRow, COL_RESULT).Style.Alignment.WrapText = true;

                    currentRow++;
                    return currentRow;
                };

            // Helper to merge the SL.No and Parameter columns
            Action<int, int, string, string> MergeColumns = (startRow, endRow, param, slVal) =>
            {
                // SL No.
                worksheet.Range(startRow, COL_SL, endRow, COL_SL).Merge();
                worksheet.Cell(startRow, COL_SL).Value = slVal;
                worksheet.Cell(startRow, COL_SL).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(startRow, COL_SL).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Parameter
                worksheet.Range(startRow, COL_PARAM, endRow, COL_PARAM).Merge();
                worksheet.Cell(startRow, COL_PARAM).Value = param;
                worksheet.Cell(startRow, COL_PARAM).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Cell(startRow, COL_PARAM).Style.Alignment.WrapText = true;

                // Set the row height to adjust contents for the merged cells
                // NOTE: Using a loop to adjust row height after content is added
                for (int i = startRow; i <= endRow; i++)
                {
                    worksheet.Row(i).AdjustToContents();
                }
            };

            // Helper for single-row sections
            Action<string, string, string?, string?, string?, string?, string?, string?> AddSingleRowSection =
                (param, spec, s1, s2, s3, s4, s5, result) =>
                {
                    int startRow = currentRow;
                    AddDataRow(spec, s1, s2, s3, s4, s5, result);
                    MergeColumns(startRow, currentRow - 1, param, slNo.ToString());
                    slNo++;
                };


            // 1. Wipro Branding (Single Row)
            AddSingleRowSection("Wipro Branding on Product & Driver , SPD", "As per design docket",
                data.WiproBranding_Sample1, data.WiproBranding_Sample2, data.WiproBranding_Sample3,
                data.WiproBranding_Sample4, data.WiproBranding_Sample5, data.WiproBranding_Result);

            // 2. Product Label/Driver Label / SPD Labels (Single Row)
            AddSingleRowSection("Product Label/Driver Label / SPD Labels",
                "As per BIS , Dockets/ NPI and readable , Non Erasable, non tearable, or by Laser Sticker should not come out easily.",
                data.ProductDriverLabels_Sample1, data.ProductDriverLabels_Sample2, data.ProductDriverLabels_Sample3,
                data.ProductDriverLabels_Sample4, data.ProductDriverLabels_Sample5, data.ProductDriverLabels_Result);

            // 3. Packing Stickers / Dimension / FIFO Sticker (Single Row)
            AddSingleRowSection("Packing Stickers / Dimension / FIFO Sticker",
                "As per BIS , Dockets/ NPI and readable , Non Erasable, Sticker should not come out easily.",
                data.PackingStickers_Sample1, data.PackingStickers_Sample2, data.PackingStickers_Sample3,
                data.PackingStickers_Sample4, data.PackingStickers_Sample5, data.PackingStickers_Result);

            // 4. Dimensions (Single Row)
            AddSingleRowSection("Dimensions",
                "Length –\nBreadth –\nHeight -\nCut out -\nHousing Pole ID -\nMaterial Thickness-\nMounting bracket dimension-",
                data.Dimensions_Sample1, data.Dimensions_Sample2, data.Dimensions_Sample3,
                data.Dimensions_Sample4, data.Dimensions_Sample5, data.Dimensions_Result);

            // 5. Surface Finish (Multi-Row: 2 rows)
            int startRow_5 = currentRow;

            // 5a. General Surface Finish
            AddDataRow(
                "Free from Burr, Dent , Sharp Edges, No Powder Coating Peel off, Dust free etc. Colour Shade should match",
                data.SurfaceFinish_Sample1, data.SurfaceFinish_Sample2, data.SurfaceFinish_Sample3,
                data.SurfaceFinish_Sample4, data.SurfaceFinish_Sample5, data.SurfaceFinish_Result);

            // 5b. LED/COB Surface Finish
            AddDataRow(
                "LED/COB should be free from dust.",
                data.SurfaceFinishLED_Sample1, data.SurfaceFinishLED_Sample2, data.SurfaceFinishLED_Sample3,
                data.SurfaceFinishLED_Sample4, data.SurfaceFinishLED_Sample5, data.SurfaceFinishLED_Result);

            MergeColumns(startRow_5, currentRow - 1, "Surface Finish", slNo.ToString());
            slNo++;

            // 6. DFT (Single Row)
            AddSingleRowSection("DFT (Dry Film Thickness)",
                "50-120µ",
                data.DFT_Sample1, data.DFT_Sample2, data.DFT_Sample3,
                data.DFT_Sample4, data.DFT_Sample5, data.DFT_Result);

            // 7. Visual (Multi-Row: 2 rows)
            int startRow_7 = currentRow;

            // 7a. Visual (Gaps)
            AddDataRow(
                "Free from gaps at Joinery, Edges, In-between Diffuser/Lens and Housing.",
                data.Visual_Sample1, data.Visual_Sample2, data.Visual_Sample3,
                data.Visual_Sample4, data.Visual_Sample5, data.Visual_Result);

            // 7b. Visual at Lit up Condition
            AddDataRow(
                "No Black Spots Visibility, Band, dark patch /Shadow, Uniformity , NO LED Visibility, NO light leakage, NO blinking, flickering",
                data.VisualLitUp_Sample1, data.VisualLitUp_Sample2, data.VisualLitUp_Sample3,
                data.VisualLitUp_Sample4, data.VisualLitUp_Sample5, data.VisualLitUp_Result);

            MergeColumns(startRow_7, currentRow - 1, "Visual", slNo.ToString());
            slNo++;

            // 8. PCB/COB fitment (Multi-Row: 5 rows)
            int startRow_8 = currentRow;

            // 8a. Thermal Paste
            AddDataRow(
                "Thermal Paste should not be dry, cover entire surface of PCB mounting area",
                data.PcbCobFitment_Sample1, data.PcbCobFitment_Sample2, data.PcbCobFitment_Sample3,
                data.PcbCobFitment_Sample4, data.PcbCobFitment_Sample5, data.PcbCobFitment_Result);

            // 8b. No Gaps
            AddDataRow(
                "No gaps in between Housing and PCB.",
                data.PcbCobFitmentNoGaps_Sample1, data.PcbCobFitmentNoGaps_Sample2, data.PcbCobFitmentNoGaps_Sample3,
                data.PcbCobFitmentNoGaps_Sample4, data.PcbCobFitmentNoGaps_Sample5, data.PcbCobFitmentNoGaps_Result);

            // 8c. Screw
            AddDataRow(
                "PCB holding screw / Button should be intact.",
                data.PcbCobFitmentScrew_Sample1, data.PcbCobFitmentScrew_Sample2, data.PcbCobFitmentScrew_Sample3,
                data.PcbCobFitmentScrew_Sample4, data.PcbCobFitmentScrew_Sample5, data.PcbCobFitmentScrew_Result);

            // 8d. Washer
            AddDataRow(
                "screw or washers should not touches LED",
                data.PcbCobFitmentWasher_Sample1, data.PcbCobFitmentWasher_Sample2, data.PcbCobFitmentWasher_Sample3,
                data.PcbCobFitmentWasher_Sample4, data.PcbCobFitmentWasher_Sample5, data.PcbCobFitmentWasher_Result);

            // 8e. Drawing
            AddDataRow(
                "PCB tracks and dimensions as per drawing.",
                data.PcbCobFitmentDrawing_Sample1, data.PcbCobFitmentDrawing_Sample2, data.PcbCobFitmentDrawing_Sample3,
                data.PcbCobFitmentDrawing_Sample4, data.PcbCobFitmentDrawing_Sample5, data.PcbCobFitmentDrawing_Result);

            MergeColumns(startRow_8, currentRow - 1, "PCB/COB fitment", slNo.ToString());
            slNo++;

            // 9. Soldering (Multi-Row: 3 rows)
            int startRow_9 = currentRow;

            // 9a. Dry
            AddDataRow("Free from Dry soldering",
                data.Soldering_Sample1, data.Soldering_Sample2, data.Soldering_Sample3,
                data.Soldering_Sample4, data.Soldering_Sample5, data.Soldering_Result);

            // 9b. Spatter
            AddDataRow("Free from Spatter",
                data.SolderingSpatter_Sample1, data.SolderingSpatter_Sample2, data.SolderingSpatter_Sample3,
                data.SolderingSpatter_Sample4, data.SolderingSpatter_Sample5, data.SolderingSpatter_Result);

            // 9c. Globule
            AddDataRow("Globule formation with Shiny finish",
                data.SolderingGlobule_Sample1, data.SolderingGlobule_Sample2, data.SolderingGlobule_Sample3,
                data.SolderingGlobule_Sample4, data.SolderingGlobule_Sample5, data.SolderingGlobule_Result);

            MergeColumns(startRow_9, currentRow - 1, "Soldering", slNo.ToString());
            slNo++;

            // 10. Wiring Dressing (Single Row)
            AddSingleRowSection("Wiring & dressing",
                "As per design docket/drawing and without stress.",
                data.WiringDressing_Sample1, data.WiringDressing_Sample2, data.WiringDressing_Sample3,
                data.WiringDressing_Sample4, data.WiringDressing_Sample5, data.WiringDressing_Result);

            // 11. Mechanical Fitment (Single Row)
            AddSingleRowSection("Mechanical fitment with mating part",
                "Smooth and secure fitment as per design",
                data.MechanicalFitment_Sample1, data.MechanicalFitment_Sample2, data.MechanicalFitment_Sample3,
                data.MechanicalFitment_Sample4, data.MechanicalFitment_Sample5, data.MechanicalFitment_Result);

            // 12. LED Lens Gap (Single Row)
            AddSingleRowSection("Gap between LED & Lens",
                "Gap should be there, as per drawing.",
                data.LedLensGap_Sample1, data.LedLensGap_Sample2, data.LedLensGap_Sample3,
                data.LedLensGap_Sample4, data.LedLensGap_Sample5, data.LedLensGap_Result);

            // 13. Gasket (Single Row)
            AddSingleRowSection("Gasket",
                "Fitment & Hardness, proper seating",
                data.Gasket_Sample1, data.Gasket_Sample2, data.Gasket_Sample3,
                data.Gasket_Sample4, data.Gasket_Sample5, data.Gasket_Result);

            // 14. Fragmentation for toughen glass (Single Row)
            AddSingleRowSection("Fragmentation for toughen glass",
                "As per Docket.",
                data.GlassFragmentation_Sample1, data.GlassFragmentation_Sample2, data.GlassFragmentation_Sample3,
                data.GlassFragmentation_Sample4, data.GlassFragmentation_Sample5, data.GlassFragmentation_Result);

            int dataEndRow = currentRow - 1; // The row where the last data item was placed

            // Final Result Row (Row after last data line) - FIXED LABEL HERE
            worksheet.Range(currentRow, COL_SL, currentRow, COL_RESULT).Merge();
            worksheet.Cell(currentRow, COL_SL).Value = "Result- Pass/ Fail : ";
            worksheet.Cell(currentRow, COL_SL).Style.Font.Bold = true;
            worksheet.Cell(currentRow, COL_SL).Style.Font.FontColor = XLColor.FromColor(Color.Red);
            worksheet.Cell(currentRow, COL_RESULT).Value = data.Final_Result;
            worksheet.Cell(currentRow, COL_RESULT).Style.Font.Bold = true;


            worksheet.Range(7, COL_SL, currentRow, COL_RESULT).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);

            currentRow++;

            // --- 6. Apply All Borders and Styles to the Main Table Data ---
            var dataRange = worksheet.Range(dataStartRow, COL_SL, dataEndRow, COL_RESULT);
            {
                dataRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                dataRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                dataRange.Style.Alignment.WrapText = true;
                dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            }

            // Apply borders to the Overall Result row
            worksheet.Range(dataEndRow + 1, COL_SL, dataEndRow + 1, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            worksheet.Range(dataEndRow + 1, COL_SL, dataEndRow + 1, COL_RESULT).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);


            // --- 5. Signatures (Bottom of Report) ---
            currentRow++;

            // Tested By
            worksheet.Range(currentRow, 1, currentRow, 4).Merge();
            worksheet.Range(currentRow, 1, currentRow, 4).Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 1, currentRow, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range(currentRow, 1, currentRow, 4).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 1).GetRichText().AddText("Tested by :  ").SetBold();
            worksheet.Cell(currentRow, 1).GetRichText().AddText(data.TestedBy ?? "");

            // Verified By
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Merge();
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range(currentRow, 5, currentRow, COL_RESULT).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            worksheet.Cell(currentRow, 5).GetRichText().AddText("Verified by :  ").SetBold();
            worksheet.Cell(currentRow, 5).GetRichText().AddText(data.VerifiedBy ?? "");

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            var filename = Id + ".xlsx";
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }
        catch (Exception ex)
        {
            var exc = new OperationResult
            {
                Success = false,
                Message = ex.Message
            };
            return Json(exc);
        }
    }

    #region SuregTestReport
    public IActionResult SurgeTestReportDetails()
    {

        return View();
    }
    public IActionResult SuregTestReport()
    {
        return View();
    }




    #endregion

    #region TemperatureRiseTestOfLuminaire
    public IActionResult TemperatureRiseTestOfLuminaire()
    {

        return View();
    }
    public IActionResult TemperatureRiseTestOfLuminaireDetails()
    {
        return View();
    }
    #endregion

    #region PhotometryTestReport 
    public IActionResult PhotometryTestReport()
    {

        return View();
    }
    public IActionResult PhotometryTestReportDetails()
    {
        return View();
    }

    #endregion
    #region InstallationTrial

    public IActionResult InstallationTrialReport()
    {

        return View();
    }
    public IActionResult InstallationTrialReportDetails()
    {
        return View();
    }



    #endregion
    #region Impact test

    public IActionResult Impacttest()
    {

        return View();
    }
    public IActionResult ImpactTestDetails()
    {
        return View();
    }


    #endregion
    #region Impact test

    public IActionResult IngressProtetction()
    {

        return View();
    }
    public IActionResult IngressProtetctionDetails()
    {
        return View();
    }


    #endregion
    #region Regulatory
    public IActionResult RegulatoryRequirements()
    {

        return View();
    }
    public IActionResult RegulatoryRequirementsDetails()
    {
        return View();
    }


#endregion



