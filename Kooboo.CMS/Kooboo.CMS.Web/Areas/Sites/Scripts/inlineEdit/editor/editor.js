/*
*   editor
*   author: ronglin
*   create date: 2010.11.23
*   modified by Raoh in 2013.05.03
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
        /*

        // environment scope
        win: null, doc: null,

        // flags
        activated: null, sourceView: false, enable: true, lastSelection: null, 

        // extend components
        Selection: null, Redoundo: null, Sniffer: null,
        */
        /****************************************/
        //原有的
        el: null,
        renderTo: null,
        initialized: false,
        richText: true,
        /****************************************/
        //新增的
        onSave: null,
        onCancel: null,
        tooltip: null,
        eidtorInstance: null,
        remove: function () {
            if (this.initialized) {
                this._onBeforeExit();
                this.el.unbind();
                this.editorInstance && this.editorInstance.remove();
            }
        },
        _onBeforeExit: function () {
            this.tooltip && this.tooltip.remove();
            this.tooltip = null;
        },
        getSaveEl: function () {
            return $("i.mce-i-save").parent("button").get(0);
        },
        message: function (msg, ref) {
            if (!msg) {
                this.tooltip && this.tooltip.hide();
            } else {
                if (!this.tooltip) {
                    this.tooltip = new tinyMCE.ui.Tooltip({ type: 'tooltip' });
                    $(document).bind('mouseup', function () {
                        this.tooltip && this.tooltip.hide();
                    });
                }
                this.tooltip.renderTo(document.body);
                this.tooltip.getEl().lastChild.innerHTML = msg;
                this.tooltip.moveTo(-0xFFFF).show().moveRel(ref, 'bc tc');
            }
        },
        initialize: function () {
            /*************Start*****************/
            //Added by Raoh in 2013-06-20
            if (this.initialized) {
                return;
            }
            this.initialized = true;
            var self = this;
            // define events
            $.each([
                'onInitialized', 'onSetHtml', 'editorEvents', 'updateToolbar', 'onExecCommand',
                'onKeyup', 'onKeydown', 'onMouseup', 'onMousedown', 'onClick', 'onFocus', 'onBlur'
            ], function (index, item) {
                var cache = self[item];
                self[item] = new ctx.dispatcher(self);
                cache && ctx.isFunction(cache) && self[item].add(cache);
            });

            // gen environment scopes
            this.doc = this.el.get(0).ownerDocument;
            // create elem
            if (!this.el) { this.el = $('<div class="kb-editor"></div>').appendTo(this.renderTo); }
            this.editable(true);

            this.buildEditor(this.getSelector());

            // fire callback
            this.onInitialized.dispatch(this);
            /*************End*****************/
            /*
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
            */
        },
        getSelector: function () {
            return this.el.get(0).tagName + '.kb-editor-on';
        },
        buildEditor: function (selector) {
            var self = this;
            tinymce.init({
                selector: selector,
                plugins: [
                    ["advlist autolink link image lists charmap hr anchor pagebreak spellchecker"],
		    ["searchreplace wordcount visualblocks visualchars rawcode media nonbreaking"],
                    ["exit table contextmenu directionality emoticons template paste"]
                ],
                schema: "html5",
                inline: true,
                menubar: false,
                allow_script_urls: true,
                toolbar_items_size: 'small',
		toolbar: "save exit | searchreplace undo redo | bold italic forecolor formatselect | indent outdent | alignleft aligncenter alignright alignjustify | bullist numlist | image link unlink | rawcode",
                setup: function (ed) {                   
		     //tinymce.ui.FloatPanel.zIndex=0x7FFFFFFF;
                     ed.on('BeforeSetContent', function (e) {
                         e.format = 'raw';
                     });
                },
                verify_html: false,
                init_instance_callback: function (ed) {
                    self.editorInstance = ed;
                    setTimeout(function () {
                        ed.focus();
                        ed.off('blur');
                    }, 500);
                },
                exit_onsavecallback: function (ed) {
                    self.onSave && self.onSave();
                },
                exit_onbeforeexit: function (ed) {
                    //Need to delay for a while,otherwise, 
                    //immediately remove the tinymce editor instance, there was an error will be
                    setTimeout(function () {
                        self.onCancel && self.onCancel();
                        self._onBeforeExit();
                    }, 100);
                },
                /*added by Raoh in 2013-07-05*/
                file_browser_callback: function (inputId, value, fileType, window) {

                    // execute popup
                    var topJQ = top._jQuery || top.jQuery;
                    var id = new Date().getTime();
                    topJQ.pop({
                        id: id,
                        url: __inlineEditVars.MediaLibrary,
                        width: 900,
                        height: 500,
                        dialogClass: 'iframe-dialog',
                        frameHeight: '100%',
                        beforeLoad: function () {
                        },
                        onload: function (handle, pop, config) {
                            top.onFileSelected = function (src, text, option) {
                                var $srcInput = topJQ('#' + inputId);
                                $srcInput.val(src);
                                var $descriptionInput = $srcInput.parent().parent().parent().next().find('input');
                                $descriptionInput.val(text);
                            };
                            top.fileSelectPop = pop;
                        },
                        onclose: function (handle, pop, config) {

                        }
                    });
                    var contentCon = topJQ('#' + id);
                    if (contentCon.length > 0) {
                        var dialogCon = contentCon.parent('.ui-dialog').first();
                        var maskCon = dialogCon.next('.ui-widget-overlay').first();
                    }
                    var win = yardi.zindexCenter.getDomOwner(maskCon);
                    var zix = yardi.zindexCenter.queryWinZMax(win);
                    maskCon.css("z-index", parseInt(zix) + 1);
                    win = yardi.zindexCenter.getDomOwner(dialogCon);
                    zix = yardi.zindexCenter.queryWinZMax(win);
                    dialogCon.css("z-index", parseInt(zix) + 1);
                    //tinymce.ztopKoobooDialog(id);
                }
            });
        },

        editable: function (edit) {console.log(edit);
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
            //Added by Raoh in 2013-06-24
            this.editorInstance && this.editorInstance.setProgressState(!edit);
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
        },

        isRichText: function () {
            return this.richText;
        },

        setEnable: function (enable) {
            this.enable = (enable !== false);
            // test if support 'disabled'
            //Modified by Raoh in 2013-06-20
            //becuase tinymce will be set disabled property for this element.
            /*if (this.el.get(0).disabled !== undefined) {
                if (this.enable) {
                    this.el.removeAttr('disabled');
                } else {
                    this.el.attr('disabled', 'DISABLED');
                }
            } else {
                this.
(this.enable);
            }*/
            this.editable(this.enable);
        },

        setHtml: function (html, set) {
            ctx.rawHtml(this.el, html, set);
            this.processInnerHtml();
            this.onSetHtml.dispatch(this, html);
        },

        getHtml: function () {
            return this.el.html();
        }
    };

    // register
    ctx.editor = editor;

})(yardi, jQuery);
