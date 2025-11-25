let empleadoDataTable;
let nominaDataTable;

$(function () {
    if ($('#empleadosTable').length) {
        empleadoDataTable = $('#empleadosTable').DataTable({
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.13.4/i18n/es-ES.json"
            }
        });
    }

    if ($('#nominasTable').length) {
        nominaDataTable = $('#nominasTable').DataTable({
            "language": {
                "url": "//cdn.datatables.net/plug-ins/1.13.4/i18n/es-ES.json"
            }
        });
    }
});



function refrescarDataTable() { }

/**
 * @param {string} url - La URL del controlador ej: /Empleado/Create.
 */
async function OpenModal(url) {
    try {
        const response = await axios.get(url);
        document.getElementById('modalContent').innerHTML = response.data;

        const modal = new bootstrap.Modal(document.getElementById('modalContainer'));
        modal.show();

        bindFormHandler();
    } catch (error) {
        console.error('Error al abrir el modal:', error);
    }
}



// Función de notificación 
function showToast(message, type = 'success') {
    const Toast = Swal.mixin({
        toast: true,
        position: 'bottom-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });
    Toast.fire({
        icon: type,
        title: message
    })
}

// Funciones de Eliminación Específicas de Empleado y Nómina

/**
 * @param {string} cedula - La cédula del empleado.
 */
async function eliminarEmpleado(cedula) {
    const result = await Swal.fire({
        title: '¿Estás seguro?',
        text: `¡Deseas eliminar al empleado con cédula ${cedula}?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    });

    if (result.isConfirmed) {
        try {
            await axios.post(`/Empleado/Delete?cedula=${cedula}`);
            showToast(`Empleado ${cedula} eliminado con éxito.`, 'success');
            location.reload();
        } catch (error) {
            console.error('Error al eliminar el empleado:', error);
            const errorMsg = error.response?.data?.error || 'Error desconocido del servidor.';
            Swal.fire('Error', errorMsg, 'error');
        }
    }
}

/**
 * @param {number} codigoId - El ID de la nómina.
 */
async function eliminarNomina(codigoId) {
    const result = await Swal.fire({
        title: '¿Estás seguro?',
        text: `¡Deseas eliminar la nómina con ID ${codigoId}?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    });

    if (result.isConfirmed) {
        try {
            await axios.post(`/Nomina/Delete?codigoId=${codigoId}`);
            showToast(`Nómina ${codigoId} eliminada con éxito.`, 'success');
            location.reload();
        } catch (error) {
            console.error('Error al eliminar la nómina:', error);
            const errorMsg = error.response?.data?.error || 'Error desconocido del servidor.';
            Swal.fire('Error', errorMsg, 'error');
        }
    }
}



async function bindFormHandler() {
    const form = document.getElementById('crudForm');

    if (!form) return;

    let url;
    let isEdit;

    const action = form.dataset.action;

    const currentPath = window.location.pathname.split('/');
    const controller = currentPath[1] || '';

    const validator = new JustValidate(`#${form.id}`, {
        errorFieldCssClass: 'is-invalid',
        successFieldCssClass: 'is-valid',
        errorLabelCssClass: 'text-danger',
    });

    //  Lógica y Validación Específica 

    if (controller.toLowerCase() === 'empleado') {
        const cedula = form.querySelector('[name="Cedula"]').value;
        url = action === 'Create' ? `/Empleado/Create` : `/Empleado/Edit?cedula=${cedula}`;
        isEdit = action === 'Edit';

        validator.addField('[name="Cedula"]', [
            { rule: 'required', errorMessage: 'La cédula es obligatoria.' },
            { rule: 'minLength', value: 5, errorMessage: 'Cédula debe tener al menos 5 caracteres.' }
        ]);
        validator.addField('[name="NombreCompleto"]', [
            { rule: 'required', errorMessage: 'El nombre completo es obligatorio.' },
            { rule: 'minLength', value: 5, errorMessage: 'Mínimo 5 caracteres.' }
        ]);
        validator.addField('[name="Telefono"]', [
            { rule: 'required', errorMessage: 'El teléfono es obligatorio.' }
        ]);
        validator.addField('[name="Sexo"]', [
            { rule: 'required', errorMessage: 'Debe seleccionar el sexo.' }
        ]);
        validator.addField('[name="Cargo"]', [
            { rule: 'required', errorMessage: 'El cargo es obligatorio.' }
        ]);

    } else if (controller.toLowerCase() === 'nomina') {
        const codigoId = form.querySelector('[name="CodigoId"]').value;
        url = action === 'Create' ? `/Nomina/CalculateAndGenerate` : `/Nomina/Edit?codigoId=${codigoId}`;
        isEdit = action === 'Edit';

        validator.addField('[name="EmpleadoCedula"]', [
            { rule: 'required', errorMessage: 'Debe seleccionar un empleado.' }
        ]);
        validator.addField('[name="Salario"]', [
            { rule: 'required', errorMessage: 'El salario base es obligatorio.' },
            { rule: 'minNumber', value: 0.01, errorMessage: 'El salario debe ser positivo.' }
        ]);
        validator.addField('[name="HorasExtras"]', [
            { rule: 'required', errorMessage: 'Campo obligatorio.' },
            { rule: 'minNumber', value: 0, errorMessage: 'Debe ser un valor positivo o cero.' }
        ]);
        validator.addField('[name="Inasistencia"]', [
            { rule: 'required', errorMessage: 'Campo obligatorio.' },
            { rule: 'minNumber', value: 0, errorMessage: 'Debe ser un valor positivo o cero.' }
        ]);
    } else {
        return;
    }


    validator.onSuccess(async () => {
        const formData = new FormData(form);
        const data = Object.fromEntries(formData.entries());

        delete data.__Invariant;

        const disabledCedula = form.querySelector('[name="Cedula"][disabled]');
        if (disabledCedula) data.Cedula = disabledCedula.value;

        try {
            const response = await axios.post(url, JSON.stringify(data), {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            bootstrap.Modal.getInstance(document.getElementById('modalContainer')).hide();

            showToast(`Registro ${isEdit ? 'actualizado' : 'creado'} con éxito.`, 'success');

            location.reload();

        } catch (error) {
            console.error('Error al enviar el formulario:', error);
            const errorResponse = error.response?.data;
            let errorMsg = errorResponse?.error || 'Error desconocido al guardar.';

            if (errorResponse?.errors && Array.isArray(errorResponse.errors)) {
                errorMsg = errorResponse.errors.join('\n');
            }

            Swal.fire('Error', errorMsg, 'error');
        }
    });
}