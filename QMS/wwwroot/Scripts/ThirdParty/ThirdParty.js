let filterStartDate = moment().startOf('month').format('YYYY-MM-DD');
let filterEndDate = moment().endOf('month').format('YYYY-MM-DD');
var table = '';
let vendorOptions = {};
var tabledata = [];

$(document).ready(function () {

    $('#dateRangeText').text(
        moment(filterStartDate).format('MMMM D, YYYY') + ' - ' + moment(filterEndDate).format('MMMM D, YYYY')
    );

    // Initialize Litepicker and store reference
    const picker = new Litepicker({
        element: document.getElementById('customDateTrigger'),
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
                filterStartDate = start.format('YYYY-MM-DD');
                filterEndDate = end.format('YYYY-MM-DD');
                $('#dateRangeText').text(`${start.format('MMMM D, YYYY')} - ${end.format('MMMM D, YYYY')}`);
                loadThirdInsData();
            });

            picker.on('clear', () => {
                filterStartDate = "";
                filterEndDate = "";
                $('#dateRangeText').text("Select Date Range");
                loadThirdInsData();
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

    // 🔑 Ensure calendar opens on click
    $('#customDateTrigger').on('click', function () {
        picker.show();
    });

    $('#upload-button').on('click', async function () {
        var expectedColumns = [
            'PC','Inspection Date', 'Customer/Project Name', 'Inspector Name', 'Product Code', 'Product Description', 'LOT QTY(No)',
            'Project Value(Mn).', 'TPI Duration(DAYS)', 'Location(Waluj Lab/Vendor Location)', 'Mode Of Inspection(Physical/Online)',
            'Cleared in first attempt', 'Remark', 'Actions plan (If any issue)', 'MOM performed date'
        ];

        var url = '/ThirdPartyInspection/UploadTPIExcel';
        handleImportExcelFile(url, expectedColumns);
    });

    loadThirdInsData();
});

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

function loadThirdInsData() {
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

        // Nested AJAX call to get actual data after vendorOptions is populated
        $.ajax({
            url: '/ThirdPartyInspection/GetAll',
            type: 'GET',
            dataType: 'json',
            data: {
                startDate: filterStartDate,
                endDate: filterEndDate
            },
        })
            .done(function (data) {
                const safeData = Array.isArray(data) ? data : [];
                if (safeData.length) {
                    OnTabThirdInsGridLoad(safeData);
                } else {
                    showDangerAlert('No data available to load.');
                    OnTabThirdInsGridLoad([]);
                }
            })
            .fail(function (xhr, status, error) {
                console.error('Error retrieving data:', status, error, xhr.responseText);
                showDangerAlert('Error retrieving data: ' + (error || status));
                OnTabThirdInsGridLoad([]);
            })
            .always(function () {
                Blockloaderhide();
            });

    });
}


//Tabulator.extendModule("edit", "editors", {
//    autocomplete_ajax: function (cell, onRendered, success, cancel, editorParams) {
//        const input = document.createElement("input");
//        input.setAttribute("type", "text");
//        input.style.width = "100%";
//        input.value = cell.getValue() || "";

//        let dropdown = null;

//        function removeDropdown() {
//            if (dropdown && dropdown.parentNode && document.body.contains(dropdown)) {
//                dropdown.parentNode.removeChild(dropdown);
//            }
//            dropdown = null;
//        }

//        function fetchSuggestions(query) {
//            $.ajax({
//                url: '/PDITracker/GetCodeSearch',
//                type: 'GET',
//                data: { search: query },
//                success: function (data) {
//                    removeDropdown(); // clear old

//                    dropdown = document.createElement("div");
//                    dropdown.className = "autocomplete-dropdown";

//                    data.forEach(item => {
//                        const option = document.createElement("div");
//                        option.textContent = item.oldPart_No;
//                        option.className = "autocomplete-option";

//                        option.addEventListener("mousedown", function (e) {
//                            e.stopPropagation();
//                            success(item.oldPart_No);

//                            const row = cell.getRow();
//                            const data = row.getData();
//                            //row.update({ ProductDescription: item.description });
//                            row.update({ ProductDescription: item.description }).then(() => {
//                                // Save full row (with updated description)
//                                saveEditedRow({ ...data, ProductCode: item.oldPart_No, ProductDescription: item.description });
//                            });

//                            removeDropdown();
//                        });

//                        dropdown.appendChild(option);
//                    });

//                    document.body.appendChild(dropdown);
//                    const rect = input.getBoundingClientRect();
//                    dropdown.style.top = (window.scrollY + rect.bottom) + "px";
//                    dropdown.style.left = (window.scrollX + rect.left) + "px";
//                    dropdown.style.width = rect.width + "px";
//                },
//                error: function () {
//                    console.error("Failed to fetch suggestions.");
//                }
//            });
//        }

//        input.addEventListener("input", function () {
//            const val = input.value;
//            if (val.length >= 4) {
//                fetchSuggestions(val);
//            } else {
//                removeDropdown();
//            }
//        });

//        input.addEventListener("blur", function () {
//            setTimeout(() => {
//                removeDropdown();
//                success(input.value);
//            }, 150); // delay to allow click
//        });

//        return input;
//    }
//});


//<style>
///* minimal styles for tags & dropdown */
//.tab-multi-ac { display:flex; flex-wrap:wrap; gap:.35rem; align-items:center; min-height:30px; padding:4px; }
//.tab-multi-ac input { border:none; outline:none; flex:1 0 120px; min-width:100px; }
//.tab-tag { display:inline-flex; align-items:center; gap:.35rem; padding:2px 8px; border-radius:12px; background:#eef1f6; font-size:12px; }
//.tab-tag .tab-x { cursor:pointer; font-weight:700; }
//.autocomplete-dropdown { position:absolute; z-index:99999; max-height:240px; overflow:auto; border:1px solid #ddd; background:#fff; box-shadow:0 6px 18px rgba(0,0,0,.08); }
//.autocomplete-option { padding:6px 10px; cursor:pointer; white-space:nowrap; }
//.autocomplete-option:hover, .autocomplete-option.active { background:#f2f6ff; }
//</style>


// Utility


// make sure this runs before OnTabThirdInsGridLoad
// --- Put this near the top of your JS (before OnTabThirdInsGridLoad) ---
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

    // Kept for other columns if needed; NOT used for ProductCode now
    global.tagsFormatterPairs = function (cell, params) {
        const joinWith = params?.joinWith ?? ", ";
        const pairSeparator = params?.pairSeparator ?? " — ";
        const pairJoinWith = params?.pairJoinWith ?? " | ";
        const descFieldParam = params?.descField;

        const raw = cell.getValue();
        const codes = Array.isArray(raw)
            ? raw
            : String(raw || "").split(joinWith).map(s => s.trim()).filter(Boolean);

        const row = cell.getRow().getData();
        let descText = "";
        if (Array.isArray(descFieldParam)) {
            descText = descFieldParam.map(f => row?.[f]).find(Boolean) || "";
        } else if (typeof descFieldParam === "string") {
            descText = row?.[descFieldParam] || "";
        } else {
            descText = row?.ProductDescription || row?.ProdDesc || "";
        }

        const map = parsePairs(descText, pairSeparator, pairJoinWith);

        const labels = codes.map(code => {
            const d = map.get(String(code));
            return d ? `${code}${pairSeparator}${d}` : String(code);
        });

        return labels.map(lbl => `<span class="tab-tag">${escapeHTML(lbl)}</span>`).join(" ");
    };
})(window);

