/*
*   toolbarButton
*   author: ronglin
*   create date: 2010.06.13
*/

/*
* config parameters:
* renderTo, editor, width, height
*/

(function (ctx, $) {

    // text resource
    var options = {
        bold: 'bold',
        italic: 'italic',
        underline: 'underline',
        alignLeft: 'align left',
        alignCenter: 'align center',
        alignRight: 'align right',
        alignJustify: 'align justify',
        numberList: 'number list',
        bulletList: 'bullet list',
        indent: 'increase indent',
        outdent: 'decrease indent',
        insertImage: 'insert image',
        insertLink: 'insert link',
        editSource: 'edit source',
        redo: 'redo',
        undo: 'undo',
        unformat: 'remove format',
        horizontalRuler: 'insert horizontal ruler',
        pastePlainText: 'paste with plain text'
    };

    // override text resource
    if (window.__inlineEditVars) { $.extend(options, __inlineEditVars.toolbarButton_js); }

    /*
    * toolbar button base class
    */
    var toolbarButton = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    toolbarButton.prototype = {

        el: null, renderTo: null, editor: null,

        checked: false, disabled: true,

        constructor: toolbarButton,

        initialize: function () {
            // dom
            var html = this.buildHtml();
            this.el = $(html).appendTo(this.renderTo);

            // events
            var self = this;
            this.el.hover(function () {
                if (self.disabled == false && ctx.dialoging != true) {
                    $(this).addClass('kb-toolbarButton-hl');
                    //if (self.checked) {
                    //    $(this).removeClass('kb-toolbarButton-on');
                    //}
                }
            }, function () {
                if (self.disabled == false) {
                    $(this).removeClass('kb-toolbarButton-hl');
                    //if (self.checked) {
                    //    $(this).addClass('kb-toolbarButton-on');
                    //}
                }
            }).mousedown(function () {
                if (self.disabled == false && ctx.dialoging != true) {
                    if (self.onClick(self.editor) !== false) {
                        self.isChecked(!self.checked);
                    }
                }
            }).addClass('kb-toolbarButton');

            // editor callback to update toolbar button status
            this.editor.updateToolbar.add(function (sender, hasSel) {
                self.onUpdate(sender, hasSel);
            });

            // disabled css
            this.isEnable(!this.disabled);
        },

        isChecked: function (on) {
            this.checked = (on === true);
            if (this.checked) {
                this.el.addClass('kb-toolbarButton-on');
            } else {
                this.el.removeClass('kb-toolbarButton-on');
            }
        },

        isEnable: function (enable) {
            this.disabled = (enable == false);
            if (this.disabled) {
                this.el.addClass('kb-toolbarButton-disabled').removeClass('kb-toolbarButton-hl');
            } else {
                this.el.removeClass('kb-toolbarButton-disabled');
            }
        },

        buildHtml: function () {
            alert('buildHtml must be overrided by children class.');
        },

        onClick: function (editor) {
            alert('onClick must be overrided by children class.');
        },

        onUpdate: function (editor, hasSel) {
            alert('onUpdate must be overrided by children class.');
        },

        remove: function () {
            this.el.remove();
        }
    };

    var formatHtml = function (cls, title) { return '<var class="' + cls + '" title="' + title + '"></var>'; };

    /*
    * bold
    */
    var boldButton = function (config) {
        boldButton.superclass.constructor.call(this, config);
    };
    ctx.extend(boldButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnBold', options.bold);
        },
        onClick: function (editor) {
            editor.bold();
        },
        onUpdate: function (editor, hasSel) {
            this.isChecked(editor.bold(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.boldButton = boldButton;

    /*
    * italic
    */
    var italicButton = function (config) {
        italicButton.superclass.constructor.call(this, config);
    };
    ctx.extend(italicButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnItalic', options.italic);
        },
        onClick: function (editor) {
            editor.italic();
        },
        onUpdate: function (editor, hasSel) {
            this.isChecked(editor.italic(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.italicButton = italicButton;

    /*
    * underline
    */
    var underlineButton = function (config) {
        underlineButton.superclass.constructor.call(this, config);
    };
    ctx.extend(underlineButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnUnderline', options.underline);
        },
        onClick: function (editor) {
            editor.underline();
        },
        onUpdate: function (editor, hasSel) {
            this.isChecked(editor.underline(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.underlineButton = underlineButton;

    /*
    * align left
    */
    var alignleftButton = function (config) {
        alignleftButton.superclass.constructor.call(this, config);
    };
    ctx.extend(alignleftButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnAlignLeft', options.alignLeft);
        },
        onClick: function (editor) {
            editor.justifyLeft();
        },
        onUpdate: function (editor, hasSel) {
            this.isChecked(editor.justifyLeft(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.alignleftButton = alignleftButton;

    /*
    * align center
    */
    var aligncenterButton = function (config) {
        aligncenterButton.superclass.constructor.call(this, config);
    };
    ctx.extend(aligncenterButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnAlignCenter', options.alignCenter);
        },
        onClick: function (editor) {
            editor.justifyCenter();
        },
        onUpdate: function (editor, hasSel) {
            this.isChecked(editor.justifyCenter(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.aligncenterButton = aligncenterButton;

    /*
    * align right
    */
    var alignrightButton = function (config) {
        alignrightButton.superclass.constructor.call(this, config);
    };
    ctx.extend(alignrightButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnAlignRight', options.alignRight);
        },
        onClick: function (editor) {
            editor.justifyRight();
        },
        onUpdate: function (editor, hasSel) {
            this.isChecked(editor.justifyRight(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.alignrightButton = alignrightButton;

    /*
    * align justify
    */
    var alignjustifyButton = function (config) {
        alignjustifyButton.superclass.constructor.call(this, config);
    };
    ctx.extend(alignjustifyButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnAlignJustify', options.alignJustify);
        },
        onClick: function (editor) {
            editor.justifyFull();
        },
        onUpdate: function (editor, hasSel) {
            this.isChecked(editor.justifyFull(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.alignjustifyButton = alignjustifyButton;

    /*
    * number list
    */
    var numberlistButton = function (config) {
        numberlistButton.superclass.constructor.call(this, config);
    };
    ctx.extend(numberlistButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnNumberList', options.numberList);
        },
        onClick: function (editor) {
            editor.insertOrderedList();
        },
        onUpdate: function (editor, hasSel) {
            this.isChecked(editor.insertOrderedList(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.numberlistButton = numberlistButton;

    /*
    * bullet list
    */
    var bulletlistButton = function (config) {
        bulletlistButton.superclass.constructor.call(this, config);
    };
    ctx.extend(bulletlistButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnBulletList', options.bulletList);
        },
        onClick: function (editor) {
            editor.insertUnorderedList();
        },
        onUpdate: function (editor, hasSel) {
            this.isChecked(editor.insertUnorderedList(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.bulletlistButton = bulletlistButton;

    /*
    * indent
    */
    var indentButton = function (config) {
        indentButton.superclass.constructor.call(this, config);
    };
    ctx.extend(indentButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnIndent', options.indent);
        },
        onClick: function (editor) {
            editor.indent();
        },
        onUpdate: function (editor, hasSel) {
            this.isChecked(editor.indent(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.indentButton = indentButton;

    /*
    * outdent
    */
    var outdentButton = function (config) {
        outdentButton.superclass.constructor.call(this, config);
    };
    ctx.extend(outdentButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnOutdent', options.outdent);
        },
        onClick: function (editor) {
            editor.outdent();
        },
        onUpdate: function (editor, hasSel) {
            this.isChecked(editor.outdent(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.outdentButton = outdentButton;

    /*
    * insert image
    */
    var insertimageButton = function (config) {
        insertimageButton.superclass.constructor.call(this, config);
    };
    ctx.extend(insertimageButton, toolbarButton, {
        panel: null,
        //remove: function () {
        //    if (this.panel) {
        //        this.panel.remove();
        //        this.panel = null;
        //    }
        //    insertimageButton.superclass.remove.call(this);
        //},
        buildHtml: function () {
            return formatHtml('btnInsertImage', options.insertImage);
        },
        onClick: function (editor) {
            editor.Sniffer.insertNewImg();
            //this.panel = new ctx.imagePanel({
            //    onOk: function (val) {
            //        editor.insertImage(val);
            //        this.remove();
            //    },
            //    onCancel: function () {
            //        this.remove();
            //    }
            //});
            //this.panel.show(this.el);
            return false;
        },
        onUpdate: function (editor, hasSel) {
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.insertimageButton = insertimageButton;

    /*
    * insert link
    */
    var insertlinkButton = function (config) {
        insertlinkButton.superclass.constructor.call(this, config);
    };
    ctx.extend(insertlinkButton, toolbarButton, {
        disabled: true,
        panel: null,
        //remove: function () {
        //    if (this.panel) {
        //        this.panel.remove();
        //        this.panel = null;
        //    }
        //    insertlinkButton.superclass.remove.call(this);
        //},
        buildHtml: function () {
            return formatHtml('btnInsertLink', options.insertLink);
        },
        onClick: function (editor) {
            editor.Sniffer.insertNewLink();
            //var text = editor.Selection.getText();
            //this.panel = new ctx.linkPanel({
            //    linkText: text,
            //    onOk: function (html) {
            //        editor.execCommand('unlink');
            //        editor.replaceSelect('link', html);
            //        this.remove();
            //    },
            //    onCancel: function () {
            //        this.remove();
            //    }
            //});
            //this.panel.show(this.el);
            return false;
        },
        onUpdate: function (editor, hasSel) {
            this.isEnable(editor.sourceView == false && hasSel);
        }
    });
    // register
    ctx.insertlinkButton = insertlinkButton;

    /*
    * edit source
    */
    var editsourceButton = function (config) {
        editsourceButton.superclass.constructor.call(this, config);
    };
    ctx.extend(editsourceButton, toolbarButton, {
        sourcePanel: null,
        buildHtml: function () {
            return formatHtml('btnEditSource', options.editSource);
        },
        onClick: function (editor) {
            var self = this;
            this.sourcePanel = new ctx.textPanel({
                title: 'source view',
                textValue: editor.getHtml(),
                onOk: function (newHtml) {
                    editor.setHtml(newHtml);
                    self.closePanel();
                },
                onCancel: function (oldHtml) {
                    editor.setHtml(oldHtml, { processScript: false });
                    self.closePanel();
                },
                onPreview: function (html) {
                    editor.setHtml(html);
                }
            });
            this.sourcePanel.show(this.el);
            return false;
        },
        closePanel: function () {
            if (!this.sourcePanel) { return; }
            this.sourcePanel.remove();
            this.sourcePanel = null;
        },
        onUpdate: function (editor, hasSel) {
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.editsourceButton = editsourceButton;

    /*
    * redo
    */
    var redoButton = function (config) {
        redoButton.superclass.constructor.call(this, config);
    };
    ctx.extend(redoButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnRedo', options.redo);
        },
        onClick: function (editor) {
            editor.redo();
            return false;
        },
        onUpdate: function (editor, hasSel) {
            this.isEnable(editor.sourceView == false && editor.Redoundo.canRedo());
        }
    });
    // register
    ctx.redoButton = redoButton;

    /*
    * undo
    */
    var undoButton = function (config) {
        undoButton.superclass.constructor.call(this, config);
    };
    ctx.extend(undoButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnUndo', options.undo);
        },
        onClick: function (editor) {
            editor.undo();
            return false;
        },
        onUpdate: function (editor, hasSel) {
            this.isEnable(editor.sourceView == false && editor.Redoundo.canUndo());
        }
    });
    // register
    ctx.undoButton = undoButton;

    /*
    * unformat
    */
    var unformatButton = function (config) {
        unformatButton.superclass.constructor.call(this, config);
    };
    ctx.extend(unformatButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnUnformat', options.unformat);
        },
        onClick: function (editor) {
            editor.unformat();
            return false;
        },
        onUpdate: function (editor, hasSel) {
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.unformatButton = unformatButton;

    /*
    * horizontal ruler
    */
    var horizontalRulerButton = function (config) {
        horizontalRulerButton.superclass.constructor.call(this, config);
    };
    ctx.extend(horizontalRulerButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnHorizontalRuler', options.horizontalRuler);
        },
        onClick: function (editor) {
            editor.horizontalRuler();
            return false;
        },
        onUpdate: function (editor, hasSel) {
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    ctx.horizontalRulerButton = horizontalRulerButton;

    /*
    * paste plain text
    */
    var pastePlainTextButton = function (config) {
        pastePlainTextButton.superclass.constructor.call(this, config);
    };
    ctx.extend(pastePlainTextButton, toolbarButton, {
        buildHtml: function () {
            return formatHtml('btnPastePlainText', options.pastePlainText);
        },
        onClick: function (editor) {
            var self = this;
            if (this.checked) {
                self._unhandlePaste(editor.el);
            } else {
                self._handlePaste(editor.el, function () {
                    self._storeSelection();
                }, function (val) {
                    self._restoreSelection();
                    editor.pasteHtml(val);
                });
            }
            return true;
        },
        onUpdate: function (editor, hasSel) {
            this.isEnable(editor.sourceView == false);
        },

        _lastSelection: null,
        _storeSelection: function () {
            this._lastSelection = this.editor.Selection.getRange();
        },
        _restoreSelection: function () {
            this.editor.Selection.selectRange(this._lastSelection);
        },
        _keydownHandler: null,
        _handlePaste: function (el, before, callback) {
            $(el).bind('keydown', self._keydownHandler = function (ev) {
                if ((ev.ctrlKey && ev.keyCode === 86) ||  // ctrl+v
                    (ev.shiftKey && ev.keyCode === 45)) { // shift+insert
                    before(); var appendTo = ctx.cacheCon || 'body';
                    var temp = $('<var style="position:relative;width:0px;height:0px;"><textarea style="position:absolute;left:-99999px;"></textarea></var>').appendTo(appendTo);
                    var area = temp.children().focus();
                    setTimeout(function () {
                        callback(area.val());
                        temp.remove();
                    }, 0);
                }
            });
        },
        _unhandlePaste: function (el) {
            $(el).unbind('keydown', self._keydownHandler);
        }
    });
    // register
    ctx.pastePlainTextButton = pastePlainTextButton;

})(yardi, jQuery);
