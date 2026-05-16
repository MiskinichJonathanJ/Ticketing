import { getSeats } from '../config/api.js';
import { appStore } from '../appStore.js';
import { formatPrice, escapeHtml, showAlert } from '../utils/helpers.js';
import { MESSAGES, SEAT_STATUS, RESERVATION_TIMEOUT } from '../config/constants.js';

// ── Estado del mapa ───────────────────────────────────────────
let selectedSeatId = null;
let currentSector = null;
let currentSeatData = null;
let reservationTimer = null;
let allSeats = [];
let currentReservedSeatId = null;
let pollingInterval = null;

// ── SVG silla ─────────────────────────────────────────────────
export function seatSVG(color) {
    return `<svg viewBox="0 0 100 100" width="22" height="22">
        <rect x="10" y="5" width="80" height="55" rx="12" ry="12" fill="${color}"/>
        <rect x="5" y="55" width="90" height="28" rx="8" ry="8" fill="${color}"/>
        <rect x="12" y="80" width="14" height="18" rx="4" ry="4" fill="${color}"/>
        <rect x="74" y="80" width="14" height="18" rx="4" ry="4" fill="${color}"/>
        <rect x="0" y="38" width="14" height="30" rx="6" ry="6" fill="${color}"/>
        <rect x="86" y="38" width="14" height="30" rx="6" ry="6" fill="${color}"/>
    </svg>`;
}

// ── Getters de estado ─────────────────────────────────────────
export function getCurrentSector() { return currentSector; }
export function getCurrentReservedSeatId() { return currentReservedSeatId; }
export function getAllSeats() { return allSeats; }
export function resetAllSeats() { allSeats = []; }

// ── Timer de reserva ──────────────────────────────────────────
export function startReservationTimer(seatId, seatBtn) {
    let secondsLeft = RESERVATION_TIMEOUT * 60;

    const timerEl = document.getElementById('reservation-timer');
    if (timerEl) timerEl.style.display = 'block';

    reservationTimer = setInterval(() => {
        secondsLeft--;

        const mins = Math.floor(secondsLeft / 60);
        const secs = secondsLeft % 60;
        const display = `${mins}:${secs.toString().padStart(2, '0')}`;

        if (timerEl) timerEl.textContent = `⏱ Reserva expira en: ${display}`;

        if (secondsLeft === 60) {
            showAlert('⚠️ Tu reserva expira en 1 minuto', 'warning');
        }

        if (secondsLeft <= 0) {
            clearInterval(reservationTimer);
            reservationTimer = null;

            if (seatBtn) {
                seatBtn.innerHTML = seatSVG('#1d4ed8');
                seatBtn.disabled = false;
                seatBtn.addEventListener('click', () => {
                    const seat = allSeats.find(s => s.id === seatId);
                    if (seat) onSeatClick(seatBtn, seat);
                });
            }

            const seat = allSeats.find(s => s.id === seatId);
            if (seat) seat.status = 'Available';

            showAlert('⏰ Tu reserva expiró. La butaca volvió a estar disponible.', 'error');
            if (timerEl) timerEl.style.display = 'none';

            const panelSeat = document.getElementById('panel-seat');
            if (panelSeat) panelSeat.style.color = '';
            updatePanel();
        }
    }, 1000);
}

export function stopReservationTimer() {
    if (reservationTimer) {
        clearInterval(reservationTimer);
        reservationTimer = null;
    }
    const timerEl = document.getElementById('reservation-timer');
    if (timerEl) timerEl.style.display = 'none';
}

// ── Polling ───────────────────────────────────────────────────
export function startPolling() {
    stopPolling();
    pollingInterval = setInterval(async () => {
        if (currentSector) {
            const sectorToRefresh = currentSector;
            const event = appStore.getState('currentEvent');
            if (!event) return;

            try {
                const freshSeats = await getSeats(event.id, sectorToRefresh.id);
                if (currentSector && currentSector.id === sectorToRefresh.id) {
                    allSeats = freshSeats;
                    buildGrid(document.getElementById('seat-grid'), allSeats);
                }
            } catch (error) {
                console.error('Error en polling:', error);
            }
        }
    }, 30000);
}

