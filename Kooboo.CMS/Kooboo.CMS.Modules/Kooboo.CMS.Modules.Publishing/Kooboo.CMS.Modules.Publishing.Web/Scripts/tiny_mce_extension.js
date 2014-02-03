/*
*   tinymce extension
*   author: ronglin
*   create date: 2011.5.24
*/

(function ($, tinymce) {

    if (!$ || !tinymce) { return; }

    // get global config
    tinymce.getKoobooConfig = function (params) {
        params = params || {};
        return {
            mode: "exact",
            theme: "modern",
            menubar: false,
            toolbar_items_size: 'small',
            resize: false,
            plugins: [
                "advlist autolink lists link image charmap anchor",
                "searchreplace visualblocks rawcode fullscreen",
                "media table contextmenu paste insertPage textcolor"
            ],
            toolbar: "searchreplace undo redo | bold italic forecolor formatselect | indent outdent | alignleft aligncenter alignright alignjustify | bullist numlist | image link unlink insertPage | rawcode fullscreen",
            relative_urls: false,
            convert_urls: false,
            extended_valid_elements: "style[type],script[type|src],iframe[src|style|width|height|scrolling|marginwidth|marginheight|frameborder]",
            verify_html: false,
            valid_elements: '*[*]',         
            valid_children: "+body[style]" //http://tinymce.moxiecode.com/wiki.php/Configuration:valid_children
    };
};

// apply top zIndex
tinymce.ztopKoobooDialog = function (id) {
    var topJQ = top._jQuery || top.jQuery;
    var yardi = window.yardi || top.yardi;
    var contentCon = topJQ('#' + id);
    if (contentCon.length > 0) {
        var dialogCon = contentCon.parent('.ui-dialog').first();
        var maskCon = dialogCon.next('.ui-widget-overlay').first();
        if (yardi) {
            yardi.zTop(maskCon);
            yardi.zTop(dialogCon);
        } else {
            // get max zIndex
            var max = function () {
                var zmax, cur;
                topJQ('*').each(function () {
                    cur = parseInt($(this).css('z-index'));
                    zmax = cur > (zmax || 0) ? cur : zmax;
                });
                return zmax;
            }() || 0;
            // apply
            maskCon.css('z-index', (parseInt(maskCon.css('z-index')) || 0) + max);
            dialogCon.css('z-index', (parseInt(dialogCon.css('z-index')) || 0) + max);
        }
    }
};

// tinymce comment out the javascript and style content by default
// it need to be restored to original format, and available for cms.
var uncommentHtml = function (val) {
    var rscript = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi;
    val = val.replace(rscript, function (matched) {
        return matched.replace('>// <![CDATA[', '>').replace('// ]]></', '</');
    });
    var rstyle = /<style\b[^<]*(?:(?!<\/style>)<[^<]*)*<\/style>/gi;
    val = val.replace(rstyle, function (matched) {
        return matched.replace('><!--', '>').replace('--></', '</');
    });
    return val;
};

// to fix the problem when custom html all css style are reseted
// we decide to add a class name(unreset) to all elements at top layer.
// attention: can not use jQuery to analyze, jQuery will ignore script tag.
var unresetHtml = function (val) {
    var wrap = document.createElement('div');
    wrap.style.position = 'absolute';
    wrap.style.left = '-10000px';
    wrap.style.top = '-10000px';
    document.body.appendChild(wrap);
    // fix bug: set html to element innerHTML, while style or script tag at the first position of html.
    // ie6 ie7 ie8 will remove the first style or script tag, so add a temporary span at front of the html.
    if ($.browser.msie && $.browser.version < 9) {
        wrap.innerHTML = '<span>temp</span>' + val;
        wrap.removeChild(wrap.firstChild);
    } else {
        wrap.innerHTML = val;
    }
    var ignoreTags = '|STYLE|SCRIPT|NOSCRIPT|'.toUpperCase();
    for (var i = 0; i < wrap.childNodes.length; i++) {
        var child = wrap.childNodes[i];
        if (child.nodeType === 1) { // element type
            if (ignoreTags.indexOf('|' + child.tagName + '|') === -1) {
                $(child).addClass('unreset');
            }
        }
    }
    val = wrap.innerHTML;
    document.body.removeChild(wrap);
    return val;
};

// hack: getContent
var getContent = tinymce.Editor.prototype.getContent;
tinymce.Editor.prototype.getContent = function () {
    var content = getContent.apply(this, arguments);
    // for html source (tinymce plugin)
    if (arguments[0] && arguments[0].source_view === true) {
        return uncommentHtml(content);
    } else {
        return content;
    }
};

// hack: triggerSave
var triggerSave = tinymce.triggerSave;
tinymce.triggerSave = function () {
    triggerSave.apply(this, arguments);
    // core
    var len = tinymce.editors.length;
    for (var i = 0; i < len; i++) {
        var id = tinymce.editors[i].id;
        var textarea = $('#' + id);
        if (!textarea.length) { textarea = $('[name="' + id + '"]'); }
        textarea.each(function () {
            var val = $(this).val();
            //val = unresetHtml(val);
            val = uncommentHtml(val);
            $(this).val(val);
        });
    }
};

}(jQuery, window.tinymce || window.tinyMCE));
