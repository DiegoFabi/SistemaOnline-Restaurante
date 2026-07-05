// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// ---------- Mensajes de validación en español (jQuery Validate) ----------
if (typeof $ !== 'undefined' && $.validator) {
    $.extend($.validator.messages, {
        required: "Este campo es obligatorio.",
        remote: "Por favor corrige este campo.",
        email: "Por favor escribe una dirección de correo válida.",
        url: "Por favor escribe una URL válida.",
        date: "Por favor escribe una fecha válida.",
        number: "Por favor escribe un número válido.",
        digits: "Por favor escribe solo dígitos.",
        equalTo: "Por favor escribe el mismo valor de nuevo.",
        maxlength: $.validator.format("Por favor escribe a lo sumo {0} caracteres."),
        minlength: $.validator.format("Por favor escribe al menos {0} caracteres."),
        rangelength: $.validator.format("Por favor escribe un valor de entre {0} y {1} caracteres."),
        range: $.validator.format("Por favor escribe un valor entre {0} y {1}."),
        max: $.validator.format("Por favor escribe un valor menor o igual a {0}."),
        min: $.validator.format("Por favor escribe un valor mayor o igual a {0}."),
        pattern: "Por favor escribe un valor con el formato correcto."
    });

    // ---------- Métodos de validación personalizados ----------

    // Sin espacios dobles consecutivos (aplica a todos los campos de texto libre)
    $.validator.addMethod("noDoubleSpaces", function (value, element) {
        return this.optional(element) || !/ {2,}/.test(value);
    }, "El campo no debe contener espacios dobles consecutivos.");

    // Teléfono peruano: exactamente 9 dígitos comenzando con 9
    $.validator.addMethod("telefonoPeru", function (value, element) {
        return this.optional(element) || /^9[0-9]{8}$/.test(value);
    }, "El teléfono debe iniciar con 9 y tener exactamente 9 dígitos.");

    // DNI peruano: exactamente 8 dígitos numéricos
    $.validator.addMethod("dniPeru", function (value, element) {
        return this.optional(element) || /^[0-9]{8}$/.test(value);
    }, "El DNI debe tener exactamente 8 dígitos numéricos.");

    // RUC peruano: exactamente 11 dígitos numéricos
    $.validator.addMethod("rucPeru", function (value, element) {
        return this.optional(element) || /^[0-9]{11}$/.test(value);
    }, "El RUC debe tener exactamente 11 dígitos numéricos.");

    // Nombre / texto libre: solo letras, espacios y caracteres comunes (sin números)
    $.validator.addMethod("soloLetras", function (value, element) {
        return this.optional(element) || /^[a-záéíóúüñA-ZÁÉÍÓÚÜÑ\s\-'.]+$/.test(value);
    }, "Este campo solo debe contener letras.");
}

// ---------- Aplicar validaciones contextuales por nombre de campo ----------
$(document).ready(function () {
    // Campos de texto libre donde no se permiten espacios dobles
    var camposTextoLibre = [
        'Nombre', 'Apellidos', 'Nombre_Usuario', 'Nombre_Carta', 'Nombre_Turno',
        'Nombre_Rol', 'Nombre_Empresa', 'Nombre_Plato', 'Nombre_Ingrediente',
        'Nombre_Categoria', 'Descripcion', 'Detalle_Pedido', 'Clausula',
        'Ocasion_Especial', 'Notas', 'Cargo', 'Ubicacion', 'Tipo_Suministro',
        'Tipo_Contrato', 'Razon_Social', 'Direccion', 'Direccion_Fiscal',
        'Dias_Semana', 'Unidad_Medida'
    ];

    // Campos con formato específico por contexto
    var camposTelefono = ['Telefono', 'Telefono_Contacto'];
    var camposDNI = ['DNI'];
    var camposRUC = ['RUC'];
    var camposEmail = ['Email', 'Email_Contacto'];

    // Campos de nombre de persona (solo letras)
    var camposNombre = ['Nombre', 'Apellidos', 'Nombre_Usuario'];

    function aplicarReglas(form) {
        var $form = $(form);
        if (!$form.data('validator')) return;

        camposTextoLibre.forEach(function (campo) {
            $form.find('[name="' + campo + '"]').each(function () {
                $(this).rules('add', { noDoubleSpaces: true });
            });
        });

        camposTelefono.forEach(function (campo) {
            $form.find('[name="' + campo + '"]').each(function () {
                $(this).rules('add', {
                    telefonoPeru: true,
                    messages: { telefonoPeru: "El teléfono debe iniciar con 9 y tener exactamente 9 dígitos." }
                });
            });
        });

        camposDNI.forEach(function (campo) {
            $form.find('[name="' + campo + '"]').each(function () {
                $(this).rules('add', {
                    dniPeru: true,
                    messages: { dniPeru: "El DNI debe tener exactamente 8 dígitos numéricos." }
                });
            });
        });

        camposRUC.forEach(function (campo) {
            $form.find('[name="' + campo + '"]').each(function () {
                $(this).rules('add', {
                    rucPeru: true,
                    messages: { rucPeru: "El RUC debe tener exactamente 11 dígitos numéricos." }
                });
            });
        });

        camposNombre.forEach(function (campo) {
            $form.find('[name="' + campo + '"]').each(function () {
                $(this).rules('add', {
                    soloLetras: true,
                    messages: { soloLetras: "Este campo solo debe contener letras." }
                });
            });
        });

        camposEmail.forEach(function (campo) {
            $form.find('[name="' + campo + '"]').each(function () {
                $(this).rules('add', {
                    email: true,
                    messages: { email: "Por favor escribe una dirección de correo válida." }
                });
            });
        });
    }

    // setTimeout(0) garantiza que jQuery Unobtrusive Validation ya inicializó los forms
    setTimeout(function () {
        $('form').each(function () {
            aplicarReglas(this);
        });
    }, 0);
});

// ---------- Función reutilizable para el botón "Sin teléfono" ----------
function onSetDefault(inputId) {
    var input = document.getElementById(inputId);
    if (!input) return;
    input.value = '';
    input.required = false;
    input.removeAttribute('required');
}