/// <reference path="_references.js" />
$(function () {
    var ChangeUserSignatures = "";
    ChangeUserSignatures = $("#ChangeUserSignatures").attr("href");
    ChangeUserSignatures = ChangeUserSignatures+"?noCache="+new Date().getTime();
    $("#ChangeUserSignatures").attr("href", ChangeUserSignatures);

    var UploadUserPhoto = $("#UploadUserPhoto").attr("href");
    UploadUserPhoto = UploadUserPhoto + "?noCache=" + new Date().getTime();
    $("#UploadUserPhoto").attr("href", UploadUserPhoto);
    //alert(ChangeUserSignatures);
    $("#tabs").tabs({
        beforeLoad: function (event, ui) {
            ui.jqXHR.error(function () {
                ui.panel.html(
						"Couldn't load this tab. We'll try to fix this as soon as possible. " +
						"If this wouldn't be a demo.");
            });
        }
    });
});