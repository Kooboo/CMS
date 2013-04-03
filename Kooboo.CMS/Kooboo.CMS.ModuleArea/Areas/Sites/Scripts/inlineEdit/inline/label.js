/*
*
* inline label edit
* author: ronglin
* create date: 2011.02.10
*
*/

(function ($) {

    var labelClass = function (config) {
        labelClass.superclass.constructor.call(this, config);
    };

    yardi.extend(labelClass, yardi.htmlClass, {

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
    yardi.labelClass = labelClass;

})(jQuery);
