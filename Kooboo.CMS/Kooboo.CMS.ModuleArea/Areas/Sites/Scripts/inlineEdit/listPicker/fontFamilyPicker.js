/*
*
* fontFamilyPicker
* author: ronglin
* create date: 2010.05.28
*
*/

(function ($) {

    var fontFamilyPicker = function () {

        // font family setting
        var familySetting = [{
            text: 'Arial',
            itemStyle: 'Arial,Helvetica,sans-serif'
        }, {
            text: 'Arial Black',
            itemStyle: 'Arial Black,Arial,Helvetica,sans-serif'
        }, {
            text: 'Arial Narrow',
            itemStyle: 'Arial Narrow,Arial MT Condensed Light,sans-serif'
        }, {
            text: 'Century Gothic',
            itemStyle: 'Century Gothic,ITC Avant Garde,Arial,Helvetica,sans-serif'
        }, {
            text: 'Comic Sans MS',
            itemStyle: 'Comic Sans MS,Verdana,Helvetica,sans-serif'
        }, {
            text: 'Consolas',
            itemStyle: 'Consolas,Lucida Console,Courier New,monospace'
        }, {
            text: 'Courier New',
            itemStyle: 'Courier New,Courier,monospace'
        }, {
            text: 'Franklin Gothic Medium',
            itemStyle: 'Franklin Gothic Medium,Arial,Helvetica,sans-serif'
        }, {
            text: 'French Script MT',
            itemStyle: 'French Script MT,Bradley Hand ITC,Monotype Corsiva,Times New Roman,Times,serif'
        }, {
            text: 'Garamond',
            itemStyle: 'Garamond,Times New Roman,Times,serif'
        }, {
            text: 'Georgia',
            itemStyle: 'Georgia,Times New Roman,Times,serif'
        }, {
            text: 'Haettenschweiler',
            itemStyle: 'Haettenschweiler,Impact,Arial,Helvetica,sans-serif'
        }, {
            text: 'Impact',
            itemStyle: 'Impact,Verdana,Arial,sans-serif'
        }, {
            text: 'Lucida Console',
            itemStyle: 'Lucida Console,Lucida,Arial,Helvetica,sans-serif'
        }, {
            text: 'Palatino Linotype',
            itemStyle: 'Palatino Linotype,Book Antiqua,Palatino,Times New Roman,Times,serif'
        }, {
            text: 'Papyrus',
            itemStyle: 'Papyrus,Comic Sans MS,Verdana,Helvetica,sans-serif'
        }, {
            text: 'Sylfaen',
            itemStyle: 'Sylfaen,Book Antiqua,Times New Roman,Times,serif'
        }, {
            text: 'Tahoma',
            itemStyle: 'Tahoma,Arial,sans-serif'
        }, {
            text: 'Terminal',
            itemStyle: 'Terminal,Monaco,monospace'
        }, {
            text: 'Times New Roman',
            itemStyle: 'Times New Roman,Times,serif'
        }, {
            text: 'Trebuchet MS',
            itemStyle: 'Trebuchet MS,Verdana,Helvetica,sans-serif'
        }, {
            text: 'Verdana',
            itemStyle: 'Verdana,Geneva,Arial,Helvetica,sans-serif'
        }];

        // closure parameters
        var callbackFn, picker;
        var resetting = function () {
            if (picker) {
                picker.hide();
                picker.remove();
                picker = null;
            }
        };
        var initialize = function () {
            resetting();
            picker = new yardi.listPicker({
                width: 190,
                dataList: familySetting,
                onSelect: function (item, ev) { callbackFn(item, ev); }
            });
            picker.onInitialized.add(function () {
                $('.kb-item', this.el).each(function () {
                    var index = $(this).attr('itemIndex');
                    var item = familySetting[parseInt(index)];
                    $(this).css('font-family', item.itemStyle);
                    $(this).css('font-size', '16px');
                });
            });
        };

        // core func
        return {
            // public
            show: function (refEl, callback) {
                if (!picker) { initialize(); }
                // set fn
                callbackFn = callback;
                // show
                picker.show(refEl);
            },
            // public
            hide: function () {
                picker && picker.hide();
            },
            // public
            queryItem: function (value) {
                for (var i = 0; i < familySetting.length; i++) {
                    var item = familySetting[i];
                    if (item.text.toLowerCase() == value.toLowerCase()) {
                        return item;
                    }
                }
            },
            // public extensible api
            setting: function (array) {
                if (yardi.isArray(array)) {
                    familySetting = array;
                    resetting();
                } else {
                    return familySetting;
                }
            },
            getSetting: function () {
                return this.setting();
            },
            setSetting: function (set) {
                return this.setting(set);
            }
        };
    } ();

    // register
    yardi.fontFamilyPicker = fontFamilyPicker;

})(jQuery);
