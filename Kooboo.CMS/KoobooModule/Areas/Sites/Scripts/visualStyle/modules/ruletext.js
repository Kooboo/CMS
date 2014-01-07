/*
*   ruletext
*   author: ronglin
*   create date: 2011.12.28
*/

/*
* config parameters:
* renderTo, host, textMode
*/

(function ($, ctx) {

    // localization
    var txtRes = {
        title: 'Rule text',
        moveTitle: 'move up/down',
        btnSwapTitle: 'edit all',
        btnSwapBackTitle: 'save',
        statusWarningTitle: 'warning',
        statusEnableTitle: 'enable',
        statusDisabledTitle: 'disabled',
        titleExpended: 'expended',
        titleCollapsed: 'collapsed',
        defaultMessage: 'Select a css rule to edit here.'
    };
    // localize text resource
    if (window.__localization) { $.extend(txtRes, __localization.ruletext_js); }

    // helpers
    var moving = false;
    var markSelf = function (obj) {
        obj['ruletext_js'] = true; return obj;
    }, isSelf = function (obj) {
        return (obj['ruletext_js'] === true);
    };

    /*
    * ruletextClass
    */
    var ruletextClass = function (config) {
        $.extend(this, config);
        //this.initialize();
    };

    ruletextClass.prototype = {

        renderTo: null, host: null, textMode: false, el: null, contentCon: null, propertyCon: null, swapBtn: null,

        currentRule: null, propertyItems: null, editorObj: null, selectorObj: null, textareaObj: null, moveDragObj: null,

        _docclick: null, _onload: null, _onselect: null, _onpropertysort: null, _onpropertychange: null, _onselectorchange: null,

        constructor: ruletextClass,

        initialize: function () {
            var self = this;
            this.propertyItems = [];
            this.el = $(this.bulidHtml()).appendTo(this.renderTo);
            this.contentCon = this.el.find('.content');
            this.propertyCon = this.contentCon.find('.property');
            this.swapBtn = this.contentCon.find('.swap').addClass(this.textMode ? 'swapback' : '');
            this.textareaObj = new textareaClass({ host: this, el: this.contentCon.find('.textarea'), scroll: this.contentCon });
            this.selectorObj = new selectorClass({ host: this, el: this.contentCon.find('.selector'), toggle: this.contentCon.find('.toggle') });
            this.initializeMove(this.propertyCon);
            // dom events
            $(document).click(this._docclick = function (ev) {
                self.applyEdit($(ev.target));
            });
            this.swapBtn.click(function () {
                self._docclick({ target: this }); // ensure fire docuemnt click event before swapBtn click event
                self.toggleShowMode();
                return false; // document click event has been fired manually, so cancel bubble current event.
            });
            this.contentCon.dblclick(function (ev) {
                if (self.editorObj && self.editorObj.contains(ev.target)) { return; }
                if (yardi.isAncestor(self.contentCon.children('.head').get(0), ev.target)) { return; }
                if (ev.target.tagName === 'SPAN' && $(ev.target.parentNode).hasClass('item')) { return; }
                if (self.isTextShowMode()) { return; }
                if (self.currentRule) {
                    var prop = self.newProperty();
                    self.applyEdit(prop.getNameEl());
                }
            });
            // subscribe host events
            this._onpropertysort = function (sender, set) {
                if (isSelf(set)) { return; }
                self.updateRule(self.currentRule);
            };
            this._onpropertychange = function (sender, set) {
                if (isSelf(set)) { return; }
                self.updateRule(self.currentRule);
            };
            this._onselectorchange = function (sender, set) {
                if (isSelf(set)) { return; }
                self.selectorObj.setName(set.selector);
            };
            this.host.onLoad.add(this._onload = function () { self.clean(); });
            this.host.onSelect.add(this._onselect = function (sender, set) {
                if (isSelf(set)) { return; }
                self.removeRuleSubscribe();
                self.updateRule(set.rule, true);
                self.currentRule.onPropertySort.add(self._onpropertysort);
                self.currentRule.onPropertyChange.add(self._onpropertychange);
                self.currentRule.onSelectorChange.add(self._onselectorchange);
            });
        },

        initializeMove: function (con) {
            // move initialiation
            var self = this, current, event, pos, css, holder, positions, refreshPositions = function () {
                positions = [];
                con.children().each(function () {
                    if (this != current.get(0) && this != holder.get(0)) {
                        var top = $(this).position().top, height = $(this).height();
                        positions.push({ baseline: top + height / 2, dom: this });
                    }
                });
            };
            this.moveDragObj = new yardi.dragClass(con, {
                valid: function (ev) {
                    if (moving) { return false; }
                    if (self.editorObj && self.editorObj.isEditing()) { return false; }
                    if ($(ev.target).hasClass('move')) {
                        current = $(ev.target).parent();
                        return true;
                    } else if ($(ev.target).hasClass('item')) {
                        current = $(ev.target);
                        return true;
                    } else { return false; }
                },
                start: function (ev) {
                    // cache
                    event = ev;
                    moving = true;
                    pos = current.position();
                    css = { width: current.width(), height: current.height(), opacity: 0.7, position: 'absolute' };
                    // helper
                    current.addClass('helper').css($.extend(css, pos));
                    // holder
                    holder = $('<div class="item holder"></div>');
                    holder.height(css.height).insertAfter(current);
                    refreshPositions();
                },
                end: function (ev) {
                    positions = [];
                    current.insertBefore(holder);
                    self.sortProperties();
                    current.animate({
                        opacity: 1,
                        top: holder.position().top
                    }, {
                        duration: 256,
                        complete: function () {
                            setTimeout(function () {
                                moving = false;
                                current.removeClass('helper').removeAttr('style');
                                current.triggerHandler('hoverOut');
                                holder.remove();
                                holder = undefined;
                            }, 256);
                        }
                    });
                },
                move: function (ev) {
                    // position
                    var offset = event.pageY - ev.pageY;
                    var upwards = (offset < 0);
                    pos.top -= offset;
                    current.css(pos);
                    event = ev;
                    // move holder
                    var dom, baseline = pos.top + css.height / 2;
                    $.each(positions, function () {
                        if (Math.abs(this.baseline - baseline) < 5) {
                            dom = this.dom;
                            return false;
                        }
                    });
                    if (dom) {
                        if (upwards) {
                            holder.insertAfter(dom);
                        } else {
                            holder.insertBefore(dom);
                        }
                        refreshPositions();
                    }
                }
            });
        },

        initializeEditor: function () {
            if (this.editorObj) { return; }
            var self = this, getEditItem = function (t, next) {
                var tIndex = -1, dom = t.get(0);
                var all = self.el.find('[editable="true"]');
                all.each(function (index) {
                    if (this === dom) { tIndex = index; }
                });
                if (tIndex !== -1) {
                    if (next === true) {
                        if (self.isTextShowMode()) {
                            return self.textareaObj.setFocus();
                        } else if (tIndex + 1 < all.length) {
                            return all.eq(tIndex + 1);
                        } else {
                            return self.newProperty().getNameEl();
                        }
                    } else {
                        if (tIndex - 1 > -1) {
                            return all.eq(tIndex - 1);
                        } else {
                            return t;
                        }
                    }
                }
            };
            var checkEmpty = function (target, newTarget) {
                var prop = target.parent().data('propIns');
                if (prop && (!newTarget || newTarget.parent().get(0) != target.parent().get(0))) {
                    prop.validateEmpty({ remove: true });
                }
            };
            this.editorObj = new editorClass({ renderTo: this.contentCon });
            this.editorObj.onPrev.add(function (target) {
                var t = getEditItem(target, false);
                if (t) { self.applyEdit(t); }
                checkEmpty(target, t);
            });
            this.editorObj.onNext.add(function (target) {
                var t = getEditItem(target, true);
                if (t) { self.applyEdit(t); }
                checkEmpty(target, t);
            });
            this.editorObj.onComplete.add(function (target, newVal, oldVal, isSave) {
                var prop = target.parent().data('propIns');
                if (prop) {
                    if (!isSave) {
                        checkEmpty(target, null);
                        // bubble event
                        self.currentRule.onPreviewEnd.dispatch(self.editorObj, markSelf({
                            name: target.hasClass('name') ? oldVal : prop.getName(),
                            value: target.hasClass('name') ? prop.getValue() : oldVal
                        }));
                    }
                } else {
                    if (!newVal) { target.data('seleIns').setName(oldVal); }
                }
            });
            this.editorObj.onPreview.add(function (target, currVal) {
                var warning = undefined;
                var prop = target.parent().data('propIns');
                if (prop) {
                    if (currVal.length > 0) {
                        var name = prop.getName(), value = prop.getValue();
                        if (target.hasClass('name')) {
                            if (value) {
                                warning = !self.currentRule.testPropertyValue(currVal, value);
                            } else {
                                warning = !self.currentRule.testPropertyName(currVal);
                            }
                        } else {
                            warning = !self.currentRule.testPropertyValue(name, currVal);
                        }
                    }
                } else {
                    warning = !self.currentRule.testSelectorText(currVal);
                }
                self.editorObj.setWarning(warning);
                // bubble event
                if (prop) {
                    self.currentRule.onPreview.dispatch(self.editorObj, markSelf({
                        valid: !!(!warning && currVal),
                        name: target.hasClass('name') ? currVal : prop.getName(),
                        value: target.hasClass('name') ? prop.getValue() : currVal,
                        important: prop.isImportant()
                    }));
                }
            });
        },

        bulidHtml: function () {
            var html = [];
            html.push('<div class="vs-ruletext">');
            html.push('<h3>' + txtRes.title + '</h3>');
            html.push('<div class="content">');
            html.push('<div class="message">' + txtRes.defaultMessage + '</div>');
            html.push('<div class="head">');
            html.push('<span class="toggle" title="' + txtRes.titleCollapsed + '">\uFEFF</span>');
            html.push('<span class="selector"' + (ctx.selectorEditable ? ' editable="true"' : '') + '></span>');
            html.push('<span class="brace">{</span>');
            html.push('</div>');
            html.push('<div class="body">');
            html.push('<div class="property"></div>');
            html.push('<div class="textarea">');
            html.push('<textarea class="actual"></textarea>');
            html.push('<textarea class="measure"></textarea>');
            html.push('</div>');
            html.push('</div>');
            html.push('<div class="foot">');
            html.push('<span class="brace">}</span>');
            html.push('<span class="swap" title="' + txtRes.btnSwapTitle + '"></span>');
            html.push('</div>');
            html.push('</div>');
            html.push('</div>');
            return html.join('');
        },

        updateRule: function (rule, ellipsis) {
            // cancel editor
            if (this.editorObj) { this.editorObj.cancel(); }
            if (this.currentRule != rule && ctx.proactiveSaving && this.isTextShowMode())
            { this.swapBtn.triggerHandler('click'); }
            // cache rule
            this.currentRule = rule;
            // head foot
            this.applyShowMode();
            this.contentCon.children('.message').hide();
            this.contentCon.children('.head, .foot').show();
            // set value
            this.textareaObj.setValue(rule.getPropertiesText());
            this.selectorObj.setName(rule.getSelectorText());
            if (ellipsis) { this.selectorObj.autoEllipsisText() }
            // properties
            this.clearProperties();
            var items = rule.getProperties(), len = items.length, to = this.propertyCon;
            for (var i = 0; i < len; i++) {
                var item = items[i];
                this.propertyItems.push(new propertyClass({
                    host: this,
                    renderTo: to,
                    refProp: item
                }));
            }
        },

        clean: function () {
            if (!this.currentRule) { return; }
            // editor
            if (this.editorObj) { this.editorObj.cancel(); }
            // head foot
            this.textareaObj.hide(false);
            this.contentCon.children('.message').show();
            this.contentCon.children('.head, .foot').hide();
            // clear value
            this.selectorObj.clean();
            this.textareaObj.clean();
            // properties
            this.clearProperties();
            this.removeRuleSubscribe();
            this.currentRule = null;
        },

        applyShowMode: function () {
            if (this.isTextShowMode()) {
                this.propertyCon.hide();
                this.textareaObj.show();
                this.swapBtn.attr('title', txtRes.btnSwapBackTitle);
            } else {
                this.propertyCon.show();
                this.textareaObj.hide();
                this.swapBtn.attr('title', txtRes.btnSwapTitle);
            }
        },

        toggleShowMode: function () {
            this.swapBtn.toggleClass('swapback');
            this.applyShowMode();
        },

        isTextShowMode: function () {
            return this.swapBtn.hasClass('swapback');
        },

        applyEdit: function (target) {
            this.initializeEditor();
            if (!this.editorObj.contains(target)) {
                if (ctx.proactiveSaving && this.editorObj.isValid()) {
                    this.editorObj.save();
                } else {
                    this.editorObj.cancel();
                }
                if (target.attr('editable') === 'true' && target.is(':visible')) {
                    var options, valueFunc;
                    if (target.hasClass('name')) {
                        var prop = target.parent().data('propIns');
                        options = ctx.propertySet;
                        valueFunc = function (val) { if (val === undefined) { return prop.getName(); } else { prop.setName(val); } };
                    } else if (target.hasClass('value')) {
                        var prop = target.parent().data('propIns');
                        options = ctx.propertySet[prop.getName().toLowerCase()];
                        valueFunc = function (val) { if (val === undefined) { return prop.getValue(); } else { prop.setValue(val); } };
                    } else if (target.hasClass('selector')) {
                        var sele = target.data('seleIns');
                        options = null;
                        valueFunc = function (val) { if (val === undefined) { return sele.getName(); } else { sele.setName(val); } };
                    }
                    if (valueFunc) {
                        this.editorObj.edit(target, options, valueFunc);
                    }
                }
            }
        },

        newProperty: function () {
            var referencedProperty = this.currentRule.newProperty();
            var prop = new propertyClass({
                host: this,
                renderTo: this.propertyCon,
                refProp: referencedProperty
            });
            this.propertyItems.push(prop);
            return prop;
        },

        removeProperty: function (item) {
            var success = false, len = this.propertyItems.length;
            for (var i = len - 1; i > -1; i--) {
                if (this.propertyItems[i] === item) {
                    this.propertyItems.splice(i, 1);
                    success = true;
                    break;
                }
            }
            return success;
        },

        sortProperties: function () {
            var properties = [], refProperties = [];
            this.propertyCon.children().each(function () {
                var prop = $(this).data('propIns');
                if (prop) {
                    properties.push(prop);
                    refProperties.push(prop.refProp);
                }
            });
            this.propertyItems = properties;
            this.currentRule.sortProperties(markSelf({ properties: refProperties })); // sync
        },

        clearProperties: function () {
            $.each(this.propertyItems, function () {
                this.remove({ removeRef: false });
            });
            this.propertyItems = [];
        },

        removeRuleSubscribe: function () {
            if (this.currentRule) {
                this.currentRule.onPropertySort.remove(this._onpropertysort);
                this.currentRule.onPropertyChange.remove(this._onpropertychange);
                this.currentRule.onSelectorChange.remove(this._onselectorchange);
            }
        },

        remove: function () {
            $(document).unbind(this._docclick);
            this.host.onLoad.remove(this._onload);
            this.host.onSelect.remove(this._onselect);
            this.removeRuleSubscribe();
            if (this.editorObj) { this.editorObj.remove(); }
            this.moveDragObj.destroy();
            this.clearProperties();
            this.selectorObj.remove();
            this.textareaObj.remove();
            this.el.remove();
            this.host = null;
        }
    };


    var selectorClass = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    selectorClass.prototype = {

        host: null, el: null, toggle: null, charSize: null,

        constructor: selectorClass,

        initialize: function () {
            var self = this;
            this.el.data('seleIns', this);
            this.toggle.click(function () {
                $(this).toggleClass('ellipsis');
                self.ellipsisText($(this).hasClass('ellipsis'));
            });
            this.validateRule();
        },

        getNameEl: function () {
            return this.el;
        },

        getName: function () {
            return this.host.currentRule.getSelectorText();
        },

        setName: function (name) {
            var old = this.getName(), trim = name.trim();
            if (old !== trim) {
                this.host.currentRule.setSelectorText(markSelf({ selector: trim })); // sync
                this.validateRule();
            }
            this.el.html(this.getName());
        },

        autoEllipsisText: function () {
            this.el.removeClass('ellipsis');
            this.charSize = this.charSize || ctx.testCharSize(this.el);
            if (this.el.innerHeight() > this.charSize.height) {
                this.toggle.css({ display: 'inline-block' });
                this.ellipsisText(true);
            } else {
                this.toggle.hide();
                this.ellipsisText(false);
            }
        },

        ellipsisText: function (ellipsis) {
            if (ellipsis) {
                this.el.addClass('ellipsis').attr('title', this.el.text());
                this.toggle.addClass('ellipsis').attr('title', txtRes.titleCollapsed);
            } else {
                this.el.removeClass('ellipsis').removeAttr('title');
                this.toggle.removeClass('ellipsis').attr('title', txtRes.titleExpended);
            }
        },

        validateRule: function (set) {
            set = set || {};
            if (!this.host.currentRule) { return; }
            var success = true, name = this.getName();
            if (name) { success = this.host.currentRule.testSelectorText(name); }
            if (set.warning !== false) { this.setWarning(!success); }
            return success;
        },

        setWarning: function (warning) {
            if (warning) {
                this.el.addClass('warning');
            } else {
                this.el.removeClass('warning');
            }
        },

        clean: function () {
            this.el.empty();
            this.setWarning(false);
        },

        remove: function () {
            this.el.remove();
            this.host = null;
        }
    };


    var textareaClass = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    textareaClass.prototype = {

        host: null, el: null, area: null, measure: null, scroll: null,

        value: '', showing: false, intervalId: null, lastHeight: null,

        constructor: textareaClass,

        initialize: function () {
            var self = this;
            this.area = this.el.children('.actual');
            this.measure = this.el.children('.measure');
            this.area.bind('keydown', function (ev) {
                setTimeout(function () { self.syncSize(); }, 0);
                self.helpInput(ev);
            });
            if ($.browser.msie) {
                this.area.get(0).attachEvent('onpropertychange', function (ev) {
                    if (ev.propertyName === 'value') { self.syncSize(); }
                });
            }
        },

        helpInput: function (ev) {
            if (ev.keyCode === 9) {// TAB
                ev.preventDefault();
                if (ev.shiftKey) {
                    this.host.applyEdit(this.host.selectorObj.getNameEl());
                } else {
                    ctx.insertText(this.area, '    ');
                }
            } else if (ev.keyCode === 13) {// ENTER
                ev.preventDefault();
                ctx.insertText(this.area, $.browser.opera ? '\r\n    ' : '\n    ');
            } else if (ev.keyCode === 83 && ev.ctrlKey) {// CTRL+S
                ev.preventDefault();
                this.host.swapBtn.triggerHandler('click');
            }
        },

        syncSize: function () {
            this.measure.val(this.area.val());
            var scrollHeight = this.measure.get(0).scrollHeight;
            this.area.css({ height: scrollHeight });
            // force the sub part of cursor scroll static
            if (this.lastHeight !== scrollHeight) {
                var lineHeight = parseInt(this.area.css('line-height').replace(/\D/g, ''));
                this.scroll.scrollTop(this.scroll.scrollTop() + lineHeight);
                this.lastHeight = scrollHeight;
            }
        },

        show: function () {
            if (this.showing) { return; } this.showing = true;
            this.el.show(); var val = this.host.currentRule.getPropertiesText();
            this.setValue(val); this.setFocus(); var self = this;
            this.intervalId = setInterval(function () {
                self.syncSize();
            }, ($.browser.msie ? 1000 : 100)); // in ie: when interval time less than 800, the cursor will be not flash(wink).
        },

        hide: function (sync) {
            if (!this.showing) { return; } this.showing = false;
            if (sync !== false && this.isChanged())
            { this.host.currentRule.setPropertiesText({ text: this.getValue() }); } // sync
            clearInterval(this.intervalId);
            this.el.hide();
        },

        isChanged: function () {
            return (this.value !== this.getValue(true));
        },

        getValue: function (native) {
            if (native) {
                return this.area.val();
            } else {
                // distinguish each text line to a property
                var parts = this.area.val().split('\n');
                $.each(parts, function (index) {
                    var item = ctx.nospace(this);
                    if (item.length > 0 && item.charAt(item.length - 1) !== ';') {
                        parts[index] = this + ';';
                    }
                });
                return parts.join('');
            }
        },

        setValue: function (val) {
            this.value = val.trim();
            this.area.val(val);
            this.syncSize();
        },

        setFocus: function (all) {
            this.area.focus();
            var val = this.area.val(), len = val.length;
            if ($.browser.opera) { len = val.replace(/\n/ig, '\r\n').length; }
            ctx.selectText(this.area, all ? 0 : len, len);
            this.scroll.scrollTop(this.scroll.get(0).scrollHeight);
        },

        clean: function () {
            this.area.empty();
            this.measure.empty();
        },

        remove: function () {
            clearInterval(this.intervalId);
            this.el.remove();
            this.host = null;
        }
    };


    var propertyClass = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    propertyClass.prototype = {

        host: null, renderTo: null, refProp: null, warning: false,

        el: null, moveEl: null, statusEl: null, nameEl: null, valueEl: null,

        constructor: propertyClass,

        initialize: function () {
            var self = this;
            this.el = $(this.buildHtml());
            this.el.appendTo(this.renderTo);
            this.el.data('propIns', this);
            this.moveEl = this.el.find('.move');
            this.statusEl = this.el.find('.status');
            this.nameEl = this.el.find('.name');
            this.valueEl = this.el.find('.value');
            // dom events
            var hoverIn = function () {
                if (moving) { return; }
                self.moveEl.css('display', 'block');
                if (self.isEnable() && !self.warning) { self.statusEl.css('display', 'block'); }
            }, hoverOut = function () {
                if (moving) { return; }
                self.moveEl.removeAttr('style');
                if (self.isEnable() && !self.warning) { self.statusEl.removeAttr('style'); }
            };
            this.el.hover(hoverIn, hoverOut).bind('hoverOut', hoverOut);
            this.statusEl.click(function () {
                self.setEnable(!self.isEnable());
                hoverIn();
            });
            // status
            if (!this.isEnable()) { this.setEnable(false); }
            // validate rule
            this.validateRule();
        },

        buildHtml: function () {
            var html = [];
            html.push('<div class="item">');
            html.push('<span class="move" title="' + txtRes.moveTitle + '"></span>');
            html.push('<span class="status" title="' + txtRes.statusEnableTitle + '"></span>');
            html.push('<span class="nbsp"></span>');
            html.push('<span class="name" editable="true">' + (this.getName() || ($.browser.opera ? '&nbsp;' : '')) + '</span>'); // opera need a whitespace, or nameEl is unvisible
            html.push('<span class="colon">:</span>');
            html.push('<span class="value" editable="true">' + (this.getValue() || ($.browser.opera ? '&nbsp;' : '')) + '</span>');
            html.push('<span class="semicolon">;</span>');
            html.push('</div>');
            return html.join('');
        },

        getNameEl: function () {
            return this.nameEl;
        },

        getName: function () {
            return this.refProp.getName();
        },

        setName: function (name) {
            var old = this.getName(), trim = name.trim();
            if (trim !== old) {
                this.refProp.setName(markSelf({ name: trim })); // sync
                this.nameEl.html(this.getName() || ($.browser.opera ? '&nbsp;' : ''));
                this.validateRule();
            }
        },

        getValue: function () {
            return this.refProp.getValue();
        },

        setValue: function (value) {
            var old = this.getValue(), trim = value.trim();
            if (old !== trim) {
                this.refProp.setValue(markSelf({ value: trim })); // sync
                this.valueEl.html(this.getValue() || ($.browser.opera ? '&nbsp;' : ''));
                this.validateRule();
            }
        },

        isEnable: function () {
            return this.refProp.isEnable();
        },

        isImportant: function () {
            return this.refProp.isImportant();
        },

        setEnable: function (enable) {
            if (enable) {
                this.el.removeClass('disabled');
                this.statusEl.attr('title', this.warning ? txtRes.statusWarningTitle : txtRes.statusEnableTitle);
            } else {
                this.el.addClass('disabled');
                this.statusEl.attr('title', txtRes.statusDisabledTitle);
            }
            this.refProp.setEnable(markSelf({ enable: enable })); // sync
        },

        setWarning: function (warningName, warningValue) {
            if (warningName || warningValue) {
                this.warning = true;
                this.el.addClass('warning');
                this.statusEl.attr('title', this.isEnable() ? txtRes.statusWarningTitle : txtRes.statusDisabledTitle);
            } else {
                this.warning = false;
                this.el.removeClass('warning');
                this.statusEl.attr('title', this.isEnable() ? txtRes.statusEnableTitle : txtRes.statusDisabledTitle);
            }
            if (warningName) {
                this.nameEl.addClass('warning');
            } else {
                this.nameEl.removeClass('warning');
            }
            if (warningValue) {
                this.valueEl.addClass('warning');
            } else {
                this.valueEl.removeClass('warning');
            }
        },

        validateRule: function (set) {
            set = set || {};
            var nameRet = true, valueRet = true, name = this.getName(), value = this.getValue();
            if (name) { nameRet = this.host.currentRule.testPropertyName(name); }
            if (name && value) { valueRet = this.host.currentRule.testPropertyValue(name, value); }
            if (set.warning !== false) { this.setWarning(!nameRet, !valueRet); }
            return nameRet && valueRet;
        },

        validateEmpty: function (set) {
            set = set || {};
            if (!this.getName() || !this.getValue()) {
                if (set.remove !== false) { this.remove(); }
                return true;
            } else {
                return false;
            }
        },

        remove: function (set) {
            set = set || {};
            if (set.removeRef !== false) {
                this.host.removeProperty(this);
                this.refProp.remove(markSelf({})); // sync
            }
            this.el.remove();
            this.host = null;
        }
    };


    var editorClass = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    editorClass.prototype = {

        renderTo: null, onNext: null, onPrev: null, onComplete: null, onPreview: null,

        el: null, input: null, measure: null, target: null, valueFunc: null, options: null,

        oldValue: '', lastValue: '', maxWidth: 0, minWidth: 20, charSize: { width: 7, height: 15 },

        constructor: editorClass,

        initialize: function () {
            var self = this;
            this.el = $(this.buildHtml());
            this.el.appendTo(this.renderTo);
            this.input = this.el.find('input');
            this.measure = this.el.find('.measure');
            // dom events
            var timeoutId1, timeoutId2;
            this.input.keydown(function (ev) {
                clearTimeout(timeoutId1);
                clearTimeout(timeoutId2);
                if (ev.keyCode === 9) {// TAB
                    ev.preventDefault();
                    self.save({ shift: ev.shiftKey });
                } else if (ev.keyCode === 13) {// ENTER
                    ev.preventDefault();
                    self.save({ shift: ev.shiftKey });
                } else if (ev.keyCode === 27) {// ESC
                    ev.preventDefault();
                    self.cancel();
                } else if (ev.keyCode === 38) {// UP
                    self.selectOption({ isNext: false });
                } else if (ev.keyCode === 40) {// DOWN
                    self.selectOption({ isNext: true });
                }
                if (!ev.isDefaultPrevented()) {
                    timeoutId1 = setTimeout(keyup, 0);
                }
            });
            var keyup = function () {
                // complete
                var val = self.getValue(), found;
                if (val.length > self.lastValue.length) {
                    found = self.autoComplete(val);
                }
                if (!found) {
                    self.measureWidth(self.getValue());
                }
                self.lastValue = val;
                // preview
                clearTimeout(timeoutId2);
                timeoutId2 = setTimeout(function () {
                    self.onPreview.dispatch(self.target, self.getValue());
                }, 192);
            };
            // customize events
            this.onNext = new yardi.dispatcher(this);
            this.onPrev = new yardi.dispatcher(this);
            this.onPreview = new yardi.dispatcher(this);
            this.onComplete = new yardi.dispatcher(this);
        },

        buildHtml: function () {
            var html = [];
            html.push('<div class="editor">');
            html.push('<input type="text" />');
            html.push('<span class="measure"></span>');
            html.push('</div>');
            return html.join('');
        },

        contains: function (target) {
            var con = this.el.get(0);
            target = target.jquery ? target.get(0) : target;
            return yardi.isAncestor(con, target) || (target == con);
        },

        measureWidth: function (val) {
            var width = this.measure.html(val).innerWidth();
            width = Math.max(Math.min(width, this.maxWidth), this.minWidth);
            this.el.css({ width: width + this.charSize.width });
        },

        isEditing: function () {
            return !!this.target;
        },

        setValue: function (val) {
            this.measureWidth(val);
            this.input.val(val);
        },

        getValue: function () {
            return this.input.val();
        },

        setTargetValue: function (val) {
            return this.valueFunc(val);
        },

        getTargetValue: function () {
            return this.valueFunc(undefined);
        },

        edit: function (target, options, valueFunc) {
            // cache
            this.target = target;
            this.options = options;
            this.valueFunc = valueFunc;
            // test
            this.charSize = ctx.testCharSize(target);
            // pos
            var tarPos = this.target.position();
            var pos = { left: tarPos.left, top: tarPos.top - 2 };
            var innerWidth = this.target.parent().innerWidth();
            this.maxWidth = innerWidth - pos.left - this.charSize.width - 2;
            if (this.target.innerHeight() > this.charSize.height) {// is multi line
                pos.top += this.charSize.height;
            }
            // dom
            this.target.parent().append(this.el);
            this.target.addClass('editing');
            this.el.css(pos).show();
            // apply
            this.lastValue = this.getTargetValue();
            this.setValue(this.lastValue);
            this.input.focus().select();
        },

        save: function (set) {
            set = set || {};
            // cache
            var t = this.target;
            // value
            this.oldValue = this.getTargetValue();
            this.setTargetValue(this.getValue());
            // reset
            this.cancel({ isSave: true });
            // event
            if (set.shift !== undefined) {
                if (set.shift === true) {
                    this.onPrev.dispatch(t);
                } else {
                    this.onNext.dispatch(t);
                }
            }
        },

        cancel: function (set) {
            set = set || {};
            if (!this.isEditing()) { return; }
            // restore
            this.el.hide();
            this.el.appendTo(this.renderTo);
            this.setWarning(undefined);
            // check value
            if (this.target) {
                var val = this.getTargetValue();
                this.onComplete.dispatch(this.target, val, this.oldValue, set.isSave);
            }
            // reset
            this.oldValue = '';
            this.lastValue = '';
            this.options = null;
            this.valueFunc = null;
            if (this.target) {
                this.target.removeClass('editing');
                this.target = null;
            }
        },

        selectOption: function (set) {
            set = set || {};
            if (!this.options) { return; }
            var val = this.getValue().toLowerCase()
            var first, last, prev, next, find = false, breaked = false;
            $.each(this.options, function (key) {
                last = key;
                if (!first) { first = key; }
                if (!breaked) {
                    if (find === true) { next = key; breaked = true; }
                    if (val === key.toLowerCase()) { find = true; }
                    if (find === false) { prev = key; }
                }
            });
            var val;
            if (find) {
                val = set.isNext ? (next || first) : (prev || last);
            } else if (first) {
                val = first;
            }
            if (val) {
                this.setValue(val);
                this.lastValue = val; // set lastValue to prevent fire autoComplete
            }
        },

        autoComplete: function (val) {
            if (!this.options) { return; }
            var len = val.length;
            if (!len) { return; }
            // do search
            var result, lowerVal = val.toLowerCase();
            $.each(this.options, function (key) {
                if (key.toLowerCase().substr(0, len) == lowerVal) {
                    result = key;
                    return false;
                }
            });
            if (result) {
                // set value
                this.setValue(val + result.substr(len));
                // set selection
                var rlen = result.length;
                if (len < rlen) { ctx.selectText(this.input, len, rlen); }
                return true;
            }
        },

        setWarning: function (warning) {
            this.el.removeClass('valid warning');
            if (warning !== undefined) {
                if (warning === true) {
                    this.el.addClass('warning');
                } else {
                    this.el.addClass('valid');
                }
            }
        },

        isValid: function () {
            return this.el.hasClass('valid');
        },

        remove: function () {
            this.cancel();
            this.el.remove();
        }
    };


    // register
    ctx.ruletextClass = ruletextClass;

} (jQuery, visualstyle));
