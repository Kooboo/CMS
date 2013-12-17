/*
*   front
*   author: ronglin
*   create date: 2012.02.15
*/

(function ($) {

    $(function () {

        // localization
        var txtRes = {
            editUrl: '',
            dialogTitle: 'Style Editor',
            btnAbleTitle: 'Open Style Editor'
        };
        // localize text resource
        if (window.__localization) { $.extend(txtRes, __localization.front_js); }

        // build html
        var html = [];
        html.push('<var class="vs-visualstyle-front">');
        html.push('<var class="btnAble" title="' + txtRes.btnAbleTitle + '"></var>');
        html.push('</var>');

        // append dom
        var el = $(html.join('')).appendTo('body');

        // dom event
        var cancelClick = false;
        el.children('.btnAble').click(function () {
            if (cancelClick) {
                cancelClick = false;
                return;
            }
            // register effect window
            top.effectWin = window;
            // open editor dialog
            if (top.styleEditingPop) { return; }
            el.css({ opacity: 0.2 }).hide();
            var id = new Date().getTime(), dialogCon, maskCon;
            top.styleEditingPop = $.pop({
                url: txtRes.editUrl,
                title: txtRes.dialogTitle,
                id: id,
                width: 800,
                height: 530,
                padding: 0,
                maxAble: false,
                frameHeight: '100%',
                popupOnTop: true,
                onclose: function () {
                    el.css({ opacity: 1 }).show();
                    dialogCon.remove();
                    maskCon.remove();
                    // break leak
                    top.effectWin = undefined;
                    top.styleEditingPop = undefined;
                }
            });
            // mask opacity
            var contentCon = $('#' + id);
            dialogCon = contentCon.parent('.ui-dialog').first();
            maskCon = dialogCon.next('.ui-widget-overlay').first();
            yardi.zTop(maskCon); yardi.zTop(dialogCon);
            maskCon.css({ opacity: 0.01 });
        });

        // drag move
        var event, region, position = null;
        var enableSelection = function (jqel) { jqel.removeAttr('unselectable').css({ 'MozUserSelect': '', '-moz-user-select': '', '-webkit-user-select': '' }).unbind('selectstart'); };
        var disableSelection = function (jqel) { jqel.attr('unselectable', 'on').css({ 'MozUserSelect': 'none', '-moz-user-select': 'none', '-webkit-user-select': 'none' }).bind('selectstart', function () { return false; }); };
        var docMouseup = function () {
            position = null;
            enableSelection($('body'));
            $(document).unbind('mouseup', docMouseup).unbind('mousemove', docMousemove);
        };
        var docMousemove = function (ev) {
            var offset = {
                x: event.pageX - ev.pageX,
                y: event.pageY - ev.pageY
            };
            if (Math.abs(offset.x) > 10 || Math.abs(offset.y) > 10) {
                if (!position) {
                    cancelClick = true;
                    position = {
                        top: parseInt(el.css('top').replace(/\D/g, '')),
                        right: parseInt(el.css('right').replace(/\D/g, ''))
                    };
                    region = {
                        h: $(window).width() - el.outerWidth(),
                        v: $(window).height() - el.outerHeight()
                    };
                }
                el.css({
                    top: Math.min(Math.max(position.top - offset.y, 0), region.v),
                    right: Math.min(Math.max(position.right + offset.x, 0), region.h)
                });
            }
        };
        el.mousedown(function (ev) {
            event = ev;
            disableSelection($('body'));
            $(document).mouseup(docMouseup).mousemove(docMousemove);
        });

    });

    // an custom entrance for iframe environment in dialog. eg:top._jQuery
    if (!window._jQuery) { window._jQuery = $; }
    window._jQueryOfStyleEditing = $;

    // attention:
    // this must be called at the last of component,
    // this file was loaded at last and this function place at the end.
    // resolve the confilict problem, when there ara more then one jquery instance in the page.
    $.noConflict(true);

} (jQuery));
