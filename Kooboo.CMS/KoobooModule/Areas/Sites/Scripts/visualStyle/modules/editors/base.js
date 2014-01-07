/*
*   base
*   author: ronglin
*   create date: 2011.12.28
*/

/*
* config parameters:
* renderTo, host, name, title
*/

(function ($, ctx) {

    // localization
    var txtRes = {
        enable: 'enable',
        disabled: 'disabled',
        important: 'important',
        noImportant: 'no important'
    };
    // localize text resource
    if (window.__localization) { $.extend(txtRes, __localization.editors_js.base_js); }

    var baseClass = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    baseClass.prototype = {

        renderTo: null, host: null, name: '', title: '', className: '',

        el: null, editCon: null, btnStatus: null, btnImportant: null, refProp: null,

        isImportant: false, isEnable: true, hasProperty: false,

        constructor: baseClass,

        initialize: function () {
            var self = this;
            this.el = $(this.baseHtml());
            this.el.appendTo(this.renderTo);
            this.editCon = this.el.children('.edit');
            this.btnStatus = this.el.children('.status');
            this.btnImportant = this.el.children('.important');
            // dom events
            this.btnStatus.click(function () {
                if (!self.hasProperty) { return; }
                self.setEnable(!self.isEnable);
                self.syncValue();
            });
            this.btnImportant.click(function () {
                if (!self.hasProperty) { return; }
                self.setImportant(!self.isImportant);
                self.syncValue();
            });
            this.editCon.find('input[type="text"]').focus(function () {
                var picker = yardi.pickerPanel.current;
                if (picker) { picker.hide(); }
            });
        },

        baseHtml: function () {
            var html = [];
            html.push('<div class="item">');
            html.push('<span class="status" title="' + txtRes.enable + '"></span>');
            html.push('<span class="important" title="' + txtRes.noImportant + '"></span>');
            html.push('<span class="title">' + this.title + '</span>');
            html.push('<span class="edit ' + this.className + '">');
            this.buildHtml(html);
            html.push('</span>');
            html.push('</div>');
            return html.join('');
        },

        syncValue: function (value) {
            value = value || this.getValue();
            var name = this.refProp ? this.refProp.getName() : this.name;
            var prop = this.host.syncProperty({
                refProp: this.refProp, sender: this,
                name: name, value: value, enable: this.isEnable, important: this.isImportant
            });
            if (prop != this.refProp) { //alert(null == undefined);  true
                this.assignProperty(prop);
            }
        },

        assignProperty: function (property) {
            if (!this.refProp || !property || property.isImportant() ||
               (!this.refProp.isImportant() && !property.isImportant())) {
                this.refProp = property;
                this.setValue(property ? property.getValue() : '', true);
                this.setEnable(property ? property.isEnable() : true);
                if (this.refProp) { this.setPropertied(true); }
            }
        },

        clean: function () {
            this.refProp = null;
            this.setValue('', true);
            this.setImportant(false);
            this.setEnable(true);
            this.setPropertied(false);
        },

        firePreview: function (set) {
            set = set || {};
            set.valid = true;
            set.name = this.name;
            set.important = this.refProp ? this.refProp.isImportant() : false;
            this.host.firePreview(set);
        },

        firePreviewEnd: function (set) {
            set = set || {};
            set.name = this.name;
            this.host.firePreviewEnd(set);
        },

        buildHtml: function (html) { },

        setEnable: function (enable) {
            enable = !!enable;
            if (enable === this.isEnable) { return; }
            this.isEnable = enable;
            if (this.isEnable) {
                this.el.removeClass('disabled');
                this.btnStatus.attr('title', txtRes.enable);
            } else {
                this.el.addClass('disabled');
                this.btnStatus.attr('title', txtRes.disabled);
            }
        },

        setImportant: function (important) {
            important = !!important;
            if (important === this.isImportant) { return; }
            this.isImportant = important;
            if (this.isImportant) {
                this.el.addClass('important');
                this.btnImportant.attr('title', txtRes.important);
            } else {
                this.el.removeClass('important');
                this.btnImportant.attr('title', txtRes.noImportant);
            }
        },

        setPropertied: function (hasProperty) {
            hasProperty = !!hasProperty;
            if (hasProperty === this.hasProperty) { return; }
            this.hasProperty = hasProperty;
            if (this.hasProperty) {
                this.el.addClass('propertied');
            } else {
                this.el.removeClass('propertied');
            }
        },

        getValue: function () { },

        setValue: function (value, base) {
            value = value || '';
            if (base === true) {
                var temp = ctx.nospace(value);
                this.setImportant(temp.indexOf('!important') > 0);
            }
            return value.replace(/\s*!\s*important/i, '');
        },

        remove: function () {
            this.el.remove();
            this.renderTo = null;
            this.host = null;
        }

    };

    // namespace
    ctx.editors = {};

    // register
    ctx.editors.baseClass = baseClass;

} (jQuery, visualstyle));
