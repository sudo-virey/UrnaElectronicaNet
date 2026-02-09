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
