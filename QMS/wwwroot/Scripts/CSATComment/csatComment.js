var tabledata = [];
var table = '';
let filterStartCsatDate = moment().startOf('month').format('YYYY-MM-DD');
let filterEndCsatDate = moment().endOf('month').format('YYYY-MM-DD');

$(document).ready(function () {
    $('#dateRangeTexCsat').text(
        moment(filterStartCsatDate).format('MMMM D, YYYY') + ' - ' + moment(filterEndCsatDate).format('MMMM D, YYYY')
    );

    // Initialize Litepicker for date range selection
    const picker = new Litepicker({
        element: document.getElementById('customDateTriggerCsat'),
        singleMode: false,
        format: 'DD-MM-YYYY',
        numberOfMonths: 2,
        numberOfColumns: 2,
        dropdowns: {
            minYear: 2020,
            maxYear: null,
            months: true,
            years: true
        },
        plugins: ['ranges'],
        setup: (picker) => {
            picker.on('selected', (start, end) => {
                filterStartCsatDate = start.format('YYYY-MM-DD');
                filterEndCsatDate = end.format('YYYY-MM-DD');
                $('#dateRangeTexCsat').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadCSATData();
            });

            picker.on('clear', () => {
                filterStartCsatDate = "";
                filterEndCsatDate = "";
                $('#dateRangeTexCsat').text("Select Date Range");
                loadCSATData();
            });
        },
        ranges: {
            Today: [moment(), moment()],
            Yesterday: [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        startDate: moment().startOf('week').format('DD-MM-YYYY'),
        endDate: moment().endOf('week').format('DD-MM-YYYY')
    });

    // Open calendar on click
    $('#customDateTriggerCsat').on('click', function () {
        picker.show();
    });

    $('#backButton').on('click', function () {
        window.history.back();
    });

    loadCSATData();
});

function loadCSATData() {
    Blockloadershow();

    $.ajax({
        url: '/CSATComment/GetAll',
        type: 'GET',
        dataType: 'json',
        data: {
            startDate: filterStartCsatDate,
            endDate: filterEndCsatDate
        },
        success: function (data) {
            if (Array.isArray(data)) {
                OnTabGridLoad(data);
            } else {
                showDangerAlert('No CSAT Comment data available.');
            }
            Blockloaderhide();
        },
        error: function () {
            showDangerAlert('Error loading CSAT Comment data.');
            Blockloaderhide();
        }
    });

}

var headerMenu = function () {
    var menu = [];
    var columns = this.getColumns();

    for (let column of columns) {
        let icon = document.createElement("i");
        icon.classList.add("fas");
        icon.classList.add(column.isVisible() ? "fa-check-square" : "fa-square");

        let label = document.createElement("span");
        let title = document.createElement("span");
        title.textContent = " " + column.getDefinition().title;

        label.appendChild(icon);
        label.appendChild(title);

        menu.push({
            label: label,
            action: function (e) {
                e.stopPropagation();
                column.toggle();

                if (column.isVisible()) {
                    icon.classList.remove("fa-square");
                    icon.classList.add("fa-check-square");
                } else {
                    icon.classList.remove("fa-check-square");
                    icon.classList.add("fa-square");
                }
            }
        });
    }

    return menu;
};

function OnTabGridLoad(response) {
    debugger
    Blockloadershow();
    console.log(response);
    function formatDate(value) {
        return value ? new Date(value).toLocaleDateString("en-GB") : "";
    }

    if (response.length > 0) {
        $.each(response, function (index, item) {
            tabledata.push({
                Sr_No: index + 1,
                Id: item.id,
                Quarter: item.quarter || "",
                Organisation: item.organisation || "",
                Region: item.region || "",
                Q1: item.q1 || "",
                Q2: item.q2 || "",
                Q3: item.q3 || "",
                Q4: item.q4 || "",
                Q5: item.q5 || "",
                Q6: item.q6 || "",
                Q7: item.q7 || "",
                Q8: item.q8 || "",
                Q9: item.q9 || "",
                Q10: item.q10 || "",
                Q11: item.q11 || "",
                Q12: item.q12 || "",
                Q13: item.q13 || "",
                Cust_Critical_Aspect: item.cust_Critical_Aspect || "",
                Comment: item.comment || "",
                CSAT_Business: item.csaT_Business ||"",
                CreatedDate: item.createdDate || "",
                CreatedBy: item.createdBy || "",
                UpdatedDate: item.updatedDate || "",
                UpdatedBy: item.updatedBy || ""
            });
        });
    }

    console.log(tabledata);

    const columns = [
        {
            title: "Action",
            field: "action",
            frozen: true,
            hozAlign: "center", headerMenu: headerMenu,
            headerSort: false,
            width: 90,
            formatter: function (cell) {
                const rowData = cell.getRow().getData();
                return `<i onclick="delConfirm(${rowData.Id}, this)" class="fas fa-trash-alt text-danger" title="Delete" style="cursor:pointer;"></i>`;
            }
        },
        { title: "S.No", field: "Sr_No", frozen: true, hozAlign: "center", headerSort: false, width: 80, headerMenu: headerMenu },

        editableColumn("CSAT Business", "CSAT_Business", true, "left"),
        editableColumn("Quarter", "Quarter", true),
        editableColumn("Organisation", "Organisation", true, "left"),
        editableColumn("Powder Region", "Region", true, "left"),
        //editableColumn("Q1", "Q1", true),
        editableColumn("Q1", "Q1", "list", "left", "input", {}, {
            values: [
                { label: "Strongly Disagree", value: "Strongly Disagree" },
                { label: "Disagree", value: "Disagree" },
                { label: "Agree", value: "Agree" },
                { label: "Strongly Agree", value: "Strongly Agree" },
                { label: "Neither agree nor disagree", value: "Neither agree nor disagree" },
                { label: "Cannot rate", value: "Cannot rate" },
            ]
        }, null, "Wipro Lighting's executive is in regular touch with us."),

        //editableColumn("Q2", "Q2", true),
        editableColumn("Q2", "Q2", "list", "left", "input", {}, {
            values: [
                { label: "Strongly Disagree", value: "Strongly Disagree" },
                { label: "Disagree", value: "Disagree" },
                { label: "Agree", value: "Agree" },
                { label: "Strongly Agree", value: "Strongly Agree" },
                { label: "Neither agree nor disagree", value: "Neither agree nor disagree" },
                { label: "Cannot rate", value: "Cannot rate" },
            ]
        }, null, "Proposals submitted by Wipro Lighting meet our requirements."),

        //editableColumn("Q3", "Q3", true),
        editableColumn("Q3", "Q3", "list", "left", "input", {}, {
            values: [
                { label: "Strongly Disagree", value: "Strongly Disagree" },
                { label: "Disagree", value: "Disagree" },
                { label: "Agree", value: "Agree" },
                { label: "Strongly Agree", value: "Strongly Agree" },
                { label: "Neither agree nor disagree", value: "Neither agree nor disagree" },
                { label: "Cannot rate", value: "Cannot rate" },
            ]
        }, null, "When requested by us,Wipro Lighting's team conducted an effective mock up & demo."),

        //editableColumn("Q4", "Q4", true),
        editableColumn("Q4", "Q4", "list", "left", "input", {}, {
            values: [
                { label: "Strongly Disagree", value: "Strongly Disagree" },
                { label: "Disagree", value: "Disagree" },
                { label: "Agree", value: "Agree" },
                { label: "Strongly Agree", value: "Strongly Agree" },
                { label: "Neither agree nor disagree", value: "Neither agree nor disagree" },
                { label: "Cannot rate", value: "Cannot rate" },
            ]
        }, null, "Products were delivered in good condition."),

        //editableColumn("Q5", "Q5", true),
        editableColumn("Q5", "Q5", "list", "left", "input", {}, {
            values: [
                { label: "Strongly Disagree", value: "Strongly Disagree" },
                { label: "Disagree", value: "Disagree" },
                { label: "Agree", value: "Agree" },
                { label: "Strongly Agree", value: "Strongly Agree" },
                { label: "Neither agree nor disagree", value: "Neither agree nor disagree" },
                { label: "Cannot rate", value: "Cannot rate" },
            ]
        }, null, "The products were delivered as per committed delivery schedule."),

        //editableColumn("Q6", "Q6", true),
        editableColumn("Q6", "Q6", "list", "left", "input", {}, {
            values: [
                { label: "Strongly Disagree", value: "Strongly Disagree" },
                { label: "Disagree", value: "Disagree" },
                { label: "Agree", value: "Agree" },
                { label: "Strongly Agree", value: "Strongly Agree" },
                { label: "Neither agree nor disagree", value: "Neither agree nor disagree" },
                { label: "Cannot rate", value: "Cannot rate" },
            ]
        }, null, "Products delivered are as per our order."),

        //editableColumn("Q7", "Q7", true),
        editableColumn("Q7", "Q7", "list", "left", "input", {}, {
            values: [
                { label: "Strongly Disagree", value: "Strongly Disagree" },
                { label: "Disagree", value: "Disagree" },
                { label: "Agree", value: "Agree" },
                { label: "Strongly Agree", value: "Strongly Agree" },
                { label: "Neither agree nor disagree", value: "Neither agree nor disagree" },
                { label: "Cannot rate", value: "Cannot rate" },
            ]
        }, null, "Bills were submitted on time."),

        //editableColumn("Q8", "Q8", true),
        editableColumn("Q8", "Q8", "list", "left", "input", {}, {
            values: [
                { label: "Strongly Disagree", value: "Strongly Disagree" },
                { label: "Disagree", value: "Disagree" },
                { label: "Agree", value: "Agree" },
                { label: "Strongly Agree", value: "Strongly Agree" },
                { label: "Neither agree nor disagree", value: "Neither agree nor disagree" },
                { label: "Cannot rate", value: "Cannot rate" },
            ]
        }, null, "Payment related documentation was complete."),

        //editableColumn("Q9", "Q9", true),
        editableColumn("Q9", "Q9", "list", "left", "input", {}, {
            values: [
                { label: "Strongly Disagree", value: "Strongly Disagree" },
                { label: "Disagree", value: "Disagree" },
                { label: "Agree", value: "Agree" },
                { label: "Strongly Agree", value: "Strongly Agree" },
                { label: "Neither agree nor disagree", value: "Neither agree nor disagree" },
                { label: "Cannot rate", value: "Cannot rate" },
            ]
        }, null, "When we faced technical problems,Wipro Lighting responded effectively."),

        //editableColumn("Q10", "Q10", true),
        editableColumn("Q10", "Q10", "list", "left", "input", {}, {
            values: [
                { label: "Strongly Disagree", value: "Strongly Disagree" },
                { label: "Disagree", value: "Disagree" },
                { label: "Agree", value: "Agree" },
                { label: "Strongly Agree", value: "Strongly Agree" },
                { label: "Neither agree nor disagree", value: "Neither agree nor disagree" },
                { label: "Cannot rate", value: "Cannot rate" },
            ]
        }, null, "In case of escalations,Wipro Lighting's senior management team was easily available."),

        //editableColumn("Q11", "Q11", true),
        editableColumn("Q11", "Q11", "list", "left", "input", {}, {
            values: [
                { label: "Strongly Disagree", value: "Strongly Disagree" },
                { label: "Disagree", value: "Disagree" },
                { label: "Agree", value: "Agree" },
                { label: "Strongly Agree", value: "Strongly Agree" },
                { label: "Neither agree nor disagree", value: "Neither agree nor disagree" },
                { label: "Cannot rate", value: "Cannot rate" },
            ]
        }, null, "We are satisfied with Wipro Lighting."),

        //editableColumn("Q12", "Q12", true),
        editableColumn("Q12", "Q12", "list", "left", "input", {}, {
            values: [
                { label: "Strongly Disagree", value: "Strongly Disagree" },
                { label: "Disagree", value: "Disagree" },
                { label: "Agree", value: "Agree" },
                { label: "Strongly Agree", value: "Strongly Agree" },
                { label: "Neither agree nor disagree", value: "Neither agree nor disagree" },
                { label: "Cannot rate", value: "Cannot rate" },
            ]
        }, null, "Taking all things into consideration,we would\nrecommend Wipro Lighting products to other users\nand professional contacts over competition offerings\nfor their requirements."),

        //editableColumn("Q13", "Q13", true),
        editableColumn("Q13", "Q13", "list", "left", "input", {}, {
            values: [
                { label: "Strongly Disagree", value: "Strongly Disagree" },
                { label: "Disagree", value: "Disagree" },
                { label: "Agree", value: "Agree" },
                { label: "Strongly Agree", value: "Strongly Agree" },
                { label: "Neither agree nor disagree", value: "Neither agree nor disagree" },
                { label: "Cannot rate", value: "Cannot rate" },
            ]
        }, null, "We are willing to give right of refusal to Wipro Lighting."),

        editableColumn("Cust Critical Aspect", "Cust_Critical_Aspect", true),
        editableColumn("Comments & Suggestions", "Comment", true, "left"),

        { title: "Created Date", field: "CreatedDate", visible: false, headerMenu: headerMenu },
        { title: "Created By", field: "CreatedBy", visible: false, headerMenu: headerMenu },
        { title: "Updated Date", field: "UpdatedDate", visible: false },
        { title: "Updated By", field: "UpdatedBy", visible: false, headerMenu: headerMenu },
         { title: "Id", field: "Id", visible: false }
    ];

    if (table) {
        table.replaceData(tabledata);
    } else {
        table = new Tabulator("#csat_table", {
            data: tabledata,
            layout: "fitDataFill",
            movableColumns: true,
            pagination: "local",
            paginationSize: 10,
            paginationSizeSelector: [10, 50, 100, 500],
            paginationCounter: "rows",
            placeholder: "No data available",
            columns: columns,
            index: "Id"
        });

        table.on("cellEdited", function (cell) {
            const rowData = cell.getRow().getData();
            saveEditedRow(rowData);
        });
    }

    (function bindAddButtonOnce() {
        var $btn = $("#addCsatButton");
        $btn.attr("type", "button");                       // avoid form submit duplicates
        $btn.off("click.addrow").on("click.addrow", function (e) {
            e.preventDefault(); e.stopPropagation();
            if ($btn.data("busy")) return;                 // guard double-clicks
            $btn.data("busy", true).prop("disabled", true);

            try {
                const newRow = {
                    Id: 0,
                    Sr_No: table.getDataCount() + 1,
                    CSAT_Business: "",
                    Quarter: "",
                    Organisation: "",
                    Region: "",
                    Q1: "",
                    Q2: "",
                    Q3: "",
                    Q4: "",
                    Q5: "",
                    Q6: "",
                    Q7: "",
                    Q8: "",
                    Q9: "",
                    Q10: "",
                    Q11: "",
                    Q12: "",
                    Q13: "",
                    Cust_Critical_Aspect: "",
                    Comment: "",
                    CreatedDate: "",
                    CreatedBy: "",
                    UpdatedDate: "",
                    UpdatedBy: ""
                };

                table.addRow(newRow, true).then(function (row) {
                    table.scrollToRow(row, "top", false);
                    if (row.select) row.select();

                    const el = row.getElement();
                    el.classList.add("row-flash");
                    setTimeout(() => el.classList.remove("row-flash"), 1200);

                    renumberSrNo(); // keep S.No sequence
                }).catch(console.error).finally(function () {
                    $btn.data("busy", false).prop("disabled", false);
                });

            } catch (err) {
                console.error(err);
                $btn.data("busy", false).prop("disabled", false);
            }
        });
    })();

    document.getElementById("exportCsatButton").addEventListener("click", async function () {
        // ===== 0) OPTIONS =====
        const EXPORT_SCOPE = "active";   // "active" | "selected" | "all"
        const EXPORT_RAW = false;      // false = formatted values exactly as shown in Tabulator

        // ===== 1) COLUMNS from Tabulator (exact view) with EXCLUDES =====
        if (!window.table) { console.error("Tabulator 'table' not found."); return; }

        const EXCLUDE_FIELDS = new Set(["Action", "action", "Actions", "CreatedBy"]);
        const EXCLUDE_TITLES = new Set(["Action", "Actions", "User"]);

        const tabCols = table.getColumns(true)
            .filter(c => c.getField())
            .filter(c => c.isVisible())
            .filter(c => {
                const def = c.getDefinition();
                const field = def.field || "";
                const title = (def.title || "").trim();
                return !EXCLUDE_FIELDS.has(field) && !EXCLUDE_TITLES.has(title);
            });

        const excelCols = tabCols.map(col => {
            const def = col.getDefinition();
            const label = def.title || def.field;
            const px = (def.width || col.getWidth() || 120);
            const width = Math.max(8, Math.min(40, Math.round(px / 7))); // px->char heuristic
            return { label, key: def.field, width };
        });

        if (!excelCols.length) { alert("No visible columns to export."); return; }

        // ===== 2) DOC DETAILS (will be placed in second-last + last column) =====
        const docDetails = [
            ["Document No", "WCIB/LS/QA/R/005"],
            ["Effective Date", "01/10/2022"],
            ["Revision No", "0"],
            ["Revision Date", "01/10/2022"],
            ["Page No", "1 of 1"]
        ];

        // ===== 3) LAYOUT =====
        const TOTAL_COLS = excelCols.length;

        const HEADER_TOP = 1;
        const HEADER_BOTTOM = 5;
        const GRID_HEADER_ROW = HEADER_BOTTOM + 1;
        const TITLE_TEXT = "CSAT COMMENTS";

        const LOGO_COL_START = 1;
        const LOGO_COL_END = 2;
        const LOGO_ROW_START = HEADER_TOP;
        const LOGO_ROW_END = HEADER_BOTTOM;

        // Title must not overlap the final 2 columns (reserved for details)
        const TITLE_COL_START = Math.min(3, TOTAL_COLS);
        const TITLE_COL_END = Math.max(TITLE_COL_START, TOTAL_COLS - 2);

        // Document details strictly in second-last & last column
        const DETAILS_LABEL_COL = Math.max(1, TOTAL_COLS - 1);
        const DETAILS_VALUE_COL = TOTAL_COLS;

        // ===== 4) HELPERS =====
        async function fetchAsBase64(url) {
            const res = await fetch(url);
            const blob = await res.blob();
            return new Promise((resolve) => {
                const reader = new FileReader();
                reader.onloadend = () => resolve(reader.result.split(",")[1]);
                reader.readAsDataURL(blob);
            });
        }
        function setBorder(cell, style = "thin") {
            cell.border = { top: { style }, bottom: { style }, left: { style }, right: { style } };
        }
        function outlineRange(ws, r1, c1, r2, c2, style = "thin") {
            for (let c = c1; c <= c2; c++) {
                const top = ws.getCell(r1, c), bottom = ws.getCell(r2, c);
                top.border = { ...top.border, top: { style } };
                bottom.border = { ...bottom.border, bottom: { style } };
            }
            for (let r = r1; r <= r2; r++) {
                const left = ws.getCell(r, c1), right = ws.getCell(r, c2);
                left.border = { ...left.border, left: { style } };
                right.border = { ...right.border, right: { style } };
            }
        }

        // ===== 5) WORKBOOK / SHEET =====
        const wb = new ExcelJS.Workbook();
        const ws = wb.addWorksheet("Csat Comments", {
            properties: { defaultRowHeight: 15 },
            views: [{ state: "frozen", xSplit: 0, ySplit: GRID_HEADER_ROW }] // sticky header
        });

        // Set column widths (no header to avoid duplicates)
        ws.columns = excelCols.map(c => ({ key: c.key, width: c.width }));

        // Give header band height so logo fits
        for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) {
            ws.getRow(r).height = 18; // ~34px
        }

        ws.pageSetup = {
            orientation: "landscape",
            fitToPage: true,
            fitToWidth: 1,
            fitToHeight: 0,
            margins: { left: 0.3, right: 0.3, top: 0.5, bottom: 0.5, header: 0.2, footer: 0.2 },
            printTitlesRow: `${HEADER_TOP}:${GRID_HEADER_ROW}`
        };

        // ===== 6) HEADER BAND FILL =====
        for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) {
            for (let c = 1; c <= TOTAL_COLS; c++) {
                ws.getCell(r, c).fill = {
                    type: "pattern", pattern: "solid", fgColor: { argb: "FFF7F7F7" }
                };
            }
        }

        // ===== 7) LOGO — centered in A2:B6 (no stretch) + outline =====
        // Desired logo size (px)
        const LOGO_WIDTH_PX = 100;  // adjust as you like
        const LOGO_HEIGHT_PX = 100;

        // Approx column/row pixel helpers
        const COL_PX = (c) => ((ws.getColumn(c).width || 8) * 7);       // ~7px per char width
        const ROW_PX = (r) => ((ws.getRow(r).height || 15) * (96 / 72));  // pt -> px

        // Size of A2:B6 rect in px
        let rectWpx = 0; for (let c = LOGO_COL_START; c <= LOGO_COL_END; c++) rectWpx += COL_PX(c);
        let rectHpx = 0; for (let r = LOGO_ROW_START; r <= LOGO_ROW_END; r++) rectHpx += ROW_PX(r);

        // Average px per column/row in that rectangle
        const avgColPx = rectWpx / (LOGO_COL_END - LOGO_COL_START + 1);
        const avgRowPx = rectHpx / (LOGO_ROW_END - LOGO_ROW_START + 1);

        // Convert desired pixel size → fractional col/row units
        const logoCols = LOGO_WIDTH_PX / avgColPx;
        const logoRows = LOGO_HEIGHT_PX / avgRowPx;

        // Centered TL anchor (fractional col/row)
        const tlCol = (LOGO_COL_START - 1) + ((LOGO_COL_END - LOGO_COL_START + 1) - logoCols) / 2;
        const tlRow = (LOGO_ROW_START - 1) + ((LOGO_ROW_END - LOGO_ROW_START + 1) - logoRows) / 2;

        const logoUrl = window.LOGO_URL || (window.APP_BASE && (window.APP_BASE + "images/wipro-logo.png"));
        if (logoUrl) {
            try {
                const base64 = await fetchAsBase64(logoUrl);
                const imgId = wb.addImage({ base64, extension: "png" });
                ws.addImage(imgId, {
                    tl: { col: tlCol, row: tlRow },
                    ext: { width: LOGO_WIDTH_PX, height: LOGO_HEIGHT_PX },
                    editAs: "oneCell"
                });
            } catch (e) { console.warn("Logo load failed:", e); }
        }
        outlineRange(ws, LOGO_ROW_START, LOGO_COL_START, LOGO_ROW_END, LOGO_COL_END, "thin");

        // ===== 8) TITLE (merge) + outline (from column C to third-last col) =====
        ws.mergeCells(HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END);
        const titleCell = ws.getCell(HEADER_TOP, TITLE_COL_START);
        titleCell.value = TITLE_TEXT;
        titleCell.font = { bold: true, size: 18 };
        titleCell.alignment = { horizontal: "center", vertical: "middle" };

        outlineRange(ws, HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END, "thin");

        // ===== 9) DOCUMENT DETAILS in (second-last, last) columns =====
        // Rows: HEADER_TOP..(HEADER_TOP + docDetails.length - 1)
        const detailsRowsEnd = HEADER_TOP + docDetails.length - 1;
        docDetails.forEach((pair, i) => {
            const r = HEADER_TOP + i;

            const labelCell = ws.getCell(r, DETAILS_LABEL_COL);
            const valueCell = ws.getCell(r, DETAILS_VALUE_COL);

            labelCell.value = pair[0];
            valueCell.value = pair[1];

            labelCell.font = { bold: true };
            [labelCell, valueCell].forEach(cell => {
                cell.alignment = { vertical: "middle", horizontal: "left", wrapText: true };
                setBorder(cell, "thin");
            });
        });
        // Optional: outline the whole two-column details block
        outlineRange(ws, HEADER_TOP, DETAILS_LABEL_COL, detailsRowsEnd, DETAILS_VALUE_COL, "thin");

        // ===== 10) MANUAL TABLE HEADER (row 7) =====
        while (ws.rowCount < GRID_HEADER_ROW - 1) ws.addRow([]); // up to row 6

        const headerTitles = excelCols.map(c => c.label);
        const headerRow = ws.addRow(headerTitles);
        headerRow.height = 22;
        headerRow.eachCell((cell) => {
            cell.font = { bold: true };
            cell.alignment = { horizontal: "center", vertical: "middle", wrapText: true };
            cell.fill = { type: "pattern", pattern: "solid", fgColor: { argb: "FFD9E1F2" } };
            setBorder(cell);
        });

        // ===== 11) DATA ROWS (exact Tabulator view, minus Action) =====
        let tabRows;
        switch (EXPORT_SCOPE) {
            case "selected": tabRows = table.getSelectedRows(); break;
            case "all": tabRows = table.getRows(); break;
            case "active":
            default: tabRows = table.getRows("active"); break;
        }

        tabRows.forEach(row => {
            const cells = row.getCells();
            const byField = {};
            cells.forEach(cell => {
                const f = cell.getField();
                if (!f) return;
                // Skip excluded
                const def = cell.getColumn().getDefinition();
                const title = (def.title || "").trim();
                if (EXCLUDE_FIELDS.has(f) || EXCLUDE_TITLES.has(title)) return;

                byField[f] = EXPORT_RAW ? row.getData()[f] : cell.getValue(); // exact display value by default
            });

            const values = excelCols.map(c => byField[c.key] ?? "");
            const xRow = ws.addRow(values);

            xRow.eachCell((cell, colNumber) => {
                cell.alignment = { vertical: "middle", horizontal: colNumber === 1 ? "center" : "left", wrapText: true };
                setBorder(cell);
            });
        });

        // ===== 12) DOWNLOAD =====
        const buffer = await wb.xlsx.writeBuffer();
        const blob = new Blob([buffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = "Csat Comments.xlsx";
        document.body.appendChild(link);
        link.click();
        link.remove();
    });

    Blockloaderhide();
}

function renumberSrNo() {
    const rows = table.getRows("active");
    $.each(rows, function (i, r) {
        const d = r.getData();
        if (d.Sr_No !== i + 1) { r.update({ Sr_No: i + 1 }); }
    });
}


function delConfirm(recid, element) {

    if (!recid || recid <= 0) {
        // Unsaved row: just remove from UI
        const rowEl = $(element).closest(".tabulator-row")[0];
        const row = table.getRow(rowEl);
        if (row) row.delete();
        return;
    }

    PNotify.prototype.options.styling = "bootstrap3";

    const notice = new PNotify({
        title: false,
        text: 'Are you sure you want to delete?',
        icon: 'glyphicon glyphicon-question-sign',
        hide: false,
        width: '360px',
        confirm: {
            confirm: true,
            buttons: [
                {
                    text: 'Yes',
                    addClass: 'btn btn-sm btn-danger',
                    click: function (notice, value) {
                        // lock buttons while deleting
                        notice.get().find('.btn').prop('disabled', true);
                        $.ajax({
                            url: '/CSATComment/Delete',
                            type: 'POST',
                            data: { id: recid }
                        }).done(function (data) {
                            if (data && data.success === true) {
                                showSuccessNewAlert("CSAT Comment Detail deleted successfully.");
                                // remove row immediately
                                const rowEl = $(element).closest(".tabulator-row")[0];
                                const row = table.getRow(rowEl);
                                if (row) row.delete();
                                loadCSATData();
                            } else if (data && data.success === false && data.message === "Not_Deleted") {
                                showDangerAlert("Record is used in QMS Log transactions.");
                            } else {
                                showDangerAlert((data && data.message) || "Delete failed.");
                            }
                        }).fail(function () {
                            showDangerAlert('Server error during delete.');
                        }).always(function () {
                            notice.remove(); // close the confirm
                        });
                    }
                },
                {
                    text: 'No',
                    addClass: 'btn btn-sm btn-default',
                    click: function (notice) {
                        notice.remove();      // just close
                        // no reload; your current date filter stays as-is
                    }
                }
            ]
        },
        buttons: {
            closer: false,
            sticker: false
        },
        history: { history: false },
        // Accessibility/keyboard
        animate_speed: 'fast',
        destroy: true
    });

    // Focus the "Yes" button by default
    notice.get().one('shown.bs.modal pnotify.afterOpen', function () {
        notice.get().find('.btn.btn-danger').focus();
    });

    // Also handle Enter/Esc
    notice.get().on('keydown', function (e) {
        if (e.key === 'Enter') {
            notice.get().find('.btn.btn-danger').click();
        } else if (e.key === 'Escape') {
            notice.remove();
        }
    });
}
function editableColumn(title, field, editorType = true, align = "center", headerFilterType = "input", headerFilterParams = {}, editorParams = {}, formatter = null,headerTooltip = null) {
    let columnDef = {
        title: title,
        field: field,
        editor: editorType,
        editorParams: editorParams,
        formatter: formatter,
        headerFilter: headerFilterType,
        headerFilterParams: headerFilterParams,
        headerMenu: headerMenu,
        hozAlign: align,
        headerHozAlign: "left"
    };

    // 👇 add this block
    if (headerTooltip) {
        columnDef.headerTooltip = headerTooltip;
    }

    // Set custom width for specific fields
    if (field === "ProductCode") {
        columnDef.width = 220;
        columnDef.minWidth = 220;
    }
    else if (field === "ProductDescription") {
        columnDef.width = 290;
        columnDef.minWidth = 290;
        columnDef.hozAlign = "left";
    }

    return columnDef;
}


function saveEditedRow(rowData) {
    console.log(rowData);

    function toIsoDate(value) {
        if (!value || value === "" || value === "Invalid Date") return null;

        if (typeof value === "string") {
            const s = value.trim();
            // dd/MM/yyyy
            if (s.includes("/")) {
                const parts = s.split("/");
                if (parts.length === 3) {
                    const [day, month, year] = parts;
                    return `${year}-${month.padStart(2, "0")}-${day.padStart(2, "0")}`;
                }
            }
            // yyyy-MM-dd (already)
            if (/^\d{4}-\d{2}-\d{2}$/.test(s)) return s;
        }

        const d = new Date(value);
        return isNaN(d.getTime()) ? null : d.toISOString().substring(0, 10);
    }

    function normalizeMultiToString(val, joinWith) {
        if (val == null) return "";
        if (Array.isArray(val)) {
            return val
                .map(v => String(v ?? "").trim())
                .filter(v => v !== "")
                .join(joinWith);
        }
        return String(val).trim();
    }

    const cleanedData = {
        Id: rowData.Id || 0,
        Quarter: rowData.Quarter || "",
        Organisation: rowData.Organisation || "",
        Region: rowData.Region || "",
        Q1: rowData.Q1 || "",
        Q2: rowData.Q2 || "",
        Q3: rowData.Q3 || "",
        Q4: rowData.Q4 || "",
        Q5: rowData.Q5 || "",
        Q6: rowData.Q6 || "",
        Q7: rowData.Q7 || "",
        Q8: rowData.Q8 || "",
        Q9: rowData.Q9 || "",
        Q10: rowData.Q10 || "",
        Q11: rowData.Q11 || "",
        Q12: rowData.Q12 || "",
        Q13: rowData.Q13 || "",
        Cust_Critical_Aspect: rowData.Cust_Critical_Aspect || "",
        Comment: rowData.Comment || "",
        CSAT_Business: rowData.CSAT_Business || ""
    };

    console.log(cleanedData);

    const isNew = cleanedData.Id === 0;
    const url = isNew ? "/CSATComment/Create" : "/CSATComment/Update";

    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(cleanedData),
        contentType: "application/json",
        success: function (response) {
            if (response.success) {
                if (isNew) {
                    showSuccessNewAlert("Saved successfully!.");
                    loadCSATData();
                }
            }
            else if (response.message === "Exist") {
                showDangerAlert("CSAT Comment Detail already exists.");
            }
            else {
                var errorMessg = "";
                if (response.errors) {
                    for (var error in response.errors) {
                        if (response.errors.hasOwnProperty(error)) {
                            errorMessg += `${response.errors[error]}\n`;
                        }
                    }
                }
                showDangerAlert(errorMessg || response.message || "An error occurred while saving.");
            }
        },
        error: function (xhr) {
            showDangerAlert(xhr.responseText || "Error saving record.");
        }
    });
}

