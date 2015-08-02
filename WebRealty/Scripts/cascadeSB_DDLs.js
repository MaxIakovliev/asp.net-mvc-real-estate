/// <reference path="_references.js" />

$(function () {

    if ($('#PropertyTypeActionDiv option').size() == 0) {
        $('#PropertyTypeActionDiv').hide();
    }
    else {
        $('#PropertyTypeActionDiv').show();
    }


    if ($('#PropertyTypeCitiesDiv option').size() == 0) {
        $('#PropertyTypeCitiesDiv').hide();
    }
    else {
        $('#PropertyTypeCitiesDiv').show();
    }

    if ($('#PropertyTypeCityDistrictDiv option').size() == 0) {
        $('#PropertyTypeCityDistrictDiv').hide();
    }
    else {
        $('#PropertyTypeCityDistrictDiv').show();
    }



    //$('#PropertyTypeCitiesDiv').hide();
    // $('#PropertyTypeCityDistrictDiv').hide();


    $('#PropertyType').change(function () {
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

        $.getJSON(URL, function (data) {
            if (data.length > 0) {
                var items = "<option value='-1'>Выберите...</option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('#PropertyTypeAction').html(items);
                $('#PropertyTypeActionDiv').show();
                
                //hide all other
                $('#PropertyTypeCitiesDiv').hide();
                $('#PropertyTypeCityDistrictDiv').hide();
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
    });

    $('#PropertyTypeAction').change(function () {
        //        var URL = $('#PropertyTypeAction').attr('data-propertyAction') + '/' + $('#PropertyTypeAction').val();
        var URL = '';
        var currentPath = location.pathname.toLowerCase();
        var requiredPath = $('#PropertyTypeAction').attr('data-controler').toLowerCase();

        if (currentPath.indexOf(requiredPath) == -1) {
            URL = $('#PropertyTypeAction').attr('data-controler') + "/" + $('#PropertyTypeAction').attr('data-propertyAction') + '/' + $('#PropertyTypeAction').val();
        }
        else {
            URL = $('#PropertyTypeAction').attr('data-propertyAction') + '/' + $('#PropertyTypeAction').val();
        }

        $.getJSON(URL, function (data) {
            if (data.length > 0) {
                var items = "<option value='-1'>Выберите...</option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('#PropertyTypeCities').html(items);
                $('#PropertyTypeCitiesDiv').show();

                //hide others
                $('#PropertyTypeCityDistrictDiv').hide();
            }
            else {

                $('#PropertyTypeCities option').remove();
                $('#PropertyTypeCityDistrict option').remove();

                $('#PropertyTypeCitiesDiv').hide();
                $('#PropertyTypeCityDistrictDiv').hide();

            }
        });
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

        $.getJSON(URL, function (data) {
            if (data.length > 0) {
                var items = "<option value='-1'>Выберите...</option>";
                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('#PropertyTypeCityDistrict').html(items);
                $('#PropertyTypeCityDistrictDiv').show();
            }
            else {
                $('#PropertyTypeCityDistrict option').remove();

                $('#PropertyTypeCityDistrictDiv').hide();
            }
        });
    });


});