// Horarios de trabajo definidos
const workingSlots = [
    '08:00-09:30',
    '09:45-11:15',
    '13:00-14:30',
    '14:45-16:15',
    '18:00-19:30',
    '19:45-21:15'
];

// Base de datos de horarios ocupados
const occupiedSchedules = {
    // Lunes
    '1': {
        '1': [{ start: '08:00', end: '09:30' }],
        '2': [],
        '3': [{ start: '08:00', end: '09:30' }],
        '4': [{ start: '09:45', end: '11:15' }]
    },
    // Martes
    '2': {
        '1': [],
        '2': [{ start: '14:45', end: '16:15' }],
        '3': [{ start: '13:00', end: '14:30' }],
        '4': [{ start: '13:00', end: '14:30' }]
    },
    // Miércoles
    '3': {
        '1': [{ start: '09:45', end: '11:15' }],
        '2': [{ start: '08:00', end: '09:30' }, { start: '18:00', end: '19:30' }],
        '3': [],
        '4': [{ start: '14:45', end: '16:15' }]
    },
    // Jueves
    '4': {
        '1': [{ start: '14:45', end: '16:15' }],
        '2': [],
        '3': [{ start: '08:00', end: '09:30' }, { start: '19:45', end: '21:15' }],
        '4': [{ start: '09:45', end: '11:15' }]
    },
    // Viernes
    '5': {
        '1': [{ start: '18:00', end: '19:30' }],
        '2': [{ start: '09:45', end: '11:15' }],
        '3': [{ start: '14:45', end: '16:15' }],
        '4': []
    },
    // Sábado
    '6': {
        '1': [],
        '2': [{ start: '09:45', end: '11:15' }],
        '3': [],
        '4': [{ start: '09:45', end: '11:15' }]
    },
    // Domingo
    '0': {
        '1': [],
        '2': [],
        '3': [],
        '4': []
    }
};

// Variables globales
let selectedLabId = null;
let selectedTimeSlot = null;
let selectedGlobalTime = null;
let currentDate = new Date();

// Inicialización
document.addEventListener('DOMContentLoaded', function () {
    // Establecer fecha actual
    const today = new Date().toISOString().split('T')[0];
    document.getElementById('globalDate').value = today;

    // Event listeners para horarios globales
    const globalTimeSlots = document.querySelectorAll('.global-time-slot');
    globalTimeSlots.forEach(slot => {
        slot.addEventListener('click', function () {
            selectGlobalTimeSlot(this);
        });
    });

    // Actualizar al cambiar la fecha
    document.getElementById('globalDate').addEventListener('change', function () {
        updateGlobalTimeSlots();
        updateAllLabsStatus();
        clearTimeSelection();
    });

    // Event listeners de laboratorios
    const labCards = document.querySelectorAll('.lab-card');
    labCards.forEach(card => {
        card.addEventListener('click', function () {
            selectLab(this);
        });
    });

    // Event listeners de time slots en las tarjetas
    document.addEventListener('click', function (e) {
        if (e.target.classList.contains('time-slot-preview')) {
            const time = e.target.dataset.time;
            const globalSlot = document.querySelector(`.global-time-slot[data-time="${time}"]`);
            if (globalSlot) {
                selectGlobalTimeSlot(globalSlot);
            }
        }
    });

    // Filtros
    const filterButtons = document.querySelectorAll('.filter-btn');
    filterButtons.forEach(btn => {
        btn.addEventListener('click', function () {
            filterButtons.forEach(b => b.classList.remove('active'));
            this.classList.add('active');
            filterLabs(this.dataset.filter);
        });
    });

    // Modal controls
    const quickReserveBtn = document.getElementById('quickReserveBtn');
    const reservationModal = document.getElementById('reservationModal');
    const closeModal = document.getElementById('closeModal');
    const reservationForm = document.getElementById('reservationForm');

    quickReserveBtn.addEventListener('click', openReservationModal);
    closeModal.addEventListener('click', closeReservationModal);
    reservationModal.addEventListener('click', function (e) {
        if (e.target === reservationModal) closeReservationModal();
    });

    reservationForm.addEventListener('submit', handleFormSubmit);

    // Actualizar estado inicial
    updateGlobalTimeSlots();
    updateAllLabsStatus();
});

