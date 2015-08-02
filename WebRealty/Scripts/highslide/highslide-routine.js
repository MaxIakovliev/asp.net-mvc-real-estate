/// <reference path="../_references.js" />
$(function () {
    hs.graphicsDir = '../../Content/highslide/graphics/';
    hs.outlineType = 'rounded-white';
    $(".highslide").each(function () {

        $(this).click(function () {
            return hs.expand(this);
        });
    });

});