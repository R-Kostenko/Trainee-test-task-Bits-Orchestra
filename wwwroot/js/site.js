

function filterTable() {
    var table, tr, i, j, td, txtValue, input, filter, shouldDisplay;

    table = document.getElementById("recordsTable");
    tr = table.getElementsByTagName("tr");

    for (i = 1; i < tr.length; i++) {
        shouldDisplay = true;

        for (j = 0; j < 6; j++) {
            input = document.getElementById("filterInput" + (j + 1));
            filter = input.value.toUpperCase();
            td = tr[i].getElementsByTagName("td")[j];

            if (td) {
                txtValue = td.textContent || td.innerText;

                if (txtValue.toUpperCase().indexOf(filter) === -1) {
                    shouldDisplay = false;
                    break;
                }
            }
        }

        tr[i].style.display = shouldDisplay ? "" : "none";
    }
}

function sortTable(columnIndex) {
    var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;
    table = document.getElementById("recordsTable");
    switching = true;

    dir = "asc";

    while (switching) {
        switching = false;
        rows = table.rows;

        for (i = 1; i < (rows.length - 1); i++) {
            shouldSwitch = false;

            x = rows[i].getElementsByTagName("TD")[columnIndex];
            y = rows[i + 1].getElementsByTagName("TD")[columnIndex];

            if (x && y) {
                const xText = x.innerHTML.trim();
                const yText = y.innerHTML.trim();

                const dateRegex = /^\d{2}\.\d{2}\.\d{4}$/;
                if (dateRegex.test(xText) && dateRegex.test(yText)) {
                    const xDate = new Date(xText.split('.').reverse().join('-'));
                    const yDate = new Date(yText.split('.').reverse().join('-'));

                    if (dir == "asc") {
                        if (xDate > yDate) {
                            shouldSwitch = true;
                            break;
                        }
                    }
                    else if (dir == "desc") {
                        if (xDate < yDate) {
                            shouldSwitch = true;
                            break;
                        }
                    }
                }
                else {
                    if (dir == "asc") {
                        if (xText.toLowerCase() > yText.toLowerCase()) {
                            shouldSwitch = true;
                            break;
                        }
                    } else if (dir == "desc") {
                        if (xText.toLowerCase() < yText.toLowerCase()) {
                            shouldSwitch = true;
                            break;
                        }
                    }
                }
            }
        }

        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
            switchcount++;
        } else {
            if (switchcount == 0 && dir == "asc") {
                dir = "desc";
                switching = true;
            }
        }
    }
}

function deletePerson(id) {
    $.ajax({
        url: '/Home/Delete/' + id,
        type: 'POST',
        success: (response) => {
            location.reload();
        },
        error: () => {
            alert('Error occurred while deleting the person.');
        }
    });
}