/*
*
* image button
* author: ronglin
* create date: 2010.06.22
*
*/

/*
* config parameters:
* renderTo, imageUrl, onClick, highlightCss, disabledCss
*/

(function ($) {

    var imageButton = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    imageButton.prototype = {

        el: null,

        title: null,

        inited: false,

        disabled: false,

        // public config
        renderTo: null,

        // public config
        imageUrl: '',

        // public config
        highlightCss: null, disabledCss: null,

        constructor: imageButton,

        initialize: function () {
            var html = this.buildHtml();
            this.el = $(html).appendTo(this.renderTo);
            // events
            this.bindEvents();
            // disabled css
            this.isEnable(!this.disabled);
            // title
            if (this.title) { this.setTitle(this.title); }
            // set status
            this.inited = true;
        },

        bindEvents: function () {
            var self = this;
            this.el.hover(function () {
                if (self.disabled == false && yardi.dialoging != true)
                    $(this).addClass(self.highlightCss || 'kb-imagebutton-highlight');
            }, function () {
                //if (self.disabled == false)
                $(this).removeClass(self.highlightCss || 'kb-imagebutton-highlight');
            }).click(function (ev) {
                if (self.disabled == false && yardi.dialoging != true)
                    self.onClick(ev);
            });
        },

        buildHtml: function () {
            var src = (yardi.rootPath || '') + this.imageUrl;
            return '<img class="kb-imagebutton" border="0" src="' + src + '" />';
        },

        setSrc: function (src) {
            src = (yardi.rootPath || '') + src;
            this.el.attr('src', src);
        },

        setTitle: function (title) {
            var t = title || this.title;
            this.el.attr('alt', t);
            this.el.attr('title', t);
        },

        // public
        remove: function () {
            this.el.remove();
        },

        // public
        isEnable: function (enable) {
            this.disabled = (enable === false);
            if (this.disabled) {
                this.el.addClass(this.disabledCss || 'kb-imagebutton-disabled');
            } else {
                this.el.removeClass(this.disabledCss || 'kb-imagebutton-disabled');
            }
        },

        // public config
        onClick: function (ev) {
        }
    };

    // register
    yardi.imageButton = imageButton;

})(jQuery);
