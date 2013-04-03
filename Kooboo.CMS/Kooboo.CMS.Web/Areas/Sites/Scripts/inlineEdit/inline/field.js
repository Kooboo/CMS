/*
*   field
*   author: ronglin
*   create date: 2011.01.28
*/

(function (ctx, $) {

    // text resource
    var options = {
        networkError: 'Network error, the action has been cancelled.',
        saving: 'saving...',
        saveSuccess: 'save successfully.',
        saveFailure: 'The attempt to save has failed.'
    };

    // override text resource
    if (window.__inlineEditVars) { $.extend(options, __inlineEditVars.field_js); }

    /*
    * field class
    */
    var fieldClass = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    fieldClass.prototype = {

        el: null, wrapper: null,

        params: null, menu: null, editor: null,

        updateAction: null,

        constructor: fieldClass,

        initialize: function () {
            this.params = this.getParams();
            this.ensureEditSpace();
            this.createMenu();
        },

        createMenu: function () {
            var self = this;
            this.menu = new ctx.fieldAnchor({
                title: this.params.fieldName,
                alignTo: this.el,
                renderTo: ctx.cacheCon,
                onEdit: function () { self.edit(); }
            });
        },

        getParams: function () {
            return {
                uuid: this.el.attr('uuid'),
                schema: this.el.attr('schema'),
                folder: this.el.attr('folder'),
                fieldName: this.el.attr('fieldName'),
                dataType: this.el.attr('dataType')
            };
        },

        ensureEditSpace: function () {
            this.removeEditSpace();
            var empty = ((this.el.text() || '').trim() === '');
            // hack, in ie8 and ie9, el will can not be touched when it contains cufon element
            if (!empty && $.browser.msie && $.browser.version > 7 && this.el.css('display') === 'inline') {
                if (this.el.width() === 0 || this.el.height() === 0) {
                    this.el.css('display', 'inline-block');
                }
            }
            // apply
            if (empty && (this.el.height() < 20 || this.el.width() < 40)) {
                this.el.addClass('kb-field-empty');
            }
        },

        removeEditSpace: function () {
            this.el.removeClass('kb-field-empty');
        },

        message: function (msg) {
            if (!this.editor) { return; }
            this.editor.message(msg);
        },

        doPost: function (url, data, error, succsss) {
            var self = this;
            $.ajax({
                url: url, data: data,
                type: 'post', dataType: 'json', timeout: 30000,
                beforeSend: function (jqXHR, settings) { self.editor.disabled(true); },
                complete: function (jqXHR, textStatus) { self.editor.disabled(false); },
                success: function (data, textStatus, jqXHR) { succsss(data); },
                error: function (jqXHR, textStatus, errorThrown) { error(); }
            });
        },

        wrap: function () {
            var dom = this.el.get(0), wrapper;
            if ($.browser.mozilla) {
                // ensure firefox use DIV as editor element, 
                // in firefox there ara some problems in other element types.
                // eg: SPAN within A will cause the content can not be removed.
                if (dom.tagName == 'DIV') { return this.el; }
                wrapper = $('<div></div>').appendTo(this.el);
            } else {
                if (dom.tagName == 'DIV' || dom.tagName == 'SPAN') { return this.el; }
                wrapper = $('<span></span>').appendTo(this.el);
            }
            // content
            var wrapperDom = wrapper.get(0);
            while (dom.childNodes.length > 1)
            { wrapperDom.appendChild(dom.childNodes[0]); }
            // style
            var float = this.el.css('float');
            if (float && float != 'none') {
                // fix widget float bug.
                // when widget set to float left or right, it break away the layout
                wrapper.css({
                    float: float,
                    minWidth: this.el.width()
                });
            }
            // ret
            return wrapper;
        },

        unwrap: function () {
            if (!this.wrapper) { return; }
            if (this.wrapper == this.el) { return; }
            var node = this.wrapper.get(0);
            while (node.childNodes.length > 0)
                node.parentNode.insertBefore(node.childNodes[0], node);
            this.wrapper.remove();
        },

        edit: function () {
            if (this.editor) { return; }
            var self = this;
            this.removeEditSpace();
            this.wrapper = this.wrap();
            ctx.stripFonts(this.wrapper, this);
            this.editor = new ctx.inlineEditor({
                el: this.wrapper,
                dataType: this.params.dataType || 'Auto',
                onSave: function () { self.save(); },
                onCancel: function () { self.cancel(); }
            });
            this.editor.edit();
        },

        cancel: function (revert) {
            if (!this.editor) { return; }
            this.editor.cancel(revert);
            ctx.restoreFonts(this.wrapper, this);
            this.unwrap();
            this.wrapper = null;
            this.ensureEditSpace();
            var self = this;
            setTimeout(function () { self.editor = null; }, 100);
        },

        save: function () {
            if (!this.editor) { return; }
            this.message(); //this.message(options.saving);
            var html = this.editor.value(), self = this;
            var postData = $.extend({}, this.params, { value: html });
            this.doPost(this.updateAction, postData, function () {
                self.message(options.saveFailure + '<br/>' + options.networkError);
            }, function (data) {
                if (!data.Success) {
                    self.message(options.saveFailure);
                    return;
                }
                // successful
                self.message(); //self.message(options.saveSuccess);
                //setTimeout(function () {
                self.cancel(false);
                //}, 800);
            });
        }
    };

    // register
    ctx.fieldClass = fieldClass;

})(yardi, jQuery);
