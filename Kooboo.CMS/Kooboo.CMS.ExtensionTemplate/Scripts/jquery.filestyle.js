/*
* Style File - jQuery plugin for styling file input elements
*  
* Copyright (c) 2007-2008 Mika Tuupola
*
* Licensed under the MIT license:
*   http://www.opensource.org/licenses/mit-license.php
*
* Based on work by Shaun Inman
*   http://www.shauninman.com/archive/2007/09/10/styling_file_inputs_with_css_and_the_dom
*
* Revision: $Id: jquery.filestyle.js 303 2008-01-30 13:53:24Z tuupola $

*** WARNING: The file style plugin was changed intent to clear the old file input value. Jifeng.

*
*/

(function ($) {

    $.fn.filestyle = function (options) {
        /* TODO: This should not override CSS. */
        var settings = {
            width: 250
        };

        if (options) {
            $.extend(settings, options);
        };

        return this.each(function () {

            var self = this;
            var wrapper = $("<a>").addClass('file-upload').append("<span></span>");

            var filename = $('<input class="long" type="text">').val($(self).data('value')).addClass($(self).attr("class")).focus(function () {
                filename.get(0).select();
            }).attr('name', $(self).attr('name'));

            $(self).before(filename);
            $(self).wrap(wrapper);


            $(self).bind("change", function () {
                filename.val($(self).val().replace(/(c:\\)*fakepath\\/i, '')).attr('disabled', true);
                $(self).after($("<input type='hidden'></input>").attr('name', $(self).attr('name')));
            });

        });


    };

})(jQuery);
