/*
*   htmlBlock edit
*   author: ronglin
*   create date: 2011.04.02
*/

(function (ctx, $) {

    var htmlBlockClass = function (config) {
        htmlBlockClass.superclass.constructor.call(this, config);
    };

    ctx.extend(htmlBlockClass, ctx.htmlClass, {

        createMenu: function () {
            htmlBlockClass.superclass.createMenu.call(this);
            this.menu.setTitle(this.params.blockName);
        },

        getParams: function () {
            return {
                dataType: this.startNode.attr('dataType'),
                blockName: this.startNode.attr('blockName')
            };
        }
    });

    // register
    ctx.htmlBlockClass = htmlBlockClass;

})(yardi, jQuery);
