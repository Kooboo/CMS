/*
*   visualStyle
*   author: ronglin
*   create date: 2011.12.28
*   description: stylesheet file editor and front end online css editor.
*/

/*
* config parameters:
* renderTo
*/

/*
* customize events:
* visualStyleClass: onLoad, onError, onUnload, onSelect, onPreSelect, onRuleSort
* ruleClass: onPropertyChange, onPropertySort, onSelectorChange, onPreview, onPreviewEnd
*/

(function ($, ctx) {

    var backupRuleSelector = '.backup' + new Date().getTime();

    var visualStyleClass = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    visualStyleClass.prototype = {

        renderTo: null, loader: null, components: null, loaded: false,

        nativeSheet: null, nativeRules: null, nativeContent: '', nativeContentLast: '', ruleItems: null,

        onLoad: null, onUnload: null, onError: null, onSelect: null, onPreSelect: null, onRuleSort: null,

        constructor: visualStyleClass,

        initialize: function () {
            var self = this;
            this.components = {};
            this.renderTo.addClass('vs-visualstyle');
            this.renderTo.prepend(this.bulidHtml());
            // customize events
            this.onLoad = new yardi.dispatcher(this);
            this.onError = new yardi.dispatcher(this);
            this.onUnload = new yardi.dispatcher(this);
            this.onSelect = new yardi.dispatcher(this);
            this.onPreSelect = new yardi.dispatcher(this);
            this.onRuleSort = new yardi.dispatcher(this);
            // create loader
            this.loader = new sheetLoader();
            this.loader.onError.add(function () { self.onError.dispatch(); });
            this.loader.onLoad.add(function (nativeSheet, nativeContent) {
                // do cache
                self.nativeSheet = nativeSheet;
                self.nativeRules = nativeSheet.rules || nativeSheet.cssRules;
                var result = self.parseContent(nativeContent, self.nativeRules);
                self.ruleItems = result.ruleItems;
                self.nativeContent = nativeContent;
                self.nativeContentLast = nativeContent.substr(result.lastIndex);
                // fire onload
                self.onLoad.dispatch(nativeSheet, nativeContent);
                self.releaseLock();
                self.loaded = true;
            });
            // unload event
            $(window).unload(function () {
                self.onUnload.dispatch(self);
            });
            // onLoad hack
            var add = this.onLoad.add;
            this.onLoad.add = function (fn, scope) {
                add.apply(self.onLoad, arguments);
                if (self.isLoaded()) { fn.call(scope || self.onLoad.scope); }
            };
        },

        bulidHtml: function () {
            return '<div class="loading"></div>';
        },

        loadFile: function (src) {
            this.applyLock();
            this.loaded = false;
            this.loader.loadFile(src);
        },

        loadText: function (txt) {
            this.applyLock();
            this.loaded = false;
            this.loader.loadText(txt);
        },

        isLoaded: function () {
            return this.loaded;
        },

        isChanged: function () {
            if (!this.isLoaded()) { return; }
            var changed = false;
            $.each(this.ruleItems, function () {
                if (this.isChanged()) {
                    changed = true;
                    return false;
                }
            });
            return changed;
        },

        parseContent: function (nativeContent, nativeRules) {
            // hash nativeRules
            var hashedRules = {}, nativeRule, len = nativeRules.length;
            for (var i = 0; i < len; i++) {
                nativeRule = nativeRules.item(i);
                hashedRules[ctx.nospace(nativeRule.selectorText)] = nativeRule;
            }
            // split analysis
            var re = /([^{}]+?)\s*{([^}]*)}/ig, items = [], block = '';
            var match = re.exec(nativeContent), index = nativeContent.length;
            while (match) {
                block = match[0];
                items.push(new ruleClass({
                    hashedRules: hashedRules,
                    nativeSheet: this.nativeSheet,
                    originalContent: block,
                    originalSelectorText: match[1],
                    originalPropertiesText: match[2]
                }));
                index = match.index;
                match = re.exec(nativeContent);
            }
            // ret
            return {
                ruleItems: items,
                lastIndex: index + block.length
            }
        },

        fireSelect: function (set) { // set { index }
            set = set || {};
            set.rule = this.getRuleItem(set.index);
            if (set.rule) {
                set.event = {
                    success: true,
                    cancel: function ()
                    { success = false; }
                };
                this.onPreSelect.dispatch(this, set);
                if (set.event.success === true) {
                    this.onSelect.dispatch(this, set);
                }
            }
        },

        sortRuleItems: function (set) { // set { ruleItems,fireEvent }
            set = set || {};
            this.ruleItems = set.ruleItems;
            if (set.fireEvent !== false) { this.onRuleSort.dispatch(this, set); }
        },

        getRuleItems: function () {
            return this.ruleItems;
        },

        getRuleItem: function (index) {
            return this.ruleItems[index];
        },

        getValue: function () {
            var content = [];
            $.each(this.ruleItems, function () {
                content.push(this.toString());
            });
            content.push(this.nativeContentLast);
            return content.join('');
        },

        reset: function () {
            if (!this.isLoaded()) { return; }
            this.loadText(this.nativeContent);
        },

        applyLock: function () {
            this.renderTo.children('.loading').show();
        },

        releaseLock: function () {
            this.renderTo.children('.loading').hide();
        },

        getComponent: function (key) {
            return this.components[key];
        },

        addComponent: function (key, com) {
            com.host = this;
            com.initialize();
            this.components[key] = com;
            return com;
        },

        removeComponent: function (key) {
            var com = this.getComponent(key);
            if (com) {
                delete this.components[key];
                com.remove();
                return true;
            }
        },

        remove: function () {
            this.onUnload.dispatch(this);
            $.each(this.components, function () {
                this.remove();
            });
            this.components = null;
            this.loader.remove();
        }
    };


    var sheetLoader = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    sheetLoader.prototype = {

        iframe: null, onLoad: null, onError: null,

        constructor: sheetLoader,

        initialize: function () {
            // create dom
            var iframe = this.iframe = document.createElement('iframe');
            iframe.style.cssText = 'width:0px;height:0px;position:absolute;top:-99999px;';
            iframe.src = 'javascript:false;';
            document.body.appendChild(iframe);
            // customize events
            this.onLoad = new yardi.dispatcher(this);
            this.onError = new yardi.dispatcher(this);
            // dom events
            var self = this;
            $(iframe).load(function () {
                var doc = self.iframeDoc();
                var node = doc.getElementById('css');
                if (node) {
                    var nativeSheet = node.styleSheet || node.sheet;
                    var originalContent = decodeURIComponent(doc.body.innerHTML);
                    if (originalContent, nativeSheet) { self.onLoad.dispatch(nativeSheet, originalContent); }
                }
            }).error(function () {
                self.onError.dispatch();
            });
        },

        iframeDoc: function () {
            var win = this.iframe.contentWindow ||
            this.iframe.contentDocument.parentWindow;
            return win.document;
        },

        writeContent: function (content) {
            var doc = this.iframeDoc();
            if (doc) {
                doc.open();
                doc.write(content);
                doc.close();
            }
        },

        loadFile: function (src) {
            var self = this;
            $.ajax({
                url: src,
                type: 'GET',
                dataType: 'text',
                cache: false,
                success: function (content) { self.loadText(content); },
                error: function () { self.onError.dispatch(); },
                complete: function () { }
            });
            //setTimeout(function () {
            //    var html = [];
            //    html.push('<html><head><title></title>');
            //    html.push('<link id="css" href="' + src + '" rel="stylesheet" type="text/css" />');
            //    html.push('</head><body>' + src + '</body></html>');
            //    self.writeContent(html.join(''));
            //}, 0);
        },

        loadText: function (txt) {
            var self = this, css = txt + '\n\n' + backupRuleSelector + '{}'; // append empty backup rule
            setTimeout(function () {
                var html = [];
                html.push('<html><head><title></title>');
                html.push('<style id="css" type="text/css">' + css + '</style>');
                html.push('</head><body>' + encodeURIComponent(txt) + '</body></html>');
                self.writeContent(html.join(''));
            }, 0);
        },

        remove: function () {
            document.body.removeChild(this.iframe);
        }
    };


    var ruleClass = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    ruleClass.prototype = {

        hashedRules: null, originalContent: '', originalSelectorText: '', originalPropertiesText: '',

        selectorText: '', selectorTextCache: '', nativeRule: null, nativeSheet: null, properties: null, success: false,

        onPropertySort: null, onPropertyChange: null, onSelectorChange: null, onPreview: null, onPreviewEnd: null,

        constructor: ruleClass,

        initialize: function () {
            // split selectorText
            var txt = ctx.noComment(this.originalSelectorText).trim();
            var len = txt.length, idx = -1;
            for (var i = len - 1; i > -1; i--) {
                var chr = txt.charAt(i);
                // for deal @import url('s.css');
                if (chr === ';' || chr === ')' || chr === '(') {
                    idx = i;
                    break;
                }
            }
            if (idx !== -1) {
                var j = idx + 1;
                for (; j < len; j++) {
                    if (!(/\s/).test(txt.charAt(j))) {
                        break;
                    }
                }
                this.selectorText = txt.substr(j);
            } else {
                this.selectorText = txt;
            }
            this.selectorTextCache = this.selectorText;
            // create properties objects
            this.setPropertiesText({ text: this.originalPropertiesText, fireEvent: false });
            // get nativeRule
            this.nativeRule = this.hashedRules[ctx.nospace(this.selectorText)];
            if (!this.nativeRule) { this.nativeRule = this.hashedRules[backupRuleSelector]; }
            // is success
            if (this.selectorText && this.nativeRule) { this.success = true; }
            // customize events
            this.onPreview = new yardi.dispatcher(this);
            this.onPreviewEnd = new yardi.dispatcher(this);
            this.onPropertySort = new yardi.dispatcher(this);
            this.onPropertyChange = new yardi.dispatcher(this);
            this.onSelectorChange = new yardi.dispatcher(this);
        },

        isSuccess: function () {
            return this.success;
        },

        isChanged: function () {
            var changed = (this.selectorText !== this.selectorTextCache);
            if (!changed) {
                $.each(this.properties, function (idx) {
                    if (this.isChanged() || this.initIndex !== idx) {
                        changed = true;
                        return false;
                    }
                });
                if (!changed) {
                    var textEqual = ctx.nospace(this.getPropertiesText(true)) === ctx.nospace(ctx.noComment(this.originalPropertiesText));
                    $.each(this.properties, function (idx) {
                        if (this.initIndex !== idx) {
                            changed = true;
                            return false;
                        }
                    });
                    if (changed) {
                        if (textEqual) { changed = false; }
                    } else {
                        changed = !textEqual;
                    }
                }
            }
            return changed;
        },

        getProperties: function () {
            return this.properties;
        },

        sortProperties: function (set) { // set { properties,fireEvent }
            set = set || {};
            this.properties = set.properties;
            if (set.fireEvent !== false) { this.onPropertySort.dispatch(this, set); }
        },

        getSelectorText: function () {
            return this.selectorText;
        },

        setSelectorText: function (set) { // set { selector,fireEvent }
            set = set || {};
            if (this.selectorText !== set.selector) {
                this.selectorText = set.selector;
                if (set.fireEvent !== false) { this.onSelectorChange.dispatch(this, set); }
            }
        },

        newProperty: function (set) { // set { name,value,enable,important }
            set = set || {};
            set.host = this;
            var prop = new propertyClass(set);
            prop.initIndex = this.properties.length;
            this.properties.push(prop);
            return prop;
        },

        removeProperty: function (set) { // set { property,fireEvent }
            set = set || {};
            var self = this, obj = set.property;
            $.each(this.properties, function (index) {
                if (this === obj) {
                    self.properties.splice(index, 1);
                    if (set.fireEvent !== false) { self.onPropertyChange.dispatch(self, set); }
                    return false;
                }
            });
        },

        setProperty: function (set) { // set { refProp,name,value,enable,important,fireEvent }
            set = set || {};
            var foundItem, changed = false;
            if (set.refProp) {
                $.each(this.properties, function () {
                    if (this === set.refProp) { foundItem = this; }
                });
            } else {
                var nospaceName = ctx.nospace(set.name);
                $.each(this.properties, function () {
                    if (this.nospaceName === nospaceName) { foundItem = this; }
                });
            }
            if (set.value) {
                if (!foundItem) { foundItem = this.newProperty(); }
                foundItem.setName({ name: set.name, fireEvent: false });
                foundItem.setValue({ value: set.value, fireEvent: false });
                foundItem.setEnable({ enable: set.enable, fireEvent: false });
                foundItem.setImportant({ important: set.important, fireEvent: false });
                changed = true;
            } else if (foundItem) {
                foundItem.remove();
                foundItem = null;
                changed = true;
            }
            if (set.fireEvent !== false && changed) {
                this.onPropertyChange.dispatch(this, set);
            }
            // ret
            return foundItem;
        },

        setPropertiesText: function (set) { // set { text,fireEvent }
            set = set || {};
            this.properties = [];
            var self = this, parts = (set.text + ';').split(';');
            var forTest = ctx.noComment(ctx.nospace(set.text));
            var pair, name, value, enable, exist;
            $.each(parts, function () {
                pair = this.split(':');
                if (pair.length === 2) {
                    value = pair[1].trim();
                    name = ctx.noComment(pair[0]).replace('*/', '').trim();
                    enable = (name.indexOf('/*') !== 0);
                    name = name.replace('/*', '').trim();
                    exist = (forTest.indexOf(name.toLowerCase() + ':' + value.toLowerCase()) !== -1);
                    self.newProperty({ name: name, value: value, enable: enable && exist });
                }
            });
            if (set.fireEvent !== false) { this.onPropertyChange.dispatch(this, set); }
        },

        getPropertiesText: function (compress) {
            var ret = [], txt;
            $.each(this.properties, function () {
                if (!this.isEmpty()) {
                    txt = this.toString(compress);
                    if (txt) { ret.push(txt); }
                }
            });
            return ret.join(compress ? '' : '\n');
        },

        testPropertiesText: function (text) {
            if (!this.nativeRule) { return false; }
            var style = this.nativeRule.style;
            var cache = style.cssText;
            style.cssText = text;
            var success = (style.cssText === text)
            style.cssText = cache;
            return success;
        },

        testPropertyName: function (name) {
            if (!name) { return false; }
            if (this.isPrefixIgnore(name)) { return true; }
            if (name.toLowerCase() in ctx.propertySet) { return true; }
            try {
                return (ctx.cameName(name.toLowerCase()) in this.nativeRule.style);
            } catch (ex) {
                return false;
            }
        },

        testPropertyValue: function (name, value) {
            if (!value) { return false; }
            if (!this.testPropertyName(name)) { return false; }
            value = value.replace(/(!|important)/ig, '').trim();
            var dictionary = ctx.propertySet[name.toLowerCase()];
            if (dictionary && value in dictionary) { return true; }
            try {
                name = ctx.cameName(name.toLowerCase());
                var style = this.nativeRule.style, cache = style[name];
                //if (!(name in style)) { return false; } // comment out this line because testPropertyName had done
                style[name] = '';
                style[name] = value;
                var ret = (style[name] !== '');
                style[name] = cache;
                return ret;
            } catch (ex) {
                return false;
            }
        },

        testSelectorText: function (selectorText) {
            if (!selectorText) { return false; }
            try {
                var sheet = this.nativeSheet, rules = sheet.cssRules || sheet.rules, index = rules.length;
                // create
                if (sheet.insertRule) {
                    sheet.insertRule(selectorText + '{ }', index);
                } else {
                    sheet.addRule(selectorText, ' ', index);
                }
                // judge
                var ret = !!rules.item(index);
                // remove
                if (ret) {
                    if (sheet.removeRule) {
                        sheet.removeRule(index);
                    } else {
                        sheet.deleteRule(index);
                    }
                }
                // ret
                return ret;
            } catch (ex) {
                return false;
            }
        },

        isPrefixIgnore: function (text) {
            var lower = text.toLowerCase(), ignore = false;
            $.each(ctx.ignorePrefixs, function () {
                if (lower.indexOf(this) === 0) {
                    ignore = true;
                    return false;
                }
            });
            return ignore;
        },

        toString: function (compress) {
            var lastIndex = this.originalSelectorText.lastIndexOf(this.selectorTextCache), length = this.selectorTextCache.length;
            var header = this.originalSelectorText.substring(0, lastIndex) + this.selectorText + this.originalSelectorText.substr(lastIndex + length);
            //var header = this.originalSelectorText.replace(this.selectorTextCache, this.selectorText);
            return header + (compress ? '{' : '{\n') + this.getPropertiesText(compress) + (compress ? '}' : '\n}');
        },

        clearProperties: function () {
            $.each(this.propertyItems, function () {
                this.remove({ removeRef: false });
            });
            this.propertyItems = [];
        },

        remove: function () {
            this.clearProperties();
            this.hashedRules = undefined;
            this.nativeRule = undefined;
            this.nativeSheet = undefined;
        }
    };


    var propertyClass = function (config) {
        //$.extend(this, config);
        this.initialize(config);
    };

    propertyClass.prototype = {

        host: null, name: '', value: '', important: false, enable: true, nospaceName: '', nospaceValue: '', importantStr: '',

        nospaceNameOriginal: undefined, nospaceValueOriginal: undefined, importantOriginal: undefined, enableOriginal: undefined,

        constructor: propertyClass,

        initialize: function (config) {
            this.host = config.host;
            this.setName({ name: config.name, fireEvent: false });
            this.setValue({ value: config.value, fireEvent: false });
            this.setEnable({ enable: config.enable, fireEvent: false });
            // override the value setValue function generated if config specified a boolean value
            if ($.type(config.important) === 'boolean') {
                this.importantOriginal = undefined;
                this.setImportant({ important: config.important, fireEvent: false });
            }
        },

        getName: function () {
            return this.name.trim();
        },

        getValue: function () {
            return this.value.trim() + (this.important ? ' !' + (this.importantStr || 'important') : '');
        },

        isEnable: function () {
            return this.enable;
        },

        isImportant: function () {
            return this.important;
        },

        isChanged: function () {
            return (this.nospaceValue !== this.nospaceValueOriginal) ||
                   (this.nospaceName !== this.nospaceNameOriginal) ||
                   (this.important !== this.importantOriginal) ||
                   (this.enable !== this.enableOriginal);
        },

        isEmpty: function () {
            return !(this.name && this.value);
        },

        setName: function (set) { // set { name,fireEvent }
            set = set || {};
            set.name = set.name || '';
            this.name = set.name.trim();
            this.nospaceName = ctx.nospace(set.name);
            if (this.nospaceNameOriginal === undefined) { this.nospaceNameOriginal = this.nospaceName; }
            if (set.fireEvent !== false) { this.host.onPropertyChange.dispatch(this, set); }
        },

        setValue: function (set) { // set { value,fireEvent }
            set = set || {};
            set.value = set.value || '';
            var parts = set.value.split('!');
            this.value = parts[0].trim();
            this.importantStr = parts[1] || '';
            this.nospaceValue = ctx.nospace(this.value);
            this.setImportant({ important: ctx.nospace(this.importantStr) === 'important', fireEvent: false });
            if (this.nospaceValueOriginal === undefined) { this.nospaceValueOriginal = this.nospaceValue; }
            if (set.fireEvent !== false) { this.host.onPropertyChange.dispatch(this, set); }
        },

        setEnable: function (set) { // set { enable,fireEvent }
            set = set || {};
            this.enable = (set.enable !== false);
            if (this.enableOriginal === undefined) { this.enableOriginal = this.isEnable(); }
            if (set.fireEvent !== false) { this.host.onPropertyChange.dispatch(this, set); }
        },

        setImportant: function (set) { // set { important,fireEvent }
            set = set || {};
            this.important = (set.important === true);
            if (this.importantOriginal === undefined) { this.importantOriginal = this.isImportant(); }
            if (set.fireEvent !== false) { this.host.onPropertyChange.dispatch(this, set); }
        },

        toString: function (compress) {
            if (compress && !this.isEnable()) { return ''; }
            var ret = this.getName() + (compress ? ':' : ': ') + this.getValue() + ';';
            if (!this.isEnable()) { ret = '/*' + ret + '*/'; }
            return (compress ? '' : '    ') + ret;
        },

        remove: function (set) { // set { }
            set = set || {};
            if (set.removeRef !== false) {
                set.property = this;
                this.host.removeProperty(set);
            }
            this.host = null;
        }
    };


    // register
    ctx.visualStyleClass = visualStyleClass;

} (jQuery, visualstyle));
