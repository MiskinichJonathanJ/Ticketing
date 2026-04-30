import { renderEventList, selectEvent } from './components/EventList.js';
import { renderSectorList } from './components/SectorList.js';
import { renderSeatMap } from './components/SeatMap.js';
import { appStore } from './appStore.js';
import { selectSector } from './components/SectorList.js';

function showSection(sectionId) {
    document.querySelectorAll('.section').forEach(s => s.classList.remove('active'));
    document.getElementById(sectionId).classList.add('active');
}


document.getElementById('btn-back-sectors').addEventListener('click', () => {
    appStore.resetSelection();
    showSection('section-events');
});

document.getElementById('btn-back-seats').addEventListener('click', () => {
    appStore.setState('currentSeat', null);
    showSection('section-sectors');
});

document.getElementById('btn-cancel').addEventListener('click', () => {
    appStore.setState('currentSeat', null);
    showSection('section-sectors');
});


document.addEventListener('eventSelected', () => {
    renderSectorList();
    showSection('section-sectors');
});

document.addEventListener('sectorSelected', () => {
    renderSeatMap();
    showSection('section-seats');
});


window.selectEvent = selectEvent;
window.selectSector = selectSector;

renderEventList();