//$(function () {
//    $('#Tipo').change(function () {
//        var tipoID = $(this).val();
//        $.ajax({
//            url: 'http://' + window.location.host + '/Materiais/CarregarTipo',
//            type: 'GET',
//            data: { Idtipo: tipoID, URL: "@Html.Raw(Model.URL)"  },
//            cache: false,
//            success: function (result) {
//                $('#container').html(result);
//                PrepararFileUpload(tipoID);
//            }
//        });
//    });
//});

function PrepararFileUpload(tipoID) {

    switch (tipoID) {
        case "1":
            $("#Arquivo").fileinput({
                language: 'pt-BR',
                maxFileCount: 1,
                showUpload: false,
                showCaption: false,
                browseClass: "btn btn-primary btn-lg",
                fileType: "any",
                previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
                allowedFileExtensions: ['mp4', 'webm'],
                maxFileSize: 307200
            });
            break;

        case "2":
            $("#Arquivo").fileinput({
                language: 'pt-BR',
                maxFileCount: 1,
                showUpload: false,
                showCaption: false,
                browseClass: "btn btn-primary btn-lg",
                fileType: "any",
                allowedFileExtensions: ['doc', 'docx', 'xls', 'xlsx', 'pdf', 'rar', 'zip', 'txt', 'ppt', 'pptx', 'exe'],
                maxFileSize: 307200
            });
            break;            

        case "4":
            $("#Arquivo").fileinput({
                language: 'pt-BR',
                maxFileCount: 1,
                showUpload: false,
                showCaption: false,
                browseClass: "btn btn-primary btn-lg",
                fileType: "any",
                previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
                allowedFileExtensions: ['jpg', 'png', 'gif'],
                maxFileSize: 307200
            });
            break;

        default:
            $("#Arquivo").fileinput({
                language: 'pt-BR',
                maxFileCount: 1,
                showUpload: false,
                showCaption: false,
                browseClass: "btn btn-primary btn-lg",
                fileType: "any",
                previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
                maxFileSize: 307200
            });
            break;
    }
}