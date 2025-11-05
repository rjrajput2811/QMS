var tabledata = [];
var table = null;
var tabledataNatProject = [];
var tableNatProject = '';
const searchTerms = {};
let vendorOptions = {};
let natProjectOptions = {};
let selectedNatProjectCell = null;
let filterStartFIFODate = moment().startOf('month').format('YYYY-MM-DD');
let filterEndFIFODate = moment().endOf('month').format('YYYY-MM-DD');

$(document).ready(function () {

    $('#dateRangeFIFO').text(
        moment(filterStartFIFODate).format('MMMM D, YYYY') + ' - ' + moment(filterEndFIFODate).format('MMMM D, YYYY')
    );

    const picker = new Litepicker({
        element: document.getElementById('customDateTriggerFIFO'),
        singleMode: false,
        format: 'DD-MM-YYYY',
        numberOfMonths: 2,
        numberOfColumns: 2,
        dropdowns: { minYear: 2020, maxYear: null, months: true, years: true },
        plugins: ['ranges'],
        setup: (picker) => {
            picker.on('selected', (start, end) => {
                filterStartFIFODate = start.format('YYYY-MM-DD');
                filterEndFIFODate = end.format('YYYY-MM-DD');
                $('#dateRangeFIFO').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadData();
            });

            picker.on('clear', () => {
                filterStartFIFODate = "";
                filterEndFIFODate = "";
                $('#dateRangeFIFO').text("Select Date Range");
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

    $('#customDateTriggerFIFO').on('click', function () {
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
        // Step 3: Load grid data
        $.ajax({
            url: '/FIFOTrac/GetAll',
            type: 'GET',
            dataType: 'json',
            data: {
                startDate: filterStartFIFODate,
                endDate: filterEndFIFODate
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


(function (global) {
    function escapeHTML(s) {
        return String(s ?? "").replace(/[&<>"']/g, m => (
            { "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" }[m]
        ));
    }

    // Parse "CODE — DESC | CODE — DESC" into Map(code -> desc)
    function parsePairs(text, pairSep, itemSep) {
        const map = new Map();
        const raw = String(text || "").trim();
        if (!raw) return map;
        raw.split(itemSep).map(s => s.trim()).filter(Boolean).forEach(part => {
            const i = part.indexOf(pairSep);
            if (i > -1) {
                const code = part.slice(0, i).trim();
                const desc = part.slice(i + pairSep.length).trim();
                if (code) map.set(code, desc);
            }
        });
        return map;
    }

    // NEW: formatter that shows CODE — DESC chips in the ProductCode cell
    global.tagsFormatterPairs = function (cell, params) {
        const joinWith = params?.joinWith ?? ", ";
        const pairSeparator = params?.pairSeparator ?? " — "; // change to " - " if you prefer hyphen
        const pairJoinWith = params?.pairJoinWith ?? " | ";
        const descFieldParam = params?.descField; // string or array of possible fields

        // get codes from cell
        const raw = cell.getValue();
        const codes = Array.isArray(raw)
            ? raw
            : String(raw || "").split(joinWith).map(s => s.trim()).filter(Boolean);

        // find the description text in the row (e.g. ProductDescription / Sample_Desc)
        const row = cell.getRow().getData();
        let descText = "";
        if (Array.isArray(descFieldParam)) {
            descText = descFieldParam.map(f => row?.[f]).find(Boolean) || "";
        } else if (typeof descFieldParam === "string") {
            descText = row?.[descFieldParam] || "";
        } else {
            descText = row?.ProductDescription || row?.Sample_Desc || "";
        }

        const map = parsePairs(descText, pairSeparator, pairJoinWith);

        // build labels: CODE — DESC (fallback to CODE if desc not found yet)
        const labels = codes.map(code => {
            const d = map.get(String(code));
            return d ? `${code}${pairSeparator}${d}` : String(code);
        });

        // render chips
        return labels.map(lbl => `<span class="tab-tag">${escapeHTML(lbl)}</span>`).join(" ");
    };
})(window);

// Optional minimal chip styling
/* .tab-tag{display:inline-block;padding:2px 8px;margin:2px 4px 0 0;border:1px solid #ddd;border-radius:9999px;font-size:12px;background:#f7f7f7;line-height:20px;} */



Tabulator.extendModule("edit", "editors", {
    autocomplete_ajax_multi: function (cell, onRendered, success, cancel, editorParams) {
        // ---------- utilities ----------
        const _escapeHtml = s => String(s ?? "").replace(/[&<>"']/g,
            m => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[m]));

        function _firstField(obj, candidates) {
            for (const k of candidates) { if (obj && Object.prototype.hasOwnProperty.call(obj, k) && obj[k] != null) return obj[k]; }
            return undefined;
        }
        function _parseCodeDesc(label) {
            if (!label) return { code: undefined, desc: undefined };
            const s = String(label);
            const m = s.match(/^\s*([^—\-|]+?)\s*[—\-|]\s*(.+)\s*$/); // em dash OR hyphen OR pipe
            if (m) return { code: m[1].trim(), desc: m[2].trim() };
            return { code: s.trim(), desc: undefined };
        }

        // Parse "CODE — DESC | CODE — DESC" or "DESC | DESC" into Map(code->desc).
        function parsePairsFromText(text, pairSep, itemSep) {
            const map = new Map();
            const raw = String(text || "").trim();
            if (!raw) return map;

            const items = raw.split(itemSep).map(s => s.trim()).filter(Boolean);
            for (const part of items) {
                const m = part.split(pairSep);
                if (m.length >= 2) {
                    const code = m[0].trim();
                    const desc = m.slice(1).join(pairSep).trim();
                    if (code) map.set(code, desc);
                } else {
                    // If only desc present (legacy), we can't map to code reliably
                    // we'll keep it for later display but not code-map
                }
            }
            return map;
        }

        // chips formatter for read mode (re-used here)
        function tagsFormatterInline(arr, joinWith) {
            return arr.map(x => `<span class="tab-tag">${_escapeHtml(x)}</span>`).join(' ');
        }

        // ---------- config ----------
        const cfg = Object.assign({
            url: '/PDITracker/GetCodeSearch',
            queryParam: 'search',
            minChars: 3,
            debounce: 250,

            valueField: 'oldPart_No',
            valueFieldCandidates: ['oldPart_No', 'code', 'Code', 'sample_Cat_Ref', 'Sample_Cat_Ref'],
            labelField: 'oldPart_No',
            labelFieldCandidates: ['label', 'name', 'text', 'oldPart_No', 'sample_Cat_Ref', 'Sample_Cat_Ref'],
            descItemField: 'description',
            descItemFieldCandidates: ['description', 'desc', 'productDescription', 'ProductDescription', 'sample_Desc', 'Sample_Desc'],

            // Linked row description field(s) to update
            descField: ['Sample_Desc', 'ProductDescription'], // can be string or array
            descAsArray: false,              // store descs as array or as joined string
            descJoinWith: ' | ',             // joiner for desc-only format

            // NEW: actually used now
            descFormat: 'pair',              // 'pair' -> "CODE — DESC" ; 'desc' -> "DESC"
            descPairSeparator: ' — ',
            descPairJoinWith: ' | ',

            updateRowOnPick: true,

            asArray: true,                   // store codes as array in the cell
            joinWith: ', ',
            maxSelect: 0,
            allowFreeText: false
        }, editorParams || {});

        const descFields = Array.isArray(cfg.descField) ? cfg.descField : (cfg.descField ? [cfg.descField] : []);

        // ---------- UI ----------
        const wrap = document.createElement('div');
        wrap.className = 'tab-multi-ac'; wrap.style.width = "100%";
        const input = document.createElement('input');
        input.type = 'text'; input.autocomplete = 'off';
        wrap.appendChild(input);

        let dropdown = null, activeIndex = -1, pendingXHR = null, lastQuery = '';
        let selected = []; const selectedSet = new Set();

        function removeDropdown() { if (dropdown && dropdown.parentNode) dropdown.parentNode.removeChild(dropdown); dropdown = null; activeIndex = -1; }
        function positionDropdown() {
            if (!dropdown) return;
            const r = wrap.getBoundingClientRect();
            dropdown.style.top = (r.bottom) + "px";
            dropdown.style.left = (r.left) + "px";
            dropdown.style.minWidth = r.width + "px";
        }
        function cancelPending() { if (pendingXHR && pendingXHR.readyState !== 4) { try { pendingXHR.abort(); } catch (e) { } } pendingXHR = null; }

        const debouncedFetch = (() => { let t = null; return (q) => { clearTimeout(t); t = setTimeout(() => fetchSuggestions(q), cfg.debounce); }; })();

        function normalizeItem(raw) {
            let code = _firstField(raw, [cfg.valueField, ...cfg.valueFieldCandidates]);
            let desc = _firstField(raw, [cfg.descItemField, ...cfg.descItemFieldCandidates]);
            let label = _firstField(raw, [cfg.labelField, ...cfg.labelFieldCandidates]);
            if (!desc && label) { const parsed = _parseCodeDesc(label); if (!code && parsed.code) code = parsed.code; if (parsed.desc) desc = parsed.desc; }
            if (!label) label = desc ? `${code} ${cfg.descPairSeparator}${desc}` : (code ?? '');
            return { code: String(code || '').trim(), desc: desc ? String(desc).trim() : undefined, label: String(label || '').trim() };
        }

        function fetchSuggestions(q) {
            if (q === lastQuery) return;
            lastQuery = q;
            cancelPending();
            if (q.length < cfg.minChars) { removeDropdown(); return; }

            pendingXHR = $.ajax({
                url: cfg.url, type: 'GET', data: { [cfg.queryParam]: q }
            }).done(function (data) {
                renderDropdown(Array.isArray(data) ? data : []);
            }).always(function () { pendingXHR = null; });
        }

        function renderDropdown(items) {
            removeDropdown();
            dropdown = document.createElement('div');
            dropdown.className = 'autocomplete-dropdown';

            const normalized = items.map(normalizeItem).filter(it => it.code);
            const filtered = normalized.filter(it => !selectedSet.has(it.code));
            if (filtered.length === 0) {
                const empty = document.createElement('div');
                empty.className = 'autocomplete-option'; empty.style.opacity = .6;
                empty.textContent = 'No matches'; dropdown.appendChild(empty);
            } else {
                filtered.forEach((it) => {
                    const opt = document.createElement('div');
                    opt.className = 'autocomplete-option';
                    opt.innerHTML = _escapeHtml(it.label);
                    opt.addEventListener('mousedown', function (e) {
                        e.preventDefault(); e.stopPropagation();
                        addTag(it); flushTags();
                        input.focus(); debouncedFetch(input.value.trim());
                    });
                    dropdown.appendChild(opt);
                });
            }

            document.body.appendChild(dropdown);
            dropdown.addEventListener('mousedown', (e) => { e.stopPropagation(); e.preventDefault(); });
            dropdown.addEventListener('click', (e) => e.stopPropagation());
            dropdown.addEventListener('wheel', (e) => e.stopPropagation(), { passive: true });
            dropdown.addEventListener('touchmove', (e) => e.stopPropagation(), { passive: true });
            positionDropdown();
        }

        function flushTags() {
            [...wrap.querySelectorAll('.tab-tag')].forEach(e => e.remove());
            const labels = selected.map(it => it.desc ? `${it.code} ${cfg.descPairSeparator}${it.desc}` : (it.label || it.code));
            labels.forEach((label, i) => {
                const tag = document.createElement('span'); tag.className = 'tab-tag';
                tag.innerHTML = _escapeHtml(label) + ' <span class="tab-x" title="Remove">×</span>';
                tag.querySelector('.tab-x').addEventListener('mousedown', e => { e.preventDefault(); e.stopPropagation(); removeItem(selected[i].code); });
                wrap.insertBefore(tag, input);
            });
        }

        function addTag(item, repaint = true) {
            const code = (item.code ?? '').trim();
            if (!code || selectedSet.has(code)) return;
            if (cfg.maxSelect && selected.length >= cfg.maxSelect) return;

            selected.push({ code, desc: item.desc ?? undefined, label: item.label ?? code });
            selectedSet.add(code);
            if (repaint) flushTags();
            if (cfg.updateRowOnPick) syncLinkedDesc(); // will join ALL, not overwrite
            input.value = '';
        }

        function removeItem(code) {
            selected = selected.filter(it => it.code !== code);
            selectedSet.delete(code);
            flushTags();
            if (cfg.updateRowOnPick) syncLinkedDesc();
        }

        function codesArray() { return selected.map(it => it.code); }

        function buildDescOutput() {
            if (cfg.descFormat === 'pair') {
                const parts = selected.map(it => {
                    if (it.desc) return `${it.code}${cfg.descPairSeparator}${it.desc}`;
                    // if desc is unknown, still keep the code so user knows it exists
                    return it.code;
                });
                return parts.join(cfg.descPairJoinWith);
            } else { // 'desc'
                const descs = selected.map(it => it.desc).filter(Boolean);
                return cfg.descAsArray ? descs : descs.join(cfg.descJoinWith);
            }
        }

        function syncLinkedDesc() {
            if (!descFields.length) return;
            const row = cell.getRow();
            const val = buildDescOutput();
            const patch = {};
            descFields.forEach(f => patch[f] = val);
            row.update(patch); // reflect combined descriptions
        }

        function commit() {
            removeDropdown();
            const val = cfg.asArray ? codesArray() : codesArray().join(cfg.joinWith);
            if (descFields.length) syncLinkedDesc();
            success(val);
        }

        // ---------- init from current cell value + hydrate descs from row ----------
        (function initFromValue() {
            const raw = cell.getValue();
            const initialCodes = Array.isArray(raw) ? raw :
                (raw ? String(raw).split(cfg.joinWith).map(s => s.trim()).filter(Boolean) : []);

            // Try to read existing description text from row and map code→desc
            const rowData = cell.getRow().getData();
            const firstDescText = descFields.map(f => rowData && rowData[f]).find(Boolean);
            const knownMap = parsePairsFromText(firstDescText, cfg.descPairSeparator, cfg.descPairJoinWith);

            initialCodes.forEach(code => {
                const desc = knownMap.get(code);         // hydrate if we can
                addTag({ code, desc, label: desc ? `${code} ${cfg.descPairSeparator}${desc}` : code }, false);
            });

            flushTags();
            if (cfg.updateRowOnPick) syncLinkedDesc();
        })();

        // ---------- event handlers ----------
        input.addEventListener('keydown', function (e) {
            e.stopPropagation();

            const opts = dropdown ? dropdown.querySelectorAll('.autocomplete-option') : [];
            const max = opts.length - 1;

            if (e.key === 'ArrowDown') { e.preventDefault(); if (!dropdown) { debouncedFetch(input.value.trim()); return; } activeIndex = Math.min(max, activeIndex + 1); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); activeIndex = Math.max(0, activeIndex - 1); }
            else if (e.key === 'PageDown') { e.preventDefault(); activeIndex = Math.min(max, (activeIndex < 0 ? 0 : activeIndex) + 10); }
            else if (e.key === 'PageUp') { e.preventDefault(); activeIndex = Math.max(0, (activeIndex < 0 ? 0 : activeIndex) - 10); }
            else if (e.key === 'Home') { e.preventDefault(); activeIndex = 0; }
            else if (e.key === 'End') { e.preventDefault(); activeIndex = max; }
            else if (e.key === 'Enter') {
                e.preventDefault();
                if (dropdown && activeIndex >= 0 && opts[activeIndex]) {
                    opts[activeIndex].dispatchEvent(new MouseEvent('mousedown', { bubbles: true }));
                } else if (cfg.allowFreeText && input.value.trim() !== '') {
                    const v = input.value.trim();
                    addTag({ code: v, desc: v, label: v }); flushTags();
                } else {
                    commit();
                }
            } else if (e.key === 'Backspace' && input.value === '') {
                const last = selected[selected.length - 1];
                if (last) removeItem(last.code);
            } else if (e.key === 'Escape') {
                e.preventDefault(); removeDropdown(); return;
            }

            if (dropdown) {
                opts.forEach((el, i) => el.classList.toggle('active', i === activeIndex));
                if (activeIndex >= 0 && opts[activeIndex]) opts[activeIndex].scrollIntoView({ block: 'nearest' });
            }
        });

        input.addEventListener('input', () => debouncedFetch(input.value.trim()));

        const onDocClick = (e) => { if (!wrap.contains(e.target) && !(dropdown && dropdown.contains(e.target))) commit(); };
        document.addEventListener('mousedown', onDocClick, true);
        window.addEventListener('resize', positionDropdown, true);
        onRendered(() => { input.focus({ preventScroll: true }); positionDropdown(); });

        function destroy() {
            if (pendingXHR && pendingXHR.readyState !== 4) { try { pendingXHR.abort(); } catch (e) { } }
            removeDropdown();
            document.removeEventListener('mousedown', onDocClick, true);
            window.removeEventListener('resize', positionDropdown, true);
        }
        const _success = success, _cancel = cancel;
        success = (v) => { destroy(); _success(v); };
        cancel = () => { destroy(); _cancel(); };

        return wrap;
    }
});


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
                Sample_Recv_Date: formatDate(item.sample_Recv_Date),
                Sample_Cat_Ref: item.sample_Cat_Ref,
                Sample_Desc: item.sample_Desc,
                Vendor: item.vendor,
                Sample_Qty: item.sample_Qty,
                Test_Req: item.test_Req,
                Test_Status: item.test_Status,
                Responsbility: item.responsbility,
                Test_Completion_Date: formatDate(item.test_Completion_Date),
                Report_Release_Date: formatDate(item.report_Release_Date),
                NABL_Released_Date: formatDate(item.nabL_Released_Date),
                CreatedBy: item.createdBy,
                UpdatedBy: item.updatedBy,
                UpdatedDate: formatDate(item.updatedDate),
                CreatedDate: formatDate(item.createdDate),
                Current_Status: item.current_Status
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

        editableColumn("Sample Received Date", "Sample_Recv_Date", "date", "center"),

        {
            title: "Sample Cat Ref.",
            field: "Sample_Cat_Ref",
            editor: "autocomplete_ajax_multi",
            headerSort: false,
            headerMenu: headerMenu,
            headerFilter: "input",
            // use the PAIR-AWARE formatter instead of tagsFormatter
            formatter: tagsFormatterPairs,
            formatterParams: {
                joinWith: ", ",
                // where to read the combined desc text from (tries in order)
                descField: "",
                // how your description field is stored:
                pairSeparator: " — ",   // use " - " if you want a hyphen
                pairJoinWith: " | "
            },
            editorParams: {
                url: "/PDITracker/GetCodeSearch",
                queryParam: "search",
                minChars: 4,
                valueField: "oldPart_No",
                descItemField: "description",
                labelField: "oldPart_No",

                // keep these to ensure the row description is maintained as pairs:
                descField: "",
                descFormat: "pair",
                descPairSeparator: " — ",
                descPairJoinWith: " | ",

                asArray: true,
                joinWith: ", ",
                maxSelect: 0
            },
            widthGrow: 2
        },

        { title: "Sample Description", field: "Sample_Desc", widthGrow: 3, headerSort: false, headerMenu: headerMenu, headerFilter: "input" },

        editableColumn("Vendor.Requestor", "Vendor", "select2", "center", "input", {}, {
            values: vendorOptions
        }, function (cell) {
            const val = cell.getValue();
            return vendorOptions[val] || val;
        }, 130),

        editableColumn("Sample Qty", "Sample_Qty", true),
        editableColumn("Test Required", "Test_Req", true),
        editableColumn("Testing Status", "Test_Status", true),

        editableColumn("Test Completion Date", "Test_Completion_Date", "date", "center"),
        editableColumn("Report Release Date", "Report_Release_Date", "date", "center"),
        editableColumn("NABL Released Date", "NABL_Released_Date", "date", "center"),
        
        editableColumn("Current Status", "Current_Status", true),
        {
            title: "Final Report",
            field: "Final_Report",
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
    table = new Tabulator("#fifoTrac_Table", {
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
        //const field = cell.getField();
        //const row = cell.getRow();
        //const data = row.getData();

        //if (["Test_Duration", "Bis_Duration"].includes(field)) {
        //    return;
        //}

        //if (["Start_Date", "Comp_Date"].includes(field)) {
        //    const start = parseDate(data.Start_Date);
        //    const end = parseDate(data.Comp_Date);
        //    const diff = start && end ? Math.floor((end - start) / (1000 * 60 * 60 * 24)) : "";
        //    row.update({ Test_Duration: diff.toString() });
        //}

        //if (["Ven_Sample_Sub_Date", "Received_Date"].includes(field)) {
        //    const sub = parseDate(data.Ven_Sample_Sub_Date);
        //    const rec = parseDate(data.Received_Date);
        //    const diff = sub && rec ? Math.floor((rec - sub) / (1000 * 60 * 60 * 24)) : "";
        //    row.update({ Bis_Duration: diff.toString() });
        //}

        InsertUpdateFIFO(cell.getRow().getData());
    });


    (function bindAddButtonOnce() {
        var $btn = $("#addFIFOButton");
        $btn.attr("type", "button");                       // avoid form submit duplicates
        $btn.off("click.addrow").on("click.addrow", function (e) {
            e.preventDefault(); e.stopPropagation();
            if ($btn.data("busy")) return;                 // guard double-clicks
            $btn.data("busy", true).prop("disabled", true);

            try {

                const newRow = {
                    Sr_No: 1,               // will renumber after insert
                    Id: 0,
                    Sample_Recv_Date: "",
                    Sample_Cat_Ref: "",
                    Sample_Desc: "",
                    Vendor: "",
                    Sample_Qty: "",
                    Test_Req: "",
                    Test_Status: "",
                    Responsbility: "",
                    Test_Completion_Date: "",
                    Report_Release_Date: "",
                    NABL_Released_Date: "",
                    Current_Status : "",
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
    document.getElementById("exportFIFOButton").addEventListener("click", async function () {
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
        const TITLE_TEXT = "BIS PROJECT TRACKER";

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

function renumberSrNo() {
    const rows = table.getRows("active");
    $.each(rows, function (i, r) {
        const d = r.getData();
        if (d.Sr_No !== i + 1) { r.update({ Sr_No: i + 1 }); }
    });
}

$('#fifoTrac_Table').on('change', '.bis-upload', function () {
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
        url: "/FIFOTrac/UploadFIFOAttachment",
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
//            url: '/BisProjectTrac/Delete',
//            type: 'POST',
//            data: { id: recid },
//            success: function (data) {
//                if (data.success == true) {
//                    showSuccessAlert("Bis Projecet Deleted successfully.");
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

function delConfirm(recid, element) {
    if (!recid || recid <= 0) {
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
                            url: '/FIFOTrac/Delete',
                            type: 'POST',
                            data: { id: recid }
                        }).done(function (data) {
                            if (data && data.success === true) {
                                showSuccessNewAlert("FIFOTracker Detail deleted successfully.");
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

//function openBisProject(id) {
//    debugger
//    var url = '/BisProjectTrac/BisProjectTrackerDetail';
//    url = url + '?id=' + id
//    window.location.href = url;
//}

function InsertUpdateFIFO(rowData) {
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

    var Model = {
        Id: rowData.Id || 0,
        Sample_Recv_Date: toIsoDate(rowData.Sample_Recv_Date) || null,
        Sample_Cat_Ref: normalizeMultiToString(rowData.Sample_Cat_Ref, ", "),
        Sample_Desc: normalizeMultiToString(rowData.Sample_Desc, " | "),
        Vendor: rowData.Vendor || null,
        Sample_Qty: rowData.Sample_Qty || null,
        Test_Req: rowData.Test_Req || null,
        Test_Status: rowData.Test_Status || null,
        Responsbility: rowData.Responsbility || null,
        Test_Completion_Date: toIsoDate(rowData.Test_Completion_Date) || null,
        Report_Release_Date: toIsoDate(rowData.Report_Release_Date) || null,
        NABL_Released_Date: toIsoDate(rowData.NABL_Released_Date) || null,
        Current_Status: rowData.Current_Status || null
    };

    const isNew = Model.Id === 0;
    var ajaxUrl = isNew ? '/FIFOTrac/Create' : '/FIFOTrac/Update';

    $.ajax({
        url: ajaxUrl,
        type: "POST",
        data: JSON.stringify(Model),
        contentType: 'application/json',
        success: function (response) {
            //Blockloaderhide();
            if (response.success) {
                if (isNew) {
                    showSuccessNewAlert("Saved successfully!.");
                    loadData();
                }
            }
            else if (response.message === "Exist") {
                showDangerAlert("FIFO Tracker Detail already exists.");
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


//function loadNatProjectData() {
//    Blockloadershow();
//    $.ajax({
//        url: '/BisProjectTrac/GetNatProject',
//        type: 'GET',
//        success: function (data) {
//            Blockloaderhide();
//            if (data && Array.isArray(data)) {
//                OnNatProjectTabGridLoad(data);
//            } else {
//                showDangerAlert('No data available to load.');
//            }
//        },
//        error: function (xhr, status, error) {
//            Blockloaderhide();
//            showDangerAlert('Error retrieving data: ' + error);
//        }
//    });
//}

//function OnNatProjectTabGridLoad(response) {
//    debugger;
//    Blockloadershow();

//    tabledataNatProject = [];
//    let columns = [];

//    // Map the response to the table format
//    if (response.length > 0) {
//        $.each(response, function (index, item) {

//            function formatDate(value) {
//                return value ? new Date(value).toLocaleDateString("en-GB") : "";
//            }

//            tabledataNatProject.push({
//                Sr_No: index + 1,
//                Id: item.id,
//                Nat_Project: item.nat_Project,
//                CreatedBy: item.createdBy,
//                UpdatedBy: item.updatedBy,
//                UpdatedDate: formatDate(item.updatedDate),
//                CreatedDate: formatDate(item.createdDate),
//            });
//        });
//    }

//    if (tabledataNatProject.length === 0 && tableNatProject) {
//        tableNatProject.clearData();
//        Blockloaderhide();
//        return;
//    }

//    columns.push(
//        {
//            title: "Action",
//            field: "Action",
//            width: 46,
//            hozAlign: "center",
//            headerHozAlign: "center",
//            formatter: function (cell, formatterParams) {
//                const rowData = cell.getRow().getData();
//                let actionButtons = "";

//                actionButtons += `<i onclick="delNatProjectConfirm(${rowData.Id},this)" class="fas fa-trash-alt mr-2 fa-1x" title="Delete" style="color:red;cursor:pointer;margin-left: 5px;"></i>`

//                return actionButtons;
//            }
//        },
//        {
//            title: "SNo", field: "Sr_No", width: 48, sorter: "number", hozAlign: "center", headerHozAlign: "left"
//        },
//        editableColumn("Nature of Project", "Nat_Project", true),
//        { title: "CreatedBy", field: "CreatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
//        { title: "Created Date", field: "CreatedDate", width: 129, sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
//        { title: "Updated By", field: "UpdatedBy", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
//        { title: "Update Date", field: "UpdatedDate", sorter: "date", headerMenu: headerMenu, headerFilter: "input", hozAlign: "center", headerHozAlign: "center" },
//    );

//    // // Initialize Tabulator
//    tableNatProject = new Tabulator("#natProject_Table", {
//        data: tabledataNatProject,
//        renderHorizontal: "virtual",
//        movableColumns: true,
//        pagination: "local",
//        paginationSize: 10,
//        paginationSizeSelector: [50, 100, 500, 1500, 2000],
//        paginationCounter: "rows",
//        dataEmpty: "<div style='text-align: center; font-size: 1rem; color: gray;'>No data available</div>", // Placeholder message
//        columns: columns
//    });

//    tableNatProject.on("cellEdited", function (cell) {
//        InsertUpdateNatProject(cell.getRow().getData());
//    });

//    $("#addNatProjectBtn").on("click", function () {
//        const newRow1 = {
//            Sr_No: tableNatProject.getDataCount() + 1,
//            Id: 0,
//            Nat_Project: "",
//            CreatedBy: "",
//            UpdatedBy: "",
//            UpdatedDate: "",
//            CreatedDate: ""
//        };
//        tableNatProject.addRow(newRow1, false);
//    });



//    Blockloaderhide();
//}

//function InsertUpdateNatProject(rowData) {
//    debugger
//    if (!rowData) {
//        showDangerAlert("Invalid data provided.");
//        return;
//    }

//    Blockloadershow();
//    var errorMsg = "";

//    if (errorMsg !== "") {
//        Blockloaderhide();
//        showDangerAlert(errorMsg);
//        return false;
//    }

//    var Model = {
//        Id: rowData.Id || 0,
//        Nat_Project: rowData.Nat_Project || null,
//    };

//    var ajaxUrl = Model.Id === 0 ? '/BisProjectTrac/CreateNatProject' : '/BisProjectTrac/UpdateNatProject';

//    $.ajax({
//        url: ajaxUrl,
//        type: "POST",
//        data: JSON.stringify(Model),
//        contentType: 'application/json',
//        success: function (response) {
//            Blockloaderhide();
//            if (response.success) {
//                const msg = Model.Id != 0
//                    ? "Nature of Project updated successfully!"
//                    : "Nature of Project saved successfully!";
//                showSuccessAlert(msg);
//                loadNatProjectData();
//            }
//            else if (response.message === "Exist") {
//                showDangerAlert("Nature of Project already exists.");
//            }
//            else {
//                var errorMessg = "";
//                if (response.errors) {
//                    for (var error in response.errors) {
//                        if (response.errors.hasOwnProperty(error)) {
//                            errorMessg += `${response.errors[error]}\n`;
//                        }
//                    }
//                }
//                showDangerAlert(errorMessg || response.message || "An error occurred while saving.");
//            }
//        },
//        error: function (xhr, status, error) {
//            Blockloaderhide();
//            showDangerAlert("An unexpected error occurred. Please refresh the page and try again.");
//        }
//    });
//}

//$('#natProjectModel').on('hidden.bs.modal', function () {
//    loadData(); // uncomment if you want full reload
//});

//function delNatProjectConfirm(recid, element) {
//    debugger;

//    if (!recid || recid <= 0) {
//        const rowEl = $(element).closest(".tabulator-row")[0];
//        const row = tableNatProject.getRow(rowEl);
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
//            url: '/BisProjectTrac/DeleteNatProjectAsync',
//            type: 'POST',
//            data: { id: recid },
//            success: function (data) {
//                if (data.success == true) {
//                    showSuccessNewAlert("Nature of Project Deleted successfully.");
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
//        loadNatProjectData();
//    });
//}

//function clearForm() {
//    // Clear all input fields
//    document.querySelectorAll('.form-control').forEach(function (input) {
//        if (input.tagName === 'INPUT') {
//            if (input.type === 'hidden' || input.readOnly) {
//                // Skip hidden or readonly inputs
//                return;
//            }
//            input.value = ''; // Clear input value
//        } else if (input.tagName === 'SELECT') {
//            input.selectedIndex = 0; // Reset dropdown to first option
//        }
//    });

//    // Clear error messages if needed
//    document.querySelectorAll('.text-danger').forEach(function (error) {
//        error.textContent = '';
//    });
//}


