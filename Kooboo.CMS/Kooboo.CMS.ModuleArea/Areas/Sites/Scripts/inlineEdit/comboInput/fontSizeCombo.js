/*
*
* font size combo
* author: ronglin
* create date: 2010.05.28
*
*/

(function ($) {

    var fontSizeCombo = function (config) {
        config = config || {};
        fontSizeCombo.superclass.constructor.call(this, config);
    };

    yardi.extend(fontSizeCombo, yardi.comboInput, {
        // cache
        selectedItem: null,
        // public config
        onSelect: function (item) { },
        // public config
        onClick: function (input) {
            var self = this;
            yardi.fontSizePicker.show(input, function (item) {
                self.selectedItem = item;
                self.val(item.value);
                self.onSelect(item);
            });
        },
        // override
        val: function (num) {
            var item = yardi.fontSizePicker.queryItem(num);
            this.selectedItem = item;
            num = item ? item.text : num;
            fontSizeCombo.superclass.val.apply(this, arguments);
        }
    });

    // register
    yardi.fontSizeCombo = fontSizeCombo;

})(jQuery);