export function stopPolling() {
    if (pollingInterval) {
        clearInterval(pollingInterval);
        pollingInterval = null;
    }
}

// ── Panel ─────────────────────────────────────────────────────
export function updatePanel() {
    const panelSector = document.getElementById('panel-sector');
    const panelSectorPrice = document.getElementById('panel-sector-price');
    const panelSeat = document.getElementById('panel-seat');
    const panelPrice = document.getElementById('panel-price');
    const sumCount = document.getElementById('sum-count');
    const sumPrice = document.getElementById('sum-price');
    const sumTotal = document.getElementById('sum-total');
    const btn = document.getElementById('btn-reserve');

    if (currentSector) {
        panelSector.textContent = currentSector.name;
        panelSectorPrice.textContent = formatPrice(currentSector.price);
    }

    if (!selectedSeatId || !currentSector) {
        panelSeat.textContent = 'Ninguna';
        panelSeat.className = 'pempty';
        panelPrice.style.display = 'none';
        sumCount.textContent = '0 butacas';
        sumPrice.textContent = '$0';
        sumTotal.textContent = '$0';
        btn.disabled = true;
        return;
    }

    panelSeat.textContent = `Fila ${currentSeatData.rowIdentifier} · Butaca ${currentSeatData.seatNumber}`;
    panelSeat.className = 'pvalue';
    panelPrice.textContent = formatPrice(currentSector.price);
    panelPrice.style.display = 'block';
    sumCount.textContent = '1 butaca';
    sumPrice.textContent = formatPrice(currentSector.price);
    sumTotal.textContent = formatPrice(currentSector.price);
    btn.disabled = false;
}

export function updatePanelReservation(seat) {
    const panelSeat = document.getElementById('panel-seat');
    const panelPrice = document.getElementById('panel-price');
    const sumCount = document.getElementById('sum-count');
    const sumPrice = document.getElementById('sum-price');
    const sumTotal = document.getElementById('sum-total');
    const btn = document.getElementById('btn-reserve');

    panelSeat.textContent = `Fila ${seat.rowIdentifier} · Butaca ${seat.seatNumber}`;
    panelSeat.className = 'pvalue';
    panelSeat.style.color = '#f59e0b';
    panelPrice.textContent = formatPrice(currentSector.price);
    panelPrice.style.display = 'block';
    sumCount.textContent = '1 butaca reservada';
    sumPrice.textContent = formatPrice(currentSector.price);
    sumTotal.textContent = formatPrice(currentSector.price);
    btn.disabled = true;
    btn.textContent = 'Butaca reservada ⏱';
}

// ── Mapa ──────────────────────────────────────────────────────
function onSeatClick(btn, seat) {
    if (seat.status !== SEAT_STATUS.AVAILABLE) return;

    document.querySelectorAll('.seat-btn:not(:disabled)').forEach(b => {
        b.innerHTML = seatSVG('#1d4ed8');
    });

    if (selectedSeatId === seat.id) {
        selectedSeatId = null;
        currentSeatData = null;
        updatePanel();
        return;
    }

    btn.innerHTML = seatSVG('#60a5fa');
    selectedSeatId = seat.id;
    currentSeatData = seat;
    updatePanel();
}

