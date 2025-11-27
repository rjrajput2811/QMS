var tabledata = [];
var table = '';



$(document).ready(function () {
    OnTabGridLoad();

    const csatId = parseInt($("#hdnCsatId").val() || "0", 10);

    if (csatId > 0) {
        loadCsatSummary(csatId);
    }
});

function OnTabGridLoad() {

    const quarters = {
        1: ["PC1", "PC2", "PC3", "Q1"],
        2: ["PC4", "PC5", "PC6", "Q2"],
        3: ["PC7", "PC8", "PC9", "Q3"],
        4: ["PC10", "PC11", "PC12", "Q4"]
    };

    function isSummaryRow(rowData) {
        return ["Q1", "Q2", "Q3", "Q4"].includes(rowData.pc) || rowData.quarter === "YTD";
    }

    tableData = [];

    Object.keys(quarters).forEach(q => {
        quarters[q].forEach((pc, index) => {
            tableData.push({
                quarter: index === 0 ? q.toString() : "",
                pc: pc,
                reqSent: "",
                resRec: "",
                promoters: "",
                collection: "",
                detractors: "",
                nps: "",
                detDetails: "",
                Id: 0    
            });
        });
    });

    tableData.push({
        quarter: "YTD",
        pc: "",
        reqSent: "",
        resRec: "",
        promoters: "",
        collection: "",
        detractors: "",
        nps: "",
        detDetails: "",
        Id: 0
    });

    table = new Tabulator("#csatTable", {
        data: tableData,
        layout: "fitColumns",
        columns: [
            { title: "Quarter", field: "quarter", hozAlign: "center", width: 100 },

            { title: "PC", field: "pc", hozAlign: "center", width: 100 },

            {
                title: "Request Sent",
                field: "reqSent",
                editor: "input",
                editable: cell => !isSummaryRow(cell.getRow().getData()),
                hozAlign: "center"
            },

            {
                title: "Responses Received",
                field: "resRec",
                editor: "input",
                editable: cell => !isSummaryRow(cell.getRow().getData()),
                hozAlign: "center"
            },

            {
                title: "Promoters",
                field: "promoters",
                editor: "input",
                editable: cell => !isSummaryRow(cell.getRow().getData()),
                hozAlign: "center"
            },

            {
                title: "% Collection",
                field: "collection",
                hozAlign: "center"
            },

            {
                title: "Detractors",
                field: "detractors",
                editor: "input",
                editable: cell => !isSummaryRow(cell.getRow().getData()),
                hozAlign: "center"
            },

            {
                title: "NPS",
                field: "nps",
                editor: "input",
                editable: cell => !isSummaryRow(cell.getRow().getData()),
                hozAlign: "center"
            },

            {
                title: "Detractor Details",
                field: "detDetails",
                editor: "input",
                editable: cell => !isSummaryRow(cell.getRow().getData()),
                hozAlign: "center"
            },

            { title: "Id", field: "Id", visible: false }
        ],

        movableColumns: false,
        resizableRows: false
    });

    table.on("cellEdited", function (cell) {
        // 1) Recalculate all %Collection, NPS, Q1–Q4, YTD (like Excel)
        calculateAllSummaries();

        //// 2) Save the edited row to DB (your existing method)
        //saveEditedPDIRow(cell.getRow().getData());
    });


    // Export
    //document.getElementById("exportButton").addEventListener("click", function () {
    //    window.csatTable.download("xlsx", "CSAT_Summary.xlsx", { sheetName: "CSAT Summary" });
    //});

    //// Save button
    //document.getElementById("saveButton").addEventListener("click", function () {
    //    console.log("Saved Data:", window.csatTable.getData());
    //    alert("Data saved to console (for demo).");
    //});
}

function safeNum(v) {
    if (v === null || v === undefined) return 0;
    const s = String(v).trim();
    if (!s) return 0;
    const n = parseFloat(s);
    return isNaN(n) ? 0 : n;
}

