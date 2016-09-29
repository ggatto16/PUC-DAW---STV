var tipoId = @Convert.ToInt32(Model.Tipo);
$(document).ready(function () {
    if (tipoId == 1 || tipoId == 2 || tipoId == 4)
        PrepararFileUpload(tipoId);
});

function EnviarArquivo(){

    var formData = new FormData($("#formArquivo")[0]);

    $.ajax({
        url: '/Materiais/UploadFile?id=' + @Model.Idmaterial,
        type: 'POST',
        data: formData,
        async: false,
        timeout: 0,
        beforeSend: onBeginAjax(),
        complete: onCompleteAjax(),
        error: MostrarMensagem(data, 2),
        success: function (data) {
            MostrarMensagem(data, 2);
            var url = '@Url.Action("Details", "Cursos", new { id = ViewBag.Idcurso, Idunidade = Model.Idunidade })';
            window.location.href = url;
        },
        cache: false,
        contentType: false,
        processData: false
    });

    return false;
}
