/*
*   unit
*   author: ronglin
*   create date: 2011.12.28
*/

/*
* config parameters:
* units
*/

(function ($, ctx) {

    // localization
    var txtRes = {
        unitTitle: 'unit'
    };
    // localize text resource
    if (window.__localization) { $.extend(txtRes, __localization.editors_js.unit_js); }

    var unitClass = function (config) {
        if (!this.className) { this.className = 'unitjs'; }
        unitClass.superclass.constructor.call(this, config);
    };

    yardi.extend(unitClass, ctx.editors.numberClass, {

        unitInput: null, picker: null, units: null,

        initialize: function () {
            var self = this;
            unitClass.superclass.initialize.call(this);
            this.unitInput = this.editCon.children('input.unit');
            this.unitInput.change(function () { self.syncValue(); });
            this.unitInput.focus(function () {
                if (!self.picker) {
                    var data = [];
                    $.each(self.units, function (key) { data.push({ text: key }); });
                    self.picker = new yardi.listPicker({
                        width: 50, height: data.length * 20, dataList: data,
                        onPreview: function (item, ev) { self.firePreview({ value: self.input.val() + item.text }); },
                        onSelect: function (item, ev) { self.unitInput.val(item.text); self.syncValue(); }
                    });
                    self.picker.onBaseCancel.add(function () { self.firePreviewEnd({ value: self.getValue() }); });
                }
                setTimeout(function () { self.picker.show(self.unitInput); }, 0);
            });
            // auto complete
            ctx.bindAutoComplete(this.unitInput, this.units, function () {
                self.syncValue();
            });
        },

        buildHtml: function (html) {
            unitClass.superclass.buildHtml.call(this, html);
            html.push('<input class="unit" type="text" title="' + txtRes.unitTitle + '" />');
        },

        getValue: function () {
            var t = this.input.val(),
                u = this.unitInput.val();
            return (t) ? (t + u) : '';
        },

        setValue: function (value) {
            value = unitClass.superclass.setValue.apply(this, arguments);
            var text = '', unit = '', chr, len = value.length;
            for (var i = 0; i < len; i++) {
                chr = value.charAt(i);
                if (/[0-9]/i.test(chr)) {
                    text += chr;
                } else {
                    unit = value.substr(i);
                    break;
                }
            }
            this.input.val(text);
            this.unitInput.val(unit);
            return value;
        },

        remove: function () {
            if (this.picker) { this.picker.remove(); }
            unitClass.superclass.remove.call(this);
        }

    });

    // register
    ctx.editors.unitClass = unitClass;

} (jQuery, visualstyle));
