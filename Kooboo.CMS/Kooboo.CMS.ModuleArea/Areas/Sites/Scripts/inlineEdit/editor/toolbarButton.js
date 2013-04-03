/*
*
* toolbar button
* author: ronglin
* create date: 2010.06.13
*
*/


/*
* config parameters:
* renderTo, editor, width, height
*/

(function ($) {

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
        horizontalRuler: 'insert horizontal ruler'
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

        el: null,

        editor: null,

        renderTo: 'body',

        width: null,

        height: null,

        _isOn: false,

        disabled: true,

        imgDir: 'editor/images',

        // protected
        initialize: function () {
            // img directory
            this.imgDir = (yardi.rootPath || '') + this.imgDir;
            var formatParams = [this.imgDir];
            // init element
            var html = this.buildHtml();
            html = html.replace(/\{(\d+)\}/g, function (m, i) {
                return formatParams[i];
            });
            this.el = $(html).appendTo(this.renderTo);
            if (this.height != null) this.el.css({ height: this.height });
            if (this.width != null) this.el.css({ width: this.width });
            // events
            var self = this;
            this.el.hover(function () {
                if (self.disabled == false && yardi.dialoging != true) {
                    $(this).addClass('kb-toolbarbtn-hl');
                    if (self._isOn) {
                        $(this).removeClass('kb-toolbarbtn-on');
                    }
                }
            }, function () {
                if (self.disabled == false) {
                    $(this).removeClass('kb-toolbarbtn-hl');
                    if (self._isOn) {
                        $(this).addClass('kb-toolbarbtn-on');
                    }
                }
            }).mousedown(function () {
                if (self.disabled == false && yardi.dialoging != true) {
                    if (self.onClick(self.editor) !== false) {
                        self.isOn(!self._isOn);
                    }
                }
            }).addClass('kb-toolbarbtn');
            // editor callback to update toolbar button status
            this.editor.updateToolbar.add(function (sender, hasSel) {
                self.onUpdate(sender, hasSel);
            });
            // disabled css
            this.isEnable(!this.disabled);
        },

        remove: function () {
            this.el.remove();
        },

        // public
        isOn: function (on) {
            this._isOn = (on === true);
            if (this._isOn) {
                this.el.addClass('kb-toolbarbtn-on');
            } else {
                this.el.removeClass('kb-toolbarbtn-on');
            }
        },

        // public
        isEnable: function (enable) {
            this.disabled = (enable == false);
            if (this.disabled) {
                this.el.addClass('kb-toolbarbtn-disabled').removeClass('kb-toolbarbtn-hl');
            } else {
                this.el.removeClass('kb-toolbarbtn-disabled');
            }
        },

        // virtual
        buildHtml: function () {
            alert('buildHtml must be overrided by children class.');
        },

        // virtual
        onClick: function (editor) {
            alert('onClick must be overrided by children class.');
        },

        // virtual
        onUpdate: function (editor, hasSel) {
            alert('onUpdate must be overrided by children class.');
        }
    };

    /*
    * bold
    */
    var boldButton = function (config) {
        boldButton.superclass.constructor.call(this, config);
    };
    yardi.extend(boldButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_bold_off.gif" alt="bold" title="' + options.bold + '" />';
        },
        onClick: function (editor) {
            editor.bold();
        },
        onUpdate: function (editor, hasSel) {
            this.isOn(editor.bold(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    yardi.boldButton = boldButton;

    /*
    * italic
    */
    var italicButton = function (config) {
        italicButton.superclass.constructor.call(this, config);
    };
    yardi.extend(italicButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_italic_off.gif" alt="italic" title="' + options.italic + '" />';
        },
        onClick: function (editor) {
            editor.italic();
        },
        onUpdate: function (editor, hasSel) {
            this.isOn(editor.italic(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    yardi.italicButton = italicButton;

    /*
    * underline
    */
    var underlineButton = function (config) {
        underlineButton.superclass.constructor.call(this, config);
    };
    yardi.extend(underlineButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_underline_off.gif" alt="underline" title="' + options.underline + '" />';
        },
        onClick: function (editor) {
            editor.underline();
        },
        onUpdate: function (editor, hasSel) {
            this.isOn(editor.underline(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    yardi.underlineButton = underlineButton;

    /*
    * align left
    */
    var alignleftButton = function (config) {
        alignleftButton.superclass.constructor.call(this, config);
    };
    yardi.extend(alignleftButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_alignleft_off.gif" alt="align left" title="' + options.alignLeft + '" />';
        },
        onClick: function (editor) {
            editor.justifyLeft();
        },
        onUpdate: function (editor, hasSel) {
            this.isOn(editor.justifyLeft(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    yardi.alignleftButton = alignleftButton;

    /*
    * align center
    */
    var aligncenterButton = function (config) {
        aligncenterButton.superclass.constructor.call(this, config);
    };
    yardi.extend(aligncenterButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_aligncenter_off.gif" alt="align center" title="' + options.alignCenter + '" />';
        },
        onClick: function (editor) {
            editor.justifyCenter();
        },
        onUpdate: function (editor, hasSel) {
            this.isOn(editor.justifyCenter(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    yardi.aligncenterButton = aligncenterButton;

    /*
    * align right
    */
    var alignrightButton = function (config) {
        alignrightButton.superclass.constructor.call(this, config);
    };
    yardi.extend(alignrightButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_alignright_off.gif" alt="align right" title="' + options.alignRight + '" />';
        },
        onClick: function (editor) {
            editor.justifyRight();
        },
        onUpdate: function (editor, hasSel) {
            this.isOn(editor.justifyRight(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    yardi.alignrightButton = alignrightButton;

    /*
    * align justify
    */
    var alignjustifyButton = function (config) {
        alignjustifyButton.superclass.constructor.call(this, config);
    };
    yardi.extend(alignjustifyButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_alignjustify_off.gif" alt="align justify" title="' + options.alignJustify + '" />';
        },
        onClick: function (editor) {
            editor.justifyFull();
        },
        onUpdate: function (editor, hasSel) {
            this.isOn(editor.justifyFull(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    yardi.alignjustifyButton = alignjustifyButton;

    /*
    * number list
    */
    var numberlistButton = function (config) {
        numberlistButton.superclass.constructor.call(this, config);
    };
    yardi.extend(numberlistButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_numberlist_off.gif" alt="number list" title="' + options.numberList + '" />';
        },
        onClick: function (editor) {
            editor.insertOrderedList();
        },
        onUpdate: function (editor, hasSel) {
            this.isOn(editor.insertOrderedList(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    yardi.numberlistButton = numberlistButton;

    /*
    * bullet list
    */
    var bulletlistButton = function (config) {
        bulletlistButton.superclass.constructor.call(this, config);
    };
    yardi.extend(bulletlistButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_bulletlist_off.gif" alt="bullet list" title="' + options.bulletList + '" />';
        },
        onClick: function (editor) {
            editor.insertUnorderedList();
        },
        onUpdate: function (editor, hasSel) {
            this.isOn(editor.insertUnorderedList(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    yardi.bulletlistButton = bulletlistButton;

    /*
    * indent
    */
    var indentButton = function (config) {
        indentButton.superclass.constructor.call(this, config);
    };
    yardi.extend(indentButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_indent_off.gif" alt="increase indent" title="' + options.indent + '" />';
        },
        onClick: function (editor) {
            editor.indent();
        },
        onUpdate: function (editor, hasSel) {
            this.isOn(editor.indent(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    yardi.indentButton = indentButton;

    /*
    * outdent
    */
    var outdentButton = function (config) {
        outdentButton.superclass.constructor.call(this, config);
    };
    yardi.extend(outdentButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_outdent_off.gif" alt="decrease indent" title="' + options.outdent + '" />';
        },
        onClick: function (editor) {
            editor.outdent();
        },
        onUpdate: function (editor, hasSel) {
            this.isOn(editor.outdent(true));
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    yardi.outdentButton = outdentButton;

    /*
    * insert image
    */
    var insertimageButton = function (config) {
        insertimageButton.superclass.constructor.call(this, config);
    };
    yardi.extend(insertimageButton, toolbarButton, {
        panel: null,
        //remove: function () {
        //    if (this.panel) {
        //        this.panel.remove();
        //        this.panel = null;
        //    }
        //    insertimageButton.superclass.remove.call(this);
        //},
        buildHtml: function () {
            return '<img src="{0}/tb_insert_image_off.gif" alt="insert image" title="' + options.insertImage + '" />';
        },
        onClick: function (editor) {
            editor.Sniffer.insertNewImg();
            //this.panel = new yardi.imagePanel({
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
    yardi.insertimageButton = insertimageButton;

    /*
    * insert link
    */
    var insertlinkButton = function (config) {
        insertlinkButton.superclass.constructor.call(this, config);
    };
    yardi.extend(insertlinkButton, toolbarButton, {
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
            return '<img src="{0}/tb_insert_link_off.gif" alt="insert link" title="' + options.insertLink + '" />';
        },
        onClick: function (editor) {
            editor.Sniffer.insertNewLink();
            //var text = editor.Selection.getText();
            //this.panel = new yardi.linkPanel({
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
    yardi.insertlinkButton = insertlinkButton;

    /*
    * edit source
    */
    var editsourceButton = function (config) {
        editsourceButton.superclass.constructor.call(this, config);
    };
    yardi.extend(editsourceButton, toolbarButton, {
        sourcePanel: null,
        buildHtml: function () {
            return '<img src="{0}/tb_eidt_source_off.gif" alt="edit source" title="' + options.editSource + '" />';
        },
        onClick: function (editor) {
            var self = this;
            this.sourcePanel = new yardi.textPanel({
                title: 'source view',
                textValue: editor.getHtml(),
                onOk: function (newHtml) {
                    editor.setHtml(newHtml);
                    self.closePanel();
                },
                onCancel: function (oldHtml) {
                    editor.setHtml(oldHtml);
                    self.closePanel();
                },
                onPreview: function (html) {
                    editor.setHtml(html);
                }
            });
            this.sourcePanel.show(this.el);
            // bind tween event
            setTimeout(function () {
                self._twinkle = function (ev) {
                    if (self.sourcePanel && !yardi.isAncestor(self.sourcePanel.el[0], ev.target)) {
                        self.sourcePanel.twinkle();
                    }
                };
                $(document).mousedown(self._twinkle);
            }, 300);
        },
        closePanel: function () {
            if (!this.sourcePanel) { return; }
            this.sourcePanel.remove();
            this.sourcePanel = null;
            $(document).unbind('mousedown', this._twinkle);
        },
        onUpdate: function (editor, hasSel) {
            this.isEnable(editor.sourceView == false);
        }
    });
    // register
    yardi.editsourceButton = editsourceButton;

    /*
    * redo
    */
    var redoButton = function (config) {
        redoButton.superclass.constructor.call(this, config);
    };
    yardi.extend(redoButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_redo_off.gif" alt="redo" title="' + options.redo + '" />';
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
    yardi.redoButton = redoButton;

    /*
    * undo
    */
    var undoButton = function (config) {
        undoButton.superclass.constructor.call(this, config);
    };
    yardi.extend(undoButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_undo_off.gif" alt="undo" title="' + options.undo + '" />';
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
    yardi.undoButton = undoButton;

    /*
    * unformat
    */
    var unformatButton = function (config) {
        unformatButton.superclass.constructor.call(this, config);
    };
    yardi.extend(unformatButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_unformat_off.gif" alt="remove format" title="' + options.unformat + '" />';
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
    yardi.unformatButton = unformatButton;

    /*
    * horizontal ruler
    */
    var horizontalRulerButton = function (config) {
        horizontalRulerButton.superclass.constructor.call(this, config);
    };
    yardi.extend(horizontalRulerButton, toolbarButton, {
        buildHtml: function () {
            return '<img src="{0}/tb_horizontalruler_off.gif" alt="hr" title="' + options.horizontalRuler + '" />';
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
    yardi.horizontalRulerButton = horizontalRulerButton;

})(jQuery);
