/// <reference path="../../../../Scripts/jquery.js" />
/// <reference path="codemirror.js" />
/// <reference path="codemirror.all.js" />
;
(function () {
    var $codeMirror = function (textArea, option) {
        option = $.extend({ mode: 'text/html'
        }, option);
        var codemirror ;
        try
        {
            codemirror = CodeMirror.fromTextArea(textArea.get(0), option)
        }catch(E){}
        
        
        if (typeof option.sizeProvider == 'function') {
            var codeMirrorDiv = textArea.next();
            $(window).resize(function () {
                option.sizeProvider.call(textArea, codeMirrorDiv);
            });

            option.sizeProvider.call(textArea, codeMirrorDiv);
        };

        if(!codemirror){
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
            var offset =totalChar?( totalChar.length - cursor1.ch):0;


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
                try
                {
                    var codemirror = $codeMirror(handle, option);
                    handle.data('codeMirror', codemirror);
                    handle.codeMirrorAPI = codemirror;
                }catch(E){ }
            }
        });
        return this;
    };
})();
