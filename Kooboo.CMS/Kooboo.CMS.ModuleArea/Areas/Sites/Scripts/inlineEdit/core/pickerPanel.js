/*
*
* pickerPanel
* author: ronglin
* create date: 2010.06.01
*
*/


/*
* config parameters:
*   width, height, buildFn
*
* dispatch events:
*   onInitialized, onBaseCancel
*/

(function ($) {

    var pickerPanel = function (config) {
        $.extend(this, config);
        this.onBaseCancel = new yardi.dispatcher(this);
        this.onInitialized = new yardi.dispatcher(this);
    };

    pickerPanel.prototype = {

        el: null, inited: false, showed: false, width: 160, height: 190,

        referenceEl: null, buildFn: null, onInitialized: null, onBaseCancel: null,

        constructor: pickerPanel,

        initialize: function () {
            // build
            var html = this.buildFn();
            this.el = $(html).appendTo('body');
            if (this.height != null) this.el.css({ height: this.height });
            if (this.width != null) this.el.css({ width: this.width });
            // events
            this.bindEvents();
            // set status
            this.inited = true;
            // fire event
            this.onInitialized.dispatch(this);
        },

        // private, bind events
        bindEvents: function () {
            var self = this;
            $(document).mouseup(function (ev) {
                var parent = self.el[0], child = ev.target;
                if (self.showed == true) {
                    if (child == self.referenceEl || yardi.isAncestor(self.referenceEl, child)) {
                        return;
                    }
                    if (parent != child && !yardi.isAncestor(parent, child)) {
                        self.hide();
                        self.onBaseCancel.dispatch(self);
                    }
                }
            }).keydown(function (ev) {
                if (ev.keyCode == 27) {
                    self.hide();
                    self.onBaseCancel.dispatch(self);
                }
            });
        },

        // public
        getPos: function (refEl) {
            return yardi.flatPos(this.el, $(refEl));
        },

        // public
        remove: function () {
            this.el.unbind().remove();
        },

        // public
        show: function (refEl) {
            if (this.inited !== true) { this.initialize(); }
            if (this.showed == true) { return; }
            // show
            var pos = this.getPos(refEl);
            yardi.zTop(this.el);
            this.el.css({
                top: pos.top,
                left: pos.left
            });
            this.el.slideDown('fast');
            this.showed = true;
            this.referenceEl = $(refEl).get(0);
            pickerPanel.current = this;
            yardi.modeling = true;
        },

        // public
        hide: function () {
            if (this.inited !== true) { return; }
            if (this.showed == false) { return; }
            // hide
            this.el.hide();
            yardi.zOld(this.el);
            this.showed = false;
            this.referenceEl = null;
            pickerPanel.current = null;
            yardi.modeling = false;
        }
    };

    // use to handler the current opened panel.
    pickerPanel.current = null;

    // register
    yardi.pickerPanel = pickerPanel;

})(jQuery);
