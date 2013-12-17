/*
*   blockAnchor
*   author: ronglin
*   create date: 2011.01.28
*/

/*
* config parameters:
* title, alignTo, renderTo, onEdit, onCopy, onDelete, onPublish
*/

(function (ctx, $) {

    // text resource
    var options = {
        editBtnTitle: 'edit',
        copyBtnTitle: 'copy',
        deleteBtnTitle: 'delete',
        publishBtnTitle: 'publish',
        unpublishedTip: 'This item has not been published yet.<br/>Click to publish this item.'
    };

    // override text resource
    if (window.__inlineEditVars) { $.extend(options, __inlineEditVars.blockAnchor_js); }

    /*
    * block menu
    */
    var blockAnchor = function (config) {
        blockAnchor.superclass.constructor.call(this, config);
    };

    ctx.extend(blockAnchor, ctx.anchor, {

        title: null, onEdit: null, onCopy: null, onDelete: null, onPublish: null,

        isMenu: true, components: null, callout: null,

        buildHtml: function () {
            var html = [];
            html.push('<var class="kb-blockAnchor kb-fieldAnchor" onselectstart="return false;">');
            html.push('<var class="kb-con"></var>');
            html.push('<var class="kb-name"></var>');
            html.push('</var>');
            return html.join('');
        },

        initialize: function () {
            blockAnchor.superclass.initialize.call(this);
            this.alignTo.removeClass('kb-block-hl');

            this.components = {};
            var self = this, split = '<var class="kb-sep">&nbsp;</var>';
            var btnCon = this.el.find('.kb-con');

            // edit
            this.components.btnEdit = new ctx.imageButton({
                title: options.editBtnTitle,
                renderTo: btnCon,
                imageClass: 'btnEdit',
                onClick: function (ev) { self.onEdit(); }
            });
            btnCon.append(split);

            // copy
            this.components.btnCopy = new ctx.imageButton({
                title: options.copyBtnTitle,
                renderTo: btnCon,
                imageClass: 'btnCopy',
                onClick: function (ev) { self.onCopy(); }
            });
            btnCon.append(split);

            // delete
            this.components.btnRemove = new ctx.imageButton({
                title: options.deleteBtnTitle,
                renderTo: btnCon,
                imageClass: 'btnDelete',
                onClick: function (ev) { self.onDelete(); }
            });
            btnCon.append(split);

            // publish
            this.components.btnPublish = new ctx.imageButton({
                title: options.publishBtnTitle,
                renderTo: btnCon,
                imageClass: 'btnPublish',
                onClick: function (ev) { self.onPublish(); }
            });
            btnCon.append(split);

            // fix
            $.each(this.components, function () {
                this.el.removeClass('kb-imageButton').addClass('kb-btn');
                this.highlightCss = 'kb-hover';
            });

            // set name
            this.el.find('.kb-name').html(this.title);

            // callout
            this.callout = new $.yardiTip({
                renderTo: ctx.cacheCon,
                upwardsFixed: false,
                hideManual: true
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
            $('.kb-block-mask', ctx.cacheCon).remove();
            this.callout.remove();
            blockAnchor.superclass.remove.call(this);
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
                ctx.anchor.hideAll(self);
                ctx.anchor.fixAll(true, self);
                //show block mask
                self.removeBlockMask();
                self.blockMask = new ctx.widgetMask({
                    cssClass: 'kb-block-mask',
                    containerNode: self.alignTo
                });
                self.blockMask.mask();
            }, 128);
        },

        hoverOut: function () {
            if (this.fixed === true) { return; }
            clearTimeout(this._hoverId);
            ctx.anchor.fixAll(false, this);
            this.removeBlockMask();
        }
    });

    // register
    ctx.blockAnchor = blockAnchor;

})(yardi, jQuery);
