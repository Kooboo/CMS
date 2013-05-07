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
            theme: "advanced",
            plugins: "fullscreen," + (params.autoresize !== false ? 'autoresize' : '') + ",searchreplace,inlinepopups,media,insertPage,KoobooMediaLibrary",
            theme_advanced_toolbar_location: "top",
            theme_advanced_toolbar_align: "left",
            theme_advanced_buttons1: "search,replace,undo,redo,|,insertPage,KoobooMediaLibrary,|,image,media,link,unlink,charmap,|,code,fullscreen",
            theme_advanced_buttons2: "bold,italic,fontselect,forecolor,backcolor,|,justifyleft,justifycenter,justifyright,justifyfull,|,numlist,bullist,formatselect",
            theme_advanced_buttons3: "",
            relative_urls: false,
            convert_urls: false,
            extended_valid_elements: "style[type],script[type|src],iframe[src|style|width|height|scrolling|marginwidth|marginheight|frameborder]",
            verify_html: false,
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

    // hack: window manager
    var create = tinymce.create;
    tinymce.create = function (s) {
        create.apply(this, arguments);
        if (s.indexOf('tinymce.InlineWindowManager') > -1) {
            overrideOpen(tinymce.InlineWindowManager.prototype);
        } else if (s.indexOf('tinymce.WindowManager') > -1) {
            overrideOpen(tinymce.WindowManager.prototype);
        }
    };
    var overrideOpen = function (proto) {
        var open = proto.open;
        proto.open = function () {
            var param = arguments[0];
            this.onOpen.remove(onDialogOpen);
            if (param) {
                if ($.type(param.url) === 'string') {
                    var lowerUrl = param.url.toLowerCase(), flag = '/image.htm'; // end with
                    if (lowerUrl.indexOf(flag) === lowerUrl.length - flag.length) {
                        arguments[0].width += 50;
                        this.onOpen.add(onDialogOpen);
                    }
                }
                if ($.type(param.file) === 'string') {
                    var lowerUrl = param.file.toLowerCase(), flag = '/media.htm'; // end with
                    if (lowerUrl.indexOf(flag) === lowerUrl.length - flag.length) {
                        //arguments[0].height -= 100;
                        this.onOpen.add(onDialogOpen);
                    }
                }
            }
            open.apply(this, arguments);
        };
    };
    var onDialogOpen = function (sender, win) {
        $(win).load(function () {
            // timeout to ensure inject behind all
            setTimeout(function () { injectMediaLibrary(win, sender.editor); }, 0);
        });
    };
    var fireEvent = function (el, type) {
        if (document.all) {
            el.fireEvent('on' + type);
        } else {
            var ev = document.createEvent('HTMLEvents');
            ev.initEvent(type, false, false);
            el.dispatchEvent(ev);
        }
    };
    var setActive = function (win) { win.focus(); };
    var injectMediaLibrary = function (win, editor) {
        // get input and container
        var doc = win.document, input = doc.getElementById('src');
        if (!input) { input = doc.getElementsByName('src')[0]; }
        if (!input) { input = doc.getElementsByTagName('input')[0]; }
        var container = doc.getElementById('srcbrowsercontainer'); // image.htm
        if (!container) { container = doc.getElementById('filebrowsercontainer'); } // media.htm
        if (!container && input) { container = input.parentNode; }
        if (!container) { return; }
        //TODO: virtual path
        var applicationPath = '/';
        // create button
        var temp = doc.createElement('div');
        temp.style.cssText = 'position:absolute;top:-99999px;';
        doc.body.appendChild(temp);
        temp.innerHTML = '<img style="cursor:pointer;margin:3px 0 0 5px;" src="' + applicationPath + 'Styles/Images/media-library.png" />';
        var button = temp.childNodes[0];
        container.appendChild(button);
        doc.body.removeChild(temp);
        // button event
        var poped = false;
        button.onclick = function () {
            setActive(top);
            if (poped) { return; }
            poped = true;
            var onSelected = function (url) {
                poped = false;
                setActive(win);
                input.value = url;
                fireEvent(input, 'change');
            };
            var topJQ = top._jQuery || top.jQuery;
            var id = new Date().getTime();
            topJQ.pop({
                id: id,
                url: editor.getParam('media_library_url'),
                title: editor.getParam('media_library_title'),
                width: 900, height: 500, frameHeight: '100%',
                beforeLoad: function () { },
                onload: function (handle, pop, config) {
                    top.onFileSelected = onSelected;
                    top.fileSelectPop = pop;
                },
                onclose: function (handle, pop, config) {
                    poped = false;
                    delete top.onFileSelected;
                    delete top.fileSelectPop;
                    //setTimeout(function () {
                    //    pop.destory();
                    //    pop.remove();
                    //}, 10);
                }
            });
            tinymce.ztopKoobooDialog(id);
        };
    };

}(jQuery, window.tinymce || window.tinyMCE));
