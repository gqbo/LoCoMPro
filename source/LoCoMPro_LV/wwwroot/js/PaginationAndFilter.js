// Variables para el manejo de datos y paginación
let allRows = [];
let filteredRows = [];
let pageSize = 10;
let currentPage = 1;
let totalPages = 1;

// Variables para el manejo de ordenamiento
let selectedOrdering = "";
let orderingState = 0;
let clearFilter = 0;

// Elementos del DOM
const tableBody = document.querySelector('#miTabla tbody'); // Cuerpo de la tabla
const priceOrdering = document.getElementById('orderPrice'); // Botón de ordenamiento por precio
const dateOrdering = document.getElementById('orderDate'); // Botón de ordenamiento por fecha
const distanceOrdering = document.getElementById('orderDistance'); // Botón de ordenamiento por distancia
const clearFilters = document.getElementById('clear-filters'); // Botón de limpieza de filtros
const pageButtonsContainer = document.getElementById("pageButtonsContainer"); // Contenedor de botones de paginación
const previousButton = document.getElementById('pagination-button-previous'); // Botón de página anterior
const nextButton = document.getElementById('pagination-button-next'); // Botón de página siguiente

// Inicialización de eventos y paginación al cargar la página
function initialize() {
    // Obtener todas las filas y copiarlas para filtrar
    allRows = Array.from(document.querySelectorAll('#miTabla tbody tr'));
    filteredRows = allRows.slice();
    totalPages = Math.ceil(filteredRows.length / pageSize);
    clearFilter = 0;

    // Eventos para la paginación
    previousButton.addEventListener('click', handlePreviousButtonClick);
    nextButton.addEventListener('click', handleNextButtonClick);

    // Eventos para los checkboxes de y establecimientos
    document.querySelectorAll('.store-checkbox').forEach(checkbox => {
        checkbox.addEventListener('change', applyFilters);
    });

    // Evento para limpiar filtros
    clearFilters.addEventListener('click', handleClearFilterClick);

    // Mostrar las filas de la página actual y actualizar botones de navegación
    priceOrdering.addEventListener('click', () => handleOrderingClick("Price"));
    dateOrdering.addEventListener('click', () => handleOrderingClick("Date"));
    distanceOrdering.addEventListener('click', () => handleOrderingClick("Distance"));

    showPageRows(currentPage);
    updateNavigationButtons();
}

// Función para mostrar las filas de la página actual
function showPageRows(page) {
    const startIndex = (page - 1) * pageSize;
    const endIndex = startIndex + pageSize;
    const rowsToDisplay = filteredRows.slice(startIndex, endIndex);

    // Oculta todas las filas y muestra las seleccionadas
    allRows.forEach(row => row.style.display = 'none');
    rowsToDisplay.forEach(row => row.style.display = 'table-row');
}

// Función para crear los botones de la paginación
function createPageButtons(index) {
    const button = document.createElement("button");
    button.className = index === currentPage ? "pagination-current-page" : "pagination-pages";
    button.innerText = index;
    button.addEventListener("click", function () {
        currentPage = index;
        showPageRows(currentPage);
        updateNavigationButtons();
    });
    return button;
}

// Función para actualizar la apariencia de los botones de navegación
function updateNavigationButtons() {
    previousButton.hidden = currentPage === 1;
    pageButtonsContainer.innerHTML = "";

    pageButtonsContainer.appendChild(createPageButtons(1));

    let startPage, endPage;

    if (totalPages <= 5 || currentPage <= 3) {
        startPage = 2;
        endPage = Math.min(5, totalPages);
    } else if (currentPage >= totalPages - 2) {
        startPage = Math.max(1, totalPages - 4);
        endPage = totalPages;
    } else {
        startPage = Math.max(1, currentPage - 2);
        endPage = Math.min(currentPage + 2, totalPages - 1);
    }

    if (startPage > 2) {
        const ellipsisStart = document.createElement('span');
        ellipsisStart.textContent = '...';
        ellipsisStart.classList.add('pagination-suspensives');
        pageButtonsContainer.appendChild(ellipsisStart);
    }

    for (let i = startPage; i <= endPage; i++) {
        if (i < totalPages) {
            pageButtonsContainer.appendChild(createPageButtons(i));
        }
    }

    if (endPage < totalPages - 1) {
        const ellipsisEnd = document.createElement('span');
        ellipsisEnd.textContent = '...';
        ellipsisEnd.classList.add('pagination-suspensives');
        pageButtonsContainer.appendChild(ellipsisEnd);
    }

    if (totalPages > 1) {
        pageButtonsContainer.appendChild(createPageButtons(totalPages));
    }

    nextButton.hidden = currentPage === totalPages;
}

// Manejador de eventos para el botón siguiente de la paginación.
function handlePreviousButtonClick() {
    if (currentPage > 1) {
        currentPage--;
        showPageRows(currentPage);
        updateNavigationButtons();
    }
}

// Manejador de eventos para el botón anterior de la paginación.
function handleNextButtonClick() {
    if (currentPage < totalPages) {
        currentPage++;
        showPageRows(currentPage);
        updateNavigationButtons();
    }
}

