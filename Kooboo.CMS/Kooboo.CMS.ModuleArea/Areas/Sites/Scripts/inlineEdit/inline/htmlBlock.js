/*
*
* inline htmlBlock edit
* author: ronglin
* create date: 2011.04.02
*
*/

(function ($) {

    var htmlBlockClass = function (config) {
        htmlBlockClass.superclass.constructor.call(this, config);
    };

    yardi.extend(htmlBlockClass, yardi.htmlClass, {

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
    yardi.htmlBlockClass = htmlBlockClass;

})(jQuery);
