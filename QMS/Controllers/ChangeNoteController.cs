using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
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
                document.SetMargins(15, 15, 15, 15); // Tight margins like the image

                // FONTS
                PdfFont fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont fontRegular = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                // =========================================================
                // PAGE 1: CHANGE NOTE DETAILS
                // =========================================================

                // =========================================================
                // 1. UNIFIED HEADER GRID (The only way to guarantee alignment)
                // =========================================================
                // We define 4 columns to handle the layout perfectly:
                // Col 1: Logo (Left)
                // Col 2: Title Space (Center-Left)
                // Col 3: Metadata Labels (Right)
                // Col 4: Metadata Values (Far Right)
                float[] headerWidths = { 1.5f, 8f, 2f, 2.5f };
                Table headerTable = new Table(UnitValue.CreatePercentArray(headerWidths)).UseAllAvailableWidth();

                // ----------------------------------------------------------
                // ROW 1: Logo + Title   |   Document No
                // ----------------------------------------------------------

                // Col 1: Logo
                string logoPath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "images", "wipro-logo.png");
                Cell logoCell = new Cell().SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                if (System.IO.File.Exists(logoPath))
                {
                    ImageData imageData = ImageDataFactory.Create(logoPath);
                    // Adjusted scale to fit nicely in the row
                    logoCell.Add(new Image(imageData).ScaleToFit(50, 30));
                }
                headerTable.AddCell(logoCell);

                // Col 2: Title (Centered in its column)
                Cell titleCell = new Cell().SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.CENTER);
                titleCell.Add(new Paragraph("CHANGE NOTE").SetFont(fontBold).SetFontSize(16));
                titleCell.Add(new Paragraph("Proposal for change in product").SetFont(fontRegular).SetFontSize(9));
                headerTable.AddCell(titleCell);

                // Col 3: Label
                headerTable.AddCell(CreateMetaLabel("Document No. :", fontBold));
                // Col 4: Value
                headerTable.AddCell(CreateMetaValue(model.DocumentNo, fontRegular));


                // ----------------------------------------------------------
                // ROW 2: Spacer (Empty) |   Date of Issue
                // ----------------------------------------------------------
                // This empty cell pushes the description down, creating the gap you see in the image
                // We align it with "Date of Issue"

                headerTable.AddCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetHeight(10)); // Span 2 cols, fixed height spacer
                headerTable.AddCell(CreateMetaLabel("Date of Issue :", fontBold));
                headerTable.AddCell(CreateMetaValue(model.DateOfIssue?.ToString("dd-MM-yyyy"), fontRegular));


                // ----------------------------------------------------------
                // ROW 3: Description    |   Revision No
                // ----------------------------------------------------------

                // Left Side: Description (Spanning Logo & Title columns)
                // We use a Paragraph to format "Description: value"
                Paragraph descPara = new Paragraph()
                    .Add(new Text("Description:      ").SetFont(fontBold).SetFontSize(9)) // Fixed spaces for alignment
                    .Add(new Text(model.Description ?? "").SetFont(fontRegular).SetFontSize(9));

                headerTable.AddCell(new Cell(1, 2).Add(descPara).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.BOTTOM));

                // Right Side
                headerTable.AddCell(CreateMetaLabel("Revision No. :", fontBold));
                headerTable.AddCell(CreateMetaValue(model.RevisionNo, fontRegular));


                // ----------------------------------------------------------
                // ROW 4: Vendor         |   Sheet No  <-- THIS LOCKS THEM TOGETHER
                // ----------------------------------------------------------

                // Left Side: Vendor
                Paragraph vendorPara = new Paragraph()
                    .Add(new Text("Vendor       :      ").SetFont(fontBold).SetFontSize(9))
                    .Add(new Text(model.VendorName ?? "").SetFont(fontRegular).SetFontSize(9));

                headerTable.AddCell(new Cell(1, 2).Add(vendorPara).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.BOTTOM));

                // Right Side
                headerTable.AddCell(CreateMetaLabel("Sheet No. :", fontBold));
                headerTable.AddCell(CreateMetaValue("1 of 2", fontRegular));


                // ADD HEADER TO MAIN CONTAINER
                Table mainContainer = new Table(1).UseAllAvailableWidth().SetBorder(new SolidBorder(1));
                mainContainer.AddCell(new Cell().Add(headerTable).SetBorder(Border.NO_BORDER).SetPadding(5));

                // --- 3. MAIN GRID (Sr No, From, To, Category) ---
                // This mimics the tall grid in the image
                float[] columnWidths = { 0.5f, 4, 4, 1 };
                Table gridTable = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();

                // Header Row
                gridTable.AddHeaderCell(CreateGridHeader("Sr.\nNo."));
                gridTable.AddHeaderCell(CreateGridHeader("CHANGE FROM"));
                gridTable.AddHeaderCell(CreateGridHeader("CHANGE TO"));
                gridTable.AddHeaderCell(CreateGridHeader("CATEG\nORY*"));

                // Data Rows
                int srNo = 1;
                foreach (var item in model.Items)
                {
                    gridTable.AddCell(CreateGridCell(srNo++.ToString(), fontRegular, TextAlignment.CENTER));
                    gridTable.AddCell(CreateGridCell(item.ChangeFrom, fontRegular, TextAlignment.LEFT));
                    gridTable.AddCell(CreateGridCell(item.ChangeTo, fontRegular, TextAlignment.LEFT));
                    gridTable.AddCell(CreateGridCell(item.Category, fontRegular, TextAlignment.CENTER));
                }

                // FILLER ROW (To push borders down to fill the page like the image)
                // We calculate a fixed height or add a giant empty cell
                Cell emptyCell = new Cell().SetHeight(350).SetBorder(new SolidBorder(1)); // Adjust 350 based on page needs
                gridTable.AddCell(emptyCell.Clone(true)); // Sr
                gridTable.AddCell(emptyCell.Clone(true)); // From
                gridTable.AddCell(emptyCell.Clone(true)); // To
                gridTable.AddCell(emptyCell.Clone(true)); // Cat

                // Add Grid to Main Container (Removing padding to make borders touch)
                mainContainer.AddCell(new Cell().Add(gridTable).SetBorder(Border.NO_BORDER).SetPadding(0));


                // =========================================================
                // 4. REMARKS & LEGEND FOOTER
                // =========================================================
                Table bottomSection = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1 })).UseAllAvailableWidth();

                // ---------------------------------------------------------
                // LEFT COLUMN: REMARKS (Top) / LINE / COPY TO (Bottom)
                // ---------------------------------------------------------
                // We use a nested table to split the left side horizontally
                Cell leftContainer = new Cell().SetBorder(new SolidBorder(1)).SetPadding(0);

                Table leftNestedTable = new Table(1).UseAllAvailableWidth();

                // A. REMARKS CELL (With Bottom Border acting as the separator line)
                Cell remarksCell = new Cell().SetPadding(5).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1));
                remarksCell.Add(new Paragraph("REMARKS :").SetFont(fontBold).SetFontSize(9));
                remarksCell.Add(new Paragraph(model.Remarks ?? "\n").SetFont(fontRegular).SetFontSize(9));
                remarksCell.SetMinHeight(50); // Optional: Give remarks some guaranteed height
                leftNestedTable.AddCell(remarksCell);

                // B. COPY TO CELL (No Border, just padding)
                Cell copyToCell = new Cell().SetPadding(5).SetBorder(Border.NO_BORDER);
                copyToCell.Add(new Paragraph("COPY TO :").SetFont(fontBold).SetFontSize(8).SetMarginBottom(5));

                // Boxes
                Table copyBoxTable = new Table(5).SetWidth(250);
                copyBoxTable.AddCell(CreateSquareBox("PMG"));
                copyBoxTable.AddCell(CreateSquareBox("LSG"));
                copyBoxTable.AddCell(CreateSquareBox("MATL"));
                copyBoxTable.AddCell(CreateSquareBox("QA"));
                copyBoxTable.AddCell(CreateSquareBox("PDG"));

                copyToCell.Add(copyBoxTable.SetMarginBottom(2));
                leftNestedTable.AddCell(copyToCell);

                // Add the nested table to the main left container
                leftContainer.Add(leftNestedTable);


                // ---------------------------------------------------------
                // RIGHT COLUMN: LEGEND (Stays the same)
                // ---------------------------------------------------------
                Cell rightContainer = new Cell().SetBorder(new SolidBorder(1)).SetPadding(5);
                rightContainer.Add(new Paragraph("* CHANGE NOTE CATEGORY").SetFont(fontBold).SetFontSize(8));
                rightContainer.Add(new Paragraph("A: EXTREMELY URGENT & CRITICAL\nTo be implemented with immediate effect").SetFont(fontRegular).SetFontSize(7).SetMarginTop(5));
                rightContainer.Add(new Paragraph("B: FOR PRODUCT UPGRADATION\nTo be implemented after existing inventory consumption.").SetFont(fontRegular).SetFontSize(7).SetMarginTop(5));

                bottomSection.AddCell(leftContainer);
                bottomSection.AddCell(rightContainer);

                mainContainer.AddCell(new Cell().Add(bottomSection).SetBorder(Border.NO_BORDER).SetPadding(0));


                // =========================================================
                // 5. SIGNATURE FOOTER (Fixed Separation & Date Position)
                // =========================================================

                // We apply a Top Border to this cell to create the distinct line you asked for
                Cell sigContainerCell = new Cell().SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(1));

                Table sigTable = new Table(UnitValue.CreatePercentArray(new float[] { 1 })).UseAllAvailableWidth();

                Cell sigCell = new Cell().SetPadding(5).SetBorder(Border.NO_BORDER);

                // [!] FIX: Signature and Date on separate lines
                sigCell.Add(new Paragraph("Signature : ").SetFont(fontRegular).SetFontSize(9).SetMarginBottom(2));
                sigCell.Add(new Paragraph($"Date       : {model.SignatureDate?.ToString("dd-MM-yyyy") ?? ""}").SetFont(fontRegular).SetFontSize(9));

                sigTable.AddCell(sigCell);
                sigContainerCell.Add(sigTable);

                mainContainer.AddCell(sigContainerCell);


                // --- Group Footer Line ---
                Table groupFooter = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1, 1, 1, 1 })).UseAllAvailableWidth();

                // Helper wrapper to simplify footer calls
                groupFooter.AddCell(CreateFooterText("Group   : Product Design", fontBold));
                groupFooter.AddCell(CreateFooterText("Quality Assurance", fontBold));
                groupFooter.AddCell(CreateFooterText("Materials", fontBold));
                groupFooter.AddCell(CreateFooterText("PMG", fontBold));
                groupFooter.AddCell(CreateFooterText("Lighting Technology", fontBold, TextAlignment.RIGHT));

                // Add Footer to Main Container with Top Border
                mainContainer.AddCell(new Cell().Add(groupFooter).SetBorderTop(new SolidBorder(1)).SetPadding(5));

                document.Add(mainContainer);

                // =========================================================
                // PAGE 2: IMPLEMENTATION
                // =========================================================
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                Table page2Container = new Table(1).UseAllAvailableWidth().SetBorder(new SolidBorder(1));

                // Header
                page2Container.AddCell(new Cell().Add(new Paragraph("CHANGE NOTE IMPLEMENTATION").SetFont(fontBold).SetFontSize(11).SetTextAlignment(TextAlignment.CENTER)).SetPadding(10).SetBorder(Border.NO_BORDER));

                // Ref Info
                Table refTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 })).UseAllAvailableWidth();
                refTable.AddCell(CreateNoBorderCell($"Change Note Ref. No. {model.ChangeNoteRefNo}", fontRegular));
                refTable.AddCell(CreateNoBorderCell($"Date of Issue: {model.DateOfIssue?.ToString("dd-MM-yyyy")}", fontRegular));
                refTable.AddCell(CreateNoBorderCell($"Vendor / QA In charge : {model.User}", fontRegular)); // Spanning 2 cols logic if needed
                page2Container.AddCell(new Cell().Add(refTable).SetPadding(5).SetBorder(Border.NO_BORDER));

                // Implementation Grid
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
                    impTable.AddCell(CreateGridCell(imp.ProposedCutOffDate?.ToString("dd-MM-yyyy"), fontRegular, TextAlignment.CENTER));
                    impTable.AddCell(CreateGridCell(imp.ActualDate?.ToString("dd-MM-yyyy"), fontRegular, TextAlignment.CENTER));
                }

                // Empty Filler for Impl Grid
                Cell impEmpty = new Cell().SetHeight(200).SetBorder(new SolidBorder(1));
                impTable.AddCell(impEmpty.Clone(true));
                impTable.AddCell(impEmpty.Clone(true));
                impTable.AddCell(impEmpty.Clone(true));
                impTable.AddCell(impEmpty.Clone(true));
                impTable.AddCell(impEmpty.Clone(true));

                page2Container.AddCell(new Cell().Add(impTable).SetBorder(Border.NO_BORDER).SetPadding(0));

                // Final QA Remarks Section
                Table qaSection = new Table(1).UseAllAvailableWidth().SetHeight(250); // Large fixed height box
                Cell qaInner = new Cell().SetPadding(10);
                qaInner.Add(new Paragraph("FINAL REMARKS FROM QA").SetFont(fontRegular).SetFontSize(10));
                qaInner.Add(new Paragraph("\nTo. : Group manager : Product Design").SetFont(fontRegular).SetFontSize(10));
                qaInner.Add(new Paragraph("\nFrom. :").SetFont(fontRegular).SetFontSize(10));

                qaInner.Add(new Paragraph($"\n\nThe change has been implemented at {model.VendorName}           for all dispatches from date {model.UpdatedOn?.ToString("dd-MM-yyyy")}").SetFont(fontRegular).SetFontSize(10));
                qaInner.Add(new Paragraph("\nSignature").SetFont(fontRegular).SetFontSize(10));

                qaInner.Add(new Paragraph("Note:\nQA Team member will provide his final remarks as soon as the changes are implemented at vendor location and acknowledge to Product Design Group by sending a copy of this implementation note.\nThis is required to update the Technical Specification.")
                    .SetFont(fontRegular).SetFontSize(8).SetMarginTop(20));

                qaSection.AddCell(qaInner);
                page2Container.AddCell(new Cell().Add(qaSection).SetBorderTop(new SolidBorder(1)));

                document.Add(page2Container);

                document.Close();
                return File(stream.ToArray(), "application/pdf", $"ChangeNote_{model.DocumentNo}.pdf");
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    private Cell CreateMetaLabel(string text, PdfFont font)
    {
        return new Cell()
            .Add(new Paragraph(text).SetFont(font).SetFontSize(9))
            .SetBorder(Border.NO_BORDER)
            .SetVerticalAlignment(VerticalAlignment.BOTTOM) // Ensures it aligns with the text next to it
            .SetPaddingTop(5);
    }

    private Cell CreateMetaValue(string text, PdfFont font)
    {
        return new Cell()
            .Add(new Paragraph(text ?? "").SetFont(font).SetFontSize(9)) // Use Bold font here if you want values to be bold
            .SetBorder(Border.NO_BORDER)
            .SetVerticalAlignment(VerticalAlignment.BOTTOM)
            .SetPaddingTop(5);
    }

    private void AddMetaRow(Table table, string label, string value, PdfFont labelFont, PdfFont valueFont)
    {
        PdfFont fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont fontRegular = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        table.AddCell(new Cell().Add(new Paragraph(label).SetFont(fontBold).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetPadding(1));
        table.AddCell(new Cell().Add(new Paragraph(value ?? "").SetFont(fontBold).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetPadding(1));
    }

    private Cell CreateNoBorderCell(string text, PdfFont font)
    {
        return new Cell().Add(new Paragraph(text ?? "").SetFont(font).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetPadding(2);
    }

    private Cell CreateGridHeader(string text)
    {
        return new Cell()
            .Add(new Paragraph(text).SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)).SetFontSize(8))
            .SetTextAlignment(TextAlignment.CENTER)
            .SetVerticalAlignment(VerticalAlignment.MIDDLE)
            .SetPadding(5)
            .SetBorder(new SolidBorder(1));
    }

    private Cell CreateGridCell(string text, PdfFont font, TextAlignment alignment)
    {
        return new Cell()
            .Add(new Paragraph(text ?? "").SetFont(font).SetFontSize(9))
            .SetTextAlignment(alignment)
            .SetPadding(5)
            .SetBorder(new SolidBorder(1));
    }

    private Cell CreateSquareBox(string text)
    {
        return new Cell()
            .Add(new Paragraph(text)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(8))
            .SetTextAlignment(TextAlignment.CENTER)
            .SetVerticalAlignment(VerticalAlignment.BOTTOM) // Text at bottom
            .SetHeight(35)                                  // Taller box (was 25)
            .SetBorder(new SolidBorder(1));
    }

    private Cell CreateFooterText(string text, PdfFont font, TextAlignment align = TextAlignment.LEFT)
    {
        return new Cell().Add(new Paragraph(text).SetFont(font).SetFontSize(7)).SetBorder(Border.NO_BORDER).SetTextAlignment(align);
    }
}
