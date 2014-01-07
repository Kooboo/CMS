/*
*   textPanel
*   author: ronglin
*   create date: 2010.06.22
*/

/*
* config parameters:
* width, title, renderTo, onOk, onCancel, onPreview
*/

(function (ctx, $) {

    // text resource
    var options = {
        headTitle: 'text panel',
        btnPreview: 'preview',
        btnReset: 'reset',
        btnOk: 'ok',
        btnCancel: 'cancel'
    };

    // override text resource
    if (window.__inlineEditVars) { $.extend(options, __inlineEditVars.textPanel_js); }

    /*
    * text panel class
    */
    var textPanel = function (config) {
        config.width = 450;
        textPanel.superclass.constructor.call(this, config);
    };

    ctx.extend(textPanel, ctx.arrowPanel, {

        // public config
        title: options.headTitle,
        textValue: '',

        // public event
        onOk: function (newCssText) { },
        onCancel: function (oldCssText) { },
        onPreview: function (cssText) { },
        onClose: function (ev) { this.el.find('input[fcel="1"]').click(); },

        bodyBuilder: function () {
            var html = [];
            html.push('<var class="kb-textPanel">');
            html.push('<var class="kb-row">');
            html.push('<textarea farea="1" class="kb-area"></textarea>');
            html.push('</var>');
            html.push('<var class="kb-row">');
            html.push('<input fprv="1" class="kb-btn" type="button" value="' + options.btnPreview + '" />');
            html.push('<input frst="1" class="kb-btn" type="button" value="' + options.btnReset + '" />');
            html.push('<input fok="1" class="kb-btn" type="button" value="' + options.btnOk + '" />');
            html.push('<input fcel="1" class="kb-btn" type="button" value="' + options.btnCancel + '" />');
            html.push('</var>');
            html.push('</var>');
            return html.join('');
        },

        bindEvents: function () {
            textPanel.superclass.bindEvents.call(this);

            var self = this;
            var oText = this.el.find('textarea[farea="1"]').val(this.textValue);

            this.el.find('input[fok="1"]').click(function () {
                self.onOk(oText.val());
                self.remove();
            });
            this.el.find('input[fcel="1"]').click(function () {
                self.onCancel(self.textValue);
                self.remove();
            });
            this.el.find('input[fprv="1"]').click(function () {
                self.onPreview(oText.val());
            });
            this.el.find('input[frst="1"]').click(function () {
                oText.val(self.textValue);
                self.onPreview(self.textValue);
            });
        }
    });

    // register
    ctx.textPanel = textPanel;

})(yardi, jQuery);
