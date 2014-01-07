/*
*   redoundo
*   author: ronglin
*   create date: 2012.2.2
*/

/*
* config parameters:
* btnRedo, btnUndo, host
*/

(function ($, ctx) {

    // localization
    var txtRes = {
    };
    // localize text resource
    if (window.__localization) { $.extend(txtRes, __localization.redoundo_js); }

    // helpers
    var markSelf = function (obj) {
        obj['redoundo_js'] = true; return obj;
    }, isSelf = function (obj) {
        return (obj['redoundo_js'] === true);
    };

    /*
    * redoundoClass
    */
    var redoundoClass = function (config) {
        $.extend(this, config);
        //this.initialize();
    };

    redoundoClass.prototype = {

        btnRedo: null, btnUndo: null, host: null,

        currentRule: null, historyObjs: null, currentHistory: null,

        _redoclick: null, _undoclick: null, _onload: null, _onselect: null, _onrulechange: null,

        constructor: redoundoClass,

        initialize: function () {
            var self = this;
            this.historyObjs = {};
            this.btnRedo = $(this.btnRedo);
            this.btnUndo = $(this.btnUndo);
            // dom events
            this.btnRedo.click(this._redoclick = function () { self.currentHistory.redo(); });
            this.btnUndo.click(this._undoclick = function () { self.currentHistory.undo(); });
            // subscribe host events
            this._onrulechange = function (sender, set) {
                if (isSelf(set)) { return; }
                self.checkAndCommit();
            };
            this.host.onLoad.add(this._onload = function () { self.clean(); });
            this.host.onSelect.add(this._onselect = function (sender, set) {
                if (isSelf(set)) { return; }
                self.removeRuleSubscribe();
                self.updateRule(set.rule);
                self.currentRule.onPropertySort.add(self._onrulechange);
                self.currentRule.onPropertyChange.add(self._onrulechange);
                self.currentRule.onSelectorChange.add(self._onrulechange);
            });
        },

        updateRule: function (rule) {
            this.currentRule = rule;
            if (!rule._redoundoId) { rule._redoundoId = new Date().getTime(); }
            this.currentHistory = this.historyObjs[this.currentRule._redoundoId];
            if (!this.currentHistory) {
                // create history
                this.currentHistory = new yardi.redoundoCore({ async: false });
                this.historyObjs[this.currentRule._redoundoId] = this.currentHistory;
                // history hack
                this.currentHistory.canUndo = function () { return (this.index > 0); };
                // history event
                var self = this, disableFunc = function () { self.updateButtonState(); };
                this.currentHistory.onUndo.add(disableFunc);
                this.currentHistory.onRedo.add(disableFunc);
                this.currentHistory.onCommit.add(disableFunc);
                // commit default command
                this.checkAndCommit();
            }
            // update button state
            this.updateButtonState();
        },

        clean: function () {
            this.removeRuleSubscribe();
            this.historyObjs = {};
            this.currentRule = null;
            this.currentHistory = null;
            this.updateButtonState();
        },

        updateButtonState: function () {
            if (this.currentHistory && this.currentHistory.canUndo()) {
                this.btnUndo.removeAttr('disabled').removeClass('vs-redoundo-disabled');
            } else {
                this.btnUndo.attr('disabled', 'DISABLED').addClass('vs-redoundo-disabled');
            }
            if (this.currentHistory && this.currentHistory.canRedo()) {
                this.btnRedo.removeAttr('disabled').removeClass('vs-redoundo-disabled');
            } else {
                this.btnRedo.attr('disabled', 'DISABLED').addClass('vs-redoundo-disabled');
            }
        },

        checkAndCommit: function () {
            var property = this.currentRule.getPropertiesText();
            var selector = this.currentRule.getSelectorText();
            var lastCommand = this.currentHistory.history[this.currentHistory.index];
            if (!lastCommand || lastCommand.propertyChanged(property) || lastCommand.selectorChanged(selector)) {
                this.currentHistory.commit(new commandClass({
                    rule: this.currentRule,
                    prevCommand: lastCommand,
                    property: property, selector: selector
                }));
            }
        },

        removeRuleSubscribe: function () {
            if (this.currentRule) {
                this.currentRule.onPropertySort.remove(this._onrulechange);
                this.currentRule.onPropertyChange.remove(this._onrulechange);
                this.currentRule.onSelectorChange.remove(this._onrulechange);
            }
        },

        remove: function () {
            this.btnRedo.unbind('click', this._redoclick);
            this.btnUndo.unbind('click', this._undoclick);
            this.host.onLoad.remove(this._onload);
            this.host.onSelect.remove(this._onselect);
            this.removeRuleSubscribe();
            this.host = null;
        }
    };

    var commandClass = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    commandClass.prototype = {

        rule: null, prevCommand: null, selector: null, property: null,

        constructor: commandClass,

        initialize: function () { },

        selectorChanged: function (selector) {
            return (this.selector != selector);
        },

        propertyChanged: function (property) {
            return (this.property != property);
        },

        redo: function () {
            this.rule.setSelectorText(markSelf({ selector: this.selector }));
            this.rule.setPropertiesText(markSelf({ text: this.property }));
        },

        undo: function () {
            if (this.prevCommand) {
                this.rule.setSelectorText(markSelf({ selector: this.prevCommand.selector }));
                this.rule.setPropertiesText(markSelf({ text: this.prevCommand.property }));
            }
        }

    };

    // register
    ctx.redoundoClass = redoundoClass;

} (jQuery, visualstyle));
