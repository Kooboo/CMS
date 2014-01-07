/*
*   editors
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
        title: 'Editors',
        fontGroup: 'Font properties',
        textGroup: 'Text properties',
        boxGroup: 'Box properties',
        positioningGroup: 'Positioning properties',
        backgroundGroup: 'Background properties'
    };
    // localize text resource
    if (window.__localization) { $.extend(txtRes, __localization.editors_js); }

    // helpers
    var markSelf = function (obj) {
        obj['editors_js'] = true; return obj;
    }, isSelf = function (obj) {
        return (obj['editors_js'] === true);
    };

    /*
    * editorsClass
    */
    var editorsClass = function (config) {
        $.extend(this, config);
        //this.initialize();
    };

    editorsClass.prototype = {

        renderTo: null, host: null, el: null,

        currentRule: null, itemEditors: null,

        _onload: null, _onpropertychange: null,

        constructor: editorsClass,

        initialize: function () {
            var self = this;
            this.itemEditors = {};
            this.el = $(this.bulidHtml()).appendTo(this.renderTo);
            // dom events
            var groupHeads = this.el.find('.content .group .head');
            groupHeads.click(function () {
                if ($(this).hasClass('select')) { return; }
                groupHeads.each(function ()
                { $(this).next().slideUp('fast'); });
                $(this).next().slideDown('fast');
                groupHeads.removeClass('select');
                $(this).addClass('select');
            }).hover(function () {
                $(this).addClass('hover');
            }, function () {
                $(this).removeClass('hover');
            });
            // subscribe host events
            this._onload = function () { self.clean(); };
            this._onpropertychange = function (sender, set) {
                if (isSelf(set)) { return; }
                self.updateRule(self.currentRule);
            };
            this._onselect = function (sender, set) {
                if (isSelf(set)) { return; }
                self.removeRuleSubscribe();
                self.updateRule(set.rule);
                self.currentRule.onPropertySort.add(self._onpropertychange);
                self.currentRule.onPropertyChange.add(self._onpropertychange);
            };
            this.host.onLoad.add(this._onload);
            this.host.onSelect.add(this._onselect);
            // register editors
            this.registerComponents(this.itemEditors);
        },

        bulidHtml: function () {
            var html = [];
            html.push('<div class="vs-editors">');
            html.push('<h3>' + txtRes.title + '</h3>');
            html.push('<div class="content">');
            html.push('<div class="group"><div class="head">' + txtRes.fontGroup + '</div><div class="body"></div></div>');
            html.push('<div class="group"><div class="head">' + txtRes.textGroup + '</div><div class="body"></div></div>');
            html.push('<div class="group"><div class="head">' + txtRes.boxGroup + '</div><div class="body"></div></div>');
            html.push('<div class="group"><div class="head">' + txtRes.positioningGroup + '</div><div class="body"></div></div>');
            html.push('<div class="group"><div class="head">' + txtRes.backgroundGroup + '</div><div class="body"></div></div>');
            html.push('</div>');
            html.push('</div>');
            return html.join('');
        },

        registerComponents: function (holder) {
            var bodys = this.el.find('.content .group .body');
            // Font properties
            var group0 = bodys.eq(0);
            holder['font-family'] = new ctx.editors.textClass({ host: this, renderTo: group0, title: 'Font-family', name: 'font-family' });
            holder['font-size'] = new ctx.editors.unitClass({ host: this, renderTo: group0, title: 'Font-size', name: 'font-size', units: ctx.cssLengthUnits });
            holder['font-weight'] = new ctx.editors.comboClass({ host: this, renderTo: group0, title: 'Font-weight', name: 'font-weight', items: ctx.propertySet['font-weight'] });
            holder['line-height'] = new ctx.editors.unitClass({ host: this, renderTo: group0, title: 'Line-height', name: 'line-height', units: ctx.cssLengthUnits });
            holder['font-variant'] = new ctx.editors.comboClass({ host: this, renderTo: group0, title: 'Font-variant', name: 'font-variant', items: ctx.propertySet['font-variant'] });
            holder['letter-spacing'] = new ctx.editors.unitClass({ host: this, renderTo: group0, title: 'Letter-spacing', name: 'letter-spacing', units: ctx.cssLengthUnits });
            holder['font-style'] = new ctx.editors.comboClass({ host: this, renderTo: group0, title: 'Font-style', name: 'font-style', items: ctx.propertySet['font-style'] });
            holder['color'] = new ctx.editors.colorClass({ host: this, renderTo: group0, title: 'Color', name: 'color' });
            // Text properties
            var group1 = bodys.eq(1);
            holder['text-align'] = new ctx.editors.comboClass({ host: this, renderTo: group1, title: 'Text-align', name: 'text-align', items: ctx.propertySet['text-align'] });
            holder['text-decoration'] = new ctx.editors.comboClass({ host: this, renderTo: group1, title: 'Text-decoration', name: 'text-decoration', items: ctx.propertySet['text-decoration'] });
            holder['text-transform'] = new ctx.editors.comboClass({ host: this, renderTo: group1, title: 'Text-transform', name: 'text-transform', items: ctx.propertySet['text-transform'] });
            holder['text-indent'] = new ctx.editors.unitClass({ host: this, renderTo: group1, title: 'Text-indent', name: 'text-indent', units: ctx.cssLengthUnits });
            holder['word-wrap'] = new ctx.editors.comboClass({ host: this, renderTo: group1, title: 'Word-wrap', name: 'word-wrap', items: ctx.propertySet['word-wrap'] });
            // Box properties
            var group2 = bodys.eq(2);
            holder['width'] = new ctx.editors.unitClass({ host: this, renderTo: group2, title: 'Width', name: 'width', units: ctx.cssLengthUnits });
            holder['height'] = new ctx.editors.unitClass({ host: this, renderTo: group2, title: 'Height', name: 'height', units: ctx.cssLengthUnits });
            holder['left'] = new ctx.editors.unitClass({ host: this, renderTo: group2, title: 'Left', name: 'left', units: ctx.cssLengthUnits });
            holder['top'] = new ctx.editors.unitClass({ host: this, renderTo: group2, title: 'Top', name: 'top', units: ctx.cssLengthUnits });
            holder['right'] = new ctx.editors.unitClass({ host: this, renderTo: group2, title: 'Right', name: 'right', units: ctx.cssLengthUnits });
            holder['bottom'] = new ctx.editors.unitClass({ host: this, renderTo: group2, title: 'Bottom', name: 'bottom', units: ctx.cssLengthUnits });
            holder['border'] = new ctx.editors.textClass({ host: this, renderTo: group2, title: 'Border', name: 'border' });
            holder['margin'] = new ctx.editors.textClass({ host: this, renderTo: group2, title: 'Margin', name: 'margin' });
            holder['padding'] = new ctx.editors.textClass({ host: this, renderTo: group2, title: 'Padding', name: 'padding' });
            // Positioning properties
            var group3 = bodys.eq(3);
            holder['display'] = new ctx.editors.comboClass({ host: this, renderTo: group3, title: 'Display', name: 'display', items: ctx.propertySet['display'] });
            holder['float'] = new ctx.editors.comboClass({ host: this, renderTo: group3, title: 'Float', name: 'float', items: ctx.propertySet['float'] });
            holder['overflow'] = new ctx.editors.comboClass({ host: this, renderTo: group3, title: 'Overflow', name: 'overflow', items: ctx.propertySet['overflow'] });
            holder['position'] = new ctx.editors.comboClass({ host: this, renderTo: group3, title: 'Position', name: 'position', items: ctx.propertySet['position'] });
            holder['opacity'] = new ctx.editors.numberClass({ host: this, renderTo: group3, title: 'Opacity', name: 'opacity' });
            holder['visibility'] = new ctx.editors.comboClass({ host: this, renderTo: group3, title: 'Visibility', name: 'visibility', items: ctx.propertySet['visibility'] });
            holder['clear'] = new ctx.editors.comboClass({ host: this, renderTo: group3, title: 'Clear', name: 'clear', items: ctx.propertySet['clear'] });
            holder['cursor'] = new ctx.editors.comboClass({ host: this, renderTo: group3, title: 'Cursor', name: 'cursor', items: ctx.propertySet['cursor'] });
            holder['z-index'] = new ctx.editors.numberClass({ host: this, renderTo: group3, title: 'Z-index', name: 'z-index' });
            // Background properties
            var group4 = bodys.eq(4);
            holder['background-color'] = new ctx.editors.colorClass({ host: this, renderTo: group4, title: 'Bg-color', name: 'background-color' });
            holder['background-image'] = new ctx.editors.fileClass({ host: this, renderTo: group4, title: 'Image', name: 'background-image' });
            holder['background-repeat'] = new ctx.editors.comboClass({ host: this, renderTo: group4, title: 'Repeat', name: 'background-repeat', items: ctx.propertySet['background-repeat'] });
            holder['background-attachment'] = new ctx.editors.comboClass({ host: this, renderTo: group4, title: 'Attachment', name: 'background-attachment', items: ctx.propertySet['background-attachment'] });
            holder['background-position'] = new ctx.editors.comboClass({ host: this, renderTo: group4, title: 'Position', name: 'background-position', items: ctx.propertySet['background-position'] });
        },

        updateRule: function (rule) {
            this.clean();
            this.currentRule = rule;
            var items = rule.getProperties(), self = this;
            $.each(items, function () {
                var editor = self.itemEditors[ctx.nospace(this.getName())];
                if (editor) { editor.assignProperty(this); }
            });
        },

        syncProperty: function (set) {
            if (this.currentRule) {
                set = set || {};
                return this.currentRule.setProperty(markSelf(set));
            }
        },

        firePreview: function (set) {
            if (this.currentRule) {
                set = set || {};
                this.currentRule.onPreview.dispatch(this, markSelf(set));
            }
        },

        firePreviewEnd: function (set) {
            if (this.currentRule) {
                set = set || {};
                this.currentRule.onPreviewEnd.dispatch(this, markSelf(set));
            }
        },

        clean: function () {
            $.each(this.itemEditors, function () { this.clean(); });
            this.currentRule = null;
        },

        removeRuleSubscribe: function () {
            if (this.currentRule) {
                this.currentRule.onPropertySort.add(this._onpropertychange);
                this.currentRule.onPropertyChange.remove(this._onpropertychange);
            }
        },

        remove: function () {
            this.removeRuleSubscribe();
            this.host.onLoad.remove(this._onload);
            this.host.onSelect.remove(this._onselect);
            $.each(this.itemEditors, function () { this.remove(); });
            this.el.remove();
            this.host = null;
        }
    };

    // register
    ctx.editorsClass = editorsClass;

} (jQuery, visualstyle));
