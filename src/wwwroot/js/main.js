// Main JavaScript file for Urna Electrónica

document.addEventListener('DOMContentLoaded', function() {
    console.log('Urna Electrónica application loaded');
    
    // Initialize tooltips or other interactive elements if needed
    initializeEventListeners();
});

function initializeEventListeners() {
    // Add delete confirmation
    const deleteButtons = document.querySelectorAll('a[href*="/Delete/"]');
    deleteButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            if (!confirm('¿Está seguro de que desea eliminar este elemento?')) {
                e.preventDefault();
            }
        });
    });

    // Toggle password visibility
    const togglePasswordBtn = document.getElementById('togglePassword');
    if (togglePasswordBtn) {
        const passwordInput = document.getElementById('Contraseña');
        const eyeIcon = togglePasswordBtn.querySelector('.eye-icon');
        const eyeOffIcon = togglePasswordBtn.querySelector('.eye-off-icon');

        togglePasswordBtn.addEventListener('click', function(e) {
            e.preventDefault();
            const isPassword = passwordInput.type === 'password';
            passwordInput.type = isPassword ? 'text' : 'password';
            eyeIcon.style.display = isPassword ? 'none' : 'block';
            eyeOffIcon.style.display = isPassword ? 'block' : 'none';
        });
    }

    document.body.addEventListener('candidatoCreado', function (event) {
        const idEleccion = event?.detail?.idEleccion
            ?? event?.detail?.value?.idEleccion
            ?? event?.detail?.elt?.getAttribute?.('data-id-eleccion');
        const modalContainer = document.getElementById('modal-container');

        if (modalContainer) {
            modalContainer.innerHTML = '';
        }

        if (!idEleccion || typeof htmx === 'undefined') {
            return;
        }

        const targetSelector = `#candidatos-area-${idEleccion}`;
        if (document.querySelector(targetSelector)) {
            htmx.ajax('GET', `/Candidato/Listar/${idEleccion}`, {
                target: targetSelector,
                swap: 'innerHTML'
            });
        }
    });

    document.body.addEventListener('click', function (event) {
        const toggleButton = event.target.closest('.js-toggle-candidatos');
        if (!toggleButton) {
            return;
        }

        const idEleccion = toggleButton.getAttribute('data-eleccion-id');
        if (!idEleccion) {
            return;
        }

        const targetSelector = `#candidatos-area-${idEleccion}`;
        const target = document.querySelector(targetSelector);
        if (!target) {
            return;
        }

        const expanded = toggleButton.getAttribute('data-expanded') === 'true';
        const label = toggleButton.querySelector('i')
            ? '<i class="fas fa-users me-1"></i> '
            : '';

        if (expanded) {
            target.innerHTML = '';
            toggleButton.setAttribute('data-expanded', 'false');
            toggleButton.innerHTML = `${label}Ver Candidatos`;
            return;
        }

        if (typeof htmx === 'undefined') {
            return;
        }

        htmx.ajax('GET', `/Candidato/Listar/${idEleccion}`, {
            target: targetSelector,
            swap: 'innerHTML'
        });

        toggleButton.setAttribute('data-expanded', 'true');
        toggleButton.innerHTML = `${label}Ocultar Candidatos`;
    });
}

// Utility function to show notifications
function showNotification(message, type = 'info') {
    const alert = document.createElement('div');
    alert.className = `alert alert-${type}`;
    alert.textContent = message;
    alert.style.margin = '1rem';
    
    const main = document.querySelector('main');
    if (main) {
        main.insertBefore(alert, main.firstChild);
        
        // Auto-remove after 5 seconds
        setTimeout(() => {
            alert.remove();
        }, 5000);
    }
}
