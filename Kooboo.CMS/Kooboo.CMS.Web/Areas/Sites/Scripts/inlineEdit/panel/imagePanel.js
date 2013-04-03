/*
*   imagePanel
*   author: ronglin
*   create date: 2010.06.17
*/

/*
* config parameters:
* width, title, renderTo, onOk, onCancel
*/

(function (ctx, $) {

    // text resource
    var options = {
        title: 'image options',
        imgLibTitle: 'Image library',
        attrURL: 'URL:',
        attrALT: 'ALT:',
        attrWidth: 'Width:',
        attrHeight: 'Height:',
        btnOk: 'ok',
        btnCancel: 'cancel',
        btnRemove: 'remove',
        btnLibrary: 'library',
        btnView: 'view',
        attrLinkHref: 'LINK:',
        emptyUrlMsg: 'please input the url.'
    };

    // override text resource
    if (window.__inlineEditVars) { $.extend(options, __inlineEditVars.imagePanel_js); }

    /*
    * image panel class
    */
    var imagePanel = function (config) {
        config.width = 300;
        imagePanel.superclass.constructor.call(this, config);
    };

    ctx.extend(imagePanel, ctx.arrowPanel, {

        // public config
        title: options.title,

        // public event
        onOk: function (val) { },
        onCancel: function () { },
        onDelete: function () { },

        urlTarget: null, descTarget: null, wTarget: null, hTarget: null, linkHrefTarget: null,

        onLibrary: function (sender, ev) {
            var self = this;
            var lib = new ctx.imgLib({
                title: options.imgLibTitle,
                width: 900, height: 500,
                url: __inlineEditVars.MediaLibrary + '&fileType=image',
                OnSelect: function (url, w, h, alt) {
                    self.setSrc(url); self.setAlt(alt);
                    if (!self.getWidth()) { self.setWidth(w); }
                    if (!self.getHeight()) { self.setHeight(h); }
                }
            });
            lib.show();
        },

        bodyBuilder: function () {
            var html = [];
            html.push('<var class="kb-imagePanel">');
            html.push('<var class="kb-row">');
            html.push(options.attrURL + '<input furl="1" class="kb-url" type="text" />');
            html.push('<input flib="1" class="kb-btn kb-lib" type="button" value="' + options.btnLibrary + '" />');
            html.push('</var>');
            html.push('<var class="kb-row">');
            html.push(options.attrALT + '<input fdesc="1" class="kb-alt" type="text" />');
            html.push('</var>');
            html.push('<var class="kb-row">');
            html.push(options.attrWidth + '<input type="text" fw="1" class="kb-size" />&nbsp;' + options.attrHeight + '<input fh="1" type="text" class="kb-size" />');
            html.push('</var>');
            html.push('<var class="kb-row kb-sep">');
            html.push('</var>');
            html.push('<var class="kb-row">');
            html.push(options.attrLinkHref + '<input class="kb-linkhref" type="text" flinkHref="1">');
            html.push('<input fviw="1" class="kb-btn kb-view" type="button" value="' + options.btnView + '" />');
            html.push('</var>');
            html.push('<var class="kb-bottom">');
            html.push('<input fok="1" class="kb-btn" type="button" value="' + options.btnOk + '" style="float:left;" />');
            html.push('<input fcnl="1" class="kb-btn" type="button" value="' + options.btnCancel + '" style="float:left;" />');
            html.push('<input fdel="1" class="kb-btn" type="button" value="' + options.btnRemove + '" />');
            html.push('</var>');
            html.push('</var>');
            return html.join('');
        },

        buildImageHtml: function () {
            var html = [];
            var linkHref = this.getHref();
            if (linkHref) { html.push('<a href="' + linkHref + '" target="_blank">'); }
            html.push('<img alt="' + this.getAlt() + '"');
            html.push(' src="' + this.getSrc() + '"');
            html.push(' width="' + this.getWidth() + '"');
            html.push(' height="' + this.getHeight() + '"');
            html.push(' />');
            if (linkHref) { html.push('</a>'); }
            return html.join('');
        },

        bindEvents: function () {
            imagePanel.superclass.bindEvents.call(this);
            var self = this;

            this.urlTarget = this.el.find('input[furl="1"]');
            this.descTarget = this.el.find('input[fdesc="1"]');
            this.wTarget = this.el.find('input[fw="1"]');
            this.hTarget = this.el.find('input[fh="1"]');
            this.linkHrefTarget = this.el.find('input[flinkHref="1"]');

            // btn ok
            this.el.find('input[fok="1"]').click(function () {
                self.onOk(self.getSrc(),
                          self.getAlt(),
                          self.getWidth(),
                          self.getHeight(),
                          self.getHref(),
                          self.buildImageHtml());
            });
            // btn view
            this.el.find('input[fviw="1"]').click(function () {
                var val = self.getHref();
                if (val.length > 0) {
                    (val.indexOf('#') != 0) && window.open(val);
                } else {
                    alert(options.emptyUrlMsg);
                }
            });
            // btn cancel
            this.el.find('input[fcnl="1"]').click(function () {
                self.onCancel();
            });
            // btn library
            this.el.find('input[flib="1"]').click(function (ev) {
                self.onLibrary(self, ev);
            });
            // btn delete
            this.el.find('input[fdel="1"]').click(function () {
                self.onDelete();
            });
        },

        focus: function () { this.urlTarget.focus(); },

        getSrc: function () { return this.urlTarget.val(); },
        setSrc: function (val) { this.urlTarget.val(val); },

        getAlt: function () { return this.descTarget.val(); },
        setAlt: function (val) { this.descTarget.val(val); },

        getHeight: function () { return this.hTarget.val(); },
        setHeight: function (val) { this.hTarget.val(val); },

        getWidth: function () { return this.wTarget.val(); },
        setWidth: function (val) { this.wTarget.val(val); },

        getHref: function () { return this.linkHrefTarget.val(); },
        setHref: function (val) { this.linkHrefTarget.val(val); }
    });

    // register
    ctx.imagePanel = imagePanel;

})(yardi, jQuery);
