/*
*   selectors
*   author: ronglin
*   create date: 2011.12.28
*/

/*
* config parameters:
* renderTo, host
*/

(function ($, ctx) {

    // localization
    var txtRes = {
        title: 'CSS Rules',
        searchTitle: 'Search',
        searchTitleFormat: 'Search ({0})items',
        searchInputTip: 'Case insensitive and use whitespace to separate multiple keys.',
        btnSearch: 'Go',
        btnClear: 'X',
        btnSearchTitle: 'Search',
        btnClearTitle: 'Clear',
        changeTitle: 'changed',
        noReadyMsg: 'Initialization is processing, please wait.'
    };
    // localize text resource
    if (window.__localization) { $.extend(txtRes, __localization.selectors_js); }

    // helpers
    var markSelf = function (obj) {
        obj['selectors_js'] = true; return obj;
    }, isSelf = function (obj) {
        return (obj['selectors_js'] === true);
    };

    /*
    * selectorsClass
    */
    var selectorsClass = function (config) {
        $.extend(this, config);
        //this.initialize();
    };

    selectorsClass.prototype = {

        renderTo: null, host: null, selectorTexts: null, currentRules: null,

        el: null, searchTxt: null, searchBtn: null, searchClear: null, searchLabel: null, contentCon: null,

        items: null, createLazy: null, ready: false, _onload: null, _onselect: null, _onrulechange: null,

        constructor: selectorsClass,

        initialize: function () {
            var self = this;
            this.items = [];
            this.el = $(this.buildHtml()).appendTo(this.renderTo);
            this.searchTxt = this.el.find('input.txt');
            this.searchBtn = this.el.find('input.go');
            this.searchClear = this.el.find('input.clear');
            this.searchLabel = this.el.find('.search label');
            this.contentCon = this.el.find('.content');
            if (this.selectorTexts) { this.updateSelectorTexts(this.selectorTexts); }
            // dom events
            this.searchTxt.focus(function () {
                $(this).siblings('label').hide();
            }).blur(function () {
                var has = ($(this).val() + '').length > 0;
                $(this).siblings('label')[has ? 'hide' : 'show']();
            }).keydown(function (ev) {
                if (ev.keyCode === 13) {// ENTER key
                    ev.preventDefault();
                    self.doSearch();
                } else if (ev.keyCode === 27) {// ESC key
                    ev.preventDefault();
                    self.clearSearch();
                    self.searchTxt.triggerHandler('focus');
                }
            }).keyup(function () {
                var has = ($(this).val() + '').length > 0;
                self.searchClear.css('visibility', has ? 'visible' : 'hidden');
            }).triggerHandler('blur');
            this.searchBtn.click(function () { self.doSearch(); });
            this.searchClear.click(function () {
                self.clearSearch();
                self.searchTxt.triggerHandler('keyup');
            });
            this.contentCon.click(function (ev) { self.doSelect({ index: self.eventItemIndex(ev) }); })
            this.contentCon.bind('mouseover mouseout', function (ev) { self.doNotice(ev); });
            // subscribe events
            this._onrulechange = function (sender, set) {
                if (isSelf(set)) { return; }
                var oIndex = -1, rule = this;
                $.each(self.currentRules, function (index) {//TODO: find a better implement
                    if (this === rule) {
                        oIndex = index;
                        return false;
                    }
                });
                if (oIndex > -1) {
                    var item = self.items[oIndex];
                    item.setChanged(rule.isChanged());
                    if (set.selector) { item.setText(set.selector); } // onSelectorChange
                }
            };
            this.host.onLoad.add(this._onload = function () {
                self.searchTxt.val('');
                self.searchClear.css('visibility', 'hidden');
                self.contentCon.scrollTop(0);
                self.updateRules(self.host.getRuleItems());
            });
            this.host.onSelect.add(this._onselect = function (sender, set) {
                if (isSelf(set)) { return; }
                set.fireEvent = false;
                self.doSelect(set);
            });
        },

        buildHtml: function () {
            var html = [];
            html.push('<div class="vs-selectors">');
            html.push('<h3>' + txtRes.title + '</h3>');
            html.push('<div class="search">');
            html.push('<label for="selSehTxt">' + txtRes.searchTitle + '</label>');
            html.push('<input type="text" class="txt" id="selSehTxt" title="' + txtRes.searchInputTip + '" />');
            html.push('<input type="button" class="clear" title="' + txtRes.btnClearTitle + '" value="' + txtRes.btnClear + '" />');
            html.push('<input type="button" class="go" title="' + txtRes.btnSearchTitle + '" value="' + txtRes.btnSearch + '" />');
            html.push('</div>');
            html.push('<div class="content">');
            html.push('</div>');
            html.push('</div>');
            return html.join('');
        },

        updateRules: function (rules, callback) {
            this.removeRulesSubscribe();
            this.currentRules = rules;
            var sels = [], self = this;
            $.each(rules, function () {
                sels.push(this.getSelectorText());
                this.onPropertySort.add(self._onrulechange);
                this.onPropertyChange.add(self._onrulechange);
                this.onSelectorChange.add(self._onrulechange);
            });
            this.updateSelectorTexts(sels, callback);
            $.each(rules, function (index) {
                //TODO:
                //if (!this.isSuccess()) { self.setError(index, true); }
            });
        },

        updateSelectorTexts: function (selectorTexts, callback) {
            // clear
            this.ready = false;
            $.each(this.items, function () { this.remove(); });
            this.items = [];
            // count
            var newTitle = yardi.format(txtRes.searchTitleFormat, selectorTexts.length);
            this.searchLabel.html(newTitle);
            // create
            var self = this, to = this.contentCon, index = 0, create = function (text) {
                var item = new selectorItem({
                    renderTo: to,
                    index: index++,
                    text: text
                });
                self.items.push(item);
            };
            // execute
            var lazy = this.createLazy;
            if (selectorTexts.length > 500) {
                if (!lazy) {
                    lazy = new yardi.lazyFunc({ granularity: 250 });
                    lazy.onComplete.add(function () {
                        if (callback) { callback(); }
                        self.ready = true;
                    });
                    this.createLazy = lazy;
                }
                $.each(selectorTexts, function () {
                    lazy.add(function (text) {
                        return function () { create(text); };
                    } (this));
                });
                lazy.apply();
            } else {
                $.each(selectorTexts, function () { create(this); });
                if (callback) { callback(); }
                this.ready = true;
            }
        },

        doSearch: function (key) {
            // valid state
            if (this.ready !== true) {
                alert(txtRes.noReadyMsg);
                return;
            }
            // do search
            var matched = false;
            if (key === undefined) { key = this.searchTxt.val(); }
            for (var i = this.items.length - 1; i > -1; i--) {// loop reverse
                matched = this.items[i].searchKey(key) || matched;
            }
            if (!matched) {
                //TODO: add more spliters support, eg:'|'
                var keys = key.split(' ');
                if (keys.length > 1) {
                    for (var j = this.items.length - 1; j > -1; j--) {// loop reverse
                        matched = this.items[j].searchKeys(keys) || matched;
                    }
                }
            }
            // do order
            var arr = [], arrHit = [], o;
            $.each(this.items, function (index, item) {
                o = { index: index, score: item.getScore() };
                (o.score > 0) ? arrHit.push(o) : arr.push(o);
            });
            arrHit.sort(function (a, b) {
                return (b.score - a.score);
            });
            arr = arrHit.concat(arr);
            for (var len = arr.length, k = 0; k < len; k++) {
                this.items[arr[k].index].doHit();
            }
            // scroll to view
            if (matched) {
                this.contentCon.scrollTop(0);
            }
        },

        clearSearch: function () {
            this.searchTxt.val('').triggerHandler('blur');
            this.searchBtn.triggerHandler('click');
        },

        doSelect: function (set) {
            set = set || {};
            var item = this.items[set.index];
            if (item) {
                if (currentSelect) { currentSelect.setSelect(false); }
                currentSelect = item;
                item.setSelect(true);
                // bubble event
                if (set.fireEvent !== true) {
                    this.host.fireSelect(markSelf({ index: item.getIndex() }));
                }
            }
        },

        doNotice: function (index) {
            if ($.type(index) !== 'number') { index = this.eventItemIndex(index); }
            if (currentNotice) { currentNotice.setNotice(false); }
            var item = this.items[index];
            if (item) {
                currentNotice = item;
                item.setNotice(true);
            }
        },

        eventItemIndex: function (ev) {
            var target = ev.target, item, tag;
            var con = this.contentCon.get(0);
            while (target) {
                if (target.tagName === 'BODY') { break; }
                if (target.parentNode === con) { item = target; break; }
                target = target.parentNode;
            }
            if (!item) { return -1; }
            return parseInt(item.getAttribute('index'));
        },

        removeRulesSubscribe: function () {
            if (this.currentRules) {
                var self = this;
                $.each(this.currentRules, function () {
                    this.onPropertySort.remove(self._onrulechange);
                    this.onPropertyChange.remove(self._onrulechange);
                    this.onSelectorChange.remove(self._onrulechange);
                });
            }
        },

        remove: function () {
            this.removeRulesSubscribe();
            this.host.onLoad.remove(this._onload);
            this.host.onSelect.remove(this._onselect);
            $.each(this.items, function () { this.remove(); });
            this.el.remove();
            this.host = null;
        }
    };

    var currentNotice, currentSelect;
    var selectorItem = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    selectorItem.prototype = {

        renderTo: null, text: null, index: null,

        textAt: null, key: null, matchedInfo: [],

        el: null, textSpan: null, stateSpan: null,

        constructor: selectorItem,

        initialize: function () {
            this.el = $(this.buildHtml()).appendTo(this.renderTo);
            this.stateSpan = this.el.find('.changed');
            this.textSpan = this.el.find('.text');
            this.setText(this.text + '');
        },

        buildHtml: function () {
            var html = [];
            html.push('<div class="item" index="' + this.index + '">');
            html.push('<span class="changed" title="' + txtRes.changeTitle + '"></span>');
            html.push('<span class="text"></span>');
            html.push('</div>');
            return html.join('');
        },

        getIndex: function () {
            return this.index;
        },

        getText: function () {
            return this.text;
        },

        setText: function (val) {
            this.text = val;
            this.key = ctx.nospace(val);
            this.textAt = ctx.nospace(val, '/');
            this.textSpan.html(val);
        },

        setSelect: function (select) {
            if (select) {
                this.el.addClass('select');
            } else {
                this.el.removeClass('select');
            }
        },

        setNotice: function (notice) {
            if (notice) {
                this.el.addClass('notice');
            } else {
                this.el.removeClass('notice');
            }
        },

        setChanged: function (changed) {
            if (changed) {
                this.stateSpan.css({ display: 'inline-block' });
            } else {
                this.stateSpan.css({ display: 'none' });
            }
        },

        searchKey: function (key) {
            this.matchedInfo = [];
            if (!key) {
                this.textSpan.html(this.text);
                this.matchedInfo.push({ key: key, count: 0 });
                return false;
            }
            var nsKey = ctx.nospace(key);
            if (this.key.indexOf(nsKey) === -1) {
                this.textSpan.html(this.text);
                this.matchedInfo.push({ key: key, count: 0 });
                return false;
            }
            // in order match
            var parts = this.matchKey(key, nsKey);
            // emphasizes text
            this.emphasizesText(parts);
            // ret
            this.matchedInfo.push({ key: key, count: parts.length });
            return (parts.length > 0);
        },

        searchKeys: function (keys) {
            // all wide match
            this.matchedInfo = [];
            var parts = [], len = keys.length, o;
            for (var i = 0; i < len; i++) {
                o = this.matchKey(keys[i], ctx.nospace(keys[i]));
                this.matchedInfo.push({ key: keys[i], count: o.length });
                parts = parts.concat(o);
            }
            // emphasizes text
            this.emphasizesText(parts);
            // ret
            return (parts.length > 0);
        },

        matchKey: function (key, nsKey) {
            var lastIndex = nsKey.length - 1, parts = [];
            var at = this.textAt, len = at.length, chr;
            var loop = function (startIndex) {
                var i = startIndex, keyIndex = 0, start = null, end = null;
                for (; i < len; i++) {
                    chr = at.charAt(i);
                    if (chr !== '/') {
                        if (chr === nsKey.charAt(keyIndex)) {
                            if (keyIndex === 0) { start = i; }
                            if (keyIndex === lastIndex) { end = i; break; }
                            keyIndex++;
                        } else {
                            end = null;
                            start = null;
                            keyIndex = 0;
                        }
                    }
                }
                if (start !== null && end !== null) {
                    parts.push({ start: start, end: end });
                }
                if (i < len) {
                    loop(i + 1);
                }
            };
            loop(0);
            return parts;
        },

        emphasizesText: function (parts) {
            // merge step1, generate no repeat list
            var i, j, len = parts.length, start, end, hash = {};
            for (i = 0; i < len; i++) {
                start = parts[i].start; end = parts[i].end;
                for (j = start; j <= end; j++) {
                    hash[j] = true;
                }
            }
            var arr = [], key;
            for (key in hash) { arr.push(parseInt(key, 10)); }
            arr.sort(function (a, b) { return (a - b); });
            // merge step2, generate new marged parts
            var merged = [], curr; start = null; end = null; len = arr.length;
            for (i = 0; i < len; i++) {
                curr = arr[i];
                if (start === null) { start = curr; }
                if (end === null || end < curr) { end = curr; }
                if (!hash[curr + 1]) {
                    merged.push({ start: start, end: end });
                    start = null; end = null;
                }
            }
            // emphasizes text
            var txt = this.text; len = merged.length;
            for (i = len - 1; i > -1; i--) {
                start = merged[i].start; end = merged[i].end;
                txt = txt.substring(0, end + 1) + '</em>' + txt.substr(end + 1);
                txt = txt.substring(0, start) + '<em>' + txt.substr(start);
            }
            // apply dom
            this.textSpan.html(txt);
        },

        getScore: function () {
            var score = 0, len = this.matchedInfo.length;
            $.each(this.matchedInfo, function (index, item) {
                if (item.count > 0) {
                    score += 10000; // assume that item.count less than 10000 always
                    score += item.count;
                }
            });
            return score;
        },

        doHit: function () {
            this.renderTo.append(this.el);
        },

        remove: function () {
            this.el.remove();
        }
    };

    // register
    ctx.selectorsClass = selectorsClass;

} (jQuery, visualstyle));
