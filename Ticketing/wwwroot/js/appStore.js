import { DEFAULT_USER_ID } from './config/constants.js';

class AppStore {
    #state = {
        // Datos
        events: [],
        sectors: [],
        seats: [],

        // Selecciones del usuario
        currentEvent: null,
        currentSector: null,
        currentSeat: null,
        currentReservation: null,

        // Usuario logueado
        currentUser: {
            id: DEFAULT_USER_ID,
            name: 'Usuario Test',
            role: 'Client'
        },

        // UI
        isLoading: false,
        currentError: null
    };

    #listeners = new Map();

    // Obtener un valor del store
    getState(key) {
        if (!key) return { ...this.#state };
        if (!(key in this.#state)) return undefined;
        return this.#state[key];
    }

    // Actualizar un valor del store
    setState(key, value) {
        if (!(key in this.#state)) return;
        const oldValue = this.#state[key];
        if (oldValue === value) return;
        this.#state[key] = value;
        this.#notifyListeners(key, value, oldValue);
    }

    // Actualizar múltiples valores
    updateState(updates) {
        if (!updates || typeof updates !== 'object') return;
        Object.entries(updates).forEach(([key, value]) => {
            this.setState(key, value);
        });
    }

    // Suscribirse a cambios de un valor
    // Devuelve una función para desuscribirse
    subscribe(key, callback) {
        if (typeof callback !== 'function') return () => { };
        if (!(key in this.#state)) return () => { };
        if (!this.#listeners.has(key)) {
            this.#listeners.set(key, new Set());
        }
        this.#listeners.get(key).add(callback);
        return () => {
            const listeners = this.#listeners.get(key);
            if (listeners) listeners.delete(callback);
        };
    }

    // Resetear selección al volver atrás
    resetSelection() {
        this.updateState({
            currentSector: null,
            currentSeat: null,
            currentReservation: null,
            sectors: [],
            seats: []
        });
    }

    // Resetear todo el store
    reset() {
        this.updateState({
            events: [],
            sectors: [],
            seats: [],
            currentEvent: null,
            currentSector: null,
            currentSeat: null,
            currentReservation: null,
            isLoading: false,
            currentError: null
        });
    }

    // Notificar a los listeners de un cambio
    #notifyListeners(key, newValue, oldValue) {
        const listeners = this.#listeners.get(key);
        if (!listeners || listeners.size === 0) return;
        listeners.forEach(callback => {
            try {
                callback(newValue, oldValue);
            } catch (error) {
                console.error('Error en listener:', error);
            }
        });
    }
}

export const appStore = new AppStore();