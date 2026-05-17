import { processPayment } from '../config/api.js';
import { appStore } from '../appStore.js';
import { showAlert } from '../utils/helpers.js';
import { DEFAULT_USER_ID, MESSAGES } from '../config/constants.js';

// Procesa el pago de una reserva activa
export async function tryProcessPayment(reservationId) {
    try {
        const result = await processPayment(reservationId, DEFAULT_USER_ID);
        showAlert('✅ ¡Pago exitoso! Tu entrada fue confirmada.', 'success');
        return result;
    } catch (error) {
        if (error.status === 409) {
            showAlert('⚠️ La reserva expiró o ya fue pagada.', 'warning');
        } else if (error.status === 404) {
            showAlert('⚠️ Reserva no encontrada.', 'error');
        } else {
            showAlert('Error al procesar el pago. Intentá de nuevo.', 'error');
        }
        return null;
    }
}