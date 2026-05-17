// ─── Alertas ───────────────────────────────────────────────

function getOrCreateAlertContainer() {
    let container = document.getElementById('alert-container');
    if (!container) {
        container = document.createElement('div');
        container.id = 'alert-container';
        container.style.cssText = 'position:fixed;top:1rem;right:1rem;z-index:9999;display:flex;flex-direction:column;gap:8px;';
        document.body.appendChild(container);
    }
    return container;
}

export function showAlert(message, type) {
    const container = getOrCreateAlertContainer();
    const alert = document.createElement('div');
    alert.className = `alert alert-${type}`;
    alert.textContent = message;
    container.appendChild(alert);

    const delay = type === 'error' ? 5000 : 3000;
    setTimeout(() => alert.remove(), delay);
}

export function showMessage(message, type = 'info') {
    if (!message || typeof message !== 'string') return;
    const cleanMessage = message.trim();
    if (!cleanMessage) return;
    const validTypes = ['success', 'error', 'warning', 'info'];
    const normalizedType = validTypes.includes(type) ? type : 'info';
    showAlert(cleanMessage, normalizedType);
}

export function showError(message) {
    showMessage(message, 'error');
}

export function showSuccess(message) {
    showMessage(message, 'success');
}

export function showWarning(message) {
    showMessage(message, 'warning');
}

// ─── Formato ───────────────────────────────────────────────

// Ej: 55000 → "$55.000"
export function formatPrice(price) {
    return new Intl.NumberFormat('es-AR', {
        style: 'currency',
        currency: 'ARS',
        minimumFractionDigits: 0
    }).format(price);
}

// Ej: "2026-05-23" → "23 de mayo de 2026"
export function formatDate(dateString) {
    return new Date(dateString).toLocaleDateString('es-AR', {
        day: 'numeric',
        month: 'long',
        year: 'numeric'
    });
}

// ─── Seguridad ─────────────────────────────────────────────

// Previene XSS escapando caracteres especiales
export function escapeHtml(str) {
    return String(str)
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#39;");
}

// ─── UI ────────────────────────────────────────────────────

// Muestra u oculta el spinner de carga
export function setLoading(elementId, isLoading) {
    const el = document.getElementById(elementId);
    if (!el) return;
    el.style.display = isLoading ? 'block' : 'none';
}

// Trunca texto largo
export function truncate(text, maxLength) {
    if (typeof text !== 'string') return '';
    return text.length > maxLength
        ? text.slice(0, maxLength) + '…'
        : text;
}

export function showReservationModal() {
    const modal = document.getElementById('reservation-modal');

    if (modal) {
        modal.classList.remove('hidden');
    }
}

export function closeReservationModal() {
    const modal = document.getElementById('reservation-modal');
    modal.classList.add('hidden');
}

export function initializeModal() {
    const modal = document.getElementById('reservation-modal');
    const closeBtn = document.getElementById('modal-close-btn');

    if (!modal || !closeBtn) return;

    closeBtn.addEventListener('click', () => {
        modal.classList.add('hidden');
    });

    modal.addEventListener('click', (e) => {
        if (e.target === modal) {
            modal.classList.add('hidden');
        }
    });
}