// Manejador de eventos para los botones de ordenamiento.
function handleOrderingClick(orderType) {
    if (selectedOrdering === orderType) {
        orderingState *= -1; // Alternar entre ascendente y descendente
    } else {
        orderingState = 1;  // Si es la primera vez que ordenamos por esta columna, se establece el estado como ascendente
    }

    selectedOrdering = orderType;
    applyFilters();
}

// Manejador de eventos para limpiar los filtros
function handleClearFilterClick() {
    document.querySelectorAll('.store-checkbox:checked').forEach(checkbox => {
        checkbox.checked = false;
    });

    // Volver a aplicar los filtros y reinicializar la tabla
    applyFilters();
}

// Función de ordenamiento de la tabla por fecha
function sortTableByDate() {
    var filas = filteredRows;

    filas.sort(function (a, b) {
        var dateA = new Date(a.querySelector(".fecha").textContent.replace(/(\d{2})\/(\d{2})\/(\d{4})/, '$2/$1/$3'));
        var dateB = new Date(b.querySelector(".fecha").textContent.replace(/(\d{2})\/(\d{2})\/(\d{4})/, '$2/$1/$3'));

        if (orderingState === 1) {
            return dateA - dateB;
        } else {
            return dateB - dateA;
        }
    });

    filas.forEach(function (fila) {
        tableBody.removeChild(fila);
    });

    filas.forEach(function (fila) {
        tableBody.appendChild(fila);
    });
}

// Función de ordenamiento de la tabla por precio
function sortTableByPrice() {
    var filas = filteredRows;

    filas.sort(function (a, b) {
        var valueA = a.querySelector(".precio").textContent.replace("₡", "").trim();
        var valueB = b.querySelector(".precio").textContent.replace("₡", "").trim();

        if (orderingState === 1) {
            return parseFloat(valueA) - parseFloat(valueB);
        } else {
            return parseFloat(valueB) - parseFloat(valueA);
        }
    });

    filas.forEach(function (fila) {
        tableBody.removeChild(fila);
    });

    filas.forEach(function (fila) {
        tableBody.appendChild(fila);
    });
}

// Función de ordenamiento de la tabla por distancia
function sortTableByDistance() {
    var filas = filteredRows;

    filas.sort(function (a, b) {
        var valueA = parseFloat(a.querySelector(".distancia").innerText.replace(' km', '')) || 0;
        var valueB = parseFloat(b.querySelector(".distancia").innerText.replace(' km', '')) || 0;

        if (orderingState === 1) {
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

// Función para actualizar la apariencia de los botones de ordenamiento

function updateOrderingButtonAppearance() {
    const orderPriceButton = document.getElementById('orderPrice');
    const orderDateButton = document.getElementById('orderDate');
    const orderDistanceButton = document.getElementById('orderDistance');

    orderPriceButton.innerHTML = orderPriceButton.innerHTML.replace(' 🡅', '').replace(' 🡇', '');
    orderDateButton.innerHTML = orderDateButton.innerHTML.replace(' 🡅', '').replace(' 🡇', '');
    orderDistanceButton.innerHTML = orderDistanceButton.innerHTML.replace(' 🡅', '').replace(' 🡇', '');

    if (selectedOrdering === "Price") {
        if (orderingState === 1) {
            orderPriceButton.innerHTML += ' 🡅';
        } else if (orderingState === -1) {
            orderPriceButton.innerHTML += ' 🡇';
        }
    } else if (selectedOrdering === "Date") {
        if (orderingState === 1) {
            orderDateButton.innerHTML += ' 🡅';
        } else if (orderingState === -1) {
            orderDateButton.innerHTML += ' 🡇';
        }
    }
    if (selectedOrdering == "Distance") {
        if (orderingState === 1) {
            orderDistanceButton.innerHTML += ' 🡅';
        } else if (orderingState === -1) {
            orderDistanceButton.innerHTML += ' 🡇';
        }
    }
}

// Función para aplicar filtros
function applyFilters() {
    // Obtener establecimientos seleccionados
    var selectedStores = Array.from(document.querySelectorAll('.store-checkbox:checked')).map(checkbox => checkbox.value);

    currentPage = 1;

    // Filtrar las filas según los establecimientos seleccionados
    filteredRows = allRows.filter(row =>
        (selectedStores.length === 0 || selectedStores.includes(row.dataset.store))
    );

    // Desmarcar todos los checkboxes antes de aplicar los filtros
    document.querySelectorAll('.filter-checkbox').forEach(checkbox => {
        checkbox.checked = false;
    });

    // Marcar los checkboxes según los filtros aplicados
    selectedStores.forEach(store => {
        const checkbox = document.querySelector('.store-checkbox[value="' + store + '"]');
        if (checkbox) {
            checkbox.checked = true;
        }
    });

    // Ordenar la tabla y actualizar la apariencia de los botones de ordenamiento
    if (selectedOrdering === "Price") {
        sortTableByPrice();
        updateOrderingButtonAppearance();
    } else if (selectedOrdering === "Date") {
        sortTableByDate();
        updateOrderingButtonAppearance();
    }
    if (selectedOrdering === "Distance") {
        sortTableByDistance();
        updateOrderingButtonAppearance();
    }

    // Actualizar el número total de páginas y mostrar la página actual
    totalPages = Math.ceil(filteredRows.length / pageSize);

    showPageRows(currentPage);
    updateNavigationButtons();
}

// Inicializar la funcionalidad al cargar la página
initialize();