/*
*   widgetMask
*   author: ronglin
*   create date: 2011.02.09
*/

/*
* config parameters:
* cssClass, [containerNode], [startNode,[endNode]]
*/

(function (ctx, $) {

    var minWidth = 20, minHeight = 20;

    var widgetMask = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    widgetMask.prototype = {

        maskEl: null, borderCon: null, regionCon: null,

        cssClass: null, containerNode: null, startNode: null, endNode: null,

        initialize: function () {
            if (!this.endNode && this.startNode) {
                this.endNode = this.findEndNode();
                if (!this.endNode) {
                    alert('inline edit dom error! can not find the end flag node');
                    return;
                }
            }
        },

        findEndNode: function () {
            var start = this.startNode.get(0);
            var end, find = function (next) {
                while (next) {
                    if (next.tagName === 'VAR' && $(next).attr('end') === 'true') {
                        end = next;
                        break;
                    }
                    next = next.nextSibling;
                }
            };
            find(start.nextSibling);
            // fix element nest error
            // eg: <a><var start />Sample<a>Site</a><var end /></a> 
            // browser render it as: <a><var start />Sample</a><a>Site</a><var end />
            if (!end) {
                var parent = start.parentNode;
                find(parent.nextSibling);
                if (end) {
                    // force the escaped elements back to right container
                    var node = parent.nextSibling;
                    while (node) {
                        parent.appendChild(node);
                        if (node == end) { break; }
                        node = parent.nextSibling;
                    }
                }
            }
            return end ? $(end) : undefined;
        },

        registerResize: function () {
            this.unregisterResize();
            var self = this;
            this._correctPosition = function () { self.correctPosition(); };
            $(window.top).monitorResize(this._correctPosition);
        },

        unregisterResize: function () {
            if (this._correctPosition) {
                $(window.top).unmonitorResize(this._correctPosition);
                this._correctPosition = null;
            }
        },

        correctPosition: function () {
            // mask
            if (this.maskEl) { this.maskPosition(); }
            // border
            if (this.borderCon) { this.borderPosition(); }
            // region
            if (this.regionCon) { this.regionPosition(); }
            // check
            if (!this.maskEl && !this.borderCon) { this.unregisterResize(); }
        },

        genStyle: function () {
            var position, width, height;
            // container mode
            if (this.containerNode) {
                position = this.containerNode.offset();
                width = this.containerNode.outerWidth();
                height = this.containerNode.outerHeight();
                // ret
                return {
                    top: position.top,
                    left: position.left,
                    width: Math.max(width || 0, minWidth),
                    height: Math.max(height || 0, minHeight)
                }
            }
            // startToEnd mode.
            var start = this.startNode.get(0), end = this.endNode.get(0), p = start.parentNode;
            if (p.firstChild == start && p.lastChild == end) {
                var parent = $(p);
                position = parent.offset(), width = parent.outerWidth(), height = parent.outerHeight();
            } else {
                var next = start.nextSibling;
                var minX, minY, maxX, maxY, set = function (el) {
                    if (el.filter(':visible').length === 0) { return; } // filter out the elements like style/script etc.
                    var pos = el.offset(), w = el.outerWidth(), h = el.outerHeight();
                    if (w === 0 && h === 0) { return; }
                    minX = Math.min(minX || 99999999, pos.left);
                    minY = Math.min(minY || 99999999, pos.top);
                    maxX = Math.max(maxX || 0, pos.left + w);
                    maxY = Math.max(maxY || 0, pos.top + h);
                };
                while (next != end) {
                    // element node
                    if (next.nodeType === 1)
                    { set($(next)); }
                    // text node
                    else if (next.nodeType === 3) {
                        var wrap = document.createElement('var');
                        wrap.style.display = 'inline';
                        p.insertBefore(wrap, next);
                        wrap.appendChild(next);
                        set($(wrap));
                        p.insertBefore(next, wrap);
                        p.removeChild(wrap);
                    }
                    next = next.nextSibling;
                }
                position = { top: minY, left: minX };
                width = maxX - minX;
                height = maxY - minY;
            }
            // ret
            return {
                top: position.top,
                left: position.left,
                width: Math.max(width || 0, minWidth),
                height: Math.max(height || 0, minHeight),
                originalWidth: width || 0,
                originalHeight: height || 0
            };
        },

        mask: function () {
            this.unmask();
            // create
            this.maskEl = $('<var style="position:absolute;"></var>').appendTo(ctx.cacheCon || 'body');
            // css
            this.maskEl.addClass(this.cssClass);
            this.maskPosition();
            ctx.zTop(this.maskEl);
            // event
            this.registerResize();
        },

        maskPosition: function () {
            if (!this.maskEl) { return; }
            this.maskEl.css(this.genStyle());
        },

        region: function () {
            this.unregion();
            // create
            var markup = [];
            markup.push('<var class="kb-region">');
            markup.push('<var style="position:absolute;"></var>'); // left
            markup.push('<var style="position:absolute;"></var>'); // top
            markup.push('<var style="position:absolute;"></var>'); // right
            markup.push('<var style="position:absolute;"></var>'); // bottom
            markup.push('</var>');
            this.regionCon = $(markup.join('')).appendTo(ctx.cacheCon || 'body');
            // css
            this.regionPosition();
            var cls = this.cssClass;
            this.regionCon.children().each(function () {
                ctx.zTop($(this)).addClass(cls);
            });
            // event
            this.registerResize();
        },

        regionPosition: function () {
            if (!this.regionCon) { return; }
            var css = this.genStyle();
            var els = this.regionCon.children();
            // get window size
            // note: ie7 use document.documentElement
            // $(window).height() get the work area size,
            // $(document).height() get the page size (contain the scrolled size)
            var winWidth = Math.max($(window).width(), document.body.scrollWidth, document.documentElement.scrollWidth); //$(document).width();
            var winHeight = Math.max($(window).height(), document.body.scrollHeight, document.documentElement.scrollHeight); //$(document).height();
            // left
            els.eq(0).css({
                top: 0,
                left: 0,
                width: css.left,
                height: css.top + css.height
            });
            // top
            els.eq(1).css({
                top: 0,
                left: css.left,
                width: winWidth - css.left,
                height: css.top
            });
            // right
            els.eq(2).css({
                top: css.top,
                left: css.left + css.width,
                width: winWidth - css.left - css.width,
                height: winHeight - css.top
            });
            // bottom
            els.eq(3).css({
                top: css.top + css.height,
                left: 0,
                width: css.left + css.width,
                height: winHeight - css.top - css.height
            });
        },

        border: function () {
            this.unborder();
            // create
            var markup = [];
            markup.push('<var>');
            markup.push('<var style="position:absolute;border-left:1px dashed transparent;width:0px;"></var>'); // left
            markup.push('<var style="position:absolute;border-top:1px dashed transparent;height:0px;"></var>'); // top
            markup.push('<var style="position:absolute;border-left:1px dashed transparent;width:0px;"></var>'); // right
            markup.push('<var style="position:absolute;border-top:1px dashed transparent;height:0px;"></var>'); // bottom
            markup.push('</var>');
            this.borderCon = $(markup.join('')).appendTo(ctx.cacheCon || 'body');
            // gen border color
            var temp = $('<var></var>').appendTo(this.borderCon);
            var borderColor = temp.addClass(this.cssClass).css('background-color');
            temp.remove();
            // css
            this.borderPosition();
            this.borderCon.children().each(function () {
                ctx.zTop($(this)).css({ borderColor: borderColor });
            });
            // event
            this.registerResize();
        },

        borderPosition: function () {
            if (!this.borderCon) { return; }
            var css = this.genStyle();
            var els = this.borderCon.children();
            // left
            els.eq(0).css({
                top: css.top - 1,
                left: css.left - 1,
                height: css.height
            });
            // top
            els.eq(1).css({
                top: css.top - 1,
                left: css.left - 1,
                width: css.width
            });
            // right
            els.eq(2).css({
                top: css.top - 1,
                left: css.left - 1 + css.width,
                height: css.height
            });
            // bottom
            els.eq(3).css({
                top: css.top - 1 + css.height,
                left: css.left - 1,
                width: css.width
            });
        },

        unmask: function () {
            this.maskEl && this.maskEl.remove();
            this.maskEl = null;
        },

        unborder: function () {
            this.borderCon && this.borderCon.remove();
            this.borderCon = null;
        },

        unregion: function () {
            this.regionCon && this.regionCon.remove();
            this.regionCon = null;
        },

        remove: function () {
            this.unmask();
            this.unborder();
            this.unregion();
            this.unregisterResize();
        }
    };

    // register
    ctx.widgetMask = widgetMask;

})(yardi, jQuery);
