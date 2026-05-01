const port = window.location.port;
const base = window.location.origin;

export const BASE_URL = (port === '5118' || port === '5000' || port === '')
    ? `${base}/api/v1`
    : 'http://localhost:5118/api/v1';


export const RESERVATION_TIMEOUT = 5;


export const SELECTORS = {
    EVENTS_SECTION: 'section-events',
    SECTORS_SECTION: 'section-sectors',
    SEATS_SECTION: 'section-seats',
    EVENTS_CONTAINER: 'events-container',
    SECTORS_CONTAINER: 'sectors-container',
    SEATS_CONTAINER: 'seat-grid',
    RESERVATION_PANEL: 'reservation-panel',
    SELECTED_SEAT_INFO: 'selected-seat-info',
    RESERVE_BTN: 'btn-reserve',
    ALERT: 'alert-global',
    SECTOR_SUBTITLE: 'sector-subtitle',
    EVENT_NAME: 'event-name',
    EVENT_DATE: 'event-date',
    EVENT_VENUE: 'event-venue',
};


export const SEAT_STATUS = {
    AVAILABLE: 'Available',
    RESERVED: 'Reserved',
    SOLD: 'Sold'
};


export const MESSAGES = {
    
    RESERVE_SUCCESS: '¡Butaca reservada exitosamente!',

    
    LOAD_EVENTS_ERROR: 'Error al cargar los eventos',
    LOAD_SECTORS_ERROR: 'Error al cargar los sectores',
    LOAD_SEATS_ERROR: 'Error al cargar los asientos',
    SEAT_NOT_AVAILABLE: 'La butaca ya no está disponible',
    SEAT_NOT_FOUND: 'Butaca no encontrada',
    RESERVE_ERROR: 'Error al reservar la butaca',
    CONNECTION_ERROR: 'Error de conexión. Por favor intente más tarde.',
    UNEXPECTED_ERROR: 'Ocurrió un error inesperado'
};


export const DEFAULT_USER_ID = 1;

export default {
    BASE_URL,
    RESERVATION_TIMEOUT,
    SELECTORS,
    SEAT_STATUS,
    MESSAGES,
    DEFAULT_USER_ID
};;