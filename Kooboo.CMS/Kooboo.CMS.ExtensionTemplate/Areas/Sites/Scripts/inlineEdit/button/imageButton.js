/*
*   imageButton
*   author: ronglin
*   create date: 2010.06.22
*/

/*
* config parameters:
* renderTo, [imageUrl || imageClass], title, disabled, disabledCss, highlightCss, onClick
*/

(function (ctx, $) {

    var imageButton = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    imageButton.prototype = {

        el: null, renderTo: null, title: null, imageUrl: null, imageClass: null,

        disabled: false, disabledCss: null, highlightCss: null, onClick: null,

        constructor: imageButton,

        initialize: function () {
            // dom
            if (!this.el) {
                var html = this.buildHtml();
                this.el = $(html).appendTo(this.renderTo);
            }
            // init
            this.bindEvents();
            if (this.title) { this.setTitle(this.title); }
            if (this.imageUrl) { this.setImage(this.imageUrl); }
            if (!this.disabled) { this.isEnable(!this.disabled); }
        },

        bindEvents: function () {
            var self = this;
            this.el.hover(function () {
                if (self.disabled == false && ctx.dialoging != true)
                    $(this).addClass(self.highlightCss || 'kb-imageButton-highlight');
            }, function () {
                //if (self.disabled == false)
                $(this).removeClass(self.highlightCss || 'kb-imageButton-highlight');
            }).click(function (ev) {
                if (self.disabled == false && ctx.dialoging != true)
                    self.onClick && self.onClick(ev);
            });
        },

        buildHtml: function () {
            if (this.imageClass) {
                return '<var class="kb-imageButton ' + this.imageClass + '"></var>';
            } else {
                // no 'src' attribute no request
                return '<img class="kb-imageButton" border="0" />';
            }
        },

        isImg: function () {
            return (this.el.prop('tagName') === 'IMG');
        },

        setImage: function (url) {
            this.imageUrl = url;
            var src = (ctx.rootPath || '') + url;
            if (this.isImg()) {
                this.el.attr({ src: src });
            } else {
                this.el.css('background-image', 'url("' + src + '")');
            }
        },

        setTitle: function (title) {
            var t = this.title = title;
            if (!this.isImg()) {
                this.el.attr({ title: t });
            } else {
                this.el.attr({ title: t, alt: t });
            }
        },

        isEnable: function (enable) {
            // get
            if (enable === undefined)
            { return !this.disabled; }
            // set
            this.disabled = (enable === false);
            if (this.disabled) {
                this.el.addClass(this.disabledCss || 'kb-imageButton-disabled');
            } else {
                this.el.removeClass(this.disabledCss || 'kb-imageButton-disabled');
            }
        },

        bgBtn: function () {
            if (!this.isImg()) { return this; }
            var self = this, asCore = function () {
                ctx.imgNaturalSize(this.el.get(0), function (size) {
                    var style = self.el.attr('style');
                    var newel = $('<var class="kb-imageButton"></var>');
                    newel.attr('style', style).css(size).insertAfter(self.el)
                    self.el.remove(); self.el = newel; self.initialize();
                });
            };
            var state = this.el.get(0).readyState;
            if (state && (/^(?:loaded|complete)$/i).test(state)) {
                asCore.call(self);
            } else {
                this.el.bind('load', function () { asCore.call(self); });
            }
            return this;
        },

        remove: function () {
            this.el.remove();
        }
    };

    // register
    ctx.imageButton = imageButton;

})(yardi, jQuery);
