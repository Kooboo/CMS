/*
*   effect
*   author: ronglin
*   create date: 2011.2.10
*/

/*
* config parameters:
* effectWin, host
*/

(function ($, ctx) {

    /*
    * effectClass
    */
    var effectClass = function (config) {
        $.extend(this, config);
        //this.initialize();
    };

    effectClass.prototype = {

        effectWin: null, host: null, currentRule: null, _onload: null, _onselect: null, _onunload: null,

        _onpropertychange: null, _onselectorchange: null, _onpropertysort: null, _onpreview: null, _onpreviewend: null,

        constructor: effectClass,

        initialize: function () {
            // substribe events
            var self = this;
            this._onpreview = function (sender, set) { self.applyCss(set); };
            this._onpreviewend = function (sender, set) { self.updateRule(self.currentRule); };
            this._onpropertysort = function (sender, set) { self.updateRule(self.currentRule); };
            this._onpropertychange = function (sender, set) { self.updateRule(self.currentRule); };
            this._onselectorchange = function (sender, set) { self.applySelector(set.selector); };
            this.host.onUnload.add(this._onunload = function () { self.removeRuleManager(); });
            this.host.onLoad.add(this._onload = function () { self.clean(); });
            this.host.onSelect.add(this._onselect = function (sender, set) {
                self.removeRuleSubscribe();
                self.updateRule(set.rule, true);
                self.currentRule.onPreview.add(self._onpreview);
                self.currentRule.onPreviewEnd.add(self._onpreviewend);
                self.currentRule.onPropertySort.add(self._onpropertysort);
                self.currentRule.onPropertyChange.add(self._onpropertychange);
                self.currentRule.onSelectorChange.add(self._onselectorchange);
            });
        },

        updateRule: function (rule, onselect) {
            this.currentRule = rule || this.currentRule;
            if (this.currentRule && !onselect) {
                this.applyCss({ propertyText: this.currentRule.getPropertiesText() });
            }
        },

        applySelector: function (selector) {
            //TODO:
        },

        clean: function () {
            if (this.ruleManager) { this.ruleManager.resetChanges(); }
        },

        applyCss: function (set) {
            if (!this.ruleManager) {
                this.ruleManager = new pageRuleManager({
                    win: this.effectWin
                });
            }
            set.selector = this.currentRule.getSelectorText();
            this.ruleManager.applyCss(set);
        },

        removeRuleManager: function () {
            if (this.ruleManager) {
                this.ruleManager.remove();
                this.ruleManager = null;
            }
        },

        removeRuleSubscribe: function () {
            if (this.currentRule) {
                this.currentRule.onPreview.remove(this._onpreview);
                this.currentRule.onPreviewEnd.remove(this._onpreviewend);
                this.currentRule.onPropertySort.remove(this._onpropertysort);
                this.currentRule.onPropertyChange.remove(this._onpropertychange);
                this.currentRule.onSelectorChange.remove(this._onselectorchange);
            }
        },

        remove: function () {
            this.host.onLoad.remove(this._onload);
            this.host.onSelect.remove(this._onselect);
            this.host.onUnload.remove(this._onunload);
            this.removeRuleSubscribe();
            this.removeRuleManager();
            this.el.remove();
            this.host = null;
            this.effectWin = null;
        }

    };


    var pageRuleManager = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    pageRuleManager.prototype = {

        win: null, changed: null, extendSheet: null,

        constructor: pageRuleManager,

        initialize: function () {
            this.changed = {};
        },

        applyCss: function (set) { // set { selector,propertyText }
            var selector = set.selector;
            var nospaceSel = ctx.nospace(selector);
            var sheets = this.win.document.styleSheets, effectList = [];
            for (var i = 0, slen = sheets.length; i < slen; i++) {
                var sheetItem = sheets[i];
                try {// try catch for cross domain access issue
                    var sheetRules = sheetItem.cssRules || sheetItem.rules;
                    for (var j = 0, rlen = sheetRules.length; j < rlen; j++) {
                        if (ctx.nospace(sheetRules.item(j).selectorText) === nospaceSel) {
                            effectList.push({
                                rule: sheetRules.item(j),
                                node: sheetItem.ownerNode || sheetItem.owningElement
                            });
                        }
                    }
                } catch (e) { }
            }
            var effectRule;
            if (effectList.length > 0) {
                effectRule = effectList[effectList.length - 1].rule; // use last one
            }
            if (effectRule) {
                var cssText = effectRule.style.cssText;
                var cache = this.changed[nospaceSel];
                if (!cache) {
                    this.changed[nospaceSel] = {
                        cssText: [cssText],
                        rules: [effectRule]
                    };
                } else {
                    var index = -1, clen = cache.rules.length;
                    for (var i = 0; i < clen; i++) {
                        if (cache.rules[i] === effectRule) {
                            index = i;
                        }
                    }
                    if (index === -1) {
                        cache.rules.push(effectRule);
                        cache.cssText.push(cssText);
                    }
                }
            } else {
                var extendSheet = this.getExtendSheet();
                var extendRules = extendSheet.cssRules || extendSheet.rules;
                var insertIndex = extendRules.length;
                try {// catch unvalidate selector issue
                    if (extendSheet.insertRule) {
                        extendSheet.insertRule(selector + '{}', insertIndex);
                    } else {
                        extendSheet.addRule(selector, '', insertIndex);
                    }
                } catch (ex) { }
                effectRule = extendRules[insertIndex];
            }
            if (effectRule) {
                try {// catch unvalidate property issue
                    if (set.name) {
                        effectRule.style[ctx.cameName(set.name)] = (set.value || '') + (set.important ? '!important' : '');
                    } else if (set.propertyText) {
                        effectRule.style.cssText = set.propertyText;
                    }
                } catch (ex) { }
            }
        },

        persistenceChanges: function () {
            this.changed = {};
            if (this.extendSheet) {// persistence current referenced but not all nodes that has "visualstyle" attribute.
                var node = this.extendSheet.ownerNode || this.extendSheet.owningElement;
                if (node) { node.setAttribute('visualstyle', 'persisted'); }
                this.extendSheet = null;
            }
        },

        resetChanges: function () {
            this.removeExtendSheet();
            $.each(this.changed, function () {
                var len = this.rules.length;
                for (var i = 0; i < len; i++) {
                    var cssText = this.cssText[i];
                    this.rules[i].style.cssText = cssText;
                }
            });
        },

        getExtendSheet: function () {
            if (this.extendSheet) {
                return this.extendSheet;
            } else {
                this.removeExtendSheet();
            }
            var win = this.win, doc = win.document;
            var head = doc.getElementsByTagName('head')[0] || doc.body;
            var node = doc.createElement('style');
            node.setAttribute('type', 'text/css');
            node.setAttribute('visualstyle', 'temporary');
            var cssText = '.test{}';
            if ($.browser.msie) {
                head.appendChild(node);
                try { node.styleSheet.cssText = cssText; }
                catch (ex) { }
            } else {
                try { node.appendChild(doc.createTextNode(cssText)); }
                catch (ex) { node.cssText = cssText; }
                head.appendChild(node);
            }
            // ret
            this.extendSheet = node.styleSheet ? node.styleSheet : (node.sheet || doc.styleSheets[doc.styleSheets.length - 1]);
            return this.extendSheet;
        },

        removeExtendSheet: function () {
            // remove referenced ownerNode
            if (this.extendSheet) {
                var ownerNode = this.extendSheet.ownerNode || this.extendSheet.owningElement;
                if (ownerNode) { ownerNode.parentNode.removeChild(ownerNode); }
                this.extendSheet = null;
            }
            // remove temporary nodes
            var nodes = this.win.document.getElementsByTagName('style'), node;
            for (var i = nodes.length - 1; i > -1; i--) {
                if ((node = nodes[i]) && node.getAttribute('visualstyle') === 'temporary') {
                    node.parentNode.removeChild(node);
                }
            }
            // clear empty sheet
            // when sheet's owningElement was removed, it was setted to empty.
            var sheets = this.win.document.styleSheets;
            for (var i = sheets.length - 1; i > -1; i--) {
                if (!sheets[i]) { sheets.splice(i, 1); }
            }
        },

        remove: function () {
            this.removeExtendSheet();
            this.resetChanges();
            this.changed = null;
        }
    };


    // register
    ctx.effectClass = effectClass;

} (jQuery, visualstyle));
