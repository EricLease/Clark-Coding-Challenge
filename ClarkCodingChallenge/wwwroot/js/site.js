// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function selectRow(tr) {
    const hdnFilter = document.getElementById('Filter');
    const hdnSort = document.getElementById('SortDirection');
    window.location.replace(`/Contacts/Index/${tr.dataset.id}?filter=${hdnFilter.value}&sort=${hdnSort.value}`);
}

function createRow(c, selected) {
    const tr = document.createElement('tr');

    tr.scope = 'row';
    tr.dataset.id = c.id;
    tr.addEventListener('click', () => selectRow(tr));

    if (c.id == selected) tr.classList.add('bg-info');

    const tdLast = document.createElement('td');
    const tdFirst = document.createElement('td');
    const tdEmail = document.createElement('td');

    tdLast.innerHTML = c.lastName;
    tdFirst.innerHTML = c.firstName;
    tdEmail.innerHTML = c.email;

    tr.appendChild(tdLast);
    tr.appendChild(tdFirst);
    tr.appendChild(tdEmail);

    return tr;
}

function createThead(empty) {
    const thead = document.createElement('thead');
    const tr = document.createElement('tr');

    if (empty) {
        const th = document.createElement('th');

        th.innerHTML = 'None';
        tr.appendChild(th);
        thead.appendChild(tr);

        return thead;
    }

    const thLast = document.createElement('th');
    const thFirst = document.createElement('th');
    const thEmail = document.createElement('th');

    thLast.innerHTML = 'Last Name';
    thFirst.innerHTML = 'First Name';
    thEmail.innerHTML = 'Email Address';
    thLast.scope = 'col';
    thFirst.scope = 'col';
    thEmail.scope = 'col';
    tr.appendChild(thLast);
    tr.appendChild(thFirst);
    tr.appendChild(thEmail);
    thead.appendChild(tr);
    thead.classList.add('thead-dark');

    return thead;
}

function createTBody(trs) {
    const tbody = document.createElement('tbody');

    tbody.classList.add('table-hover');
    trs.forEach(tr => tbody.appendChild(tr));

    return tbody;
}

function clearTable(table) {
    while (table.lastChild) table.removeChild(table.lastChild);
}

function applyFilter() {
    const hdnContactId = document.getElementById('Contact_Id');
    const hdnFilter = document.getElementById('Filter');
    const hdnSort = document.getElementById('SortDirection');
    const txtFilter = document.getElementById('txtFilter');
    const filter = txtFilter.value;
    const selected = hdnContactId.value;

    hdnFilter.value = filter;

    $.ajax({
        type: 'GET',
        url: `/Contacts/List?filter=${filter}&sort=${hdnSort.value}`,
        contentType: 'json',
        success: (result) => {
            const trs = result.map(c => createRow(c, selected));
            const table = document.getElementById('tblExisting');

            clearTable(table);

            if (trs.length) {
                const thead = createThead(false);
                const tbody = createTBody(trs);

                table.appendChild(thead);
                table.appendChild(tbody);
            } else {
                const thead = createThead(true);

                table.appendChild(thead);
            }
        }
    });
}

function clearFilter() {
    const txtFilter = document.getElementById('txtFilter');

    if (!txtFilter.value.length) return;

    txtFilter.value = '';
    applyFilter();
}

function applySort() {
    const radioAscending = document.getElementById('radioAscending');
    const hdnSortDirection = document.getElementById('SortDirection');
    const descending = radioDescending.checked;

    radioAscending.checked = !descending;
    radioDescending.checked = descending;
    hdnSortDirection.value = descending ? '1' : '0';
    applyFilter();
}

$(document).ready(() => {
    const hdnFilter = document.getElementById('Filter');
    const hdnSortDirection = document.getElementById('SortDirection');
    const radioAscending = document.getElementById('radioAscending');
    const radioDescending = document.getElementById('radioDescending');
    const txtFilter = document.getElementById('txtFilter');
    const descending = hdnSortDirection.value === '1';

    txtFilter.value = hdnFilter.value;
    radioAscending.checked = !descending;
    radioDescending.checked = descending;
    applyFilter();
});