function updateGlobalTimeSlots() {
    const dateInput = document.getElementById('globalDate');
    const selectedDate = new Date(dateInput.value);
    const dayOfWeek = selectedDate.getDay();
    const dayKey = dayOfWeek === 0 ? '0' : dayOfWeek.toString();

    const globalTimeSlots = document.querySelectorAll('.global-time-slot');
    globalTimeSlots.forEach(slot => {
        const time = slot.dataset.time;
        const occupiedCount = getOccupiedLabsCount(time, dayKey);
        const labCountElement = slot.querySelector('.lab-count');

        if (occupiedCount === 0) {
            labCountElement.textContent = 'Todos libres';
            labCountElement.style.color = 'var(--available)';
        } else if (occupiedCount === 4) {
            labCountElement.textContent = 'Todos ocupados';
            labCountElement.style.color = 'var(--occupied)';
        } else {
            labCountElement.textContent = `${occupiedCount} ocupados`;
            labCountElement.style.color = 'var(--occupied)';
        }
    });
}

function getOccupiedLabsCount(time, dayKey) {
    let count = 0;
    for (let labId = 1; labId <= 4; labId++) {
        const occupiedTimes = occupiedSchedules[dayKey][labId];
        if (isTimeSlotOccupied(time, occupiedTimes)) {
            count++;
        }
    }
    return count;
}

function selectGlobalTimeSlot(slot) {
    // Remover selección previa
    document.querySelectorAll('.global-time-slot').forEach(s => s.classList.remove('selected'));

    // Seleccionar nuevo slot
    slot.classList.add('selected');
    selectedGlobalTime = slot.dataset.time;

    // Actualizar indicador
    const timeIndicator = document.getElementById('timeIndicator');
    timeIndicator.classList.add('active');
    timeIndicator.innerHTML = `
        <i class="fas fa-clock"></i>
        <span>Horario: ${selectedGlobalTime}</span>
    `;

    // Aplicar cambios a TODOS los laboratorios simultáneamente
    highlightAllOccupiedLabs(selectedGlobalTime);

    // Actualizar previews en TODAS las tarjetas
    updateAllTimeSlotPreviews(selectedGlobalTime);

    // Mostrar notificación
    const dateInput = document.getElementById('globalDate');
    const selectedDate = new Date(dateInput.value);
    const dayName = selectedDate.toLocaleDateString('es-ES', { weekday: 'long' });
    const occupiedCount = getOccupiedLabsCount(selectedGlobalTime, selectedDate.getDay().toString());

    if (occupiedCount > 0) {
        showToast('Horario Seleccionado', `${occupiedCount} laboratorios ocupados el ${dayName} a las ${selectedGlobalTime}`, 'error');
    } else {
        showToast('Horario Disponible', 'Todos los laboratorios libres en este horario', 'success');
    }
}

function highlightAllOccupiedLabs(time) {
    const dateInput = document.getElementById('globalDate');
    const selectedDate = new Date(dateInput.value);
    const dayOfWeek = selectedDate.getDay();
    const dayKey = dayOfWeek === 0 ? '0' : dayOfWeek.toString();

    const labCards = document.querySelectorAll('.lab-card');
    labCards.forEach(card => {
        const labId = card.dataset.labId;
        const occupiedTimes = occupiedSchedules[dayKey][labId];
        const header = card.querySelector('.lab-header');

        // Remover clases previas
        card.classList.remove('occupied-at-time');
        header.classList.remove('occupied-at-selected-time');

        // Verificar si está ocupado en el horario seleccionado
        if (isTimeSlotOccupied(time, occupiedTimes)) {
            card.classList.add('occupied-at-time');
            header.classList.add('occupied-at-selected-time');
        }
    });
}

