import { getSectors } from '../config/api.js';
import { appStore } from '../appStore.js';
import { showError, formatPrice, formatDate, escapeHtml } from '../utils/helpers.js';
import { MESSAGES, SELECTORS } from '../config/constants.js';
import { renderSeatMap } from './SeatMap.js';

export async function renderSectorList() {
    const event = appStore.getState('currentEvent');
    if (!event) return;

    // Actualiza la barra de info del evento
    const eventBar = document.getElementById('event-bar');
    if (eventBar) {
        eventBar.innerHTML = `
            <div class="bar-item">
                <div class="bar-item-label">Evento</div>
                <div class="bar-item-value">${escapeHtml(event.name)}</div>
            </div>
            <div class="bar-item">
                <div class="bar-item-label">Fecha</div>
                <div class="bar-item-value">${formatDate(event.eventDate)}</div>
            </div>
            <div class="bar-item">
                <div class="bar-item-label">Lugar</div>
                <div class="bar-item-value">${escapeHtml(event.venue)}</div>
            </div>
        `;
    }

    const container = document.getElementById(SELECTORS.SECTORS_CONTAINER);
    if (!container) return;

    container.innerHTML = '<div class="spinner"></div>';

    try {
        const sectors = await getSectors(event.id);
        appStore.setState('sectors', sectors);

        if (sectors.length === 0) {
            container.innerHTML = '<p class="no-data">No hay sectores disponibles</p>';
            return;
        }

        container.innerHTML = `
            <div class="sector-grid">
                ${sectors.map(sector => `
                    <div class="sector-card" onclick="selectSector(${sector.id})">
                        <div class="sector-name">${escapeHtml(sector.name)}</div>
                        <div class="sector-price">${formatPrice(sector.price)}</div>
                        <div class="sector-cap">${sector.capacity} butacas</div>
                    </div>
                `).join('')}
            </div>
        `;

    } catch (error) {
        showError(MESSAGES.LOAD_SECTORS_ERROR);
        container.innerHTML = `<p class="error-msg">${MESSAGES.LOAD_SECTORS_ERROR}</p>`;
    }
}

export function selectSector(sectorId) {
    const sectors = appStore.getState('sectors');
    const sector = sectors.find(s => s.id === sectorId);
    if (!sector) return;

    appStore.setState('currentSector', sector);

    // solo actualiza el mapa
    renderSeatMap();
}