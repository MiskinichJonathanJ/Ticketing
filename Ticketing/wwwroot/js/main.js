import { getEvents, getSectors, getSeats, reserveSeat } from './config/api.js';
import { appStore } from './appStore.js';
import { formatDate, formatPrice, escapeHtml } from './utils/helpers.js';
import { MESSAGES, SEAT_STATUS, DEFAULT_USER_ID, RESERVATION_TIMEOUT } from './config/constants.js';
import { initializeModal, showReservationModal } from './utils/helpers.js';

// ── SVG silla ────────────────────────────────────────────────
function seatSVG(color) {
    return `<svg viewBox="0 0 100 100" width="22" height="22">
        <!-- Respaldo -->
        <rect x="10" y="5" width="80" height="55" rx="12" ry="12" fill="${color}"/>
        <!-- Asiento -->
        <rect x="5" y="55" width="90" height="28" rx="8" ry="8" fill="${color}"/>
        <!-- Patas izquierda -->
        <rect x="12" y="80" width="14" height="18" rx="4" ry="4" fill="${color}"/>
        <!-- Patas derecha -->
        <rect x="74" y="80" width="14" height="18" rx="4" ry="4" fill="${color}"/>
        <!-- Apoyabrazos izquierdo -->
        <rect x="0" y="38" width="14" height="30" rx="6" ry="6" fill="${color}"/>
        <!-- Apoyabrazos derecho -->
        <rect x="86" y="38" width="14" height="30" rx="6" ry="6" fill="${color}"/>
    </svg>`;
}

// ── Helpers UI ───────────────────────────────────────────────
function showSection(id) {
    document.querySelectorAll('.section').forEach(s => s.classList.remove('active'));
    document.getElementById(id).classList.add('active');
}

function showAlert(msg, type) {
    const el = document.getElementById('alert-global');
    el.className = 'alert ' + type;
    el.textContent = msg;
    setTimeout(() => { el.className = 'alert'; el.textContent = ''; }, 4000);
}

// ── Estado ───────────────────────────────────────────────────
let selectedSeatId = null;
let currentSector = null;
let currentSeatData = null;
let reservationTimer = null;
let allSeats = []; // guardamos todos los asientos para el mapa completo
let currentReservedSeatId = null; // ← guardamos el id de la butaca reservada

// ── Timer de reserva ─────────────────────────────────────────
function startReservationTimer(seatId, seatBtn) {
    let secondsLeft = RESERVATION_TIMEOUT * 60; // 5 minutos en segundos

    // Mostrar timer en el panel
    const timerEl = document.getElementById('reservation-timer');
    if (timerEl) timerEl.style.display = 'block';

    reservationTimer = setInterval(() => {
        secondsLeft--;

        const mins = Math.floor(secondsLeft / 60);
        const secs = secondsLeft % 60;
        const display = `${mins}:${secs.toString().padStart(2, '0')}`;

        if (timerEl) timerEl.textContent = `⏱ Reserva expira en: ${display}`;

        // Alerta cuando quedan 60 segundos
        if (secondsLeft === 60) {
            showAlert('⚠️ Tu reserva expira en 1 minuto', 'warning');
        }

        // Momento en que expira
        if (secondsLeft <= 0) {
            clearInterval(reservationTimer);
            reservationTimer = null;

            // Butaca vuelve a estar disponible
            if (seatBtn) {
                seatBtn.innerHTML = seatSVG('#1d4ed8');
                seatBtn.disabled = false;

                // Restaura el evento de click
                seatBtn.addEventListener('click', () => {
                    const seat = allSeats.find(s => s.id === seatId);
                    if (seat) onSeatClick(seatBtn, seat);
                });
            }

            // Actualiza el array
            const panelSeat = document.getElementById('panel-seat');
            panelSeat.style.color = ''; 
            updatePanel();
        }
    }, 1000);
}

function stopReservationTimer() {
    if (reservationTimer) {
        clearInterval(reservationTimer);
        reservationTimer = null;
    }
    const timerEl = document.getElementById('reservation-timer');
    if (timerEl) timerEl.style.display = 'none';
}

