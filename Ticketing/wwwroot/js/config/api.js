import { BASE_URL } from './constants.js';

// ══════════════════════════════════════════════
// MODO MOCK — datos falsos hasta que el back esté listo
// Cambiar a false cuando el backend esté funcionando
const USE_MOCK = false;
// ══════════════════════════════════════════════

// Genera 50 butacas por sector con algunos estados variados
function generateSeats(sectorName, sectorId) {
    const seats = [];
    for (let i = 1; i <= 50; i++) {
        const row = `F${Math.ceil(i / 10)}`;
        let status = 'Available';
        if ([3, 15, 27, 38, 44].includes(i)) status = 'Reserved';
        if ([7, 19, 31, 42, 48].includes(i)) status = 'Sold';
        seats.push({
            id: `${sectorId}-seat-${i}`,
            sectorName,
            rowIdentifier: row,
            seatNumber: i,
            status,
            price: sectorId === 1 ? 55000 : 110000
        });
    }
    return seats;
}

const MOCK_DATA = {
    events: [
        {
            id: 1,
            name: "Airbag - El Club de la Pelea II",
            eventDate: "2026-05-23T21:00:00",
            venue: "Estadio José Amalfitani",
            status: "Active"
        }
    ],
    sectors: [
        { id: 1, eventId: 1, name: "Campo", price: 55000, capacity: 50 },
        { id: 2, eventId: 1, name: "Platea", price: 110000, capacity: 50 }
    ],
    seats: [
        ...generateSeats("Campo", 1),
        ...generateSeats("Platea", 2)
    ]
};

// ── API Config ──────────────────────────────────────────────

export const API_CONFIG = {
    BASE_URL,
    ENDPOINTS: {
        EVENTS: '/events',
        RESERVATIONS: '/reservations'
    },
    HEADERS: {
        'Content-Type': 'application/json; charset=utf-8',
        'Accept': 'application/json; charset=utf-8'
    }
};

export function buildApiUrl(endpoint, params = '') {
    return `${API_CONFIG.BASE_URL}${endpoint}${params}`;
}

class ApiError extends Error {
    constructor(status, message) {
        super(message);
        this.name = 'ApiError';
        this.status = status;
    }
}

async function getErrorMessage(response) {
    try {
        const data = await response.json();
        return data.message || data.error || `Error ${response.status}`;
    } catch {
        return `Error ${response.status}: ${response.statusText}`;
    }
}

function normalizeError(error) {
    if (error.name === 'AbortError') return new Error('La petición tardó demasiado');
    if (error instanceof ApiError) return error;
    return new Error(error.message || 'Error de conexión');
}

async function fetchWithTimeout(url, config) {
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), 10000);
    try {
        const response = await fetch(url, {
            ...config,
            signal: controller.signal,
            headers: { ...API_CONFIG.HEADERS, ...config.headers }
        });
        clearTimeout(timeoutId);
        if (!response.ok) {
            const errorMsg = await getErrorMessage(response);
            throw new ApiError(response.status, errorMsg);
        }
        return await response.json();
    } catch (error) {
        clearTimeout(timeoutId);
        throw normalizeError(error);
    }
}

async function retryWithBackoff(fn, retries = 3, delay = 500) {
    try {
        return await fn();
    } catch (error) {
        if (retries <= 0) throw error;
        await new Promise(res => setTimeout(res, delay));
        return retryWithBackoff(fn, retries - 1, delay * 2);
    }
}

export const apiRequest = async (url, options = {}) => {
    const config = { timeout: 10000, retries: 3, retryDelay: 1000, ...options };
    return retryWithBackoff(() => fetchWithTimeout(url, config), config.retries, config.retryDelay);
};

// ── Endpoints ───────────────────────────────────────────────

export async function getEvents() {
    if (USE_MOCK) return MOCK_DATA.events;
    return apiRequest(buildApiUrl(API_CONFIG.ENDPOINTS.EVENTS));
}

export async function getSectors(eventId) {
    if (USE_MOCK) return MOCK_DATA.sectors.filter(s => s.eventId === eventId);
    return apiRequest(buildApiUrl(API_CONFIG.ENDPOINTS.EVENTS, `/${eventId}/sectors`));
}

export async function getSeats(eventId, sectorId) {
    if (USE_MOCK) return MOCK_DATA.seats;
    return apiRequest(
        buildApiUrl(API_CONFIG.ENDPOINTS.EVENTS, `/${eventId}/sectors/${sectorId}/seats`)
    );
}

export async function reserveSeat(seatId, userId) {
    if (USE_MOCK) {
        // Simulamos respuesta exitosa
        return {
            id: `res-${Date.now()}`,
            seatId,
            userId,
            status: 'Pending',
            reservedAt: new Date().toISOString(),
            expiresAt: new Date(Date.now() + 5 * 60 * 1000).toISOString()
        };
    }
    return apiRequest(buildApiUrl(API_CONFIG.ENDPOINTS.RESERVATIONS), {
        method: 'POST',
        body: JSON.stringify({ seatId, userId })
    });
}