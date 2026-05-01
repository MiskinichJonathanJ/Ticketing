import { getEvents } from '../config/api.js';
import { appStore } from '../appStore.js';
import { showError, formatDate, escapeHtml } from '../utils/helpers.js';
import { MESSAGES, SELECTORS } from '../config/constants.js';
import { renderSectorList } from './SectorList.js';
import { renderSeatMap } from './SeatMap.js';


export async function renderEventList() {
    const container = document.getElementById(SELECTORS.EVENTS_CONTAINER);
    if (!container) return;

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
                        📅 ${formatDate(event.eventDate)} · 📍 ${escapeHtml(event.venue)}
                    </div>
                    <span class="badge-active">${escapeHtml(event.status)}</span>
                </div>
                <button class="btn-primary" onclick="selectEvent(${event.id})">
                    Ver asientos
                </button>
            </div>
        `).join('');

    } catch (error) {
        showError(MESSAGES.LOAD_EVENTS_ERROR);
        container.innerHTML = `<p class="error-msg">${MESSAGES.LOAD_EVENTS_ERROR}</p>`;
    }
}

export function selectEvent(eventId) {
    const events = appStore.getState('events');
    const event = events.find(e => e.id === eventId);
    if (!event) return;

    appStore.setState('currentEvent', event);
    document.dispatchEvent(new CustomEvent('eventSelected', { detail: event }));
}