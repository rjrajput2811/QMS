
function openUploadComplaint() {
    const columns = [
        'CCN No', 'CCCN Date', 'ReportedBy', 'Location', 'Customer Name', 'Dealer Name',
        'Description', 'Status', 'Completion', 'ClosureRemarks', 'Time Taken for Closure (DAYS)'
    ];
    setUploadConfig('/Service/UploadComplaintDumpExcel', columns);
}

function openUploadPo() {
    const columns = [
        'Vendor', 'Material', 'Reference No', 'PO No', 'PO Date', 'Batch No',
        'PO Qty', 'Balance Qty', 'Destination', 'Balance Value'
    ];
    setUploadConfig('/Service/UploadPoDumpExcel', columns);
}

function openUploadIndent() {
    const columns = [
        'Indent No', 'Indent Date', 'Business Unit', 'Vertical', 'Branch', 'Indent Status',
        'End Customer Name', 'CCN No', 'Customer Code', 'Customer Name', 'Bill Request Date',
        'Created By', 'Wipro Commit Date', 'Material No', 'Item Description', 'Quantity', 'Price', 'Discount',
        'Final Price', 'SAPSO No', 'Create SoQty', 'Inv_qnty', 'Inv_value', 'Wipro Catelog No',
        'Batch Code', 'Batch Date', 'MainProd Code', 'User Name'
    ];
    setUploadConfig('/Service/UploadIndentDumpExcel', columns);
}

function openUploadInvoice() {
    const columns = [
        'Key', 'Invoice No.', 'Invoice Type', 'Sales Order', 'Plant Code', 'Plant Name', 'Material No.', 'Dealer Name', 'End Customer',
        'Collective No', 'Indent No', 'Invoice Date', 'Quantity', 'Cost'
    ];
    setUploadConfig('/Service/UploadInvoiceExcel', columns);
}

function openUploadPCChart() {
    const columns = [
        'Date', 'PC', 'FY', 'Qtr'
    ];
    setUploadConfig('/Service/UploadPcChartExcel', columns);
}

function openUploadRegion() {
    const columns = [
        'Location', 'Region'
    ];
    setUploadConfig('/Service/UploadRegionExcel', columns);
}

function setUploadConfig(url, columns) {
    $('#upload-url').val(url);
    $('#expected-columns').val(JSON.stringify(columns));
    $('#fileInput').val('');
    $('#uploadModal').modal('show');
}

function AutoReload() {
    setTimeout(function () { window.location.reload(); }, 1500);
}
function createUploadButton() {
    // Create the uploadData button
    var uploadButton = document.createElement('button');
    uploadButton.id = 'btnUploadData';
    uploadButton.type = 'button';
    uploadButton.dataset.toggle = 'modal';
    uploadButton.dataset.target = '#uploadModal';
    uploadButton.style.cssText = 'float: right; margin-left: 15px';
    uploadButton.className = 'btn btn-outline-success legitRipple';
    uploadButton.innerHTML = '<i class="fas fa-upload mr-2 fa-1x"></i>Upload Data';

    // Return the button so it can be appended outside the function
    return uploadButton;
}

$('#btnUploadData').on('click', function () {
    $('#fileInput').val(''); // Clear the file input
    $('#uploadModal').modal('show').on('shown.bs.modal', function () {
        var backdrops = $('.modal-backdrop');

        if (backdrops.length === 0) {
            // If no backdrop exists, add it manually
            $('<div class="modal-backdrop fade show"></div>').appendTo(document.body);
        } else if (backdrops.length > 1) {
            // If more than one backdrop exists, remove the extras
            backdrops.not(':first').remove();
        }
    });
});

$('#fileInput').change(function (event) {
    const file = event.target.files[0];
    if (file) {
        // Validate file type
        const validTypes = [
            'application/vnd.ms-excel', // .xls
            'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', // .xlsx
            'application/vnd.ms-excel.sheet.macroEnabled.12' // .xlsm
        ];

        if (!validTypes.includes(file.type)) {
            showDangerAlert('Please select a valid Excel file (xls or xlsx).');
            $('#fileInput').val('');
        }
    }
});

