/*
*
* nest
* author: ronglin
* create date: 2010.12.20
*
*/

(function ($) {

    // text resource
    var options = {
        leaveConfirm: 'Are you sure you want to leave page? All your changes might be lost.',
        undoBtnTitle: 'Undo',
        redoBtnTitle: 'Redo'
    };

    // override text resource
    if (window.__pageDesign) { $.extend(options, __pageDesign); }

    // app settings
    var settings = {
        // iframe
        iframeSelector: '#pageDesign',
        iframeLoading: '#iframeLoading',
        iframeMask: '#iframeMask',
        iframeMarkup: '<iframe id="pageDesign" width="100%" height="500" frameborder="0" scrolling="yes" src="javascript:false;"></iframe>',
        // redo undo
        btnUndo: '#btnUndo',
        btnRedo: '#btnRedo',
        // hidden inputs
        pagePositionsJson: 'input[name="PagePositionsJson"]', // post used
        pageDesignJs: '#PageDesignJs',
        pageDesignCss: '#PageDesignCss',
        pageDesignUrl: '#PageDesignUrl'
    };

    var designContext = null;
    var dialogObject = null;
    var isSubmit = false;
    var pageDesignOnLoadHandlers = [];

    window.pageDesignLoaded = function (designCtx, contentWindow) {
        designContext = designCtx;
        if (designCtx) {
            bindDesignerDialog(designCtx);
            bindRedoundo(designCtx.getRedoundo());
            // leave confirm
            if (options.leaveConfirm) {
                // before unload, unbind first for this function maight be call many times.
                $(window).unbind('beforeunload').bind('beforeunload', function () {
                    if ((designCtx.isChanged()) && !isSubmit) {
                        return options.leaveConfirm;
                    }
                });
            }
        }
        // hide
        $(settings.iframeMask).hide();
        $(settings.iframeLoading).hide();
        window.loading.hide();
        // submit
        $('*[type="submit"]').removeAttr('disabled');
        // event handlers
        $.each(pageDesignOnLoadHandlers, function (i, fn) { fn(); });
    };

    window.pageDesignUnload = function () {
        // show iframe mask
        $(settings.iframeMask).show();
        $(settings.iframeLoading).show();
        window.loading.show();
        // submit
        $('*[type="submit"]').attr('disabled', 'DISABLED');
    };

    window.pageDesignOnLoad = function (fn) {
        pageDesignOnLoadHandlers.push(fn);
    };

    function bindRedoundo(redoundo) {
        var btnUndo = $(settings.btnUndo).unbind('click').click(function () { redoundo.undo(); return false; });
        var btnRedo = $(settings.btnRedo).unbind('click').click(function () { redoundo.redo(); return false; });
        var disableFunc = function () {
            if (redoundo.canUndo()) {
                btnUndo.attr('title', options.undoBtnTitle + ' ' + redoundo.prevCmd().name);
                btnUndo.removeAttr('disabled');
            } else {
                btnUndo.attr('title', options.undoBtnTitle);
                btnUndo.attr('disabled', 'DISABLED');
            }
            if (redoundo.canRedo()) {
                btnRedo.attr('title', options.redoBtnTitle + ' ' + redoundo.nextCmd().name);
                btnRedo.removeAttr('disabled');
            } else {
                btnRedo.attr('title', options.redoBtnTitle);
                btnRedo.attr('disabled', 'DISABLED');
            }
        };
        disableFunc();
        redoundo.onUndo.add(disableFunc);
        redoundo.onRedo.add(disableFunc);
        redoundo.onCommit.add(disableFunc);
    }

    function removeDialog() {
        if (dialogObject) {
            var iframe = dialogObject.find('iframe').get(0);
            // break leaks
            iframe.outerApi = undefined;
            try { delete iframe['outerApi']; } catch (ex) { }
            iframe.src = 'javascript:false;';
            iframe.parentNode.removeChild(iframe);
            // remove
            dialogObject.remove();
            dialogObject = undefined;
        }
    }

    function bindDesignerDialog(designCtx) {
        designCtx.showDialog = function (cfg, onOk, outer) {
            removeDialog();
            var settings = $.extend({}, cfg, {
                position: 'center',
                modal: true,
                width: 750,
                height: 500,
                dialogClass: 'iframe-dialog'
            });
            dialogObject = $('<div style="overflow:hidden;"></div>');
            dialogObject.appendTo('body').dialog(settings);
            dialogObject.bind('dialogclose', removeDialog);
            // fix ie9 bug, append iframe after the dialog is showed.
            dialogObject.append('<iframe frameBorder="0" style="width:100%;height:100%;" src="' + settings.url + '"></iframe>');
            // concat
            dialogObject.children('iframe').get(0).outerApi = $.extend({}, {
                onInnerOk: function (html) { onOk(html); },
                onInnerClose: function () { dialogObject.dialog('close'); }
            }, outer || {});
        };
    }

    var nodeOnload = {
        bind: function (node, handler) {
            var listener = function () {
                if (node && node.readyState && /^(?!(?:loaded|complete)$)/.test(node.readyState))
                    return;
                nodeOnload.unbind(node, arguments.callee);
                node.onload = node.onreadystatechange = node.onerror = null;
                handler.call(node);
            };
            if (node.addEventListener) {
                node.addEventListener('load', listener, false);
                node.addEventListener('error', listener, false);
                node.addEventListener('readystatechange', listener, false);
            } else if (node.attachEvent) {
                node.attachEvent('onload', listener);
                node.attachEvent('onerror', listener);
                node.attachEvent('onreadystatechange', listener);
            } else {
                node.onload = node.onreadystatechange = node.onerror = listener;
            }
        },
        unbind: function (node, listener) {
            if (node.removeEventListener) {
                node.removeEventListener('load', listener, false);
                node.removeEventListener('error', listener, false);
                node.removeEventListener('readystatechange', listener, false);
            } else if (node.detachEvent) {
                node.detachEvent('onload', listener);
                node.detachEvent('onerror', listener);
                node.detachEvent('onreadystatechange', listener);
            } else {
                node.onload = node.onreadystatechange = node.onerror = null;
            }
        }
    };

    // insert iframe component
    // for high efficiency, this action executed immediately after the nest.js loaded
    // make sure to put the markup of <script src="nest.js"> after iframe loading element.
    var iframeUrl = decodeURIComponent($(settings.pageDesignUrl).val());
    $(settings.iframeMarkup).insertAfter(settings.iframeMask);
    $(settings.iframeSelector).load(function () {
        // get doc
        var doc;
        try {
            doc = this.contentWindow.document;
            var test = doc.body.childNodes;
        } catch (ex) { // is xml document
            window.pageDesignLoaded(null, null);
            return;
        }
        // con
        var head = doc.getElementsByTagName('head'), con;
        if (head && head[0]) {
            con = head[0];
        } else {
            con = doc.body;
        }
        // css
        var cssHtml = decodeURIComponent($(settings.pageDesignCss).val());
        $(con).append(cssHtml);
        // js
        var jsHtml = decodeURIComponent($(settings.pageDesignJs).val());
        jsHtml = jsHtml.replace(/<script/g, '<div');
        jsHtml = jsHtml.replace(/<\/script>/g, '</div>');
        var cache = $(jsHtml).appendTo('body'), index = -1;
        (function () {
            index++;
            var callee = arguments.callee;
            if (!cache[index]) {
                cache.remove();
                return;
            }
            var src = cache.eq(index).attr('src');
            if (!src) {
                callee();
                return;
            }
            var node = document.createElement('script');
            node.type = 'text/javascript';
            node.src = src;
            nodeOnload.bind(node, callee);
            con.appendChild(node);
        })();
    }).attr('src', iframeUrl);

    $(window).bind('unload', function () {
        // if exist
        removeDialog();
        // break leaks
        designContext = undefined;
        window.pageDesignLoaded = undefined;
        window.pageDesignUnload = undefined;
        // remove designer iframe
        try { // some ie9 client throw error.
            var iframe = $(settings.iframeSelector).get(0);
            if (iframe) {
                iframe.src = 'javascript:false;';
                iframe.parentNode.removeChild(iframe);
            }
        } catch (ex) { }
    });

    $(function () {
        // I can not ensure kooboo.cms.js loaded before nest.js.
        // so put this in jQuery ContentLoaded event.
        window.ajaxFormParam = {
            beforeSerialize: function ($form, options) {
                if (!designContext) { return; }
                isSubmit = true;
                // in ie7, the array object in inner iframe content window scope, 
                // when it pass to the top window scope is become to a native object but not a array.
                // so now parse the setting object to json string in inner iframe scope.
                var json = designContext.getSettingsJson(); //var json = $.toJSON(designContext.getSettings());
                $(settings.pagePositionsJson).val(encodeURIComponent(json));
            }
        };
    });

})(jQuery);
