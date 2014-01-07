/*
*   color
*   author: ronglin
*   create date: 2011.12.28
*/

(function ($, ctx) {

    var colorClass = function (config) {
        if (!this.className) { this.className = 'colorjs'; }
        colorClass.superclass.constructor.call(this, config);
    };

    yardi.extend(colorClass, ctx.editors.textClass, {

        button: null,

        initialize: function () {
            var self = this;
            colorClass.superclass.initialize.call(this);
            // color picker
            this.button = new yardi.colorPickerButton({
                renderTo: self.editCon, title: self.name,
                onPreview: function (value) { self.firePreview({ value: value }); },
                onCancel: function (value) { self.firePreviewEnd({ value: self.getValue() }); },
                onSelect: function (value) { self.setValue(value); self.syncValue(); }
            });
        },

        setValue: function (value) {
            value = colorClass.superclass.setValue.apply(this, arguments);
            this.button.setColor(value);
            return value;
        },

        remove: function () {
            this.button.remove();
            colorClass.superclass.remove.call(this);
        }

    });

    // register
    ctx.editors.colorClass = colorClass;

} (jQuery, visualstyle));