function updateAllTimeSlotPreviews(selectedTime) {
    const dateInput = document.getElementById('globalDate');
    const selectedDate = new Date(dateInput.value);
    const dayOfWeek = selectedDate.getDay();
    const dayKey = dayOfWeek === 0 ? '0' : dayOfWeek.toString();

    // Actualizar TODAS las tarjetas de laboratorio
    for (let labId = 1; labId <= 4; labId++) {
        const occupiedTimes = occupiedSchedules[dayKey][labId];
        const slotsPreview = document.querySelector(`#lab${labId}-slots`);

        // Limpiar y reconstruir previews
        slotsPreview.innerHTML = '';
        workingSlots.forEach(slot => {
            const slotElement = document.createElement('div');
            slotElement.className = 'time-slot-preview';
            slotElement.dataset.time = slot;

            if (isTimeSlotOccupied(slot, occupiedTimes)) {
                slotElement.classList.add('occupied');
            } else {
                slotElement.classList.add('available');
            }

            if (slot === selectedTime) {
                slotElement.classList.add('selected-time');
            }

            slotElement.textContent = slot;
            slotsPreview.appendChild(slotElement);
        });
    }
}

function updateAllLabsStatus() {
    const dateInput = document.getElementById('globalDate');
    const selectedDate = new Date(dateInput.value);
    const dayOfWeek = selectedDate.getDay();
    const dayKey = dayOfWeek === 0 ? '0' : dayOfWeek.toString();

    const labCards = document.querySelectorAll('.lab-card');
    labCards.forEach(card => {
        const labId = card.dataset.labId;
        const occupiedTimes = occupiedSchedules[dayKey][labId];
        const header = card.querySelector('.lab-header');
        const statusDot = card.querySelector('.status-dot');
        const statusText = card.querySelector('.lab-status span:last-child');
        const slotsPreview = card.querySelector(`#lab${labId}-slots`);

        // No actualizar si hay un tiempo global seleccionado
        if (selectedGlobalTime) {
            return;
        }

        // Limpiar slots previos
        slotsPreview.innerHTML = '';

        // Determinar estado - simplificado a solo dos estados
        if (occupiedTimes.length === 0) {
            header.className = 'lab-header';
            statusDot.className = 'status-dot available';
            statusText.textContent = 'Disponible';
        } else {
            header.className = 'lab-header occupied';
            statusDot.className = 'status-dot occupied';
            statusText.textContent = 'Ocupado';
        }

        // Mostrar preview de horarios
        workingSlots.forEach(slot => {
            const slotElement = document.createElement('div');
            slotElement.className = 'time-slot-preview';
            slotElement.dataset.time = slot;

            if (isTimeSlotOccupied(slot, occupiedTimes)) {
                slotElement.classList.add('occupied');
            } else {
                slotElement.classList.add('available');
            }

            slotElement.textContent = slot;
            slotsPreview.appendChild(slotElement);
        });
    });
}

function clearTimeSelection() {
    selectedGlobalTime = null;
    document.querySelectorAll('.global-time-slot').forEach(s => s.classList.remove('selected'));

    const timeIndicator = document.getElementById('timeIndicator');
    timeIndicator.classList.remove('active');
    timeIndicator.innerHTML = `
        <i class="fas fa-info-circle"></i>
        <span>Selecciona un horario para ver disponibilidad</span>
    `;

    // Limpiar resaltados de TODOS los laboratorios
    document.querySelectorAll('.lab-card').forEach(card => {
        card.classList.remove('occupied-at-time');
        card.querySelector('.lab-header').classList.remove('occupied-at-selected-time');
    });

    updateAllLabsStatus();
    updateQuickReserveButton();
}

function selectLab(card) {
    const labId = card.dataset.labId;
    const dateInput = document.getElementById('globalDate');
    const selectedDate = new Date(dateInput.value);
    const dayOfWeek = selectedDate.getDay();
    const dayKey = dayOfWeek === 0 ? '0' : dayOfWeek.toString();

    const occupiedTimes = occupiedSchedules[dayKey][labId];

    // Simplificado: si tiene algún horario ocupado, se considera ocupado
    if (occupiedTimes.length > 0 && occupiedTimes.length >= workingSlots.length) {
        showToast('Laboratorio Ocupado', 'Este laboratorio está ocupado todo el día', 'error');
        return;
    }

    // Remover selección previa
    document.querySelectorAll('.lab-card').forEach(c => c.classList.remove('selected'));

    // Seleccionar nuevo laboratorio
    card.classList.add('selected');
    selectedLabId = labId;

    showToast('Laboratorio Seleccionado', `Laboratorio #${labId} seleccionado`, 'success');
    updateQuickReserveButton();
}

