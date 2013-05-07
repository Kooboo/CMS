;
(function () {
    function isFullScreen(cm) {
        return /\bCodeMirror-fullscreen\b/.test(cm.getWrapperElement().className);
    }
    function winHeight() {
        return window.innerHeight || (document.documentElement || document.body).clientHeight;
    }
    function setFullScreen(cm, full) {
        var wrap = cm.getWrapperElement();
        if (full) {
            wrap.className += " CodeMirror-fullscreen";
            document.body.className += " fullscreen";
            wrap.style.height = winHeight() + "px";
            document.documentElement.style.overflow = "hidden";

        } else {
            wrap.className = wrap.className.replace(" CodeMirror-fullscreen", "");
            document.body.className = document.body.className.replace(" fullscreen", "");
            wrap.style.height = "";
            document.documentElement.style.overflow = "";
        }
        cm.refresh();
    }
    $(function () {
        CodeMirror.on(window, "resize", function () {
            var showing = $(".CodeMirror-fullscreen")[0];
            if (!showing) return;
            showing.CodeMirror.getWrapperElement().style.height = winHeight() + "px";
        });

    });
    var $codeMirror = function (textArea, option) {
        option = $.extend({
            mode: 'text/html',
            extraKeys: {
                "F11": function (cm) {
                    setFullScreen(cm, !isFullScreen(cm));
                },
                "Esc": function (cm) {
                    if (isFullScreen(cm)) setFullScreen(cm, false);
                }
            },
            autoCloseTags: true,
            lineNumbers: true,
            lineWrapping: true
        }, option);
        var codemirror;
        try {
            codemirror = CodeMirror.fromTextArea(textArea.get(0), option)
        } catch (E) {
            console.log(E);
        }


        if (typeof option.sizeProvider == 'function') {
            var codeMirrorDiv = textArea.next();
            $(window).resize(function () {
                option.sizeProvider.call(textArea, codeMirrorDiv);
            });

            option.sizeProvider.call(textArea, codeMirrorDiv);
        };

        if (!codemirror) {
            return undefined;
        }

        codemirror.init = function (value) {
            value = value || this.getValue();
            this.setValue(value);
        };

        codemirror.getText = codemirror.getValue;
        codemirror.getCode = codemirror.getValue;
        codemirror.insertAtCursor = function (text) {
            var cursor1 = this.getCursor();
            var l1 = this.lineCount();

            var totalChar = this.getLine(cursor1.line);
            var offset = totalChar ? (totalChar.length - cursor1.ch) : 0;


            this.replaceSelection(text);

            var l2 = this.lineCount();
            var dif = l2 - l1;

            var line2 = cursor1.line + dif;
            var ch2 = this.getLine(line2).length;

            this.setCursor({
                line: line2,
                ch: ch2 - offset
            });
        };

        codemirror.changeMode = function (mode, mime) {
            this.setOption('mode', mode);
            return this;
        };

        var dic = {
            javascript: 'javascript',
            css: 'css',
            mixed: 'text/html'
        };

        for (var d in dic) {
            (function () {
                codemirror.changeMode[d] = function () { codemirror.changeMode.call(codemirror, dic[d]) };
            })();
        };

        return codemirror;
    };

    $.fn.codeMirror = function (option) {
        this.each(function () {
            var handle = $(this);
            if (!handle.data('codeMirror')) {
                try {
                    var codemirror = $codeMirror(handle, option);
                    handle.data('codeMirror', codemirror);
                    handle.codeMirrorAPI = codemirror;
                } catch (E) { }
            }
        });
        return this;
    };
})();