$('#close-button').on('click', function () {
    $('#uploadModal').modal('hide');
});

$('#modalClose').on('click', function () {
    $('#uploadModal').modal('hide');
});

function handleImportExcelFileBak(url, expectedColumns)
{
    $('#uploadModal').modal('hide').on('hidden.bs.modal', function () {
        $('.modal-backdrop').remove(); // Remove the backdrop after modal hides
    });
    Blockloadershow();
    var fileInput = $('#fileInput')[0];
    var file = fileInput.files[0];

    if (file) {
        // Validate file type
        var validTypes = ['application/vnd.ms-excel', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 'application/vnd.ms-excel.sheet.macroEnabled.12'];
        if (validTypes.includes(file.type)) {
            // Read the uploaded Excel file
            var reader = new FileReader();
            reader.onload = async function (e) {
                var data = e.target.result;

                // Load the workbook using exceljs
                const ExcelJS = window.ExcelJS; // Ensure ExcelJS is included
                const workbook = new ExcelJS.Workbook();
                await workbook.xlsx.load(data);

                // Get the first sheet and its header
                const worksheet = workbook.worksheets[0];
                const firstRow = worksheet.getRow(1);
                const uploadedColumns = firstRow.values.slice(1).map(cell => {
                    if (cell === null || cell === undefined) return '';
                    if (typeof cell === 'object') {
                        if (cell.richText && Array.isArray(cell.richText)) {
                            // Concatenate rich text segments
                            return cell.richText.map(segment => segment.text || '').join('').trim();
                        }
                        return (cell.text || cell.toString()).trim();
                    }
                    return cell.toString().trim();
                });

                // Check if uploaded columns match expected columns
                var columnsMatch = uploadedColumns.length === expectedColumns.length &&
                    uploadedColumns.every(function (col, index) {
                        return col === expectedColumns[index];
                    });

                if (columnsMatch) {

                    const numberOfRecords = worksheet.rowCount - 1;
                    const uploadDate = new Date().toLocaleString();
                    var fileName = file.name;

                    var formData = new FormData();
                    formData.append('file', file);
                    formData.append('fileName', fileName);
                    formData.append('uploadDate', uploadDate);
                    formData.append('recordCount', numberOfRecords);
                    $.ajax({
                        url: url,
                        type: 'POST',
                        data: formData,
                        contentType: false,
                        processData: false,
                        success: function (response) {
                            Blockloaderhide();
                            if (response.success) {
                                showSuccessAlert("Data uploaded successfully.");

                                //// Display file details in the modal
                                //$('#uploadModal').modal('show');
                                //$('#fileNameDisplay').text(file.name);
                                //$('#uploadDateDisplay').text(uploadDate);
                                //$('#recordCountDisplay').text(numberOfRecords);

                                AutoReload();
                            }
                            else {
                                showDangerAlert(response.message);
                                AutoReload();
                            }
                        },
                        error: function () {
                            Blockloaderhide();
                            showDangerAlert('Error occured while uploading data.');
                        }
                    });
                } else {
                    Blockloaderhide();
                    $('#fileInput').val('');
                    showDangerAlert('Please upload data using the provided sample Master template file.');
                }
            };

            reader.readAsArrayBuffer(file);
        } else {
            Blockloaderhide();
            $('#fileInput').val('');
            showDangerAlert('Please select a valid Excel file (xls or xlsx).');
        }
    } else {
        Blockloaderhide();
        showDangerAlert('Please select a file to upload.');
    }
}