function updateQuickReserveButton() {
    const quickReserveBtn = document.getElementById('quickReserveBtn');

    if (selectedLabId && selectedGlobalTime) {
        quickReserveBtn.disabled = false;
    } else {
        quickReserveBtn.disabled = true;
    }
}

function openReservationModal() {
    if (!selectedLabId || !selectedGlobalTime) {
        showToast('Información Incompleta', 'Por favor selecciona un laboratorio y un horario', 'error');
        return;
    }

    const modal = document.getElementById('reservationModal');
    const dateInput = document.getElementById('globalDate');
    const selectedDate = new Date(dateInput.value);

    // Actualizar resumen
    document.getElementById('summaryLab').textContent = `Laboratorio #${selectedLabId}`;
    document.getElementById('summaryDate').textContent = selectedDate.toLocaleDateString('es-ES', {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
    document.getElementById('summaryTime').textContent = selectedGlobalTime;

    // Limpiar formulario
    document.getElementById('motivos').value = '';

    // Mostrar modal
    modal.classList.add('show');
}

function closeReservationModal() {
    const modal = document.getElementById('reservationModal');
    modal.classList.remove('show');
}

function handleFormSubmit(e) {
    e.preventDefault();

    const motivos = document.getElementById('motivos').value.trim();
    if (!motivos) {
        showToast('Error', 'Por favor describe el motivo de la reserva', 'error');
        return;
    }

    // Simular reserva exitosa
    showToast('Reserva Exitosa', `Laboratorio #${selectedLabId} reservado para ${selectedGlobalTime}`, 'success');

    // Cerrar modal y resetear después de 2 segundos
    setTimeout(() => {
        closeReservationModal();
        resetSelection();
    }, 2000);
}

function resetSelection() {
    // Limpiar selecciones
    document.querySelectorAll('.lab-card').forEach(card => card.classList.remove('selected'));
    document.querySelectorAll('.global-time-slot').forEach(slot => slot.classList.remove('selected'));

    selectedLabId = null;
    selectedGlobalTime = null;

    // Resetear indicador
    const timeIndicator = document.getElementById('timeIndicator');
    timeIndicator.classList.remove('active');
    timeIndicator.innerHTML = `
        <i class="fas fa-info-circle"></i>
        <span>Selecciona un horario para ver disponibilidad</span>
    `;

    // Actualizar UI
    updateAllLabsStatus();
    updateQuickReserveButton();
}

function filterLabs(filter) {
    const labCards = document.querySelectorAll('.lab-card');

    labCards.forEach(card => {
        const statusDot = card.querySelector('.status-dot');

        if (filter === 'all') {
            card.style.display = 'block';
        } else if (filter === 'available') {
            card.style.display = statusDot.classList.contains('available') ? 'block' : 'none';
        } else if (filter === 'occupied') {
            card.style.display = statusDot.classList.contains('occupied') ? 'block' : 'none';
        }
    });
}

function isTimeSlotOccupied(slotTime, occupiedTimes) {
    const [slotStart, slotEnd] = slotTime.split('-');
    const slotStartMinutes = timeToMinutes(slotStart);
    const slotEndMinutes = timeToMinutes(slotEnd);

    return occupiedTimes.some(occupied => {
        const occupiedStart = timeToMinutes(occupied.start);
        const occupiedEnd = timeToMinutes(occupied.end);

        return (slotStartMinutes < occupiedEnd && slotEndMinutes > occupiedStart);
    });
}

function timeToMinutes(time) {
    const [hours, minutes] = time.split(':').map(Number);
    return hours * 60 + minutes;
}

function showToast(title, message, type) {
    const toast = document.getElementById('toast');
    const toastIcon = document.getElementById('toastIcon');
    const toastTitle = document.getElementById('toastTitle');
    const toastMessage = document.getElementById('toastMessage');

    toastTitle.textContent = title;
    toastMessage.textContent = message;

    toastIcon.className = `toast-icon ${type}`;
    if (type === 'success') {
        toastIcon.innerHTML = '<i class="fas fa-check"></i>';
    } else if (type === 'error') {
        toastIcon.innerHTML = '<i class="fas fa-times"></i>';
    }

    toast.classList.add('show');

    setTimeout(() => {
        toast.classList.remove('show');
    }, 3000);
}