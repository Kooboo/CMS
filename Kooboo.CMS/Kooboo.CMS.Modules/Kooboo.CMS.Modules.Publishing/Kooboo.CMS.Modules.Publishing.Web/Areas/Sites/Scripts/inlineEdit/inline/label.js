/*
*   label edit
*   author: ronglin
*   create date: 2011.02.10
*/

(function (ctx, $) {

    var labelClass = function (config) {
        labelClass.superclass.constructor.call(this, config);
    };

    ctx.extend(labelClass, ctx.htmlClass, {

        initialize: function () {
            labelClass.superclass.initialize.call(this);
            this.menu.setTitle('Label');
        },

        getParams: function () {
            return {
                key: this.startNode.attr('key'),
                category: this.startNode.attr('category'),
                dataType: this.startNode.attr('dataType')
            };
        }
    });

    // register
    ctx.labelClass = labelClass;

})(yardi, jQuery);
