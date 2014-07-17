/*
*   editorPlain
*   author: ronglin
*   create date: 2012.3.2
*/

(function (ctx, $) {

    var editorPlain = function (config) {
        editorPlain.superclass.constructor.call(this, config);
    };

    ctx.extend(editorPlain, ctx.editor, {

        richText: false, _plainBlur: null, _plainPaste: null, _plainKeydown: null, _timeoutId: null,

        initialize: function () {
            var self = this;
            editorPlain.superclass.initialize.call(this);
            this.el.addClass("kb-editor-hl");
            //this.Sniffer.destroy();
            //// events
            /*this.el.blur(this._plainBlur = function () { self.stripHtml(false); });
            if (this.el.get(0).onpaste === null) { // test if support paste event
                this.el.bind('paste', self._plainPaste = function () {
                    self.stripHtml();
                });
            }*/
            this.el.keydown(self._plainKeydown = function (ev) {
                var keyCode = ev.keyCode;
                if (ev.ctrlKey) {
                    if (keyCode === 65 || // ctrl+a
                        keyCode === 67 || // ctrl+c
                        keyCode === 88 || // ctrl+x
                        keyCode === 89 || // ctrl+y
                        keyCode === 90) { // ctrl+z
                        // do default
                    } else if (keyCode === 86) { // ctrl+v
                        /*if (self._plainPaste === null) {
                            self.stripHtml();
                        }*/
                    } else {
                        // prevent some browser default comments.
                        // for example: prevent ctrl+b to set bold style to text.
                        ev.preventDefault();
                    }
                }
                if (ev.shiftKey) {
                    if (keyCode === 45) { // shift+insert
                        /*if (self._plainPaste === null) {
                            self.stripHtml();
                        }*/
                    }
                }
            });
        },
        getSelector: function () {
            return this.el.get(0).tagName + '.kb-editor-on';
        },
        buildEditor: function (selector) {
            var self = this;
            tinymce.init({
                selector: selector,
                plugins: [
                    "exit"
                ],
                inline: true,
                menubar: false,
                force_p_newlines: false,
                forced_root_block: false,
                toolbar_items_size: 'small',
                toolbar: "save exit | undo redo |",
                init_instance_callback: function (ed) {
                    self.editorInstance = ed;
                    setTimeout(function () {
                        ed.focus();
                        ed.off('blur');
                    }, 500);
                },
                exit_onsavecallback: function (ed) {
                    self.onSave && self.onSave();
                    //self.stripHtml(false);
                },
                exit_onbeforeexit: function (ed) {
                    setTimeout(function () {
                        self.onCancel && self.onCancel();
                        self._onBeforeExit();
                    }, 100);
                }
            });
        },

        stripHtml: function (timeout) {
            clearTimeout(this._timeoutId);
            if (timeout === false) {
                this.el.text(this.el.text());
            } else {
                var self = this;
                this._timeoutId = setTimeout(function () {
                    self.el.text(self.el.text());
                }, 0);
            }
        },

        //_firstFocus: function () {
        //    // override with a empty implement
        //},

        //_unifyInputs: function (ev) {
        //    // enter
        //    if (ev.which == 13) {
        //        return false;
        //    }
        //    // tab
        //    if (ev.which == 9) {
        //        this.pasteHtml('    ');
        //        return false;
        //    }
        //},

        getHtml: function () {
            return this.el.html();//text();
        },

        setHtml: function (str) {
            this.el.html(str);//text(str);
        },

        remove: function () {
            editorPlain.superclass.remove.call(this);
            if (this._plainBlur) { this.el.unbind('blur', this._plainBlur); }
            if (this._plainPaste) { this.el.unbind('paste', this._plainPaste); }
            if (this._plainKeydown) { this.el.unbind('keydown', this._plainKeydown); }
        }

    });

    // register
    ctx.editorPlain = editorPlain;

})(yardi, jQuery);
