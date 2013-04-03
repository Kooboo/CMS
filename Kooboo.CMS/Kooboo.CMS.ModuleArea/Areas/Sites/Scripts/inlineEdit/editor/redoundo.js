/*
*
* redo undo for htmleditor
* author: ronglin
* create date: 2010.06.18
*
*/

/*
* config parameters:
* size, async
*
* dispatch events:
* onCommit, onUndo, onRedo, onTrim
*/

/*
* config parameters:
* editor
*/

(function ($) {

    var editorRedoundo = function (config) {
        config.async = false;
        editorRedoundo.superclass.constructor.call(this, config);
    };

    yardi.extend(editorRedoundo, yardi.redoundoCore, {

        editor: null,

        filled: false,

        // override
        initialize: function () {
            editorRedoundo.superclass.initialize.call(this);
            // handler events
            var self = this, ed = this.editor;

            // fill
            //ed.onFocus.add(function (sender, ev) { self.fillOriginal(); });
            ed.onInitialized.add(function (sender) { self.fillOriginal(); });

            // check shoutcut
            ed.onKeyup.add(function (sender, ev) {
                var c = ev.keyCode;
                if (ev.ctrlKey) {
                    if (ev.shiftKey) {
                        (c == 90) && self.redo(); // ctrl + shift + z
                    } else {
                        (c == 90) && self.undo(); // ctrl + z
                        (c == 89) && self.redo(); // ctrl + y
                    }
                }
            });

            // check to do commit
            //ed.onBlur.add(function (sender, ev) { self.checkCommit(); });
            ed.onSetHtml.add(function (sender, html) { self.checkCommit(); });
            ed.onMousedown.add(function (sender, ev) { self.checkCommit(); });
            ed.onExecCommand.add(function (sender, ev) { self.checkCommit(); });
            ed.onKeyup.add(function (sender, ev) {
                var c = ev.keyCode;
                if ((c >= 33 && c <= 36) || // pageUp, pageDown, end, home
                (c >= 37 && c <= 40) || //left, up, right, down
                (c == 13) || // enter
                (c == 45) || // insert
                (c == 32) || // space
                (c == 9) || // tab
                (ev.ctrlKey) // ctrl
                ) { self.checkCommit(); }
            });
        },

        getContent: function () {
            return this.editor.el.html();
        },

        setContent: function (html) {
            this.editor.el.html(html);
        },

        createBookmark: function () {
            try {
                return this.editor.Selection.createBookmark();
            } catch (ex) { }
        },

        moveToBookmark: function (bm) {
            try {
                this.editor.Selection.moveToBookmark(bm);
            } catch (ex) { }
        },

        removeBookmark: function (bm) {
            this.editor.Selection.removeBookmark(bm);
        },

        fillOriginal: function () {
            if (this.filled == false) {
                this.filled = true;
                var content = this.getContent();
                this.doCommit(content, null);
            }
        },

        checkCommit: function () {
            var content = this.getContent();
            var current = this.history[this.index];
            if (current && current.changed(content)) {
                this.doCommit(content, current);
            }
        },

        doCommit: function (content, prevCommand) {
            var bookmark = this.createBookmark();
            var contentMarked = this.getContent();
            this.commit(new redoundoCommand({
                scope: this,
                content: content,
                contentMarked: contentMarked,
                bookmark: bookmark,
                prevCommand: prevCommand
            }));
            this.removeBookmark(bookmark);
        },

        // override
        canUndo: function () {
            return (this.index > 0);
        },

        // override
        clear: function () {
            editorRedoundo.superclass.clear.call(this);
            this.filled = false;
            this.fillOriginal();
        },

        destroy: function () {
            editorRedoundo.superclass.clear.call(this);
        }

    });

    /*
    * redoundo command class
    * used by editorRedoundo only.
    */
    var redoundoCommand = function (config) {
        $.extend(this, config);
    };

    redoundoCommand.prototype = {

        scope: null,

        content: null,

        contentMarked: null,

        bookmark: null,

        prevCommand: null,

        changed: function (content) {
            return (this.content != content);
        },

        redo: function () {
            this.scope.setContent(this.contentMarked);
            this.scope.moveToBookmark(this.bookmark);
            this.scope.removeBookmark(this.bookmark);
        },

        undo: function () {
            if (this.prevCommand) {
                this.scope.setContent(this.prevCommand.contentMarked);
                this.scope.moveToBookmark(this.prevCommand.bookmark);
                this.scope.removeBookmark(this.prevCommand.bookmark);
            }
        }

    };

    // register
    yardi.editorRedoundo = editorRedoundo;

})(jQuery);
