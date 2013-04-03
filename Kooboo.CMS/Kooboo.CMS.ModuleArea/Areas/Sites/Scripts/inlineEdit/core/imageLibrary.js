/*
*
* img library
* author: ronglin
* create date: 2010.10.28
*
*/

(function ($) {

    var imgLib = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    imgLib.prototype = {

        url: null, width: 700, height: 400, title: 'Image Library',

        dialogTarget: null, outerDialog: false, styleEditing: false,

        initialize: function () { },

        // callback envents
        OnSelect: function (url, w, h, alt) { },

        breakLeak: function () {
            if (!this.dialogTarget) { return; }
            var iframe = this.dialogTarget.children('iframe').get(0);
            if (!iframe) { return; }
            try {
                iframe.outerApi = undefined;
                delete iframe['outerApi'];
            } catch (ex) { }
            iframe.src = 'javascript:false;';
            iframe.parentNode.removeChild(iframe);
        },

        show: function () {
            if (this.outerDialog) {
                window.designer && window.designer.showDialog({
                    url: this.url,
                    title: this.title,
                    zIndex: yardi.zindexCenter.getMax(top) + 100
                }, {
                    OnSelect: this.OnSelect
                });
            } else {
                this.close();
                var self = this, jq = $;
                if (this.styleEditing) { jq = top._jQueryOfStyleEditing; }
                this.dialogTarget = jq('<div></div>').appendTo('body').dialog({
                    position: 'center',
                    modal: true,
                    width: this.width,
                    height: this.height,
                    title: this.title
                });
                this.dialogTarget.bind('dialogclose', function () { self.close(); });
                this.dialogTarget.append('<iframe id="__imagelibrary" frameBorder="0" style="width:100%;height:100%;" src="' + this.url + '"></iframe>');
                this.dialogTarget.children('iframe').get(0).outerApi = this;
                // zindex
                var dialogCon = this.dialogTarget.parent('.ui-dialog').first();
                var maskCon = dialogCon.next('.ui-widget-overlay').first();
                yardi.zTop(maskCon); yardi.zTop(dialogCon);
            }
        },

        close: function () {
            if (this.dialogTarget) {
                this.breakLeak();
                this.dialogTarget.remove();
                this.dialogTarget = null;
            }
        }
    };

    // register
    yardi.imgLib = imgLib;

})(jQuery);
