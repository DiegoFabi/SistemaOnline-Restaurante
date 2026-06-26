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
}

// Función reutilizable para el botón "Sin teléfono": limpia el campo,
// deshabilita la validación requerida y bloquea la edición manual.
function onSetDefault(inputId) {
    var input = document.getElementById(inputId);
    if (!input) return;
    input.value = '';
    input.required = false;
    input.removeAttribute('required');
}