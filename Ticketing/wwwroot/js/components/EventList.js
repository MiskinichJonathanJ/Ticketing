import { getEvents } from '../config/api.js';
import { appStore } from '../appStore.js';
import { formatDate, escapeHtml, showAlert } from '../utils/helpers.js';
import { MESSAGES } from '../config/constants.js';

// ── Renderiza la lista de eventos ─────────────────────────────
export async function loadEvents() {
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
                        ${formatDate(event.eventDate)} ${escapeHtml(event.venue)}
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

// ── Selecciona un evento ──────────────────────────────────────
export async function selectEvent(eventId, onEventSelected) {
    const events = appStore.getState('events');
    const event = events.find(e => e.id === eventId);
    if (!event) return;

    appStore.setState('currentEvent', event);

    document.getElementById('event-bar').innerHTML = `
        <div><div class="bi-label">Evento</div><div class="bi-value">${escapeHtml(event.name)}</div></div>
        <div><div class="bi-label">Fecha</div><div class="bi-value">${formatDate(event.eventDate)}</div></div>
        <div><div class="bi-label">Lugar</div><div class="bi-value">${escapeHtml(event.venue)}</div></div>
    `;

    onEventSelected(eventId);
}