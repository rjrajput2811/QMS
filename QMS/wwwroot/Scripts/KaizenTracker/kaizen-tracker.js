var table = null;
let vendorOptions = {};
let filterStartKaizDate = moment().startOf('month').format('YYYY-MM-DD');
let filterEndKaizDate = moment().endOf('month').format('YYYY-MM-DD');

$(document).ready(function () {

    $('#dateRangeKaiz').text(
        moment(filterStartKaizDate).format('MMMM D, YYYY') + ' - ' + moment(filterEndKaizDate).format('MMMM D, YYYY')
    );

    // Initialize Litepicker
    const picker = new Litepicker({
        element: document.getElementById('customDateTriggerKaiz'),
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
                filterStartKaizDate = start.format('YYYY-MM-DD');
                filterEndKaizDate = end.format('YYYY-MM-DD');
                $('#dateRangeKaiz').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadData();
            });

            picker.on('clear', () => {
                filterStartKaizDate = "";
                filterEndKaizDate = "";
                $('#dateRangeKaiz').text("Select Date Range");
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

    $('#customDateTriggerKaiz').on('click', function () {
        picker.show();
    });

    $('#backButton').on('click', function () {
        window.history.back();
    });

    $('#upload-button').on('click', async function () {
        var expectedColumns = ['FY', 'Month', 'Vendor', 'Kaizen theme', 'Team', 'Kaizen','Remarks' ];

        var url = '/KaizenTracker/UploadKaizenExcel';
        handleImportExcelFile(url, expectedColumns);
    });

    loadData();
});

function loadData() {
    Blockloadershow();

    $.ajax({
        url: '/Service/GetVendor',
        type: 'GET'
    }).done(function (vendorData) {
        if (Array.isArray(vendorData)) {
            vendorOptions = vendorData.reduce((acc, v) => {
                acc[v.value] = v.label;
                return acc;
            }, {});
        }

        $.ajax({
            url: '/KaizenTracker/GetAll',
            type: 'GET',
            dataType: 'json',
            data: {
                startDate: filterStartKaizDate,
                endDate: filterEndKaizDate
            },
            success: function (data) {
                if (Array.isArray(data)) {
                    OnTabGridLoad(data);
                } else {
                    showDangerAlert('No Kaizen data available.');
                }
                Blockloaderhide();
            },
            error: function () {
                showDangerAlert('Error loading Kaizen data.');
                Blockloaderhide();
            }
        });

    }).fail(function () {
        showDangerAlert('Failed to load vendor dropdown options.');
        Blockloaderhide();
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

function getMonthOptions() {
    const months = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    const yearSuffix = new Date().getFullYear().toString().slice(-2);
    return months.map(m => `${m} - ${yearSuffix}`); // array of strings
}

function getMonthString() {
    const months = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    const today = new Date();
    const monthName = months[today.getMonth()];
    const yearSuffix = today.getFullYear().toString().slice(-2);
    return monthName + " - " + yearSuffix;
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

const KAIZEN_BASE = '@Url.Content("~/KaizenTrac_Attach/")'; // -> "/KaizenTrac_Attach/"

function buildAttachmentUrl(id, fileName) {
    if (!fileName) return '';
    return `${KAIZEN_BASE}${id}/${encodeURIComponent(fileName)}`;
}

function OnTabGridLoad(response) {
    Blockloadershow();

    let tabledata = [];
    let columns = [];

    if (response.length > 0) {
        $.each(response, function (index, item) {

            function formatDate(value) {
                return value ? new Date(value).toLocaleDateString("en-GB") : "";
            }

            tabledata.push({
                Sr_No: index + 1,
                Id: item.id,
                Vendor: item.vendor || "",
                Kaizen_Theme: item.kaizen_Theme || "",
                Month: item.month || "",
                Team: item.team || "",
                Kaizen_Attch: item.kaizen_Attch || "",
                Remark: item.remark || "",
                FY: item.fy || "",
                CreatedDate: formatDate(item.createdDate || ""),
                UpdatedDate: formatDate(item.updatedDate || ""),
                CreatedBy: item.createdBy || "",
                UpdatedBy: item.updatedBy || ""
            });
        });
    }
    console.log(tabledata);


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

        {
            title: "FY",
            field: "FY",
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

        {
            title: "Month",
            field: "Month",
            editor: "list",
            editorParams: {
                values: getMonthOptions(),   // array or object
                clearable: true
            },
            headerFilter: "list",
            headerFilterParams: { values: getMonthOptions() },
            hozAlign: "center",
            headerHozAlign: "center",
            headerMenu: headerMenu
        },

        editableColumn("Vendor", "Vendor", "select2Test", "center", "input", {}, {
            values: vendorOptions
        }, function (cell) {
            const val = cell.getValue();
            return vendorOptions[val] || val;
        }, 130),

        editableColumn("Kaizen Theme", "Kaizen_Theme", true),
        editableColumn("Team", "Team", true),
        editableColumn("Kaizen", "Kaizen_Attch", true),

        {
            title: "Remark",
            field: "Remark",
            hozAlign: "center",
            headerHozAlign: "center",
            headerMenu: headerMenu,
            formatter: function (cell, formatterParams) {
                const rowData = cell.getRow().getData();
                const fileName = cell.getValue();
                const fileDisplay = fileName
                    ? `<a href="~/KaizenTrac_Attach/${rowData.Id}/${fileName}" target="_blank">${fileName}</a><br/>`
                    : '';

                return `
            ${fileDisplay}
            <input type="file" accept=".pdf,image/*" class="form-control-file bis-upload" data-id="${cell.getRow().getData().Id}" style="width:160px;" />`;
            },
            cellClick: function (e, cell) {
                // prevent Tabulator from swallowing the file input click
                e.stopPropagation();
            }
        },

        { title: "User", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
        { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false }
    );

    // // Initialize Tabulator
    table = new Tabulator("#kaizen_table", {
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
        InsertUpdateKaizen(cell.getRow().getData());
    });

    (function bindAddButtonOnce() {
        var $btn = $("#addKaizenButton");
        $btn.attr("type", "button");                       
        $btn.off("click.addrow").on("click.addrow", function (e) {
            e.preventDefault(); e.stopPropagation();
            if ($btn.data("busy")) return;  
            $btn.data("busy", true).prop("disabled", true);

            try {
                const fyOptions = getFinancialYears() || {};
                const currentFY = Object.keys(fyOptions)[0] || "";
                const month = getMonthString() || "";

                const newRow = {
                    Sr_No: 1,               // will renumber after insert
                    Id: 0,
                    FY: currentFY,
                    Month: month,
                    Vendor: "",
                    Kaizen_Theme:  "",
                    Team: "",
                    Kaizen_Attch: "",
                    Remark: "",
                    CreatedBy: "",
                    UpdatedBy: "",
                    UpdatedDate: "",
                    CreatedDate: ""
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

    // helper to renumber Sr_No after inserts/deletes/sorts (if needed)
    document.getElementById("exportKaizenButton").addEventListener("click", async function () {
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
        const TITLE_TEXT = "KAIZEN TRACKER";

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
        const ws = wb.addWorksheet("Kaizen Tracker", {
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
        link.download = "Kaizen Tracker.xlsx";
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

$('#kaizen_table').on('change', '.bis-upload', function () {
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
        url: "/KaizenTracker/UploadKaizenAttachment",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false
    }).done(function (response) {
        if (response.success) {
            showSuccessNewAlert("File uploaded successfully.");
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

function parseDate(value) {
    if (!value) return null;
    // If value is a string in "dd/mm/yyyy", convert to Date
    if (typeof value === "string" && value.includes("/")) {
        const parts = value.split("/");
        if (parts.length === 3) {
            const [day, month, year] = parts;
            return new Date(`${year}-${month}-${day}`);
        }
    }
    return new Date(value);
}

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
        //if (fieldName === "Nat_Project") {
        //    let addOption = document.createElement("option");
        //    addOption.value = "__add_new__";
        //    addOption.text = "➕ Add New Project Type";
        //    select.appendChild(addOption);
        //}

        //onRendered(function () {
        //    $(select).select2({
        //        dropdownParent: document.body,
        //        width: 'resolve',
        //        placeholder: "Select value",
        //        templateResult: function (data) {
        //            if (data.id === "__add_new__") {
        //                return $('<span style="color: blue;"><i class="fas fa-plus-circle"></i> ' + data.text + '</span>');
        //            }
        //            return data.text;
        //        },
        //        templateSelection: function (data) {
        //            return values[data.id] || data.text;
        //        }
        //    }).on("select2:select", function (e) {
        //        const selectedVal = select.value;

        //        if (selectedVal === "__add_new__") {
        //            $(select).select2('close');
        //            cancel(); // cancel cell edit
        //            selectedNatProjectCell = cell; // store the cell
        //            $('#natProjectModel').modal('show');
        //            loadNatProjectData();
        //        } else {
        //            success(selectedVal);
        //        }
        //    });
        //});

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
                            url: '/KaizenTracker/Delete',
                            type: 'POST',
                            data: { id: recid }
                        }).done(function (data) {
                            if (data && data.success === true) {
                                showSuccessNewAlert("Kaizen Detail deleted successfully.");
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
    //function fileFormatter(cell, formatterParams, onRendered) {

    //    return `<button class="btn btn-sm btn-outline-primary">Upload</button>`;
    //}


    //function fileEditor(cell, onRendered, success, cancel, editorParams) {
    //    const input = document.createElement("input");
    //    input.setAttribute("type", "file");
    //    input.style.width = "100%";
    //    console.log(cell);
    //    input.addEventListener("change", function (e) {
    //        const file = e.target.files[0];
    //        if (!file) {
    //            cancel();
    //            return;
    //        }
    //        const rowData = cell.getRow().getData();
    //        const kId = rowData.Id;
    //        console.log(kId);
    //        if (!kId || kId === 0) {
    //            showDangerAlert("Please save the record before uploading a file.");
    //            cancel();
    //            return;
    //        }
    //        uploadCAPAFile(kId, file);
    //        cancel();  // Close editor immediately as upload is async
    //    });

    //    onRendered(() => {
    //        input.focus();
    //        input.style.height = "100%";
    //    });

    //    return input;
    //}
    //const columns = [
    //    {
    //        title: "Action",
    //        field: "Action",
    //        frozen: true,
    //        hozAlign: "center", headerMenu: headerMenu,
    //        width: 130,
    //        formatter: function (cell) {
    //            const rowData = cell.getRow().getData();
    //            return `<i onclick="delConfirm(${rowData.Id})" class="fas fa-trash-alt text-danger" title="Delete" style="cursor:pointer;"></i>`;
    //        }
    //    },
    //    { title: "ID", field: "Id", hozAlign: "center", frozen: true, visible: false },
    //    { title: "SNo", field: "Sr_No", frozen: true, hozAlign: "center", headerMenu: headerMenu, width: 110 },

    //    editableColumn("Vendor", "Vendor", "select2", "center", null, {}, { values: vendorOptions }, function (cell) {
    //        const val = cell.getValue();
    //        return vendorOptions[val] || val;
    //    }, 150),

    //    editableColumn("Kaizen Theme", "KaizenTheme", "input", "center", null, {}, {}, null, 200),

    //    editableColumn("Kaizen Month", "KMonth", "input", "center", null, {}, {}, null, 170),

    //    editableColumn("Team", "Team", "input", "center", null, {}, {}, null, 120),
    //    editableColumn("Remarks", "Remarks", "input", "center", null, {}, {}, null, 150),
    //    {
    //        title: "Kaizen File",
    //        field: "KaizenFile",
    //        formatter: fileFormatter,
    //        editor: fileEditor,
    //        hozAlign: "center",
    //        headerMenu: headerMenu,
    //        width: 140
    //    },
    //    {
    //        title: "Attachment",
    //        field: "KaizenFile",
    //        formatter: function (cell) {
    //            const value = cell.getValue();
    //            if (!value) return "";
    //            const files = value.split(/[,;]+/).map(f => f.trim()).filter(Boolean);
    //            return files.map(path =>
    //                `<a href="/${path}" target="_blank" download title="Download">
    //                <i class="fas fa-download text-primary"></i>
    //            </a>`
    //            ).join(" ");
    //        },
    //        // editor: fileEditor,
    //        hozAlign: "center", headerMenu: headerMenu,

    //        width: 140
    //    },




    //    // Optional metadata columns (hidden)
    //    { title: "Created By", field: "CreatedBy", hozAlign: "center", visible: false },
    //    { title: "Created Date", field: "CreatedDate", hozAlign: "center", visible: false },
    //    { title: "Updated By", field: "UpdatedBy", hozAlign: "center", visible: false },
    //    { title: "Updated Date", field: "UpdatedDate", hozAlign: "center", visible: false }

    //];

    //if (table) {
    //    table.replaceData(tabledata);
    //} else {
    //    table = new Tabulator("#kaizen_table", {
    //        data: tabledata,
    //        layout: "fitDataFill",
    //        movableColumns: true,
    //        pagination: "local",
    //        paginationSize: 10,
    //        paginationSizeSelector: [10, 50, 100, 500],
    //        paginationCounter: "rows",
    //        placeholder: "No data available",
    //        columns: columns
    //    });

    //    table.on("cellEdited", function (cell) {
    //        const rowData = cell.getRow().getData();
    //        saveKaizenTrackerRow(rowData);
    //    });
    //}

    //$("#addButton").on("click", function () {
    //    const newRow = {
    //        Id: 0,
    //        Sr_No: table.getDataCount() + 1,
    //        Vendor: "",
    //        KaizenTheme: "",
    //        KMonth: "",
    //        Team: "", Remarks: "",
    //        KaizenFile: ""
    //        /*CreatedBy: "", UpdatedBy: "", UpdatedDate: "", CreatedDate: ""*/
    //    };
    //    table.addRow(newRow, false);
    //});

//    Blockloaderhide();
//}
//function uploadCAPAFile(kId, file) {
 
//    var formData = new FormData();
//    formData.append("file", file);
//    formData.append("kId", kId);
//    console.log(kId);
//    $.ajax({
//        url: '/KaizenTracker/UploadFile',
//        type: 'POST',
//        data: formData,
//        processData: false,
//        contentType: false,
//        success: function (response) {
//            if (response.success) {
//                showSuccessAlert("File uploaded and record updated!");
               
//            }
//            else { showDangerAlert(response.message) }
           
//            loadData(); 
//        },
//        error: function (xhr) {
//            showDangerAlert("File upload failed: " + (xhr.responseJSON?.message || "Unknown error"));
//        }
//    });
//}



//function editableColumn(
//    title,
//    field,
//    editorType = true,
//    align = "center",
//    headerFilterType = "input",
//    headerFilterParams = {},
//    editorParams = {},
//    formatter = null,
//    width = null
//) {
//    let columnDef = {
//        title: title,
//        field: field,
//        editor: editorType,
//        editorParams: editorParams,
//        formatter: formatter,
//        headerFilter: headerFilterType,
//        headerFilterParams: headerFilterParams,
//        headerMenu: headerMenu, 
//        hozAlign: align,
//        headerHozAlign: "left"
//    };

//    if (width) {
//        columnDef.width = width;
//    }

//    return columnDef;
//}


//function delConfirm(id) {
//    PNotify.prototype.options.styling = "bootstrap3";
//    (new PNotify({
//        title: 'Confirm Deletion',
//        text: 'Are you sure you want to delete this Kaizen record?',
//        icon: 'fa fa-question-circle',
//        hide: false,
//        confirm: { confirm: true },
//        buttons: { closer: false, sticker: false },
//        history: { history: false }
//    })).get().on('pnotify.confirm', function () {
//        $.ajax({
//            url: '/KaizenTracker/Delete',
//            type: 'POST',
//            data: { id: id },
//            success: function (data) {
//                if (data.success) {
//                    showSuccessAlert("Deleted successfully.");
//                    setTimeout(() => loadData(), 1500);
//                } else {
//                    showDangerAlert(data.message || "Deletion failed.");
//                }
//            },
//            error: function () {
//                showDangerAlert('Error occurred during deletion.');
//            }
//        });
//    });
//}

//Tabulator.extendModule("edit", "editors", {
//    select2: function (cell, onRendered, success, cancel, editorParams) {
//        const values = editorParams.values || {};
//        const select = document.createElement("select");
//        select.style.width = "100%";

//        for (let val in values) {
//            let option = document.createElement("option");
//            option.value = val;
//            option.text = values[val];
//            select.appendChild(option);
//        }

//        select.value = cell.getValue();

//        onRendered(function () {
//            $(select).select2({
//                dropdownParent: $('body')
//            }).focus();

//            $(select).on("change", function () {
//                success(this.value);
//            });

//            $(select).on("blur", function () {
//                cancel();
//            });
//        });

//        return select;
//    }
//});

function InsertUpdateKaizen(rowData) {
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
    console.log(rowData);

    const cleanedData = {
        Id: rowData.Id || 0,
        Vendor: rowData.Vendor || null,
        Kaizen_Theme: rowData.Kaizen_Theme || null,
        Month: rowData.Month || null,
        Team: rowData.Team || null,
        Kaizen_Attch: rowData.Kaizen_Attch || null,
        Remark: rowData.Remark || null,
        FY : rowData.FY || null
    };

    console.log(cleanedData);
    const isNew = cleanedData.Id === 0;
    const url = isNew ? '/KaizenTracker/Create' : '/KaizenTracker/Update';

    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify(cleanedData),
        contentType: 'application/json',
        success: function (data) {
            if (data.success) {
                if (isNew) {
                    showSuccessNewAlert("Saved successfully!.");
                    loadData();
                }
            }
            else if (response.message === "Exist") {
                showDangerAlert("Kaizen Tracker Detail already exists.");
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

function clearForm() {
    // Clear all input fields
    document.querySelectorAll('.form-control').forEach(function (input) {
        if (input.tagName === 'INPUT') {
            if (input.type === 'hidden' || input.readOnly) {
                // Skip hidden or readonly inputs
                return;
            }
            input.value = ''; // Clear input value
        } else if (input.tagName === 'SELECT') {
            input.selectedIndex = 0; // Reset dropdown to first option
        }
    });

    // Clear error messages if needed
    document.querySelectorAll('.text-danger').forEach(function (error) {
        error.textContent = '';
    });
}
function openUpload() {

    clearForm();
    if (!$('#uploadModal').length) {
        $('body').append(partialView);
    }
    $('#uploadModal').modal('show');
}

$('#download-template').on('click', async function () {
    var expectedColumns = ['FY', 'Month', 'Vendor', 'Kaizen theme', 'Team', 'Kaizen', 'Remarks'];

    // Create a new workbook
    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet("Kaizen Tracker");

    // Add header row
    worksheet.addRow(expectedColumns);

    // Style header row (optional)
    worksheet.getRow(1).eachCell(cell => {
        cell.font = { bold: true };
        cell.alignment = { horizontal: 'center' };
        cell.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFD9D9D9' } // light gray
        };
    });

    // Generate Excel file and trigger download
    const buffer = await workbook.xlsx.writeBuffer();
    const blob = new Blob([buffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
    const link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    link.download = "Kaizen_Tracker.xlsx";
    link.click();
});