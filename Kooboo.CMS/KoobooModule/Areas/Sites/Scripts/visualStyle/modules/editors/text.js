/*
*   text
*   author: ronglin
*   create date: 2011.12.28
*/

(function ($, ctx) {

    var textClass = function (config) {
        if (!this.className) { this.className = 'textjs'; }
        textClass.superclass.constructor.call(this, config);
    };

    yardi.extend(textClass, ctx.editors.baseClass, {

        input: null,

        initialize: function () {
            var self = this;
            textClass.superclass.initialize.call(this);
            this.input = this.editCon.children('input.text');
            this.input.change(function () { self.syncValue(); });
        },

        buildHtml: function (html) {
            html.push('<input type="text" class="text" title="' + this.name + '" />');
        },

        getValue: function () {
            return this.input.val();
        },

        setValue: function (value) {
            value = textClass.superclass.setValue.apply(this, arguments);
            this.input.val(value);
            return value;
        }
    });

    // register
    ctx.editors.textClass = textClass;

} (jQuery, visualstyle));
