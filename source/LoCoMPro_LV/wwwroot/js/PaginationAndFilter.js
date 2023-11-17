let allRows = [];
let filteredRows = [];
let pageSize = 10;
let currentPage = 1;
let totalPages = 1;

let selectedOrdering = "";
let orderingState = 0; // 0: Sin orden, 1: Ascendente, -1: Descendente
let clearFilter = 0;

const tableBody = document.querySelector('#miTabla tbody');

const priceOrdering = document.getElementById('orderPrice');
const dateOrdering = document.getElementById('orderDate');

const pageButtonsContainer = document.getElementById("pageButtonsContainer");
const previousButton = document.getElementById('pagination-button-previous');
const nextButton = document.getElementById('pagination-button-next');

// Inicialización de eventos y paginación al cargar la página
function initialize() {
    allRows = Array.from(document.querySelectorAll('#miTabla tbody tr'));
    filteredRows = allRows.slice();
    totalPages = Math.ceil(filteredRows.length / pageSize);
    clearFilter = 0;

    // Configurar eventos de los botones de paginación
    previousButton.addEventListener('click', handlePreviousButtonClick);
    nextButton.addEventListener('click', handleNextButtonClick);

    document.querySelectorAll('.store-checkbox').forEach(checkbox => {
        checkbox.addEventListener('change', applyFilters);
    });

    // Configurar eventos de los ordenamientos
    priceOrdering.addEventListener('click', () => handleOrderingClick("Price"));
    dateOrdering.addEventListener('click', () => handleOrderingClick("Date"));
    
    // Mostrar la primera página y actualizar los botones de navegación
    showPageRows(currentPage);
    updateNavigationButtons();
}

// Función para mostrar las filas de la página actual
function showPageRows(page) {
    const startIndex = (page - 1) * pageSize;
    const endIndex = startIndex + pageSize;
    const rowsToDisplay = filteredRows.slice(startIndex, endIndex);

    allRows.forEach(row => row.style.display = 'none');
    rowsToDisplay.forEach(row => row.style.display = 'table-row');
}

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

function handlePreviousButtonClick() {
    if (currentPage > 1) {
        currentPage--;
        showPageRows(currentPage);
        updateNavigationButtons();
    }
}

function handleNextButtonClick() {
    if (currentPage < totalPages) {
        currentPage++;
        showPageRows(currentPage);
        updateNavigationButtons();
    }
}

function handleOrderingClick(orderType) {
    if (selectedOrdering === orderType) {
        orderingState *= -1; // Alternar entre ascendente y descendente
    } else {
        orderingState = 1;  // Si es la primera vez que ordenamos por esta columna, se establece el estado como ascendente
    }

    selectedOrdering = orderType;
    applyFilters();
}

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

function applyFilters() {
    var selectedStores = Array.from(document.querySelectorAll('.store-checkbox:checked')).map(checkbox => checkbox.value);

    currentPage = 1;

    filteredRows = allRows.filter(row =>
        (selectedStores.length === 0 || selectedStores.includes(row.dataset.store))
    );

    if (selectedOrdering === "Price") {
        sortTableByPrice();
    } else if (selectedOrdering === "Date") {
        sortTableByDate();
    }

    totalPages = Math.ceil(filteredRows.length / pageSize);

    showPageRows(currentPage);
    updateNavigationButtons();
}

initialize();