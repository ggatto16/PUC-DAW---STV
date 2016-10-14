function onBeginAjax(msg) {

    mostrarCarregando = true;

    if (msg)
        $('#pLoadMessage').html(msg);
    else
        $('#pLoadMessage').html('Carregando...');

    setTimeout(showLoading, 1000);
};

function showLoading() {
    if (mostrarCarregando) {
        $('#divCarregando').fadeIn();
    }
};

function onCompleteAjax() {
    mostrarCarregando = false;
    debugger;
    $('#divCarregando').fadeOut();
};

function MostrarMensagem(msg, tipo) {

    var label = $('#lblMensagem');
    var div = $('#divMensagem');
    switch (tipo) {
        case 1: //sucesso
            label.text("✔ " + msg);
            div.addClass('bg-success text-success');
            break;
        case 2: //Erro
            label.text("✖ " + msg);
            div.addClass('bg-danger text-danger');
            break;
        case 3: //Alerta
            label.text("❗ " + msg);
            div.addClass('bg-alert text-alert');
            break;
        default:
            label.text("❗ " + msg);
            div.addClass('bg-alert text-alert');
            break;
    }
    div.fadeIn();
    setTimeout(HideMessage, 5000);
};

function OnFailure(xhr, status) {
    //var error = xhr.responseText.split("<title>")[1].split("</title>")[0];
    var error = xhr.statusText;
    MostrarMensagem(error, 2);
};

function OnSuccess(xhr, status) {
    var msg = xhr.statusText;
    MostrarMensagem(msg, 1);
};

function HideMessage() {
    $('#divMensagem').fadeOut();
};

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});
