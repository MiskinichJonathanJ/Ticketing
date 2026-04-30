import { getSeats } from '../config/api.js';
import { appStore } from '../appStore.js';
import { showError, showSuccess, formatPrice, escapeHtml } from '../utils/helpers.js';
import { MESSAGES, SELECTORS, SEAT_STATUS } from '../config/constants.js';
import { tryReserveSeat } from '../services/reservationService.js';

let selectedSeatId = null;

export async function renderSeatMap() {
    const sector = appStore.getState('currentSector');
    const event = appStore.getState('currentEvent');
    if (!sector || !event) return;

    // Actualizamos el subtítulo
    const subtitle = document.getElementById(SELECTORS.SECTOR_SUBTITLE);
    if (subtitle) {
        subtitle.textContent = `Sector ${sector.name} · ${formatPrice(sector.price)}`;
    }

    const container = document.getElementById(SELECTORS.SEATS_CONTAINER);
    if (!container) return;

    container.innerHTML = '<div class="spinner"></div>';
    selectedSeatId = null;
    updatePanel(null, sector);

    try {
        const seats = await getSeats(event.id);
        appStore.setState('seats', seats);

        // Filtramos solo los asientos del sector actual
        const sectorSeats = seats.filter(s => s.sectorName === sector.name);

        if (sectorSeats.length === 0) {
            container.innerHTML = '<p class="no-data">No hay butacas disponibles</p>';
            return;
        }

        buildGrid(container, sectorSeats, sector);

    } catch (error) {
        showError(MESSAGES.LOAD_SEATS_ERROR);
        container.innerHTML = `<p class="error-msg">${MESSAGES.LOAD_SEATS_ERROR}</p>`;
    }

    // Botón reservar
    document.getElementById(SELECTORS.RESERVE_BTN).onclick = async () => {
        if (!selectedSeatId) return;

        const reservation = await tryReserveSeat(selectedSeatId);

        if (reservation) {
            // Reserva exitosa → actualizamos la butaca en el mapa
            const seatEl = document.querySelector(`[data-seat-id="${selectedSeatId}"]`);
            if (seatEl) {
                seatEl.classList.remove('selected', 'available');
                seatEl.classList.add('reserved');
                seatEl.disabled = true;
            }
            selectedSeatId = null;
            updatePanel(null, sector);
        } else {
            // Error → refrescamos el mapa
            renderSeatMap();
        }
    };
}


function buildGrid(container, seats, sector) {
    const rows = {};

    seats.forEach(seat => {
        if (!rows[seat.rowIdentifier]) rows[seat.rowIdentifier] = [];
        rows[seat.rowIdentifier].push(seat);
    });

    container.innerHTML = '';

    Object.entries(rows).forEach(([rowId, rowSeats]) => {
        const rowWrapper = document.createElement('div');
        rowWrapper.className = 'row-wrapper';

        // letra fila (A, B, C...)
        const label = document.createElement('div');
        label.className = 'row-label';
        label.textContent = rowId;

        const seatsRow = document.createElement('div');
        seatsRow.className = 'seats-row';

        rowSeats
            .sort((a, b) => a.seatNumber - b.seatNumber)
            .forEach((seat, index) => {

                // pasillo en el medio
                if (index === Math.floor(rowSeats.length / 2)) {
                    const aisle = document.createElement('div');
                    aisle.className = 'aisle';
                    seatsRow.appendChild(aisle);
                }

                const btn = document.createElement('button');
                btn.className = 'seat';
                btn.dataset.seatId = seat.id;

                if (seat.status === "AVAILABLE") {
                    btn.classList.add('available');
                    btn.onclick = () => onSeatClick(btn, seat, sector);
                } else if (seat.status === "RESERVED") {
                    btn.classList.add('reserved');
                    btn.disabled = true;
                } else {
                    btn.classList.add('occupied');
                    btn.disabled = true;
                }

                seatsRow.appendChild(btn);
            });

        rowWrapper.appendChild(label);
        rowWrapper.appendChild(seatsRow);
        container.appendChild(rowWrapper);
    });
}



function onSeatClick(btn, seat, sector) {
    // Deseleccionamos la anterior
    document.querySelectorAll('.seat.selected').forEach(s => {
        s.classList.remove('selected');
        s.classList.add('available');
    });

    // Si clickeamos la misma la deseleccionamos
    if (selectedSeatId === seat.id) {
        selectedSeatId = null;
        updatePanel(null, sector);
        return;
    }

    // Seleccionamos la nueva
    btn.classList.remove('available');
    btn.classList.add('selected');
    selectedSeatId = seat.id;
    updatePanel(seat, sector);
}

function updatePanel(seat, sector) {
    const panelSeat = document.getElementById('panel-seat');
    const panelPrice = document.getElementById('panel-price');
    const summaryCount = document.getElementById('summary-count');
    const summaryPrice = document.getElementById('summary-price');
    const summaryTotal = document.getElementById('summary-total');
    const btn = document.getElementById(SELECTORS.RESERVE_BTN);

    if (!seat) {
        panelSeat.textContent = 'Ninguna';
        panelSeat.className = 'empty-seat';
        panelPrice.textContent = '';
        summaryCount.textContent = '0 butacas';
        summaryPrice.textContent = '$0';
        summaryTotal.textContent = '$0';
        btn.disabled = true;
        return;
    }

    panelSeat.textContent = `Fila ${seat.rowIdentifier} · Butaca ${seat.seatNumber}`;
    panelSeat.className = 'panel-value';
    panelPrice.textContent = formatPrice(sector.price);
    summaryCount.textContent = '1 butaca';
    summaryPrice.textContent = formatPrice(sector.price);
    summaryTotal.textContent = formatPrice(sector.price);
    btn.disabled = false;
}