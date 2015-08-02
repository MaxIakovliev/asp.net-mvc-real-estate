/// <reference path="_references.js" />

$(function () {
    var host = document.domain;
    var port = window.document.location.port;
    if (port != "80")
        host += (":" + port);

    host = "http://" + host;
    //alert(host);


    if ($('#PropertyTypeActionDiv option').size() == 0) {
        $('#PropertyTypeActionDiv').hide();
    }
    else {
        //$('#PropertyTypeActionDiv').show();
        $('#PropertyTypeActionDiv').css('display', 'inline-block');
    }


    if ($('#PropertyTypeCitiesDiv option').size() == 0) {
        $('#PropertyTypeCitiesDiv').hide();
    }
    else {
        //$('#PropertyTypeCitiesDiv').show();
        $('#PropertyTypeCitiesDiv').css('display', 'inline-block');
    }

    if ($('#PropertyTypeCityDistrictDiv option').size() == 0) {
        $('#PropertyTypeCityDistrictDiv').hide();
    }
    else {
        //$('#PropertyTypeCityDistrictDiv').show();
        $('#PropertyTypeCityDistrictDiv').css('display', 'inline-block');
    }

    var adId = $("#adid").val();

    if (adId != '-1') {
        $("#formContainer").load(host + "/PropertyManipulation/ShowForUpdatePropertyObject?id=" + adId);
    }


    //$('#PropertyTypeCitiesDiv').hide();
    // $('#PropertyTypeCityDistrictDiv').hide();


    $('#PropertyType').change(function () {

        $("#formContainer").empty(); //удаляем форму
        $('#PropertyTypeAction option').remove(); //удаляем тип действия
        $('#PropertyTypeActionDiv').hide(); //прячем тип действия

        $('#PropertyTypeCities option').remove(); //удаляем город
        $('#PropertyTypeCitiesDiv').hide(); //прячем город

        $('#PropertyTypeCityDistrict option').remove(); //удаляем район
        $('#PropertyTypeCityDistrictDiv').hide(); //прячем район

        var iPropertyType3 = $("#PropertyType").val();

        if (iPropertyType3 == 7) {
            $("#formContainer").load(host + "/PropertyManipulation/AddService?PropertyType=" + iPropertyType3);

        } else {

            var URL = '';
            //var URL = $('#PropertyType').data('propertyAction');
            //var URL = $('#PropertyType').attr('data-propertyAction') + '/' + $('#PropertyType').val();
            var currentPath = location.pathname.toLowerCase();
            var requiredPath = $('#PropertyType').attr('data-controler').toLowerCase();

            if (currentPath.indexOf(requiredPath) == -1) {
                URL = $('#PropertyType').attr('data-controler') + "/" + $('#PropertyType').attr('data-propertyAction') + '/' + $('#PropertyType').val();
            }
            else {
                URL = $('#PropertyType').attr('data-propertyAction') + '/' + $('#PropertyType').val();
            }
            //URL = "../" + URL;
            URL = host + "/" + URL;
            $.getJSON(URL, function (data) {
                if (data.length > 0) {
                    var items = "<option value='-1'>Выберите...</option>";
                    $.each(data, function (i, state) {
                        items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                    });
                    $('#PropertyTypeAction').html(items);
                    //$('#PropertyTypeActionDiv').show();
                    $('#PropertyTypeActionDiv').css('display', 'inline-block');
                }
                else {

                    $('#PropertyTypeAction option').remove();
                    $('#PropertyTypeCities option').remove();
                    $('#PropertyTypeCityDistrict option').remove();

                    $('#PropertyTypeActionDiv').hide();
                    $('#PropertyTypeCitiesDiv').hide();
                    $('#PropertyTypeCityDistrictDiv').hide();
                }
            });
        }
    });

    $('#PropertyTypeAction').change(function () {

        $("#formContainer").empty(); //удаляем форму
        $('#PropertyTypeCities option').remove(); //удаляем город
        $('#PropertyTypeCitiesDiv').hide(); //прячем город

        $('#PropertyTypeCityDistrict option').remove(); //удаляем район
        $('#PropertyTypeCityDistrictDiv').hide(); //прячем район

        //        var URL = $('#PropertyTypeAction').attr('data-propertyAction') + '/' + $('#PropertyTypeAction').val();

        var iPropertyType2 = $("#PropertyType").val();
        var iPropertyTypeAction2 = $("#PropertyTypeAction").val();

        if (iPropertyType2 == 6) {

            $("#formContainer").load(host + "/PropertyManipulation/AddAbroadProperty?PropertyType=" + iPropertyType2 +
            "&PropertyTypeAction=" + iPropertyTypeAction2);

        }
        else {


            var URL = '';
            var currentPath = location.pathname.toLowerCase();
            var requiredPath = $('#PropertyTypeAction').attr('data-controler').toLowerCase();

            if (currentPath.indexOf(requiredPath) == -1) {
                URL = $('#PropertyTypeAction').attr('data-controler') + "/" + $('#PropertyTypeAction').attr('data-propertyAction') + '/' + $('#PropertyTypeAction').val();
            }
            else {
                URL = $('#PropertyTypeAction').attr('data-propertyAction') + '/' + $('#PropertyTypeAction').val();
            }
            //URL = "../" + URL;
            URL = host + "/" + URL;

            $.getJSON(URL, function (data) {
                if (data.length > 0) {
                    var items = "<option value='-1'>Выберите...</option>";
                    $.each(data, function (i, state) {
                        items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                    });
                    $('#PropertyTypeCities').html(items);
                    //$('#PropertyTypeCitiesDiv').show();
                    $('#PropertyTypeCitiesDiv').css('display', 'inline-block');
                }
                else {

                    $('#PropertyTypeCities option').remove();
                    $('#PropertyTypeCityDistrict option').remove();

                    $('#PropertyTypeCitiesDiv').hide();
                    $('#PropertyTypeCityDistrictDiv').hide();

                }
            });
        }
    });


    $('#PropertyTypeCities').change(function () {
        //var URL = $('#PropertyTypeCities').attr('data-propertyAction') + '/' + $('#PropertyTypeCities').val();
        // alert(URL);
        var URL = '';
        var currentPath = location.pathname.toLowerCase();
        var requiredPath = $('#PropertyTypeCities').attr('data-controler').toLowerCase();

        if (currentPath.indexOf(requiredPath) == -1) {
            URL = $('#PropertyTypeCities').attr('data-controler') + "/" + $('#PropertyTypeCities').attr('data-propertyAction') + '/' + $('#PropertyTypeCities').val();
        }
        else {
            URL = $('#PropertyTypeCities').attr('data-propertyAction') + '/' + $('#PropertyTypeCities').val();
        }
        //URL = "../" + URL;
        URL = host + "/" + URL;


        $.getJSON(URL, function (data) {
            if (data.length > 0) {
                var items = "<option value='-1'>Выберите...</option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('#PropertyTypeCityDistrict').html(items);
                //$('#PropertyTypeCityDistrictDiv').show();
                $('#PropertyTypeCityDistrictDiv').css('display', 'inline-block');
            }
            else {
                $('#PropertyTypeCityDistrict option').remove();

                $('#PropertyTypeCityDistrictDiv').hide();
            }
        });
    });


    $("#PropertyTypeCityDistrict").change(function () {

        if (adId == '-1') {
            adId = '';
        }

        // alert("PropertyTypeAction" + $("#PropertyTypeAction").val());
        // alert("PropertyTypeCities" + $("#PropertyTypeCities").val());
        // alert("PropertyTypeCityDistrict" + $("#PropertyTypeCityDistrict").val());

        var iPropertyType = $("#PropertyType").val();
        var iPropertyTypeAction = $("#PropertyTypeAction").val();
        var iPropertyTypeCities = $("#PropertyTypeCities").val();
        var iPropertyTypeCityDistrict = $("#PropertyTypeCityDistrict").val();
        //        alert("iPropertyType=" + iPropertyType);
        //        alert("iPropertyTypeAction=" + iPropertyTypeAction);
        //        alert("iPropertyTypeCities=" + iPropertyTypeCities);
        //        alert("iPropertyTypeCityDistrict=" + iPropertyTypeCityDistrict);
        //alert("am here");


        if (iPropertyType == 1) { //&& iPropertyTypeAction == 1) {
            $("#formContainer").load(host + "/PropertyManipulation/AddSellFlat?PropertyType=" + iPropertyType +
            "&PropertyTypeAction=" + iPropertyTypeAction + "&PropertyTypeCities=" + iPropertyTypeCities +
            "&PropertyTypeCityDistrict=" + iPropertyTypeCityDistrict + "&id=" + adId);
        }
        else if (iPropertyType == 2 && iPropertyTypeAction == 4) {//дома/дачи-продажа
            $("#formContainer").load(host + "/PropertyManipulation/AddCountryHouse?PropertyType=" + iPropertyType +
            "&PropertyTypeAction=" + iPropertyTypeAction + "&PropertyTypeCities=" + iPropertyTypeCities +
            "&PropertyTypeCityDistrict=" + iPropertyTypeCityDistrict + "&id=" + adId);
        }
        else if (iPropertyType == 2 && iPropertyTypeAction == 5) {//дома/дачи-аренда
            $("#formContainer").load(host + "/PropertyManipulation/AddCountryHouse?PropertyType=" + iPropertyType +
            "&PropertyTypeAction=" + iPropertyTypeAction + "&PropertyTypeCities=" + iPropertyTypeCities +
            "&PropertyTypeCityDistrict=" + iPropertyTypeCityDistrict + "&id=" + adId);
        }
        else if (iPropertyType == 3) {//земельные участки
            $("#formContainer").load(host + "/PropertyManipulation/AddLand?PropertyType=" + iPropertyType +
            "&PropertyTypeAction=" + iPropertyTypeAction + "&PropertyTypeCities=" + iPropertyTypeCities +
            "&PropertyTypeCityDistrict=" + iPropertyTypeCityDistrict + "&id=" + adId);
        }
        else if (iPropertyType == 4) {//коммерческая недвижимость
            $("#formContainer").load(host + "/PropertyManipulation/AddCommercialProperty?PropertyType=" + iPropertyType +
            "&PropertyTypeAction=" + iPropertyTypeAction + "&PropertyTypeCities=" + iPropertyTypeCities +
            "&PropertyTypeCityDistrict=" + iPropertyTypeCityDistrict + "&id=" + adId);
        }
        else if (iPropertyType == 5) {//гаражи паркинги
            $("#formContainer").load(host + "/PropertyManipulation/AddParkingGarage?PropertyType=" + iPropertyType +
            "&PropertyTypeAction=" + iPropertyTypeAction + "&PropertyTypeCities=" + iPropertyTypeCities +
            "&PropertyTypeCityDistrict=" + iPropertyTypeCityDistrict + "&id=" + adId);
        }


    });


});