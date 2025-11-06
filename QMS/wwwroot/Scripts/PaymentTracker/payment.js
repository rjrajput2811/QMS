var tabledata = [];
var table = null;
const searchTerms = {};
let vendorOptions = {};
var tabledataLab = [];
var tableLab = '';
let labOptions = {};
let selectedLabCell = null;
let filterStartPaymentDate = moment().startOf('month').format('YYYY-MM-DD');
let filterEndPaymentDate = moment().endOf('month').format('YYYY-MM-DD');

$(document).ready(function () {

    $('#dateRangePayment').text(
        moment(filterStartPaymentDate).format('MMMM D, YYYY') + ' - ' + moment(filterEndPaymentDate).format('MMMM D, YYYY')
    );

    const picker = new Litepicker({
        element: document.getElementById('customDateTriggerPayment'),
        singleMode: false,
        format: 'DD-MM-YYYY',
        numberOfMonths: 2,
        numberOfColumns: 2,
        dropdowns: { minYear: 2020, maxYear: null, months: true, years: true },
        plugins: ['ranges'],
        setup: (picker) => {
            picker.on('selected', (start, end) => {
                filterStartPaymentDate = start.format('YYYY-MM-DD');
                filterEndPaymentDate = end.format('YYYY-MM-DD');
                $('#dateRangePayment').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadData();
            });

            picker.on('clear', () => {
                filterStartPaymentDate = "";
                filterEndPaymentDate = "";
                $('#dateRangePayment').text("Select Date Range");
                loadData();
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
        startDate: moment().startOf('month').format('DD-MM-YYYY'),
        endDate: moment().endOf('month').format('DD-MM-YYYY')
    });

    $('#customDateTriggerPayment').on('click', function () {
        picker.show();
    });

    document.addEventListener('DOMContentLoaded', function () {
        document.getElementById('backButton').addEventListener('click', function () {
            window.history.back();
        });
    });

    loadData();
});


function loadData() {
    Blockloadershow();

    // Step 1: Load vendor data
    $.ajax({
        url: '/Service/GetVendor',
        type: 'GET'
    }).done(function (vendorData) {
        //let vendorOptions = {};

        if (Array.isArray(vendorData)) {
            vendorOptions = vendorData.reduce((acc, v) => {
                acc[v.value] = v.label;
                return acc;
            }, {});
        }

        // Step 2: Load Nat Project data
        $.ajax({
            url: '/PaymentTracker/GetLabDropdown',
            type: 'GET'
        }).done(function (lab) {
            //let natProjectOptions = {};

            if (Array.isArray(lab)) {
                labOptions = lab.reduce((acc, v) => {
                    acc[v.value] = v.label;
                    return acc;
                }, {});
            }

            $.ajax({
                url: '/PaymentTracker/GetAll',
                type: 'GET',
                dataType: 'json',
                data: {
                    startDate: filterStartPaymentDate,
                    endDate: filterEndPaymentDate
                },
                success: function (data) {
                    Blockloaderhide();
                    if (data && Array.isArray(data)) {
                        OnTabGridLoad(data);
                    } else {
                        showDangerAlert('No data available to load.');
                    }
                },
                error: function (xhr, status, error) {
                    Blockloaderhide();
                    showDangerAlert('Error retrieving data: ' + error);
                }
            });
        }).fail(function () {
            Blockloaderhide();
            showDangerAlert('Failed to load Nat Project data.');
        });

    }).fail(function () {
        Blockloaderhide();
        showDangerAlert('Failed to load vendor data.');
    });
}


//define column header menu as column visibility toggle
var headerMenu = function () {
    var menu = [];
    var columns = this.getColumns();

    for (let column of columns) {

        //create checkbox element using font awesome icons
        let icon = document.createElement("i");
        icon.classList.add("fas");
        icon.classList.add(column.isVisible() ? "fa-check-square" : "fa-square");

        //build label
        let label = document.createElement("span");
        let title = document.createElement("span");

        title.textContent = " " + column.getDefinition().title;

        label.appendChild(icon);
        label.appendChild(title);

        //create menu item
        menu.push({
            label: label,
            action: function (e) {
                //prevent menu closing
                e.stopPropagation();

                //toggle current column visibility
                column.toggle();

                //change menu item icon
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

function getFinancialYears() {
    let today = new Date();
    let year = today.getFullYear();
    let month = today.getMonth() + 1; // Jan=0, so +1

    let startYear;
    if (month < 4) {
        // Before April → FY belongs to previous year
        startYear = year - 1;
    } else {
        startYear = year;
    }

    // Build last 5 financial years
    let years = {};
    for (let i = 0; i < 5; i++) {
        let sYear = startYear - i;
        let eYear = sYear + 1;
        let fy = sYear + "-" + eYear.toString().slice(-2);
        years[fy] = fy;
    }

    return years;
}

var financialYears = getFinancialYears();

function _valuesToMap(values) {
    if (!values) return null;
    if (Array.isArray(values)) {
        const map = {};
        values.forEach(opt => {
            if (opt && typeof opt === "object") {
                map[String(opt.value)] = opt.label ?? opt.text ?? opt.value;
            } else {
                map[String(opt)] = String(opt);
            }
        });
        return map;
    }
    if (typeof values === "object") return values;
    return null;
}

// Safely run a column's custom formatter (function formatters only)
function _runFormatter(cell, def) {
    if (typeof def.formatter !== "function") return null; // built-in string formatters won't work here
    try {
        const out = def.formatter(cell, def.formatterParams || {}, function onRendered() { });
        if (out == null) return null;
        if (typeof out === "string") return out.replace(/<[^>]*>/g, "").trim();
        if (out instanceof HTMLElement) return (out.textContent || "").trim();
    } catch (e) {
        // swallow—fallbacks will handle
    }
    return null;
}

// Get display string for Excel export WITHOUT using cell.getFormattedValue()
function getDisplayValue(cell) {
    const def = cell.getColumn().getDefinition();
    let v = cell.getValue(); // raw

    // 1) Try mapping via editor/headerFilter configs (good for lists/selects)
    const maps = [];
    if (def.editorParams && def.editorParams.values) maps.push(_valuesToMap(def.editorParams.values));
    if (def.headerFilterParams && def.headerFilterParams.values) maps.push(_valuesToMap(def.headerFilterParams.values));

    // 2) Field-specific fallbacks to your global dictionaries
    if (def.field === "Lab" && window.labOptions) maps.push(window.labOptions);
    if (def.field === "Vendor" && window.vendorOptions) maps.push(window.vendorOptions);

    for (const map of maps) {
        if (map && Object.prototype.hasOwnProperty.call(map, String(v))) {
            v = map[String(v)];
            break;
        }
    }

    // 3) If still raw and there is a custom formatter FUNCTION, use it
    if (typeof def.formatter === "function") {
        const formatted = _runFormatter(cell, def);
        if (formatted != null && formatted !== "") v = formatted;
    }

    // 4) Clean up to plain text
    if (v == null) return "";
    return (typeof v === "string") ? v.replace(/<[^>]*>/g, "").trim() : v;
}

function OnTabGridLoad(response) {
    debugger;
    Blockloadershow();

    tabledata = [];
    let columns = [];

    // Map the response to the table format
    if (response.length > 0) {
        $.each(response, function (index, item) {

            function formatDate(value) {
                return value ? new Date(value).toLocaleDateString("en-GB") : "";
            }

            tabledata.push({
                Sr_No: index + 1,
                Id: item.id,
                Fin_Year: item.fin_Year,
                Month: item.month,
                Lab: item.lab,
                Vendor: item.vendor,
                Type_Test: item.type_Test,
                Description: item.description,
                Bis_Id: item.bis_Id,
                Invoice_No: item.invoice_No,
                Invoice_Date: formatDate(item.invoice_Date),
                Testing_Fee: item.testing_Fee,
                Approval_By: item.approval_By,
                Remark: item.remark,
                Attachment: item.attachment,
                CreatedBy: item.createdBy,
                UpdatedBy: item.updatedBy,
                UpdatedDate: formatDate(item.updatedDate),
                CreatedDate: formatDate(item.createdDate),
            });
        });
    }

    if (tabledata.length === 0 && table) {
        table.clearData();
        Blockloaderhide();
        return;
    }

    columns.push(
        {
            title: "Action",
            field: "Action",
            width: 40,
            headerMenu: headerMenu,
            hozAlign: "center",
            headerHozAlign: "center",
            formatter: function (cell, formatterParams) {
                const rowData = cell.getRow().getData();
                let actionButtons = "";

                actionButtons += `<i onclick="delConfirm(${rowData.Id}, this)" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>`

                return actionButtons;
            }
        },
        {
            title: "SNo", field: "Sr_No", sorter: "number", headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "left", width: 60
        },
        editableColumn("Invoice Date", "Invoice_Date", "date", "center"),
        editableColumn("Invoice No.", "Invoice_No", true),
        {
            title: "Financial Year",
            field: "Fin_Year",
            editor: "list",
            editorParams: {
                values: financialYears,
                clearable: true
            },
            headerFilter: "list",
            headerFilterParams: { values: financialYears },
            hozAlign: "center",
            headerHozAlign: "center",
            headerMenu: headerMenu
        },
        //editableColumn("Financial Year", "Fin_Year", true),
        editableColumn("Month", "Month", "select2Test", "center", "input", {}, {
            values: [
                { label: "January", value: "January" },
                { label: "February", value: "February" },
                { label: "March", value: "March" },
                { label: "April", value: "April" },
                { label: "May", value: "May" },
                { label: "June", value: "June" },
                { label: "July", value: "July" },
                { label: "August", value: "August" },
                { label: "September", value: "September" },
                { label: "October", value: "October" },
                { label: "November", value: "November" },
                { label: "December", value: "December" }
            ]
        }),
        //editableColumn("Lab", "Lab", true),
        editableColumn("Lab", "Lab", "select2", "center", "input", {}, {
            values: labOptions
        }, function (cell) {
            const val = cell.getValue();
            return labOptions[val] || val;
        }, 170),
        editableColumn("Vendor", "Vendor", "select2", "center", "input", {}, {
            values: vendorOptions
        }, function (cell) {
            const val = cell.getValue();
            return vendorOptions[val] || val;
        }, 130),
        editableColumn("Type of Test/ Standard", "Type_Test", true),

        editableColumn("Description", "Description", true),
        editableColumn("Test report no./ BIS ID", "Bis_Id", true),

        //editableColumn("Testing fees (Rs)", "Testing_Fee", true),
        editableColumn("Testing fees (Rs)", "Testing_Fee", "input", "right", "input", {}, {
            elementAttributes: {
                type: "number",
                min: 0,
                step: "0.01" // Optional: allows decimal
            }
        }),

        editableColumn("Approval by", "Approval_By", true),
        editableColumn("Remark", "Remark", true),
        {
            title: "Attachment",
            field: "Attachment",
            hozAlign: "center",
            headerHozAlign: "center",
            headerMenu: headerMenu,
            formatter: function (cell, formatterParams) {
                const rowData = cell.getRow().getData();
                const fileName = cell.getValue();
                const fileDisplay = fileName
                    ? `<a href="/PaymentTrac_Attach/${rowData.Id}/${fileName}" target="_blank">${fileName}</a><br/>`
                    : '';

                return `
            ${fileDisplay}
            <input type="file" accept=".pdf,image/*" class="form-control-file payment-upload" data-id="${cell.getRow().getData().Id}" style="width:160px;" />`;
            },
            cellClick: function (e, cell) {
                // prevent Tabulator from swallowing the file input click
                e.stopPropagation();
            }
        },

        { title: "User", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Date", field: "CreatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
        { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
    );

    // // Initialize Tabulator
    table = new Tabulator("#payment_Table", {
        data: tabledata,
        renderHorizontal: "virtual",
        movableColumns: true,
        pagination: "local",
        paginationSize: 10,
        paginationSizeSelector: [50, 100, 500, 1500, 2000],
        paginationCounter: "rows",
        dataEmpty: "<div style='text-align: center; font-size: 1rem; color: gray;'>No data available</div>", // Placeholder message
        columns: columns
    });

    table.on("cellEdited", function (cell) {

        InsertUpdatePayment(cell.getRow().getData());
    });

    (function bindAddButtonOnce() {
        var $btn = $("#addPaymentButton");
        $btn.attr("type", "button");                       // avoid form submit duplicates
        $btn.off("click.addrow").on("click.addrow", function (e) {
            e.preventDefault(); e.stopPropagation();
            if ($btn.data("busy")) return;                 // guard double-clicks
            $btn.data("busy", true).prop("disabled", true);

            try {
                const fyOptions = getFinancialYears(); // returns object
                const currentFY = Object.keys(fyOptions)[0]; // take the first key (e.g. "2025-26")
                const currentMonth = new Date().toLocaleString('default', { month: 'long' });

                const newRow = {
                    Sr_No: table.getDataCount() + 1,
                    Id: 0,
                    CreatedDate: "",
                    Fin_Year: currentFY,
                    Month: currentMonth,
                    Lab: "",
                    Vendor: "",
                    Type_Test: "",
                    Description: "",
                    Bis_Id: "",
                    Invoice_No: "",
                    Invoice_Date: "",
                    Testing_Fee: "",
                    Approval_By: "",
                    Remark: "",
                    CreatedBy: "",
                    UpdatedBy: "",
                    UpdatedDate: ""
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

    //$("#addPaymentButton").on("click", function () {
    //    const fyOptions = getFinancialYears(); // returns object
    //    const currentFY = Object.keys(fyOptions)[0]; // take the first key (e.g. "2025-26")
    //    const currentMonth = new Date().toLocaleString('default', { month: 'long' });

    //    const newRow = {
    //        Sr_No: table.getDataCount() + 1,
    //        Id: 0,
    //        CreatedDate: "",
    //        Fin_Year: currentFY,
    //        Month: currentMonth,
    //        Lab: "",
    //        Vendor: "",
    //        Type_Test: "",
    //        Description: "",
    //        Bis_Id: "",
    //        Invoice_No: "",
    //        Invoice_Date: "",
    //        Testing_Fee: "",
    //        Approval_By: "",
    //        Remark: "",
    //        CreatedBy: "",
    //        UpdatedBy: "",
    //        UpdatedDate: ""
    //    };
    //    table.addRow(newRow, false);
    //});

    // Export to Excel on button click
    document.getElementById("exportPayButton").addEventListener("click", async function () {
        // ===== 0) OPTIONS =====
        const EXPORT_SCOPE = "active"; // "active" | "selected" | "all"
        const EXPORT_RAW = false;    // false = use display/labels

        // ===== 1) Build columns list from visible Tabulator columns (exclude Action/User)
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

        // ===== 2) DOC DETAILS (fixed two rightmost columns)
        const docDetails = [
            ["Document No", "WCIB/LS/QA/R/005"],
            ["Effective Date", "01/10/2022"],
            ["Revision No", "0"],
            ["Revision Date", "01/10/2022"],
            ["Page No", "1 of 1"]
        ];

        // ===== 3) Layout constants =====
        const TOTAL_COLS = excelCols.length;
        const HEADER_TOP = 1;
        const HEADER_BOTTOM = 5;
        const GRID_HEADER_ROW = HEADER_BOTTOM + 1;
        const TITLE_TEXT = "PAYMENT TRACKER";

        // Logo block (A1:B5)
        const LOGO_COL_START = 1, LOGO_COL_END = 2;
        const LOGO_ROW_START = HEADER_TOP, LOGO_ROW_END = HEADER_BOTTOM;

        const TITLE_COL_START = Math.min(3, TOTAL_COLS);
        const TITLE_COL_END = Math.max(TITLE_COL_START, TOTAL_COLS - 2);

        const DETAILS_LABEL_COL = Math.max(1, TOTAL_COLS - 1);
        const DETAILS_VALUE_COL = TOTAL_COLS;

        // ===== 4) Helpers =====
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

        // ===== 5) Workbook / Sheet =====
        const wb = new ExcelJS.Workbook();
        const ws = wb.addWorksheet("Payment Tracker", {
            properties: { defaultRowHeight: 15 },
            views: [{ state: "frozen", xSplit: 0, ySplit: GRID_HEADER_ROW }] // sticky header
        });

        ws.columns = excelCols.map(c => ({ key: c.key, width: c.width }));

        // Row heights so logo fits
        for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) ws.getRow(r).height = 18;

        ws.pageSetup = {
            orientation: "landscape",
            fitToPage: true,
            fitToWidth: 1,
            fitToHeight: 0,
            margins: { left: 0.3, right: 0.3, top: 0.5, bottom: 0.5, header: 0.2, footer: 0.2 },
            printTitlesRow: `${HEADER_TOP}:${GRID_HEADER_ROW}`
        };

        // ===== 6) Header band fill =====
        for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) {
            for (let c = 1; c <= TOTAL_COLS; c++) {
                ws.getCell(r, c).fill = { type: "pattern", pattern: "solid", fgColor: { argb: "FFF7F7F7" } };
            }
        }

        // ===== 7) Logo centered in A1:B5 (no stretch) + outline =====
        const LOGO_WIDTH_PX = 100, LOGO_HEIGHT_PX = 100;

        const COL_PX = (c) => ((ws.getColumn(c).width || 8) * 7);
        const ROW_PX = (r) => ((ws.getRow(r).height || 15) * (96 / 72));

        let rectWpx = 0; for (let c = LOGO_COL_START; c <= LOGO_COL_END; c++) rectWpx += COL_PX(c);
        let rectHpx = 0; for (let r = LOGO_ROW_START; r <= LOGO_ROW_END; r++) rectHpx += ROW_PX(r);

        const avgColPx = rectWpx / (LOGO_COL_END - LOGO_COL_START + 1);
        const avgRowPx = rectHpx / (LOGO_ROW_END - LOGO_ROW_START + 1);

        const logoCols = LOGO_WIDTH_PX / avgColPx;
        const logoRows = LOGO_HEIGHT_PX / avgRowPx;

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

        // ===== 8) Title (merge) + outline =====
        ws.mergeCells(HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END);
        const titleCell = ws.getCell(HEADER_TOP, TITLE_COL_START);
        titleCell.value = TITLE_TEXT;
        titleCell.font = { bold: true, size: 18 };
        titleCell.alignment = { horizontal: "center", vertical: "middle" };
        outlineRange(ws, HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END, "thin");

        // ===== 9) Document details in the last two columns =====
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
        outlineRange(ws, HEADER_TOP, DETAILS_LABEL_COL, detailsRowsEnd, DETAILS_VALUE_COL, "thin");

        // ===== 10) Manual table header row =====
        while (ws.rowCount < GRID_HEADER_ROW - 1) ws.addRow([]); // pad to row before header

        const headerTitles = excelCols.map(c => c.label);
        const headerRow = ws.addRow(headerTitles);
        headerRow.height = 22;
        headerRow.eachCell((cell) => {
            cell.font = { bold: true };
            cell.alignment = { horizontal: "center", vertical: "middle", wrapText: true };
            cell.fill = { type: "pattern", pattern: "solid", fgColor: { argb: "FFD9E1F2" } };
            setBorder(cell);
        });

        // ===== 11) DATA ROWS (use display text for dropdowns) =====
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

                const def = cell.getColumn().getDefinition();
                const title = (def.title || "").trim();

                if (EXCLUDE_FIELDS.has(f) || EXCLUDE_TITLES.has(title)) return;

                byField[f] = EXPORT_RAW ? row.getData()[f] : getDisplayValue(cell);
            });

            const values = excelCols.map(c => byField[c.key] ?? "");
            const xRow = ws.addRow(values);

            xRow.eachCell((cell, colNumber) => {
                cell.alignment = {
                    vertical: "middle",
                    horizontal: colNumber === 1 ? "center" : "left",
                    wrapText: true
                };
                setBorder(cell);
            });
        });

        // ===== 12) DOWNLOAD =====
        const buffer = await wb.xlsx.writeBuffer();
        const blob = new Blob([buffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = "Payment Tracker.xlsx";
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

$('#payment_Table').on('change', '.payment-upload', function () {
    const input = this;
    const file = input.files[0];

    if (!file) return;

    const allowedTypes = [
        "application/pdf",
        "image/jpeg",
        "image/png",
        "image/gif",
        "image/bmp",
        "image/webp"
    ];

    if (!allowedTypes.includes(file.type)) {
        showDangerAlert("Only PDF and image files (PDF, JPG, PNG, GIF, BMP, WEBP) are allowed.");
        $(this).val(""); // reset the input
        return;
    }

    const formData = new FormData();
    formData.append("file", file);
    formData.append("id", $(this).data("id"));

    Blockloadershow();

    $.ajax({
        url: "/PaymentTracker/UploadPaymentAttachment",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false
    }).done(function (response) {
        if (response.success) {
            showSuccessAlert("File uploaded successfully.");
            table.updateData([{ Id: response.id, BIS_Attachment: response.fileName }]);
        } else {
            showDangerAlert(response.message || "Upload failed.");
        }
    }).fail(function () {
        showDangerAlert("Upload failed due to server error.");
    }).always(function () {
        Blockloaderhide();
    });
});

function editableColumn(title, field, editorType = true, align = "center", headerFilterType = "input", headerFilterParams = {}, editorParams = {}, formatter = null) {
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

    return columnDef;
}

Tabulator.extendModule("edit", "editors", {
    select2Test: function (cell, onRendered, success, cancel, editorParams) {
        const values = editorParams.values || {};
        const select = document.createElement("select");
        select.style.width = "100%";

        if (Array.isArray(values)) {
            values.forEach(function (item) {
                let option = document.createElement("option");
                option.value = item.value;
                option.text = item.label;
                if (item.value === cell.getValue()) option.selected = true;
                select.appendChild(option);
            });
        }
        else {

            for (let val in values) {
                let option = document.createElement("option");
                option.value = val;
                option.text = values[val];
                if (val === cell.getValue()) option.selected = true;
                select.appendChild(option);
            }
        }
        onRendered(function () {
            $(select).select2({
                dropdownParent: document.body,
                width: 'resolve',
                placeholder: "Select value"
            }).on("change", function () {
                success(select.value);
            });
        });

        return select;
    }
});

Tabulator.extendModule("edit", "editors", {
    select2: function (cell, onRendered, success, cancel, editorParams) {
        const fieldName = cell.getField(); // column field
        const values = editorParams.values || {};
        const select = document.createElement("select");
        select.style.width = "100%";

        // Add regular options
        for (let val in values) {
            let option = document.createElement("option");
            option.value = val;
            option.text = values[val];
            if (val === cell.getValue()) option.selected = true;
            select.appendChild(option);
        }

        // Add "Add New" option only for Nat_Project
        if (fieldName === "Lab") {
            let addOption = document.createElement("option");
            addOption.value = "__add_new__";
            addOption.text = "➕ Add New Lab";
            select.appendChild(addOption);
        }

        onRendered(function () {
            $(select).select2({
                dropdownParent: document.body,
                width: 'resolve',
                placeholder: "Select value",
                templateResult: function (data) {
                    if (data.id === "__add_new__") {
                        return $('<span style="color: blue;"><i class="fas fa-plus-circle"></i> ' + data.text + '</span>');
                    }
                    return data.text;
                },
                templateSelection: function (data) {
                    return values[data.id] || data.text;
                }
            }).on("select2:select", function (e) {
                const selectedVal = select.value;

                if (selectedVal === "__add_new__") {
                    $(select).select2('close');
                    cancel(); // cancel cell edit
                    selectedLabCell = cell; // store the cell
                    $('#labPaymentModel').modal('show');
                    loadLabData();
                } else {
                    success(selectedVal);
                }
            });
        });

        return select;
    }
});


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
                            url: '/PaymentTracker/Delete',
                            type: 'POST',
                            data: { id: recid }
                        }).done(function (data) {
                            if (data && data.success === true) {
                                showSuccessNewAlert("Payment Detail deleted successfully.");
                                // remove row immediately
                                const rowEl = $(element).closest(".tabulator-row")[0];
                                const row = table.getRow(rowEl);
                                if (row) row.delete();
                                // reload using SAME filter (globals preserved)
                                loadData();
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

//function delConfirm(recid, element) {
//    debugger;

//    if (!recid || recid <= 0) {
//        const rowEl = $(element).closest(".tabulator-row")[0];
//        const row = table.getRow(rowEl);
//        if (row) {
//            row.delete();
//        }
//        return;
//    }

//    PNotify.prototype.options.styling = "bootstrap3";
//    (new PNotify({
//        title: 'Confirmation Needed',
//        text: 'Are you sure to delete? It will not delete if this record is used in transactions.',
//        icon: 'glyphicon glyphicon-question-sign',
//        hide: false,
//        confirm: {
//            confirm: true
//        },
//        buttons: {
//            closer: false,
//            sticker: false
//        },
//        history: {
//            history: false
//        },
//    })).get().on('pnotify.confirm', function () {

//        $.ajax({
//            url: '/PaymentTracker/Delete',
//            type: 'POST',
//            data: { id: recid },
//            success: function (data) {
//                if (data.success == true) {
//                    showSuccessAlert("Payment Detail Deleted successfully.");
//                    setTimeout(function () {
//                        window.location.reload();
//                    }, 2500);
//                }
//                else if (data.success == false && data.message == "Not_Deleted") {
//                    showDangerAlert("Record is used in QMS Log transactions.");
//                }
//                else {
//                    showDangerAlert(data.message);
//                }
//            },
//            error: function () {
//                showDangerAlert('Error retrieving data.');
//            }
//        });
//    }).on('pnotify.cancel', function () {
//        loadData();
//    });
//}



function InsertUpdatePayment(rowData) {
    debugger
    if (!rowData) {
        showDangerAlert("Invalid data provided.");
        return;
    }

    //Blockloadershow();
    var errorMsg = "";

    if (errorMsg !== "") {
        //Blockloaderhide();
        showDangerAlert(errorMsg);
        return false;
    }

    function toIsoDate(value) {
        if (!value || value === "" || value === "Invalid Date") return null;

        if (typeof value === "string" && value.includes("/")) {
            const parts = value.split("/");
            if (parts.length === 3) {
                const [day, month, year] = parts;
                return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
            }
        }

        const parsed = new Date(value);
        return isNaN(parsed.getTime()) ? null : parsed.toISOString().substring(0, 10);
    }

    var Model = {
        Id: rowData.Id || 0,
        Fin_Year: rowData.Fin_Year || null,
        Month: rowData.Month || null,
        Lab: rowData.Lab || null,
        Vendor: rowData.Vendor || null,
        Type_Test: rowData.Type_Test || null,
        Description: rowData.Description || null,
        Bis_Id: rowData.Bis_Id || null,
        Invoice_No: rowData.Invoice_No || null,
        Invoice_Date: toIsoDate(rowData.Invoice_Date),
        Testing_Fee: rowData.Testing_Fee || 0,
        Approval_By: rowData.Approval_By || null,
        Remark: rowData.Remark || null,
    };

    const isNew = Model.Id === 0;
    var ajaxUrl = isNew ? '/PaymentTracker/Create' : '/PaymentTracker/Update';

    $.ajax({
        url: ajaxUrl,
        type: "POST",
        data: JSON.stringify(Model),
        contentType: 'application/json',
        success: function (response) {
            //Blockloaderhide();
            if (response.success) {
                //const msg = Model.Id != 0
                //    ? "Payment Tracker updated successfully!"
                //    : "Payment Tracker saved successfully!";
                //showSuccessAlert(msg);
                if (isNew) {
                    showSuccessAlert("Saved successfully!.");
                    loadData();
                }
                //loadData();
            }
            else if (response.message === "Exist") {
                showDangerAlert("Payment Tracker Detail already exists.");
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
        error: function (xhr, status, error) {
            //Blockloaderhide();
            showDangerAlert("An unexpected error occurred. Please refresh the page and try again.");
        }
    });
}

function loadLabData() {
    Blockloadershow();
    $.ajax({
        url: '/PaymentTracker/GetLabPayment',
        type: 'GET',
        success: function (data) {
            Blockloaderhide();
            if (data && Array.isArray(data)) {
                OnNatLabGridLoad(data);
            } else {
                showDangerAlert('No data available to load.');
            }
        },
        error: function (xhr, status, error) {
            Blockloaderhide();
            showDangerAlert('Error retrieving data: ' + error);
        }
    });
}

function OnNatLabGridLoad(response) {
    debugger;
    Blockloadershow();

    tabledataLab = [];
    let columns = [];

    // Map the response to the table format
    if (response.length > 0) {
        $.each(response, function (index, item) {

            function formatDate(value) {
                return value ? new Date(value).toLocaleDateString("en-GB") : "";
            }

            tabledataLab.push({
                Sr_No: index + 1,
                Id: item.id,
                Lab: item.lab,
                CreatedBy: item.createdBy,
                UpdatedBy: item.updatedBy,
                UpdatedDate: formatDate(item.updatedDate),
                CreatedDate: formatDate(item.createdDate),
            });
        });
    }

    if (tabledataLab.length === 0 && tableLab) {
        tableLab.clearData();
        Blockloaderhide();
        return;
    }

    columns.push(
        {
            title: "Action",
            field: "Action",
            width: 46,
            hozAlign: "center",
            headerHozAlign: "center",
            formatter: function (cell, formatterParams) {
                const rowData = cell.getRow().getData();
                let actionButtons = "";

                actionButtons += `<i onclick="delLabConfirm(${rowData.Id},this)" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>`

                return actionButtons;
            }
        },
        {
            title: "SNo", field: "Sr_No", width: 48, sorter: "number", hozAlign: "center", headerHozAlign: "left"
        },
        editableColumn("Lab", "Lab", true),
        { title: "CreatedBy", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Created Date", field: "CreatedDate", width: 129, sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
    );

    // // Initialize Tabulator
    tableLab = new Tabulator("#lab_Table", {
        data: tabledataLab,
        renderHorizontal: "virtual",
        movableColumns: true,
        pagination: "local",
        paginationSize: 10,
        paginationSizeSelector: [50, 100, 500, 1500, 2000],
        paginationCounter: "rows",
        dataEmpty: "<div style='text-align: center; font-size: 1rem; color: gray;'>No data available</div>", // Placeholder message
        columns: columns
    });

    tableLab.on("cellEdited", function (cell) {
        InsertUpdateLab(cell.getRow().getData());
    });

    $("#addLabBtn").on("click", function () {
        const newRow1 = {
            Sr_No: tableLab.getDataCount() + 1,
            Id: 0,
            Lab: "",
            CreatedBy: "",
            UpdatedBy: "",
            UpdatedDate: "",
            CreatedDate: ""
        };
        tableLab.addRow(newRow1, false);
    });


    Blockloaderhide();
}

function InsertUpdateLab(rowData) {
    debugger
    if (!rowData) {
        showDangerAlert("Invalid data provided.");
        return;
    }

    //Blockloadershow();
    var errorMsg = "";

    if (errorMsg !== "") {
        Blockloaderhide();
        showDangerAlert(errorMsg);
        return false;
    }

    var Model = {
        Id: rowData.Id || 0,
        Lab: rowData.Lab || null,
    };

    var ajaxUrl = Model.Id === 0 ? '/PaymentTracker/CreateLabPayment' : '/PaymentTracker/UpdateLabPayment';

    $.ajax({
        url: ajaxUrl,
        type: "POST",
        data: JSON.stringify(Model),
        contentType: 'application/json',
        success: function (response) {
            //Blockloaderhide();
            if (response.success) {
                const msg = Model.Id != 0
                    ? "Lab updated successfully!"
                    : "Lab saved successfully!";
                showSuccessAlert(msg);
                loadLabData();
            }
            else if (response.message === "Exist") {
                showDangerAlert("Lab already exists.");
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
        error: function (xhr, status, error) {
            //Blockloaderhide();
            showDangerAlert("An unexpected error occurred. Please refresh the page and try again.");
        }
    });
}

$('#labPaymentModel').on('hidden.bs.modal', function () {
    loadData(); // uncomment if you want full reload
});


function delLabConfirm(recid, element) {
    debugger;

    if (!recid || recid <= 0) {
        const rowEl = $(element).closest(".tabulator-row")[0];
        const row = tableLab.getRow(rowEl);
        if (row) {
            row.delete();
        }
        return;
    }

    PNotify.prototype.options.styling = "bootstrap3";
    (new PNotify({
        title: 'Confirmation Needed',
        text: 'Are you sure to delete? It will not delete if this record is used in transactions.',
        icon: 'glyphicon glyphicon-question-sign',
        hide: false,
        confirm: {
            confirm: true
        },
        buttons: {
            closer: false,
            sticker: false
        },
        history: {
            history: false
        },
    })).get().on('pnotify.confirm', function () {
        $.ajax({
            url: '/PaymentTracker/DeleteLabPayment',
            type: 'POST',
            data: { id: recid },
            success: function (data) {
                if (data.success == true) {
                    showSuccessAlert("Nature of Project Deleted successfully.");
                    setTimeout(function () {
                        window.location.reload();
                    }, 2500);
                }
                else if (data.success == false && data.message == "Not_Deleted") {
                    showDangerAlert("Record is used in QMS Log transactions.");
                }
                else {
                    showDangerAlert(data.message);
                }
            },
            error: function () {
                showDangerAlert('Error retrieving data.');
            }
        });
    }).on('pnotify.cancel', function () {
        loadLabData();
    });
}




