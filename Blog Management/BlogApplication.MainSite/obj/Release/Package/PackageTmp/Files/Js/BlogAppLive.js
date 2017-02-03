(function ($) {
    $.fn.BlogAppLive = function () {
        return $.BlogAppLive;
    };
    $.BlogAppLive = function () {

    };
    $.BlogAppLive.Settings = {
        alphaNumeric: /^[a-z0-9]+$/i,
        codeISO: /^[a-z]{2}-[A-Z]{2}$/i,
        codeISO3: /^[a-z]{3}-[A-Z]{3}$/i,
        twitterStatusURL: /^https?:\/\/twitter\.com\/(?:#!\/)?(\w+)\/status(es)?\/(\d+)$/
    };

    $.BlogAppLive.Initialize = function () {
        $.BlogAppLive.ReInitialize();
        $.BlogAppLive.Validator();
    };
    $.BlogAppLive.ReInitialize = function () {
        $(document).ready(function () {
            $("img.lazy").unveil();
        });
    }

    $.BlogAppLive.Facebook = {
        Share : function(url) {
            FB.ui({
                method: 'share',
                href: url,
            }, function (response) { });
        }
    }

    $.BlogAppLive.Twitter = {
        Tweet : function(url) {
            var width  = 575,
                height = 400,
                left   = ($(window).width()  - width)  / 2,
                top    = ($(window).height() - height) / 2,
                opts   = 'status=1' +
                 ',width='  + width  +
                 ',height=' + height +
                 ',top='    + top    +
                 ',left=' + left;
            url = 'https://twitter.com/intent/tweet?url=' + $.BlogAppLive.Utility.htmlEscape(url);
            window.open(url, 'twitter', opts);

            return false;
        }
    }

    $.BlogAppLive.Utility = {
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
    $.BlogAppLive.Validator = function () {
       

    };



    $.BlogAppLive.Initialize();
}(jQuery));