/**
 * FUNCIONES DE MODAL - PROCESOS ELECTORALES
 * Gestión de crear, editar y eliminar procesos
 */

/**
 * Abrir modal para crear nuevo proceso
 */
function abrirModalNuevoProceso() {
    document.getElementById('modalNuevoProceso').style.display = 'block';
    document.getElementById('modalOverlay').style.display = 'block';
    document.getElementById('nombreProceso').focus();
}

/**
 * Cerrar modal
 */
function cerrarModalNuevoProceso() {
    document.getElementById('modalNuevoProceso').style.display = 'none';
    document.getElementById('modalOverlay').style.display = 'none';
    document.getElementById('formNuevoProceso').reset();
}

/**
 * Guardar nuevo proceso - envía solicitud al servidor
 */
async function guardarNuevoProceso() {
    const nombre = document.getElementById('nombreProceso').value.trim();

    if (!nombre) {
        alert('Por favor ingresa el nombre del proceso');
        return;
    }

    try {
        const response = await fetch('/ProcesoElectoral/CreateProceso', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify({ nombre: nombre })
        });

        const resultado = await response.json();

        if (resultado.success) {
            alert('✓ ' + resultado.message);
            cerrarModalNuevoProceso();
            // Recargar la página para ver el nuevo proceso
            setTimeout(() => location.reload(), 500);
        } else {
            alert('Error: ' + resultado.message);
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Error al crear el proceso. Intenta de nuevo.');
    }
}

/**
 * Editar proceso - función en desarrollo
 */
function editarProceso(id, nombre) {
    alert('Función de editar en desarrollo (Proceso #' + id + ': ' + nombre + ')');
}

/**
 * Eliminar proceso - función en desarrollo
 */
function eliminarProceso(id) {
    if (confirm('¿Estás seguro de que deseas eliminar este proceso?')) {
        alert('Función de eliminar en desarrollo (Proceso #' + id + ')');
    }
}

/**
 * Inicializar eventos - ejecutar cuando carga el DOM
 */
document.addEventListener('DOMContentLoaded', function() {
    // Permitir Enter para guardar en el modal
    const inputNombre = document.getElementById('nombreProceso');
    if (inputNombre) {
        inputNombre.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                guardarNuevoProceso();
            }
        });
    }
});