function handleImportExcelFile(url, expectedColumns) {
    $('#uploadModal').modal('hide').on('hidden.bs.modal', function () {
        $('.modal-backdrop').remove();
    });

    Blockloadershow();

    var fileInput = $('#fileInput')[0];
    var file = fileInput.files[0];

    if (!file) {
        Blockloaderhide();
        showDangerAlert('Please select a file to upload.');
        return;
    }

    var validTypes = [
        'application/vnd.ms-excel',
        'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        'application/vnd.ms-excel.sheet.macroEnabled.12'
    ];

    if (!validTypes.includes(file.type)) {
        Blockloaderhide();
        $('#fileInput').val('');
        showDangerAlert('Please select a valid Excel file (xls or xlsx).');
        return;
    }

    var reader = new FileReader();
    reader.onload = async function (e) {
        var data = e.target.result;

        const ExcelJS = window.ExcelJS;
        const workbook = new ExcelJS.Workbook();
        await workbook.xlsx.load(data);

        const worksheet = workbook.worksheets[0];
        const firstRow = worksheet.getRow(1);
        const uploadedColumns = firstRow.values.slice(1).map(cell => {
            if (!cell) return '';
            if (typeof cell === 'object') {
                if (cell.richText && Array.isArray(cell.richText)) {
                    return cell.richText.map(segment => segment.text || '').join('').trim();
                }
                return (cell.text || cell.toString()).trim();
            }
            return cell.toString().trim();
        });

        var columnsMatch = uploadedColumns.length === expectedColumns.length &&
            uploadedColumns.every((col, index) => col === expectedColumns[index]);

        if (!columnsMatch) {
            Blockloaderhide();
            $('#fileInput').val('');
            showDangerAlert('Please upload data using the provided sample Master template file.');
            return;
        }

        const numberOfRecords = worksheet.rowCount - 1;
        const uploadDate = new Date().toISOString(); // ISO format for backend
        const fileName = file.name;

        var formData = new FormData();
        formData.append('file', file);
        formData.append('fileName', fileName);
        formData.append('uploadDate', uploadDate);
        formData.append('recordCount', numberOfRecords);

        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            xhrFields: {
                responseType: 'blob'
            }, // For Excel file if returned
            success: function (blob, status, xhr) {
                Blockloaderhide();

                const disposition = xhr.getResponseHeader('Content-Disposition');
                const isFile = disposition && disposition.indexOf('attachment') !== -1;

                if (isFile) {
                    // Download Excel log
                    const filename = disposition.split('filename=')[1].replace(/"/g, '');
                    const url = window.URL.createObjectURL(blob);
                    const a = document.createElement('a');
                    a.href = url;
                    a.download = filename;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();

                    showWarningAlert("Some records failed to import. Please review the downloaded log.");
                    AutoReload();
                } else {
                    // JSON result
                    const reader = new FileReader();
                    reader.onload = function () {
                        try {
                            // Check if result is empty
                            if (!reader.result || reader.result.trim() === "") {
                                showDangerAlert("Empty response from server. Please check server logs for details.");
                                return;
                            }
                            const response = JSON.parse(reader.result);
                            if (response.success) {
                                showSuccessAlert(response.message);
                            } else {
                                showDangerAlert(response.message);

                                if (response.failedRecords && response.failedRecords.length > 0) {
                                    let details = '<strong>Failed Records:</strong><ul>';
                                    response.failedRecords.forEach(function (item) {
                                        details += `<li>Key: ${item.key || 'N/A'}, PO No: ${item.po_No || 'N/A'} — Reason: ${item.reason}</li>`;
                                    });
                                    details += '</ul>';
                                    $('#errorLogContainer').html(details).show();
                                }
                            }
                            AutoReload();
                        } catch (err) {
                            showDangerAlert("Error occurred while processing response. " + err.message);
                        }
                    };
                    reader.readAsText(blob);
                }
            },
            error: function () {
                Blockloaderhide();
                showDangerAlert('Error occurred while uploading data.');
            }
        });
    };

    reader.readAsArrayBuffer(file);
}
