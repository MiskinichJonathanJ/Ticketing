import { BASE_URL, MESSAGES } from './constants.js';


const USE_MOCKS = true;

// Datos falsos
const mockEvents = [
    {
        id: 1,
        name: "Concierto de Rock",
        eventDate: "2026-05-10T20:00:00",
        venue: "Estadio River Plate",
        status: "Activo"
    },
    {
        id: 2,
        name: "Obra de Teatro",
        eventDate: "2026-05-15T21:00:00",
        venue: "Teatro Colon",
        status: "Disponible"
    }
];

const mockSectors = {
    1: [
        {
            id: 1,
            name: "VIP",
            price: 8000,
            capacity: 50
        },
        {
            id: 2,
            name: "General",
            price: 4000,
            capacity: 100
        }
    ],
    2: [
        {
            id: 3,
            name: "Platea",
            price: 6000,
            capacity: 100
        }
    ]
};

const mockSeats = {
    1: generateSeats("VIP"),
    2: generateSeats("Platea")
};

function generateSeats(sectorName) {
    const seats = [];
    const rows = ["A", "B", "C", "D", "E"]; // 5 filas
    let id = 1;

    rows.forEach(row => {
        for (let i = 1; i <= 10; i++) { // 10 por fila → 50 total
            seats.push({
                id: id++,
                sectorName,
                rowIdentifier: row,
                seatNumber: i,
                status: "AVAILABLE"
            });
        }
    });

    return seats;
}




// Configuración de la API
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

// Construye la URL completa
export function buildApiUrl(endpoint, params = '') {
    return `${API_CONFIG.BASE_URL}${endpoint}${params}`;
}

// Error personalizado con código HTTP
class ApiError extends Error {
    constructor(status, message) {
        super(message);
        this.name = 'ApiError';
        this.status = status;
    }
}

// Extrae el mensaje de error de la respuesta
async function getErrorMessage(response) {
    try {
        const data = await response.json();
        return data.message || data.error || `Error ${response.status}`;
    } catch {
        return `Error ${response.status}: ${response.statusText}`;
    }
}

// Normaliza cualquier error en un formato consistente
function normalizeError(error) {
    if (error.name === 'AbortError') {
        return new Error('La petición tardó demasiado tiempo');
    }
    if (error instanceof ApiError) {
        return error;
    }
    return new Error(error.message || MESSAGES.CONNECTION_ERROR);
}

// Reintenta la petición con espera exponencial
async function retryWithBackoff(fn, retries = 3, delay = 500) {
    try {
        return await fn();
    } catch (error) {
        if (retries <= 0) throw error;
        await new Promise(res => setTimeout(res, delay));
        return retryWithBackoff(fn, retries - 1, delay * 2);
    }
}

// Fetch con timeout
async function fetchWithTimeout(url, config) {
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), config.timeout);
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

// Función base para todos los requests
export const apiRequest = async (url, options = {}) => {
    const config = {
        timeout: 10000,
        retries: 3,
        retryDelay: 1000,
        ...options
    };
    return retryWithBackoff(
        () => fetchWithTimeout(url, config),
        config.retries,
        config.retryDelay
    );
};

export async function getEvents() {
    if (USE_MOCKS) {
        return Promise.resolve(mockEvents);
    }
    return apiRequest(buildApiUrl(API_CONFIG.ENDPOINTS.EVENTS));
}

export async function getSectors(eventId) {
    if (USE_MOCKS) {
        return Promise.resolve(mockSectors[eventId] || []);
    }
    return apiRequest(buildApiUrl(API_CONFIG.ENDPOINTS.EVENTS, `/${eventId}/sectors`));
}

export async function getSeats(eventId) {
    if (USE_MOCKS) {
        return Promise.resolve(mockSeats[eventId] || []);
    }
    return apiRequest(buildApiUrl(API_CONFIG.ENDPOINTS.EVENTS, `/${eventId}/seats`));
}

export async function reserveSeat(seatId, userId) {
    if (USE_MOCKS) {
        return Promise.resolve({
            success: true,
            message: "Reserva simulada OK",
            seatId,
            userId
        });
    }

    return apiRequest(buildApiUrl(API_CONFIG.ENDPOINTS.RESERVATIONS), {
        method: 'POST',
        body: JSON.stringify({ seatId, userId })
    });
}