(function ($) {
    $.fn.BlogApp = function () {
        return $.BlogApp;
    };
    $.BlogApp = function () {

    };
    $.BlogApp.Settings = {
        alphaNumeric: /^[a-z0-9]+$/i,
        codeISO: /^[a-z]{2}-[A-Z]{2}$/i,
        codeISO3: /^[a-z]{3}-[A-Z]{3}$/i,
        twitterStatusURL: /^https?:\/\/twitter\.com\/(?:#!\/)?(\w+)\/status(es)?\/(\d+)$/
    };

    $.BlogApp.Facebook = {
        Share : function(url) {
            FB.ui({
                method: 'share',
                href: url,
            }, function (response) { });
        }
    };

    $.BlogApp.Initialize = function () {
        $.BlogApp.ReInitialize();
        $.BlogApp.Validator();
    };
    $.BlogApp.ReInitialize = function () {
        $(document).ready(function () {
            $(".lazyImg").lazyload();


        });
    }

    $.BlogApp.Utility = {
        htmlEscape: function (str) {
            return String(str)
           .replace(/&/g, '&amp;')
           .replace(/"/g, '&quot;')
           .replace(/'/g, '&#39;')
           .replace(/</g, '&lt;')
           .replace(/>/g, '&gt;');
        },
        htmlUnescape: function (value) {
            return String(value)
       .replace(/&quot;/g, '"')
       .replace(/&#39;/g, "'")
       .replace(/&lt;/g, '<')
       .replace(/&gt;/g, '>')
       .replace(/&amp;/g, '&');
        },
        Twitter: {
            validateTwitterStatus: function (value) {
                return $.BlogApp.Settings.twitterStatusURL.test(value);
            },
            validataTwitterEmbed: function (value) {
                return value.indexOf("<blockquote") > -1 && value.indexOf("</blockquote>") > -1;
            }
        }


    }
    $.BlogApp.Validator = function () {
        jQuery.validator.addMethod("alphaNumeric", function (value, element) {
            return $.BlogApp.Settings.alphaNumeric.test(value);
        }, "* must be alpha-numeric");
        jQuery.validator.addMethod("codeISO", function (value, element) {
            return $.BlogApp.Settings.codeISO.test(value);
        }, "* must be in the correct format.");
        jQuery.validator.addMethod("codeISO3", function (value, element) {
            return $.BlogApp.Settings.codeISO3.test(value);
        }, "* must be in the correct format.");

    };



    $.BlogApp.Initialize();
}(jQuery));