// Optional minimal chip styling
/* .tab-tag{display:inline-block;padding:2px 8px;margin:2px 4px 0 0;border:1px solid #ddd;border-radius:9999px;font-size:12px;background:#f7f7f7;line-height:20px;} */



Tabulator.extendModule("edit", "editors", {
    autocomplete_ajax_multi: function (cell, onRendered, success, cancel, editorParams) {
        // ---------- utilities ----------
        const _escapeHtml = s => String(s ?? "").replace(/[&<>"']/g,
            m => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': "&quot;", "'": '&#39;' }[m]));

        function _firstField(obj, candidates) {
            for (const k of candidates) {
                if (obj && Object.prototype.hasOwnProperty.call(obj, k) && obj[k] != null) return obj[k];
            }
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
                    // Only desc present (legacy) – can't map to code, ignore for mapping
                }
            }
            return map;
        }

        // ---------- config ----------
        const cfg = Object.assign({
            url: '/PDITracker/GetCodeSearch',
            queryParam: 'search',
            minChars: 3,
            debounce: 250,

            valueField: 'oldPart_No',
            valueFieldCandidates: ['oldPart_No', 'code', 'Code', 'productCode', 'ProductCode'],
            labelField: 'oldPart_No',
            labelFieldCandidates: ['label', 'name', 'text', 'oldPart_No', 'productCode', 'ProductCode'],
            descItemField: 'description',
            descItemFieldCandidates: ['description', 'desc', 'productDescription', 'ProductDescription', 'prodDesc', 'ProdDesc'],

            // Linked row description field(s) to update
            // IMPORTANT: final behaviour = ONLY DESCRIPTIONS in ProdDesc
            descField: ['ProdDesc'],   // will update only ProdDesc
            descAsArray: false,        // store desc as a single string, not array
            descJoinWith: ' | ',       // join multiple descs with pipe

            // Store mode:
            //  - 'desc'  -> only descriptions in descField
            //  - 'pair'  -> "CODE — DESC" in descField
            descFormat: 'desc',        // <<< KEY: we want ONLY DESC

            descPairSeparator: ' — ',  // still used if descFormat='pair'
            descPairJoinWith: ' | ',

            updateRowOnPick: true,

            asArray: true,             // ProductCode stored as array of codes
            joinWith: ', ',
            maxSelect: 0,
            allowFreeText: false
        }, editorParams || {});

        const descFields = Array.isArray(cfg.descField) ? cfg.descField : (cfg.descField ? [cfg.descField] : []);

        // ---------- UI ----------
        const wrap = document.createElement('div');
        wrap.className = 'tab-multi-ac';
        wrap.style.width = "100%";

        const input = document.createElement('input');
        input.type = 'text';
        input.autocomplete = 'off';
        wrap.appendChild(input);

        let dropdown = null, activeIndex = -1, pendingXHR = null, lastQuery = '';
        let selected = [];
        const selectedSet = new Set();

        function removeDropdown() {
            if (dropdown && dropdown.parentNode) dropdown.parentNode.removeChild(dropdown);
            dropdown = null;
            activeIndex = -1;
        }

        function positionDropdown() {
            if (!dropdown) return;
            const r = wrap.getBoundingClientRect();
            dropdown.style.top = (r.bottom) + "px";
            dropdown.style.left = (r.left) + "px";
            dropdown.style.minWidth = r.width + "px";
        }

        function cancelPending() {
            if (pendingXHR && pendingXHR.readyState !== 4) {
                try { pendingXHR.abort(); } catch (e) { }
            }
            pendingXHR = null;
        }

        const debouncedFetch = (() => {
            let t = null;
            return (q) => {
                clearTimeout(t);
                t = setTimeout(() => fetchSuggestions(q), cfg.debounce);
            };
        })();

        function normalizeItem(raw) {
            let code = _firstField(raw, [cfg.valueField, ...cfg.valueFieldCandidates]);
            let desc = _firstField(raw, [cfg.descItemField, ...cfg.descItemFieldCandidates]);
            let label = _firstField(raw, [cfg.labelField, ...cfg.labelFieldCandidates]);

            if (!desc && label) {
                const parsed = _parseCodeDesc(label);
                if (!code && parsed.code) code = parsed.code;
                if (parsed.desc) desc = parsed.desc;
            }

            if (!label) label = desc ? `${code} ${cfg.descPairSeparator}${desc}` : (code ?? '');

            return {
                code: String(code || '').trim(),
                desc: desc ? String(desc).trim() : undefined,
                label: String(label || '').trim()
            };
        }

        function fetchSuggestions(q) {
            if (q === lastQuery) return;
            lastQuery = q;
            cancelPending();
            if (q.length < cfg.minChars) {
                removeDropdown();
                return;
            }

            pendingXHR = $.ajax({
                url: cfg.url,
                type: 'GET',
                data: { [cfg.queryParam]: q }
            }).done(function (data) {
                renderDropdown(Array.isArray(data) ? data : []);
            }).always(function () {
                pendingXHR = null;
            });
        }

        function renderDropdown(items) {
            removeDropdown();

            dropdown = document.createElement('div');
            dropdown.className = 'autocomplete-dropdown';

            const normalized = items.map(normalizeItem).filter(it => it.code);
            const filtered = normalized.filter(it => !selectedSet.has(it.code));

            if (filtered.length === 0) {
                const empty = document.createElement('div');
                empty.className = 'autocomplete-option';
                empty.style.opacity = .6;
                empty.textContent = 'No matches';
                dropdown.appendChild(empty);
            } else {
                filtered.forEach((it) => {
                    const opt = document.createElement('div');
                    opt.className = 'autocomplete-option';
                    opt.innerHTML = _escapeHtml(it.label);
                    opt.addEventListener('mousedown', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        addTag(it);
                        flushTags();
                        input.focus();
                        debouncedFetch(input.value.trim());
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

            const labels = selected.map(it =>
                it.desc
                    ? `${it.code} ${cfg.descPairSeparator}${it.desc}`
                    : (it.label || it.code)
            );

            labels.forEach((label, i) => {
                const tag = document.createElement('span');
                tag.className = 'tab-tag';
                tag.innerHTML = _escapeHtml(label) + ' <span class="tab-x" title="Remove">×</span>';
                tag.querySelector('.tab-x').addEventListener('mousedown', e => {
                    e.preventDefault();
                    e.stopPropagation();
                    removeItem(selected[i].code);
                });
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
            if (cfg.updateRowOnPick) syncLinkedDesc();
            input.value = '';
        }

        function removeItem(code) {
            selected = selected.filter(it => it.code !== code);
            selectedSet.delete(code);
            flushTags();
            if (cfg.updateRowOnPick) syncLinkedDesc();
        }

        function codesArray() {
            return selected.map(it => it.code);
        }

        function buildDescOutput() {
            if (cfg.descFormat === 'pair') {
                const parts = selected.map(it => {
                    if (it.desc) return `${it.code}${cfg.descPairSeparator}${it.desc}`;
                    return it.code;
                });
                return parts.join(cfg.descPairJoinWith);
            } else { // 'desc' → keep old desc + add new ones
                const rowData = cell.getRow().getData();

                // get existing description text from the row (first descField that has value)
                const existingRaw =
                    (descFields
                        .map(f => rowData && rowData[f])
                        .find(v => v != null && v !== "")) || "";

                const existingList = existingRaw
                    ? String(existingRaw)
                        .split(cfg.descJoinWith)
                        .map(s => s.trim())
                        .filter(Boolean)
                    : [];

                // new descs coming from current selected items (from API)
                const newDescs = selected
                    .map(it => it.desc)
                    .filter(d => d && !existingList.includes(d));

                const allDescs = existingList.concat(newDescs);

                return cfg.descAsArray ? allDescs : allDescs.join(cfg.descJoinWith);
            }
        }

        function syncLinkedDesc() {
            if (!descFields.length) return;
            const row = cell.getRow();
            const val = buildDescOutput();
            const patch = {};
            descFields.forEach(f => patch[f] = val);
            row.update(patch);
        }

        function commit() {
            removeDropdown();
            const val = cfg.asArray ? codesArray() : codesArray().join(cfg.joinWith);
            if (descFields.length) syncLinkedDesc();
            success(val);
        }

        // ---------- init from current cell value ----------
        (function initFromValue() {
            const raw = cell.getValue();
            const initialCodes = Array.isArray(raw)
                ? raw
                : (raw ? String(raw).split(cfg.joinWith).map(s => s.trim()).filter(Boolean) : []);

            const rowData = cell.getRow().getData();
            const firstDescText = descFields.map(f => rowData && rowData[f]).find(Boolean);

            // If previously stored as CODE — DESC | CODE — DESC, this will map.
            // If only DESC is stored, map will be empty -> tags will still show code.
            const knownMap = parsePairsFromText(firstDescText, cfg.descPairSeparator, cfg.descPairJoinWith);

            initialCodes.forEach(code => {
                const desc = knownMap.get(code);
                addTag({
                    code,
                    desc,
                    label: desc ? `${code} ${cfg.descPairSeparator}${desc}` : code
                }, false);
            });

            flushTags();
            if (cfg.updateRowOnPick) syncLinkedDesc();
        })();

        // ---------- events ----------
        input.addEventListener('keydown', function (e) {
            e.stopPropagation();

            const opts = dropdown ? dropdown.querySelectorAll('.autocomplete-option') : [];
            const max = opts.length - 1;

            if (e.key === 'ArrowDown') {
                e.preventDefault();
                if (!dropdown) {
                    debouncedFetch(input.value.trim());
                    return;
                }
                activeIndex = Math.min(max, activeIndex + 1);
            } else if (e.key === 'ArrowUp') {
                e.preventDefault();
                activeIndex = Math.max(0, activeIndex - 1);
            } else if (e.key === 'PageDown') {
                e.preventDefault();
                activeIndex = Math.min(max, (activeIndex < 0 ? 0 : activeIndex) + 10);
            } else if (e.key === 'PageUp') {
                e.preventDefault();
                activeIndex = Math.max(0, (activeIndex < 0 ? 0 : activeIndex) - 10);
            } else if (e.key === 'Home') {
                e.preventDefault();
                activeIndex = 0;
            } else if (e.key === 'End') {
                e.preventDefault();
                activeIndex = max;
            } else if (e.key === 'Enter') {
                e.preventDefault();
                if (dropdown && activeIndex >= 0 && opts[activeIndex]) {
                    opts[activeIndex].dispatchEvent(
                        new MouseEvent('mousedown', { bubbles: true })
                    );
                } else if (cfg.allowFreeText && input.value.trim() !== '') {
                    const v = input.value.trim();
                    addTag({ code: v, desc: v, label: v });
                    flushTags();
                } else {
                    commit();
                }
            } else if (e.key === 'Backspace' && input.value === '') {
                const last = selected[selected.length - 1];
                if (last) removeItem(last.code);
            } else if (e.key === 'Escape') {
                e.preventDefault();
                removeDropdown();
                return;
            }

            if (dropdown) {
                opts.forEach((el, i) => el.classList.toggle('active', i === activeIndex));
                if (activeIndex >= 0 && opts[activeIndex]) {
                    opts[activeIndex].scrollIntoView({ block: 'nearest' });
                }
            }
        });

        input.addEventListener('input', () => debouncedFetch(input.value.trim()));

        const onDocClick = (e) => {
            if (!wrap.contains(e.target) && !(dropdown && dropdown.contains(e.target))) commit();
        };

        document.addEventListener('mousedown', onDocClick, true);
        window.addEventListener('resize', positionDropdown, true);

        onRendered(() => {
            input.focus({ preventScroll: true });
            positionDropdown();
        });

        function destroy() {
            if (pendingXHR && pendingXHR.readyState !== 4) {
                try { pendingXHR.abort(); } catch (e) { }
            }
            removeDropdown();
            document.removeEventListener('mousedown', onDocClick, true);
            window.removeEventListener('resize', positionDropdown, true);
        }

        const _success = success;
        const _cancel = cancel;

        success = (v) => { destroy(); _success(v); };
        cancel = () => { destroy(); _cancel(); };

        return wrap;
    }
});

function vendorEditor(cell, onRendered, success, cancel, editorParams) {
    const currentValue = cell.getValue() || "";

    // Create a <select> element for Select2
    const select = document.createElement("select");
    select.style.width = "100%";

    // Build options from editorParams.values (your vendorOptions dictionary)
    const values = editorParams && editorParams.values ? editorParams.values : {};

    // Optional empty option
    const emptyOpt = document.createElement("option");
    emptyOpt.value = "";
    emptyOpt.text = "";
    select.appendChild(emptyOpt);

    Object.keys(values).forEach(key => {
        const opt = document.createElement("option");
        opt.value = key;
        opt.text = values[key];
        select.appendChild(opt);
    });

    // Set initial value (if any)
    if (currentValue) {
        // If value does not exist in the list (manual text from before), add it
        if (!values[currentValue]) {
            const opt = document.createElement("option");
            opt.value = currentValue;
            opt.text = currentValue;
            select.appendChild(opt);
        }
        select.value = currentValue;
    }

    onRendered(function () {
        // Initialize Select2 with tags:true so user can type new values
        $(select).select2({
            width: "resolve",
            tags: true,              // 👈 allows manual input
            placeholder: "",
            dropdownAutoWidth: true
        });

        // Ensure Select2 shows current value
        $(select).val(currentValue).trigger("change.select2");

        // Commit value when user closes Select2
        $(select).on("select2:close", function () {
            const val = $(this).val();
            success(val);           // send final value back to Tabulator
        });
    });

    // Handle ESC -> cancel edit
    select.addEventListener("keydown", function (e) {
        if (e.key === "Escape") {
            e.stopPropagation();
            cancel();
        }
        if (e.key === "Enter") {
            e.preventDefault();
        }
    });

    return select;
}

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
    //if (def.field === "BatchCodeVendor" && window.batchCodeOptions) maps.push(window.batchCodeOptions);
    if (def.field === "Location" && window.vendorOptions) maps.push(window.vendorOptions);

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



function OnTabThirdInsGridLoad(response) {
    Blockloadershow();
    let tabledata = [];

    // Utility function to format date to dd/MM/yyyy
    function formatDate(value) {
        return value ? new Date(value).toLocaleDateString("en-GB") : "";
    }

    // Prepare data
    if (response.length > 0) {
        $.each(response, function (index, item) {
            tabledata.push({
                Sr_No: index + 1,
                PC: item.pc,
                InspectionID: item.inspectionID,
                InspectionDate: formatDate(item.inspectionDate),
                ProjectName: item.projectName || "",
                InspName: item.inspName || "",
                ProductCode: item.productCode || "",
                ProdDesc: item.prodDesc || "",
                LOTQty: item.lotQty,
                ProjectValue: item.projectValue || "",
                Tpi_Duration: item.tpi_Duration || "",
                Location: item.location || "",
                Mode: item.mode || "",
                FirstAttempt: item.firstAttempt || "",
                Remark: item.remark || "",
                ActionPlan: item.actionPlan || "",
                MOMDate: formatDate(item.momDate),
                Attachment: item.attachment || "",
                UpdatedDate: formatDate(item.updatedDate),
                CreatedDate: formatDate(item.createdDate),
                CreatedBy: item.createdBy || "",
                UpdatedBy: item.updatedBy || ""
            });
        });
    }

    // Define columns
    const columns = [
        {
            title: "Action", field: "Action", frozen: true, hozAlign: "center", headerSort: false,
            width: 90,
            headerMenu: headerMenu,
            formatter: function (cell) {
                const rowData = cell.getRow().getData();
                return `<i onclick="delConfirm(${rowData.InspectionID},this)" class="fas fa-trash-alt text-danger" title="Delete" style="cursor:pointer;"></i>`;
            }
        },
        { title: "S.No", field: "Sr_No", frozen: true, hozAlign: "center", headerSort: false, headerMenu: headerMenu, width: 80 },

        editableColumn("PC", "PC", "input", "center", "input", {}, {}, 120),
        editableColumn("Inspection Date", "InspectionDate", "date", "center", "input", {}, {}, 120),
        editableColumn("Customer/Project Name", "ProjectName", "input", "left", "input", {}, {} ),
        editableColumn("TPI Agency/Inspector Name", "InspName", "input", "left", "input", {}, {} ),
        //editableColumn("Product Code", "ProductCode", "autocomplete_ajax"),
        {
            title: "Product Codes",
            field: "ProductCode",
            editor: "autocomplete_ajax_multi",
            headerSort: false,
            headerMenu: headerMenu,
            headerFilter: "input",

            // Show ONLY CODES as chips
            formatter: function (cell) {
                const raw = cell.getValue();
                const codes = Array.isArray(raw)
                    ? raw
                    : String(raw || "").split(',').map(s => s.trim()).filter(Boolean);

                return codes.join(", ");
            },

            editorParams: {
                url: "/PDITracker/GetCodeSearch",
                queryParam: "search",
                minChars: 4,

                valueField: "oldPart_No",
                descItemField: "description",
                labelField: "oldPart_No",

                // IMPORTANT: only ProdDesc will be updated with DESC ONLY
                descField: ["ProdDesc"],
                descFormat: "desc",      // ONLY descriptions
                descAsArray: false,
                descJoinWith: " | ",

                asArray: true,           // ProductCode stored as array of codes
                joinWith: ", ",
                maxSelect: 0
            },

            widthGrow: 2
        },
        /*editableColumn("Product Description", "ProdDesc", "input"),*/
        { title: "Product Description", field: "ProdDesc", widthGrow: 3, headerSort: false, headerMenu: headerMenu, headerFilter: "input" },

        editableColumn("LOT QTY(No's)", "LOTQty", "input", "center", "input", {}, 110),
        editableColumn("Project Value(Mn).", "ProjectValue", "input", "left", "input", {}, {}),
        editableColumn("TPI Duration(DAYS)", "Tpi_Duration", "input", "center", "input", {}, {}),
        //editableColumn("Location(Waluj Lab/Vendor Location)", "Location", "input", "left", "input", {}, {}),
        //editableColumn(
        //    "Location(Waluj Lab/Vendor Location)",
        //    "Location",
        //    "list",                        // editor type
        //    "center",                      // hoz align
        //    "list",                        // header filter type
        //    { values: { "Waluj Lab": "Waluj Lab", "Vendor Location": "Vendor Location" } },
        //    { values: { "Waluj Lab": "Waluj Lab", "Vendor Location": "Vendor Location" }, allowEmpty: true },
        //    null
        //),

        {
            title: "Location(Waluj Lab/Vendor Location)",
            field: "Location",
            headerSort: false,
            headerMenu: headerMenu,
            headerFilter: "input",
            hozAlign: "center",
            headerHozAlign: "center",
            width: 130,

            // Display: if value is a key in vendorOptions, show name; else show raw value
            formatter: function (cell) {
                const val = cell.getValue();
                return vendorOptions[val] || val || "";
            },

            // Custom Select2 editor that allows manual typing
            editor: vendorEditor,
            editorParams: {
                values: vendorOptions      // pass your dictionary here
            }
        },
        //editableColumn("Mode Of Inspection(Physical/Online)", "Mode", "input", "center", "input", {}, {}),
        editableColumn(
            "Mode Of Inspection(Physical/Online)",
            "Mode",
            "list",                        // editor type
            "center",                      // hoz align
            "list",                        // header filter type
            { values: { Physical: "Physical", Online: "Online" } },  // headerFilterParams
            { values: { Physical: "Physical", Online: "Online" } },  // editorParams
            null
        ),
        editableColumn("Cleared in first attempt", "FirstAttempt", "input", "left", "input", {}, {}),
        editableColumn("Remark", "Remark", "input", "left", "input", {}, {}),
        editableColumn("Actions plan (If any issue)", "ActionPlan", "input", "left", "input", {}, {}),
        editableColumn("MOM performed date", "MOMDate", "date", "center", "input", {}, {}, 120),
        editableColumn("Remark", "Remark", "input", "center", null, {}, {}, 180),
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
                    ? `<a href="/ThirdPartyTrac_Atta/${rowData.InspectionID}/${fileName}" target="_blank">${fileName}</a><br/>`
                    : '';

                return `
            ${fileDisplay}
            <input type="file" accept=".pdf,image/*" class="form-control-file pdi-upload" data-id="${cell.getRow().getData().InspectionID}" style="width:160px;" />
        `;
            },
            cellClick: function (e, cell) {
                // prevent Tabulator from swallowing the file input click
                e.stopPropagation();
            }
        },

        {
            title: "Created By", field: "CreatedBy",
            hozAlign: "center",
            headerSort: false,
            headerMenu: headerMenu,
            width: 100,
            visible: false
        },
        {
            title: "Created Date", field: "CreatedDate",
            hozAlign: "center",
            headerSort: false,
            headerMenu: headerMenu,
            width: 100,
            visible: false
        },
        {
            title: "Updated By", field: "UpdatedBy",
            hozAlign: "center",
            headerSort: false,
            headerMenu: headerMenu,
            width: 100,
            visible: false
        },
        {
            title: "Updated Date", field: "UpdatedDate",
            hozAlign: "center",
            headerSort: false,
            headerMenu: headerMenu,
            width: 100,
            visible: false
        }
    ];

    // Create or replace Tabulator table
    if (table) {
        table.replaceData(tabledata);
    } else {
        table = new Tabulator("#thirdInspection_Table", {
            data: tabledata,
            layout: "fitDataFill",
            movableColumns: true,
            pagination: "local",
            paginationSize: 10,
            paginationSizeSelector: [10, 50, 100, 500],
            paginationCounter: "rows",
            placeholder: "No data available",
            columns: columns
        });

        // Handle cell edit event
        table.on("cellEdited", function (cell) {
            debugger
            saveEditedRow(cell.getRow().getData());
        });
    }

    //$("#addTPIButton").on("click", function () {
    //    const newRow = {
    //        InspectionID: 0,
    //        Sr_No: table.getDataCount() + 1,
    //        InspectionDate: "",
    //        ProjectName: "",
    //        InspName: "",
    //        ProductCode: "",
    //        ProdDesc: "",
    //        LOTQty: "",
    //        ProjectValue: "",
    //        Tpi_Duration: "",
    //        Location: "",
    //        Mode: "",
    //        FirstAttempt: "",
    //        Remark: "",
    //        ActionPlan: "",
    //        MOMDate: "",
    //        Attachment: "",
    //        Document_No: "",
    //        Revision_No: "",
    //        Effective_Date: "",
    //        Revision_Date: "",
    //    };
    //    table.addRow(newRow, false); // false = add to bottom
    //});

    (function bindAddButtonOnce() {
        var $btn = $("#addTPIButton");
        $btn.attr("type", "button");                       // avoid form submit duplicates
        $btn.off("click.addrow").on("click.addrow", function (e) {
            e.preventDefault(); e.stopPropagation();
            if ($btn.data("busy")) return;                 // guard double-clicks
            $btn.data("busy", true).prop("disabled", true);

            try {
                const newRow = {
                    InspectionID: 0,
                    Sr_No: table.getDataCount() + 1,
                    InspectionDate: "",
                    ProjectName: "",
                    InspName: "",
                    ProductCode: "",
                    ProdDesc: "",
                    LOTQty: "",
                    ProjectValue: "",
                    Tpi_Duration: "",
                    Location: "",
                    Mode: "",
                    FirstAttempt: "",
                    Remark: "",
                    ActionPlan: "",
                    MOMDate: "",
                    Attachment: "",
                    Document_No: "",
                    Revision_No: "",
                    Effective_Date: "",
                    Revision_Date: "",
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

    //function getDisplayValue(cell) {
    //    if (!cell) return "";
    //    const v = cell.getValue();
    //    return v == null ? "" : String(v);
    //}

    // helper to renumber Sr_No after inserts/deletes/sorts (if needed)
    document.getElementById("exportThirdPartyInsButton").addEventListener("click", async function () {
        const EXPORT_SCOPE = "active"; // "active" | "selected" | "all"

        if (!window.table) {
            console.error("Tabulator 'table' not found.");
            return;
        }

        // ===== 1) BUILD COLUMNS FROM TABULATOR (VISIBLE ONLY) =====
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

        const TOTAL_COLS = excelCols.length;
        if (!TOTAL_COLS) {
            alert("No visible columns to export.");
            return;
        }

        // find product code / description / remark columns BY TITLE
        const PRODUCT_CODE_TITLES = new Set(["Product Codes", "Product Code"]);
        const PRODUCT_DESC_TITLES = new Set(["Product Description", "Product Desc"]);
        const REMARK_TITLES = new Set(["Remark", "Remarks"]);

        // LOT Qty title set – include exact header from Excel
        const LOT_QTY_TITLES = new Set([
            "LOT QTY(No's)",   // exact from your Excel
            "LOT Qty",
            "Lot Qty",
            "LOT Quantity",
            "Lot Quantity",
            "LOTQty",
            "LotQty"
        ]);

        let codeColKey = null, descColKey = null;
        let codeColIndex = null, descColIndex = null, remarkColIndex = null; // 1-based for Excel

        let lotQtyColKey = null;
        let lotQtyColIndex = null; // 1-based

        excelCols.forEach((col, idx) => {
            const title = (col.label || "").trim();
            const i = idx + 1;

            if (PRODUCT_CODE_TITLES.has(title)) {
                codeColKey = col.key;
                codeColIndex = i;
            }
            if (PRODUCT_DESC_TITLES.has(title)) {
                descColKey = col.key;
                descColIndex = i;
            }
            if (REMARK_TITLES.has(title)) {
                // last "Remark" column – this matches col 17 in your Excel
                remarkColIndex = i;
            }
            if (LOT_QTY_TITLES.has(title)) {
                lotQtyColKey = col.key;
                lotQtyColIndex = i;
            }
        });

        if (!codeColKey || !descColKey) {
            console.warn("Product Codes / Product Description columns not found by title.");
        }

        // columns that must be LEFT-aligned (data rows)
        const leftAlignCols = new Set(
            [codeColIndex, descColIndex, remarkColIndex].filter(Boolean)
        );

        // ===== 2) DOC DETAILS (last two columns) =====
        const docDetails = [
            ["Document No", "WCIB/LS/QA/R/005"],
            ["Effective Date", "01/10/2022"],
            ["Revision No", "0"],
            ["Revision Date", "01/10/2022"],
            ["Page No", "1 of 1"]
        ];

        // ===== 3) Layout constants =====
        const HEADER_TOP = 1;
        const HEADER_BOTTOM = 5;
        const GRID_HEADER_ROW = HEADER_BOTTOM + 1; // row 6
        const TITLE_TEXT = "THIRD PARTY INSPECTION TRACKER";

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
            cell.border = {
                top: { style },
                bottom: { style },
                left: { style },
                right: { style }
            };
        }

        function outlineRange(ws, r1, c1, r2, c2, style = "thin") {
            for (let c = c1; c <= c2; c++) {
                const top = ws.getCell(r1, c);
                const bottom = ws.getCell(r2, c);
                top.border = { ...top.border, top: { style } };
                bottom.border = { ...bottom.border, bottom: { style } };
            }
            for (let r = r1; r <= r2; r++) {
                const left = ws.getCell(r, c1);
                const right = ws.getCell(r, c2);
                left.border = { ...left.border, left: { style } };
                right.border = { ...right.border, right: { style } };
            }
        }

        // Product Codes – split by <br>, newline, or ", "
        function splitCodes(displayValue) {
            if (!displayValue) return [];
            let txt = String(displayValue)
                .replace(/<br\s*\/?>/gi, "\n")
                .replace(/\r\n/g, "\n");
            txt = txt.replace(/\s*\|\s*/g, "\n");  // if you ever use " | "
            return txt
                .split(/\n|,\s*/g)
                .map(s => s.trim())
                .filter(Boolean);
        }

        // Product Description – split by " | " or newline (keep commas)
        function splitDescs(displayValue) {
            if (!displayValue) return [];
            let txt = String(displayValue)
                .replace(/<br\s*\/?>/gi, "\n")
                .replace(/\r\n/g, "\n");
            txt = txt.replace(/\s*\|\s*/g, "\n");
            return txt
                .split("\n")
                .map(s => s.trim())
                .filter(Boolean);
        }

        // LOT Qty – split "30,40,50" into ["30","40","50"]
        function splitLotQty(displayValue) {
            if (!displayValue) return [];
            let txt = String(displayValue)
                .replace(/<br\s*\/?>/gi, "\n")
                .replace(/\r\n/g, "\n");
            return txt
                .split(/[\n,]+/g)   // split by newline OR comma
                .map(s => s.trim())
                .filter(Boolean);
        }

        // ===== 5) Workbook / Sheet =====
        const wb = new ExcelJS.Workbook();
        const ws = wb.addWorksheet("Third Party Inspection Tracker", {
            properties: { defaultRowHeight: 15 },
            views: [{ state: "frozen", xSplit: 0, ySplit: GRID_HEADER_ROW }]
        });

        ws.columns = excelCols.map(c => ({ key: c.key, width: c.width }));

        for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) {
            ws.getRow(r).height = 18;
        }

        ws.pageSetup = {
            orientation: "landscape",
            fitToPage: true,
            fitToWidth: 1,
            fitToHeight: 0,
            margins: {
                left: 0.3,
                right: 0.3,
                top: 0.5,
                bottom: 0.5,
                header: 0.2,
                footer: 0.2
            },
            printTitlesRow: `${HEADER_TOP}:${GRID_HEADER_ROW}`
        };

        // ===== 6) Header band fill (rows 1–5) =====
        for (let r = HEADER_TOP; r <= HEADER_BOTTOM; r++) {
            for (let c = 1; c <= TOTAL_COLS; c++) {
                ws.getCell(r, c).fill = {
                    type: "pattern",
                    pattern: "solid",
                    fgColor: { argb: "FFF7F7F7" }
                };
            }
        }

        // ===== 7) Logo block =====
        const LOGO_WIDTH_PX = 100, LOGO_HEIGHT_PX = 100;
        const COL_PX = (c) => ((ws.getColumn(c).width || 8) * 7);
        const ROW_PX = (r) => ((ws.getRow(r).height || 15) * (96 / 72));

        let rectWpx = 0;
        for (let c = LOGO_COL_START; c <= LOGO_COL_END; c++) rectWpx += COL_PX(c);
        let rectHpx = 0;
        for (let r = LOGO_ROW_START; r <= LOGO_ROW_END; r++) rectHpx += ROW_PX(r);

        const avgColPx = rectWpx / (LOGO_COL_END - LOGO_COL_START + 1);
        const avgRowPx = rectHpx / (LOGO_ROW_END - LOGO_ROW_START + 1);

        const logoCols = LOGO_WIDTH_PX / avgColPx;
        const logoRows = LOGO_HEIGHT_PX / avgRowPx;

        const tlCol = (LOGO_COL_START - 1) +
            ((LOGO_COL_END - LOGO_COL_START + 1) - logoCols) / 2;
        const tlRow = (LOGO_ROW_START - 1) +
            ((LOGO_ROW_END - LOGO_ROW_START + 1) - logoRows) / 2;

        const logoUrl =
            window.LOGO_URL ||
            (window.APP_BASE && (window.APP_BASE + "images/wipro-logo.png"));

        if (logoUrl) {
            try {
                const base64 = await fetchAsBase64(logoUrl);
                const imgId = wb.addImage({ base64, extension: "png" });
                ws.addImage(imgId, {
                    tl: { col: tlCol, row: tlRow },
                    ext: { width: LOGO_WIDTH_PX, height: LOGO_HEIGHT_PX },
                    editAs: "oneCell"
                });
            } catch (e) {
                console.warn("Logo load failed:", e);
            }
        }
        outlineRange(ws, LOGO_ROW_START, LOGO_COL_START, LOGO_ROW_END, LOGO_COL_END, "thin");

        // ===== 8) Title =====
        ws.mergeCells(HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END);
        const titleCell = ws.getCell(HEADER_TOP, TITLE_COL_START);
        titleCell.value = TITLE_TEXT;
        titleCell.font = { bold: true, size: 18 };
        titleCell.alignment = { horizontal: "center", vertical: "middle" };
        outlineRange(ws, HEADER_TOP, TITLE_COL_START, HEADER_TOP + 2, TITLE_COL_END, "thin");

        // ===== 9) Document details (top-right) =====
        const detailsRowsEnd = HEADER_TOP + docDetails.length - 1;
        docDetails.forEach((pair, i) => {
            const r = HEADER_TOP + i;
            const labelCell = ws.getCell(r, DETAILS_LABEL_COL);
            const valueCell = ws.getCell(r, DETAILS_VALUE_COL);

            labelCell.value = pair[0];
            valueCell.value = pair[1];

            labelCell.font = { bold: true };
            [labelCell, valueCell].forEach(cell => {
                cell.alignment = {
                    vertical: "middle",
                    horizontal: "left",
                    wrapText: true
                };
                setBorder(cell);
            });
        });
        outlineRange(ws, HEADER_TOP, DETAILS_LABEL_COL, detailsRowsEnd, DETAILS_VALUE_COL, "thin");

        // ===== 10) Table header row =====
        while (ws.rowCount < GRID_HEADER_ROW - 1) {
            ws.addRow([]);
        }

        const headerTitles = excelCols.map(c => c.label);
        const headerRow = ws.addRow(headerTitles);
        headerRow.height = 22;
        headerRow.eachCell((cell) => {
            cell.font = { bold: true };
            cell.alignment = {
                horizontal: "center",
                vertical: "center",
                wrapText: true
            };
            cell.fill = {
                type: "pattern",
                pattern: "solid",
                fgColor: { argb: "FFD9E1F2" }
            };
            setBorder(cell);
        });

        // ===== 11) DATA ROWS WITH MERGED NON-PRODUCT COLUMNS =====
        let tabRows;
        switch (EXPORT_SCOPE) {
            case "selected":
                tabRows = table.getSelectedRows();
                break;
            case "all":
                tabRows = table.getRows();
                break;
            case "active":
            default:
                tabRows = table.getRows("active");
                break;
        }

        tabRows.forEach(row => {
            const baseValues = {};
            excelCols.forEach(col => {
                const cell = row.getCell(col.key);
                let val = getDisplayValue(cell);
                baseValues[col.key] = val;
            });

            const codeDisplay = codeColKey ? baseValues[codeColKey] : "";
            const descDisplay = descColKey ? baseValues[descColKey] : "";
            const lotQtyDisplay = lotQtyColKey ? baseValues[lotQtyColKey] : "";

            const codes = splitCodes(codeDisplay);
            const descs = splitDescs(descDisplay);
            const lotQtyArr = splitLotQty(lotQtyDisplay);

            const maxLines = Math.max(codes.length, descs.length, lotQtyArr.length, 1);
            const startRowIdx = ws.rowCount + 1;

            for (let i = 0; i < maxLines; i++) {
                const isFirst = (i === 0);

                const values = excelCols.map((col) => {
                    const key = col.key;

                    if (key === codeColKey) {
                        return codes[i] || "";
                    }
                    if (key === descColKey) {
                        return descs[i] || "";
                    }

                    // LOT Qty logic:
                    // - 0 values -> blank
                    // - 1 value  -> only in first row
                    // - >1 values -> 1:1 mapping, extra rows blank
                    if (key === lotQtyColKey) {
                        if (lotQtyArr.length === 0) {
                            return "";
                        }
                        if (lotQtyArr.length === 1) {
                            return i === 0 ? lotQtyArr[0] : "";
                        }
                        return lotQtyArr[i] || "";
                    }

                    // non-product columns: only in first row, blank later
                    return isFirst ? (baseValues[key] ?? "") : "";
                });

                const xRow = ws.addRow(values);

                // ALIGNMENT:
                // Product Codes / Product Description / last Remark -> left
                // Everything else (incl. LOT Qty) -> center
                xRow.eachCell((cell, colNumber) => {
                    const horizontal = leftAlignCols.has(colNumber) ? "left" : "center";
                    cell.alignment = {
                        vertical: "middle",
                        horizontal: horizontal,
                        wrapText: true
                    };
                    setBorder(cell);
                });
            }

            const endRowIdx = ws.rowCount;

            if (maxLines > 1) {
                // Merge every column EXCEPT Product Codes, Product Description & LOT Qty
                const mergeCols = [];
                for (let i = 1; i <= TOTAL_COLS; i++) {
                    if (i !== codeColIndex && i !== descColIndex && i !== lotQtyColIndex) {
                        mergeCols.push(i);
                    }
                }

                mergeCols.forEach(colIdx => {
                    ws.mergeCells(startRowIdx, colIdx, endRowIdx, colIdx);
                    const cell = ws.getCell(startRowIdx, colIdx);
                    const horizontal = leftAlignCols.has(colIdx) ? "left" : "center";
                    cell.alignment = {
                        vertical: "middle",
                        horizontal: horizontal,
                        wrapText: true
                    };
                });
            }
        });

        // ===== 12) DOWNLOAD =====
        const buffer = await wb.xlsx.writeBuffer();
        const blob = new Blob(
            [buffer],
            { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }
        );
        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = "Third Party Inspection Tracker.xlsx";
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


$('#thirdInspection_Table').on('change', '.pdi-upload', function () {
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
        url: "/ThirdPartyInspection/UploadAttachment",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false
    }).done(function (response) {
        if (response.success) {
            showSuccessNewAlert("File uploaded successfully.");
            table.updateData([{ Id: response.id, Attachment: response.fileName }]);
        } else {
            showDangerAlert(response.message || "Upload failed.");
        }
    }).fail(function () {
        showDangerAlert("Upload failed due to server error.");
    }).always(function () {
        Blockloaderhide();
    });
});


// Helper functions below — place these outside of OnTabGridLoad

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

//function delConfirm(recid, element) {

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
//        title: 'Confirm Deletion',
//        text: 'Are you sure to delete? It will not delete if this record is used in transactions.',
//        icon: 'fa fa-question-circle',
//        hide: false,
//        confirm: { confirm: true },
//        buttons: { closer: false, sticker: false },
//        history: { history: false }
//    })).get().on('pnotify.confirm', function () {
//        $.ajax({
//            url: '/ThirdPartyInspection/Delete',
//            type: 'POST',
//            data: { id: recid },
//            success: function (data) {
//                if (data.success) {
//                    showSuccessNewAlert("Deleted successfully.");
//                    setTimeout(() => window.location.reload(), 1500);
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
                            url: '/ThirdPartyInspection/Delete',
                            type: 'POST',
                            data: { id: recid }
                        }).done(function (data) {
                            if (data && data.success === true) {
                                showSuccessNewAlert("ThirdParty Inspection Detail deleted successfully.");
                                // remove row immediately
                                const rowEl = $(element).closest(".tabulator-row")[0];
                                const row = table.getRow(rowEl);
                                if (row) row.delete();
                                // reload using SAME filter (globals preserved)
                                loadThirdInsData();
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

Tabulator.extendModule("edit", "editors", {
    select2: function (cell, onRendered, success, cancel, editorParams) {
        const values = editorParams.values || {};
        const select = document.createElement("select");
        select.style.width = "100%";

        for (let val in values) {
            let option = document.createElement("option");
            option.value = val;
            option.text = values[val];
            if (val === cell.getValue()) option.selected = true;
            select.appendChild(option);
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

//function saveEditedRow(rowData) {
//    function emptyToNull(value) {
//        return value === "" ? null : value;
//    }

//    // Converts "dd/MM/yyyy" to "yyyy-MM-dd"
//    function toIsoDate(value) {
//        if (!value || value === "" || value === "Invalid Date") return null;

//        if (typeof value === "string" && value.includes("/")) {
//            const parts = value.split("/");
//            if (parts.length === 3) {
//                const [day, month, year] = parts;
//                return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
//            }
//        }

//        const parsed = new Date(value);
//        return isNaN(parsed.getTime()) ? null : parsed.toISOString().substring(0, 10);
//    }

//    const cleanedData = {
//        Id: rowData.InspectionID || 0,
//        InspectionDate: toIsoDate(rowData.InspectionDate),
//        ProjectName: rowData.ProjectName || "",
//        InspName: rowData.InspName || "",
//        ProductCode: rowData.ProductCode || "",
//        ProdDesc: rowData.ProdDesc || "",
//        LOTQty: rowData.LOTQty || null,
//        ProjectValue: rowData.ProjectValue || "",
//        Tpi_Duration: rowData.Tpi_Duration || "",
//        Location: rowData.Location || "",
//        Mode: rowData.Mode || "",
//        FirstAttempt: rowData.FirstAttempt || "",
//        Remark: rowData.Remark || "",
//        ActionPlan: rowData.ActionPlan || "",
//        MOMDate: toIsoDate(rowData.MOMDate),
//        Attahcment: rowData.Attahcment || null,
//        Document_No: rowData.Document_No || "",
//        Revision_No: rowData.Revision_No || "",
//        Effective_Date: toIsoDate(rowData.Effective_Date),
//        Revision_Date: toIsoDate(rowData.Revision_Date),
//    };

//    console.log("Cleaned data:", cleanedData);

//    const isNew = cleanedData.Id === 0;
//    const url = isNew ? '/ThirdPartyInspection/Create' : '/ThirdPartyInspection/Update';

//    $.ajax({
//        url: url,
//        type: 'POST',
//        data: JSON.stringify(cleanedData),
//        contentType: 'application/json',
//        success: function (data) {
//            if (data.success) {
//                if (isNew) {
//                    loadThirdInsData();
//                }
//                if (isNew && data.id) {
//                    rowData.Id = data.id;
//                }
//            } else {
//                var errorMessg = "";
//                if (data.errors) {
//                    for (var error in data.errors) {
//                        if (data.errors.hasOwnProperty(error)) {
//                            errorMessg += `${data.errors[error]}\n`;
//                        }
//                    }
//                }
//                showDangerAlert(errorMessg || data.message || "An error occurred while saving.");
//            }
//        },
//        error: function (xhr, status, error) {
//            showDangerAlert(xhr.responseText || "Error saving record.");
//        }
//    });
//}

function saveEditedRow(rowData) {
    debugger;
    var errorMsg = "";
    var fields = "";


    if (rowData.InspectionDate == '' || rowData.InspectionDate == null || rowData.InspectionDate == undefined) {
        fields += " - Inspection Date" + "<br>";
    }

    if (fields != "") {
        errorMsg = "Please fill following mandatory field(s):" + "<br><br>" + fields;
    }

    if (errorMsg != "") {
        showDangerAlert(errorMsg);
        return false;
    }

    // ----- helpers -----
    const toStrOrNull = (v) => {
        if (v === undefined || v === null) return null;
        const s = String(v).trim();
        return s === "" ? null : s;
    };

    // Accepts Date, "dd/MM/yyyy", "yyyy-MM-dd", etc. -> "yyyy-MM-dd" or null
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

    // Join array -> string, or pass through string, trimming empties.
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

    // ----- map to your model's property names & types -----
    // NOTE: Your model key is Id (mapped to column "InspectionID" in DB). Send "Id" in JSON.
    const cleanedData = {
        Id: Number(rowData.Id ?? rowData.InspectionID ?? 0) || 0,
        Pc: toStrOrNull(rowData.PC),
        InspectionDate: toIsoDate(rowData.InspectionDate),
        ProjectName: toStrOrNull(rowData.ProjectName),
        InspName: toStrOrNull(rowData.InspName),

        // Multi-select → comma-separated string for your string fields
        ProductCode: normalizeMultiToString(rowData.ProductCode, ", "),
        // Read desc from either ProdDesc or ProductDescription (editor updates both)
        ProdDesc: normalizeMultiToString(rowData.ProdDesc ?? rowData.ProductDescription, " | "),

        // These are strings in your model, so don't coerce to numbers
        LOTQty: toStrOrNull(rowData.LOTQty),
        ProjectValue: toStrOrNull(rowData.ProjectValue),

        Location: toStrOrNull(rowData.Location),
        Mode: toStrOrNull(rowData.Mode),
        FirstAttempt: toStrOrNull(rowData.FirstAttempt),
        Remark: toStrOrNull(rowData.Remark),
        ActionPlan: toStrOrNull(rowData.ActionPlan),

        MOMDate: toIsoDate(rowData.MOMDate),

        // Correct property name per model: Attachment
        Attachment: toStrOrNull(rowData.Attachment ?? rowData.Attahcment),

        Tpi_Duration: toStrOrNull(rowData.Tpi_Duration),
        Document_No: toStrOrNull(rowData.Document_No),
        Revision_No: toStrOrNull(rowData.Revision_No),
        Effective_Date: toIsoDate(rowData.Effective_Date),
        Revision_Date: toIsoDate(rowData.Revision_Date),
    };

    console.log("Cleaned data:", cleanedData);

    const isNew = cleanedData.Id === 0;
    const url = isNew ? "/ThirdPartyInspection/Create" : "/ThirdPartyInspection/Update";

    $.ajax({
        url,
        type: "POST",
        data: JSON.stringify(cleanedData),
        contentType: "application/json",
        success: function (data) {
            if (data && data.success) {
                if (isNew) {
                    loadThirdInsData?.();
                }
                if (isNew && (data.id || data.Id)) {
                    rowData.Id = data.id ?? data.Id;
                }
            } else {
                let errorMessg = "";
                if (data && data.errors) {
                    for (const k in data.errors) {
                        if (Object.prototype.hasOwnProperty.call(data.errors, k)) {
                            errorMessg += `${data.errors[k]}\n`;
                        }
                    }
                }
                showDangerAlert(errorMessg || (data && data.message) || "An error occurred while saving.");
            }
        },
        error: function (xhr) {
            showDangerAlert(xhr.responseText || "Error saving record.");
        },
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

$('#download-template').on('click', function () {
    const expectedColumns = [
        'PC',
        'Inspection Date',
        'Customer/Project Name',
        'Inspector Name',
        'Product Code',
        'Product Description',
        'LOT QTY(No)',
        'Project Value(Mn).',
        'TPI Duration(DAYS)',
        'Location(Waluj Lab/Vendor Location)',
        'Mode Of Inspection(Physical/Online)',
        'Cleared in first attempt',
        'Remark',
        'Actions plan (If any issue)',
        'MOM performed date'
    ];

    // Create a worksheet with only headers
    const worksheetData = [expectedColumns];

    // Create worksheet and workbook
    const worksheet = XLSX.utils.aoa_to_sheet(worksheetData);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "TPI Template");

    // Trigger download
    XLSX.writeFile(workbook, "TPI_Blank_Template.xlsx");
});






