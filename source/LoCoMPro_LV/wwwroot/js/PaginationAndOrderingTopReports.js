document.addEventListener("DOMContentLoaded", function () {
    var tabla = document.getElementById("miTabla");
    var tableBody = tabla.querySelector("tbody");
    var pagination = document.getElementById("pagination");
    var registrosPorPagina = 10;
    var currentPage = 1;
    var sortOrderRecordsCount = "desc";
    var sortOrderReportsReceived = "desc";
    var sortOrderAcceptedReportsCount = "desc";
    var currentSortColumn = null;

    // Esta función se encarga de mostrar las filas de la tabla que deben estar visibles en la página actual.
    // Filtra las filas según la página actual y oculta las demás.
    function mostrarFilas() {
        var filas = tableBody.querySelectorAll("tr");
        var inicio = (currentPage - 1) * registrosPorPagina;
        var fin = currentPage * registrosPorPagina;

        filas.forEach(function (fila, index) {
            if (index >= inicio && index < fin) {
                fila.style.display = "table-row";
            } else {
                fila.style.display = "none";
            }
        });
    }

    // Esta función se encarga de ordenar la tabla por la cantidad de registros en orden ascendente o descendente.
    // Utiliza el valor de sortOrder para determinar el orden y actualiza la tabla en consecuencia.
    function ordenarTablaPorCantidadRegistros() {
        var filas = Array.from(tableBody.querySelectorAll("tr"));

        filas.sort(function (a, b) {
            var valueA = parseInt(a.querySelector(".recordsCount").innerText) || 0;
            var valueB = parseInt(b.querySelector(".recordsCount").innerText) || 0;

            if (sortOrderRecordsCount === "asc") {
                return valueA - valueB;
            } else {
                return valueB - valueA;
            }
        });

        filas.forEach(function (fila) {
            tableBody.removeChild(fila);
        });

        filas.forEach(function (fila) {
            tableBody.appendChild(fila);
        });
    }

    // Esta función se encarga de ordenar la tabla por la cantidad de reportes recibidos en orden ascendente o descendente.
    // Utiliza el valor de sortOrder para determinar el orden y actualiza la tabla en consecuencia.
    function ordenarTablaPorReportesRecibidos() {
        var filas = Array.from(tableBody.querySelectorAll("tr"));

        filas.sort(function (a, b) {
            var valueA = parseInt(a.querySelector(".reportsReceived").innerText) || 0;
            var valueB = parseInt(b.querySelector(".reportsReceived").innerText) || 0;

            if (sortOrderReportsReceived === "asc") {
                return valueA - valueB;
            } else {
                return valueB - valueA;
            }
        });

        filas.forEach(function (fila) {
            tableBody.removeChild(fila);
        });

        filas.forEach(function (fila) {
            tableBody.appendChild(fila);
        });
    }

    // Esta función se encarga de ordenar la tabla por la cantidad de reportes aceptados recibidos en orden ascendente o descendente.
    // Utiliza el valor de sortOrder para determinar el orden y actualiza la tabla en consecuencia.
    function ordenarTablaPorReportesAceptadosRecibidos() {
        var filas = Array.from(tableBody.querySelectorAll("tr"));

        filas.sort(function (a, b) {
            var valueA = parseInt(a.querySelector(".acceptedReportsCount").innerText) || 0;
            var valueB = parseInt(b.querySelector(".acceptedReportsCount").innerText) || 0;

            if (sortOrderAcceptedReportsCount === "asc") {
                return valueA - valueB;
            } else {
                return valueB - valueA;
            }
        });

        filas.forEach(function (fila) {
            tableBody.removeChild(fila);
        });

        filas.forEach(function (fila) {
            tableBody.appendChild(fila);
        });
    }

    // Genera la paginación en la parte inferior de la tabla. Calcula el número de páginas y muestra los botones de
    // navegación para ir a páginas anteriores y siguientes. Además, muestra un número limitado de botones de página alrededor de la página actual.
    function generarPaginacion() {
        var filas = tableBody.querySelectorAll("tr");
        var totalRegistros = filas.length;
        var totalPaginas = Math.ceil(totalRegistros / registrosPorPagina);
        var paginaCercana = 2;

        var html = "";

        var inicioPaginacion = Math.max(1, currentPage - paginaCercana);
        var finPaginacion = Math.min(totalPaginas, currentPage + paginaCercana);

        if (currentPage > 1) {
            html += '<button class="page-link" data-page="' + (currentPage - 1) + '">Anterior</button>';
        }

        if (currentPage > (paginaCercana + 1)) {
            html += '<button class="page-link" data-page="1">1</button>';
            if (currentPage > (paginaCercana + 2)) {
                html += '<span class="page-link">...</span>';
            }
        }

        for (var i = inicioPaginacion; i <= finPaginacion; i++) {
            var isCurrentPage = i === currentPage ? 'current-page' : '';
            html += '<button class="page-link ' + isCurrentPage + '" data-page="' + i + '">' + i + '</button>';
        }

        if (currentPage < (totalPaginas - paginaCercana)) {
            if (currentPage < (totalPaginas - paginaCercana - 1)) {
                html += '<span class="page-link">...</span>';
            }
            html += '<button class="page-link" data-page="' + totalPaginas + '">' + totalPaginas + '</button>';
        }

        if (currentPage < totalPaginas) {
            html += '<button class="page-link" data-page="' + (currentPage + 1) + '">Siguiente</button>';
        }

        pagination.innerHTML = html;

        var pageButtons = pagination.querySelectorAll("button");

        pageButtons.forEach(function (button) {
            button.addEventListener("click", function () {
                currentPage = parseInt(this.getAttribute("data-page"));
                if (currentSortColumn === "recordsCount") {
                    ordenarTablaPorCantidadRegistros();
                } else if (currentSortColumn === "reportsReceived") {
                    ordenarTablaPorReportesRecibidos();
                } else if (currentSortColumn === "acceptedReportsCount") {
                    ordenarTablaPorReportesAceptadosRecibidos();
                }
                mostrarFilas();
                generarPaginacion();
            });
        });
    }

    var orderRecordsCountLink = document.getElementById("orderRecordsCount");
        orderRecordsCountLink.addEventListener("click", function () {
            if (sortOrderRecordsCount === "asc") {
                sortOrderRecordsCount = "desc";
                document.getElementById("RecordsCountArrow").innerHTML = " &#129095;";
                document.getElementById("ReportsReceivedArrow").innerHTML = "";
                document.getElementById("AcceptedReportsCountArrow").innerHTML = "";
            } else {
                sortOrderRecordsCount = "asc";
                document.getElementById("RecordsCountArrow").innerHTML = "&#129093;";
                document.getElementById("ReportsReceivedArrow").innerHTML = "";
                document.getElementById("AcceptedReportsCountArrow").innerHTML = "";
            }
            currentSortColumn = "recordsCount";
            ordenarTablaPorCantidadRegistros();
            mostrarFilas();
            generarPaginacion();
        });

    var orderReportsReceivedLink = document.getElementById("orderReportsReceived");
        orderReportsReceivedLink.addEventListener("click", function () {
            if (sortOrderReportsReceived === "asc") {
                sortOrderReportsReceived = "desc";
                document.getElementById("ReportsReceivedArrow").innerHTML = " &#129095;";
                document.getElementById("RecordsCountArrow").innerHTML = "";
                document.getElementById("AcceptedReportsCountArrow").innerHTML = "";
            } else {
                sortOrderReportsReceived = "asc";
                document.getElementById("ReportsReceivedArrow").innerHTML = "&#129093;";
                document.getElementById("RecordsCountArrow").innerHTML = "";
                document.getElementById("AcceptedReportsCountArrow").innerHTML = "";
            }
            currentSortColumn = "reportsReceived";
            ordenarTablaPorReportesRecibidos();
            mostrarFilas();
            generarPaginacion();
        });

    var orderAcceptedReportsCountLink = document.getElementById("orderAcceptedReportsCount");
        orderAcceptedReportsCountLink.addEventListener("click", function () {
            if (sortOrderAcceptedReportsCount === "asc") {
                sortOrderAcceptedReportsCount = "desc";
                document.getElementById("AcceptedReportsCountArrow").innerHTML = " &#129095;";
                document.getElementById("RecordsCountArrow").innerHTML = "";
                document.getElementById("ReportsReceivedArrow").innerHTML = "";
            } else {
                sortOrderAcceptedReportsCount = "asc";
                document.getElementById("AcceptedReportsCountArrow").innerHTML = "&#129093;";
                document.getElementById("RecordsCountArrow").innerHTML = "";
                document.getElementById("ReportsReceivedArrow").innerHTML = "";
            }
            currentSortColumn = "acceptedReportsCount";
            ordenarTablaPorReportesAceptadosRecibidos();
            mostrarFilas();
            generarPaginacion();
        });

    generarPaginacion();
    mostrarFilas();
});