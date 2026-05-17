import { appStore } from '../appStore.js';
import { formatDate, formatPrice, escapeHtml, showAlert } from '../utils/helpers.js';
import { DEFAULT_USER_ID } from '../config/constants.js';
import { tryProcessPayment } from '../services/paymentService.js';
import { updateSeatStatus, seatSVG } from '../components/SeatMap.js';

let paymentTimer = null;
let currentReservationId = null;
let currentSectorForPayment = null;
let currentSeatId = null;

// ── Renderiza la sección de pagos ─────────────────────────────
export function showPaymentSection(reservation, seat, sector) {
    currentReservationId = reservation.id;
    currentSectorForPayment = sector;
    currentSeatId = seat.id; 

    const event = appStore.getState('currentEvent');

    // Actualizamos la barra del evento
    document.getElementById('payment-event-bar').innerHTML = `
        <div><div class="bi-label">Evento</div><div class="bi-value">${escapeHtml(event.name)}</div></div>
        <div><div class="bi-label">Fecha</div><div class="bi-value">${formatDate(event.eventDate)}</div></div>
        <div><div class="bi-label">Lugar</div><div class="bi-value">${escapeHtml(event.venue)}</div></div>
    `;

    // Llenamos el resumen
    document.getElementById('pay-event-name').textContent = event.name;
    document.getElementById('pay-sector-name').textContent = sector.name;
    document.getElementById('pay-seat-info').textContent =
        `Fila ${seat.rowIdentifier} · Butaca ${seat.seatNumber}`;
    document.getElementById('pay-price').textContent = formatPrice(sector.price);
    document.getElementById('pay-total').textContent = formatPrice(sector.price);
    document.getElementById('btn-pay').textContent =
        `Confirmar pago — ${formatPrice(sector.price)}`;
    document.getElementById('btn-pay').disabled = false;

    // Calculamos segundos restantes desde el ExpiresAt
    const expiresAt = new Date(reservation.expiresAt);
    const secondsLeft = Math.floor((expiresAt - new Date()) / 1000);

    startPaymentTimer(secondsLeft > 0 ? secondsLeft : 300);

    // Mostramos la sección
    showSection('section-payment');
}

// ── Timer de pago ─────────────────────────────────────────────
function startPaymentTimer(secondsLeft) {
    stopPaymentTimer();

    const timerEl = document.getElementById('payment-timer');
    const timerCard = document.getElementById('payment-timer-card');

    function updateDisplay() {
        const mins = Math.floor(secondsLeft / 60);
        const secs = secondsLeft % 60;
        timerEl.textContent = `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;

        // Rojo parpadeante cuando quedan menos de 60 segundos
        if (secondsLeft <= 60) {
            timerEl.classList.add('urgent');
            timerCard.style.background = '#fee2e2';
            timerCard.style.borderColor = '#dc2626';
        }
    }

    updateDisplay();

    paymentTimer = setInterval(() => {
        secondsLeft--;
        updateDisplay();

        if (secondsLeft === 60) {
            showAlert('⚠️ Tu reserva expira en 1 minuto. ¡Completá el pago!', 'warning');
        }

        if (secondsLeft <= 0) {
            stopPaymentTimer();
            showAlert('⏰ Tu reserva expiró. La butaca volvió a estar disponible.', 'error');

            const btnPay = document.getElementById('btn-pay');
            if (btnPay) {
                btnPay.disabled = true;
                btnPay.textContent = 'Reserva expirada';
            }

            setTimeout(() => {
                showSection('section-seats');

                // Butaca vuelve a azul
                const seatBtn = document.querySelector(`[data-seat-id="${currentSeatId}"]`);
                if (seatBtn) {
                    seatBtn.innerHTML = seatSVG('#1d4ed8');
                    seatBtn.disabled = false;
                }

                // Estado local → Available
                updateSeatStatus(currentSeatId, 'Available');

                // Panel reseteado
                const panelSeat = document.getElementById('panel-seat');
                if (panelSeat) {
                    panelSeat.textContent = 'Ninguna';
                    panelSeat.className = 'pempty';
                    panelSeat.style.color = '';
                }
                const panelPrice = document.getElementById('panel-price');
                if (panelPrice) panelPrice.style.display = 'none';
                const sumCount = document.getElementById('sum-count');
                if (sumCount) sumCount.textContent = '0 butacas';
                const sumPrice = document.getElementById('sum-price');
                if (sumPrice) sumPrice.textContent = '$0';
                const sumTotal = document.getElementById('sum-total');
                if (sumTotal) sumTotal.textContent = '$0';
                const btnReserve = document.getElementById('btn-reserve');
                if (btnReserve) {
                    btnReserve.disabled = true;
                    btnReserve.textContent = 'Reservar ahora';
                }

                // Timer card reseteado
                const timerCard = document.getElementById('payment-timer-card');
                if (timerCard) {
                    timerCard.style.background = '';
                    timerCard.style.borderColor = '';
                }
                const timerEl = document.getElementById('payment-timer');
                if (timerEl) {
                    timerEl.classList.remove('urgent');
                    timerEl.textContent = '05:00';
                }

            }, 3000);
        }
        }
    , 1000);
}

export function stopPaymentTimer() {
    if (paymentTimer) {
        clearInterval(paymentTimer);
        paymentTimer = null;
    }
}

// ── Inicializa los listeners de la sección de pagos ───────────
export function initPaymentSection(onPaymentSuccess, onCancel) {
    document.getElementById('btn-pay').addEventListener('click', async () => {
        const btn = document.getElementById('btn-pay');
        btn.disabled = true;
        btn.textContent = 'Procesando pago...';

        const result = await tryProcessPayment(currentReservationId);

        if (result) {
            stopPaymentTimer();
            btn.textContent = '✅ Pago confirmado';
            setTimeout(() => onPaymentSuccess(), 3000);
        } else {
            btn.disabled = false;
            btn.textContent = `Confirmar pago — ${formatPrice(currentSectorForPayment.price)}`;
        }
    });

    document.getElementById('btn-cancel-payment').addEventListener('click', () => {
        stopPaymentTimer();
        onCancel();
    });

    document.getElementById('btn-back-payment').addEventListener('click', () => {
        stopPaymentTimer();
        onCancel();
    });
}

// ── Helper local ──────────────────────────────────────────────
function showSection(id) {
    document.querySelectorAll('.section').forEach(s => s.classList.remove('active'));
    document.getElementById(id).classList.add('active');
}