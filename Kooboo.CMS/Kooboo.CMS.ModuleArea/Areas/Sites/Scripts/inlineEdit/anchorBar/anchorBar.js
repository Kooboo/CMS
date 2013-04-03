/*
*
* anchor bar
* author: ronglin
* create date: 2010.06.07
*
*/

(function ($) {

    var anchorBar = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    anchorBar.overel = false;

    anchorBar.awakeDate = null;

    anchorBar.sleep = function (ms) {
        var d = new Date();
        ms = ms + d.getMilliseconds();
        d.setMilliseconds(ms);
        anchorBar.awakeDate = d;
    };

    anchorBar.expire = function () {
        var t = anchorBar.awakeDate;
        if (t && t > new Date()) {
            return false;
        } else {
            anchorBar.awakeDate = null;
            return true;
        }
    };

    anchorBar.instances = [];

    anchorBar.hideAll = function (except) {
        $.each(anchorBar.instances, function () {
            if (except != this) { this.hide(); }
        });
    };

    anchorBar.fixAll = function (isfix, except) {
        $.each(anchorBar.instances, function () {
            if (except != this) { this.fix(isfix); }
        });
    };

    anchorBar.prototype = {

        el: null,

        alignTo: null,

        renderTo: null,

        fixed: false,

        showed: false,

        _docMousemoveDelayFn: null, _correctPositionDelayFn: null, positionMonitor: null,

        buildHtml: function () {
            alert('buildHtml must be override by child class.');
        },

        initialize: function () {
            if (this.isMenu === true) { anchorBar.instances.push(this); }
            this.alignTo = $(this.alignTo);
            //this.alignTo.addClass('kb-anchorbar-alignto');

            // build
            this.el = $(this.buildHtml());
            this.renderTo ? (this.el.appendTo(this.renderTo)) : (this.el.insertBefore(this.alignTo));

            // bar events
            var timeoutId, self = this;
            this._docMousemoveDelayFn = function (ev) {
                clearTimeout(timeoutId);
                timeoutId = setTimeout(function () {
                    if (self.fixed === true) { return; }
                    if (anchorBar.overel === true) { return; }
                    anchorBar.expire() && self.checkMouseIn(ev);
                }, 50);
            };
            $(document).mousemove(this._docMousemoveDelayFn);
            this.alignTo.mouseover(this._docMousemoveDelayFn);

            // monitor
            var oldSize = { w: 0, h: 0, ww: 0, wh: 0 };
            this.positionMonitor = new yardi.monitor({
                scope: this,
                interval: 50,
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
                anchorBar.overel = true;
                self.hoverIn();
            }, function () {
                anchorBar.overel = false;
                self.hoverOut();
            });
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
            var nested = yardi.isAncestor(this.alignTo.get(0), ev.target);
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
            if (yardi.modeling == true) { return; }
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
                yardi.zTop(this.el);
                this.el.fadeIn('fast', function () {
                    self.onShowed();
                    self.registerResize();
                });
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
                        yardi.zOld(self.el);
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
    yardi.anchorBar = anchorBar;

})(jQuery);
