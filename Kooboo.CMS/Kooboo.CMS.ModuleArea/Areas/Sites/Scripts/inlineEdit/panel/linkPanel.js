/*
*
* link panel
* author: ronglin
* create date: 2010.06.17
*
*/


/*
* config parameters:
* width, title, renderTo, onOk, onUnlink, onCancel
*/

(function ($) {

    // text resource
    var options = {
        headTitle: 'link panel',
        btnOk: 'ok',
        btnCancel: 'cancel',
        btnUnlink: 'unlink',
        lblText: 'Text:',
        lblUrl: 'Url:',
        lblTitle: 'Title:',
        lblLinkType: 'Link Type:',
        lblNewWin: 'Open in a new window',
        urlEmptyMsg: 'please input the url.'
    };

    // override text resource
    if (window.__inlineEditVars) { $.extend(options, __inlineEditVars.linkPanel_js); }

    /*
    * link panel class
    */
    var linkPanel = function (config) {
        config.width = 300;
        linkPanel.superclass.constructor.call(this, config);
    };

    yardi.extend(linkPanel, yardi.arrowPanel, {

        // public config
        title: options.headTitle,
        linkText: '',

        // public event
        onOk: function (txt, url, title, target, html) { },
        onUnlink: function () { },
        onCancel: function () { },

        textTarget: null, urlTarget: null, titleTarget: null, targetTarget: null, //linkTypes: null,

        bodyBuilder: function () {
            var html = [];
            html.push('<var class="kb-linkpanel">');
            html.push('<var class="kb-row">');
            html.push(options.lblText + '<input ftxt="1" class="kb-text" type="text" />');
            html.push('</var>');
            html.push('<var class="kb-row">');
            html.push(options.lblUrl + '<input furl="1" class="kb-url" type="text" />');
            html.push('<input fviw="1" class="kb-btn kb-view" type="button" value="view" />');
            html.push('</var>');
            html.push('<var class="kb-row">');
            html.push(options.lblTitle + '<input ftil="1" class="kb-tle" type="text" />');
            html.push('</var>');
            //html.push('<var class="kb-row">');
            //html.push(options.lblLinkType + '<input id="rdoNormal" name="linktype" type="radio" value="" class="kb-normal" /><label for="rdoNormal" class="kb-normal-text">normal</label><input id="rdoConfirm" name="linktype" type="radio" value="confirm" class="kb-confirm" /><label for="rdoConfirm" class="kb-confirm-text">confirm subscription</label><br/>');
            //html.push('<input id="rdoUnsubscribe" name="linktype" type="radio" value="unsubscribe" class="kb-unsubscribe" /><label for="rdoUnsubscribe" class="kb-unsubscribe-text">unsubscribe</label><input id="rdoOnlineversion" name="linktype" type="radio" value="onlineversion" class="kb-onlineversion" /><label for="rdoOnlineversion" class="kb-onlineversion-text">online version</label>');
            //html.push('</var>');
            html.push('<var class="kb-row kb-newwin">');
            html.push('<input fchk="1" class="kb-check" id="__check" type="checkbox" /><label for="__check"> ' + options.lblNewWin + '</label>');
            html.push('</var>');
            html.push('<var class="kb-bottom">');
            html.push('<input fok="1" class="kb-btn" type="button" value="' + options.btnOk + '" style="float:left;" />');
            html.push('<input fcnl="1" class="kb-btn" type="button" value="' + options.btnCancel + '" style="float:left;" />');
            html.push('<input fulk="1" class="kb-btn" type="button" value="' + options.btnUnlink + '" />');
            html.push('</var>');
            html.push('</var>');
            return html.join('');
        },

        buildLinkHtml: function () {
            var html = [];
            html.push('<a href="');
            html.push(this.urlTarget.val());
            html.push('" title="');
            html.push(this.titleTarget.val());
            html.push('" target="');
            html.push(this.targetTarget.attr('checked') ? '_blank' : '_self');
            html.push('">');
            html.push(this.textTarget.val());
            html.push('</a>');
            return html.join('');
        },

        bindEvents: function () {
            linkPanel.superclass.bindEvents.call(this);

            this.textTarget = this.el.find('input[ftxt="1"]');
            this.urlTarget = this.el.find('input[furl="1"]');
            this.titleTarget = this.el.find('input[ftil="1"]');
            this.targetTarget = this.el.find('input[fchk="1"]');
            //this.linkTypes = this.el.find('input[name="linktype"]');

            var self = this;
            this.textTarget.val(this.linkText);

            // btn ok
            this.el.find('input[fok="1"]').click(function () {
                self.onOk(self.textTarget.val(),
                self.urlTarget.val(),
                self.titleTarget.val(),
                self.targetTarget.attr('checked') ? '_blank' : '_self',
                self.buildLinkHtml());
            });
            // btn unlink
            this.el.find('input[fulk="1"]').click(function () {
                self.onUnlink();
            });
            // btn cancel
            this.el.find('input[fcnl="1"]').click(function () {
                self.onCancel();
            });
            // btn view
            this.el.find('input[fviw="1"]').click(function () {
                var val = self.urlTarget.val();
                if (val.length > 0) {
                    (val.indexOf('#') != 0) && window.open(val);
                } else {
                    alert(options.urlEmptyMsg);
                }
            });
        }
    });

    // register
    yardi.linkPanel = linkPanel;

})(jQuery);
