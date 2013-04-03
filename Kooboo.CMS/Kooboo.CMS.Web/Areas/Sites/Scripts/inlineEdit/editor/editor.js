/*
*   editor
*   author: ronglin
*   create date: 2010.11.23
*/

/*
* config parameters:
* el, renderTo
*
* dispatch events:
* onInitialized', 'editorEvents', 'updateToolbar', 'onExecCommand', 'onSetHtml'
* onKeyup', 'onKeydown', 'onMouseup', 'onMousedown', 'onClick', 'onFocus', 'onBlur'
*/

(function (ctx, $) {

    var editor = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    var travelNodes = function (elements, calback) {
        for (var i = 0; i < elements.length; i++) {
            var node = elements[i];
            calback(node);
            travelNodes(node.childNodes, calback);
        }
    };

    editor.prototype = {

        el: null, renderTo: null,

        // environment scope
        win: null, doc: null,

        // flags
        activated: null, initialized: false, sourceView: false, enable: true, lastSelection: null, richText: true,

        // extend components
        Selection: null, Redoundo: null, Sniffer: null,

        initialize: function () {
            var self = this;
            if (this.initialized == true) { return; }
            this.initialized = true;

            // create elem
            if (!this.el) { this.el = $('<div class="kb-editor"></div>').appendTo(this.renderTo); }

            // gen environment scopes
            this.doc = this.el.get(0).ownerDocument;
            this.win = this.doc.parentWindow || this.doc.defaultView; // ie and opera has 'parentWindow' others 'defaultView'

            // empty editor
            if (ctx.isGecko) {
                if (!this.el.get(0).childNodes.length)
                    this.el.get(0).appendChild(this.doc.createTextNode('\uFEFF'));
            }

            // set editable
            this.editable(true);

            // hack to process html
            this.processInnerHtml();

            // define events
            $.each([
                'onInitialized', 'editorEvents', 'updateToolbar', 'onExecCommand', 'onSetHtml',
                'onKeyup', 'onKeydown', 'onMouseup', 'onMousedown', 'onClick', 'onFocus', 'onBlur'
            ], function (index, item) {
                var cache = self[item];
                self[item] = new ctx.dispatcher(self);
                cache && ctx.isFunction(cache) && self[item].add(cache);
            });

            // instances
            this.Sniffer = new ctx.editorSniffer(this);
            this.Selection = new ctx.selectionClass(this.win);
            this.Redoundo = new ctx.editorRedoundo({ editor: this });

            // bind events
            this._bindEvents();

            // fire callback
            this.onInitialized.dispatch(this);
        },

        editable: function (edit) {
            edit = (edit !== false);
            // prop
            try {
                // ie7 must set manual
                this.el.get(0).contentEditable = edit;
            } catch (ex) { }
            // attr
            if (edit) {
                this.el.addClass('kb-editor-on');
                this.el.attr('spellCheck', 'false');
                this.el.attr('contenteditable', 'true');
            } else {
                this.el.removeClass('kb-editor-on');
                this.el.removeAttr('spellCheck'); // 'spellCheck' attribute name must not be all lowercase or uppercase
                this.el.removeAttr('contenteditable'); // in firefox 'contentEditable' attribute name must be lowercase
            }
            // attention to the attribute name upper-lower-case.
            // 1.attr('spellcheck', 'false') will set attribute spellcheck="true" except IE
            // 2.removeAttr('contentEditable') will cause jquery error.
        },

        _bindEvents: function () {
            var self = this;

            // events for update toolbar
            var timeoutId;
            var editorEvent = function (ev) {
                if (!self.enable) { return; }
                self.editorEvents.dispatch(self, ev);
                clearTimeout(timeoutId);
                timeoutId = setTimeout(function () {
                    self.triggerUpdateToolbar(ev);
                }, 200);
            };
            this.onExecCommand.add(function () { self.triggerUpdateToolbar(); });

            // bind
            this.el[ctx.isOpera ? 'keypress' : 'keydown'](function (ev) {
                self.onKeydown.dispatch(self, ev);
                return self._unifyInputs(ev);
            }).keyup(function (ev) {
                self.onKeyup.dispatch(self, ev);
                editorEvent(ev);
            }).mouseup(function (ev) {
                self.onMouseup.dispatch(self, ev);
                editorEvent(ev);
            }).mousedown(function (ev) {
                self.onMousedown.dispatch(self, ev);
                editorEvent(ev);
            }).click(function (ev) {
                self.onClick.dispatch(self, ev);
                editorEvent(ev);
            }).focus(function (ev) {
                self.onFocus.dispatch(self, ev);
                self.activated = true;
            }).blur(function (ev) {
                self.onBlur.dispatch(self, ev);
                self.activated = false;
            });

            // some browser scroll editor element to view when focus, so prevent it
            this.el.focus(function (ev) { ev.preventDefault(); });
            this.el.one('focus', function (ev) { self._firstFocus(); });

            // 1.ctrl+a or other actions no tested may select the content outside the editor.
            // 2.in firefox3.6 and opera the cursor will invisible after the second time edit a same widget.
            // for fix these issues, do some hacks and set focus to first text inside the editor
            if ((ctx.isGecko && !ctx.isGecko4) || ctx.isOpera) {
                // the comment 6 is my post. https://bugzilla.mozilla.org/show_bug.cgi?id=542727
                var markup = '<input type="text" value="v" style="width:0;height:0;border:0;padding:0;margin:0;" />';
                $(markup).appendTo(self.el).focus().select().remove(); // select a text, this is the core
            }
            this._focusAtText();

            // change some firefox feature
            if (ctx.isGecko) {
                try { this.doc.execCommand('enableInlineTableEditing', false, false); } catch (ex) { }
                try { this.doc.execCommand('enableObjectResizing', false, false); } catch (ex) { }
            }
            // change some ie feature
            if (ctx.isIE) {
                // when div set contenteditable="true" to implement inline edit, 
                // the elements contained that is display:none or visibility:hidden will be showed.
                // this feature is a default behavior of ie.
                // to fix it seeto: http://msdn.microsoft.com/en-us/library/aa770023%28VS.85%29.aspx
                try { this.doc.execCommand('RespectVisibilityInDesign', true, true); } catch (ex) { }
            }
        },

        _focusAtText: function (start) {
            // get first text node
            var textNode = function (el) {
                for (var i = 0; i < el.childNodes.length; i++) {
                    var node = el.childNodes[i];
                    if (node.nodeType === 3 && node.nodeValue.replace(/\s/g, '')) {
                        return node;
                    } else {
                        var n = arguments.callee(node);
                        if (n) { return n; }
                    }
                }
            } (this.el.get(0));

            // focus
            this.el.focus();

            // select the text node
            if (textNode) {
                textNode = ctx.isIE ? textNode.parentNode : textNode;
                try {
                    // these selection actions may cause unknow error, so try to exec them.
                    this.Selection.selectElement(textNode);
                    this.Selection.collapse(start || true);
                } catch (ex) { }
                // events created with .triggerHandler() do not bubble up the DOM hierarchy
                this.el.triggerHandler('click');
                this.el.focus();
            }
        },

        _cacheSel: null, _restoreSel: null,

        _firstFocus: function () {
            var retain = ['kb-editorAnchor', 'kb-arrowPanel', /*'kb-colorPicker',*/'kb-listPicker', 'kb-combo'];
            $.each(retain, function (i, str) { retain[i] = str.toLowerCase(); });
            var withIn = function (el) {
                if (!el) { return false; }
                var cls = el.className, hasClass = false;
                if (cls) {
                    cls = cls.toLowerCase();
                    $.each(retain, function () {
                        if (cls.indexOf(this) !== -1) {
                            hasClass = true;
                            return false;
                        }
                    });
                }
                if (hasClass) {
                    return true;
                } else {
                    return withIn(el.parentNode);
                }
            };
            // cache selection
            var cachedSelection = null, self = this;
            $(this.doc).mousedown(this._cacheSel = function (ev) {
                if (ctx.dialoging === true) { return; }
                if (withIn(ev.target)) {
                    if (cachedSelection == null) {
                        cachedSelection = self.Selection.getRange();
                        self.lastSelection = cachedSelection;
                    }
                }
            }).mouseup(this._restoreSel = function (ev) {
                if (ctx.dialoging === true) { return; }
                if (withIn(ev.target)) {
                    if (cachedSelection) {
                        self.focus();
                        self.Selection.selectRange(cachedSelection);
                        cachedSelection = null;
                    }
                }
            });
            ctx.eventPropagation.subscribe('mousedown', this._cacheSel);
            ctx.eventPropagation.subscribe('mouseup', this._restoreSel);
        },

        _unifyInputs: function (ev) {
            // enter
            if (ev.which == 13) {
                // FIREFOX BUG, 
                // sometimes unify enter will cause the mouse cursor escape from editor element and be hiddened.
                // CHROME BUG,
                // sometimes press enter will lost mouse cursor. ??
                // sometimes press enter key the cursor jump to the end of content. add back at 2011.6.20
                // IE BUG,
                // ie insert a P element when press enter by default, it cause nested error usually.
                if (ctx.isChrome || ctx.isIE) {
                    this.pasteHtml('<br/>');
                    return false;
                }
            }
            // tab
            if (ev.which == 9) {
                this.pasteHtml('&nbsp;&nbsp;&nbsp;&nbsp;');
                return false;
            }
        },

        pasteHtml: function (html) {
            // fix firefox bug, pasteHtml() may be paste html into 'br' element, 
            // may be other element types and browsers has the same or other problems too.
            // so this function select cursor manual for all browsers.
            if (!html) { return; }
            var tid = Math.random().toString();
            //TODO:
            // when there is a no-width-whitespace user must press backspace key twice to do remove.
            // but when i remove the no-width-whitespace, the redo undo selection in chrome has some problems.
            this.Selection.pasteHtml('<span id="' + tid + '">' + html + '</span>'); //this.Selection.pasteHtml('<span id="' + tid + '">' + html + '\uFEFF</span>');
            var node = this.doc.getElementById(tid);
            if (node) {
                while (node.childNodes.length) { node.parentNode.insertBefore(node.childNodes[0], node); }
                this.Selection.moveToBookmark({ id: tid });
                node.parentNode.removeChild(node);
            }
        },

        restoreLastSelection: function () {
            if (this.lastSelection) {
                this.Selection.selectRange(this.lastSelection);
            }
        },

        clearGarbage: function (el) {
            var el = el || this.el;
            var val = this.doc.createTextNode('\uFEFF').nodeValue;
            var reg = new RegExp(val, 'g'), removeList = [];
            travelNodes(el.get(0).childNodes, function (node) {
                if (node.nodeType != 3) { return; }
                try {
                    if (node.nodeValue == val) {
                        removeList.push(node);
                        return;
                    }
                    node.nodeValue = node.nodeValue.replace(reg, '');
                } catch (ex) { }
            });
            $.each(removeList, function () {
                this.parentNode.removeChild(this);
            });
        },

        processInnerHtml: function (revert, el) {
            var el = el || this.el;
            /*
            * clear impermissibility attributes 
            */
            var delAttrs = ['contentEditable'];
            travelNodes(el.get(0).childNodes, function (node) {
                if (node.nodeType != 1) { return; }
                $.each(delAttrs, function (i, key) {
                    node.removeAttribute(key);
                });
            });

            /*
            * in the msdn http://msdn.microsoft.com/en-us/library/ms533776.aspx
            * we notice that some css properties and corresponding values that, if set, cause an element to have layout.
            * and when element have layout in the editor, browser will attach resize handler function by default.
            * when focus to the element is appear some resize block around the element.
            * this is not comfortably in editor, it cause some edit problems too.
            * unfortunately, the hasLayout property in element is readonly.
            * so there is only one way to prevent resize handler: change css.
            * this link have some detail: http://www.satzansatz.de/cssd/onhavinglayout.html
            *
            * TODO: more css must be fixed.
            * follow function do remove or revert the effected css properties.
            */
            return;
            if (!ctx.isIE) { return; }
            revert = (revert === true);
            var cssHacks = ['width', 'height', 'min-width', 'min-height', 'max-width', 'max-height'];
            travelNodes(el.get(0).childNodes, function (node) {
                if (node.nodeType != 1) { return; }
                if (node.tagName == 'IMG' || node.tagName == 'A') { return; }
                node = $(node);
                if (revert) {
                    var cssText = node.attr('style');
                    if (cssText) {
                        $.each(cssHacks, function (i, key) {
                            cssText = cssText.replace(new RegExp('\s*' + key + ' *: *auto *;*\s*', 'ig'), '');
                        });
                        if (cssText) {
                            node.attr('style', cssText);
                        } else {
                            node.removeAttr('style');
                        }
                    }
                } else {
                    $.each(cssHacks, function (i, key) {
                        var v = ctx.currentStyle(node.get(0), key);
                        if (v != 'auto' && v != 'none' && v != 'inherit') {
                            node.css(key, 'auto');
                        }
                    });
                }
            });
        },

        remove: function () {
            if (!this.initialized) { return; }
            this.processInnerHtml(true);
            this.clearGarbage();
            this.editable(false);
            this.el.unbind();
            this.Sniffer.destroy();
            this.Redoundo.destroy();
            this.Selection.clearSelection();
            this._cacheSel && $(this.doc).unbind('mousedown', this._cacheSel);
            this._restoreSel && $(this.doc).unbind('mouseup', this._restoreSel);
            this._cacheSel && ctx.eventPropagation.unsubscribe('mousedown', this._cacheSel);
            this._restoreSel && ctx.eventPropagation.unsubscribe('mouseup', this._restoreSel);
        },

        isRichText: function () {
            return this.richText;
        },

        setEnable: function (enable) {
            this.enable = (enable !== false);
            // test if support 'disabled'
            if (this.el.get(0).disabled !== undefined) {
                if (this.enable) {
                    this.el.removeAttr('disabled');
                } else {
                    this.el.attr('disabled', 'DISABLED');
                }
            } else {
                this.editable(this.enable);
            }
        },

        setHtml: function (html, set) {
            ctx.rawHtml(this.el, html, set);
            this.processInnerHtml();
            this.onSetHtml.dispatch(this, html);
        },

        getHtml: function () {
            return this.el.html();
            //var clone = this.el.clone(false);
            //this.processInnerHtml(true, clone);
            //this.clearGarbage(clone);
            //var html = clone.html();
            //clone.remove();
            //return html;
        },

        focus: function () {
            this.el.focus();
        },

        triggerUpdateToolbar: function (ev) {
            var selectedText = this.Selection.getText();
            var hasSel = (selectedText != null && selectedText.length > 0);
            this.updateToolbar.dispatch(this, hasSel, ev);
        },

        /******** command helpers *********/
        replaceSelect: function (cmd, html) {
            if (this.activated === null) { return; }
            var elem = this.Selection.getParentElement();
            if (elem && !ctx.isAncestor(this.el.get(0), elem)) { return; }
            this.pasteHtml(html);
            this.onExecCommand.dispatch(this, cmd, html);
            //this.focus();
        },

        querySelectionStyle: function (name) {
            if (this.activated === null) { return; }
            var elem = this.Selection.getParentElement();
            if (elem) { return ctx.currentStyle(elem, name); }
        },

        queryCommandValue: function (cmd) {
            if (this.activated === null) { return; }
            try {
                var usecss = true;
                if (cmd.toLowerCase() == 'backcolor' && (ctx.isOpera || ctx.isGecko)) {
                    cmd = "HiliteColor";
                    usecss = false;
                }
                var ret, exec = function () {
                    ret = this.doc.queryCommandValue(cmd);
                };
                if (usecss === false) {
                    this.mozUseCss(exec);
                } else {
                    exec.call(this);
                }
                return ret;
            } catch (ex) { }
        },

        queryCommandState: function (cmd) {
            if (this.activated === null) { return; }
            try {
                return this.doc.queryCommandState(cmd);
            } catch (ex) { }
        },

        execCommand: function (cmd, val, fire) {
            if (this.activated === null) { return; }
            val = (val === undefined ? null : val);

            var cmdLower = cmd.toLowerCase();
            if (cmdLower == 'absoluteposition') // AbsolutePosition
                this.doc.execCommand('2D-Position', false, true);

            var usecss = true;
            if (cmdLower == 'backcolor' && (ctx.isOpera || ctx.isGecko)) {
                cmd = "HiliteColor";
                usecss = false;
            }
            var ret, exec = function () {
                ret = this.execDocumentCommand(cmd, val);
            };
            if (usecss === false) {
                this.mozUseCss(exec);
            } else {
                exec.call(this);
            }

            // fire custom exec command
            if (fire !== false) { this.onExecCommand.dispatch(this, cmd, val); }

            // ret
            return ret;
        },

        execDocumentCommand: function (cmd, val) {
            try {
                return this.doc.execCommand(cmd, false, val);
            } catch (ex) {
                var elem = this.Selection.getParentElement();
                var temp = this.doc.createElement('div');
                elem.insertBefore(temp, elem.firstChild);
                var ret = this.doc.execCommand(cmd, false, val);
                elem.removeChild(elem.firstChild);
                return ret;
            }
        },

        mozUseCss: function (cb) {
            try {
                this.doc.execCommand('UseCSS', false, false);
                this.doc.execCommand('styleWithCSS', false, true);
            } catch (ex) { }

            cb.call(this);

            try {
                this.doc.execCommand('UseCSS', false, true);
                this.doc.execCommand('styleWithCSS', false, false);
            } catch (ex) { }
        },

        /******** command start *********/
        bold: function (isGet) {
            if (isGet == true) {
                return this.queryCommandState('bold');
            } else {
                return this.execCommand('bold');
            }
        },

        italic: function (isGet) {
            if (isGet == true) {
                return this.queryCommandState('italic');
            } else {
                return this.execCommand('italic');
            }
        },

        underline: function (isGet) {
            if (isGet == true) {
                return this.queryCommandState('underline');
            } else {
                return this.execCommand('underline');
            }
        },

        indent: function (isGet) {
            if (isGet == true) {
                return this.queryCommandState('indent');
            } else {
                return this.execCommand('indent');
            }
        },

        outdent: function (isGet) {
            if (isGet == true) {
                return this.queryCommandState('outdent');
            } else {
                return this.execCommand('outdent');
            }
        },

        justifyLeft: function (isGet) {
            if (isGet == true) {
                return this.queryCommandState('justifyleft');
            } else {
                return this.execCommand('justifyleft');
            }
        },

        justifyCenter: function (isGet) {
            if (isGet == true) {
                return this.queryCommandState('justifycenter');
            } else {
                return this.execCommand('justifycenter');
            }
        },

        justifyRight: function (isGet) {
            if (isGet == true) {
                return this.queryCommandState('justifyright');
            } else {
                return this.execCommand('justifyright');
            }
        },

        justifyFull: function (isGet) {
            if (isGet == true) {
                return this.queryCommandState('justifyfull');
            } else {
                return this.execCommand('justifyfull');
            }
        },

        insertOrderedList: function (isGet) {
            if (isGet == true) {
                return this.queryCommandState('insertorderedlist');
            } else {
                return this.execCommand('insertorderedlist');
            }
        },

        insertUnorderedList: function (isGet) {
            if (isGet == true) {
                return this.queryCommandState('insertunorderedlist');
            } else {
                return this.execCommand('insertunorderedlist');
            }
        },

        fontSize: function (size, fire) {
            if (size === undefined) {
                var val = this.queryCommandValue('fontsize');
                return val ? val : this.querySelectionStyle('font-size');
            } else {
                return this.execCommand('fontsize', size, fire);
            }
        },

        fontName: function (name, fire) {
            if (name === undefined) {
                var val = this.queryCommandValue('fontname');
                return val ? val : this.querySelectionStyle('font-family');
            } else {
                return this.execCommand('fontname', name, fire);
            }
        },

        foreColor: function (color, fire) {
            if (color === undefined) {
                var value = this.queryCommandValue('forecolor');
                if (!value) { value = this.querySelectionStyle('color'); }
                return value ? (ctx.isIE ? ctx.colorHex(value) : value) : '#ffffff';
            } else {
                return this.execCommand('forecolor', color, fire);
            }
        },

        backColor: function (color, fire) {
            if (color === undefined) {
                // ie queryCommandValue will always return the color #ffffff.
                // and chrome queryCommandValue return the wrong color sometimes.
                var value = (ctx.isIE || ctx.isChrome) ? this.querySelectionStyle('background-color') : this.queryCommandValue('backcolor');
                return value ? (ctx.isIE ? ctx.colorHex(value) : value) : '#000000';
            } else {
                //TODO: execCommand in firefox will change the container background color but not text background color.
                return this.execCommand('backcolor', color, fire);
            }
        },

        strikeThrough: function (isGet) {
            if (isGet == true) {
                return this.queryCommandState('strikethrough');
            } else {
                return this.execCommand('strikethrough');
            }
        },

        insertImage: function (url) {
            if (ctx.isIE) {
                this.replaceSelect('insertimage', '<img src="' + url + '" />');
            } else {
                return this.execCommand('insertimage', url);
            }
        },

        createLink: function (url) {
            this.execCommand('unlink');
            return this.execCommand('createlink', url);
        },

        unformat: function () {
            this.execCommand('unlink');
            return this.execCommand('removeFormat');
        },

        insertHtml: function (html) {
            return this.execCommand('inserthtml', html);
        },

        horizontalRuler: function () {
            this.replaceSelect('hr', '<hr/>');
        },

        undo: function () {
            this.Redoundo.undo();
            this.triggerUpdateToolbar();
            //if (ctx.isIE) {
            //    this.Redoundo.undo();
            //} else {
            //    return this.execCommand('undo');
            //}
        },

        redo: function () {
            this.Redoundo.redo();
            this.triggerUpdateToolbar();
            //if (ctx.isIE) {
            //    this.Redoundo.redo();
            //} else {
            //    return this.execCommand('redo');
            //}
        }
        /******** command end *********/
    };

    // register
    ctx.editor = editor;

})(yardi, jQuery);
