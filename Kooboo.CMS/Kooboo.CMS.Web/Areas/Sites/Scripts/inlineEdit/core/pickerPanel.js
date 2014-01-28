/*
*   pickerPanel
*   author: ronglin
*   create date: 2010.06.01
*/

/*
* config parameters:
*   width, height, buildHtml
*
* dispatch events:
*   onInitialized, onBaseCancel
*/

(function (ctx, $) {

    var pickerPanel = function (config) {
        $.extend(this, config);
        this.onBaseCancel = new ctx.dispatcher(this);
        this.onInitialized = new ctx.dispatcher(this);
    };

    pickerPanel.prototype = {

        el: null, inited: false, showed: false, width: 160, height: 190,

        referenceEl: null, buildHtml: null, onInitialized: null, onBaseCancel: null,

        _docMouseup: null, _docKeydown: null,

        constructor: pickerPanel,

        initialize: function () {
            // build
        	var html = this.buildHtml();
            this.el = $(html).appendTo(ctx.cacheCon || 'body');
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
            $(document).mouseup(this._docMouseup = function (ev) {
                var parent = self.el[0], child = ev.target;
                if (self.showed == true) {
                    if (child == self.referenceEl || ctx.isAncestor(self.referenceEl, child)) {
                        return;
                    }
                    if (parent != child && !ctx.isAncestor(parent, child)) {
                        self.hide();
                        self.onBaseCancel.dispatch(self);
                    }
                }
            }).keydown(this._docKeydown = function (ev) {
                if (ev.keyCode == 27) {
                    self.hide();
                    self.onBaseCancel.dispatch(self);
                }
            });
            ctx.eventPropagation.subscribe('mouseup', this._docMouseup).subscribe('keydown', this._docKeydown);
        },

        // public
        getPos: function (refEl) {
            return ctx.flatPos(this.el, $(refEl));
        },

        // public
        remove: function () {
            this.el.unbind().remove();
            $(document).unbind('mouseup', this._docMouseup).unbind('keydown', this._docKeydown);
            ctx.eventPropagation.unsubscribe('mouseup', this._docMouseup).unsubscribe('keydown', this._docKeydown);
        },

        // public
        show: function (refEl) {
            if (this.inited !== true) { this.initialize(); }
            if (this.showed == true) { return; }
            // show
            var pos = this.getPos(refEl);
            ctx.zTop(this.el);
            this.el.css({
                top: pos.top,
                left: pos.left
            });
            this.el.slideDown('fast');
            this.showed = true;
            this.referenceEl = $(refEl).get(0);
            pickerPanel.current = this;
            ctx.modeling = true;
        },

        // public
        hide: function () {
            if (this.inited !== true) { return; }
            if (this.showed == false) { return; }
            // hide
            this.el.hide();
            ctx.zOld(this.el);
            this.showed = false;
            this.referenceEl = null;
            pickerPanel.current = null;
            ctx.modeling = false;
        },
    };

    // use to handler the current opened panel.
    pickerPanel.current = null;

    // register
    ctx.pickerPanel = pickerPanel;

})(yardi, jQuery);
