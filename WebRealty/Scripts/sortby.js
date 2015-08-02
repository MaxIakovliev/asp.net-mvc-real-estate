/// <reference path="_references.js" />

$(function () {
    $("#sortType").change(function () {
        document.forms["sortByForm"].submit();
        //alert("fired");
        //$("#sortByForm").submit();
    });
});