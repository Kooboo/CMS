/*
*
* comboInput
* author: ronglin
* create date: 2010.05.28
*
*/

(function ($) {

    var comboInput = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    comboInput.prototype = {

        width: 160,

        height: 18,

        title: null,

        disabled: false,

        onClick: null,

        renderTo: 'body',

        el: null, input: null,

        constructor: comboInput,

        initialize: function () {
            // el
            this.el = $(this.getHtml()).appendTo(this.renderTo);
            if (this.title) { this.el.attr('title', this.title); }
            // input
            this.input = $('input', this.el);
            this.input.width(this.width - 12); // 12 is the background image width
            this.input.height(this.height);
            // event
            this.el.click(function () {
                if (self.disabled == false && yardi.dialoging != true) {
                    self.onClick && self.onClick.call(self, self.el);
                }
            });
            // highlight css
            var self = this;
            this.el.hover(function () {
                if (self.disabled == false && yardi.dialoging != true)
                    $(this).addClass('kb-comboinput-hl');
            }, function () {
                //if (self.disabled == false)
                $(this).removeClass('kb-comboinput-hl');
            });
            // disabled css
            this.isEnable(!this.disabled);
        },

        getHtml: function () {
            var html = [];
            html.push('<var class="kb-comboinput">');
            html.push('<input type="text" readonly="readonly" />');
            html.push('</var>');
            return html.join('');
        },

        remove: function () {
            this.el.remove();
        },

        isEnable: function (enable) {
            this.disabled = (enable === false);
            //this.input.attr('disabled', this.disabled);
            if (this.disabled) {
                this.el.addClass('kb-comboinput-disabled');
            } else {
                this.el.removeClass('kb-comboinput-disabled');
            }
        },

        val: function () {
            return this.input.val.apply(this.input, arguments);
        },

        css: function () {
            return this.input.css.apply(this.input, arguments);
        }
    };

    // register
    yardi.comboInput = comboInput;

})(jQuery);
