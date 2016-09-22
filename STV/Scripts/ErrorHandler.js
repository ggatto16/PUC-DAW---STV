var mostrarCarregando = true;

function onBeginAjax() {
    mostrarCarregando = true;
    setTimeout(myFunction, 1000);
}

function myFunction() {
    if (mostrarCarregando) {
        $('#divCarregando').fadeIn();
    }
}

function onCompleteAjax() {
    mostrarCarregando = false;
    $('#divCarregando').fadeOut();
}

function MostrarMensagem(msg, tipo) {
    switch (tipo) {
        case 1: //sucesso
            $('#lblMensagem').text("✔ " + msg);
            $('#divMensagem').addClass('bg-success text-success');
            break;
        case 2: //Erro
            $('#lblMensagem').text("✖ " + msg);
            $('#divMensagem').addClass('bg-danger text-danger');
            break;
        case 3: //Alerta
            $('#lblMensagem').text("❗ " + msg);
            $('#divMensagem').addClass('bg-alert text-alert');
            break;
        default:
            $('#lblMensagem').text("❗ " + msg);
            $('#divMensagem').addClass('bg-alert text-alert');
            break;
    }
    $('#divMensagem').fadeIn();
}

function OnFailure(xhr, status) {
    var error = xhr.responseText.split("<title>")[1].split("</title>")[0];
    MostrarMensagem(error, 2);
}
