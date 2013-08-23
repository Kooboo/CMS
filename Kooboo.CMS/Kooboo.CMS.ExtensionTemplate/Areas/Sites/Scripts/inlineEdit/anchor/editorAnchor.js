/*
*   editorAnchor
*   author: ronglin
*   create date: 2011.01.28
*   modified by Raoh in 2013.05.03
*/

/*
* config parameters:
* alignTo, renderTo, editor
*/

(function (ctx, $) {

    // text resource
    var options = {
        saveBtnTitle: 'save',
        cancelBtnTitle: 'cancel',
        fontFamilyTitle: 'font family',
        fontSizeTitle: 'font size',
	headingTitle: 'heading',
        fontColorTitle: 'font color',
        backColorTitle: 'background color'
    };

    // override text resource
    if (window.__inlineEditVars) { $.extend(options, __inlineEditVars.editorAnchor_js); }

    /*
    * inline editor menu
    */
    var editorAnchor = function (config) {
        editorAnchor.superclass.constructor.call(this, config);
    };

    ctx.extend(editorAnchor, ctx.anchor, {

        editor: null, richText: true, onSave: null, onCancel: null,

        isMenu: false, components: null, callout: null,

        buildHtml: function () {
            var html = [];
            html.push('<var class="kb-editorAnchor">');
            html.push('<var class="kb-rowhr" tbs="0"></var>');
            // palin editor hacks
            if (this.richText) { html.push('<var class="kb-row" tbs="1"></var>'); }
            html.push('</var>');
            return html.join('');
        },

        initialize: function () {
            // init params
            var self = this;
            this.components = {};
            this.richText = this.editor.isRichText();
            var btnFactory = function (renderTo, tbs, fixFn) {
                for (var i = 0; i < tbs.length; i++) {
                    this.components[tbs[i].key] = new tbs[i].type({
                        renderTo: renderTo,
                        editor: this.editor
                    });
                }
            };

            // call base
            editorAnchor.superclass.initialize.call(this);

            // callout
            this.callout = new $.yardiTip({
                hideManual: true,
                upwardsFixed: false,
                renderTo: ctx.cacheCon
            });

            // rows
            var row1 = this.el.find('var[tbs="0"]'), row2 = this.el.find('var[tbs="1"]');

            // save
            this.components.btnSave = new ctx.imageButton({
                title: options.saveBtnTitle,
                renderTo: row1,
                imageClass: 'btnSave',
                onClick: function (ev) { self.onSave(ev); }
            });

            // cancel
            this.components.btnCancel = new ctx.imageButton({
                title: options.cancelBtnTitle,
                renderTo: row1,
                imageClass: 'btnCancel',
                onClick: function (ev) { self.onCancel(ev); }
            });

            // palin editor hacks
            if (!this.richText) {
                btnFactory.call(this, row1, [{
                    key: 'btnUndo',
                    type: ctx.undoButton
                }, {
                    key: 'btnRedo',
                    type: ctx.redoButton
                }]);
                row1.css('border-width', 0);
                row1.children().addClass('kb-toolbarButton');
                this.el.css({ width: 96, height: 22 });
                return;
            }

            // row1
            btnFactory.call(this, row1, [
            {
                key: 'btnUnformat',
                type: ctx.unformatButton
            }, {
                key: 'btnUndo',
                type: ctx.undoButton
            }, {
                key: 'btnRedo',
                type: ctx.redoButton
            }, {
                key: 'btnEditsource',
                type: ctx.editsourceButton
            }, {
                key: 'btnInsertImage',
                type: ctx.insertimageButton
            }, {
                key: 'btnInsertlink',
                type: ctx.insertlinkButton
            }, {
                key: 'btnPastePlainText',
                type: ctx.pastePlainTextButton
            }]);

            // row2
            btnFactory.call(this, row2, [
            {
                key: 'btnBold',
                type: ctx.boldButton
            }, {
                key: 'btnItalicBold',
                type: ctx.italicButton
            }, {
                key: 'btnUnderline',
                type: ctx.underlineButton
            }]);

            // font family
            this.components.btnFontFamily = new ctx.fontFamilyCombo({
                width: 59,
                disabled: true,
                title: options.fontFamilyTitle,
                renderTo: row2,
                onSelect: function (item) { self.editor.fontName(item.text); }
            });

            // font size
            this.components.btnFontSize = new ctx.fontSizeCombo({
                width: 36,
                disabled: true,
                title: options.fontSizeTitle,
                renderTo: row2,
                onSelect: function (item) { self.editor.fontSize(item.value); }
            });

            // heading
            this.components.btnHeading = new ctx.HeadingCombo({
                width:70,
                disabled: true,
                title: options.headingTitle,
                renderTo: row2,
                onSelect: function (item) { self.editor.heading(item.value); }
            });

            // font color
            this.components.btnFontColor = new ctx.colorPickerButton({
                disabled: true,
                title: options.fontColorTitle,
                iconType: 'fontcolor',
                renderTo: row2,
                onPreview: function (value) { self.editor.restoreLastSelection(); self.editor.foreColor(value, false); },
                onCancel: function (value) { self.editor.restoreLastSelection(); self.editor.foreColor(value, false); },
                onSelect: function (value) { self.editor.restoreLastSelection(); self.editor.foreColor(value); }
            });

            // back color
            this.components.btnBackColor = new ctx.colorPickerButton({
                disabled: true,
                title: options.backColorTitle,
                iconType: 'backcolor',
                renderTo: row2,
                onPreview: function (value) { self.editor.restoreLastSelection(); self.editor.backColor(value, false); },
                onCancel: function (value) { self.editor.restoreLastSelection(); self.editor.backColor(value, false); },
                onSelect: function (value) { self.editor.restoreLastSelection(); self.editor.backColor(value); }
            });

            // fix row1 css
            $.each(this.components, function () {
                this.el.css({
                    'margin-right': '2px',
                    'vertical-align': 'middle'
                });
            });

            // regisger updateToolbar method
            this.editor.updateToolbar.add(function (editor, hasSel) {
                // enable status
                var enable = editor.sourceView ? false : true;
                // fore color
                self.components.btnFontColor.isEnable(enable);
                var foreColor = editor.foreColor();
                if (foreColor) {
                    self.components.btnFontColor.setColor(foreColor);
                }
                // back color
                self.components.btnBackColor.isEnable(enable);
                var backColor = editor.backColor();
                if (backColor) {
                    self.components.btnBackColor.setColor(backColor);
                }
                // font size
                self.components.btnFontSize.isEnable(enable);
                var size = editor.fontSize();
                if (size) {
                    self.components.btnFontSize.val(size);
                }
		//heading
		self.components.btnHeading.isEnable(enable);
		var heading = editor.heading();
		if(heading) {
			self.components.btnHeading.val(heading);
		}

                // font name
                self.components.btnFontFamily.isEnable(enable);
                var name = editor.fontName();
                if (name) {
                    name = name.replace(/(^\s*)|(\s*$)/g, '');
                    name = name.replace(/(^('|")*)|(('|")*$)/g, '');
                    self.components.btnFontFamily.val(name);
                }
            });

            // register editorevents
            this.editor.editorEvents.add(function () {
                if (ctx.pickerPanel.current) {
                    ctx.pickerPanel.current.hide();
                }
            });
        },

        disableBtns: function (disabled) {
            disabled = (disabled === true);
            for (var key in this.components) {
                this.components[key].isEnable(!disabled);
            }
        },

        message: function (msg, ref) {
            if (!msg) {
                this.callout.hide();
                return;
            }
            this.callout.setContent(msg);
            this.callout.show(ref);
        },

        remove: function () {
            $.each(this.components, function (k, o) {
                o && o.remove();
            });
            this.alignTo.removeClass('kb-editor-hl');
            this.callout.remove();
            editorAnchor.superclass.remove.call(this);
        },

        isEnable: function (enable) {
            for (var key in this.components) {
                this.components[key].isEnable(enable);
            }
        },

        getAlignCss: function (ev) {
            var pos = this.alignTo.offset();
            var height = this.el.outerHeight();
            if (pos.top < height) {
                var alignHeight = this.alignTo.outerHeight();
                return {
                    left: pos.left,
                    top: pos.top + alignHeight + 1
                };
            } else {
                return {
                    left: pos.left,
                    top: pos.top - height - 1
                };
            }
        },

        beforeShow: function () {
            this.alignTo.addClass('kb-editor-hl');
        },

        beforeHide: function () {
            this.alignTo.removeClass('kb-editor-hl');
        }
    });

    // register
    ctx.editorAnchor = editorAnchor;

})(yardi, jQuery);
