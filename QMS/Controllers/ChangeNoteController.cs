using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QMS.Core.Models;
using QMS.Core.Repositories.VendorRepository;
using QMS.Core.Services.ChangeNoteService;

namespace QMS.Controllers;

public class ChangeNoteController : Controller
{
    private readonly IChangeNoteService _changeNoteService;
    private readonly IVendorRepository _vendorRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ChangeNoteController(IChangeNoteService changeNoteService,
                                IVendorRepository vendorRepository,
                                IWebHostEnvironment webHostEnvironment)
    {
        _changeNoteService = changeNoteService;
        _vendorRepository = vendorRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult ChangeNoteAsync()
    {
        return View();
    }

    public async Task<ActionResult> ChangeNoteListAsync()
    {
        var list = await _changeNoteService.GetChangeNotesListAsync();
        return Json(list);
    }

    public async Task<IActionResult> ChangeNoteDetailsAsync(int Id)
    {
        var model = new ChangeNoteViewModel();
        var vendorList = await _vendorRepository.GetListAsync();
        var vendors = vendorList.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Name
        })
        .ToList();
        ViewBag.VendorList = vendors;
        if (Id > 0)
        {
            model = await _changeNoteService.GetChangeNotesDetailsAsync(Id);
        }
        return View(model);
    }

    public async Task<ActionResult> InsertUpdateChangeNoteAsync(ChangeNoteViewModel model)
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
            var result = await _changeNoteService.UpdateChangeNoteAsync(model);
            return Json(result);
        }
        else
        {
            model.AddedBy = HttpContext.Session.GetInt32("UserId") ?? 0;
            model.AddedOn = DateTime.Now;
            var result = await _changeNoteService.InsertChangeNoteAsync(model);
            return Json(result);
        }
    }

    public async Task<ActionResult> DeleteChangeNoteAsync(int Id)
    {
        var result = await _changeNoteService.DeleteChangeNoteAsync(Id);
        return Json(result);
    }

    public async Task<ActionResult> ExportToPDFAsync(int id)
    {
        try
        {
            var model = await _changeNoteService.GetChangeNotesDetailsAsync(id);

            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                pdf.SetDefaultPageSize(PageSize.A4);

                Document document = new Document(pdf);
                document.SetMargins(15, 15, 15, 15);

                // FONTS
                PdfFont fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont fontRegular = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                // GLOBAL BORDER WIDTH (Thin)
                float borderW = 0.5f;

                // =========================================================
                // MAIN CONTAINER
                // =========================================================
                Table mainContainer = new Table(1).UseAllAvailableWidth().SetBorder(new SolidBorder(borderW));

                // =========================================================
                // 1. UNIFIED HEADER GRID (Logo, Title, Meta, Vendor)
                // =========================================================
                // Col 1: Logo | Col 2: Title (Wide) | Col 3: Label (Tight) | Col 4: Value
                float[] headerWidths = { 1.5f, 8f, 2f, 2.5f };
                Table headerTable = new Table(UnitValue.CreatePercentArray(headerWidths)).UseAllAvailableWidth();

                // --- ROW 1: Logo + Title | Document No ---

                // Col 1: Logo
                string logoPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "images", "wipro-logo.png");
                Cell logoCell = new Cell().SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                if (System.IO.File.Exists(logoPath))
                {
                    ImageData imageData = ImageDataFactory.Create(logoPath);
                    logoCell.Add(new Image(imageData).ScaleToFit(50, 30));
                }
                headerTable.AddCell(logoCell);

                // Col 2: Title
                Cell titleCell = new Cell().SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.CENTER);
                titleCell.Add(new Paragraph("CHANGE NOTE").SetFont(fontBold).SetFontSize(16));
                titleCell.Add(new Paragraph("Proposal for change in product").SetFont(fontRegular).SetFontSize(9));
                headerTable.AddCell(titleCell);

                // Col 3 & 4: Doc No
                headerTable.AddCell(CreateMetaLabel("Document No. :", fontBold));
                headerTable.AddCell(CreateMetaValue(model.DocumentNo, fontRegular));


                // --- ROW 2: Spacer | Date of Issue ---
                headerTable.AddCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetHeight(10)); // Spacer
                headerTable.AddCell(CreateMetaLabel("Date of Issue :", fontBold));
                headerTable.AddCell(CreateMetaValue(model.DateOfIssue?.ToString("dd/MM/yyyy"), fontRegular));


                // --- ROW 3: Description | Revision No ---
                Paragraph descPara = new Paragraph()
                    .Add(new Text("Description:      ").SetFont(fontBold).SetFontSize(9))
                    .Add(new Text(model.Description ?? "").SetFont(fontRegular).SetFontSize(9));
                headerTable.AddCell(new Cell(1, 2).Add(descPara).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.BOTTOM));

                headerTable.AddCell(CreateMetaLabel("Revision No. :", fontBold));
                headerTable.AddCell(CreateMetaValue(model.RevisionNo, fontRegular));


                // --- ROW 4: Vendor | Sheet No ---
                Paragraph vendorPara = new Paragraph()
                    .Add(new Text("Vendor       :      ").SetFont(fontBold).SetFontSize(9))
                    .Add(new Text(model.VendorName ?? "").SetFont(fontRegular).SetFontSize(9));
                headerTable.AddCell(new Cell(1, 2).Add(vendorPara).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.BOTTOM));

                headerTable.AddCell(CreateMetaLabel("Sheet No. :", fontBold));
                headerTable.AddCell(CreateMetaValue("1 of 2", fontRegular));

                // Add Header to Main
                mainContainer.AddCell(new Cell().Add(headerTable).SetBorder(Border.NO_BORDER).SetPadding(5));


                // =========================================================
                // 2. MAIN ITEMS GRID
                // =========================================================
                float[] columnWidths = { 0.5f, 4, 4, 1 };
                Table gridTable = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth().SetMarginTop(5);

                gridTable.AddHeaderCell(CreateGridHeader("Sr.\nNo."));
                gridTable.AddHeaderCell(CreateGridHeader("CHANGE FROM"));
                gridTable.AddHeaderCell(CreateGridHeader("CHANGE TO"));
                gridTable.AddHeaderCell(CreateGridHeader("CATEG\nORY*"));

                int srNo = 1;
                foreach (var item in model.Items)
                {
                    gridTable.AddCell(CreateGridCell(srNo++.ToString(), fontRegular, TextAlignment.CENTER));
                    gridTable.AddCell(CreateGridCell(item.ChangeFrom, fontRegular, TextAlignment.LEFT));
                    gridTable.AddCell(CreateGridCell(item.ChangeTo, fontRegular, TextAlignment.LEFT));
                    gridTable.AddCell(CreateGridCell(item.Category, fontRegular, TextAlignment.CENTER));
                }

                // Filler Row
                Cell emptyCell = new Cell().SetHeight(350).SetBorder(new SolidBorder(borderW));
                gridTable.AddCell(emptyCell.Clone(true));
                gridTable.AddCell(emptyCell.Clone(true));
                gridTable.AddCell(emptyCell.Clone(true));
                gridTable.AddCell(emptyCell.Clone(true));

                mainContainer.AddCell(new Cell().Add(gridTable).SetBorder(Border.NO_BORDER).SetPadding(0));


                // =========================================================
                // 3. REMARKS & LEGEND FOOTER
                // =========================================================
                Table bottomSection = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1 })).UseAllAvailableWidth();

                // Left Side
                Cell leftContainer = new Cell().SetBorder(new SolidBorder(borderW)).SetPadding(0);
                Table leftNestedTable = new Table(1).UseAllAvailableWidth();

                // Remarks (Bottom Border Line)
                Cell remarksCell = new Cell().SetPadding(5).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(borderW));
                remarksCell.Add(new Paragraph("REMARKS :").SetFont(fontBold).SetFontSize(9));
                remarksCell.Add(new Paragraph(model.Remarks ?? "\n").SetFont(fontRegular).SetFontSize(9));
                remarksCell.SetMinHeight(50);
                leftNestedTable.AddCell(remarksCell);

                // Copy To Section
                Cell copyToCell = new Cell().SetPadding(5).SetBorder(Border.NO_BORDER);
                copyToCell.Add(new Paragraph("COPY TO :").SetFont(fontBold).SetFontSize(8).SetMarginBottom(5));

                // Grid for the items (5 items + 4 spacers)
                float[] boxWidths = { 1, 0.2f, 1, 0.2f, 1, 0.2f, 1, 0.2f, 1 };
                Table copyBoxTable = new Table(UnitValue.CreatePercentArray(boxWidths)).SetWidth(280);

                string copyStr = (model.CopyTo ?? "").ToUpper();

                // Add items using the new Helper
                copyBoxTable.AddCell(CreateCheckBoxCell("PMG", copyStr.Contains("PMG")));
                copyBoxTable.AddCell(CreateEmptySpacer());

                copyBoxTable.AddCell(CreateCheckBoxCell("LSG", copyStr.Contains("LSG")));
                copyBoxTable.AddCell(CreateEmptySpacer());

                copyBoxTable.AddCell(CreateCheckBoxCell("MATL", copyStr.Contains("MATL")));
                copyBoxTable.AddCell(CreateEmptySpacer());

                copyBoxTable.AddCell(CreateCheckBoxCell("QA", copyStr.Contains("QA")));
                copyBoxTable.AddCell(CreateEmptySpacer());

                copyBoxTable.AddCell(CreateCheckBoxCell("PDG", copyStr.Contains("PDG")));

                copyToCell.Add(copyBoxTable.SetMarginBottom(2));
                leftNestedTable.AddCell(copyToCell);
                leftContainer.Add(leftNestedTable);

                // Right Side (Legend)
                Cell rightContainer = new Cell().SetBorder(new SolidBorder(borderW)).SetPadding(5);
                rightContainer.Add(new Paragraph("* CHANGE NOTE CATEGORY").SetFont(fontBold).SetFontSize(8));
                rightContainer.Add(new Paragraph("A: EXTREMELY URGENT & CRITICAL\nTo be implemented with immediate effect").SetFont(fontRegular).SetFontSize(7).SetMarginTop(5));
                rightContainer.Add(new Paragraph("B: FOR PRODUCT UPGRADATION\nTo be implemented after existing inventory consumption.").SetFont(fontRegular).SetFontSize(7).SetMarginTop(5));

                bottomSection.AddCell(leftContainer);
                bottomSection.AddCell(rightContainer);

                mainContainer.AddCell(new Cell().Add(bottomSection).SetBorder(Border.NO_BORDER).SetPadding(0));


                // =========================================================
                // 4. SIGNATURE & GROUP FOOTER
                // =========================================================
                Cell sigContainerCell = new Cell().SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(borderW));
                Table sigTable = new Table(UnitValue.CreatePercentArray(new float[] { 1 })).UseAllAvailableWidth();

                Cell sigCell = new Cell().SetPadding(5).SetBorder(Border.NO_BORDER);
                sigCell.Add(new Paragraph("Signature : ").SetFont(fontRegular).SetFontSize(9).SetMarginBottom(10));
                sigCell.Add(new Paragraph($"Date        :  {model.SignatureDate?.ToString("dd/MM/yyyy") ?? ""}").SetFont(fontRegular).SetFontSize(9));

                sigTable.AddCell(sigCell);
                sigContainerCell.Add(sigTable);
                mainContainer.AddCell(sigContainerCell);

                // Group Footer (Visual Spacing)
                // A. Get Data and Split
                string groupData = model.ChangeNoteGroup ?? ""; // e.g. "Materials,PMG"
                string[] groups = groupData.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // B. Create Paragraph
                Paragraph groupPara = new Paragraph().SetFontSize(9);

                // Add Static Label "Group :"
                groupPara.Add(new Text("Group     :    ").SetFont(fontBold));

                // Loop through groups and add them
                for (int i = 0; i < groups.Length; i++)
                {
                    // Add the Group Name (Trimmed to remove accidental spaces around comma)
                    groupPara.Add(new Text(groups[i].Trim()).SetFont(fontBold));

                    // Add 10 Spaces if it is NOT the last item
                    if (i < groups.Length - 1)
                    {
                        // "10 space" as requested
                        groupPara.Add(new Text("          "));
                    }
                }

                // C. Add to Table Container
                Table groupFooter = new Table(1).UseAllAvailableWidth();
                groupFooter.AddCell(new Cell().Add(groupPara).SetBorder(Border.NO_BORDER));

                // Add to Main Container
                mainContainer.AddCell(new Cell().Add(groupFooter).SetBorder(Border.NO_BORDER).SetPadding(5));

                document.Add(mainContainer);

                // =========================================================
                // PAGE 2
                // =========================================================
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                Table page2Container = new Table(1).UseAllAvailableWidth().SetBorder(new SolidBorder(borderW));

                page2Container.AddCell(new Cell().Add(new Paragraph("CHANGE NOTE IMPLEMENTATION").SetFont(fontBold).SetFontSize(11).SetTextAlignment(TextAlignment.CENTER)).SetPadding(10).SetBorder(Border.NO_BORDER));

                Table refTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 })).UseAllAvailableWidth();
                refTable.AddCell(CreateNoBorderCell($"Change Note Ref. No.: {model.ChangeNoteRefNo}", fontRegular));
                refTable.AddCell(CreateNoBorderCell($"Date of Issue: {model.DateOfIssue?.ToString("dd/MM/yyyy")}", fontRegular));
                page2Container.AddCell(new Cell().Add(refTable).SetPadding(5).SetBorder(Border.NO_BORDER));

                Table vendorTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 })).UseAllAvailableWidth();
                vendorTable.AddCell(CreateNoBorderCell($"Vendor / QA In charge: {model.VendorQAInChargeName}", fontRegular));
                page2Container.AddCell(new Cell().Add(vendorTable).SetPadding(5).SetBorder(Border.NO_BORDER));

                float[] impColWidths = { 0.5f, 4, 3, 1.5f, 1.5f };
                Table impTable = new Table(UnitValue.CreatePercentArray(impColWidths)).UseAllAvailableWidth().SetMarginTop(10);
                impTable.AddHeaderCell(CreateGridHeader("Sr.\nNo."));
                impTable.AddHeaderCell(CreateGridHeader("ACTION PLANNED"));
                impTable.AddHeaderCell(CreateGridHeader("WHO WILL DO"));
                impTable.AddHeaderCell(CreateGridHeader("PROPOSED CUT OFF DATE"));
                impTable.AddHeaderCell(CreateGridHeader("ACTUAL\nDATE"));

                int impSr = 1;
                foreach (var imp in model.ImplementationItems)
                {
                    impTable.AddCell(CreateGridCell(impSr++.ToString(), fontRegular, TextAlignment.CENTER));
                    impTable.AddCell(CreateGridCell(imp.ActionPlanned, fontRegular, TextAlignment.LEFT));
                    impTable.AddCell(CreateGridCell(imp.WhoWillDo, fontRegular, TextAlignment.LEFT));
                    impTable.AddCell(CreateGridCell(imp.ProposedCutOffDate?.ToString("dd/MM/yyyy"), fontRegular, TextAlignment.CENTER));
                    impTable.AddCell(CreateGridCell(imp.ActualDate?.ToString("dd/MM/yyyy"), fontRegular, TextAlignment.CENTER));
                }

                Cell impEmpty = new Cell().SetHeight(200).SetBorder(new SolidBorder(borderW));
                impTable.AddCell(impEmpty.Clone(true)); impTable.AddCell(impEmpty.Clone(true)); impTable.AddCell(impEmpty.Clone(true)); impTable.AddCell(impEmpty.Clone(true)); impTable.AddCell(impEmpty.Clone(true));

                page2Container.AddCell(new Cell().Add(impTable).SetBorder(Border.NO_BORDER).SetPadding(0));

                Table qaSection = new Table(1).UseAllAvailableWidth().SetHeight(250);
                Cell qaInner = new Cell().SetPadding(10);
                qaInner.Add(new Paragraph("FINAL REMARKS FROM QA").SetFont(fontRegular).SetFontSize(10));
                qaInner.Add(new Paragraph("\nTo. : Group manager : Product Design").SetFont(fontRegular).SetFontSize(10));
                qaInner.Add(new Paragraph("\nFrom. :").SetFont(fontRegular).SetFontSize(10));
                qaInner.Add(new Paragraph($"\n\nThe change has been implemented at {model.VendorName}           for all dispatches from date {model.UpdatedOn?.ToString("dd-MM-yyyy")}").SetFont(fontRegular).SetFontSize(10));
                qaInner.Add(new Paragraph("\nSignature").SetFont(fontRegular).SetFontSize(10));
                qaInner.Add(new Paragraph("Note:\nQA Team member will provide his final remarks as soon as the changes are implemented at vendor location and acknowledge to Product Design Group by sending a copy of this implementation note.\nThis is required to update the Technical Specification.").SetFont(fontRegular).SetFontSize(8).SetMarginTop(20));

                qaSection.AddCell(qaInner);
                page2Container.AddCell(new Cell().Add(qaSection).SetBorderTop(new SolidBorder(borderW)));

                document.Add(page2Container);

                document.Close();
                return File(stream.ToArray(), "application/pdf", $"ChangeNote_{model.DocumentNo}.pdf");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private Cell CreateMetaLabel(string text, PdfFont font)
    {
        return new Cell()
            .Add(new Paragraph(text).SetFont(font).SetFontSize(9))
            .SetBorder(Border.NO_BORDER)
            .SetVerticalAlignment(VerticalAlignment.BOTTOM)
            .SetPaddingTop(5);
    }

    private Cell CreateMetaValue(string text, PdfFont font)
    {
        return new Cell()
            .Add(new Paragraph(text ?? "").SetFont(font).SetFontSize(9))
            .SetBorder(Border.NO_BORDER)
            .SetVerticalAlignment(VerticalAlignment.BOTTOM)
            .SetPaddingTop(5);
    }

    private Cell CreateGridHeader(string text)
    {
        return new Cell()
            .Add(new Paragraph(text).SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)).SetFontSize(8))
            .SetTextAlignment(TextAlignment.CENTER)
            .SetVerticalAlignment(VerticalAlignment.MIDDLE)
            .SetPadding(5)
            .SetBorder(new SolidBorder(0.5f));
    }

    private Cell CreateGridCell(string text, PdfFont font, TextAlignment alignment)
    {
        return new Cell()
            .Add(new Paragraph(text ?? "").SetFont(font).SetFontSize(9))
            .SetTextAlignment(alignment)
            .SetPadding(5)
            .SetBorder(new SolidBorder(0.5f));
    }

    private Cell CreateCheckBoxCell(string text, bool isChecked)
    {
        Cell container = new Cell().SetBorder(Border.NO_BORDER).SetPadding(0);

        // [!] FIX: Use PointValue to strictly enforce the 12pt width
        Table innerTable = new Table(new UnitValue[] {
            UnitValue.CreatePointValue(25),  // Rigid 12pt for box
            UnitValue.CreatePointValue(45)   // 45pt for text (slightly wider)
        });

        // --- A. THE CHECKBOX ---
        Cell boxCell = new Cell()
            .SetHeight(12)
            .SetWidth(12)        // Redundant but good for safety
            .SetBorder(new SolidBorder(0.5f))
            .SetPadding(0)
            .SetVerticalAlignment(VerticalAlignment.MIDDLE)
            .SetTextAlignment(TextAlignment.CENTER);

        if (isChecked)
        {
            PdfFont dingbats = PdfFontFactory.CreateFont(StandardFonts.ZAPFDINGBATS);
            Paragraph mark = new Paragraph("4")
                .SetFont(dingbats)
                .SetFontSize(10)
                .SetMargin(0)
                .SetFixedLeading(10)
                .SetPaddingBottom(2);
            boxCell.Add(mark);
        }
        innerTable.AddCell(boxCell);

        // --- B. THE LABEL ---
        Cell labelCell = new Cell()
            .Add(new Paragraph(text)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(8))
            .SetBorder(Border.NO_BORDER)
            .SetVerticalAlignment(VerticalAlignment.MIDDLE)
            .SetPaddingLeft(4);

        innerTable.AddCell(labelCell);

        container.Add(innerTable);
        return container;
    }

    private Cell CreateEmptySpacer()
    {
        return new Cell().SetBorder(Border.NO_BORDER);
    }

    private Cell CreateFooterText(string text, PdfFont font, TextAlignment align = TextAlignment.LEFT)
    {
        return new Cell().Add(new Paragraph(text).SetFont(font).SetFontSize(7)).SetBorder(Border.NO_BORDER).SetTextAlignment(align);
    }

    private Cell CreateNoBorderCell(string text, PdfFont font)
    {
        return new Cell().Add(new Paragraph(text ?? "").SetFont(font).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetPadding(2);
    }
}
