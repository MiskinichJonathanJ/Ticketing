import http from 'k6/http';
import { check } from 'k6';
import { Counter } from 'k6/metrics';

// ── Contadores personalizados ─────────────────────────────────
const success201 = new Counter('reservations_created');
const conflict409 = new Counter('reservations_conflict');
const otherErrors = new Counter('reservations_other_error');

// ── Configuración: 10 VUs simultáneos, 1 iteración cada uno ──
export const options = {
    vus: 10,
    iterations: 10,
    // Todos arrancan al mismo tiempo
    startTime: '0s',
};

// ── CONFIGURAR ANTES DE CORRER ────────────────────────────────
const BASE_URL = 'http://localhost:5000/api/v1';
    const SEAT_ID = 'a0000001-0000-0000-0000-000000000010'; // Sector 1, butaca 1

export default function () {
    const USER_ID = 1; // Genera un ID de usuario único por VU e iteración
    const payload = JSON.stringify({
        seatId: SEAT_ID,
        userId: USER_ID,
    });

    const params = {
        headers: { 'Content-Type': 'application/json' },
    };

    const res = http.post(`${BASE_URL}/reservations`, payload, params);

    if (res.status === 201) {
        success201.add(1);
        check(res, { '201 Created — reserva exitosa': (r) => r.status === 201 });
    } else if (res.status === 409) {
        conflict409.add(1);
        check(res, { '409 Conflict — concurrencia correcta': (r) => r.status === 409 });
    } else {
        otherErrors.add(1);
        console.error(`Status inesperado: ${res.status} — ${res.body}`);
    }
}

export function handleSummary(data) {
    const created = data.metrics.reservations_created?.values?.count ?? 0;
    const conflict = data.metrics.reservations_conflict?.values?.count ?? 0;
    const errors = data.metrics.reservations_other_error?.values?.count ?? 0;

    console.log('\n══════════════════════════════════════');
    console.log('  RESULTADO PRUEBA DE CONCURRENCIA');
    console.log('══════════════════════════════════════');
    console.log(`  ✅ 201 Created  : ${created}  (debe ser exactamente 1)`);
    console.log(`  ⚠️  409 Conflict : ${conflict} (debe ser ${10 - created})`);
    console.log(`  ❌ Otros errores: ${errors}`);
    console.log('══════════════════════════════════════\n');

    if (created !== 1) {
        console.error('FALLO: Se esperaba exactamente 1 reserva exitosa.');
    } else {
        console.log('ÉXITO: Concurrencia optimista funcionando correctamente.');
    }

    return {};
}