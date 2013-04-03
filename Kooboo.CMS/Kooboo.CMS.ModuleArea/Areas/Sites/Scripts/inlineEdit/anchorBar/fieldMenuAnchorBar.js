/*
*
* field menu anchor bar
* author: ronglin
* create date: 2011.01.28
*
*/


/*
* config parameters:
* title, alignTo, renderTo, onEdit
*/

(function ($) {

    // text resource
    var options = {
        editBtnTitle: 'edit'
    };

    // override text resource
    if (window.__inlineEditVars) { $.extend(options, __inlineEditVars.fieldMenuAnchorBar_js); }

    /*
    * field menu
    */
    var fieldMenuAnchorBar = function (config) {
        fieldMenuAnchorBar.superclass.constructor.call(this, config);
    };

    yardi.extend(fieldMenuAnchorBar, yardi.anchorBar, {

        isEditing: false,

        title: null, onEdit: null,

        components: null, isMenu: true,

        buildHtml: function () {
            var html = [];
            html.push('<var class="kb-fieldMenuAnchorBar" onselectstart="return false;">');
            html.push('<var class="kb-con"></var>');
            html.push('<var class="kb-name"></var>');
            html.push('</var>');
            return html.join('');
        },

        initialize: function () {
            fieldMenuAnchorBar.superclass.initialize.call(this);

            // btn events
            this.components = {};
            var self = this, split = '<var class="kb-sep">&nbsp;</var>';
            var btnCon = this.el.find('.kb-con');

            // edit
            this.components.btnEdit = new yardi.imageButton({
                title: options.editBtnTitle,
                renderTo: btnCon,
                imageUrl: 'anchorBar/images/menu_edit.gif',
                onClick: function (ev) { self.onEdit(); }
            });
            btnCon.append(split);

            // fix
            $.each(this.components, function () {
                this.el.removeClass('kb-imagebutton').addClass('kb-btn');
                this.highlightCss = 'kb-hover';
            });

            // set name
            var title = this.title || this.alignTo.attr('title');
            if (title) { this.el.find('.kb-name').html(title); }
        },

        getCheckElements: function () {
            return [this.alignTo];
        },

        correctPosition: function (ev) {
            // override, it's not need any implment.
        },

        getAlignCss: function (ev) {
            return {
                left: ev.pageX + 15,
                top: ev.pageY - 10
            };
        },

        beforeShow: function () {
            this.alignTo.addClass('kb-field-hl');
        },

        beforeHide: function () {
            this.alignTo.removeClass('kb-field-hl');
            this.hoverOut();
        },

        fieldMask: null,

        removeWidgetMask: function () {
            if (!this.fieldMask) { return; }
            this.fieldMask.remove();
            this.fieldMask = null;
        },

        hoverIn: function () {
            if (this.fixed === true) { return; }
            var self = this;
            clearTimeout(this._hoverId);
            this._hoverId = setTimeout(function () {
                // hide others
                yardi.anchorBar.hideAll(self);
                yardi.anchorBar.fixAll(true, self);
                //show field mask
                self.removeWidgetMask();
                self.fieldMask = new yardi.widgetMask({
                    cssClass: 'kb-field-mask',
                    containerNode: self.alignTo
                });
                self.fieldMask.mask();
            }, 150);
        },

        hoverOut: function () {
            if (this.fixed === true) { return; }
            clearTimeout(this._hoverId);
            yardi.anchorBar.fixAll(false, this);
            this.removeWidgetMask();
        }

    });

    // register
    yardi.fieldMenuAnchorBar = fieldMenuAnchorBar;

})(jQuery);
