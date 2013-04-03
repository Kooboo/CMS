/*
*
* block menu anchor bar
* author: ronglin
* create date: 2011.01.28
*
*/


/*
* config parameters:
* title, alignTo, renderTo, onEdit, onCopy, onDelete, onPublish
*/

(function ($) {

    // text resource
    var options = {
        editBtnTitle: 'edit',
        copyBtnTitle: 'copy',
        deleteBtnTitle: 'delete',
        publishBtnTitle: 'publish',
        unpublishedTip: 'This item has not been published yet.<br/>Click to publish this item.'
    };

    // override text resource
    if (window.__inlineEditVars) { $.extend(options, __inlineEditVars.blockMenuAnchorBar_js); }

    /*
    * block menu
    */
    var blockMenuAnchorBar = function (config) {
        blockMenuAnchorBar.superclass.constructor.call(this, config);
    };

    yardi.extend(blockMenuAnchorBar, yardi.anchorBar, {

        title: null,

        onEdit: null, onCopy: null, onDelete: null, onPublish: null,

        components: null, isMenu: true, callout: null,

        buildHtml: function () {
            var html = [];
            html.push('<var class="kb-fieldMenuAnchorBar" onselectstart="return false;">');
            html.push('<var class="kb-con"></var>');
            html.push('<var class="kb-name"></var>');
            html.push('</var>');
            return html.join('');
        },

        initialize: function () {
            blockMenuAnchorBar.superclass.initialize.call(this);
            this.alignTo.removeClass('kb-block-hl');

            this.components = {};
            var self = this, split = '<var class="kb-sep">&nbsp;</var>';
            var btnCon = this.el.find('.kb-con');

            // edit
            this.components.btnEdit = new yardi.imageButton({
                title: options.editBtnTitle,
                renderTo: btnCon,
                imageUrl: 'anchorBar/images/menu_eidttext.gif',
                onClick: function (ev) { self.onEdit(); }
            });
            btnCon.append(split);

            // copy
            this.components.btnCopy = new yardi.imageButton({
                title: options.copyBtnTitle,
                renderTo: btnCon,
                imageUrl: 'anchorBar/images/menu_copy.gif',
                onClick: function (ev) { self.onCopy(); }
            });
            btnCon.append(split);

            // delete
            this.components.btnRemove = new yardi.imageButton({
                title: options.deleteBtnTitle,
                renderTo: btnCon,
                imageUrl: 'anchorBar/images/menu_delete.gif',
                onClick: function (ev) { self.onDelete(); }
            });
            btnCon.append(split);

            // publish
            this.components.btnPublish = new yardi.imageButton({
                title: options.publishBtnTitle,
                renderTo: btnCon,
                imageUrl: 'anchorBar/images/menu_publish.gif',
                onClick: function (ev) { self.onPublish(); }
            });
            btnCon.append(split);

            // fix
            $.each(this.components, function () {
                this.el.removeClass('kb-imagebutton').addClass('kb-btn');
                this.highlightCss = 'kb-hover';
            });

            // set name
            this.el.find('.kb-name').html(this.title);

            // callout
            this.callout = new $.yardiTip({
                hideManual: true,
                upwardsFixed: false,
                renderTo: yardi.cacheCon
            });
        },

        publishAvailable: false,

        enablePublish: function (enable) {
            this.publishAvailable = !!enable;
            var elem = this.components.btnPublish.el, sep = elem.next('.kb-sep');
            if (this.publishAvailable) {
                elem.show();
                sep.show();
            } else {
                elem.hide();
                sep.hide();
                this.callout.hide();
            }
        },

        disableBtns: function (disabled) {
            disabled = (disabled === true);
            for (var key in this.components) {
                this.components[key].isEnable(!disabled);
            }
        },

        remove: function () {
            $.each(this.components, function () { this.remove(); });
            $('.kb-block-mask', yardi.cacheCon).remove();
            this.callout.remove();
            blockMenuAnchorBar.superclass.remove.call(this);
        },

        message: function (msg, ref) {
            if (!msg) {
                this.callout.hide();
                return;
            }
            this.callout.setContent(msg);
            this.callout.show(ref);
        },

        getCheckElements: function () {
            return [this.alignTo];
        },

        correctPosition: function (ev) {
            if (this.fixed) {
                this.fix(true);
            } else {
                this.el.css(this.getAlignCss(ev));
            }
        },

        getAlignCss: function (ev) {
            var pos = this.alignTo.offset();
            return {
                left: pos.left,
                top: pos.top - this.el.outerHeight()
            };
        },

        showCallout: false,

        beforeShow: function () {
            this.showCallout = true;
            this.alignTo.addClass('kb-block-hl');
        },

        beforeHide: function () {
            this.showCallout = false;
            this.callout.hide();
            this.alignTo.removeClass('kb-block-hl');
            this.hoverOut();
        },

        onShowed: function () {
            if (!this.showCallout) { return; }
            if (this.publishAvailable) {
                this.callout.setContent(options.unpublishedTip);
                this.callout.show(this.components.btnPublish.el);
            }
        },

        blockMask: null,

        removeBlockMask: function () {
            if (!this.blockMask) { return; }
            this.blockMask.remove();
            this.blockMask = null;
        },

        hoverIn: function () {
            if (this.fixed === true) { return; }
            var self = this;
            clearTimeout(this._hoverId);
            this._hoverId = setTimeout(function () {
                // hide others
                yardi.anchorBar.hideAll(self);
                yardi.anchorBar.fixAll(true, self);
                //show block mask
                self.removeBlockMask();
                self.blockMask = new yardi.widgetMask({
                    cssClass: 'kb-block-mask',
                    containerNode: self.alignTo
                });
                self.blockMask.mask();
            }, 150);
        },

        hoverOut: function () {
            if (this.fixed === true) { return; }
            clearTimeout(this._hoverId);
            yardi.anchorBar.fixAll(false, this);
            this.removeBlockMask();
        }
    });

    // register
    yardi.blockMenuAnchorBar = blockMenuAnchorBar;

})(jQuery);