function fmtPercent(val) {
    if (!isFinite(val) || isNaN(val)) return 0;
    return Math.round(val * 100) / 100;  // 2 decimals
}

/* ------------------------------
   SUMMARY CALCULATION (Excel logic)
---------------------------------*/

function calculateAllSummaries() {

    if (!table) return;

    const rows = table.getRows();

    const pcRows = {};        // "PC1" -> row
    const quarterRows = {};   // "Q1" etc. -> row
    let ytdRow = null;

    rows.forEach(row => {
        const d = row.getData();

        if (d.pc && d.pc.toString().startsWith("PC")) {
            pcRows[d.pc] = row;
        }

        if (["Q1", "Q2", "Q3", "Q4"].includes(d.pc)) {
            quarterRows[d.pc] = row;
        }

        if (d.quarter === "YTD") {
            ytdRow = row;
        }
    });

    // 1) Row-wise %Collection & NPS for each PC row
    const allPcCodes = [
        "PC1", "PC2", "PC3",
        "PC4", "PC5", "PC6",
        "PC7", "PC8", "PC9",
        "PC10", "PC11", "PC12"
    ];

    allPcCodes.forEach(pc => {
        const row = pcRows[pc];
        if (!row) return;
        const d = row.getData();

        const req = safeNum(d.reqSent);
        const res = safeNum(d.resRec);
        const pro = safeNum(d.promoters);
        const det = safeNum(d.detractors);

        d.collection = req > 0 ? fmtPercent((res / req) * 100) : 0;
        d.nps = res > 0 ? fmtPercent(((pro - det) / res) * 100) : 0;

        row.update(d);
    });

    // 2) Quarter summary rows (Q1, Q2, Q3, Q4)
    function calcQuarter(qLabel, pcCodes) {
        const row = quarterRows[qLabel];
        if (!row) return { req: 0, res: 0, pro: 0, det: 0 };

        const d = row.getData();

        let totalReq = 0, totalRes = 0, totalPro = 0, totalDet = 0;

        pcCodes.forEach(pc => {
            const r = pcRows[pc];
            if (!r) return;
            const rd = r.getData();
            totalReq += safeNum(rd.reqSent);
            totalRes += safeNum(rd.resRec);
            totalPro += safeNum(rd.promoters);
            totalDet += safeNum(rd.detractors);
        });

        d.reqSent = totalReq;
        d.resRec = totalRes;
        d.promoters = totalPro;
        d.detractors = totalDet;

        d.collection = totalReq > 0 ? fmtPercent((totalRes / totalReq) * 100) : 0;
        d.nps = totalRes > 0 ? fmtPercent(((totalPro - totalDet) / totalRes) * 100) : 0;

        row.update(d);

        return { req: totalReq, res: totalRes, pro: totalPro, det: totalDet };
    }

    const q1 = calcQuarter("Q1", ["PC1", "PC2", "PC3"]);
    const q2 = calcQuarter("Q2", ["PC4", "PC5", "PC6"]);
    const q3 = calcQuarter("Q3", ["PC7", "PC8", "PC9"]);
    const q4 = calcQuarter("Q4", ["PC10", "PC11", "PC12"]);

    // 3) YTD row = sum of Q1–Q4
    if (ytdRow) {
        const d = ytdRow.getData();

        const totalReq = safeNum(q1.req) + safeNum(q2.req) + safeNum(q3.req) + safeNum(q4.req);
        const totalRes = safeNum(q1.res) + safeNum(q2.res) + safeNum(q3.res) + safeNum(q4.res);
        const totalPro = safeNum(q1.pro) + safeNum(q2.pro) + safeNum(q3.pro) + safeNum(q4.pro);
        const totalDet = safeNum(q1.det) + safeNum(q2.det) + safeNum(q3.det) + safeNum(q4.det);

        d.reqSent = totalReq;
        d.resRec = totalRes;
        d.promoters = totalPro;
        d.detractors = totalDet;

        d.collection = totalReq > 0 ? fmtPercent((totalRes / totalReq) * 100) : 0;
        d.nps = totalRes > 0 ? fmtPercent(((totalPro - totalDet) / totalRes) * 100) : 0;

        ytdRow.update(d);
    }
}

