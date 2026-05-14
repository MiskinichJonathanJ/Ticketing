import { reserveSeat } from '../config/api.js';
import { appStore } from '../appStore.js';
import { showSuccess, showError, showWarning } from '../utils/helpers.js';
import { MESSAGES } from '../config/constants.js';
import { showReservationModal } from '../utils/helpers.js';

export async function tryReserveSeat(seatId) {
    const user = appStore.getState('currentUser');

   
    appStore.setState('isLoading', true);

    try {
        const reservation = await reserveSeat(seatId, user.id);

        appStore.setState('currentReservation', reservation);

        showSuccess(MESSAGES.RESERVE_SUCCESS);

        return reservation;

    } catch (error) {
        
        if (error.status === 409) {
            showWarning(MESSAGES.SEAT_NOT_AVAILABLE);
            return null;
        }
        if (error.status === 404) {
            showError(MESSAGES.SEAT_NOT_FOUND);
            return null;
        }

        showError(MESSAGES.RESERVE_ERROR);
        return null;

    } finally {
   
        appStore.setState('isLoading', false);
    }
}