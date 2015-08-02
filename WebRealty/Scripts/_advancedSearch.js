/// <reference path="_references.js" />
$(function () {


    var host = document.domain;
    var port = window.document.location.port;
    if (port != "80")
        host += (":" + port);

    host = "http://" + host;


    var PropertyType = $('#AdvancedSearchBox select[id=PropertyType]');
    var PropertyTypeAction = $('#AdvancedSearchBox select[id=PropertyTypeAction]');
    var PropertyTypeCities = $('#AdvancedSearchBox select[id=PropertyTypeCities]');
    var PropertyTypeCityDistrict = $('#AdvancedSearchBox select[id=PropertyTypeCityDistrict]');

    PropertyType.change(function () {
        var URL = '';
        var currentPath = location.pathname.toLowerCase();
        var requiredPath = PropertyType.attr('data-controler').toLowerCase();
        if (currentPath.indexOf(requiredPath) == -1) {
            URL = PropertyType.attr('data-controler') + "/" + PropertyType.attr('data-propertyAction') + '/' + PropertyType.val();
        }
        else {
            URL = PropertyType.attr('data-propertyAction') + '/' + PropertyType.val();
        }
        //URL = "../" + URL;

        URL = host + "/" + URL;

        //alert(URL);

        $.getJSON(URL, function (data) {
            if (data.length > 0) {
                // alert("fire");
                var items = "<option value='-1'>Выберите...</option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                PropertyTypeAction.html(items);
            }
        });

        //подгружаем дополнительные параметры поиска
        var selectedText = $("#AdvancedSearchBox select[id=PropertyType] option:selected").text();
        $("#advancedSearchParams").html("");
        if (selectedText == "Квартиры") {
            if (currentPath.indexOf("searchbox") == -1)
                $("#advancedSearchParams").load("SearchBox/AdvancedSearchFlatPart");
            else
                $("#advancedSearchParams").load("AdvancedSearchFlatPart");

        }
        else if (selectedText == "Дома. Дачи") {
            if (currentPath.indexOf("searchbox") == -1)
                $("#advancedSearchParams").load("SearchBox/AdvancedSearchHousePart");
                else
                    $("#advancedSearchParams").load("AdvancedSearchHousePart");

        }
        else if (selectedText == "Замельные участки") {
            if (currentPath.indexOf("searchbox") == -1)
                $("#advancedSearchParams").load("SearchBox/AdvancedSearchLandPart");
                else
                    $("#advancedSearchParams").load("AdvancedSearchLandPart");

        }
        else if (selectedText == "Коммерческая недвижимость") {
            if (currentPath.indexOf("searchbox") == -1)
                $("#advancedSearchParams").load("SearchBox/AdvancedSearchCommercialPropertyPart");
                else
                    $("#advancedSearchParams").load("AdvancedSearchCommercialPropertyPart");

        }

    });

    PropertyTypeCities.change(function () {
        var URL = '';
        var currentPath = location.pathname.toLowerCase();
        var requiredPath = PropertyTypeCities.attr('data-controler').toLowerCase();

        if (currentPath.indexOf(requiredPath) == -1) {
            URL = PropertyTypeCities.attr('data-controler') + "/" + PropertyTypeCities.attr('data-propertyAction') + '/' + PropertyTypeCities.val();
        }
        else {
            URL = PropertyTypeCities.attr('data-propertyAction') + '/' + PropertyTypeCities.val();
        }

        $.getJSON(URL, function (data) {
            if (data.length > 0) {
                var items = "<option value='-1'>Выберите...</option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                PropertyTypeCityDistrict.html(items);
            }
            else {
                //PropertyTypeCityDistrict.hide();
                //$('#PropertyTypeCityDistrict option').remove();
                $('#AdvancedSearchBox select[id=PropertyTypeCityDistrict] option').remove();
            }
        });
    });


});