function saveCsatSummary() {

    if (!table) return;

    // Ensure all summary values are up to date
    calculateAllSummaries();

    const data = table.getData();

    const toFloatOrNull = (v) => {
        if (v === null || v === undefined) return null;
        const s = String(v).trim();
        if (!s) return null;
        const n = parseFloat(s);
        return isNaN(n) ? null : n;
    };

    const toStringOrNull = (v) => {
        if (v === null || v === undefined) return null;
        const s = String(v).trim();
        return s === "" ? null : s;
    };

    // Id from hidden or YTD row
    let csatId = 0;
    const hiddenId = $("#hdnCsatId").val();
    if (hiddenId && !isNaN(parseInt(hiddenId))) {
        csatId = parseInt(hiddenId);
    } else {
        const ytd = data.find(r => r.quarter === "YTD");
        if (ytd && ytd.Id && !isNaN(parseInt(ytd.Id))) {
            csatId = parseInt(ytd.Id);
        }
    }

    const currentUser = $("#hdnUserName").val() || null;

    // Base entity – rest of the properties will be added in the loop
    const entity = {
        Id: csatId,
        IsDeleted: false,
        CreatedBy: currentUser,
        UpdatedBy: currentUser
    };

    data.forEach(r => {
        const pc = (r.pc || "").toString().trim();
        const q = (r.quarter || "").toString().trim();

        const req = toFloatOrNull(r.reqSent);
        const res = toFloatOrNull(r.resRec);
        const pro = toFloatOrNull(r.promoters);
        const coll = toFloatOrNull(r.collection);
        const det = toFloatOrNull(r.detractors);
        const nps = toFloatOrNull(r.nps);
        const detDtl = toStringOrNull(r.detDetails);

        switch (pc) {

            // Q1
            case "PC1":
                entity.Q1Pc1_ReqSent = req;
                entity.Q1Pc1_ResRece = res;
                entity.Q1Pc1_Promoter = pro;
                entity.Q1Pc1_Collection = coll;
                entity.Q1Pc1_Detractor = det;
                entity.Q1Pc1_Nps = nps;
                entity.Q1Pc1_Detractor_Detail = detDtl;
                break;

            case "PC2":
                entity.Q1Pc2_ReqSent = req;
                entity.Q1Pc2_ResRece = res;
                entity.Q1Pc2_Promoter = pro;
                entity.Q1Pc2_Collection = coll;
                entity.Q1Pc2_Detractor = det;
                entity.Q1Pc2_Nps = nps;
                entity.Q1Pc2_Detractor_Detail = detDtl;
                break;

            case "PC3":
                entity.Q1Pc3_ReqSent = req;
                entity.Q1Pc3_ResRece = res;
                entity.Q1Pc3_Promoter = pro;
                entity.Q1Pc3_Collection = coll;
                entity.Q1Pc3_Detractor = det;
                entity.Q1Pc3_Nps = nps;
                entity.Q1Pc3_Detractor_Detail = detDtl;
                break;

            case "Q1":
                entity.Q1Q1_ReqSent = req;
                entity.Q1Q1_ResRece = res;
                entity.Q1Q1_Promoter = pro;
                entity.Q1Q1_Collection = coll;
                entity.Q1Q1_Detractor = det;
                entity.Q1Q1_Nps = nps;
                entity.Q1Q1_Detractor_Detail = detDtl;
                break;

            // Q2
            case "PC4":
                entity.Q2Pc4_ReqSent = req;
                entity.Q2Pc4_ResRece = res;
                entity.Q2Pc4_Promoter = pro;
                entity.Q2Pc4_Collection = coll;
                entity.Q2Pc4_Detractor = det;
                entity.Q2Pc4_Nps = nps;
                entity.Q2Pc4_Detractor_Detail = detDtl;
                break;

            case "PC5":
                entity.Q2Pc5_ReqSent = req;
                entity.Q2Pc5_ResRece = res;
                entity.Q2Pc5_Promoter = pro;
                entity.Q2Pc5_Collection = coll;
                entity.Q2Pc5_Detractor = det;
                entity.Q2Pc5_Nps = nps;
                entity.Q2Pc5_Detractor_Detail = detDtl;
                break;

            case "PC6":
                entity.Q2Pc6_ReqSent = req;
                entity.Q2Pc6_ResRece = res;
                entity.Q2Pc6_Promoter = pro;
                entity.Q2Pc6_Collection = coll;
                entity.Q2Pc6_Detractor = det;
                entity.Q2Pc6_Nps = nps;
                entity.Q2Pc6_Detractor_Detail = detDtl;
                break;

            case "Q2":
                entity.Q2Q2_ReqSent = req;
                entity.Q2Q2_ResRece = res;
                entity.Q2Q2_Promoter = pro;
                entity.Q2Q2_Collection = coll;
                entity.Q2Q2_Detractor = det;
                entity.Q2Q2_Nps = nps;
                entity.Q2Q2_Detractor_Detail = detDtl;
                break;

            // Q3
            case "PC7":
                entity.Q3Pc7_ReqSent = req;
                entity.Q3Pc7_ResRece = res;
                entity.Q3Pc7_Promoter = pro;
                entity.Q3Pc7_Collection = coll;
                entity.Q3Pc7_Detractor = det;
                entity.Q3Pc7_Nps = nps;
                entity.Q3Pc7_Detractor_Detail = detDtl;
                break;

            case "PC8":
                entity.Q3Pc8_ReqSent = req;
                entity.Q3Pc8_ResRece = res;
                entity.Q3Pc8_Promoter = pro;
                entity.Q3Pc8_Collection = coll;
                entity.Q3Pc8_Detractor = det;
                entity.Q3Pc8_Nps = nps;
                entity.Q3Pc8_Detractor_Detail = detDtl;
                break;

            case "PC9":
                entity.Q3Pc9_ReqSent = req;
                entity.Q3Pc9_ResRece = res;
                entity.Q3Pc9_Promoter = pro;
                entity.Q3Pc9_Collection = coll;
                entity.Q3Pc9_Detractor = det;
                entity.Q3Pc9_Nps = nps;
                entity.Q3Pc9_Detractor_Detail = detDtl;
                break;

            case "Q3":
                entity.Q3Q3_ReqSent = req;
                entity.Q3Q3_ResRece = res;
                entity.Q3Q3_Promoter = pro;
                entity.Q3Q3_Collection = coll;
                entity.Q3Q3_Detractor = det;
                entity.Q3Q3_Nps = nps;
                entity.Q3Q3_Detractor_Detail = detDtl;
                break;

            // Q4
            case "PC10":
                entity.Q4Pc10_ReqSent = req;
                entity.Q4Pc10_ResRece = res;
                entity.Q4Pc10_Promoter = pro;
                entity.Q4Pc10_Collection = coll;
                entity.Q4Pc10_Detractor = det;
                entity.Q4Pc10_Nps = nps;
                entity.Q4Pc10_Detractor_Detail = detDtl;
                break;

            case "PC11":
                entity.Q4Pc11_ReqSent1 = req;
                entity.Q4Pc11_ResRece1 = res;
                entity.Q4Pc11_Promoter1 = pro;
                entity.Q4Pc11_Collection1 = coll;
                entity.Q4Pc11_Detractor1 = det;
                entity.Q4Pc11_Nps1 = nps;
                entity.Q4Pc11_Detractor_Detail1 = detDtl;
                break;

            case "PC12":
                entity.Q4Pc12_ReqSent1 = req;
                entity.Q4Pc12_ResRece1 = res;
                entity.Q4Pc12_Promoter1 = pro;
                entity.Q4Pc12_Collection1 = coll;
                entity.Q4Pc12_Detractor1 = det;
                entity.Q4Pc12_Nps1 = nps;
                entity.Q4Pc12_Detractor_Detail1 = detDtl;
                break;

            case "Q4":
                entity.Q4Q4_ReqSent1 = req;
                entity.Q4Q4_ResRece1 = res;
                entity.Q4Q4_Promoter1 = pro;
                entity.Q4Q4_Collection1 = coll;
                entity.Q4Q4_Detractor1 = det;
                entity.Q4Q4_Nps1 = nps;
                entity.Q4Q4_Detractor_Detail1 = detDtl;
                break;
        }

        // YTD row (quarter = "YTD")
        if (q === "YTD") {
            entity.Ytd_ReqSent11 = req;
            entity.Ytd_ResRece11 = res;
            entity.Ytd_Promoter11 = pro;
            entity.Ytd_Collection11 = coll;
            entity.Ytd_Detractor11 = det;
            entity.Ytd_Nps11 = nps;
            entity.Ytd_Detractor_Detail11 = detDtl;
        }
    });

    const isNew = !entity.Id || entity.Id === 0;
    const url = isNew ? "/CSATSummary/Create" : "/CSATSummary/Update";   // adjust to your controller

    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(entity),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data && data.success) {

                //if (isNew && (data.id || data.Id)) {
                //    const newId = data.id ?? data.Id;
                //    $("#hdnCsatId").val(newId);

                //    // Push Id into all rows
                //    table.getRows().forEach(r => {
                //        const d = r.getData();
                //        d.Id = newId;
                //        r.update(d);
                //    });
                //}

                if ($("#hdnCsatId").val() != "0") {
                    showSuccessAlert("CSAT summary is updated successfully!");
                }
                else {
                    showSuccessAlert("CSAT summary is Saved Successfully!");

                }

                setTimeout(function () {
                    window.location.href = '/CSATSummary/CSATSUMMARY';
                }, 2500);

            } else {
                let errorMessg = "";
                if (data && data.errors) {
                    for (const k in data.errors) {
                        if (Object.prototype.hasOwnProperty.call(data.errors, k)) {
                            errorMessg += data.errors[k] + "\n";
                        }
                    }
                }
                showDangerAlert(errorMessg || (data && data.message) || "An error occurred while saving CSAT summary.");
            }
        },
        error: function (xhr) {
            showDangerAlert(xhr.responseText || "Error saving CSAT summary.");
        }
    });
}

