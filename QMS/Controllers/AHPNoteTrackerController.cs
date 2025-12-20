using iText.Kernel.Colors;
using iText.Kernel.Font;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QMS.Core.Models;
using QMS.Core.Repositories.AHPNoteReposotory;
using QMS.Core.Repositories.VendorRepository;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace QMS.Controllers;

public class AHPNoteTrackerController : Controller
{
    private readonly IAHPNoteReposotory _ahpNoteRepository;
    private readonly IVendorRepository _vendorRepository;

    public AHPNoteTrackerController(IAHPNoteReposotory ahpNoteRepository, IVendorRepository vendorRepository)
    {
        _ahpNoteRepository = ahpNoteRepository;
        _vendorRepository = vendorRepository;
    }

    public IActionResult AHPNotetracker()
    {
        return View();
    }

    public async Task<ActionResult> GetAHPNoteListAsync(int financialYear)
    {
        var result = await _ahpNoteRepository.GetAHPNotesAsync(financialYear);
        return Json(result);
    }

    public async Task<IActionResult> AHPNotetrackerDetailsAsync(int Id, int financialYear)
    {
        var model = new AHPNoteViewModel();
        var supplierList = await _vendorRepository.GetListAsync();
        var suppliers = supplierList.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Name
        })
        .ToList();
        ViewBag.SupplierList = suppliers;
        if(financialYear > 0) { model.FinancialYear = financialYear; }
        if(Id > 0)
        {
            model = await _ahpNoteRepository.GetAHPNotesByIdAsync(Id);
        }
        return View(model);
    }

    public async Task<ActionResult> InsertUpdateAHPNoteAsync(AHPNoteViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { Success = false, Errors = errors });
        }

        if(model.Id > 0)
        {
            model.UpdatedBy = HttpContext.Session.GetInt32("UserId");
            model.UpdatedOn = DateTime.Now;
            var result = await _ahpNoteRepository.UpdateAHPNotesAsync(model);
            return Json(result);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var result = await _ahpNoteRepository.InsertAHPNotesAsync(model);
            return Json(result);
        }
    }

    public async Task<ActionResult> DeleteAHPNoteAsync(int Id)
    {
        var result = await _ahpNoteRepository.DeleteAHPNotesAsync(Id);
        return Json(result);
    }

    public async Task<ActionResult> ExportToPDFAsync(int id)
    {
        try
        {
            var data = await _ahpNoteRepository.GetAHPNotesByIdAsync(id);

            using (var stream = new MemoryStream())
            {
                // 1. Setup PDF Document (A4 Portrait)
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var doc = new Document(pdf, PageSize.A4);
                doc.SetMargins(20, 20, 20, 20);

                // Fonts
                var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                var fontRegular = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                // 2. Dynamic Year Logic
                int reqStartYY = data.FinancialYear / 100;
                int reqEndYY = data.FinancialYear % 100;
                string currFY = $"FY{reqStartYY:D2}-{reqEndYY:D2}";
                string prevFY = $"FY{(reqStartYY - 1):D2}-{(reqEndYY - 1):D2}";

                var now = DateTime.Now;
                int realCurrentStartYY = (now.Month >= 4) ? (now.Year % 100) : ((now.Year - 1) % 100);

                bool showQ1 = false, showQ2 = false, showQ3 = false, showQ4 = false;

                if (reqStartYY < realCurrentStartYY)
                {
                    showQ1 = showQ2 = showQ3 = showQ4 = true;
                }
                else if (reqStartYY == realCurrentStartYY)
                {
                    int currentQ = (now.Month >= 4 && now.Month <= 6) ? 1 :
                                   (now.Month >= 7 && now.Month <= 9) ? 2 :
                                   (now.Month >= 10 && now.Month <= 12) ? 3 : 4;
                    showQ1 = currentQ >= 1;
                    showQ2 = currentQ >= 2;
                    showQ3 = currentQ >= 3;
                    showQ4 = currentQ == 4;
                }
                bool[] qFlags = { showQ1, showQ2, showQ3, showQ4 };
                int dynCols = (showQ1 ? 1 : 0) + (showQ2 ? 1 : 0) + (showQ3 ? 1 : 0) + (showQ4 ? 1 : 0);
                string[] currQs = { $"{currFY}\nQ1", $"{currFY}\nQ2", $"{currFY}\nQ3", $"{currFY}\nQ4" };

                // Helper to get Widths
                float[] GetWidths(float snW, float descW, float dataW, int fixedDataCols, int dynamicColsCount, int trailingCols = 0)
                {
                    var w = new List<float>();
                    if (snW > 0) w.Add(snW);
                    if (descW > 0) w.Add(descW);
                    for (int i = 0; i < fixedDataCols; i++) w.Add(dataW);
                    for (int i = 0; i < dynamicColsCount; i++) w.Add(dataW);
                    for (int i = 0; i < trailingCols; i++) w.Add(dataW);
                    return w.ToArray();
                }

                // Local Helper for Manual Headers
                void AddManualHeader(Table t, string text)
                {
                    t.AddCell(new Cell()
                        .Add(new Paragraph(text ?? "").SetFontSize(9))
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE));
                }

                // ==========================================
                // SHEET 1: CIB LIGHTING
                // ==========================================
                doc.Add(new Paragraph($"CIB Lighting – Q2 {currFY} REPORT")
                    .SetFont(fontBold).SetFontSize(14).SetFontColor(ColorConstants.BLUE));

                // --- 1. SEE Metrics ---
                Table table1 = new Table(UnitValue.CreatePercentArray(GetWidths(0.7f, 4f, 1.5f, 5, dynCols))).UseAllAvailableWidth();
                AddHeaders(table1, qFlags, new[] { "S.N.", "Projects", "Target" }, new[] { $"{prevFY}\nQ1", $"{prevFY}\nQ2", $"{prevFY}\nQ3", $"{prevFY}\nQ4" }, currQs);

                AddRow(table1, qFlags, new object[] { "1", "SEE Index – Engagement", data.Lit_SEE_Engagement_Target },
                    new object[] { data.Lit_SEE_Engagement_PreviousYear_Q1, data.Lit_SEE_Engagement_PreviousYear_Q2, data.Lit_SEE_Engagement_PreviousYear_Q3, data.Lit_SEE_Engagement_PreviousYear_Q4 },
                    new object[] { data.Lit_SEE_Engagement_CurrentYear_Q1, data.Lit_SEE_Engagement_CurrentYear_Q2, data.Lit_SEE_Engagement_CurrentYear_Q3, data.Lit_SEE_Engagement_CurrentYear_Q4 },
                    centerFirstCol: true);

                AddRow(table1, qFlags, new object[] { "2", "SEE Index – Effectiveness", data.Lit_SEE_Effectiveness_Target },
                    new object[] { data.Lit_SEE_Effectiveness_PreviousYear_Q1, data.Lit_SEE_Effectiveness_PreviousYear_Q2, data.Lit_SEE_Effectiveness_PreviousYear_Q3, data.Lit_SEE_Effectiveness_PreviousYear_Q4 },
                    new object[] { data.Lit_SEE_Effectiveness_CurrentYear_Q1, data.Lit_SEE_Effectiveness_CurrentYear_Q2, data.Lit_SEE_Effectiveness_CurrentYear_Q3, data.Lit_SEE_Effectiveness_CurrentYear_Q4 },
                    centerFirstCol: true);

                AddTableSection(doc, "1. SEE Metrics", fontBold, table1); // <--- Using New Helper

                // --- 2. Six Sigma Projects ---
                Table table2 = new Table(UnitValue.CreatePercentArray(GetWidths(0.7f, 4f, 1.5f, 2, dynCols))).UseAllAvailableWidth();
                AddHeaders(table2, qFlags, new[] { "S.N.", "Projects", "Baseline", "Target" }, null, currQs);

                var pService = new Paragraph().Add(new Text("Service Complaints").SetFont(fontBold)).Add(new Text("\n95% Closure in 15 days").SetFont(fontRegular));
                AddRow(table2, qFlags, new object[] { "1", pService, data.Lit_SS_ServiceComplaints_Baseline, data.Lit_SS_ServiceComplaints_Target },
                    new object[] { data.Lit_SS_ServiceComplaints_CurrentYear_Q1, data.Lit_SS_ServiceComplaints_CurrentYear_Q2, data.Lit_SS_ServiceComplaints_CurrentYear_Q3, data.Lit_SS_ServiceComplaints_CurrentYear_Q4 }, centerFirstCol: true);

                var pDesign = new Paragraph().Add(new Text("Design LSG Conversion").SetFont(fontBold)).Add(new Text("\nMetro\nEM").SetFont(fontRegular));
                AddRow(table2, qFlags, new object[] { "2", pDesign, data.Lit_SS_DesignLSG_Baseline, data.Lit_SS_DesignLSG_Target },
                    new object[] { data.Lit_SS_DesignLSG_CurrentYear_Q1, data.Lit_SS_DesignLSG_CurrentYear_Q2, data.Lit_SS_DesignLSG_CurrentYear_Q3, data.Lit_SS_DesignLSG_CurrentYear_Q4 }, centerFirstCol: true);

                var pCost = new Paragraph().Add(new Text("Cost Reduction Project-").SetFont(fontBold)).Add(new Text("\n2x2\nArcus\nDownlighter\nFlood Light\nHigh bay\nLinear\nStreetlight\nWell glass").SetFont(fontRegular));
                AddRow(table2, qFlags, new object[] { "3", pCost, data.Lit_SS_CostReduction_Baseline, data.Lit_SS_CostReduction_Target },
                    new object[] { data.Lit_SS_CostReduction_CurrentYear_Q1, data.Lit_SS_CostReduction_CurrentYear_Q2, data.Lit_SS_CostReduction_CurrentYear_Q3, data.Lit_SS_CostReduction_CurrentYear_Q4 }, centerFirstCol: true);

                var pOtif = new Paragraph().Add(new Text("OTIF > 90 %").SetFont(fontRegular));
                AddRow(table2, qFlags, new object[] { "4", pOtif, data.Lit_SS_OTIF_Baseline, data.Lit_SS_OTIF_Target },
                    new object[] { data.Lit_SS_OTIF_CurrentYear_Q1, data.Lit_SS_OTIF_CurrentYear_Q2, data.Lit_SS_OTIF_CurrentYear_Q3, data.Lit_SS_OTIF_CurrentYear_Q4 }, centerFirstCol: true);

                var pSpm = new Paragraph().Add(new Text("SPM Score for Critical 10 vendors > 3 Stars").SetFont(fontRegular));
                AddRow(table2, qFlags, new object[] { "5", pSpm, data.Lit_SS_SPMScore_Baseline, data.Lit_SS_SPMScore_Target },
                    new object[] { data.Lit_SS_SPMScore_CurrentYear_Q1, data.Lit_SS_SPMScore_CurrentYear_Q2, data.Lit_SS_SPMScore_CurrentYear_Q3, data.Lit_SS_SPMScore_CurrentYear_Q4 }, centerFirstCol: true);

                AddTableSection(doc, "2. Six Sigma Projects", fontBold, table2);

                // --- 3. CSAT ---
                Table table3 = new Table(UnitValue.CreatePercentArray(GetWidths(0, 4f, 1.5f, 3, dynCols, 1))).UseAllAvailableWidth();
                AddHeaders(table3, qFlags, new[] { "Metrics", $"{prevFY} YTD", "Baseline", "Target" }, null, currQs, new[] { $"{currFY} YTD" });

                AddRow(table3, qFlags, new object[] { "Nos of Request sent", data.Lit_CSAT_ReqSent_YTD_PreviousYear, data.Lit_CSAT_ReqSent_Baseline, data.Lit_CSAT_ReqSent_Target },
                    new object[] { data.Lit_CSAT_ReqSent_CurrentYear_Q1, data.Lit_CSAT_ReqSent_CurrentYear_Q2, data.Lit_CSAT_ReqSent_CurrentYear_Q3, data.Lit_CSAT_ReqSent_CurrentYear_Q4 },
                    new object[] { data.Lit_CSAT_ReqSent_YTD_CurrentYear });

                AddRow(table3, qFlags, new object[] { "Nos of response received", data.Lit_CSAT_RespRecvd_YTD_PreviousYear, data.Lit_CSAT_RespRecvd_Baseline, data.Lit_CSAT_RespRecvd_Target },
                    new object[] { data.Lit_CSAT_RespRecvd_CurrentYear_Q1, data.Lit_CSAT_RespRecvd_CurrentYear_Q2, data.Lit_CSAT_RespRecvd_CurrentYear_Q3, data.Lit_CSAT_RespRecvd_CurrentYear_Q4 },
                    new object[] { data.Lit_CSAT_RespRecvd_YTD_CurrentYear });

                AddRow(table3, qFlags, new object[] { "Promoter", data.Lit_CSAT_Promoter_YTD_PreviousYear, data.Lit_CSAT_Promoter_Baseline, data.Lit_CSAT_Promoter_Target },
                    new object[] { data.Lit_CSAT_Promoter_CurrentYear_Q1, data.Lit_CSAT_Promoter_CurrentYear_Q2, data.Lit_CSAT_Promoter_CurrentYear_Q3, data.Lit_CSAT_Promoter_CurrentYear_Q4 },
                    new object[] { data.Lit_CSAT_Promoter_YTD_CurrentYear });

                AddRow(table3, qFlags, new object[] { "Detractor", data.Lit_CSAT_Detractor_YTD_PreviousYear, data.Lit_CSAT_Detractor_Baseline, data.Lit_CSAT_Detractor_Target },
                    new object[] { data.Lit_CSAT_Detractor_CurrentYear_Q1, data.Lit_CSAT_Detractor_CurrentYear_Q2, data.Lit_CSAT_Detractor_CurrentYear_Q3, data.Lit_CSAT_Detractor_CurrentYear_Q4 },
                    new object[] { data.Lit_CSAT_Detractor_YTD_CurrentYear });

                var pNps = new Paragraph().Add(new Text("NPS%= ").SetFont(fontRegular)).Add(new Text("\n(Nos of Promoters-Nos of Detractors)/\n(Nos of response Received) X 100").SetFont(fontRegular));
                AddRow(table3, qFlags, new object[] { pNps, data.Lit_CSAT_NPS_YTD_PreviousYear, data.Lit_CSAT_NPS_Baseline, data.Lit_CSAT_NPS_Target },
                    new object[] { data.Lit_CSAT_NPS_CurrentYear_Q1, data.Lit_CSAT_NPS_CurrentYear_Q2, data.Lit_CSAT_NPS_CurrentYear_Q3, data.Lit_CSAT_NPS_CurrentYear_Q4 },
                    new object[] { data.Lit_CSAT_NPS_YTD_CurrentYear });

                AddTableSection(doc, "3. CSAT", fontBold, table3);

                // --- 4. SPM Score ---
                Table table4 = new Table(UnitValue.CreatePercentArray(GetWidths(0.7f, 4f, 1.5f, 0, dynCols))).UseAllAvailableWidth();
                AddHeaders(table4, qFlags, new[] { "S.N.", "Supplier Name" }, null, currQs);

                var litSMPCounter = 1;

                if (data.Lit_SPM_Supp1 > 0)
                {
                    var supp1 = await _vendorRepository.GetByIdAsync(data.Lit_SPM_Supp1.Value);
                    AddRow(table4, qFlags, new object[] { litSMPCounter.ToString(), supp1?.Name ?? "" }, new object[] { data.Lit_SPM_Supp1_CurrentYear_Q1, data.Lit_SPM_Supp1_CurrentYear_Q2, data.Lit_SPM_Supp1_CurrentYear_Q3, data.Lit_SPM_Supp1_CurrentYear_Q4 }, centerFirstCol: true);
                    litSMPCounter++;
                }
                if (data.Lit_SPM_Supp2 > 0)
                {
                    var supp2 = await _vendorRepository.GetByIdAsync(data.Lit_SPM_Supp2.Value);
                    AddRow(table4, qFlags, new object[] { litSMPCounter.ToString(), supp2?.Name ?? "" }, new object[] { data.Lit_SPM_Supp2_CurrentYear_Q1, data.Lit_SPM_Supp2_CurrentYear_Q2, data.Lit_SPM_Supp2_CurrentYear_Q3, data.Lit_SPM_Supp2_CurrentYear_Q4 }, centerFirstCol: true);
                    litSMPCounter++;
                }
                if (data.Lit_SPM_Supp3 > 0)
                {
                    var supp3 = await _vendorRepository.GetByIdAsync(data.Lit_SPM_Supp3.Value);
                    AddRow(table4, qFlags, new object[] { litSMPCounter.ToString(), supp3?.Name ?? "" }, new object[] { data.Lit_SPM_Supp3_CurrentYear_Q1, data.Lit_SPM_Supp3_CurrentYear_Q2, data.Lit_SPM_Supp3_CurrentYear_Q3, data.Lit_SPM_Supp3_CurrentYear_Q4 }, centerFirstCol: true);
                    litSMPCounter++;
                }
                if (data.Lit_SPM_Supp4 > 0)
                {
                    var supp4 = await _vendorRepository.GetByIdAsync(data.Lit_SPM_Supp4.Value);
                    AddRow(table4, qFlags, new object[] { litSMPCounter.ToString(), supp4?.Name ?? "" }, new object[] { data.Lit_SPM_Supp4_CurrentYear_Q1, data.Lit_SPM_Supp4_CurrentYear_Q2, data.Lit_SPM_Supp4_CurrentYear_Q3, data.Lit_SPM_Supp4_CurrentYear_Q4 }, centerFirstCol: true);
                    litSMPCounter++;
                }
                if (data.Lit_SPM_Supp5 > 0)
                {
                    var supp5 = await _vendorRepository.GetByIdAsync(data.Lit_SPM_Supp5.Value);
                    AddRow(table4, qFlags, new object[] { litSMPCounter.ToString(), supp5?.Name ?? "" }, new object[] { data.Lit_SPM_Supp5_CurrentYear_Q1, data.Lit_SPM_Supp5_CurrentYear_Q2, data.Lit_SPM_Supp5_CurrentYear_Q3, data.Lit_SPM_Supp5_CurrentYear_Q4 }, centerFirstCol: true);
                    litSMPCounter++;
                }
                if (data.Lit_SPM_Supp6 > 0)
                {
                    var supp6 = await _vendorRepository.GetByIdAsync(data.Lit_SPM_Supp6.Value);
                    AddRow(table4, qFlags, new object[] { litSMPCounter.ToString(), supp6?.Name ?? "" }, new object[] { data.Lit_SPM_Supp6_CurrentYear_Q1, data.Lit_SPM_Supp6_CurrentYear_Q2, data.Lit_SPM_Supp6_CurrentYear_Q3, data.Lit_SPM_Supp6_CurrentYear_Q4 }, centerFirstCol: true);
                    litSMPCounter++;
                }
                if (data.Lit_SPM_Supp7 > 0)
                {
                    var supp7 = await _vendorRepository.GetByIdAsync(data.Lit_SPM_Supp7.Value);
                    AddRow(table4, qFlags, new object[] { litSMPCounter.ToString(), supp7?.Name ?? "" }, new object[] { data.Lit_SPM_Supp7_CurrentYear_Q1, data.Lit_SPM_Supp7_CurrentYear_Q2, data.Lit_SPM_Supp7_CurrentYear_Q3, data.Lit_SPM_Supp7_CurrentYear_Q4 }, centerFirstCol: true);
                    litSMPCounter++;
                }
                if (data.Lit_SPM_Supp8 > 0)
                {
                    var supp8 = await _vendorRepository.GetByIdAsync(data.Lit_SPM_Supp8.Value);
                    AddRow(table4, qFlags, new object[] { litSMPCounter.ToString(), supp8?.Name ?? "" }, new object[] { data.Lit_SPM_Supp8_CurrentYear_Q1, data.Lit_SPM_Supp8_CurrentYear_Q2, data.Lit_SPM_Supp8_CurrentYear_Q3, data.Lit_SPM_Supp8_CurrentYear_Q4 }, centerFirstCol: true);
                    litSMPCounter++;
                }
                if (data.Lit_SPM_Supp9 > 0)
                {
                    var supp9 = await _vendorRepository.GetByIdAsync(data.Lit_SPM_Supp9.Value);
                    AddRow(table4, qFlags, new object[] { litSMPCounter.ToString(), supp9?.Name ?? "" }, new object[] { data.Lit_SPM_Supp9_CurrentYear_Q1, data.Lit_SPM_Supp9_CurrentYear_Q2, data.Lit_SPM_Supp9_CurrentYear_Q3, data.Lit_SPM_Supp9_CurrentYear_Q4 }, centerFirstCol: true);
                    litSMPCounter++;
                }
                if (data.Lit_SPM_Supp10 > 0)
                {
                    var supp10 = await _vendorRepository.GetByIdAsync(data.Lit_SPM_Supp10.Value);
                    AddRow(table4, qFlags, new object[] { litSMPCounter.ToString(), supp10?.Name ?? "" }, new object[] { data.Lit_SPM_Supp10_CurrentYear_Q1, data.Lit_SPM_Supp10_CurrentYear_Q2, data.Lit_SPM_Supp10_CurrentYear_Q3, data.Lit_SPM_Supp10_CurrentYear_Q4 }, centerFirstCol: true);
                    litSMPCounter++;
                }

                AddTableSection(doc, "4. SPM Score", fontBold, table4);

                // --- 5. OTIF (MERGED) ---
                Table table5 = new Table(UnitValue.CreatePercentArray(GetWidths(0, 4f, 1.5f, 2, dynCols, 1))).UseAllAvailableWidth();

                Cell otifMerged = new Cell(2, 1).Add(new Paragraph("On time In Full Delivery Performance at 90% for all C&I Products"))
                    .SetFont(fontRegular).SetFontSize(8).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.LEFT);
                table5.AddCell(otifMerged);

                AddManualHeader(table5, $"{prevFY} (YTD)");
                AddManualHeader(table5, "Target");
                if (showQ1) AddManualHeader(table5, $"{currFY}\nQ1");
                if (showQ2) AddManualHeader(table5, $"{currFY}\nQ2");
                if (showQ3) AddManualHeader(table5, $"{currFY}\nQ3");
                if (showQ4) AddManualHeader(table5, $"{currFY}\nQ4");
                AddManualHeader(table5, $"{currFY} (YTD)");

                AddCell(table5, data.Lit_OTIF_YTD_PreviousYear);
                AddCell(table5, data.Lit_OTIF_Target);
                if (showQ1) AddCell(table5, data.Lit_OTIF_CurrentYear_Q1);
                if (showQ2) AddCell(table5, data.Lit_OTIF_CurrentYear_Q2);
                if (showQ3) AddCell(table5, data.Lit_OTIF_CurrentYear_Q3);
                if (showQ4) AddCell(table5, data.Lit_OTIF_CurrentYear_Q4);
                AddCell(table5, data.Lit_OTIF_YTD_CurrentYear);

                AddTableSection(doc, "5. On Time in Full Delivery", fontBold, table5);

                // --- 6. Service Complaints Closure (MERGED) ---
                Table table6 = new Table(UnitValue.CreatePercentArray(GetWidths(0, 4f, 1.5f, 3, dynCols, 1))).UseAllAvailableWidth();

                var pServClosure = new Paragraph().Add(new Text("Service Complaints Closure").SetFont(fontRegular)).Add(new Text("\n90% Closure in 15 days").SetFont(fontRegular));
                Cell servMerged = new Cell(2, 1).Add(pServClosure).SetFontSize(8).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.LEFT);
                table6.AddCell(servMerged);

                AddManualHeader(table6, $"{prevFY}");
                AddManualHeader(table6, "Baseline");
                AddManualHeader(table6, "Target");
                if (showQ1) AddManualHeader(table6, $"{currFY}\nQ1");
                if (showQ2) AddManualHeader(table6, $"{currFY}\nQ2");
                if (showQ3) AddManualHeader(table6, $"{currFY}\nQ3");
                if (showQ4) AddManualHeader(table6, $"{currFY}\nQ4");
                AddManualHeader(table6, $"{currFY} (YTD)");

                AddCell(table6, data.Lit_SC_Closure_YTD_PreviousYear);
                AddCell(table6, data.Lit_SC_Closure_Baseline);
                AddCell(table6, data.Lit_SC_Closure_Target);
                if (showQ1) AddCell(table6, data.Lit_SC_Closure_CurrentYear_Q1);
                if (showQ2) AddCell(table6, data.Lit_SC_Closure_CurrentYear_Q2);
                if (showQ3) AddCell(table6, data.Lit_SC_Closure_CurrentYear_Q3);
                if (showQ4) AddCell(table6, data.Lit_SC_Closure_CurrentYear_Q4);
                AddCell(table6, data.Lit_SC_Closure_YTD_CurrentYear);

                AddTableSection(doc, "6. Service Complaints Closure", fontBold, table6);

                // --- 7. CSO ---
                Table table7 = new Table(UnitValue.CreatePercentArray(GetWidths(0, 4f, 1.5f, 1, dynCols, 1))).UseAllAvailableWidth();
                AddHeaders(table7, qFlags, new[] { "", $"{prevFY} (YTD)" }, null, currQs, new[] { $"{currFY} YTD" });

                AddRow(table7, qFlags, new object[] { "Total CSO logged", data.Lit_CSO_TotalLogged_YTD_PreviousYear },
                    new object[] { data.Lit_CSO_TotalLogged_CurrentYear_Q1, data.Lit_CSO_TotalLogged_CurrentYear_Q2, data.Lit_CSO_TotalLogged_CurrentYear_Q3, data.Lit_CSO_TotalLogged_CurrentYear_Q4 },
                    new object[] { data.Lit_CSO_TotalLogged_YTD_CurrentYear });
                AddRow(table7, qFlags, new object[] { "Total A Class CSO's Logged", data.Lit_CSO_TotalAClass_YTD_PreviousYear },
                    new object[] { data.Lit_CSO_TotalAClass_CurrentYear_Q1, data.Lit_CSO_TotalAClass_CurrentYear_Q2, data.Lit_CSO_TotalAClass_CurrentYear_Q3, data.Lit_CSO_TotalAClass_CurrentYear_Q4 },
                    new object[] { data.Lit_CSO_TotalAClass_YTD_CurrentYear });
                AddRow(table7, qFlags, new object[] { "A Class Closed CSO's as on date", data.Lit_CSO_AClassClosed_YTD_PreviousYear },
                    new object[] { data.Lit_CSO_AClassClosed_CurrentYear_Q1, data.Lit_CSO_AClassClosed_CurrentYear_Q2, data.Lit_CSO_AClassClosed_CurrentYear_Q3, data.Lit_CSO_AClassClosed_CurrentYear_Q4 },
                    new object[] { data.Lit_CSO_AClassClosed_YTD_CurrentYear });
                AddRow(table7, qFlags, new object[] { "A Class Closed CSO's <45 Days", data.Lit_CSO_AClassClosedLess45_YTD_PreviousYear },
                    new object[] { data.Lit_CSO_AClassClosedLess45_CurrentYear_Q1, data.Lit_CSO_AClassClosedLess45_CurrentYear_Q2, data.Lit_CSO_AClassClosedLess45_CurrentYear_Q3, data.Lit_CSO_AClassClosedLess45_CurrentYear_Q4 },
                    new object[] { data.Lit_CSO_AClassClosedLess45_YTD_CurrentYear });
                AddRow(table7, qFlags, new object[] { "Percentage Closure of “A” Class CSO under 45 days", data.Lit_CSO_PercentageClosure_YTD_PreviousYear },
                    new object[] { data.Lit_CSO_PercentageClosure_CurrentYear_Q1, data.Lit_CSO_PercentageClosure_CurrentYear_Q2, data.Lit_CSO_PercentageClosure_CurrentYear_Q3, data.Lit_CSO_PercentageClosure_CurrentYear_Q4 },
                    new object[] { data.Lit_CSO_PercentageClosure_YTD_CurrentYear });

                AddTableSection(doc, "7. CSO", fontBold, table7);

                // --- 8. Cost Savings ---
                Table table8 = new Table(UnitValue.CreatePercentArray(GetWidths(0, 4f, 1.5f, 2, dynCols, 1))).UseAllAvailableWidth();
                AddHeaders(table8, qFlags, new[] { "Cost Savings", $"{prevFY}", "Target" }, null, currQs, new[] { $"{currFY} (YTD)" });

                AddRow(table8, qFlags, new object[] { "", data.Lit_CostSavings_YTD_PreviousYear, data.Lit_CostSavings_Target },
                    new object[] { data.Lit_CostSavings_CurrentYear_Q1, data.Lit_CostSavings_CurrentYear_Q2, data.Lit_CostSavings_CurrentYear_Q3, data.Lit_CostSavings_CurrentYear_Q4 },
                    new object[] { data.Lit_CostSavings_YTD_CurrentYear });

                AddTableSection(doc, "8. Cost Savings / Target", fontBold, table8);

                // --- 9. OQL ---
                Table table9 = new Table(UnitValue.CreatePercentArray(new float[] { 3f, 1.5f, 1.5f, 1f, 1f, 1f, 1f, 1f, 1f })).UseAllAvailableWidth();
                string[] oqlHead = { "Vendor", $"{prevFY}", $"Target", "PC01", "PC02", "PC03", "PC04", "PC05", "PC06" };
                foreach (var h in oqlHead) AddHeaderCell(table9, h);

                void AddOQLCell(Table t, object v, float height = 0)
                {
                    var c = new Cell().Add(new Paragraph(v?.ToString() ?? "").SetFontSize(8)).SetTextAlignment(TextAlignment.LEFT).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                    if (height > 0) c.SetHeight(height);
                    t.AddCell(c);
                }

                void AddOQL(params object[] vals) { foreach (var v in vals) AddOQLCell(table9, v); }

                if (data.Lit_OQL_Vendor1 > 0)
                {
                    var vendor1 = await _vendorRepository.GetByIdAsync(data.Lit_OQL_Vendor1.Value);
                    AddOQL(vendor1?.Name ?? "", data.Lit_OQL_Vendor1_YTD_PreviousYear, data.Lit_OQL_Vendor1_Target, data.Lit_OQL_Vendor1_PC01, data.Lit_OQL_Vendor1_PC02, data.Lit_OQL_Vendor1_PC03, data.Lit_OQL_Vendor1_PC04, data.Lit_OQL_Vendor1_PC05, data.Lit_OQL_Vendor1_PC06);
                }
                if (data.Lit_OQL_Vendor2 > 0)
                {
                    var vendor2 = await _vendorRepository.GetByIdAsync(data.Lit_OQL_Vendor2.Value);
                    AddOQL(vendor2?.Name ?? "", data.Lit_OQL_Vendor2_YTD_PreviousYear, data.Lit_OQL_Vendor2_Target, data.Lit_OQL_Vendor2_PC01, data.Lit_OQL_Vendor2_PC02, data.Lit_OQL_Vendor2_PC03, data.Lit_OQL_Vendor2_PC04, data.Lit_OQL_Vendor2_PC05, data.Lit_OQL_Vendor2_PC06);
                }
                if (data.Lit_OQL_Vendor3 > 0)
                {
                    var vendor3 = await _vendorRepository.GetByIdAsync(data.Lit_OQL_Vendor3.Value);
                    AddOQL(vendor3?.Name ?? "", data.Lit_OQL_Vendor3_YTD_PreviousYear, data.Lit_OQL_Vendor3_Target, data.Lit_OQL_Vendor3_PC01, data.Lit_OQL_Vendor3_PC02, data.Lit_OQL_Vendor3_PC03, data.Lit_OQL_Vendor3_PC04, data.Lit_OQL_Vendor3_PC05, data.Lit_OQL_Vendor3_PC06);
                }
                if (data.Lit_OQL_Vendor4 > 0)
                {
                    var vendor4 = await _vendorRepository.GetByIdAsync(data.Lit_OQL_Vendor4.Value);
                    AddOQL(vendor4?.Name ?? "", data.Lit_OQL_Vendor4_YTD_PreviousYear, data.Lit_OQL_Vendor4_Target, data.Lit_OQL_Vendor4_PC01, data.Lit_OQL_Vendor4_PC02, data.Lit_OQL_Vendor4_PC03, data.Lit_OQL_Vendor4_PC04, data.Lit_OQL_Vendor4_PC05, data.Lit_OQL_Vendor4_PC06);
                }
                if (data.Lit_OQL_Vendor5 > 0)
                {
                    var vendor5 = await _vendorRepository.GetByIdAsync(data.Lit_OQL_Vendor5.Value);
                    AddOQL(vendor5?.Name ?? "", data.Lit_OQL_Vendor5_YTD_PreviousYear, data.Lit_OQL_Vendor5_Target, data.Lit_OQL_Vendor5_PC01, data.Lit_OQL_Vendor5_PC02, data.Lit_OQL_Vendor5_PC03, data.Lit_OQL_Vendor5_PC04, data.Lit_OQL_Vendor5_PC05, data.Lit_OQL_Vendor5_PC06);
                }
                if (data.Lit_OQL_Vendor6 > 0)
                {
                    var vendor6 = await _vendorRepository.GetByIdAsync(data.Lit_OQL_Vendor6.Value);
                    AddOQL(vendor6?.Name ?? "", data.Lit_OQL_Vendor6_YTD_PreviousYear, data.Lit_OQL_Vendor6_Target, data.Lit_OQL_Vendor6_PC01, data.Lit_OQL_Vendor6_PC02, data.Lit_OQL_Vendor6_PC03, data.Lit_OQL_Vendor6_PC04, data.Lit_OQL_Vendor6_PC05, data.Lit_OQL_Vendor6_PC06);
                }
                if (data.Lit_OQL_Vendor7 > 0)
                {
                    var vendor7 = await _vendorRepository.GetByIdAsync(data.Lit_OQL_Vendor7.Value);
                    AddOQL(vendor7?.Name ?? "", data.Lit_OQL_Vendor7_YTD_PreviousYear, data.Lit_OQL_Vendor7_Target, data.Lit_OQL_Vendor7_PC01, data.Lit_OQL_Vendor7_PC02, data.Lit_OQL_Vendor7_PC03, data.Lit_OQL_Vendor7_PC04, data.Lit_OQL_Vendor7_PC05, data.Lit_OQL_Vendor7_PC06);
                }
                if (data.Lit_OQL_Vendor8 > 0)
                {
                    var vendor8 = await _vendorRepository.GetByIdAsync(data.Lit_OQL_Vendor8.Value);
                    AddOQL(vendor8?.Name ?? "", data.Lit_OQL_Vendor8_YTD_PreviousYear, data.Lit_OQL_Vendor8_Target, data.Lit_OQL_Vendor8_PC01, data.Lit_OQL_Vendor8_PC02, data.Lit_OQL_Vendor8_PC03, data.Lit_OQL_Vendor8_PC04, data.Lit_OQL_Vendor8_PC05, data.Lit_OQL_Vendor8_PC06);
                }

                Cell cumAvgCell = new Cell(2, 1).Add(new Paragraph("Cumulative Average"))
                    .SetFont(fontRegular).SetFontSize(8).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.LEFT);
                table9.AddCell(cumAvgCell);

                float rowHeight = 10f;
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg1_YTD_PreviousYear, rowHeight);
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg1_Target, rowHeight);
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg1_PC01, rowHeight);
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg1_PC02, rowHeight);
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg1_PC03, rowHeight);
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg1_PC04, rowHeight);
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg1_PC05, rowHeight);
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg1_PC06, rowHeight);

                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg2_YTD_PreviousYear, rowHeight);
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg2_Target, rowHeight);
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg2_PC01, rowHeight);
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg2_PC02, rowHeight);
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg2_PC03, rowHeight);
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg2_PC04, rowHeight);
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg2_PC05, rowHeight);
                AddOQLCell(table9, data.Lit_OQL_CumulativeAvg2_PC06, rowHeight);

                AddTableSection(doc, "9. Outgoing Quality Level", fontBold, table9);

                // ==========================================
                // SHEET 2: CIB SEATING
                // ==========================================
                doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                doc.Add(new Paragraph($"CIB Seating – Q2 {currFY} Report").SetFont(fontBold).SetFontSize(14));

                // --- 1. Seating Six Sigma ---
                Table tableS1 = new Table(UnitValue.CreatePercentArray(GetWidths(0.7f, 4f, 1.5f, 2, dynCols, 1))).UseAllAvailableWidth();
                AddHeaders(tableS1, qFlags, new[] { "S.N.", "Projects", "Baseline", "Target" }, null, currQs, new[] { $"{currFY} (YTD)" });

                AddRow(tableS1, qFlags, new object[] { "1", "CN Closure 90% in 20 days", data.Seat_SS_CNClosure_Baseline, data.Seat_SS_CNClosure_Target },
                    new object[] { data.Seat_SS_CNClosure_CurrentYear_Q1, data.Seat_SS_CNClosure_CurrentYear_Q2, data.Seat_SS_CNClosure_CurrentYear_Q3, data.Seat_SS_CNClosure_CurrentYear_Q4 },
                    new object[] { data.Seat_SS_CNClosure_YTD_CurrentYear }, centerFirstCol: true);

                AddRow(tableS1, qFlags, new object[] { "2", "OTIF > 90 %", data.Seat_SS_OTIF_Baseline, data.Seat_SS_OTIF_Target },
                    new object[] { data.Seat_SS_OTIF_CurrentYear_Q1, data.Seat_SS_OTIF_CurrentYear_Q2, data.Seat_SS_OTIF_CurrentYear_Q3, data.Seat_SS_OTIF_CurrentYear_Q4 },
                    new object[] { data.Seat_SS_OTIF_YTD_CurrentYear }, centerFirstCol: true);

                AddRow(tableS1, qFlags, new object[] { "3", "SPM Score for Critical 3 vendors > 3 Stars", data.Seat_SS_SPMScore_Baseline, data.Seat_SS_SPMScore_Target },
                    new object[] { data.Seat_SS_SPMScore_CurrentYear_Q1, data.Seat_SS_SPMScore_CurrentYear_Q2, data.Seat_SS_SPMScore_CurrentYear_Q3, data.Seat_SS_SPMScore_CurrentYear_Q4 },
                    new object[] { data.Seat_SS_SPMScore_YTD_CurrentYear }, centerFirstCol: true);

                AddTableSection(doc, "1. Six Sigma Projects", fontBold, tableS1);

                // --- 2. OTIF (MERGED CELL) ---
                Table tableS2 = new Table(UnitValue.CreatePercentArray(GetWidths(0, 4f, 1.5f, 2, dynCols, 1))).UseAllAvailableWidth();
                Cell seatOtifMerged = new Cell(2, 1).Add(new Paragraph("On time In Full Delivery Performance.")).SetFont(fontRegular).SetFontSize(8).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.LEFT);
                tableS2.AddCell(seatOtifMerged);

                AddManualHeader(tableS2, $"{prevFY} (YTD)");
                AddManualHeader(tableS2, "Target");
                if (showQ1) AddManualHeader(tableS2, $"{currFY}\nQ1");
                if (showQ2) AddManualHeader(tableS2, $"{currFY}\nQ2");
                if (showQ3) AddManualHeader(tableS2, $"{currFY}\nQ3");
                if (showQ4) AddManualHeader(tableS2, $"{currFY}\nQ4");
                AddManualHeader(tableS2, $"{currFY} YTD");

                AddCell(tableS2, data.Seat_OTIF_Performance_YTD_PreviousYear);
                AddCell(tableS2, data.Seat_OTIF_Performance_Target);
                if (showQ1) AddCell(tableS2, data.Seat_OTIF_Performance_CurrentYear_Q1);
                if (showQ2) AddCell(tableS2, data.Seat_OTIF_Performance_CurrentYear_Q2);
                if (showQ3) AddCell(tableS2, data.Seat_OTIF_Performance_CurrentYear_Q3);
                if (showQ4) AddCell(tableS2, data.Seat_OTIF_Performance_CurrentYear_Q4);
                AddCell(tableS2, data.Seat_OTIF_Performance_YTD_CurrentYear);

                AddTableSection(doc, "2. On Time in Full Delivery", fontBold, tableS2);

                // --- 3. CSAT ---
                Table tableS3 = new Table(UnitValue.CreatePercentArray(GetWidths(0, 4f, 1.5f, 3, dynCols, 1))).UseAllAvailableWidth();
                AddHeaders(tableS3, qFlags, new[] { "Metrics", $"{prevFY} YTD", "Baseline", "Target" }, null, currQs, new[] { $"{currFY} YTD" });

                AddRow(tableS3, qFlags, new object[] { "Nos of Request sent", data.Seat_CSAT_ReqSent_YTD_PreviousYear, data.Seat_CSAT_ReqSent_Baseline, data.Seat_CSAT_ReqSent_Target },
                    new object[] { data.Seat_CSAT_ReqSent_CurrentYear_Q1, data.Seat_CSAT_ReqSent_CurrentYear_Q2, data.Seat_CSAT_ReqSent_CurrentYear_Q3, data.Seat_CSAT_ReqSent_CurrentYear_Q4 },
                    new object[] { data.Seat_CSAT_ReqSent_YTD_CurrentYear });

                AddRow(tableS3, qFlags, new object[] { "Nos of response received", data.Seat_CSAT_RespRecvd_YTD_PreviousYear, data.Seat_CSAT_RespRecvd_Baseline, data.Seat_CSAT_RespRecvd_Target },
                    new object[] { data.Seat_CSAT_RespRecvd_CurrentYear_Q1, data.Seat_CSAT_RespRecvd_CurrentYear_Q2, data.Seat_CSAT_RespRecvd_CurrentYear_Q3, data.Seat_CSAT_RespRecvd_CurrentYear_Q4 },
                    new object[] { data.Seat_CSAT_RespRecvd_YTD_CurrentYear });

                AddRow(tableS3, qFlags, new object[] { "Promoter", data.Seat_CSAT_Promoter_YTD_PreviousYear, data.Seat_CSAT_Promoter_Baseline, data.Seat_CSAT_Promoter_Target },
                    new object[] { data.Seat_CSAT_Promoter_CurrentYear_Q1, data.Seat_CSAT_Promoter_CurrentYear_Q2, data.Seat_CSAT_Promoter_CurrentYear_Q3, data.Seat_CSAT_Promoter_CurrentYear_Q4 },
                    new object[] { data.Seat_CSAT_Promoter_YTD_CurrentYear });

                AddRow(tableS3, qFlags, new object[] { "Detractor", data.Seat_CSAT_Detractor_YTD_PreviousYear, data.Seat_CSAT_Detractor_Baseline, data.Seat_CSAT_Detractor_Target },
                    new object[] { data.Seat_CSAT_Detractor_CurrentYear_Q1, data.Seat_CSAT_Detractor_CurrentYear_Q2, data.Seat_CSAT_Detractor_CurrentYear_Q3, data.Seat_CSAT_Detractor_CurrentYear_Q4 },
                    new object[] { data.Seat_CSAT_Detractor_YTD_CurrentYear });

                var pSeatNps = new Paragraph().Add(new Text("NPS%= ").SetFont(fontRegular)).Add(new Text("\n(Nos of Promoters-Nos of Detractors)/\n(Nos of response Received) X 100").SetFont(fontRegular));
                AddRow(tableS3, qFlags, new object[] { pSeatNps, data.Seat_CSAT_NPS_YTD_PreviousYear, data.Seat_CSAT_NPS_Baseline, data.Seat_CSAT_NPS_Target },
                    new object[] { data.Seat_CSAT_NPS_CurrentYear_Q1, data.Seat_CSAT_NPS_CurrentYear_Q2, data.Seat_CSAT_NPS_CurrentYear_Q3, data.Seat_CSAT_NPS_CurrentYear_Q4 },
                    new object[] { data.Seat_CSAT_NPS_YTD_CurrentYear });

                AddTableSection(doc, "3. CSAT", fontBold, tableS3);

                // --- 4. CSO ---
                Table tableS4 = new Table(UnitValue.CreatePercentArray(GetWidths(0, 4f, 1.5f, 1, dynCols, 1))).UseAllAvailableWidth();
                AddHeaders(tableS4, qFlags, new[] { "Metric", $"{prevFY} YTD" }, null, currQs, new[] { $"{currFY} YTD" });

                AddRow(tableS4, qFlags, new object[] { "Total CSO logged", data.Seat_CSO_TotalLogged_YTD_PreviousYear },
                    new object[] { data.Seat_CSO_TotalLogged_CurrentYear_Q1, data.Seat_CSO_TotalLogged_CurrentYear_Q2, data.Seat_CSO_TotalLogged_CurrentYear_Q3, data.Seat_CSO_TotalLogged_CurrentYear_Q4 },
                    new object[] { data.Seat_CSO_TotalLogged_YTD_CurrentYear });
                AddRow(tableS4, qFlags, new object[] { "Total A Class CSO's Logged", data.Seat_CSO_TotalAClass_YTD_PreviousYear },
                    new object[] { data.Seat_CSO_TotalAClass_CurrentYear_Q1, data.Seat_CSO_TotalAClass_CurrentYear_Q2, data.Seat_CSO_TotalAClass_CurrentYear_Q3, data.Seat_CSO_TotalAClass_CurrentYear_Q4 },
                    new object[] { data.Seat_CSO_TotalAClass_YTD_CurrentYear });
                AddRow(tableS4, qFlags, new object[] { "A Class Closed CSO's as on date", data.Seat_CSO_AClassClosed_YTD_PreviousYear },
                    new object[] { data.Seat_CSO_AClassClosed_CurrentYear_Q1, data.Seat_CSO_AClassClosed_CurrentYear_Q2, data.Seat_CSO_AClassClosed_CurrentYear_Q3, data.Seat_CSO_AClassClosed_CurrentYear_Q4 },
                    new object[] { data.Seat_CSO_AClassClosed_YTD_CurrentYear });
                AddRow(tableS4, qFlags, new object[] { "A Class Closed CSO's <45 Days", data.Seat_CSO_AClassClosedLess45_YTD_PreviousYear },
                    new object[] { data.Seat_CSO_AClassClosedLess45_CurrentYear_Q1, data.Seat_CSO_AClassClosedLess45_CurrentYear_Q2, data.Seat_CSO_AClassClosedLess45_CurrentYear_Q3, data.Seat_CSO_AClassClosedLess45_CurrentYear_Q4 },
                    new object[] { data.Seat_CSO_AClassClosedLess45_YTD_CurrentYear });
                AddRow(tableS4, qFlags, new object[] { "Percentage Closure of A Class CSO under 45 days", data.Seat_CSO_AClassClosedUnder45_YTD_PreviousYear },
                    new object[] { data.Seat_CSO_AClassClosedUnder45_CurrentYear_Q1, data.Seat_CSO_AClassClosedUnder45_CurrentYear_Q2, data.Seat_CSO_AClassClosedUnder45_CurrentYear_Q3, data.Seat_CSO_AClassClosedUnder45_CurrentYear_Q4 },
                    new object[] { data.Seat_CSO_AClassClosedUnder45_YTD_CurrentYear });

                AddTableSection(doc, "4. CSO", fontBold, tableS4);

                // --- 5. SPM ---
                Table tableS5 = new Table(UnitValue.CreatePercentArray(GetWidths(0.7f, 4f, 1.5f, 1, dynCols))).UseAllAvailableWidth();
                AddHeaders(tableS5, qFlags, new[] { "S.N.", "Supplier Name", $"{prevFY} Q4" }, null, currQs);

                var seatSPMCounter = 1;

                if (data.Seat_SPM_Supp1 > 0)
                {
                    var supp1 = await _vendorRepository.GetByIdAsync(data.Seat_SPM_Supp1.Value);
                    AddRow(tableS5, qFlags, new object[] { seatSPMCounter.ToString(), supp1?.Name ?? "", data.Seat_SPM_Supp1_PreviousYear_Q4 }, new object[] { data.Seat_SPM_Supp1_CurrentYear_Q1, data.Seat_SPM_Supp1_CurrentYear_Q2, data.Seat_SPM_Supp1_CurrentYear_Q3, data.Seat_SPM_Supp1_CurrentYear_Q4 }, centerFirstCol: true);
                }
                if (data.Seat_SPM_Supp2 > 0)
                {
                    var supp2 = await _vendorRepository.GetByIdAsync(data.Seat_SPM_Supp2.Value);
                    AddRow(tableS5, qFlags, new object[] { seatSPMCounter.ToString(), supp2?.Name ?? "", data.Seat_SPM_Supp2_PreviousYear_Q4 }, new object[] { data.Seat_SPM_Supp2_CurrentYear_Q1, data.Seat_SPM_Supp2_CurrentYear_Q2, data.Seat_SPM_Supp2_CurrentYear_Q3, data.Seat_SPM_Supp2_CurrentYear_Q4 }, centerFirstCol: true);
                }
                if (data.Seat_SPM_Supp3 > 0)
                {
                    var supp3 = await _vendorRepository.GetByIdAsync(data.Seat_SPM_Supp3.Value);
                    AddRow(tableS5, qFlags, new object[] { seatSPMCounter.ToString(), supp3?.Name ?? "", data.Seat_SPM_Supp3_PreviousYear_Q4 }, new object[] { data.Seat_SPM_Supp3_CurrentYear_Q1, data.Seat_SPM_Supp3_CurrentYear_Q2, data.Seat_SPM_Supp3_CurrentYear_Q3, data.Seat_SPM_Supp3_CurrentYear_Q4 }, centerFirstCol: true);
                }
                if (data.Seat_SPM_Supp4 > 0)
                {
                    var supp4 = await _vendorRepository.GetByIdAsync(data.Seat_SPM_Supp4.Value);
                    AddRow(tableS5, qFlags, new object[] { seatSPMCounter.ToString(), supp4?.Name ?? "", data.Seat_SPM_Supp4_PreviousYear_Q4 }, new object[] { data.Seat_SPM_Supp4_CurrentYear_Q1, data.Seat_SPM_Supp4_CurrentYear_Q2, data.Seat_SPM_Supp4_CurrentYear_Q3, data.Seat_SPM_Supp4_CurrentYear_Q4 }, centerFirstCol: true);
                }
                if (data.Seat_SPM_Supp5 > 0)
                {
                    var supp5 = await _vendorRepository.GetByIdAsync(data.Seat_SPM_Supp5.Value);
                    AddRow(tableS5, qFlags, new object[] { seatSPMCounter.ToString(), supp5?.Name ?? "", data.Seat_SPM_Supp5_PreviousYear_Q4 }, new object[] { data.Seat_SPM_Supp5_CurrentYear_Q1, data.Seat_SPM_Supp5_CurrentYear_Q2, data.Seat_SPM_Supp5_CurrentYear_Q3, data.Seat_SPM_Supp5_CurrentYear_Q4 }, centerFirstCol: true);
                }

                AddTableSection(doc, "5. SPM", fontBold, tableS5);

                // --- 6. IQA ---
                var iqaDesc = new Paragraph("5 Mn projects executed, and Installation completed > 30 days are considered.").SetFont(fontRegular).SetFontSize(10);
                Table tableS6 = new Table(UnitValue.CreatePercentArray(GetWidths(0, 4f, 1.5f, 2, dynCols, 1))).UseAllAvailableWidth();
                AddHeaders(tableS6, qFlags, new[] { "", $"YTD ({prevFY})", "Target Sigma" }, null, currQs, new[] { $"YTD ({currFY})" });

                AddRow(tableS6, qFlags, new object[] { "Total No. Of Sites", data.Seat_IQA_TotalSites_YTD_PreviousYear, data.Seat_IQA_TotalSites_Target },
                    new object[] { data.Seat_IQA_TotalSites_CurrentYear_Q1, data.Seat_IQA_TotalSites_CurrentYear_Q2, data.Seat_IQA_TotalSites_CurrentYear_Q3, data.Seat_IQA_TotalSites_CurrentYear_Q4 },
                    new object[] { data.Seat_IQA_TotalSites_YTD_CurrentYear });

                AddRow(tableS6, qFlags, new object[] { "No. Of Sites Completed", data.Seat_IQA_SitesCompleted_YTD_PreviousYear, data.Seat_IQA_SitesCompleted_Target },
                    new object[] { data.Seat_IQA_SitesCompleted_CurrentYear_Q1, data.Seat_IQA_SitesCompleted_CurrentYear_Q2, data.Seat_IQA_SitesCompleted_CurrentYear_Q3, data.Seat_IQA_SitesCompleted_CurrentYear_Q4 },
                    new object[] { data.Seat_IQA_SitesCompleted_YTD_CurrentYear });

                AddRow(tableS6, qFlags, new object[] { "No. Of IQA Audit Completed", data.Seat_IQA_AuditsCompleted_YTD_PreviousYear, data.Seat_IQA_AuditsCompleted_Target },
                    new object[] { data.Seat_IQA_AuditsCompleted_CurrentYear_Q1, data.Seat_IQA_AuditsCompleted_CurrentYear_Q2, data.Seat_IQA_AuditsCompleted_CurrentYear_Q3, data.Seat_IQA_AuditsCompleted_CurrentYear_Q4 },
                    new object[] { data.Seat_IQA_AuditsCompleted_YTD_CurrentYear });

                AddRow(tableS6, qFlags, new object[] { "% of IQA Completed", data.Seat_IQA_PercCompleted_YTD_PreviousYear, data.Seat_IQA_PercCompleted_Target },
                    new object[] { data.Seat_IQA_PercCompleted_CurrentYear_Q1, data.Seat_IQA_PercCompleted_CurrentYear_Q2, data.Seat_IQA_PercCompleted_CurrentYear_Q3, data.Seat_IQA_PercCompleted_CurrentYear_Q4 },
                    new object[] { data.Seat_IQA_PercCompleted_YTD_CurrentYear });

                AddRow(tableS6, qFlags, new object[] { "IQA Average Sigma", data.Seat_IQA_AvgSigma_YTD_PreviousYear, data.Seat_IQA_AvgSigma_Target },
                    new object[] { data.Seat_IQA_AvgSigma_CurrentYear_Q1, data.Seat_IQA_AvgSigma_CurrentYear_Q2, data.Seat_IQA_AvgSigma_CurrentYear_Q3, data.Seat_IQA_AvgSigma_CurrentYear_Q4 },
                    new object[] { data.Seat_IQA_AvgSigma_YTD_CurrentYear });

                AddTableSection(doc, "6. IQA (Installed Quality Audit)", fontBold, tableS6, iqaDesc);

                doc.Close();
                return File(stream.ToArray(), "application/pdf", $"AHP_Note_{currFY}.pdf");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // =========================================================
    // HELPER METHODS (Including New AddTableSection)
    // =========================================================

    // NEW HELPER: Wraps Title + Table + Description in a KeepTogether DIV
    private void AddTableSection(Document doc, string title, PdfFont font, Table table, Paragraph extraDesc = null)
    {
        Div sectionDiv = new Div().SetKeepTogether(true).SetMarginBottom(10);

        // 1. Add Title
        if (!string.IsNullOrEmpty(title))
        {
            var p = new Paragraph().SetFont(font).SetFontSize(11).SetMarginBottom(2);
            int spaceIndex = title.IndexOf(' ');
            if (spaceIndex > 0 && char.IsDigit(title[0]))
            {
                p.Add(new Text(title.Substring(0, spaceIndex + 1)));
                p.Add(new Text(title.Substring(spaceIndex + 1)).SetUnderline(1f, -2f));
            }
            else
            {
                p.Add(new Text(title).SetUnderline(1f, -2f));
            }
            sectionDiv.Add(p);
        }

        // 2. Add Extra Description (if any)
        if (extraDesc != null)
        {
            sectionDiv.Add(extraDesc);
        }

        // 3. Add Table
        sectionDiv.Add(table);

        // 4. Add Section to Doc
        doc.Add(sectionDiv);
    }

    private void AddHeaderCell(Table table, string text)
    {
        table.AddHeaderCell(new Cell()
            .Add(new Paragraph(text ?? "").SetFontSize(9))
            .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetVerticalAlignment(VerticalAlignment.MIDDLE)
            .SetFontSize(9));
    }

    private void AddCell(Table table, object value, TextAlignment alignment = TextAlignment.LEFT)
    {
        Cell cell = new Cell()
            .SetFontSize(8)
            .SetTextAlignment(alignment)
            .SetVerticalAlignment(VerticalAlignment.MIDDLE);

        if (value is IBlockElement block)
        {
            cell.Add(block);
        }
        else
        {
            cell.Add(new Paragraph(value?.ToString() ?? ""));
        }

        table.AddCell(cell);
    }

    private void AddHeaders(Table table, bool[] qFlags, string[] fixedPre, string[] fixedQs, string[] dynamicQs, string[] fixedPost = null)
    {
        if (fixedPre != null) foreach (var h in fixedPre) AddHeaderCell(table, h);
        if (fixedQs != null) foreach (var h in fixedQs) AddHeaderCell(table, h);
        if (dynamicQs != null)
        {
            for (int i = 0; i < 4; i++) { if (qFlags[i]) AddHeaderCell(table, dynamicQs[i]); }
        }
        if (fixedPost != null) foreach (var h in fixedPost) AddHeaderCell(table, h);
    }

    private void AddRow(Table table, bool[] qFlags, object[] fixedPre, object[] dynamicQs, object[] fixedPost = null, bool centerFirstCol = false)
    {
        if (fixedPre != null)
        {
            for (int i = 0; i < fixedPre.Length; i++)
            {
                var align = (centerFirstCol && i == 0) ? TextAlignment.CENTER : TextAlignment.LEFT;
                AddCell(table, fixedPre[i], align);
            }
        }
        if (dynamicQs != null)
        {
            for (int i = 0; i < 4; i++) { if (qFlags[i]) AddCell(table, dynamicQs[i]); }
        }
        if (fixedPost != null) foreach (var c in fixedPost) AddCell(table, c);
    }
}