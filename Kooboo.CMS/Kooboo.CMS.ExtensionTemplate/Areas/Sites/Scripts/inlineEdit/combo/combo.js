/*
*   combo
*   author: ronglin
*   create date: 2010.05.28
*/

(function (ctx, $) {

    var combo = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    combo.prototype = {

        width: 160, height: 18, title: null, disabled: false,

        onClick: null, renderTo: 'body',

        el: null, input: null,

        constructor: combo,

        initialize: function () {
            // el
            var html = this.buildHtml();
            this.el = $(html).appendTo(this.renderTo);

            // input
            this.input = this.el.children('.kb-inner'); //this.input = $('input', this.el);
            this.input.width(this.width - 11); // 11 is the background image width
            this.input.height(this.height).css('line-height', this.height + 'px');

            // event
            var self = this;
            this.el.hover(function () {
                if (self.disabled === false && ctx.dialoging !== true)
                { $(this).addClass('kb-combo-hl'); }
            }, function () {
                $(this).removeClass('kb-combo-hl');
            }).click(function () {
                if (self.disabled === false && ctx.dialoging !== true)
                { self.onClick && self.onClick.call(self, self.el); }
            });

            // init
            if (this.title) { this.setTitle(this.title); }
            if (this.disabled) { this.isEnable(!this.disabled); }
        },

        buildHtml: function () {
            var html = [];
            html.push('<var class="kb-combo">');
            html.push('<var class="kb-inner"></var>'); //html.push('<input type="text" readonly="readonly" />');
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
                this.el.addClass('kb-combo-disabled');
            } else {
                this.el.removeClass('kb-combo-disabled');
            }
        },

        setTitle: function (title) {
            this.el.attr({ title: (this.title = title) });
        },

        val: function () {
            return this.input.text.apply(this.input, arguments);
        },

        css: function () {
            return this.input.css.apply(this.input, arguments);
        },

        remove: function () {
            this.el.remove();
        }
    };

    // register
    ctx.combo = combo;

})(yardi, jQuery);
