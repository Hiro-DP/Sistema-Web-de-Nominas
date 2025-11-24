document.addEventListener('DOMContentLoaded', () => {

    const validateResetForm = new JustValidate('#newPasswordForm');

    validateResetForm
        .addField('#newPassword', [
            {
                rule: 'required',
                errorMessage: 'La contraseña es requerida.'
            },
            {
                rule: 'password',
                errorMessage: 'Debe tener al menos 8 caracteres, una mayúscula, una minúscula y un número.'
            }
        ])
        .addField('#confirmPassword', [
            {
                rule: 'required',
                errorMessage: 'Confirma tu contraseña.'
            },
            {
                validator: (value, fields) => {
                    const passwordValue = fields['#newPassword'].elem.value;
                    return value === passwordValue;
                },
                errorMessage: 'Las contraseñas no coinciden.'
            }
        ])
        .onSuccess(async (event) => {
            event.preventDefault();

            const data = {
                Email: document.getElementById("email").value,
                Token: document.getElementById("token").value,
                NewPassword: document.getElementById("newPassword").value
            };

            try {
                const response = await axios.post('/Auth/ResetPassword', data);

                if (response.data.success) {
                    Swal.fire({
                        toast: true,
                        position: 'top-end',
                        icon: 'success',
                        title: 'Tu contraseña ha sido restablecida correctamente.',
                        showConfirmButton: false,
                        timer: 2000
                    });

                    setTimeout(() => {
                        window.location.href = '/Auth/Login';
                    }, 2000);
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.data.errorMessage || 'No se pudo restablecer la contraseña.'
                    });
                }
            } catch (err) {
                const mensaje = err.response?.data?.message || 'No se pudo conectar con el servidor.';
                Swal.fire({
                    icon: 'error',
                    title: 'Error del servidor',
                    text: mensaje
                });
            }
        });
});
