/*
*   fontSizeCombo
*   author: ronglin
*   create date: 2010.05.28
*/

(function (ctx, $) {

    var fontSizeCombo = function (config) {
        config = config || {};
        fontSizeCombo.superclass.constructor.call(this, config);
    };

    ctx.extend(fontSizeCombo, ctx.combo, {
        // cache
        selectedItem: null,
        // public config
        onSelect: function (item) { },
        // public config
        onClick: function (input) {
            var self = this;
            ctx.fontSizePicker.show(input, function (item) {
                self.selectedItem = item;
                self.val(item.value);
                self.onSelect(item);
            });
        },
        // override
        val: function (num) {
            var item = ctx.fontSizePicker.queryItem(num);
            this.selectedItem = item;
            num = item ? item.text : num;
            fontSizeCombo.superclass.val.apply(this, arguments);
        }
    });

    // register
    ctx.fontSizeCombo = fontSizeCombo;

})(yardi, jQuery);
