/*
*   combo
*   author: ronglin
*   create date: 2011.12.28
*/

/*
* config parameters:
* items
*/

(function ($, ctx) {

    var comboClass = function (config) {
        if (!this.className) { this.className = 'combojs'; }
        comboClass.superclass.constructor.call(this, config);
    };

    yardi.extend(comboClass, ctx.editors.textClass, {

        items: null, picker: null,

        initialize: function () {
            var self = this;
            comboClass.superclass.initialize.call(this);
            this.input.focus(function () {
                if (!self.picker) {
                    var data = [], width, height;
                    $.each(self.items, function (key) { data.push({ text: key }); });
                    height = data.length * 20; if (height > 180) { height = 180; }
                    width = (self.name === 'display') ? 150 : 100;
                    self.picker = new yardi.listPicker({
                        width: width, height: height, dataList: data,
                        onPreview: function (item, ev) { self.firePreview({ value: item.text }); },
                        onSelect: function (item, ev) { self.setValue(item.text); self.syncValue(); }
                    });
                    self.picker.onBaseCancel.add(function () { self.firePreviewEnd({ value: self.getValue() }); });
                }
                setTimeout(function () { self.picker.show(self.input); }, 0);
            });
            // auto complete
            ctx.bindAutoComplete(this.input, this.items, function () {
                self.syncValue();
            });
        },

        remove: function () {
            if (this.picker) { this.picker.remove(); }
            comboClass.superclass.remove.call(this);
        }

    });

    // register
    ctx.editors.comboClass = comboClass;

} (jQuery, visualstyle));