function updatePanelReservation(seat) {
    const panelSeat = document.getElementById('panel-seat');
    const panelPrice = document.getElementById('panel-price');
    const sumCount = document.getElementById('sum-count');
    const sumPrice = document.getElementById('sum-price');
    const sumTotal = document.getElementById('sum-total');
    const btn = document.getElementById('btn-reserve');

    // Mostrar info de la reserva activa
    panelSeat.textContent = `Fila ${seat.rowIdentifier} · Butaca ${seat.seatNumber}`;
    panelSeat.className = 'pvalue';
    panelSeat.style.color = '#f59e0b'; // amarillo para indicar que está reservada
    panelPrice.textContent = formatPrice(currentSector.price);
    panelPrice.style.display = 'block';
    sumCount.textContent = '1 butaca reservada';
    sumPrice.textContent = formatPrice(currentSector.price);
    sumTotal.textContent = formatPrice(currentSector.price);
    btn.disabled = true;
    btn.textContent = 'Butaca reservada ⏱';
}


// ── Panel ────────────────────────────────────────────────────
function updatePanel() {
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

// ── Mapa de asientos ─────────────────────────────────────────
function onSeatClick(btn, seat) {
    // Prevenir click en butacas que ya no están disponibles
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

function buildGrid(container, seats) {
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

async function loadSeatMap(sector, animate = true) {
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

           
            if (allSeats.length === 0) {
                allSeats = await getSeats(event.id);
            }

            const seats = allSeats.filter(s => s.sectorName === sector.name);

            if (seats.length === 0) {
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

// ── Sectores ─────────────────────────────────────────────────
async function loadSectors(eventId) {
    const container = document.getElementById('sectors-container');
    container.innerHTML = '<div class="spinner"></div>';

    try {
        const sectors = await getSectors(eventId);
        appStore.setState('sectors', sectors);

        container.innerHTML = sectors.map((sector, idx) => `
            <div class="sector-card ${idx === 0 ? 'active' : ''}"
                 id="sector-card-${sector.id}"
                 onclick="selectSector(${sector.id})">
                <div class="sname">${escapeHtml(sector.name)}</div>
                <div class="sprice">${formatPrice(sector.price)}</div>
                <div class="scap">${sector.capacity} butacas</div>
            </div>
        `).join('');

        // Carga el primer sector por defecto 
        if (sectors.length > 0) {
            await loadSeatMap(sectors[0], false);
        }

    } catch (error) {
        showAlert(MESSAGES.LOAD_SECTORS_ERROR, 'error');
        container.innerHTML = `<p class="error-msg">${MESSAGES.LOAD_SECTORS_ERROR}</p>`;
    }
}

window.selectSector = async function (sectorId) {
    const sectors = appStore.getState('sectors');
    const sector = sectors.find(s => s.id === sectorId);
    if (!sector || (currentSector && currentSector.id === sectorId)) return;

    // Actualiza cards
    document.querySelectorAll('.sector-card').forEach(c => c.classList.remove('active'));
    const card = document.getElementById(`sector-card-${sectorId}`);
    card.classList.add('active', 'pulse');
    setTimeout(() => card.classList.remove('pulse'), 300);

    await loadSeatMap(sector, true);
};

// ── Eventos ───────────────────────────────────────────────────
async function loadEvents() {
    const container = document.getElementById('events-container');
    container.innerHTML = '<div class="spinner"></div>';

    try {
        const events = await getEvents();
        appStore.setState('events', events);

        if (events.length === 0) {
            container.innerHTML = '<p class="no-data">No hay eventos disponibles</p>';
            return;
        }

        container.innerHTML = events.map(event => `
            <div class="event-card">
                <div>
                    <div class="event-name">${escapeHtml(event.name)}</div>
                    <div class="event-meta">
                         ${formatDate(event.eventDate)}  ${escapeHtml(event.venue)}
                    </div>
                    <span class="badge-active">${escapeHtml(event.status)}</span>
                </div>
                <button class="btn-primary" onclick="selectEvent(${event.id})">
                    Ver asientos
                </button>
            </div>
        `).join('');

    } catch (error) {
        showAlert(MESSAGES.LOAD_EVENTS_ERROR, 'error');
        container.innerHTML = `<p class="error-msg">${MESSAGES.LOAD_EVENTS_ERROR}</p>`;
    }
}

window.selectEvent = async function (eventId) {
    const events = appStore.getState('events');
    const event = events.find(e => e.id === eventId);
    if (!event) return;

    appStore.setState('currentEvent', event);
    allSeats = []; // resetea los asientos al cambiar de evento

    document.getElementById('event-bar').innerHTML = `
        <div><div class="bi-label">Evento</div><div class="bi-value">${escapeHtml(event.name)}</div></div>
        <div><div class="bi-label">Fecha</div><div class="bi-value">${formatDate(event.eventDate)}</div></div>
        <div><div class="bi-label">Lugar</div><div class="bi-value">${escapeHtml(event.venue)}</div></div>
    `;

    showSection('section-seats');
    await loadSectors(eventId);
    startPolling();
};

// ── Reserva ───────────────────────────────────────────────────
document.getElementById('btn-reserve').addEventListener('click', async () => {
    if (!selectedSeatId) return;

    const btn = document.getElementById('btn-reserve');
    btn.disabled = true;
    btn.textContent = 'Reservando...';

    // Guarda referencia al botón de la butaca
    const seatBtn = document.querySelector(`[data-seat-id="${selectedSeatId}"]`);
    const seatIdToReserve = selectedSeatId;
    currentReservedSeatId = selectedSeatId; //  guardamos para el cancel

    try {
        await reserveSeat(selectedSeatId, DEFAULT_USER_ID);

        // Butaca pasa a amarillo (reserva)
        if (seatBtn) {
            seatBtn.innerHTML = seatSVG('#f59e0b');
            seatBtn.disabled = true;
        }

        // Actualiza estado local
        const seat = allSeats.find(s => s.id === seatIdToReserve);
        if (seat) seat.status = 'Reserved';

        showAlert('✅ ¡Butaca reservada! Tenés 5 minutos para completar el pago.', 'success');
        showReservationModal();

        // Guarda la info de la reserva para mostrarla en el panel
        const reservedSeat = currentSeatData;
        selectedSeatId = null;
        currentSeatData = null;

        // Mostrar reserva activa en el panel
        updatePanelReservation(reservedSeat);

        // Inicia timer de expiración
        startReservationTimer(seatIdToReserve, seatBtn, reservedSeat);

    } catch (error) {
        if (error.status === 409) {
            showAlert('⚠️ Otro usuario reservó esta butaca. El mapa se actualizó.', 'warning');
        } else {
            showAlert(MESSAGES.RESERVE_ERROR || 'Error al reservar', 'error');
        }
        allSeats = [];
        if (currentSector) await loadSeatMap(currentSector, false);
    } finally {
        btn.disabled = false;
        btn.textContent = 'Reservar ahora';
    }
});

document.getElementById('btn-cancel').addEventListener('click', () => {
    // Si hay un timer corriendo, guardamos el seatId antes de pararlo
    const activeSeatId = reservationTimer ? currentReservedSeatId : null;

    // Paramos el timer
    stopReservationTimer();

    // Si había una reserva activa, volvemos la butaca a disponible
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
        // Actualizamos el estado en el array local
        const seat = allSeats.find(s => s.id === activeSeatId);
        if (seat) seat.status = 'Available';
    }

    // Deseleccionamos cualquier butaca seleccionada
    document.querySelectorAll('.seat-btn:not(:disabled)').forEach(b => {
        b.innerHTML = seatSVG('#1d4ed8');
    });

    // Reseteamos el panel
    const panelSeat = document.getElementById('panel-seat');
    if (panelSeat) panelSeat.style.color = '';
    const btn = document.getElementById('btn-reserve');
    if (btn) btn.textContent = 'Reservar ahora';

    selectedSeatId = null;
    currentSeatData = null;
    updatePanel();
});




document.getElementById('btn-back').addEventListener('click', () => {
    stopPolling(); 
    stopReservationTimer();
    allSeats = [];
    showSection('section-events');
});

// ── Polling — refresca el mapa cada 30 segundos ──────────────
let pollingInterval = null;

function startPolling() {
    stopPolling();
    pollingInterval = setInterval(async () => {
        if (currentSector) {
            // Resetea los asientos para traer datos frescos
            allSeats = [];
            await loadSeatMap(currentSector, false);
        }
    }, 30000);
}

function stopPolling() {
    if (pollingInterval) {
        clearInterval(pollingInterval);
        pollingInterval = null;
    }
}


// ── Init ──────────────────────────────────────────────────────
loadEvents();

// ── Init MODAL ──────────────────────────────────────────────────────
initializeModal();