/// <reference path="../jquery-1.4.1-vsdoc.js" />
/// <reference path="../codeMirror/codemirror.js" />
/// <reference path="selection.js" />

(function () {
    $.fn._codeMirror = function (option) {
        var textArea = this;
        var value = this.val();

        this.val(value);

        var id = this.attr("id");

        var config = {
            parserfile: ["parsexml.js", "parsecss.js", "tokenizejavascript.js", "parsejavascript.js", "parsehtmlmixed.js"],
            stylesheet: ["/Areas/Sites/Styles/codeMirror/base/xmlcolors.css", "/Areas/Sites/Styles/codeMirror/base/jscolors.css", "/Areas/Sites/Styles/codeMirror/base/csscolors.css"],
            path: "/Areas/Sites/Scripts/codeMirror/"
        };
        $.extend(config, option);
        var c = CodeMirror.fromTextArea(id, config);

        this.next().removeAttr("style");

        var iframeId = "iframe" + id;
        var iframe = this.next().find("iframe").attr('id', iframeId);
        iframe.attr("frameborder", "1");

        function getContentWin() {
            return iframe.get(0).contentWindow;
        }


        var html = iframe.contents().find("html").css({ height: "100%" });
        var body = iframe.contents().find("body");

        var api = {
            insertAtCursor: function (val) {

                try {
                    var lineHandler = c.cursorLine();
                    //temp variable
                    var lineContent = c.lineContent(lineHandler);
                    var cursorPosition = c.cursorPosition(0);
                    var positionOffset = lineContent.length - cursorPosition.character;
                    var nextLine = c.nextLine(lineHandler);
                    //inserting
                    c.insertIntoLine(lineHandler, cursorPosition.character, val);
                    //jump to after inserted lines.
                    var insertedLine = c.prevLine(nextLine);
                    c.jumpToLine(nextLine);
                    //set the cursor position.
                    var content = c.lineContent(insertedLine);
                    c.selectLines(insertedLine, content.length - positionOffset);
                    c.focus();
                }
                catch (e) {
                    //return false;
                }

                return true;
            },
            getText: function () {
                return c.getCode();
            },
            focus: function () {
                $(getContentWin()).focus();
            },
            init: function (code) {
                code = code || c.getCode();
                c.setCode(code);
            },
            getCode: function () {
                return c.getCode();
            },
            removePosition: function (positionId) {
                var code = this.getCode();

                this.init();
            },
            editPosition: function () {

            },
            onblur: function (func) {
                $(getContentWin().document).blur(function () {
                    func();
                });
            },
            keyup: function (func) {
                $(getContentWin().document).keyup(function () {
                    func();
                });
            },
            undo: function () {
                c.undo();
            },
            redo: function () {
                c.redo();
            },
            codeMirror: c
        };

        textArea.data("codeMirror", api);
    }

    $.fn.codeMirror = function (option) {
        var current = this;
        $(function () { current.each(function () { $(this)._codeMirror(option); }); });
        return this;
    }
})();
