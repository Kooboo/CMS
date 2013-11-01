/*
*   anchor fix
*   author: ronglin
*   create date: 2012.9.17
*   description: when user start select something on page, this component hide all the anchors.
*                this fix for anchor is use for aviod masking and disturbing the user select action.
*/

(function (ctx, $) {

    ctx.anchorFix = function (win) {

        var doc = win.document, mouseup, handlers = [], downing = false, distributed = false;

        var getBrowserSelection = function () {
            if (doc.selection && !win.opera) {
                return doc.selection;
            } else {
                if (win.getSelection) {
                    return win.getSelection();
                }
            }
        };

        var hasSelection = function () {
            var sel = getBrowserSelection();
            return !!sel && ($.browser.msie ? !(/None/i).test(sel.type) : !!sel.rangeCount);
        };

        $(doc).mouseup(mouseup = function () {
            downing = false;
            if (distributed) {
                distributed = false;
                $.each(handlers, function () {
                    this.end.call(win);
                });
            }
        }).mousedown(function () {
            mouseup();
            downing = true;
        }).mousemove(function () {
            if (downing && !distributed && hasSelection()) {
                distributed = true;
                $.each(handlers, function () {
                    this.start.call(win);
                });
            }
        });

        return {

            hasSelection: function () {
                return (downing && hasSelection());
            },

            register: function (start, end) {
                handlers.push({ start: start, end: end });
            },

            unregister: function (start, end) {
                for (var len = handlers.length, i = len - 1; i > -1; i--) {
                    if (handlers[i].start == start && handlers[i].end == end) {
                        handlers.splice(i, 1);
                    }
                }
            }
        };

    } (window);

    // register
    ctx.anchorFix.register(function () {
        ctx.anchor.hideAll();
        ctx.anchor.fixAll(true);
    }, function () {
        ctx.anchor.sleep(128);
        ctx.anchor.fixAll(false);
    });

})(yardi, jQuery);
