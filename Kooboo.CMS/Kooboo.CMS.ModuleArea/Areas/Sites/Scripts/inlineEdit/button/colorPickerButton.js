/*
*
* color picker button
* author: ronglin
* create date: 2010.06.01
*
*/

/*
* config parameters:
* renderTo, title, disabled, iconType
* onSelect, onPreview, onCancel, onPicker
*/

(function ($) {

    var colorPickerButton = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    colorPickerButton.prototype = {

        el: null,

        title: null,

        inited: false,

        disabled: false,

        renderTo: null,

        iconType: 'default', // default, backcolor, fontcolor

        // events config
        onSelect: function () { },
        onPreview: null,
        onCancel: null,
        onPicker: null,

        constructor: colorPickerButton,

        initialize: function () {
            var html = this.buildHtml();
            this.el = $(html).appendTo(this.renderTo);
            // events
            this.bindEvents();
            if (!this.onPreview)
                this.onPreview = this.onSelect;
            if (!this.onCancel)
                this.onCancel = this.onSelect;

            // disabled css
            this.isEnable(!this.disabled);

            // set status
            this.inited = true;
        },

        bindEvents: function () {
            var self = this;
            this.el.children().hover(function () {
                if (self.disabled == false && yardi.dialoging != true)
                    $(this).addClass('kb-hl');
            }, function () {
                //if (self.disabled == false)
                $(this).removeClass('kb-hl');
            }).click(function () {
                if (self.disabled == false && yardi.dialoging != true)
                    self.onClick(this);
            });
            if (this.title) {
                this.el.attr('alt', this.title);
                this.el.attr('title', this.title);
            }
        },

        buildHtml: function () {
            var html = [];
            html.push('<var class="kb-colorpickerbutton">');
            html.push('<var class="' + this.iconType + '"></var>');
            html.push('</var>');
            return html.join('');
        },

        // public
        remove: function () {
            this.el.remove();
        },

        // public
        isEnable: function (enable) {
            this.disabled = (enable === false);
            if (this.disabled) {
                this.el.children().addClass('kb-disabled');
            } else {
                this.el.children().removeClass('kb-disabled');
            }
        },

        onClick: function (el) {
            var self = this;
            yardi.colorPicker.show(el, this.getColor(),
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

        getColor: function () {
            return (yardi.currentStyle(this.el[0], 'background-color') || '').toUpperCase();
        },

        setColor: function (val) {
            this.el.css('background-color', val).attr('cval', val);
        }
    };

    // register
    yardi.colorPickerButton = colorPickerButton;

})(jQuery);
