/*
*   fontFamilyCombo
*   author: ronglin
*   create date: 2010.05.28
*/

(function (ctx, $) {

    var fontFamilyCombo = function (config) {
        config = config || {};
        fontFamilyCombo.superclass.constructor.call(this, config);
    };

    ctx.extend(fontFamilyCombo, ctx.combo, {
        // cache
        selectedItem: null,
        // public config
        onSelect: function (item) { },
        // public config
        onClick: function (input) {
            var self = this;
            ctx.fontFamilyPicker.show(input, function (item) {
                self.selectedItem = item;
                self.onSelect(item);
                self.input.val(item.text);
                self.input.css('fontFamily', item.text);
            });
        },
        // override
        val: function (name) {
            var item = ctx.fontFamilyPicker.queryItem(name);
            this.selectedItem = item;
            this.input.css('fontFamily', item ? item.text : name);
            name = item ? item.text : name;
            fontFamilyCombo.superclass.val.apply(this, arguments);
        }
    });

    // register
    ctx.fontFamilyCombo = fontFamilyCombo;

})(yardi, jQuery);
