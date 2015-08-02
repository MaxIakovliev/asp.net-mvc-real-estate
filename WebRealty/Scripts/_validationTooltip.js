/// <reference path="_references.js" />

$(function () {
    //validation - make sure this is included after jquery.validate.unobtrusive.js
    //unobtrusive validate plugin overrides all defaults, so override them again
    $('form').each(function () {
        OverrideUnobtrusiveSettings(this);
    });
    //in case someone calls $.validator.unobtrusive.parse, override it also
    var oldUnobtrusiveParse = $.validator.unobtrusive.parse;
    $.validator.unobtrusive.parse = function (selector) {
        oldUnobtrusiveParse(selector);
        $('form').each(function () {
            OverrideUnobtrusiveSettings(this);
        });
    };
    //replace validation settings function
    function OverrideUnobtrusiveSettings(formElement) {
        var settngs = $.data(formElement, 'validator').settings;
        //standard qTip2 stuff copied from sample
        settngs.errorPlacement = function (error, element) {
            // Set positioning based on the elements position in the form
            var elem = $(element);


            // Check we have a valid error message
            if (!error.is(':empty')) {
                // Apply the tooltip only if it isn't valid
                elem.filter(':not(.valid)').qtip({
                    overwrite: false,
                    content: error,
                    position: {
                        my: 'center left',  // Position my top left...
                        at: 'center right', // at the bottom right of...
                        viewport: $(window)
                    },
                    show: {
                        event: false,
                        ready: true
                    },
                    //hide: false,
                    hide: { when: { event: 'mousemove' },
                        delay: 1000,
                        effect: function () { $(this).qtip("destroy"); }
                    },
                    style: {
                        classes: 'qtip-red' // Make it red... the classic error colour!
                    }
                })
                // If we have a tooltip on this element already, just update its content
                .qtip('option', 'content.text', error);
            }

            // If the error is empty, remove the qTip
            else { elem.qtip('destroy'); }
        };

        settngs.success = $.noop;
    }

});