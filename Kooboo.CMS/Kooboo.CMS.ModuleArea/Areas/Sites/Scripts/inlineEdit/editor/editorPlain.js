/*
*
* editorPlain
* author: ronglin
* create date: 2012.3.2
*
*/

(function ($) {

    var editorPlain = function (config) {
        editorPlain.superclass.constructor.call(this, config);
    };

    yardi.extend(editorPlain, yardi.editor, {

        richText: false, _plainBlur: null, _plainPaste: null, _plainKeydown: null, _timeoutId: null,

        initialize: function () {
            var self = this;
            editorPlain.superclass.initialize.call(this);
            this.Sniffer.destroy();
            // events
            this.el.blur(this._plainBlur = function () { self.stripHtml(false); });
            if (this.el.get(0).onpaste === null) { // test if support paste event
                this.el.bind('paste', self._plainPaste = function () {
                    self.stripHtml();
                });
            }
            this.el.keydown(self._plainKeydown = function (ev) {
                if (!ev.ctrlKey) { return; }
                var keyCode = ev.keyCode;
                if (keyCode === 65 || // ctrl+a
                    keyCode === 67 || // ctrl+c
                    keyCode === 88 || // ctrl+x
                    keyCode === 89 || // ctrl+y
                    keyCode === 90) { // ctrl+z
                    // do default
                } else if (keyCode === 86) { // ctrl+v
                    if (self._plainPaste === null) {
                        self.stripHtml();
                    }
                } else {
                    // prevent some browser default comments.
                    // for example: prevent ctrl+b to set bold style to text.
                    ev.preventDefault();
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

        _firstFocus: function () {
            // override with a empty implement
        },

        _unifyInputs: function (ev) {
            // enter
            if (ev.which == 13) {
                return false;
            }
            // tab
            if (ev.which == 9) {
                this._pasteHtml('    ');
                return false;
            }
        },

        getHtml: function () {
            return this.el.text();
        },

        setHtml: function (str) {
            this.el.text(str);
        },

        remove: function () {
            if (this._plainBlur) { this.el.unbind('blur', this._plainBlur); }
            if (this._plainPaste) { this.el.unbind('paste', this._plainPaste); }
            if (this._plainKeydown) { this.el.unbind('keydown', this._plainKeydown); }
            editorPlain.superclass.remove.call(this);
        }

    });

    // register
    yardi.editorPlain = editorPlain;

})(jQuery);
