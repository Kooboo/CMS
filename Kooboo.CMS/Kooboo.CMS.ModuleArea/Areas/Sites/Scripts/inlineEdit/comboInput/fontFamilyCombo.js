/*
*
* font family combo
* author: ronglin
* create date: 2010.05.28
*
*/

(function ($) {

    var fontFamilyCombo = function (config) {
        config = config || {};
        fontFamilyCombo.superclass.constructor.call(this, config);
    };

    yardi.extend(fontFamilyCombo, yardi.comboInput, {
        // cache
        selectedItem: null,
        // public config
        onSelect: function (item) { },
        // public config
        onClick: function (input) {
            var self = this;
            yardi.fontFamilyPicker.show(input, function (item) {
                self.selectedItem = item;
                self.onSelect(item);
                self.input.val(item.text);
                self.input.css('fontFamily', item.text);
            });
        },
        // override
        val: function (name) {
            var item = yardi.fontFamilyPicker.queryItem(name);
            this.selectedItem = item;
            this.input.css('fontFamily', item ? item.text : name);
            name = item ? item.text : name;
            fontFamilyCombo.superclass.val.apply(this, arguments);
        }
    });

    // register
    yardi.fontFamilyCombo = fontFamilyCombo;

})(jQuery);
