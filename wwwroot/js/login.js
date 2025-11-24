
document.addEventListener("DOMContentLoaded", function () {
    //Funcion para cambiar entre formularios
    function switchForm(formId) {
        //Ocultar todos los formularios
        document.querySelectorAll('.form').forEach(form => {
            form.classList.remove('active');
        })
        document.getElementById(formId).classList.add('active');
    }

    //Event listener para cambiar de formularios
    document.querySelectorAll('.switch-form, .forgot-link').forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            const formId = this.getAttribute('data-form');
            switchForm(formId)
        });
    });
    //Validamos con JustValidate nuestro formulario
    const validate = new JustValidate('#signupForm');

    validate
        .addField('#email', [
            {
                rule: 'required',
                errorMessage: 'El usuario es obligatorio',
            }
        ])
        .addField('#password', [
            {
                rule: 'required',
                errorMessage: 'La contraseña es obligatoria',
            }
        ])
        .onSuccess(async (event) => {
            event.preventDefault();

            const data = {
                Email: document.getElementById("email").value,
                Password: document.getElementById("password").value
            };

            try {
                const response = await axios.post('/Auth/Login', data);

                if (response.data.success) {
                    Swal.fire({
                        toast: true,
                        position: 'top-end',
                        icon: 'success',
                        title: 'Inicio de sesión exitoso',
                        showConfirmButton: false,
                        timer: 2000
                    });

                    setTimeout(() => {
                        window.location.href = '/Home/Index';
                    }, 2000);
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.data.errorMessage || 'Credenciales inválidas.'
                    });
                }
            } catch (err) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error del servidor',
                    text: 'No se pudo conectar con el servidor.'
                });
            }
        });
    // ---------------- REGISTER FORM ----------------//
    const validateRegisterForm = new JustValidate('#registerFormElement');
    validateRegisterForm
        .addField('#UserName', [
            {
                rule: 'required',
                errorMessage: 'El usuarios es obligatorio',

            }
        ])
        .addField('#registerEmail', [{
            rule: 'required',
            errorMessage:'El correo es obligatorio '

        }])
        .addField('#registerPassword', [{
            rule: 'required',
            errorMessage:'La contraseña es requerida'
        },
            {
                rule: 'strongPassword',
                errorMessage: 'Debe tener 8 caracteres, una mayúscula y un número.'
            }
        ])
        .addField('#confirmPassword', [
            {
                validator: (value, fields) => {
                    return value === fields['#registerPassword']?.elem?.value;
                },
                errorMessage: 'Las contraseñas no coinciden'
            }
        ])
        //.addField('#UserRole', [
        //    { rule: 'required', errorMessage: 'Seleccione un rol' }
        //])
        .onSuccess(async (event) => {
            event.preventDefault();

            const data = {
                Username: document.getElementById("UserName").value,
                Email: document.getElementById("registerEmail").value,
                Password: document.getElementById("registerPassword").value
            };

            try {
                const response = await axios.post('/Auth/Register', data);

                if (response.data.success) {
                    Swal.fire({
                        toast: true,
                        position: 'top-end',
                        icon: 'success',
                        title: 'Cuenta creada exitosamente. Revisa tu correo para confirmarla.',
                        showConfirmButton: false,
                        timer: 2000
                    });

                    setTimeout(() => {
                        window.location.href = '/';
                    }, 2000);
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.data.errorMessage || 'Credenciales inválidas.'
                    });
                }
            } catch (err) {

                const data  = err.response?.data;
                const mensaje = data?.message || 'No se pudo conectar con el servidor.';
                Swal.fire({
                    icon: 'error',
                    title: 'Error del servidor',
                    text: mensaje
                });
            }
        });

    // ---------------- FORGET PASSWORD FORM ----------------//
    const validateForgetForm = new JustValidate('#resetPasswordForm');
    validateForgetForm
        
        .addField('#resetEmail', [{
            rule: 'required',
            errorMessage: 'La contraseña es requerida'
        },
        {
            rule: 'email',
            errorMessage: 'No es un correo valido.'
        }
        ])
     
      
        .onSuccess(async (event) => {
            event.preventDefault();

            const data = {
                Email: document.getElementById("resetEmail").value,
            };

            try {
                const response = await axios.post('/Auth/ForgotPassword', data);

                if (response.data.success) {
                    Swal.fire({
                        toast: true,
                        position: 'top-end',
                        icon: 'success',
                        title: 'Se ha enviado un enlace con el que puedas restaurar tu contraseña. Revisa tu correo para confirmarla.',
                        showConfirmButton: false,
                        timer: 2000
                    });
                    
                    setTimeout(() => {
                        window.location.href = '/';
                    }, 2000);
                } else {
                    Swal.fire({ 
                        icon: 'error',
                        title: 'Error',
                        text: response.data.errorMessage || 'Credenciales inválidas.'
                    });
                }
            } catch (err) {

                const data = err.response?.data;
                const mensaje = data?.message || 'No se pudo conectar con el servidor.';
                Swal.fire({
                    icon: 'error',
                    title: 'Error del servidor',
                    text: mensaje
                });
            }
        });

});
