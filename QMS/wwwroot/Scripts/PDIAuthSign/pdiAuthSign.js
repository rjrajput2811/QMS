var table = null;
let vendorOptions = {};
let vendorDetails = {};
let filterStartPDIAuthDate = moment().startOf('month').format('YYYY-MM-DD');
let filterEndPDIAuthDate = moment().endOf('month').format('YYYY-MM-DD');

$(document).ready(function () {

    $('#dateRangeTextPDIAuth').text(
        moment(filterStartPDIAuthDate).format('MMMM D, YYYY') + ' - ' + moment(filterEndPDIAuthDate).format('MMMM D, YYYY')
    );

    // Initialize Litepicker
    const picker = new Litepicker({
        element: document.getElementById('customDateTriggerPDIAuth'),
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
                filterStartPDIAuthDate = start.format('YYYY-MM-DD');
                filterEndPDIAuthDate = end.format('YYYY-MM-DD');
                $('#dateRangeTextPDIAuth').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadData();
            });

            picker.on('clear', () => {
                filterStartPDIAuthDate = "";
                filterEndPDIAuthDate = "";
                $('#dateRangeTextPDIAuth').text("Select Date Range");
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

    $('#customDateTriggerPDIAuth').on('click', function () {
        picker.show();
    });

    $('#backButton').on('click', function () {
        window.history.back();
    });

    loadData();
});

function loadData() {
    Blockloadershow();

    $.ajax({
        url: '/PDIAuthSign/GetVendor',
        type: 'GET'
    }).done(function (vendorData) {
        if (Array.isArray(vendorData)) {
            vendorOptions = vendorData.reduce((acc, v) => {
                acc[v.value] = v.label;
                return acc;
            }, {});

            vendorDetails = vendorData.reduce((acc, v) => {
                acc[v.value] = {
                    label: v.label,
                    address: v.address ?? v.Address ?? ""
                };
                return acc;
            }, {});
        }

        $.ajax({
            url: '/PDIAuthSign/GetAll',
            type: 'GET',
            dataType: 'json',
            data: {
                startDate: filterStartPDIAuthDate,
                endDate: filterEndPDIAuthDate
            },
            success: function (data) {
                if (Array.isArray(data)) {
                    OnTabGridLoad(data);
                } else {
                    showDangerAlert('No PDI Auth Signatory data available.');
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
                Address: item.address || "",
                Pdi_Inspector: item.pdi_Inspector || "",
                Designation: item.designation || "",
                Photo_Inspector: item.photo_Inspector || "",
                Specimen_Sign: item.specimen_Sign || "",
                Remark: item.remark || "",
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
            width: 60,
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

        editableColumn("Vendor", "Vendor", "select2Test", "center", "input", {}, {
            values: vendorOptions
        }, function (cell) {
            const val = cell.getValue();
            return vendorOptions[val] || val;
        }, 241),

        editableColumn("Address", "Address", true),
        editableColumn("Name of PDI Inspector", "Pdi_Inspector", true),
        editableColumn("Designation", "Designation", true),

        {
            title: "Photo of Inspector",
            field: "Photo_Inspector",
            hozAlign: "center",
            headerHozAlign: "center",
            headerMenu: headerMenu,
            formatter: function (cell, formatterParams) {
                const rowData = cell.getRow().getData();
                const fileName = cell.getValue();
                const fileDisplay = fileName
                    ? `<a href="/PDIAuthSign_Attach/${rowData.Id}/${fileName}" target="_blank">${fileName}</a><br/>`
                    : '';

                return `
            ${fileDisplay}
            <input type="file" accept=".pdf,image/*" class="form-control-file bis-upload" data-id="${cell.getRow().getData().Id}" data-type="Photo" style="width:160px;" />`;
            },
            cellClick: function (e, cell) {
                // prevent Tabulator from swallowing the file input click
                e.stopPropagation();
            }
        },

        {
            title: "Specimen Signature",
            field: "Specimen_Sign",
            hozAlign: "center",
            headerHozAlign: "center",
            headerMenu: headerMenu,
            formatter: function (cell, formatterParams) {
                const rowData = cell.getRow().getData();
                const fileName = cell.getValue();
                const fileDisplay = fileName
                    ? `<a href="/PDIAuthSign_Attach/${rowData.Id}/${fileName}" target="_blank">${fileName}</a><br/>`
                    : '';

                return `
            ${fileDisplay}
            <input type="file" accept=".pdf,image/*" class="form-control-file bis-upload" data-id="${cell.getRow().getData().Id}" data-type="Sign" style="width:160px;" />`;
            },
            cellClick: function (e, cell) {
                // prevent Tabulator from swallowing the file input click
                e.stopPropagation();
            }
        },

        editableColumn("Remark", "Remark", true),

        { title: "User", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
        { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
        { title: "Id", field: "Id", visible: false }
    );

    // // Initialize Tabulator
    table = new Tabulator("#pdiAuth_table", {
        data: tabledata,
        renderHorizontal: "virtual",
        movableColumns: true,
        pagination: "local",
        paginationSize: 10,
        paginationSizeSelector: [50, 100, 500, 1500, 2000],
        paginationCounter: "rows",
        dataEmpty: "<div style='text-align: center; font-size: 1rem; color: gray;'>No data available</div>", // Placeholder message
        columns: columns,
        index: "Id"
    });

    table.on("cellEdited", function (cell) {

        const field = cell.getField();

        // Auto-fill address when vendor changes
        if (field === "Vendor") {
            const vendorId = String(cell.getValue() ?? "");
            const info = vendorDetails[vendorId];
            if (info && info.address) {
                cell.getRow().update({ Address: info.address });
            }
        }

        InsertUpdatePDIAuth(cell.getRow().getData());
    });

    (function bindAddButtonOnce() {
        var $btn = $("#addPDIAuthButton");
        $btn.attr("type", "button");
        $btn.off("click.addrow").on("click.addrow", function (e) {
            e.preventDefault(); e.stopPropagation();
            if ($btn.data("busy")) return;
            $btn.data("busy", true).prop("disabled", true);

            try {

                const newRow = {
                    Sr_No: 1,               // will renumber after insert
                    Id: 0,
                    Vendor: "",
                    Address: "",
                    Pdi_Inspector: "",
                    Designation: "",
                    Photo_Inspector: "",
                    Specimen_Sign: "",
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
    //document.getElementById("exportPDIAuthButton").addEventListener("click", async function () {
    //    const EXPORT_SCOPE = "active"; // "active" | "selected" | "all"
    //    const EXPORT_RAW = false;    // false = use display/labels

    //    // ===== 1) Build columns list from visible Tabulator columns (exclude Action/User)
    //    if (!window.table) { console.error("Tabulator 'table' not found."); return; }

    //    const EXCLUDE_FIELDS = new Set(["Action", "action", "Actions", "CreatedBy"]);
    //    const EXCLUDE_TITLES = new Set(["Action", "Actions", "User"]);

    //    const tabCols = table.getColumns(true)
    //        .filter(c => c.getField())
    //        .filter(c => c.isVisible())
    //        .filter(c => {
    //            const def = c.getDefinition();
    //            const field = def.field || "";
    //            const title = (def.title || "").trim();
    //            return !EXCLUDE_FIELDS.has(field) && !EXCLUDE_TITLES.has(title);
    //        });

    //    const excelCols = tabCols.map(col => {
    //        const def = col.getDefinition();
    //        const label = def.title || def.field;
    //        const px = (def.width || col.getWidth() || 120);
    //        const width = Math.max(8, Math.min(40, Math.round(px / 7))); // px->char heuristic
    //        return { label, key: def.field, width };
    //    });

    //    if (!excelCols.length) { alert("No visible columns to export."); return; }

    //    // ===== 2) DOC DETAILS (fixed two rightmost columns)
    //    const docDetails = [
    //        ["Document No", "WCIB/LS/QA/R/005"],
    //        ["Effective Date", "01/10/2022"],
    //        ["Revision No", "0"],
    //        ["Revision Date", "01/10/2022"],
    //        ["Page No", "1 of 1"]
    //    ];

    //    // ===== 3) Layout constants =====
    //    const TOTAL_COLS = excelCols.length;
    //    const HEADER_TOP = 1;
    //    const HEADER_BOTTOM = 5;
    //    const GRID_HEADER_ROW = HEADER_BOTTOM + 1;
    //    const TITLE_TEXT = "PDI AUTHORISED SIGNATORY";

    //    // Logo block (A1:B5)
    //    const LOGO_COL_START = 1, LOGO_COL_END = 2;
    //    const LOGO_ROW_START = HEADER_TOP, LOGO_ROW_END = HEADER_BOTTOM;

    //    const TITLE_COL_START = Math.min(3, TOTAL_COLS);
    //    const TITLE_COL_END = Math.max(TITLE_COL_START, TOTAL_COLS - 2);

    //    const DETAILS_LABEL_COL = Math.max(1, TOTAL_COLS - 1);
    //    const DETAILS_VALUE_COL = TOTAL_COLS;

    //    // ===== 4) Helpers =====
    //    async function fetchAsBase64(url) {
    //        const res = await fetch(url);
    //        const blob = await res.blob();
    //        return new Promise((resolve) => {
    //            const reader = new FileReader();
    //            reader.onloadend = () => resolve(reader.result.split(",")[1]);
    //            reader.readAsDataURL(blob);
    //        });
    //    }
    //    function setBorder(cell, style = "thin") {
    //        cell.border = { top: { style }, bottom: { style }, left: { style }, right: { style } };
    //    }
    //    function outlineRange(ws, r1, c1, r2, c2, style = "thin") {
    //        for (let c = c1; c <= c2; c++) {
    //            const top = ws.getCell(r1, c), bottom = ws.getCell(r2, c);
    //            top.border = { ...top.border, top: { style } };
    //            bottom.border = { ...bottom.border, bottom: { style } };
    //        }
    //        for (let r = r1; r <= r2; r++) {
    //            const left = ws.getCell(r, c1), right = ws.getCell(r, c2);
    //            left.border = { ...left.border, left: { style } };
    //            right.border = { ...right.border, right: { style } };
    //        }
    //    }

    //    // ===== 5) Workbook / Sheet =====
    //    const wb = new ExcelJS.Workbook();
    //    const ws = wb.addWorksheet("PDI Authorised Signatory", {
    //        properties: { defaultRowHeight: 15 },
    //        views: [{ state: "frozen", xSplit: 0, ySplit: GRID_HEADER_ROW }] // sticky header
    //    });

    //    ws.columns = excelCols.map(c => ({ key: c.key, width: c.width }));

    //    // Row heights so logo fits
    //    for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) ws.getRow(r).height = 18;

    //    ws.pageSetup = {
    //        orientation: "landscape",
    //        fitToPage: true,
    //        fitToWidth: 1,
    //        fitToHeight: 0,
    //        margins: { left: 0.3, right: 0.3, top: 0.5, bottom: 0.5, header: 0.2, footer: 0.2 },
    //        printTitlesRow: `${HEADER_TOP}:${GRID_HEADER_ROW}`
    //    };

    //    // ===== 6) Header band fill =====
    //    for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) {
    //        for (let c = 1; c <= TOTAL_COLS; c++) {
    //            ws.getCell(r, c).fill = { type: "pattern", pattern: "solid", fgColor: { argb: "FFF7F7F7" } };
    //        }
    //    }

    //    // ===== 7) Logo centered in A1:B5 (no stretch) + outline =====
    //    const LOGO_WIDTH_PX = 100, LOGO_HEIGHT_PX = 100;

    //    const COL_PX = (c) => ((ws.getColumn(c).width || 8) * 7);
    //    const ROW_PX = (r) => ((ws.getRow(r).height || 15) * (96 / 72));

    //    let rectWpx = 0; for (let c = LOGO_COL_START; c <= LOGO_COL_END; c++) rectWpx += COL_PX(c);
    //    let rectHpx = 0; for (let r = LOGO_ROW_START; r <= LOGO_ROW_END; r++) rectHpx += ROW_PX(r);

    //    const avgColPx = rectWpx / (LOGO_COL_END - LOGO_COL_START + 1);
    //    const avgRowPx = rectHpx / (LOGO_ROW_END - LOGO_ROW_START + 1);

    //    const logoCols = LOGO_WIDTH_PX / avgColPx;
    //    const logoRows = LOGO_HEIGHT_PX / avgRowPx;

    //    const tlCol = (LOGO_COL_START - 1) + ((LOGO_COL_END - LOGO_COL_START + 1) - logoCols) / 2;
    //    const tlRow = (LOGO_ROW_START - 1) + ((LOGO_ROW_END - LOGO_ROW_START + 1) - logoRows) / 2;

    //    const logoUrl = window.LOGO_URL || (window.APP_BASE && (window.APP_BASE + "images/wipro-logo.png"));
    //    if (logoUrl) {
    //        try {
    //            const base64 = await fetchAsBase64(logoUrl);
    //            const imgId = wb.addImage({ base64, extension: "png" });
    //            ws.addImage(imgId, {
    //                tl: { col: tlCol, row: tlRow },
    //                ext: { width: LOGO_WIDTH_PX, height: LOGO_HEIGHT_PX },
    //                editAs: "oneCell"
    //            });
    //        } catch (e) { console.warn("Logo load failed:", e); }
    //    }
    //    outlineRange(ws, LOGO_ROW_START, LOGO_COL_START, LOGO_ROW_END, LOGO_COL_END, "thin");

    //    // ===== 8) Title (merge) + outline =====
    //    ws.mergeCells(HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END);
    //    const titleCell = ws.getCell(HEADER_TOP, TITLE_COL_START);
    //    titleCell.value = TITLE_TEXT;
    //    titleCell.font = { bold: true, size: 18 };
    //    titleCell.alignment = { horizontal: "center", vertical: "middle" };
    //    outlineRange(ws, HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END, "thin");

    //    // ===== 9) Document details in the last two columns =====
    //    const detailsRowsEnd = HEADER_TOP + docDetails.length - 1;
    //    docDetails.forEach((pair, i) => {
    //        const r = HEADER_TOP + i;
    //        const labelCell = ws.getCell(r, DETAILS_LABEL_COL);
    //        const valueCell = ws.getCell(r, DETAILS_VALUE_COL);

    //        labelCell.value = pair[0];
    //        valueCell.value = pair[1];

    //        labelCell.font = { bold: true };
    //        [labelCell, valueCell].forEach(cell => {
    //            cell.alignment = { vertical: "middle", horizontal: "left", wrapText: true };
    //            setBorder(cell, "thin");
    //        });
    //    });
    //    outlineRange(ws, HEADER_TOP, DETAILS_LABEL_COL, detailsRowsEnd, DETAILS_VALUE_COL, "thin");

    //    // ===== 10) Manual table header row =====
    //    while (ws.rowCount < GRID_HEADER_ROW - 1) ws.addRow([]); // pad to row before header

    //    const headerTitles = excelCols.map(c => c.label);
    //    const headerRow = ws.addRow(headerTitles);
    //    headerRow.height = 22;
    //    headerRow.eachCell((cell) => {
    //        cell.font = { bold: true };
    //        cell.alignment = { horizontal: "center", vertical: "middle", wrapText: true };
    //        cell.fill = { type: "pattern", pattern: "solid", fgColor: { argb: "FFD9E1F2" } };
    //        setBorder(cell);
    //    });

    //    // ===== 11) DATA ROWS (use display text for dropdowns) =====
    //    let tabRows;
    //    switch (EXPORT_SCOPE) {
    //        case "selected": tabRows = table.getSelectedRows(); break;
    //        case "all": tabRows = table.getRows(); break;
    //        case "active":
    //        default: tabRows = table.getRows("active"); break;
    //    }

    //    tabRows.forEach(row => {
    //        const cells = row.getCells();
    //        const byField = {};

    //        cells.forEach(cell => {
    //            const f = cell.getField();
    //            if (!f) return;

    //            const def = cell.getColumn().getDefinition();
    //            const title = (def.title || "").trim();

    //            if (EXCLUDE_FIELDS.has(f) || EXCLUDE_TITLES.has(title)) return;

    //            byField[f] = EXPORT_RAW ? row.getData()[f] : getDisplayValue(cell);
    //        });

    //        const values = excelCols.map(c => byField[c.key] ?? "");
    //        const xRow = ws.addRow(values);

    //        xRow.eachCell((cell, colNumber) => {
    //            cell.alignment = {
    //                vertical: "middle",
    //                horizontal: colNumber === 1 ? "center" : "left",
    //                wrapText: true
    //            };
    //            setBorder(cell);
    //        });
    //    });

    //    // ===== 12) DOWNLOAD =====
    //    const buffer = await wb.xlsx.writeBuffer();
    //    const blob = new Blob([buffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
    //    const link = document.createElement("a");
    //    link.href = URL.createObjectURL(blob);
    //    link.download = "PDI Authorised Signatory.xlsx";
    //    document.body.appendChild(link);
    //    link.click();
    //    link.remove();
    //});

    document.getElementById("exportPDIAuthButton").addEventListener("click", async function () {
        const EXPORT_SCOPE = "active"; // "active" | "selected" | "all"
        const EXPORT_RAW = false;      // false = use display/labels

        if (!window.table) { console.error("Tabulator 'table' not found."); return; }

        const EXCLUDE_FIELDS = new Set(["Action", "action", "Actions", "CreatedBy"]);
        const EXCLUDE_TITLES = new Set(["Action", "Actions", "User"]);

        // ✅ Only these two fields must be exported as images
        const IMAGE_FIELDS = new Set(["Photo_Inspector", "Specimen_Sign"]);
        const isImageField = (f) => IMAGE_FIELDS.has(f);

        // ✅ Your folder pattern
        const BASE_ATTACH_FOLDER = "/PDIAuthSign_Attach/";
        // ✅ The row field that contains the folder id (10/1/3 etc.)
        const ROW_ID_FIELD = "Id"; // <-- CHANGE if your data uses different name

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
            const width = Math.max(8, Math.min(40, Math.round(px / 7)));
            return { label, key: def.field, width };
        });

        if (!excelCols.length) { alert("No visible columns to export."); return; }

        const docDetails = [
            ["Document No", "WCIB/LS/QA/R/005"],
            ["Effective Date", "01/10/2022"],
            ["Revision No", "0"],
            ["Revision Date", "01/10/2022"],
            ["Page No", "1 of 1"]
        ];

        const TOTAL_COLS = excelCols.length;
        const HEADER_TOP = 1;
        const HEADER_BOTTOM = 5;
        const GRID_HEADER_ROW = HEADER_BOTTOM + 1;
        const TITLE_TEXT = "PDI AUTHORISED SIGNATORY";

        const LOGO_COL_START = 1, LOGO_COL_END = 2;
        const LOGO_ROW_START = HEADER_TOP, LOGO_ROW_END = HEADER_BOTTOM;

        const TITLE_COL_START = Math.min(3, TOTAL_COLS);
        const TITLE_COL_END = Math.max(TITLE_COL_START, TOTAL_COLS - 2);

        const DETAILS_LABEL_COL = Math.max(1, TOTAL_COLS - 1);
        const DETAILS_VALUE_COL = TOTAL_COLS;

        // ===== Helpers =====
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

        function guessExt(url) {
            const u = (url || "").split("?")[0].toLowerCase();
            if (u.endsWith(".jpg") || u.endsWith(".jpeg")) return "jpeg";
            if (u.endsWith(".webp")) return "webp";
            return "png";
        }

        async function fetchAsBase64(url) {
            const res = await fetch(url, { mode: "cors" });
            if (!res.ok) throw new Error(`Image fetch failed ${res.status}: ${url}`);
            const blob = await res.blob();

            const base64 = await new Promise((resolve) => {
                const reader = new FileReader();
                reader.onloadend = () => resolve(reader.result.split(",")[1]);
                reader.readAsDataURL(blob);
            });

            const mime = (blob.type || "").toLowerCase();
            let ext = "png";
            if (mime.includes("jpeg") || mime.includes("jpg")) ext = "jpeg";
            else if (mime.includes("png")) ext = "png";
            else if (mime.includes("webp")) ext = "webp";
            else ext = guessExt(url);

            return { base64, ext };
        }

        // ✅ Build URL: /PDIAuthSign_Attach/{id}/{filename}
        function buildAttachUrl(rowData, fileNameOrUrl) {
            if (!fileNameOrUrl) return "";
            const v = String(fileNameOrUrl).trim();
            if (!v) return "";

            // already full URL
            if (/^https?:\/\//i.test(v)) return v;

            // already a rooted path
            if (v.startsWith("/")) return location.origin + v;

            // filename only -> build using row folder id
            const folderId = rowData?.[ROW_ID_FIELD];
            if (folderId === undefined || folderId === null || String(folderId).trim() === "") {
                // if id missing, cannot locate correct folder
                console.warn("ROW_ID missing in row data. Set ROW_ID_FIELD correctly.", rowData);
                return "";
            }

            // encode filename because it has spaces
            const safeFile = encodeURIComponent(v);
            return location.origin + BASE_ATTACH_FOLDER + encodeURIComponent(String(folderId)) + "/" + safeFile;
        }

        // ===== Workbook / Sheet =====
        const wb = new ExcelJS.Workbook();
        const ws = wb.addWorksheet("PDI Authorised Signatory", {
            properties: { defaultRowHeight: 15 },
            views: [{ state: "frozen", xSplit: 0, ySplit: GRID_HEADER_ROW }]
        });

        ws.columns = excelCols.map(c => ({ key: c.key, width: c.width }));

        for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) ws.getRow(r).height = 18;

        ws.pageSetup = {
            orientation: "landscape",
            fitToPage: true,
            fitToWidth: 1,
            fitToHeight: 0,
            margins: { left: 0.3, right: 0.3, top: 0.5, bottom: 0.5, header: 0.2, footer: 0.2 },
            printTitlesRow: `${HEADER_TOP}:${GRID_HEADER_ROW}`
        };

        // ===== Header band fill =====
        for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) {
            for (let c = 1; c <= TOTAL_COLS; c++) {
                ws.getCell(r, c).fill = { type: "pattern", pattern: "solid", fgColor: { argb: "FFF7F7F7" } };
            }
        }

        // ===== Logo block =====
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
                const { base64 } = await fetchAsBase64(logoUrl);
                const imgId = wb.addImage({ base64, extension: "png" });
                ws.addImage(imgId, {
                    tl: { col: tlCol, row: tlRow },
                    ext: { width: LOGO_WIDTH_PX, height: LOGO_HEIGHT_PX },
                    editAs: "oneCell"
                });
            } catch (e) { console.warn("Logo load failed:", e); }
        }
        outlineRange(ws, LOGO_ROW_START, LOGO_COL_START, LOGO_ROW_END, LOGO_COL_END, "thin");

        // ===== Title =====
        ws.mergeCells(HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END);
        const titleCell = ws.getCell(HEADER_TOP, TITLE_COL_START);
        titleCell.value = TITLE_TEXT;
        titleCell.font = { bold: true, size: 18 };
        titleCell.alignment = { horizontal: "center", vertical: "middle" };
        outlineRange(ws, HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END, "thin");

        // ===== Document details =====
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

        // ===== Table header =====
        while (ws.rowCount < GRID_HEADER_ROW - 1) ws.addRow([]);

        const headerTitles = excelCols.map(c => c.label);
        const headerRow = ws.addRow(headerTitles);
        headerRow.height = 22;
        headerRow.eachCell((cell) => {
            cell.font = { bold: true };
            cell.alignment = { horizontal: "center", vertical: "middle", wrapText: true };
            cell.fill = { type: "pattern", pattern: "solid", fgColor: { argb: "FFD9E1F2" } };
            setBorder(cell);
        });

        // ===== Data rows =====
        let tabRows;
        switch (EXPORT_SCOPE) {
            case "selected": tabRows = table.getSelectedRows(); break;
            case "all": tabRows = table.getRows(); break;
            case "active":
            default: tabRows = table.getRows("active"); break;
        }

        // cache to speed up repeated image loads
        const imageCache = new Map(); // url -> {base64, ext}
        async function getCachedImage(url) {
            if (!url) return null;
            if (imageCache.has(url)) return imageCache.get(url);
            const data = await fetchAsBase64(url);
            imageCache.set(url, data);
            return data;
        }

        for (const row of tabRows) {
            const rowData = row.getData();
            const cells = row.getCells();
            const byField = {};

            cells.forEach(cell => {
                const f = cell.getField();
                if (!f) return;

                const def = cell.getColumn().getDefinition();
                const title = (def.title || "").trim();

                if (EXCLUDE_FIELDS.has(f) || EXCLUDE_TITLES.has(title)) return;

                const rawVal = rowData[f];

                // keep raw filename for image fields
                if (isImageField(f)) byField[f] = rawVal;
                else byField[f] = EXPORT_RAW ? rawVal : getDisplayValue(cell);
            });

            // ✅ IMPORTANT: For image columns, export BLANK so filename/path won't show
            const values = excelCols.map(c => isImageField(c.key) ? "" : (byField[c.key] ?? ""));
            const xRow = ws.addRow(values);

            // style row cells
            xRow.eachCell((cell, colNumber) => {
                cell.alignment = {
                    vertical: "middle",
                    horizontal: colNumber === 1 ? "center" : "left",
                    wrapText: true
                };
                setBorder(cell);
            });

            // row height for images
            ws.getRow(xRow.number).height = 85;

            // ✅ Embed images in Photo_Inspector & Specimen_Sign
            for (let i = 0; i < excelCols.length; i++) {
                const field = excelCols[i].key;
                if (!isImageField(field)) continue;

                const fileNameOrUrl = byField[field];
                if (!fileNameOrUrl) continue;

                // take first if multiple values
                const first = String(fileNameOrUrl).split(/[,;]+/).map(s => s.trim()).filter(Boolean)[0];
                if (!first) continue;

                const imgUrl = buildAttachUrl(rowData, first);

                try {
                    const imgData = await getCachedImage(imgUrl);
                    if (!imgData) continue;

                    const imgId = wb.addImage({ base64: imgData.base64, extension: imgData.ext });

                    // keep cell blank
                    ws.getCell(xRow.number, i + 1).value = "";
                    ws.getCell(xRow.number, i + 1).alignment = { horizontal: "center", vertical: "middle" };

                    // size settings
                    const isSign = (field === "Specimen_Sign");
                    const imgW = isSign ? 170 : 125;
                    const imgH = isSign ? 55 : 75;

                    // anchor (0-based)
                    const col0 = i;
                    const row0 = xRow.number - 1;

                    ws.addImage(imgId, {
                        tl: { col: col0 + 0.15, row: row0 + 0.18 },
                        ext: { width: imgW, height: imgH },
                        editAs: "oneCell"
                    });
                } catch (e) {
                    console.warn("Failed to embed image:", field, imgUrl, e);
                }
            }
        }

        // ===== Download =====
        const buffer = await wb.xlsx.writeBuffer();
        const blob = new Blob([buffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = "PDI Authorised Signatory.xlsx";
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

$('#pdiAuth_table').on('change', '.bis-upload', function () {
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
    const type = $(this).data("type");

    const formData = new FormData();
    formData.append("file", file);
    formData.append("id", $(this).data("id"));
    formData.append("type", $(this).data("type")); // Pass Photo/Sign here

    Blockloadershow();

    $.ajax({
        url: "/PDIAuthSign/UploadKaizenAttachment",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false
    }).done(function (response) {
        if (response.success) {
            const id = response.id ?? response.Id ?? $(input).data("id");
            const fileName = response.fileName;
            showSuccessNewAlert("File uploaded successfully.");
            const row = table.getRow(id);
            if (row) {
                if (type == 'Photo') {
                    row.update({ Photo_Inspector: fileName });   // triggers reformat for that cell
                }
                else {
                    row.update({ Specimen_Sign: fileName }); 
                }
            } else {

                if (type == 'Photo') {
                    table.updateOrAddData([{ Id: id, Photo_Inspector: fileName }], "Id");

                }
                else {
                    table.updateOrAddData([{ Id: id, Specimen_Sign: fileName }], "Id");

                }
            }
            //table.updateData([{ Id: response.id, BIS_Attachment: response.fileName }]);
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

function editableColumn(title, field, editorType = true, align = "center", headerFilterType = "input", headerFilterParams = {}, editorParams = {}, formatter = null, width = null) {
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

    if (width) {
        columnDef.width = width; // apply width if given
    }

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
                            url: '/PDIAuthSign/Delete',
                            type: 'POST',
                            data: { id: recid }
                        }).done(function (data) {
                            if (data && data.success === true) {
                                showSuccessNewAlert("PDI Authorised Signatory Detail deleted successfully.");
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

function InsertUpdatePDIAuth(rowData) {
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
        Address: rowData.Address || null,
        Pdi_Inspector: rowData.Pdi_Inspector || null,
        Designation: rowData.Designation || null,
        Photo_Inspector: rowData.Photo_Inspector || null,
        Specimen_Sign: rowData.Specimen_Sign || null,
        Remark: rowData.Remark || null
    };

    console.log(cleanedData);
    const isNew = cleanedData.Id === 0;
    const url = isNew ? '/PDIAuthSign/Create' : '/PDIAuthSign/Update';

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
                showDangerAlert("PDI Authorised Signatory Detail already exists.");
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
