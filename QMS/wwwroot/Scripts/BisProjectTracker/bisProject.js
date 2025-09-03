var tabledata = [];
var table = null;
var tabledataNatProject = [];
var tableNatProject = '';
const searchTerms = {};
let vendorOptions = {};
let natProjectOptions = {};
let selectedNatProjectCell = null;
let filterStartBISDate = moment().startOf('month').format('YYYY-MM-DD');
let filterEndBISDate = moment().endOf('month').format('YYYY-MM-DD');

$(document).ready(function () {

    $('#dateRangeBIS').text(
        moment(filterStartBISDate).format('MMMM D, YYYY') + ' - ' + moment(filterEndBISDate).format('MMMM D, YYYY')
    );

    const picker = new Litepicker({
        element: document.getElementById('customDateTriggerBIS'),
        singleMode: false,
        format: 'DD-MM-YYYY',
        numberOfMonths: 2,
        numberOfColumns: 2,
        dropdowns: { minYear: 2020, maxYear: null, months: true, years: true },
        plugins: ['ranges'],
        setup: (picker) => {
            picker.on('selected', (start, end) => {
                filterStartBISDate = start.format('YYYY-MM-DD');
                filterEndBISDate = end.format('YYYY-MM-DD');
                $('#dateRangeBIS').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadData();
            });

            picker.on('clear', () => {
                filterStartBISDate = "";
                filterEndBISDate = "";
                $('#dateRangeBIS').text("Select Date Range");
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

    $('#customDateTriggerBIS').on('click', function () {
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
            url: '/BisProjectTrac/GetNatProjectDropdown',
            type: 'GET'
        }).done(function (natProject) {
            //let natProjectOptions = {};

            if (Array.isArray(natProject)) {
                natProjectOptions = natProject.reduce((acc, v) => {
                    acc[v.value] = v.label;
                    return acc;
                }, {});
            }

            // Add "Others" as special static option with style/icon

            // Step 3: Load grid data
            $.ajax({
                url: '/BisProjectTrac/GetAll',
                type: 'GET',
                dataType: 'json',
                data: {
                    startDate: filterStartBISDate,
                    endDate: filterEndBISDate
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
                Financial_Year: item.financial_Year,
                Mon_PC: item.mon_Pc,
                Nat_Project: item.nat_Project,
                Lea_Model_No: item.lea_Model_No,
                No_Seri_Add: item.no_Seri_Add,
                Cat_Ref_Lea_Model: item.cat_Ref_Lea_Model,
                Section: item.section,
                Manuf_Location: item.manuf_Location,
                BIS_Project_Id: item.biS_Project_Id,
                Lab: item.lab,
                Report_Owner: item.report_Owner,
                Start_Date: formatDate(item.start_Date),
                Comp_Date: formatDate(item.comp_Date),
                Test_Duration: item.test_Duration,
                Submitted_Date: formatDate(item.submitted_Date),
                Received_Date: formatDate(item.received_Date),
                Bis_Duration: item.bis_Duration,
                Ven_Sample_Sub_Date: formatDate(item.ven_Sample_Sub_Date),
                Current_Status: item.current_Status,
                BIS_Attachment: item.biS_Attachment,
                CreatedBy: item.createdBy,
                UpdatedBy: item.updatedBy,
                UpdatedDate: formatDate(item.updatedDate),
                CreatedDate: formatDate(item.createdDate)
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
            // width: 130,
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
            title: "SNo", field: "Sr_No", sorter: "number", headerMenu: headerMenu, hozAlign: "center", headerHozAlign: "left"
        },

        {
            title: "Financial Year",
            field: "Financial_Year",
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
            title: "Month/PCr",
            field: "Mon_PC",
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
    
        //editableColumn("Month/PC", "Mon_PC", true),
        //editableColumn("Nature of Project", "Nat_Project", true),
        editableColumn("Nature of Project", "Nat_Project", "select2", "center", "input", {}, {
            values: natProjectOptions
        }, function (cell) {
            const val = cell.getValue();
            return natProjectOptions[val] || val;
        }, 170),

        editableColumn("Lead Model Number", "Lea_Model_No", true),
        editableColumn("No. of Series Added", "No_Seri_Add", true),
        editableColumn("Cat Ref of Lead Model", "Cat_Ref_Lea_Model", true),
        editableColumn("Section", "Section", true),
        //editableColumn("Manufacturing Location", "Manuf_Location", true),
        editableColumn("Manufacturing Location", "Manuf_Location", "select2", "center", "input", {}, {
            values: vendorOptions
        }, function (cell) {
            const val = cell.getValue();
            return vendorOptions[val] || val;
        }, 130),

        editableColumn("BIS Project ID", "BIS_Project_Id", true),
        editableColumn("Lab", "Lab", true),
        editableColumn("Report Owner", "Report_Owner", true),

        editableColumn("Vendor Sample Submission Date", "Ven_Sample_Sub_Date", "date", "center"),
        editableColumn("Test Start Date", "Start_Date", "date", "center"),
        editableColumn("Test Complete Date", "Comp_Date", "date", "center"),
        /*editableColumn("Test Duration", "Test_Duration", true),*/
        {
            title: "Test Duration",
            field: "Test_Duration",
            mutator: function (value, data) {
                const start = parseDate(data.Start_Date);
                const end = parseDate(data.Comp_Date);
                if (start && end) return Math.floor((end - start) / (1000 * 60 * 60 * 24));
                return "";
            },
            hozAlign: "center",
            headerFilter: "input",
            headerHozAlign: "center",
            headerMenu: headerMenu
        },
        editableColumn("BIS Submitted Date", "Submitted_Date", "date", "center"),
        editableColumn("BIS Received Date", "Received_Date", "date", "center"),
        //editableColumn("BIS Duration", "Bis_Duration", true),
        {
            title: "BIS Duration",
            field: "Bis_Duration",
            mutator: function (value, data) {
                const sub = parseDate(data.Ven_Sample_Sub_Date);
                const recv = parseDate(data.Received_Date);
                if (sub && recv) return Math.floor((recv - sub) / (1000 * 60 * 60 * 24));
                return "";
            },
            hozAlign: "center",
            headerFilter: "input",
            headerHozAlign: "center",
            headerMenu: headerMenu
        },

        editableColumn("Current Status", "Current_Status", true),
        //editableColumn("BIS Attachment", "BIS_Attachment", true),
        {
            title: "BIS Attachment",
            field: "BIS_Attachment",
            hozAlign: "center",
            headerHozAlign: "center",
            headerMenu: headerMenu,
            formatter: function (cell, formatterParams) {
                const rowData = cell.getRow().getData();
                const fileName = cell.getValue();
                const fileDisplay = fileName
                    ? `<a href="~/BISTrac_Attach/${rowData.Id}/${fileName}" target="_blank">${fileName}</a><br/>`
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

        { title: "User", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false },
        { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center", visible: false }
    );

    // // Initialize Tabulator
    table = new Tabulator("#bisProject_Table", {
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
        const field = cell.getField();
        const row = cell.getRow();
        const data = row.getData();

        if (["Test_Duration", "Bis_Duration"].includes(field)) {
            return;
        }

        if (["Start_Date", "Comp_Date"].includes(field)) {
            const start = parseDate(data.Start_Date);
            const end = parseDate(data.Comp_Date);
            const diff = start && end ? Math.floor((end - start) / (1000 * 60 * 60 * 24)) : "";
            row.update({ Test_Duration: diff.toString() });
        }

        if (["Ven_Sample_Sub_Date", "Received_Date"].includes(field)) {
            const sub = parseDate(data.Ven_Sample_Sub_Date);
            const rec = parseDate(data.Received_Date);
            const diff = sub && rec ? Math.floor((rec - sub) / (1000 * 60 * 60 * 24)) : "";
            row.update({ Bis_Duration: diff.toString() });
        }

        InsertUpdateBisProject(cell.getRow().getData());
    });

    $("#addBISButton").on("click", function () {
        const fyOptions = getFinancialYears(); // returns object
        const currentFY = Object.keys(fyOptions)[0]; // take the first key (e.g. "2025-26") 
        var month = getMonthString();

        const newRow = {
            Sr_No: table.getDataCount() + 1,
            Id: 0,
            Financial_Year: currentFY,
            Mon_PC: month,
            Nat_Project: "",
            Lea_Model_No: "",
            No_Seri_Add: "",
            Cat_Ref_Lea_Model: "",
            Section: "",
            Manuf_Location: "",
            BIS_Project_Id: "",
            Lab: "",
            Report_Owner: "",
            Start_Date: "",
            Comp_Date: "",
            Test_Duration: "",
            Submitted_Date: "",
            Received_Date: "",
            Bis_Duration: "",
            Ven_Sample_Sub_Date: "",
            Current_Status: "",
            BIS_Attachment: "",
            Effective_Date: "",
            Document_No: "",
            Revision_No: "",
            Revision_Date: "",
            CreatedBy: "",
            UpdatedBy: "",
            UpdatedDate: "",
            CreatedDate: ""
        };
        table.addRow(newRow, false);
    });

    // ===== ExcelJS Export: BIS PROJECT TRACKER
    //document.getElementById("exportBisButton").addEventListener("click", async function () {
    //    // ============= 1) GRID COLUMNS (no 'header' here to avoid auto-header) =============
    //    const excelCols = [
    //        { label: "S.NO", key: "Sr_No", width: 6 },
    //        { label: "Financial Year", key: "Financial_Year", width: 14 },
    //        { label: "Month", key: "Mon_PC", width: 12 },
    //        { label: "Nature of Project", key: "Nat_Project", width: 22 },
    //        { label: "Lead Model Number", key: "Lea_Model_No", width: 18 },
    //        { label: "No of Series Added", key: "No_Seri_Add", width: 18 },
    //        { label: "Cat Reference of Lead Model", key: "Cat_Ref_Lea_Model", width: 26 },
    //        { label: "Section", key: "Section", width: 12 },
    //        { label: "Manufacturing Location", key: "Manuf_Location", width: 22 },
    //        { label: "BIS Project ID", key: "BIS_Project_Id", width: 16 },
    //        { label: "Lab", key: "Lab", width: 10 },
    //        { label: "Report Owner", key: "Report_Owner", width: 18 },
    //        { label: "Vendor Sample Submission Date", key: "Ven_Sample_Sub_Date", width: 24 },
    //        { label: "Test Start Date", key: "Start_Date", width: 16 },
    //        { label: "Test Completed Date", key: "Comp_Date", width: 20 },
    //        { label: "Test Duration", key: "Test_Duration", width: 12 },
    //        { label: "BIS submitted date", key: "Submitted_Date", width: 18 },
    //        { label: "BIS Received Date", key: "Received_Date", width: 18 },
    //        { label: "BIS duration", key: "Bis_Duration", width: 12 },
    //        { label: "Current Status", key: "Current_Status", width: 18 },
    //        { label: "BIS Certificate", key: "BIS_Attachment", width: 22 }
    //    ];

    //    // ============= 2) DOCUMENT DETAILS RIGHT TABLE =============
    //    const docDetails = [
    //        ["Document No", "WCIB/LS/QA/R/005"],
    //        ["Effective Date", "01/10/2022"],
    //        ["Revision No", "0"],
    //        ["Revision Date", "01/10/2022"],
    //        ["Page No", "1 of 1"]
    //    ];

    //    // ============= 3) LAYOUT CONSTANTS =============
    //    const TOTAL_COLS = excelCols.length;     // 21
    //    const HEADER_TOP = 2;                    // band start
    //    const HEADER_BOTTOM = 6;                 // band end
    //    const GRID_HEADER_ROW = HEADER_BOTTOM + 1; // table header row (7)
    //    const TITLE_TEXT = "BIS PROJECT TRACKER";

    //    const LOGO_COL_START = 1;   // A
    //    const LOGO_COL_END = 4;   // D

    //    const TITLE_COL_START = LOGO_COL_END + 1;  // E
    //    const TITLE_COL_END = TOTAL_COLS - 6;    // leave space for details

    //    const DETAILS_TABLE_COL_START = TOTAL_COLS - 3; // last 4 columns
    //    const DETAILS_TABLE_COL_END = TOTAL_COLS;

    //    // ============= 4) HELPERS =============
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
    //        cell.border = {
    //            top: { style }, bottom: { style },
    //            left: { style }, right: { style }
    //        };
    //    }

    //    // ============= 5) WORKBOOK / SHEET =============
    //    const wb = new ExcelJS.Workbook();
    //    const ws = wb.addWorksheet("BIS Project Tracker", {
    //        properties: { defaultRowHeight: 18 },
    //        // TRUE sticky header: ySplit equals header row index.
    //        // Set xSplit:1 if you want first column frozen too.
    //        views: [{ state: "frozen", xSplit: 0, ySplit: GRID_HEADER_ROW }]
    //    });

    //    // IMPORTANT: no 'header' in columns => ExcelJS will NOT create an auto-header row.
    //    ws.columns = excelCols.map(c => ({ key: c.key, width: c.width }));

    //    ws.pageSetup = {
    //        orientation: "landscape",
    //        fitToPage: true,
    //        fitToWidth: 1,
    //        fitToHeight: 0,
    //        margins: { left: 0.3, right: 0.3, top: 0.5, bottom: 0.5, header: 0.2, footer: 0.2 },
    //        printTitlesRow: `${HEADER_TOP}:${GRID_HEADER_ROW}` // title band + table header
    //    };

    //    // ============= 6) HEADER BAND BACKDROP =============
    //    for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) {
    //        for (let c = 1; c <= TOTAL_COLS; c++) {
    //            ws.getCell(r, c).fill = {
    //                type: "pattern", pattern: "solid", fgColor: { argb: "FFF7F7F7" }
    //            };
    //        }
    //    }

    //    // ============= 7) LOGO (A2:D6) =============
    //    const logoUrl = window.LOGO_URL || (window.APP_BASE && (window.APP_BASE + "images/wipro-logo.png"));
    //    if (logoUrl) {
    //        try {
    //            const base64 = await fetchAsBase64(logoUrl);
    //            const imgId = wb.addImage({ base64, extension: "png" });
    //            ws.addImage(imgId, {
    //                // ExcelJS image anchor uses 0-based coords; add small padding
    //                tl: { col: LOGO_COL_START - 1 + 0.10, row: HEADER_TOP - 1 + 0.1 },
    //                br: { col: LOGO_COL_END - 0.10, row: HEADER_BOTTOM - 0.1 }
    //            });
    //        } catch (e) {
    //            console.warn("Logo load failed:", e);
    //        }
    //    }

    //    // ============= 8) TITLE (merge E2:O4 approx) =============
    //    ws.mergeCells(HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END);
    //    const titleCell = ws.getCell(HEADER_TOP, TITLE_COL_START);
    //    titleCell.value = TITLE_TEXT;
    //    titleCell.font = { bold: true, size: 18 };
    //    titleCell.alignment = { horizontal: "center", vertical: "middle" };

    //    // subtle bottom rule under header band
    //    for (let c = 1; c <= TOTAL_COLS; c++) {
    //        const cell = ws.getCell(HEADER_BOTTOM, c);
    //        cell.border = { ...cell.border, bottom: { style: "thin" } };
    //    }

    //    // ============= 9) DOCUMENT DETAILS (last 4 columns) =============
    //    docDetails.forEach((pair, i) => {
    //        const r = HEADER_TOP + i; // rows 2..6
    //        // Merge left two cells for label and right two for value
    //        ws.mergeCells(r, DETAILS_TABLE_COL_START, r, DETAILS_TABLE_COL_START + 1);
    //        ws.mergeCells(r, DETAILS_TABLE_COL_START + 2, r, DETAILS_TABLE_COL_END);

    //        const labelCell = ws.getCell(r, DETAILS_TABLE_COL_START);
    //        const valueCell = ws.getCell(r, DETAILS_TABLE_COL_START + 2);

    //        labelCell.value = pair[0];
    //        valueCell.value = pair[1];

    //        labelCell.font = { bold: true };
    //        [labelCell, valueCell].forEach(cell => {
    //            cell.alignment = { vertical: "middle", horizontal: "left", wrapText: true };
    //        });

    //        for (let c = DETAILS_TABLE_COL_START; c <= DETAILS_TABLE_COL_END; c++) {
    //            setBorder(ws.getCell(r, c));
    //        }
    //    });

    //    // ============= 10) MANUAL TABLE HEADER (ROW 7) =============
    //    while (ws.rowCount < GRID_HEADER_ROW - 1) ws.addRow([]); // push to row 6

    //    const headerTitles = excelCols.map(c => c.label);
    //    const headerRow = ws.addRow(headerTitles);
    //    headerRow.height = 22;
    //    headerRow.eachCell((cell) => {
    //        cell.font = { bold: true };
    //        cell.alignment = { horizontal: "center", vertical: "middle", wrapText: true };
    //        cell.fill = { type: "pattern", pattern: "solid", fgColor: { argb: "FFD9E1F2" } };
    //        setBorder(cell);
    //    });

    //    // ============= 11) DATA ROWS =============
    //    // Replace with your Tabulator/table datasource
    //    const rows = (window.table && table.getData) ? table.getData() : [];

    //    rows.forEach((r) => {
    //        const rowVals = {};
    //        excelCols.forEach(c => { rowVals[c.key] = r[c.key] ?? ""; });
    //        const dataRow = ws.addRow(rowVals);

    //        dataRow.eachCell((cell, colNumber) => {
    //            cell.alignment = { vertical: "middle", horizontal: "left", wrapText: true };
    //            setBorder(cell);

    //            // Center S.NO
    //            if (colNumber === 1) {
    //                cell.alignment = { vertical: "middle", horizontal: "center" };
    //            }

    //            // Hyperlink for BIS_Attachment
    //            const key = excelCols[colNumber - 1].key;
    //            if (key === "BIS_Attachment" && typeof cell.value === "string" && /^https?:\/\//i.test(cell.value)) {
    //                cell.value = { text: "Open Certificate", hyperlink: r.BIS_Attachment };
    //                cell.font = { color: { argb: "FF0563C1" }, underline: true };
    //            }

    //            // Date formatting
    //            const dateKeys = new Set(["Ven_Sample_Sub_Date", "Start_Date", "Comp_Date", "Submitted_Date", "Received_Date"]);
    //            if (dateKeys.has(key) && cell.value) {
    //                const d = new Date(cell.value);
    //                if (!isNaN(d)) {
    //                    cell.value = d;
    //                    cell.numFmt = "dd-MM-yyyy";
    //                }
    //            }
    //        });
    //    });

    //    // ============= 12) DOWNLOAD =============
    //    const buffer = await wb.xlsx.writeBuffer();
    //    const blob = new Blob([buffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
    //    const link = document.createElement("a");
    //    link.href = URL.createObjectURL(blob);
    //    link.download = "BIS Project Tracker.xlsx";
    //    document.body.appendChild(link);
    //    link.click();
    //    link.remove();
    //});


    //document.getElementById("exportBisButton").addEventListener("click", async function () {
    //    // =======================
    //    // 0) EXPORT OPTIONS
    //    // =======================
    //    const EXPORT_SCOPE = "active";     // "active" (visible + filtered + sorted), "all", or "selected"
    //    const EXPORT_RAW = false;        // false = export formatted text exactly as shown; true = raw data

    //    // =======================
    //    // 1) BUILD COLUMNS FROM TABULATOR (EXACT ORDER/VISIBILITY)
    //    // =======================
    //    if (!window.table) {
    //        console.error("Tabulator 'table' not found.");
    //        return;
    //    }

    //    // Get Tabulator columns in their current order (true = include columns from groups)
    //    const tabCols = table.getColumns(true)
    //        .filter(col => col.getField())                 // must have a data field
    //        .filter(col => col.isVisible());               // only visible columns

    //    // Map to Excel columns: label, key, width
    //    const excelCols = tabCols.map(col => {
    //        const def = col.getDefinition();
    //        const label = def.title || def.field;
    //        // rough width: convert Tabulator px width to Excel char width (heuristic)
    //        const px = (def.width || col.getWidth() || 120);
    //        const width = Math.max(8, Math.min(40, Math.round(px / 7))); // clamp 8..40
    //        return { label, key: def.field, width };
    //    });

    //    // Safety: if your grid has no visible columns, stop
    //    if (excelCols.length === 0) {
    //        alert("No visible columns to export.");
    //        return;
    //    }

    //    // =======================
    //    // 2) DOCUMENT DETAILS (unchanged)
    //    // =======================
    //    const docDetails = [
    //        ["Document No", "WCIB/LS/QA/R/005"],
    //        ["Effective Date", "01/10/2022"],
    //        ["Revision No", "0"],
    //        ["Revision Date", "01/10/2022"],
    //        ["Page No", "1 of 1"]
    //    ];

    //    // =======================
    //    // 3) LAYOUT CONSTANTS
    //    // =======================
    //    const TOTAL_COLS = excelCols.length;
    //    const HEADER_TOP = 2;
    //    const HEADER_BOTTOM = 6;
    //    const GRID_HEADER_ROW = HEADER_BOTTOM + 1; // row 7
    //    const TITLE_TEXT = "BIS PROJECT TRACKER";

    //    const LOGO_COL_START = 1;
    //    const LOGO_COL_END = Math.min(4, TOTAL_COLS); // keep safe if few cols

    //    const TITLE_COL_START = LOGO_COL_END + 1;
    //    const TITLE_COL_END = Math.max(TITLE_COL_START, TOTAL_COLS - 6); // leave right space

    //    const DETAILS_TABLE_COL_START = Math.max(TITLE_COL_END + 1, TOTAL_COLS - 3);
    //    const DETAILS_TABLE_COL_END = TOTAL_COLS;

    //    // =======================
    //    // 4) HELPERS
    //    // =======================
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

    //    // =======================
    //    // 5) WORKBOOK / SHEET
    //    // =======================
    //    const wb = new ExcelJS.Workbook();
    //    const ws = wb.addWorksheet("BIS Project Tracker", {
    //        properties: { defaultRowHeight: 18 },
    //        views: [{ state: "frozen", xSplit: 0, ySplit: GRID_HEADER_ROW }] // sticky header
    //    });

    //    // No auto header — we set only key & width
    //    ws.columns = excelCols.map(c => ({ key: c.key, width: c.width }));

    //    ws.pageSetup = {
    //        orientation: "landscape",
    //        fitToPage: true,
    //        fitToWidth: 1,
    //        fitToHeight: 0,
    //        margins: { left: 0.3, right: 0.3, top: 0.5, bottom: 0.5, header: 0.2, footer: 0.2 },
    //        printTitlesRow: `${HEADER_TOP}:${GRID_HEADER_ROW}`
    //    };

    //    // =======================
    //    // 6) HEADER BAND (fill)
    //    // =======================
    //    for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) {
    //        for (let c = 1; c <= TOTAL_COLS; c++) {
    //            ws.getCell(r, c).fill = {
    //                type: "pattern", pattern: "solid", fgColor: { argb: "FFF7F7F7" }
    //            };
    //        }
    //    }

    //    // =======================
    //    // 7) LOGO + OUTLINE
    //    // =======================
    //    const logoUrl = window.LOGO_URL || (window.APP_BASE && (window.APP_BASE + "images/wipro-logo.png"));
    //    if (logoUrl) {
    //        try {
    //            const base64 = await fetchAsBase64(logoUrl);
    //            const imgId = wb.addImage({ base64, extension: "png" });
    //            ws.addImage(imgId, {
    //                tl: { col: LOGO_COL_START - 1 + 0.15, row: HEADER_TOP - 1 + 0.1 },
    //                br: { col: LOGO_COL_END - 0.15, row: HEADER_BOTTOM - 0.1 }
    //            });
    //        } catch (e) {
    //            console.warn("Logo load failed:", e);
    //        }
    //    }
    //    outlineRange(ws, HEADER_TOP, LOGO_COL_START, HEADER_BOTTOM, LOGO_COL_END, "thin");

    //    // =======================
    //    // 8) TITLE + OUTLINE
    //    // =======================
    //    ws.mergeCells(HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END);
    //    const titleCell = ws.getCell(HEADER_TOP, TITLE_COL_START);
    //    titleCell.value = TITLE_TEXT;
    //    titleCell.font = { bold: true, size: 18 };
    //    titleCell.alignment = { horizontal: "center", vertical: "middle" };
    //    outlineRange(ws, HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END, "thin");

    //    // =======================
    //    // 9) DOCUMENT DETAILS (right)
    //    // =======================
    //    if (DETAILS_TABLE_COL_START <= DETAILS_TABLE_COL_END) {
    //        docDetails.forEach((pair, i) => {
    //            const r = HEADER_TOP + i; // rows 2..6
    //            ws.mergeCells(r, DETAILS_TABLE_COL_START, r, DETAILS_TABLE_COL_START + 1);
    //            ws.mergeCells(r, DETAILS_TABLE_COL_START + 2, r, DETAILS_TABLE_COL_END);

    //            const labelCell = ws.getCell(r, DETAILS_TABLE_COL_START);
    //            const valueCell = ws.getCell(r, DETAILS_TABLE_COL_START + 2);

    //            labelCell.value = pair[0];
    //            valueCell.value = pair[1];

    //            labelCell.font = { bold: true };
    //            [labelCell, valueCell].forEach(cell => {
    //                cell.alignment = { vertical: "middle", horizontal: "left", wrapText: true };
    //            });

    //            for (let c = DETAILS_TABLE_COL_START; c <= DETAILS_TABLE_COL_END; c++) {
    //                setBorder(ws.getCell(r, c), "thin");
    //            }
    //        });
    //    }

    //    // =======================
    //    // 10) MANUAL TABLE HEADER (ROW 7)
    //    // =======================
    //    while (ws.rowCount < GRID_HEADER_ROW - 1) ws.addRow([]); // up to row 6
    //    const headerTitles = excelCols.map(c => c.label);
    //    const headerRow = ws.addRow(headerTitles);
    //    headerRow.height = 22;
    //    headerRow.eachCell((cell) => {
    //        cell.font = { bold: true };
    //        cell.alignment = { horizontal: "center", vertical: "middle", wrapText: true };
    //        cell.fill = { type: "pattern", pattern: "solid", fgColor: { argb: "FFD9E1F2" } };
    //        setBorder(cell);
    //    });

    //    // =======================
    //    // 11) DATA ROWS: EXACTLY WHAT TABULATOR SHOWS
    //    // =======================
    //    let tabRows;
    //    switch (EXPORT_SCOPE) {
    //        case "selected":
    //            tabRows = table.getSelectedRows();
    //            break;
    //        case "all":
    //            tabRows = table.getRows();
    //            break;
    //        case "active":
    //        default:
    //            tabRows = table.getRows("active"); // respects sort + filter + pagination view
    //            break;
    //    }

    //    // For exact display values, use row.getCells() + cell.getValue()
    //    // For raw data, use row.getData()[field]
    //    tabRows.forEach(row => {
    //        const dataRow = [];
    //        if (EXPORT_RAW) {
    //            const raw = row.getData();
    //            excelCols.forEach(c => dataRow.push(raw[c.key] ?? ""));
    //        } else {
    //            const cells = row.getCells(); // cells match current visible columns order
    //            // Build a map field -> displayed value to remain robust if columns mismatch
    //            const valueByField = {};
    //            cells.forEach(cell => {
    //                const field = cell.getField();
    //                if (field) valueByField[field] = cell.getValue(); // formatted (as shown)
    //            });
    //            excelCols.forEach(c => dataRow.push(valueByField[c.key] ?? ""));
    //        }

    //        const xRow = ws.addRow(dataRow);
    //        xRow.eachCell((cell, colNumber) => {
    //            cell.alignment = { vertical: "middle", horizontal: colNumber === 1 ? "center" : "left", wrapText: true };
    //            setBorder(cell);
    //        });
    //    });

    //    // =======================
    //    // 12) DOWNLOAD
    //    // =======================
    //    const buffer = await wb.xlsx.writeBuffer();
    //    const blob = new Blob([buffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
    //    const link = document.createElement("a");
    //    link.href = URL.createObjectURL(blob);
    //    link.download = "BIS Project Tracker.xlsx";
    //    document.body.appendChild(link);
    //    link.click();
    //    link.remove();
    //});


    //document.getElementById("exportBisButton").addEventListener("click", async function () {
    //    // ===== 0) EXPORT SCOPE =====
    //    const EXPORT_SCOPE = "active";   // "active" | "selected" | "all"
    //    const EXPORT_RAW = false;      // false = formatted as shown in Tabulator

    //    // ===== 1) COLUMNS from Tabulator (exact order/visibility) =====
    //    const tabCols = table.getColumns(true)
    //        .filter(c => c.getField())
    //        .filter(c => c.isVisible());

    //    const excelCols = tabCols.map(col => {
    //        const def = col.getDefinition();
    //        const label = def.title || def.field;
    //        const px = (def.width || col.getWidth() || 120);
    //        const width = Math.max(8, Math.min(40, Math.round(px / 7)));
    //        return { label, key: def.field, width };
    //    });
    //    if (!excelCols.length) { alert("No visible columns to export."); return; }

    //    // ===== 2) Document details =====
    //    const docDetails = [
    //        ["Document No", "WCIB/LS/QA/R/005"],
    //        ["Effective Date", "01/10/2022"],
    //        ["Revision No", "0"],
    //        ["Revision Date", "01/10/2022"],
    //        ["Page No", "1 of 1"]
    //    ];

    //    // ===== 3) Layout constants =====
    //    const TOTAL_COLS = excelCols.length;

    //    const HEADER_TOP = 2;             // header band start
    //    const HEADER_BOTTOM = 6;          // header band end (gives us height for logo)
    //    const GRID_HEADER_ROW = HEADER_BOTTOM + 1; // row 7
    //    const TITLE_TEXT = "BIS PROJECT TRACKER";

    //    // Logo zone (we’ll place by pixels, not cell range)
    //    const LOGO_COL_START = 1; // A (for placement reference)
    //    const LOGO_ROW_START = HEADER_TOP;

    //    // Keep 6 columns on the right for details, so title won’t clash
    //    const RIGHT_DETAILS_COLS = 6;

    //    const TITLE_COL_START = 5;                                     // E
    //    const TITLE_COL_END = Math.max(TITLE_COL_START, TOTAL_COLS - RIGHT_DETAILS_COLS); // leaves space
    //    const DETAILS_TABLE_COL_START = Math.min(TOTAL_COLS - RIGHT_DETAILS_COLS + 1, TOTAL_COLS);
    //    const DETAILS_TABLE_COL_END = TOTAL_COLS;

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
    //    const ws = wb.addWorksheet("BIS Project Tracker", {
    //        properties: { defaultRowHeight: 18 },
    //        views: [{ state: "frozen", xSplit: 0, ySplit: GRID_HEADER_ROW }]
    //    });

    //    // set column widths (no headers here to avoid duplicate)
    //    ws.columns = excelCols.map(c => ({ key: c.key, width: c.width }));

    //    // give header band some vertical room so logo fits well
    //    for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) {
    //        ws.getRow(r).height = 26; // ~26pt ≈ 34px; adjust if you want taller
    //    }

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
    //            ws.getCell(r, c).fill = {
    //                type: "pattern", pattern: "solid", fgColor: { argb: "FFF7F7F7" }
    //            };
    //        }
    //    }

    //    // ===== 7) Logo: FIXED PIXEL SIZE (no stretching) + outline =====
    //    const LOGO_WIDTH_PX = 130; // tweak
    //    const LOGO_HEIGHT_PX = 130; // tweak
    //    const LOGO_PAD_COL = 0.25; // small left padding inside cell
    //    const LOGO_PAD_ROW = 0.12; // small top padding

    //    const logoUrl = window.LOGO_URL || (window.APP_BASE && (window.APP_BASE + "images/wipro-logo.png"));
    //    if (logoUrl) {
    //        try {
    //            const base64 = await fetchAsBase64(logoUrl);
    //            const imgId = wb.addImage({ base64, extension: "png" });
    //            ws.addImage(imgId, {
    //                // anchor by top-left cell + pixel extents — Excel keeps aspect, no stretching
    //                tl: { col: LOGO_COL_START - 1 + LOGO_PAD_COL, row: LOGO_ROW_START - 1 + LOGO_PAD_ROW },
    //                ext: { width: LOGO_WIDTH_PX, height: LOGO_HEIGHT_PX },
    //                editAs: "oneCell"
    //            });
    //        } catch (e) {
    //            console.warn("Logo load failed:", e);
    //        }
    //    }
    //    // Draw a clean outline around the visual logo block (A2:D6)
    //    outlineRange(ws, HEADER_TOP, 1, HEADER_BOTTOM, 4, "thin");

    //    // ===== 8) Title (merge) + exact outline =====
    //    // Merge exactly E2 .. (TITLE_COL_END) over 3 rows high
    //    ws.mergeCells(HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END);
    //    const titleCell = ws.getCell(HEADER_TOP, TITLE_COL_START);
    //    titleCell.value = TITLE_TEXT;
    //    titleCell.font = { bold: true, size: 18 };
    //    titleCell.alignment = { horizontal: "center", vertical: "middle" };

    //    // Outline precisely the merged title area (does not touch details area)
    //    outlineRange(ws, HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END, "thin");

    //    // ===== 9) Document details table (last 6 columns) =====
    //    if (DETAILS_TABLE_COL_START <= DETAILS_TABLE_COL_END) {
    //        docDetails.forEach((pair, i) => {
    //            const r = HEADER_TOP + i;
    //            // Merge label across 3 cols and value across 3 cols for breathing space
    //            const mid = Math.floor((DETAILS_TABLE_COL_START + DETAILS_TABLE_COL_END) / 2);
    //            ws.mergeCells(r, DETAILS_TABLE_COL_START, r, mid);
    //            ws.mergeCells(r, mid + 1, r, DETAILS_TABLE_COL_END);

    //            const labelCell = ws.getCell(r, DETAILS_TABLE_COL_START);
    //            const valueCell = ws.getCell(r, mid + 1);

    //            labelCell.value = pair[0];
    //            valueCell.value = pair[1];
    //            labelCell.font = { bold: true };
    //            [labelCell, valueCell].forEach(cell => {
    //                cell.alignment = { vertical: "middle", horizontal: "left", wrapText: true };
    //            });

    //            for (let c = DETAILS_TABLE_COL_START; c <= DETAILS_TABLE_COL_END; c++) {
    //                setBorder(ws.getCell(r, c));
    //            }
    //        });
    //    }

    //    // ===== 10) Manual table header (row 7) =====
    //    while (ws.rowCount < GRID_HEADER_ROW - 1) ws.addRow([]); // up to row 6
    //    const headerTitles = excelCols.map(c => c.label);
    //    const headerRow = ws.addRow(headerTitles);
    //    headerRow.height = 22;
    //    headerRow.eachCell((cell) => {
    //        cell.font = { bold: true };
    //        cell.alignment = { horizontal: "center", vertical: "middle", wrapText: true };
    //        cell.fill = { type: "pattern", pattern: "solid", fgColor: { argb: "FFD9E1F2" } };
    //        setBorder(cell);
    //    });

    //    // ===== 11) Data rows (exact Tabulator view) =====
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
    //        cells.forEach(cell => { const f = cell.getField(); if (f) byField[f] = EXPORT_RAW ? row.getData()[f] : cell.getValue(); });

    //        const values = excelCols.map(c => byField[c.key] ?? "");
    //        const xRow = ws.addRow(values);

    //        xRow.eachCell((cell, colNumber) => {
    //            cell.alignment = { vertical: "middle", horizontal: colNumber === 1 ? "center" : "left", wrapText: true };
    //            setBorder(cell);
    //        });
    //    });

    //    // ===== 12) Download =====
    //    const buffer = await wb.xlsx.writeBuffer();
    //    const blob = new Blob([buffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
    //    const link = document.createElement("a");
    //    link.href = URL.createObjectURL(blob);
    //    link.download = "BIS Project Tracker.xlsx";
    //    document.body.appendChild(link);
    //    link.click();
    //    link.remove();
    //});

    //document.getElementById("exportBisButton").addEventListener("click", async function () {
    //    // ========= 0) EXPORT OPTIONS =========
    //    const EXPORT_SCOPE = "active";   // "active" | "selected" | "all"
    //    const EXPORT_RAW = false;      // false = formatted (as shown in Tabulator), true = raw data

    //    // ========= 1) COLUMNS from Tabulator (exact view) with EXCLUDES =========
    //    if (!window.table) { console.error("Tabulator 'table' not found."); return; }

    //    const EXCLUDE_FIELDS = new Set(["Action", "action", "Actions"]);
    //    const EXCLUDE_TITLES = new Set(["Action", "Actions"]);

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

    //    // ========= 2) DOC DETAILS =========
    //    const docDetails = [
    //        ["Document No", "WCIB/LS/QA/R/005"],
    //        ["Effective Date", "01/10/2022"],
    //        ["Revision No", "0"],
    //        ["Revision Date", "01/10/2022"],
    //        ["Page No", "1 of 1"]
    //    ];

    //    // ========= 3) LAYOUT =========
    //    const TOTAL_COLS = excelCols.length;

    //    const HEADER_TOP = 2;                 // header band start
    //    const HEADER_BOTTOM = 6;              // header band end (gives room for logo)
    //    const GRID_HEADER_ROW = HEADER_BOTTOM + 1; // row 7
    //    const TITLE_TEXT = "BIS PROJECT TRACKER";

    //    // Logo block target (A2:D6)
    //    const LOGO_COL_START = 1; // A
    //    const LOGO_COL_END = 4; // D
    //    const LOGO_ROW_START = HEADER_TOP;
    //    const LOGO_ROW_END = HEADER_BOTTOM;

    //    // Reserve last 6 columns for details on right
    //    const RIGHT_DETAILS_COLS = Math.min(6, Math.max(0, TOTAL_COLS >= 10 ? 6 : Math.floor(TOTAL_COLS / 3)));

    //    const TITLE_COL_START = Math.min(5, TOTAL_COLS); // E (if cols < 5, clamp)
    //    const TITLE_COL_END = Math.max(TITLE_COL_START, TOTAL_COLS - RIGHT_DETAILS_COLS);

    //    const DETAILS_TABLE_COL_START = (RIGHT_DETAILS_COLS > 0) ? TITLE_COL_END + 1 : TOTAL_COLS + 1;
    //    const DETAILS_TABLE_COL_END = TOTAL_COLS;

    //    // ========= 4) HELPERS =========
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

    //    // ========= 5) WORKBOOK / SHEET =========
    //    const wb = new ExcelJS.Workbook();
    //    const ws = wb.addWorksheet("BIS Project Tracker", {
    //        properties: { defaultRowHeight: 18 },
    //        views: [{ state: "frozen", xSplit: 0, ySplit: GRID_HEADER_ROW }] // sticky header
    //    });

    //    // Column widths (no headers here to avoid duplicates)
    //    ws.columns = excelCols.map(c => ({ key: c.key, width: c.width }));

    //    // Give header band extra height for logo
    //    for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) {
    //        ws.getRow(r).height = 26; // ~34px
    //    }

    //    ws.pageSetup = {
    //        orientation: "landscape",
    //        fitToPage: true,
    //        fitToWidth: 1,
    //        fitToHeight: 0,
    //        margins: { left: 0.3, right: 0.3, top: 0.5, bottom: 0.5, header: 0.2, footer: 0.2 },
    //        printTitlesRow: `${HEADER_TOP}:${GRID_HEADER_ROW}`
    //    };

    //    // ========= 6) HEADER BAND FILL =========
    //    for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) {
    //        for (let c = 1; c <= TOTAL_COLS; c++) {
    //            ws.getCell(r, c).fill = {
    //                type: "pattern", pattern: "solid", fgColor: { argb: "FFF7F7F7" }
    //            };
    //        }
    //    }

    //    // ========= 7) LOGO — centered in A2:D6 (no stretch) + outline =========
    //    // Desired logo size (px). Adjust to taste.
    //    const LOGO_WIDTH_PX = 120;
    //    const LOGO_HEIGHT_PX = 120;

    //    // Approx column/row pixel helpers
    //    const COL_PX = (c) => ((ws.getColumn(c).width || 8) * 7);       // ~7px per char
    //    const ROW_PX = (r) => ((ws.getRow(r).height || 18) * (96 / 72));  // pt -> px

    //    // Size of the A2:D6 rectangle in px
    //    let rectWpx = 0; for (let c = LOGO_COL_START; c <= LOGO_COL_END; c++) rectWpx += COL_PX(c);
    //    let rectHpx = 0; for (let r = LOGO_ROW_START; r <= LOGO_ROW_END; r++) rectHpx += ROW_PX(r);

    //    // Average px per column/row in that rectangle
    //    const avgColPx = rectWpx / (LOGO_COL_END - LOGO_COL_START + 1);
    //    const avgRowPx = rectHpx / (LOGO_ROW_END - LOGO_ROW_START + 1);

    //    // Convert desired pixel size → fractional col/row units
    //    const logoCols = LOGO_WIDTH_PX / avgColPx;
    //    const logoRows = LOGO_HEIGHT_PX / avgRowPx;

    //    // Centered top-left anchor (fractional col/row)
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
    //    // Outline the logo block A2:D6
    //    outlineRange(ws, LOGO_ROW_START, LOGO_COL_START, LOGO_ROW_END, LOGO_COL_END, "thin");

    //    // ========= 8) TITLE (merge) + outline (no overlap with details) =========
    //    ws.mergeCells(HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END);
    //    const titleCell = ws.getCell(HEADER_TOP, TITLE_COL_START);
    //    titleCell.value = TITLE_TEXT;
    //    titleCell.font = { bold: true, size: 18 };
    //    titleCell.alignment = { horizontal: "center", vertical: "middle" };

    //    outlineRange(ws, HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END, "thin");

    //    // ========= 9) DOCUMENT DETAILS (right block) =========
    //    if (DETAILS_TABLE_COL_START <= DETAILS_TABLE_COL_END) {
    //        docDetails.forEach((pair, i) => {
    //            const r = HEADER_TOP + i;

    //            // Split right block into label|value halves
    //            const mid = Math.floor((DETAILS_TABLE_COL_START + DETAILS_TABLE_COL_END) / 2);
    //            ws.mergeCells(r, DETAILS_TABLE_COL_START, r, mid);
    //            ws.mergeCells(r, mid + 1, r, DETAILS_TABLE_COL_END);

    //            const labelCell = ws.getCell(r, DETAILS_TABLE_COL_START);
    //            const valueCell = ws.getCell(r, mid + 1);

    //            labelCell.value = pair[0];
    //            valueCell.value = pair[1];
    //            labelCell.font = { bold: true };
    //            [labelCell, valueCell].forEach(cell => {
    //                cell.alignment = { vertical: "middle", horizontal: "left", wrapText: true };
    //            });

    //            for (let c = DETAILS_TABLE_COL_START; c <= DETAILS_TABLE_COL_END; c++) {
    //                setBorder(ws.getCell(r, c));
    //            }
    //        });
    //    }

    //    // ========= 10) MANUAL TABLE HEADER (row 7) =========
    //    while (ws.rowCount < GRID_HEADER_ROW - 1) ws.addRow([]); // up to row 6

    //    const headerTitles = excelCols.map(c => c.label);
    //    const headerRow = ws.addRow(headerTitles);
    //    headerRow.height = 22;
    //    headerRow.eachCell((cell) => {
    //        cell.font = { bold: true };
    //        cell.alignment = { horizontal: "center", vertical: "middle", wrapText: true };
    //        cell.fill = { type: "pattern", pattern: "solid", fgColor: { argb: "FFD9E1F2" } };
    //        setBorder(cell);
    //    });

    //    // ========= 11) DATA ROWS (exact Tabulator view, minus Action) =========
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
    //            // skip excluded
    //            if (EXCLUDE_FIELDS.has(f)) return;
    //            const def = cell.getColumn().getDefinition();
    //            const title = (def.title || "").trim();
    //            if (EXCLUDE_TITLES.has(title)) return;

    //            byField[f] = EXPORT_RAW ? row.getData()[f] : cell.getValue();
    //        });

    //        const values = excelCols.map(c => byField[c.key] ?? "");
    //        const xRow = ws.addRow(values);

    //        xRow.eachCell((cell, colNumber) => {
    //            cell.alignment = { vertical: "middle", horizontal: colNumber === 1 ? "center" : "left", wrapText: true };
    //            setBorder(cell);
    //        });
    //    });

    //    // ========= 12) DOWNLOAD =========
    //    const buffer = await wb.xlsx.writeBuffer();
    //    const blob = new Blob([buffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
    //    const link = document.createElement("a");
    //    link.href = URL.createObjectURL(blob);
    //    link.download = "BIS Project Tracker.xlsx";
    //    document.body.appendChild(link);
    //    link.click();
    //    link.remove();
    //});

    document.getElementById("exportBisButton").addEventListener("click", async function () {
        // ===== 0) OPTIONS =====
        const EXPORT_SCOPE = "active";   // "active" | "selected" | "all"
        const EXPORT_RAW = false;      // false = formatted values exactly as shown in Tabulator

        // ===== 1) COLUMNS from Tabulator (exact view) with EXCLUDES =====
        if (!window.table) { console.error("Tabulator 'table' not found."); return; }

        const EXCLUDE_FIELDS = new Set(["Action", "action", "Actions","CreatedBy"]);
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

        const HEADER_TOP = 1;                      // header band start
        const HEADER_BOTTOM = 5;                   // header band end
        const GRID_HEADER_ROW = HEADER_BOTTOM + 1; // row 7
        const TITLE_TEXT = "BIS PROJECT TRACKER";

        // Logo block (A2:B6 exactly)
        const LOGO_COL_START = 1; // A
        const LOGO_COL_END = 2; // B
        const LOGO_ROW_START = HEADER_TOP;
        const LOGO_ROW_END = HEADER_BOTTOM;

        // Title must not overlap the final 2 columns (reserved for details)
        const TITLE_COL_START = Math.min(3, TOTAL_COLS);            // start from column C (or clamp if few cols)
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
        const ws = wb.addWorksheet("BIS Project Tracker", {
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
        link.download = "BIS Project Tracker.xlsx";
        document.body.appendChild(link);
        link.click();
        link.remove();
    });


    Blockloaderhide();
}

$('#bisProject_Table').on('change', '.bis-upload', function () {
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
        url: "/BisProjectTrac/UploadBISAttachment",
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

//Tabulator.extendModule("edit", "editors", {
//    select2: function (cell, onRendered, success, cancel, editorParams) {
//        const fieldName = cell.getField(); // Get the column field name
//        const values = editorParams.values || {};
//        const select = document.createElement("select");
//        select.style.width = "100%";

//        // Append normal options
//        for (let val in values) {
//            let option = document.createElement("option");
//            option.value = val;
//            option.text = values[val];
//            if (val === cell.getValue()) option.selected = true;
//            select.appendChild(option);
//        }

//        // 👉 Only add the "Add New" option for Nat_Project column
//        if (fieldName === "Nat_Project") {
//            let addOption = document.createElement("option");
//            addOption.value = "__add_new__";
//            addOption.text = "➕ Add New Project Type";
//            select.appendChild(addOption);
//        }

//        onRendered(function () {
//            $(select).select2({
//                dropdownParent: document.body,
//                width: 'resolve',
//                placeholder: "Select value",
//                templateResult: function (data) {
//                    if (data.id === "__add_new__") {
//                        return $('<span style="color: blue;"><i class="fas fa-plus-circle"></i> ' + data.text + '</span>');
//                    }
//                    return data.text;
//                },
//                templateSelection: function (data) {
//                    return values[data.id] || data.text;
//                }
//            }).on("select2:select", function (e) {
//                const selectedVal = select.value;
//                if (selectedVal === "__add_new__") {
//                    $(select).select2('close');
//                    cancel(); // Cancel edit
//                    $('#yourModalId').modal('show'); // Show modal
//                } else {
//                    success(selectedVal);
//                }
//            });
//        });

//        return select;
//    }
//});

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
        if (fieldName === "Nat_Project") {
            let addOption = document.createElement("option");
            addOption.value = "__add_new__";
            addOption.text = "➕ Add New Project Type";
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
                    selectedNatProjectCell = cell; // store the cell
                    $('#natProjectModel').modal('show');
                    loadNatProjectData();
                } else {
                    success(selectedVal);
                }
            });
        });

        return select;
    }
});



//function showEditBisProject(id) {
//    debugger
//    var url = '/BisProjectTrac/BisProjectTracker?id=' + id;
//    window.location.href = url;
//}

function delConfirm(recid, element) {
    debugger;

    if (!recid || recid <= 0) {
        const rowEl = $(element).closest(".tabulator-row")[0];
        const row = table.getRow(rowEl);
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
            url: '/BisProjectTrac/Delete',
            type: 'POST',
            data: { id: recid },
            success: function (data) {
                if (data.success == true) {
                    showSuccessAlert("Bis Projecet Deleted successfully.");
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
        loadData();
    });
}

//function openBisProject(id) {
//    debugger
//    var url = '/BisProjectTrac/BisProjectTrackerDetail';
//    url = url + '?id=' + id
//    window.location.href = url;
//}

function InsertUpdateBisProject(rowData) {
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
        Financial_Year: rowData.Financial_Year || null,
        Mon_Pc: rowData.Mon_PC || null,
        Nat_Project: rowData.Nat_Project || null,
        Lea_Model_No: rowData.Lea_Model_No || null,
        No_Seri_Add: rowData.No_Seri_Add || null,
        Cat_Ref_Lea_Model: rowData.Cat_Ref_Lea_Model || null,
        Section: rowData.Section || null,
        Manuf_Location: rowData.Manuf_Location || null,
        BIS_Project_Id: rowData.BIS_Project_Id || null,
        Lab: rowData.Lab || null,
        Report_Owner: rowData.Report_Owner || null,
        Ven_Sample_Sub_Date: toIsoDate(rowData.Ven_Sample_Sub_Date),
        Start_Date: toIsoDate(rowData.Start_Date),
        Comp_Date: toIsoDate(rowData.Comp_Date),
        Test_Duration: rowData.Test_Duration.toString() || null,
        Submitted_Date: toIsoDate(rowData.Submitted_Date),
        Received_Date: toIsoDate(rowData.Received_Date),
        Bis_Duration: rowData.Bis_Duration.toString() || null,
        Current_Status: rowData.Current_Status || null,
        BIS_Attachment: rowData.BIS_Attachment || null,
        Effective_Date: toIsoDate(rowData.Effective_Date),
        Document_No: rowData.Document_No || null,
        Revision_No: rowData.Revision_No || null,
        Revision_Date: toIsoDate(rowData.Revision_Date)
    };

    const isNew = Model.Id === 0;
    var ajaxUrl = isNew ? '/BisProjectTrac/Create' : '/BisProjectTrac/Update';

    $.ajax({
        url: ajaxUrl,
        type: "POST",
        data: JSON.stringify(Model),
        contentType: 'application/json',
        success: function (response) {
            //Blockloaderhide();
            if (response.success) {
                //const msg = Model.Id != 0
                //    ? "BIS Project Tracker updated successfully!"
                //    : "BIS Project Tracker saved successfully!";
                //showSuccessAlert(msg);
                //loadData();
                if (isNew) {
                    showSuccessAlert("Saved successfully!.");
                    loadData();
                }
            }
            else if (response.message === "Exist") {
                showDangerAlert("BIS Project Tracker Detail already exists.");
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


function loadNatProjectData() {
    Blockloadershow();
    $.ajax({
        url: '/BisProjectTrac/GetNatProject',
        type: 'GET',
        success: function (data) {
            Blockloaderhide();
            if (data && Array.isArray(data)) {
                OnNatProjectTabGridLoad(data);
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

function OnNatProjectTabGridLoad(response) {
    debugger;
    Blockloadershow();

    tabledataNatProject = [];
    let columns = [];

    // Map the response to the table format
    if (response.length > 0) {
        $.each(response, function (index, item) {

            function formatDate(value) {
                return value ? new Date(value).toLocaleDateString("en-GB") : "";
            }

            tabledataNatProject.push({
                Sr_No: index + 1,
                Id: item.id,
                Nat_Project: item.nat_Project,
                CreatedBy: item.createdBy,
                UpdatedBy: item.updatedBy,
                UpdatedDate: formatDate(item.updatedDate),
                CreatedDate: formatDate(item.createdDate),
            });
        });
    }

    if (tabledataNatProject.length === 0 && tableNatProject) {
        tableNatProject.clearData();
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

                actionButtons += `<i onclick="delNatProjectConfirm(${rowData.Id},this)" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>`

                return actionButtons;
            }
        },
        {
            title: "SNo", field: "Sr_No", width: 48, sorter: "number", hozAlign: "center", headerHozAlign: "left"
        },
        editableColumn("Nature of Project", "Nat_Project", true),
        { title: "CreatedBy", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Created Date", field: "CreatedDate", width: 129, sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
        { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
    );

    // // Initialize Tabulator
    tableNatProject = new Tabulator("#natProject_Table", {
        data: tabledataNatProject,
        renderHorizontal: "virtual",
        movableColumns: true,
        pagination: "local",
        paginationSize: 10,
        paginationSizeSelector: [50, 100, 500, 1500, 2000],
        paginationCounter: "rows",
        dataEmpty: "<div style='text-align: center; font-size: 1rem; color: gray;'>No data available</div>", // Placeholder message
        columns: columns
    });

    tableNatProject.on("cellEdited", function (cell) {
        InsertUpdateNatProject(cell.getRow().getData());
    });

    $("#addNatProjectBtn").on("click", function () {
        const newRow1 = {
            Sr_No: tableNatProject.getDataCount() + 1,
            Id: 0,
            Nat_Project: "",
            CreatedBy: "",
            UpdatedBy: "",
            UpdatedDate: "",
            CreatedDate: ""
        };
        tableNatProject.addRow(newRow1, false);
    });

    

    Blockloaderhide();
}

function InsertUpdateNatProject(rowData) {
    debugger
    if (!rowData) {
        showDangerAlert("Invalid data provided.");
        return;
    }

    Blockloadershow();
    var errorMsg = "";

    if (errorMsg !== "") {
        Blockloaderhide();
        showDangerAlert(errorMsg);
        return false;
    }

    var Model = {
        Id: rowData.Id || 0,
        Nat_Project: rowData.Nat_Project || null,
    };

    var ajaxUrl = Model.Id === 0 ? '/BisProjectTrac/CreateNatProject' : '/BisProjectTrac/UpdateNatProject';

    $.ajax({
        url: ajaxUrl,
        type: "POST",
        data: JSON.stringify(Model),
        contentType: 'application/json',
        success: function (response) {
            Blockloaderhide();
            if (response.success) {
                const msg = Model.Id != 0
                    ? "Nature of Project updated successfully!"
                    : "Nature of Project saved successfully!";
                showSuccessAlert(msg);
                loadNatProjectData();
            }
            else if (response.message === "Exist") {
                showDangerAlert("Nature of Project already exists.");
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
            Blockloaderhide();
            showDangerAlert("An unexpected error occurred. Please refresh the page and try again.");
        }
    });
}

$('#natProjectModel').on('hidden.bs.modal', function () {
    loadData(); // uncomment if you want full reload
});

function delNatProjectConfirm(recid, element) {
    debugger;

    if (!recid || recid <= 0) {
        const rowEl = $(element).closest(".tabulator-row")[0];
        const row = tableNatProject.getRow(rowEl);
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
            url: '/BisProjectTrac/DeleteNatProjectAsync',
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
        loadNatProjectData();
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


//function openDocModel() {
//    clearForm();
//    if (!$('#docDetModal').length) {
//        $('body').append(partialView);
//    }
//    $('#docDetModal').modal('show');

//    var today = new Date().toISOString().split('T')[0];
//    $('#effect_Date').val(today);
//    $('#rev_Date').val(today);
//}