function loadCsatSummary(id) {
    debugger;
    Blockloadershow();

    $.ajax({
        url: "/CSATSummary/GetById",
        type: "GET",
        dataType: "json",
        data: { id: id },
        success: function (entity) {
            Blockloaderhide();

            if (!entity) {
                showDangerAlert("No CSAT summary found for this Id.");
                return;
            }

            bindCsatEntityToTable(entity);
        },
        error: function (xhr, status, error) {
            Blockloaderhide();
            showDangerAlert("Error loading CSAT summary: " + (error || xhr.responseText));
        }
    });
}

function bindCsatEntityToTable(entity) {
    if (!table) return;

    const rows = table.getRows();
    const csatId = entity.id || 0;

    rows.forEach(row => {
        const d = row.getData();
        const pc = (d.pc || "").toString().trim();
        const q = (d.quarter || "").toString().trim();

        d.Id = csatId;

        switch (pc) {
            // ---------- Q1 ----------
            case "PC1":
                d.reqSent =    entity.q1Pc1_ReqSent;
                d.resRec =     entity.q1Pc1_ResRece;
                d.promoters =  entity.q1Pc1_Promoter;
                d.collection = entity.q1Pc1_Collection;
                d.detractors = entity.q1Pc1_Detractor;
                d.nps =        entity.q1Pc1_Nps;
                d.detDetails = entity.q1Pc1_Detractor_Detail;
                break;

            case "PC2":
                d.reqSent =     entity.q1Pc2_ReqSent;
                d.resRec =      entity.q1Pc2_ResRece;
                d.promoters =   entity.q1Pc2_Promoter;
                d.collection =  entity.q1Pc2_Collection;
                d.detractors =  entity.q1Pc2_Detractor;
                d.nps =         entity.q1Pc2_Nps;
                d.detDetails =  entity.q1Pc2_Detractor_Detail;
                break;

            case "PC3":
                d.reqSent =   entity.q1Pc3_ReqSent;
                d.resRec =    entity.q1Pc3_ResRece;
                d.promoters = entity.q1Pc3_Promoter;
                d.collection = entity.q1Pc3_Collection;
                d.detractors = entity.q1Pc3_Detractor;
                d.nps =        entity.q1Pc3_Nps;
                d.detDetails = entity.q1Pc3_Detractor_Detail;
                break;

            case "Q1":
                d.reqSent =  entity.q1Q1_ReqSent;
                d.resRec =   entity.q1Q1_ResRece;
                d.promoters =  entity.q1Q1_Promoter;
                d.collection = entity.q1Q1_Collection;
                d.detractors = entity.q1Q1_Detractor;
                d.nps =        entity.q1Q1_Nps;
                d.detDetails = entity.q1Q1_Detractor_Detail;
                break;

            // ---------- Q2 ----------
            case "PC4":
                d.reqSent =     entity.q2Pc4_ReqSent;
                d.resRec =      entity.q2Pc4_ResRece;
                d.promoters =   entity.q2Pc4_Promoter;
                d.collection =  entity.q2Pc4_Collection;
                d.detractors =  entity.q2Pc4_Detractor;
                d.nps =         entity.q2Pc4_Nps;
                d.detDetails =  entity.q2Pc4_Detractor_Detail;
                break;

            case "PC5":
                d.reqSent =  entity.q2Pc5_ReqSent;
                d.resRec =   entity.q2Pc5_ResRece;
                d.promoters =  entity.q2Pc5_Promoter;
                d.collection = entity.q2Pc5_Collection;
                d.detractors = entity.q2Pc5_Detractor;
                d.nps =        entity.q2Pc5_Nps;
                d.detDetails = entity.q2Pc5_Detractor_Detail;
                break;

            case "PC6":
                d.reqSent =     entity.q2Pc6_ReqSent;
                d.resRec =      entity.q2Pc6_ResRece;
                d.promoters =   entity.q2Pc6_Promoter;
                d.collection =  entity.q2Pc6_Collection;
                d.detractors =  entity.q2Pc6_Detractor;
                d.nps =         entity.q2Pc6_Nps;
                d.detDetails =  entity.q2Pc6_Detractor_Detail;
                break;

            case "Q2":
                d.reqSent =     entity.q2Q2_ReqSent;
                d.resRec =      entity.q2Q2_ResRece;
                d.promoters =   entity.q2Q2_Promoter;
                d.collection =  entity.q2Q2_Collection;
                d.detractors =  entity.q2Q2_Detractor;
                d.nps =         entity.q2Q2_Nps;
                d.detDetails =  entity.q2Q2_Detractor_Detail;
                break;

            // ---------- Q3 ----------
            case "PC7":
                d.reqSent =     entity.q3Pc7_ReqSent;
                d.resRec =      entity.q3Pc7_ResRece;
                d.promoters =   entity.q3Pc7_Promoter;
                d.collection =  entity.q3Pc7_Collection;
                d.detractors =  entity.q3Pc7_Detractor;
                d.nps =         entity.q3Pc7_Nps;
                d.detDetails =  entity.q3Pc7_Detractor_Detail;
                break;

            case "PC8":
                d.reqSent =     entity.q3Pc8_ReqSent;
                d.resRec =      entity.q3Pc8_ResRece;
                d.promoters =   entity.q3Pc8_Promoter;
                d.collection =  entity.q3Pc8_Collection;
                d.detractors =  entity.q3Pc8_Detractor;
                d.nps =         entity.q3Pc8_Nps;
                d.detDetails =  entity.q3Pc8_Detractor_Detail;
                break;

            case "PC9":
                d.reqSent =     entity.q3Pc9_ReqSent;
                d.resRec =      entity.q3Pc9_ResRece;
                d.promoters =   entity.q3Pc9_Promoter;
                d.collection =  entity.q3Pc9_Collection;
                d.detractors =  entity.q3Pc9_Detractor;
                d.nps =         entity.q3Pc9_Nps;
                d.detDetails =  entity.q3Pc9_Detractor_Detail;
                break;

            case "Q3":
                d.reqSent = entity.Q3Q3_ReqSent;
                d.resRec = entity.Q3Q3_ResRece;
                d.promoters = entity.Q3Q3_Promoter;
                d.collection = entity.Q3Q3_Collection;
                d.detractors = entity.Q3Q3_Detractor;
                d.nps = entity.Q3Q3_Nps;
                d.detDetails = entity.Q3Q3_Detractor_Detail;
                break;

            // ---------- Q4 ----------
            case "PC10":
                d.reqSent = entity.Q4Pc10_ReqSent;
                d.resRec = entity.Q4Pc10_ResRece;
                d.promoters = entity.Q4Pc10_Promoter;
                d.collection = entity.Q4Pc10_Collection;
                d.detractors = entity.Q4Pc10_Detractor;
                d.nps = entity.Q4Pc10_Nps;
                d.detDetails = entity.Q4Pc10_Detractor_Detail;
                break;

            case "PC11":
                d.reqSent = entity.Q4Pc11_ReqSent1;
                d.resRec = entity.Q4Pc11_ResRece1;
                d.promoters = entity.Q4Pc11_Promoter1;
                d.collection = entity.Q4Pc11_Collection1;
                d.detractors = entity.Q4Pc11_Detractor1;
                d.nps = entity.Q4Pc11_Nps1;
                d.detDetails = entity.Q4Pc11_Detractor_Detail1;
                break;

            case "PC12":
                d.reqSent = entity.Q4Pc12_ReqSent1;
                d.resRec = entity.Q4Pc12_ResRece1;
                d.promoters = entity.Q4Pc12_Promoter1;
                d.collection = entity.Q4Pc12_Collection1;
                d.detractors = entity.Q4Pc12_Detractor1;
                d.nps = entity.Q4Pc12_Nps1;
                d.detDetails = entity.Q4Pc12_Detractor_Detail1;
                break;

            case "Q4":
                d.reqSent = entity.Q4Q4_ReqSent1;
                d.resRec = entity.Q4Q4_ResRece1;
                d.promoters = entity.Q4Q4_Promoter1;
                d.collection = entity.Q4Q4_Collection1;
                d.detractors = entity.Q4Q4_Detractor1;
                d.nps = entity.Q4Q4_Nps1;
                d.detDetails = entity.Q4Q4_Detractor_Detail1;
                break;
        }

        // YTD row
        if (q === "YTD") {
            d.reqSent = entity.Ytd_ReqSent11;
            d.resRec = entity.Ytd_ResRece11;
            d.promoters = entity.Ytd_Promoter11;
            d.collection = entity.Ytd_Collection11;
            d.detractors = entity.Ytd_Detractor11;
            d.nps = entity.Ytd_Nps11;
            d.detDetails = entity.Ytd_Detractor_Detail11;
        }

        row.update(d);
    });

    // Optional – recalc to ensure formulas refreshed
    calculateAllSummaries();
}

