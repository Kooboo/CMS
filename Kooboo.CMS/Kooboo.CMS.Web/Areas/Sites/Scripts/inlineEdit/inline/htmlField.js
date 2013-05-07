/*
*   html field edit
*   author: ronglin
*   create date: 2011.04.02
*/

(function (ctx, $) {

    var htmlFieldClass = function (config) {
        htmlFieldClass.superclass.constructor.call(this, config);
    };

    ctx.extend(htmlFieldClass, ctx.htmlClass, {

        createMenu: function () {
            htmlFieldClass.superclass.createMenu.call(this);
            this.menu.setTitle(this.params.fieldName);
        },

        getParams: function () {
            return ctx.fieldClass.prototype.getParams.call({ el: this.startNode });
        }
    });

    // register
    ctx.htmlFieldClass = htmlFieldClass;

})(yardi, jQuery);
