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

    function onCandidatoChanged(event) {
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
    }

    document.body.addEventListener('candidatoCreado', onCandidatoChanged);
    document.body.addEventListener('candidatoActualizado', onCandidatoChanged);

    function onUrnaEleccionChanged(event) {
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

        const targetSelector = `#urnas-area-${idEleccion}`;
        if (document.querySelector(targetSelector)) {
            htmx.ajax('GET', `/UrnaEleccion/Listar/${idEleccion}`, {
                target: targetSelector,
                swap: 'innerHTML'
            });
        }
    }

    document.body.addEventListener('urnaEleccionChanged', onUrnaEleccionChanged);

    function setToggleCandidatosState(toggleButton, expanded) {
        const icon = toggleButton.querySelector('i');
        const nextTitle = expanded ? 'Ocultar candidatos' : 'Ver candidatos';

        toggleButton.setAttribute('data-expanded', expanded ? 'true' : 'false');
        toggleButton.setAttribute('title', nextTitle);
        toggleButton.setAttribute('aria-label', nextTitle);

        if (icon) {
            icon.classList.remove('fa-users', 'fa-user-slash');
            icon.classList.add(expanded ? 'fa-user-slash' : 'fa-users');
        }
    }

    function setToggleUrnasState(toggleButton, expanded) {
        const icon = toggleButton.querySelector('i');
        const nextTitle = expanded ? 'Ocultar urnas' : 'Gestionar Urnas';

        toggleButton.setAttribute('data-expanded', expanded ? 'true' : 'false');
        toggleButton.setAttribute('title', nextTitle);
        toggleButton.setAttribute('aria-label', nextTitle);

        if (icon) {
            icon.classList.remove('fa-tablet-alt', 'fa-times');
            icon.classList.add(expanded ? 'fa-times' : 'fa-tablet-alt');
        }
    }

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

        if (expanded) {
            target.innerHTML = '';
            setToggleCandidatosState(toggleButton, false);
            return;
        }

        // Ocultar urnas cuando se abren candidatos
        const urinasArea = document.querySelector(`#urnas-area-${idEleccion}`);
        if (urinasArea) {
            urinasArea.innerHTML = '';
        }

        if (typeof htmx === 'undefined') {
            return;
        }

        htmx.ajax('GET', `/Candidato/Listar/${idEleccion}`, {
            target: targetSelector,
            swap: 'innerHTML'
        });

        setToggleCandidatosState(toggleButton, true);
    });

    // Manejador para el botón de urnas asignadas
    document.body.addEventListener('click', function (event) {
        const toggleButton = event.target.closest('.js-toggle-urnas');
        if (!toggleButton) {
            return;
        }

        event.preventDefault();
        event.stopPropagation();

        const idEleccion = toggleButton.getAttribute('data-eleccion-id');
        if (!idEleccion) {
            return;
        }

        const targetSelector = `#urnas-area-${idEleccion}`;
        const target = document.querySelector(targetSelector);
        if (!target) {
            return;
        }

        const expanded = toggleButton.getAttribute('data-expanded') === 'true';

        if (expanded) {
            target.innerHTML = '';
            setToggleUrnasState(toggleButton, false);
            return;
        }

        // Ocultar candidatos cuando se abren urnas
        const candidatosArea = document.querySelector(`#candidatos-area-${idEleccion}`);
        const candidatosToggle = document.querySelector(`.js-toggle-candidatos[data-eleccion-id="${idEleccion}"]`);

        if (candidatosArea && candidatosArea.innerHTML.trim() !== '') {
            candidatosArea.innerHTML = '';
            if (candidatosToggle) {
                setToggleCandidatosState(candidatosToggle, false);
            }
        }

        if (typeof htmx === 'undefined') {
            return;
        }

        htmx.ajax('GET', `/UrnaEleccion/Listar/${idEleccion}`, {
            target: targetSelector,
            swap: 'innerHTML'
        });

        setToggleUrnasState(toggleButton, true);
    });

    document.body.addEventListener('change', function (event) {
        const input = event.target.closest('.js-candidato-foto-input');
        if (!input) {
            return;
        }

        const previewWrapId = input.getAttribute('data-preview-wrap-id');
        const previewImgId = input.getAttribute('data-preview-img-id');
        const fileNameId = input.getAttribute('data-file-name-id');

        const previewWrap = previewWrapId ? document.getElementById(previewWrapId) : null;
        const previewImg = previewImgId ? document.getElementById(previewImgId) : null;
        const fileNameLabel = fileNameId ? document.getElementById(fileNameId) : null;

        const file = input.files && input.files.length > 0 ? input.files[0] : null;
        if (!file) {
            return;
        }

        if (fileNameLabel) {
            fileNameLabel.textContent = `Archivo seleccionado: ${file.name}`;
        }

        if (!previewImg || typeof FileReader === 'undefined') {
            return;
        }   

        const reader = new FileReader();
        reader.onload = function (readerEvent) {
            previewImg.src = readerEvent.target?.result || '';
            if (previewWrap) {
                previewWrap.style.display = 'block';
            }
        };
        reader.readAsDataURL(file);
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
