/*
*
* inline html field edit
* author: ronglin
* create date: 2011.04.02
*
*/

(function ($) {

    var htmlFieldClass = function (config) {
        htmlFieldClass.superclass.constructor.call(this, config);
    };

    yardi.extend(htmlFieldClass, yardi.htmlClass, {

        createMenu: function () {
            htmlFieldClass.superclass.createMenu.call(this);
            this.menu.setTitle(this.params.fieldName);
        },

        getParams: function () {
            return yardi.fieldClass.prototype.getParams.call({ el: this.startNode });
        }
    });

    // register
    yardi.htmlFieldClass = htmlFieldClass;

})(jQuery);
