import { reserveSeat } from './config/api.js';
import { showAlert, initializeModal, showReservationModal, closeReservationModal } from './utils/helpers.js';
import { MESSAGES, DEFAULT_USER_ID } from './config/constants.js';
import { loadEvents, selectEvent } from './components/EventList.js';
import { loadSectors, selectSector } from './components/SectorList.js';
import {
    loadSeatMap, seatSVG, startPolling, stopPolling,
    stopReservationTimer, updatePanelReservation,
    handleCancelReservation, getReserveData,
    setReservedSeat, updateSeatStatus
} from './components/SeatMap.js';
import { showPaymentSection, initPaymentSection } from './components/PaymentSection.js';

function showSection(id) {
    document.querySelectorAll('.section').forEach(s => s.classList.remove('active'));
    document.getElementById(id).classList.add('active');
}

window.selectEvent = async function (eventId) {
    await selectEvent(eventId, async (id) => {
        showSection('section-seats');
        await loadSectors(id, async (sector, animate) => {
            await loadSeatMap(sector, animate);
        });
        startPolling();
    });
};

window.selectSector = async function (sectorId) {
    const { currentSector } = getReserveData();
    await selectSector(sectorId, currentSector, async (sector, animate) => {
        await loadSeatMap(sector, animate);
    });
};

document.getElementById('btn-back').addEventListener('click', () => {
    stopPolling();
    stopReservationTimer();
    showSection('section-events');
});

document.getElementById('btn-cancel').addEventListener('click', () => {
    handleCancelReservation();
});

document.getElementById('btn-reserve').addEventListener('click', async () => {
    const { selectedSeatId, currentSeatData, currentSector, allSeats } = getReserveData();
    if (!selectedSeatId) return;

    const btn = document.getElementById('btn-reserve');
    btn.disabled = true;
    btn.textContent = 'Reservando...';

    const seatBtn = document.querySelector(`[data-seat-id="${selectedSeatId}"]`);
    const seatIdToReserve = selectedSeatId;
    setReservedSeat(selectedSeatId);

    try {
        const reservation = await reserveSeat(selectedSeatId, DEFAULT_USER_ID);

        if (seatBtn) {
            seatBtn.innerHTML = seatSVG('#f59e0b');
            seatBtn.disabled = true;
        }

        updateSeatStatus(seatIdToReserve, 'Reserved');

        const reservedSeat = currentSeatData;
        const reservedSector = currentSector;

        updatePanelReservation(reservedSeat);

        showPaymentSection(reservation, reservedSeat, reservedSector);
        showReservationModal();

        document.getElementById('modal-close-btn').onclick = () => {
            closeReservationModal();
        };

    } catch (error) {
        if (error.status === 409) {
            showAlert('⚠️ Otro usuario reservó esta butaca. El mapa se actualizó.', 'warning');
        } else {
            showAlert(MESSAGES.RESERVE_ERROR || 'Error al reservar', 'error');
        }
        await loadSeatMap(getReserveData().currentSector, false);
    } finally {
        btn.disabled = false;
        btn.textContent = 'Reservar ahora';
    }
});

initPaymentSection(
    () => {
        stopPolling();
        showSection('section-events');
        loadEvents();
    },
    () => showSection('section-seats')
);

initializeModal();
loadEvents();