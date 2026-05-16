import { getSectors } from '../config/api.js';
import { appStore } from '../appStore.js';
import { formatPrice, escapeHtml, showAlert } from '../utils/helpers.js';
import { MESSAGES } from '../config/constants.js';

// ── Carga y renderiza los sectores ────────────────────────────
export async function loadSectors(eventId, onSectorSelected) {
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
            await onSectorSelected(sectors[0], false);
        }

    } catch (error) {
        showAlert(MESSAGES.LOAD_SECTORS_ERROR, 'error');
        container.innerHTML = `<p class="error-msg">${MESSAGES.LOAD_SECTORS_ERROR}</p>`;
    }
}

// ── Selecciona un sector ──────────────────────────────────────
export async function selectSector(sectorId, currentSector, onSectorSelected) {
    const sectors = appStore.getState('sectors');
    const sector = sectors.find(s => s.id === sectorId);
    if (!sector || (currentSector && currentSector.id === sectorId)) return;

    // Animación pulse
    document.querySelectorAll('.sector-card').forEach(c => c.classList.remove('active'));
    const card = document.getElementById(`sector-card-${sectorId}`);
    card.classList.add('active', 'pulse');
    setTimeout(() => card.classList.remove('pulse'), 300);

    await onSectorSelected(sector, true);
}