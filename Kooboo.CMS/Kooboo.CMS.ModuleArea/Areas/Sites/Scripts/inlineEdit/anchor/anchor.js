/*
*   anchor
*   author: ronglin
*   create date: 2010.06.07
*/

(function (ctx, $) {

    var anchor = function (config) {
        $.extend(this, config);
        this.initialize();
    };


    anchor.overel = false;

    anchor.awakeDate = null;

    anchor.sleep = function (ms) {
        var d = new Date();
        ms = ms + d.getMilliseconds();
        d.setMilliseconds(ms);
        anchor.awakeDate = d;
    };

    anchor.expire = function () {
        var t = anchor.awakeDate;
        if (t && t > new Date()) {
            return false;
        } else {
            anchor.awakeDate = null;
            return true;
        }
    };


    var instances = [];

    anchor.getAll = function () {
        return instances;
    };

    anchor.hideAll = function (except) {
        $.each(anchor.getAll(), function () {
            if (except != this) { this.hide(); }
        });
    };

    anchor.fixAll = function (isfix, except) {
        $.each(anchor.getAll(), function () {
            if (except != this) { this.fix(isfix); }
        });
    };

    anchor.regIns = function (ins) {
        instances.push(ins);
        // when the tool bar is too long to show over the page range.
        // the tow parallel element 'kb-con' and 'kb-name' will display multiline.
        // so here detect if the layout is breaked and set a fixed width to resolve this problem.
        ins.onShow.add(function () {
            var breaked = false, offsetTop = undefined, width = 0;
            this.el.children().each(function () {
                width += $(this).outerWidth(true);
                if (offsetTop === undefined) {
                    offsetTop = $(this).position().top;
                    return;
                }
                if (!breaked && offsetTop !== $(this).position().top) {
                    breaked = true;
                    return;
                }
            });
            if (breaked) {
                this.el.width(width + (ctx.isIE ? 2 : 0));
            }
        });
    };


    anchor.prototype = {

        el: null, alignTo: null, renderTo: null,

        isMenu: false, fixed: false, showed: false, positionMonitor: null,

        _docMousemoveDelayFn: null, _correctPositionDelayFn: null, onShow: null,

        buildHtml: function () {
            alert('buildHtml must be override by child class.');
        },

        initialize: function () {
            // build
            this.el = $(this.buildHtml());
            this.alignTo = $(this.alignTo);
            //this.alignTo.addClass('kb-anchor-alignto');
            this.renderTo ? (this.el.appendTo(this.renderTo)) : (this.el.insertBefore(this.alignTo));

            // bar events
            var timeoutId, self = this;
            this._docMousemoveDelayFn = function (ev) {
                clearTimeout(timeoutId);
                timeoutId = setTimeout(function () {
                    if (self.fixed === true) { return; }
                    if (anchor.overel === true) { return; }
                    anchor.expire() && self.checkMouseIn(ev);
                }, 64);
            };
            $(document).mousemove(this._docMousemoveDelayFn);
            this.alignTo.mouseover(this._docMousemoveDelayFn);

            // monitor
            var oldSize = { w: 0, h: 0, ww: 0, wh: 0 };
            this.positionMonitor = new ctx.monitor({
                scope: this,
                interval: 64,
                tester: function () {
                    var size = { w: this.alignTo.width(), h: this.alignTo.height() };
                    if (oldSize.w != size.w || oldSize.h != size.h) {
                        oldSize.w = size.w; oldSize.h = size.h;
                        return true;
                    }
                    var sizew = { ww: $(window).width(), wh: $(window).height() };
                    if (oldSize.ww != sizew.ww || oldSize.wh != sizew.wh) {
                        oldSize.ww = sizew.ww; oldSize.wh = sizew.wh;
                        return true;
                    }
                },
                handler: function () {
                    this._correctPositionDelayFn(null);
                }
            });

            // hover
            this.el.hover(function () {
                anchor.overel = true;
                self.hoverIn();
            }, function () {
                anchor.overel = false;
                self.hoverOut();
            });

            // register
            this.onShow = new ctx.dispatcher(this);
            if (this.isMenu) { anchor.regIns(this); }
        },

        isMouseInEl: function (els, ev) {
            // is position in
            var isIn = false, offset = 0;
            for (var i = 0; i < els.length; i++) {
                var itemElem = $(els[i]);
                var pos = itemElem.offset(), w = itemElem.width(), h = itemElem.height();
                if (ev.pageX + offset > pos.left && ev.pageX - offset < pos.left + w &&
                    ev.pageY + offset > pos.top && ev.pageY - offset < pos.top + h) {
                    isIn = true;
                    break;
                }
            }
            // is element in
            var equal = (this.alignTo.get(0) === ev.target);
            var nested = ctx.isAncestor(this.alignTo.get(0), ev.target);
            // ret
            return isIn && (equal || nested);
        },

        getCheckElements: function () {
            return [this.alignTo, this.el];
        },

        fix: function (isFix) {
            this.fixed = (isFix === true);
        },

        checkMouseIn: function (ev) {
            if (!ev) { return; }
            if (ctx.modeling == true) { return; }
            if (this.isMouseInEl(this.getCheckElements(), ev)) {
                this.show(ev);
            } else {
                this.hide();
            }
        },

        correctPosition: function (ev) {
            this.el.css(this.getAlignCss(ev));
        },

        registerResize: function () {
            this.unregisterResize();
            // add listener to window resize event
            var timeoutId, self = this;
            this._correctPositionDelayFn = function (ev) {
                clearTimeout(timeoutId);
                timeoutId = setTimeout(function () {
                    (self.showed === true) && self.correctPosition(ev);
                }, 0);
            };
            $(window.top).resize(this._correctPositionDelayFn);
            this.positionMonitor.start();
        },

        unregisterResize: function () {
            this.positionMonitor.stop();
            if (this._correctPositionDelayFn)
                $(window.top).unbind('resize', this._correctPositionDelayFn);
        },

        remove: function () {
            $(document).unbind('mousemove', this._docMousemoveDelayFn);
            this.alignTo.unbind('mouseover', this._docMousemoveDelayFn);
            this.unregisterResize();
            this.el.remove();
        },

        getAlignCss: function (ev) {
            alert('getAlignCss must be override by child class.');
        },

        show: function (ev) {
            if (this.fixed != true && this.showed == false && this.beforeShow() !== false) {
                this.el.css(this.getAlignCss(ev));
                var self = this;
                ctx.zTop(this.el);
                this.el.fadeIn('fast', function () {
                    self.onShowed();
                    self.registerResize();
                });
                this.onShow.dispatch();
                this.showed = true;
            }
        },

        hide: function (force) {
            if (force === true || (this.fixed != true && this.showed == true && this.beforeHide() !== false)) {
                if (force === true) {
                    this.beforeHide();
                    this.el.hide();
                    this.onHided();
                } else {
                    var self = this;
                    this.el.fadeOut('fast', function () {
                        self.onHided();
                        self.unregisterResize();
                        ctx.zOld(self.el);
                    });
                }
                this.showed = false;
            }
        },

        onShowed: function () { },
        onHided: function () { },

        beforeShow: function () { },
        beforeHide: function () { },

        hoverIn: function () { },
        hoverOut: function () { }
    };

    // register
    ctx.anchor = anchor;

})(yardi, jQuery);
