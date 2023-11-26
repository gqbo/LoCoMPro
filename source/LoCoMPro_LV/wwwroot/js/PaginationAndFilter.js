﻿// Variables para el manejo de datos y paginación
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

// Esta función se encarga de inicializar la página. Configura los manejadores de eventos para los botones de paginación, 
// las casillas de verificación de provincias, cantones y tiendas, así como los botones de ordenamiento y limpieza de filtros.
// También inicializa la visualización de las filas de la página actual y actualiza los botones de navegación.
function initialize() {
    allRows = Array.from(document.querySelectorAll('#miTabla tbody tr'));
    filteredRows = allRows.slice();
    totalPages = Math.ceil(filteredRows.length / pageSize);
    clearFilter = 0;

    previousButton.addEventListener('click', handlePreviousButtonClick);
    nextButton.addEventListener('click', handleNextButtonClick);

    document.querySelectorAll('.province-checkbox').forEach(checkbox => {
        checkbox.addEventListener('change', handleProvinceCheckboxChange);
    });

    document.querySelectorAll('.canton-checkbox').forEach(checkbox => {
        checkbox.addEventListener('change', applyFilters);
    });

    document.querySelectorAll('.store-checkbox').forEach(checkbox => {
        checkbox.addEventListener('change', applyFilters);
    });

    clearFilters.addEventListener('click', handleClearFilterClick);

    priceOrdering.addEventListener('click', () => handleOrderingClick("Price"));
    dateOrdering.addEventListener('click', () => handleOrderingClick("Date"));
    if (distanceOrdering != null) {
        distanceOrdering.addEventListener('click', () => handleOrderingClick("Distance"));
    }

    showPageRows(currentPage);
    updateNavigationButtons();
}

// Muestra las filas correspondientes a la página proporcionada y oculta las demás.
function showPageRows(page) {
    const startIndex = (page - 1) * pageSize;
    const endIndex = startIndex + pageSize;
    const rowsToDisplay = filteredRows.slice(startIndex, endIndex);

    allRows.forEach(row => row.style.display = 'none');
    rowsToDisplay.forEach(row => row.style.display = 'table-row');
}

// Crea un botón de página para la paginación, asignándole una clase específica y un manejador de eventos que actualiza 
// la visualización de las filas y los botones de navegación.
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

// Actualiza la apariencia de los botones de navegación(anterior y siguiente) y los botones de páginas, mostrando un rango 
// específico de páginas y añadiendo puntos suspensivos en caso de haber más páginas disponibles.
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

// Maneja el cambio en las casillas de verificación de las provincias, actualizando las casillas de verificación de los cantones y 
// aplicando los filtros.
function handleProvinceCheckboxChange() {
    var selectedProvinces = Array.from(document.querySelectorAll('.province-checkbox:checked')).map(checkbox => checkbox.value);

    if (selectedProvinces.length === 0) {
        document.querySelectorAll('.canton-checkbox:checked').forEach(checkbox => {
            checkbox.checked = false;
        });
    } else {
        document.querySelectorAll('.canton-checkbox').forEach(checkbox => {
            var province = checkbox.closest('.cantones-list').id.replace('cantones-', '');

            if (!selectedProvinces.includes(province)) {
                checkbox.checked = false;
            }
        });
    }

    // Ocultar los desplegables de cantones
    document.querySelectorAll('.cantones-list').forEach(cantonesList => {
        cantonesList.style.display = 'none';
    });

    selectedProvinces.forEach(province => {
        document.getElementById('cantones-' + province).style.display = 'block';
    });

    updateCantonsFilter(selectedProvinces);
    applyFilters();
}

// Actualiza las casillas de verificación de los cantones según las provincias seleccionadas.
function updateCantonsFilter(selectedProvinces) {
    var cantonCheckboxes = document.querySelectorAll('.canton-checkbox');

    cantonCheckboxes.forEach(checkbox => {
        var province = checkbox.closest('.cantones-list').id.replace('cantones-', '');

        checkbox.disabled = selectedProvinces.length > 0 && !selectedProvinces.includes(province);
        checkbox.checked = checkbox.checked && !checkbox.disabled;
    });
}

// Maneja el evento de hacer clic en el botón de limpieza de filtros, deseleccionando todas las casillas de verificación y aplicando los filtros.
function handleClearFilterClick() {
    document.querySelectorAll('.province-checkbox:checked').forEach(checkbox => {
        checkbox.checked = false;
        // Oculta el desplegable de cantones asociado
        const province = checkbox.value;
        document.getElementById('cantones-' + province).style.display = 'none';
    });

    document.querySelectorAll('.canton-checkbox:checked').forEach(checkbox => {
        checkbox.checked = false;
    });

    document.querySelectorAll('.store-checkbox:checked').forEach(checkbox => {
        checkbox.checked = false;
    });

    applyFilters();
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

// Ordena la tabla por fecha, ascendente o descendente según el estado de ordenamiento.
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

// Ordena la tabla por precio, ascendente o descendente según el estado de ordenamiento.
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

// Ordena la tabla por distancia, ascendente o descendente según el estado de ordenamiento.
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

// Actualiza la apariencia de los botones de ordenamiento indicando la dirección del ordenamiento (ascendente o descendente).
function updateOrderingButtonAppearance() {
    const orderPriceButton = document.getElementById('orderPrice');
    const orderDateButton = document.getElementById('orderDate');
    const orderDistanceButton = document.getElementById('orderDistance');

    orderPriceButton.innerHTML = orderPriceButton.innerHTML.replace(' 🡅', '').replace(' 🡇', '');
    orderDateButton.innerHTML = orderDateButton.innerHTML.replace(' 🡅', '').replace(' 🡇', '');
    if (orderDistanceButton != null) {
        orderDistanceButton.innerHTML = orderDistanceButton.innerHTML.replace(' 🡅', '').replace(' 🡇', '');
    }

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
    } else if (selectedOrdering == "Distance") {
        if (orderingState === 1) {
            orderDistanceButton.innerHTML += ' 🡅';
        } else if (orderingState === -1) {
            orderDistanceButton.innerHTML += ' 🡇';
        }
    }
}

// Aplica los filtros seleccionados, actualizando las filas a mostrar y ordenando la tabla según el criterio seleccionado.
function applyFilters() {
    var selectedProvinces = Array.from(document.querySelectorAll('.province-checkbox:checked')).map(checkbox => checkbox.value);
    var selectedCantons = Array.from(document.querySelectorAll('.canton-checkbox:checked')).map(checkbox => checkbox.value);
    var selectedStores = Array.from(document.querySelectorAll('.store-checkbox:checked')).map(checkbox => checkbox.value);

    currentPage = 1;

    filteredRows = allRows.filter(row =>
        (selectedProvinces.length === 0 || selectedProvinces.includes(row.dataset.province)) &&
        (selectedCantons.length === 0 || selectedCantons.includes(row.dataset.canton)) &&
        (selectedStores.length === 0 || selectedStores.includes(row.dataset.store))
    );

    // Ordenar la tabla y actualizar la apariencia de los botones de ordenamiento
    if (selectedOrdering === "Price") {
        sortTableByPrice();
        updateOrderingButtonAppearance();
    } else if (selectedOrdering === "Date") {
        sortTableByDate();
        updateOrderingButtonAppearance();
    } else if (selectedOrdering === "Distance") {
        sortTableByDistance();
        updateOrderingButtonAppearance();
    }

    totalPages = Math.ceil(filteredRows.length / pageSize);

    showPageRows(currentPage);
    updateNavigationButtons();
}

initialize();