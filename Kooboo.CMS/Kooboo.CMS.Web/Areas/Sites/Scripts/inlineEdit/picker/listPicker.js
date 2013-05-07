/*
*   listPicker
*   author: ronglin
*   create date: 2010.05.28
*/

/*
* config parameters:
* dataList, onSelect, onPreview, width, height
*/

(function (ctx, $) {

    var listPicker = function (config) {
        listPicker.superclass.constructor.call(this, config);
        this.dataList = this.dataList || [];
    };

    ctx.extend(listPicker, ctx.pickerPanel, {

        dataList: null, onSelect: null, onPreview: null,

        buildHtml: function () {
            var html = [];
            html.push('<var class="kb-listPicker">');
            $.each(this.dataList, function (i, item) {
                html.push('<var class="kb-item" itemIndex="' + i + '">');
                html.push(item.text);
                html.push('</var>');
            });
            html.push('</var>');
            return html.join('');
        },

        bindEvents: function () {
            var self = this;
            $('.kb-item', this.el).hover(function (ev) {
                $(this).addClass('kb-hl');
                if (self.onPreview) {
                    var index = $(this).attr('itemIndex');
                    self.onPreview(self.dataList[parseInt(index)], ev)
                }
            }, function () {
                $(this).removeClass('kb-hl');
            }).click(function (ev) {
                var index = $(this).attr('itemIndex');
                self.onSelect(self.dataList[parseInt(index)], ev);
                self.hide();
            });
            listPicker.superclass.bindEvents.call(this);
        },

        hide: function () {
            this.el.get(0).scrollTop = 0;
            listPicker.superclass.hide.call(this);
        }
    });

    // register
    ctx.listPicker = listPicker;

})(yardi, jQuery);
