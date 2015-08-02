/// <reference path="_references.js" />
$(function () {

    var host = document.domain;
    var port = window.document.location.port;
    if (port != "80")
        host += (":" + port);

    host = "http://" + host;


    $("#btnShowPhone").click(function () {
        var URL = '';
        var currentPath = location.pathname.toLowerCase();
        var requiredPath = $('#btnShowPhone').attr('data-controler').toLowerCase();
        //        alert(currentPath);
        //        if (currentPath.indexOf(requiredPath) == -1) {
        //            URL = $('#btnShowPhone').attr('data-controler') + "/" + $('#PropertyType').attr('data-propertyAction') + '/' + $('#PropertyObjectId').val();
        //        }
        //        else {
        URL = $('#btnShowPhone').attr('data-controler') + '/' + $('#btnShowPhone').attr('data-action') + "/" + $('#PropertyObjectId').val();
        //        }
        URL = host + "/" + URL;
        $.getJSON(URL, function (data) {

            $.each(data, function (i, state) {
                if (state.phone1 != null)
                    $("#phone1").html(state.phone1);

                if (state.phone2 != null)
                    $("#phone2").html(state.phone2);

                if (state.phone3 != null)
                    $("#phone3").html(state.phone3);
            });

        });
        $("#btnShowPhone").hide();
        return false;

    });
});