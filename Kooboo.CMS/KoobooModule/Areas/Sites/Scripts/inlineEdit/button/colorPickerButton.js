/*
*   colorPickerButton
*   author: ronglin
*   create date: 2010.06.01
*/

/*
* config parameters:
* renderTo, title, disabled, iconType
* onSelect, onPreview, onCancel, onPicker
*/

(function (ctx, $) {

    var colorPickerButton = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    colorPickerButton.prototype = {

        el: null, renderTo: null, title: null, disabled: false, iconType: 'default', // iconType: 'default', 'backcolor', 'fontcolor'

        onSelect: function () { }, onPreview: null, onCancel: null, onPicker: null,

        constructor: colorPickerButton,

        initialize: function () {
            // dom
            var html = this.buildHtml();
            this.el = $(html).appendTo(this.renderTo);

            // event
            this.bindEvents();
            if (!this.onCancel) { this.onCancel = this.onSelect; }
            if (!this.onPreview) { this.onPreview = this.onSelect; }

            // init
            if (this.title) { this.setTitle(this.title); }
            if (this.disabled) { this.isEnable(!this.disabled); }
        },

        bindEvents: function () {
            var self = this;
            this.el.children().hover(function () {
                if (self.disabled === false && ctx.dialoging !== true)
                { $(this).addClass('kb-hl'); }
            }, function () {
                $(this).removeClass('kb-hl');
            }).click(function () {
                if (self.disabled === false && ctx.dialoging !== true)
                { self.onClick(this); }
            });
        },

        buildHtml: function () {
            var html = [];
            html.push('<var class="kb-colorPickerButton">');
            html.push('<var class="' + this.iconType + '"></var>');
            html.push('</var>');
            return html.join('');
        },

        isEnable: function (enable) {
            // get
            if (enable === undefined)
            { return !this.disabled; }
            // set
            this.disabled = (enable === false);
            if (this.disabled) {
                this.el.children().addClass('kb-disabled');
            } else {
                this.el.children().removeClass('kb-disabled');
            }
        },

        onClick: function (el) {
            var self = this;
            ctx.colorPicker.show(el, this.getColor(),
            function (value, ev) {
                self.onSelect(value, ev);
                self.setColor(value);
            },
            function (value, ev) {
                self.onPreview(value, ev);
            },
            function (value, ev) {
                self.onCancel(value, ev);
            });
            if (this.onPicker) {
                this.onPicker();
            }
        },

        setTitle: function (title) {
            this.el.attr({ title: (this.title = title) });
        },

        getColor: function () {
            return (ctx.currentStyle(this.el[0], 'background-color') || '').toUpperCase();
        },

        setColor: function (val) {
            this.el.css('background-color', val).attr('cval', val);
        },

        remove: function () {
            this.el.remove();
        }
    };

    // register
    ctx.colorPickerButton = colorPickerButton;

})(yardi, jQuery);