export function buildGrid(container, seats) {
    const rows = {};
    seats.forEach(seat => {
        if (!rows[seat.rowIdentifier]) rows[seat.rowIdentifier] = [];
        rows[seat.rowIdentifier].push(seat);
    });

    container.innerHTML = '';

    Object.entries(rows).sort().forEach(([rowId, rowSeats]) => {
        const rowEl = document.createElement('div');
        rowEl.className = 'grow';

        const label = document.createElement('div');
        label.className = 'rlabel';
        label.textContent = rowId;
        rowEl.appendChild(label);

        const seatsEl = document.createElement('div');
        seatsEl.className = 'grow-seats';

        const sorted = rowSeats.sort((a, b) => a.seatNumber - b.seatNumber);
        const mid = Math.floor(sorted.length / 2);

        sorted.forEach((seat, idx) => {
            if (idx === mid) {
                const aisle = document.createElement('div');
                aisle.className = 'aisle';
                seatsEl.appendChild(aisle);
            }

            const btn = document.createElement('button');
            btn.className = 'seat-btn';
            btn.dataset.seatId = seat.id;
            btn.title = `Fila ${rowId} · Butaca ${seat.seatNumber}`;

            if (seat.status === SEAT_STATUS.AVAILABLE) {
                btn.innerHTML = seatSVG('#1d4ed8');
                btn.addEventListener('click', () => onSeatClick(btn, seat));
            } else if (seat.status === SEAT_STATUS.RESERVED) {
                btn.innerHTML = seatSVG('#f59e0b');
                btn.disabled = true;
            } else {
                btn.innerHTML = seatSVG('#9ca3af');
                btn.disabled = true;
            }

            seatsEl.appendChild(btn);
        });

        rowEl.appendChild(seatsEl);
        container.appendChild(rowEl);
    });
}

export async function loadSeatMap(sector, animate = true) {
    currentSector = sector;
    selectedSeatId = null;
    currentSeatData = null;
    stopReservationTimer();
    updatePanel();

    const mapSub = document.getElementById('map-sub');
    if (mapSub) mapSub.textContent = `Sector ${sector.name} · ${formatPrice(sector.price)}`;

    const mapArea = document.getElementById('map-area');
    const container = document.getElementById('seat-grid');

    if (animate) {
        mapArea.classList.add('loading');
        container.innerHTML = '<div class="spinner"></div>';
    }

    setTimeout(async () => {
        try {
            const event = appStore.getState('currentEvent');
            allSeats = await getSeats(event.id, sector.id);

            const seats = allSeats.filter(s => s.sectorName === sector.name);

            if (allSeats.length === 0) {
                container.innerHTML = '<p class="no-data">No hay butacas disponibles</p>';
                return;
            }

            buildGrid(container, seats); 

        } catch (error) {
            showAlert(MESSAGES.LOAD_SEATS_ERROR, 'error');
            container.innerHTML = `<p class="error-msg">${MESSAGES.LOAD_SEATS_ERROR}</p>`;
        }

        if (animate) mapArea.classList.remove('loading');
    }, animate ? 300 : 0);
}

// ── Cancel ────────────────────────────────────────────────────
export function handleCancelReservation() {
    const activeSeatId = reservationTimer ? currentReservedSeatId : null;

    stopReservationTimer();

    if (activeSeatId) {
        const seatEl = document.querySelector(`[data-seat-id="${activeSeatId}"]`);
        if (seatEl) {
            seatEl.innerHTML = seatSVG('#1d4ed8');
            seatEl.disabled = false;
            seatEl.addEventListener('click', () => {
                const seat = allSeats.find(s => s.id === activeSeatId);
                if (seat) onSeatClick(seatEl, seat);
            });
        }
        const seat = allSeats.find(s => s.id === activeSeatId);
        if (seat) seat.status = 'Available';
    }

    document.querySelectorAll('.seat-btn:not(:disabled)').forEach(b => {
        b.innerHTML = seatSVG('#1d4ed8');
    });

    const panelSeat = document.getElementById('panel-seat');
    if (panelSeat) panelSeat.style.color = '';
    const btn = document.getElementById('btn-reserve');
    if (btn) btn.textContent = 'Reservar ahora';

    selectedSeatId = null;
    currentSeatData = null;
    currentReservedSeatId = null;
    updatePanel();
}

// ── Reserve button data ───────────────────────────────────────
export function getReserveData() {
    return {
        selectedSeatId,
        currentSeatData,
        currentSector,
        allSeats
    };
}

export function setReservedSeat(seatId) {
    currentReservedSeatId = seatId;
}

export function updateSeatStatus(seatId, status) {
    const seat = allSeats.find(s => s.id === seatId);
    if (seat) seat.status = status;
}