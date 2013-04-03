/*
*
* fontSizePicker
* author: ronglin
* create date: 2010.05.31
*
*/

(function ($) {

    var fontSizePicker = function () {

        // font Size setting
        var sizeSetting = [{
            text: '8pt',
            value: 1,
            itemStyle: 'xx-small'
        }, {
            text: '10pt',
            value: 2,
            itemStyle: 'x-small'
        }, {
            text: '12pt',
            value: 3,
            itemStyle: 'small'
        }, {
            text: '14pt',
            value: 4,
            itemStyle: 'medium'
        }, {
            text: '18pt',
            value: 5,
            itemStyle: 'large'
        }, {
            text: '24pt',
            value: 6,
            itemStyle: 'x-large'
        }, {
            text: '36pt',
            value: 7,
            itemStyle: 'xx-large'
        }];

        // closure parameters
        var callbackFn;
        var picker = new yardi.listPicker({
            width: 100,
            dataList: sizeSetting,
            onSelect: function (item, ev) { callbackFn(item, ev); }
        });
        picker.onInitialized.add(function () {
            $('.kb-item', this.el).each(function () {
                var index = $(this).attr('itemIndex');
                var item = sizeSetting[parseInt(index)];
                $(this).css('font-size', item.itemStyle);
            });
        });

        // core func
        return {
            // public
            show: function (refEl, callback) {
                // set fn
                callbackFn = callback;
                // show
                picker.show(refEl);
            },
            // public
            hide: function () {
                picker.hide();
            },
            // public
            queryItem: function (value) {
                var parser = new yardi.sizeUnitParser(value);
                if (parser.hasType) {
                    for (var i = 0; i < sizeSetting.length; i++) {
                        var set = sizeSetting[i];
                        if (set.text == parser.toPt()) {
                            return set;
                        }
                    }
                } else if (value >= 1 && value <= sizeSetting.length) {
                    return sizeSetting[value - 1];
                } else {
                    return null;
                }
            }
        };
    } ();

    // register
    yardi.fontSizePicker = fontSizePicker;

})(jQuery);
