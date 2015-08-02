/// <reference path="_references.js" />

$(function () {
    $("#SearchTabs").tabs();
    

    $("#advancedSearchLink").click(function () {

        $("#advancedSearchDialog").dialog({
            height: 600,
            width: 500,
            modal: true
        });

        var currentPath = location.pathname.toLowerCase();
        //alert(currentPath);
        if (currentPath.indexOf("searchbox") == -1) {
            // alert("1");
            $("#advancedSearchContent").load("SearchBox/AdvancedSearchBox");
        }
        else {
            // alert("2");
            $("#advancedSearchContent").load("AdvancedSearchBox");
        }

        return false;
    });

    //меню недвижимость
    //        $("#lnkMenuNedvigimost").hover(function () {
    //            $("#divNewsMenu").hide();
    //            $("#divCompanyMenu").hide();
    //            $("#divNedvigemostMenu").show();
    //        },
    //        function () {
    //            $("#divNewsMenu").hide();
    //            $("#divCompanyMenu").hide();
    //            $("#divNedvigemostMenu").hide();
    //        }  
    //        );

    //    $("#divNedvigemostMenu").hover(function () {
    //       // $("#divNewsMenu").hide();
    //       // $("#divCompanyMenu").hide();
    //       // $("#divNedvigemostMenu").show();
    //       
    //    }
    //    , function () {
    //        $("#divNewsMenu").hide();
    //        $("#divCompanyMenu").hide();
    //        $("#divNedvigemostMenu").hide();
    //    });
    //$("#form2 input[name=name]").val('Hello World!');


    //    //Меню новости
    //    $("#lnkMenuNews").hover(function () {
    //        $("#divNewsMenu").show();
    //        $("#divNedvigemostMenu").hide();
    //        $("#divCompanyMenu").hide();
    //    });

    //    $("#divNewsMenu").hover(function () {
    //        $("#divNewsMenu").show();
    //        $("#divNedvigemostMenu").hide();
    //        $("#divCompanyMenu").hide();
    //    },
    //     function () {
    //         $("#divNewsMenu").hide();
    //         $("#divNedvigemostMenu").hide();
    //         $("#divCompanyMenu").hide();
    //     }
    //     );


    //Меню компании
    $("#lnkMenuCompany").hover(function () {
        $("#divNewsMenu").hide();
        $("#divNedvigemostMenu").hide();
        $("#divCompanyMenu").show();

    });

    $("#divCompanyMenu").hover(function () {
        $("#divNewsMenu").hide();
        $("#divNedvigemostMenu").hide();
        $("#divCompanyMenu").show();

    },
     function () {
         $("#divNewsMenu").hide();
         $("#divNedvigemostMenu").hide();
         $("#divCompanyMenu").hide();

     }
     );

    var lastOpened = "";
    var newsOpened = "";
    var companyOpened = "";
    var PropertyTypesPathOpened = "";
    var PropertyActionPathOpened = "";
    var CityPathOpened = "";
    var DistrictPathOpened = "";


    var preventDivNedvigemostMenu = 0;
    var preventDivNewsMenu = 0;
    var preventDivCompanyMenu = 0;
    var preventPropertyTypesPath = 0;
    var preventPropertyActionPath = 0;
    var preventCityPath = 0;
    var preventDistrictPath = 0;


    $(window).bind('mousemove', function (e) {
        if ($(e.target).attr('id') == 'lnkMenuNedvigimost') {
            //lastOpened = $(e.target).attr('id');
            //$(window).unbind('mousemove');
            lastOpened = "";
            $("#divCompanyMenu").hide();
            $("#divNewsMenu").hide();
            $("#divNedvigemostMenu").show();
        }
        else if ($(e.target).attr('id') != 'lnkMenuNedvigimost' && $(e.target).attr('id') != 'divNedvigemostMenu') {
            if (lastOpened == "") {
                lastOpened = "executed";
                setTimeout(function () {
                    if (preventDivNedvigemostMenu == 0) {
                        $("#divNedvigemostMenu").hide();
                    }
                }, 100);
            }

        }
        if ($(e.target).attr('id') == 'lnkMenuNews') {
            $("#divNewsMenu").show();
            $("#divNedvigemostMenu").hide();
            $("#divCompanyMenu").hide();
            newsOpened = "";
        }
        else if ($(e.target).attr('id') != 'lnkMenuNews' && $(e.target).attr('id') != 'divNewsMenu') {

            if (newsOpened == "") {
                newsOpened = "executed";
                setTimeout(function () {
                    if (preventDivNewsMenu == 0) {
                        $("#divNewsMenu").hide();
                    }
                }, 100);
            }
        }

        if ($(e.target).attr('id') == 'lnkMenuCompany') {
            $("#divNewsMenu").hide();
            $("#divNedvigemostMenu").hide();
            $("#divCompanyMenu").show();
            companyOpened = "";
        }
        else if ($(e.target).attr('id') != 'lnkMenuCompany' && $(e.target).attr('id') != 'divCompanyMenu') {

            if (companyOpened == "") {
                companyOpened = "executed";
                setTimeout(function () {
                    if (preventDivCompanyMenu == 0) {
                        $("#divCompanyMenu").hide();
                    }
                }, 100);
            }
        }


        if ($(e.target).attr('id') == 'lnkPropertyTypePath') {
            $("#divPropertyTypesPath").show();
            PropertyTypesPathOpened = "";
        }
        else if ($(e.target).attr('id') != 'lnkPropertyTypePath' && $(e.target).attr('id') != 'divPropertyTypesPath') {

            if (PropertyTypesPathOpened == "") {
                PropertyTypesPathOpened = "executed";
                setTimeout(function () {
                    if (preventPropertyTypesPath == 0) {
                        $("#divPropertyTypesPath").hide();
                    }
                }, 100);
            }
        }

        if ($(e.target).attr('id') == 'lnkPropertyActionPath') {
            $("#divPropertyActionsPath").show();
            PropertyActionPathOpened = "";
        }
        else if ($(e.target).attr('id') != 'lnkPropertyActionPath' && $(e.target).attr('id') != 'divPropertyActionsPath') {

            if (PropertyActionPathOpened == "") {
                PropertyActionPathOpened = "executed";
                setTimeout(function () {
                    if (preventPropertyActionPath == 0) {
                        $("#divPropertyActionsPath").hide();
                    }
                }, 100);
            }
        }


        if ($(e.target).attr('id') == 'lnkCityPath') {
            $("#divCityPath").show();
            CityPathOpened = "";
        }
        else if ($(e.target).attr('id') != 'lnkCityPath' && $(e.target).attr('id') != 'divCityPath') {

            if (CityPathOpened == "") {
                CityPathOpened = "executed";
                setTimeout(function () {
                    if (preventCityPath == 0) {
                        $("#divCityPath").hide();
                    }
                }, 100);
            }
        }


        if ($(e.target).attr('id') == 'lnkDistrictPath') {
            $("#divDistrictPath").show();
            DistrictPathOpened = "";
        }
        else if ($(e.target).attr('id') != 'lnkDistrictPath' && $(e.target).attr('id') != 'divDistrictPath') {

            if (DistrictPathOpened == "") {
                DistrictPathOpened = "executed";
                setTimeout(function () {
                    if (preventDistrictPath == 0) {
                        $("#divDistrictPath").hide();
                    }
                }, 100);
            }
        }



    });



    $("#divNedvigemostMenu").hover(function () {
        preventDivNedvigemostMenu = 1;
    })

    $("#divNewsMenu").hover(function () {
        preventDivNewsMenu = 1;
    })

    $("#divCompanyMenu").hover(function () {
        preventDivCompanyMenu = 1;
    })

    $("#divPropertyTypesPath").hover(function () {
        preventPropertyTypesPath = 1;
    })

    $("#divPropertyActionsPath").hover(function () {
        preventPropertyActionPath = 1;
    })

    $("#divCityPath").hover(function () {
        preventCityPath = 1;
    })

    $("#divDistrictPath").hover(function () {
        preventDistrictPath = 1;
    })
    




    $("#divNedvigemostMenu").mouseleave(function () {
        $("#divNedvigemostMenu").hide();
        preventDivNedvigemostMenu = 0;
    });

    $("#divNewsMenu").mouseleave(function () {
        $("#divNewsMenu").hide();
        preventDivNewsMenu = 0;
    });

    $("#divCompanyMenu").mouseleave(function () {
        $("#divCompanyMenu").hide();
        preventDivCompanyMenu = 0;
    });


    $("#divPropertyTypesPath").mouseleave(function () {
        $("#divPropertyTypesPath").hide();
        preventPropertyTypesPath = 0;
    });

    $("#divPropertyActionsPath").mouseleave(function () {
        $("#divPropertyActionsPath").hide();
        preventPropertyActionPath = 0;
    });

    $("#divCityPath").mouseleave(function () {
        $("#divCityPath").hide();
        preventCityPath = 0;
    });

    $("#divDistrictPath").mouseleave(function () {
        $("#divDistrictPath").hide();
        preventDistrictPath = 0;
    });



});