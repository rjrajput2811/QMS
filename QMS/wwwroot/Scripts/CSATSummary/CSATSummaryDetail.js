$(document).ready(function () {
    OnTabGridLoad();
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

    var tableData = [];
    Object.keys(quarters).forEach(q => {
        quarters[q].forEach((pc, index) => {
            tableData.push({
                quarter: index === 0 ? q : "",
                pc: pc,
                reqSent: "",
                resRec: "",
                promoters: "",
                collection: "",
                detractors: "",
                nps: "",
                detDetails: ""
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
        detDetails: ""
    });

    window.csatTable = new Tabulator("#csatTable", {
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
            }
        ],

        movableColumns: false,
        resizableRows: false
    });

    // Export
    document.getElementById("exportButton").addEventListener("click", function () {
        window.csatTable.download("xlsx", "CSAT_Summary.xlsx", { sheetName: "CSAT Summary" });
    });

    // Save button
    document.getElementById("saveButton").addEventListener("click", function () {
        console.log("Saved Data:", window.csatTable.getData());
        alert("Data saved to console (for demo).");
    });
}