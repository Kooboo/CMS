/*
*   arrowPanel
*   author: ronglin
*   create date: 2010.06.17
*/

/*
* config parameters:
* width, title, renderTo, bodyBuilder, onClose
*/

(function (ctx, $) {

    /*
    * twinkle
    * function1, functon2, callback, times
    */
    var twinkle = function (func1, func2, cb, times) {
        times = times || 2;
        var count = 0, tw;
        (tw = function () {
            if (count < times) {
                count++;
                func1();
                setTimeout(function () {
                    func2();
                    setTimeout(function () {
                        tw();
                    }, 150);
                }, 150);
            } else {
                cb && cb();
            }
        })();
    };

    var arrowPanel = function (config) {
        $.extend(this, config);
    };

    arrowPanel.prototype = {

        el: null, renderTo: null,

        inited: false, showed: false,

        title: '', width: 160, showArrow: false,

        onClose: function (ev) { }, bodyBuilder: null,

        headTarget: null, closeTarget: null, titleTarget: null, arrowTarget: null,

        _docKeydown: null, _docMousedown: null, dragObj: null,

        constructor: arrowPanel,

        // build html
        buildHtml: function () {
            var html = [];
            html.push('<var class="kb-arrowPanel">');
            if (this.showArrow) { html.push('<var class="kb-arrow"></var>'); }
            html.push('<var class="kb-head">');
            html.push('<var class="kb-close" title="close">X</var>');
            html.push('<var class="kb-title"></var>');
            html.push('</var>');
            html.push('<var class="kb-body">');
            html.push(this.bodyBuilder ? this.bodyBuilder() : '');
            html.push('</var>');
            html.push('</var>');
            return html.join('');
        },

        // private, init
        initialize: function () {
            // build
            var html = this.buildHtml();
            this.el = $(html).appendTo(this.renderTo || ctx.cacheCon || 'body');
            if (this.width != null) this.el.width(this.width);
            // targets
            this.headTarget = $('.kb-head', this.el);
            this.closeTarget = $('.kb-close', this.el);
            this.titleTarget = $('.kb-title', this.el);
            if (this.title) { this.titleTarget.html(this.title); }
            // arrow left
            if (this.showArrow) {
                this.arrowTarget = $('.kb-arrow', this.el);
                this.arrowLeft(null);
            }
            // events
            this.bindEvents();
            // set status
            this.inited = true;
        },

        // private, bind events
        bindEvents: function () {
            // drag move
            var self = this, event, pos, selfSize, viewport;
            this.dragObj = new ctx.dragClass(this.titleTarget, {
                start: function (ev) {
                    event = ev;
                    pos = self.el.position();
                    viewport = ctx.getViewportSize(window);
                    selfSize = { width: self.el.outerWidth(), height: self.el.outerHeight() };
                },
                move: function (ev) {
                    pos.top -= event.pageY - ev.pageY;
                    pos.left -= event.pageX - ev.pageX;
                    // scroll may changed when mouse drag moving, chrome
                    var scroll = { left: $(document).scrollLeft(), top: $(document).scrollTop() };
                    self.el.css({
                        left: Math.min(Math.max(pos.left, scroll.left), viewport.width - selfSize.width + scroll.left),
                        top: Math.min(Math.max(pos.top, scroll.top), viewport.height - selfSize.height + scroll.top)
                    });
                    event = ev;
                }
            });
            // close
            $(document).keydown(this._docKeydown = function (ev) {
                (ev.keyCode == 27) && self.closeTarget.click();
            });
            ctx.eventPropagation.subscribe('keydown', this._docKeydown);
            this.closeTarget.click(function (ev) {
                self.onClose(ev);
                self.remove();
            });
            // twinkle
            setTimeout(function () {
                $(document).mousedown(self._docMousedown = function (ev) {
                    if (!ctx.isAncestor(self.el.get(0), ev.target)) {
                        self.twinkle();
                    }
                });
                ctx.eventPropagation.subscribe('mousedown', self._docMousedown);
            }, 256);
        },

        // public 
        remove: function () {
            ctx.eventPropagation.unsubscribe('keydown', this._docKeydown).unsubscribe('mousedown', this._docMousedown);
            $(document).unbind('keydown', this._docKeydown).unbind('mousedown', this._docMousedown);
            this.dragObj.destroy();
            this.hide();
            this.el.remove();
        },

        // public
        getPos: function (refEl) {
            return ctx.flatPos(this.el, $(refEl));
        },

        // arrow left
        arrowLeft: function (left) {
            if (this.showArrow) {
                var max = this.el.width() - this.arrowTarget.width();
                if (ctx.isNumber(left)) {
                    left = Math.max(left, 0);
                    left = Math.min(left, max);
                } else {
                    left = (max) / 2;
                }
                this.arrowTarget.css({ left: left });
            }
        },

        // twinkle
        twinkle: function () {
            var self = this;
            twinkle(function () {
                self.el.addClass('kb-twinkle');
            }, function () {
                self.el.removeClass('kb-twinkle');
            });
        },

        // public
        show: function (refEl) {
            if (this.inited == false) { this.initialize(); }
            var pos = this.getPos(refEl);
            this.showPos(pos);
        },

        // public
        showPos: function (pos) {
            if (this.inited == false) { this.initialize(); }
            if (this.showed == true) { return; }
            if (this.showArrow && ctx.isNumber(pos.top)) { pos.top += this.arrowTarget.height(); }
            ctx.zTop(this.el);
            this.el.css({
                top: pos.top,
                left: pos.left,
                display: 'none'
            });
            this.el.slideDown('fast');
            this.showed = true;
            ctx.modeling = true;
            ctx.dialoging = true;
        },

        // public
        hide: function () {
            if (this.inited == false || this.showed == false) { return; }
            // hide
            this.el.css('display', 'none');
            ctx.zOld(this.el);
            this.showed = false;
            ctx.modeling = false;
            ctx.dialoging = false;
        }
    };

    // register
    ctx.arrowPanel = arrowPanel;

})(yardi, jQuery);
