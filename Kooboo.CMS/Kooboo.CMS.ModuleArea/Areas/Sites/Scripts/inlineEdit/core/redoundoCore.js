/*
*
* redo undo core
* author: ronglin
* create date: 2010.08.02
*
*/

/*
* config parameters:
* size, async
*
* dispatch events:
* onCommit, onUndo, onRedo, onTrim
*/

(function ($) {

    var redoundoCore = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    redoundoCore.prototype = {

        size: 50, async: true,

        index: -1, history: [],

        overflowed: false, executing: false,

        // private fn
        initialize: function () {
            // reference type object must reset propotype value
            this.history = [];

            // supported events
            var self = this;
            $.each(['onCommit', 'onUndo', 'onRedo', 'onTrim'], function (index, item) {
                self[item] = new yardi.dispatcher(self);
            });
        },

        // cmd format:  { undo: fn, redo: fn, [trim: fn] }
        commit: function (cmd) {
            var items = [];
            if (this.history.length == this.size) {
                items.push(this.history.shift());
                this.overflowed = true;
            } else {
                this.index++;
                items = this.history.splice(this.index, this.history.length - this.index);
            }
            this.trim(items);
            this.history[this.index] = cmd;
            this.onCommit.dispatch(this, cmd);
        },

        trim: function (cmds) {
            this.onTrim.dispatch(this, cmds);
            for (var i = 0; i < cmds.length; i++) {
                if (cmds[i].trim) {
                    cmds[i].trim();
                }
            }
        },

        redo: function () {
            if (this.canRedo()) {
                this.executing = true;
                this.index++;
                var cmd = this.history[this.index];
                var self = this, event = {
                    done: function () {
                        self.executing = false;
                        self.onRedo.dispatch(self, cmd);
                    }
                };
                cmd.redo(event);
                this.executing && (!this.async) && event.done();
            }
        },

        undo: function () {
            if (this.canUndo()) {
                this.executing = true;
                var cmd = this.history[this.index];
                this.index--;
                var self = this, event = {
                    done: function () {
                        self.executing = false;
                        self.onUndo.dispatch(self, cmd);
                    }
                };
                cmd.undo(event);
                this.executing && (!this.async) && event.done();
            }
        },

        prevCmd: function () {
            if (this.canUndo()) {
                return this.history[this.index];
            }
        },

        nextCmd: function () {
            if (this.canRedo()) {
                return this.history[this.index + 1];
            }
        },

        canRedo: function () {
            return (this.executing === false && this.index < this.history.length - 1);
        },

        canUndo: function () {
            return (this.executing == false && this.index > -1);
        },

        clear: function () {
            this.index = -1;
            this.history = [];
            this.executing = false;
            this.overflowed = false;
        }

    };

    // register
    yardi.redoundoCore = redoundoCore;

})(jQuery);
