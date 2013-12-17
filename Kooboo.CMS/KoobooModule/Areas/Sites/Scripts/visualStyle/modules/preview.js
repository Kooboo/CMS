/*
*   preview
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
        title: 'Preview',
        sampleText: 'Change styles from editor to see effects here.(Visual Style Editor)'
    };
    // localize text resource
    if (window.__localization) { $.extend(txtRes, __localization.preview_js); }

    /*
    * previewClass
    */
    var previewClass = function (config) {
        previewClass.superclass.constructor.call(this, config);
    };

    yardi.extend(previewClass, ctx.effectClass, {

        renderTo: null, el: null, target: null,

        initialize: function () {
            this.el = $(this.buildHtml()).appendTo(this.renderTo);
            this.target = this.el.find('.target');
            previewClass.superclass.initialize.call(this);
        },

        buildHtml: function () {
            var html = [];
            html.push('<div class="vs-preview">');
            html.push('<h3>' + txtRes.title + '</h3>');
            html.push('<div class="body">');
            html.push('<div class="target">' + txtRes.sampleText + '</div>');
            html.push('</div>');
            html.push('</div>');
            return html.join('');
        },

        updateRule: function (rule) {
            this.clean();
            this.currentRule = rule || this.currentRule;
            if (this.currentRule) { this.applyCss({ propertyText: this.currentRule.getPropertiesText() }); }
        },

        applyCss: function (set) {
            if (set.name) {
                var css = {};
                css[set.name] = (set.value || '') + (set.important ? '!important' : '');
                this.target.css(css);
            } else if (set.name) {
                this.target.attr('style', set.propertyText);
            }
        },

        clean: function () {
            this.target.removeAttr('style');
            this.currentRule = null;
        },

        remove: function () {
            this.el.remove();
            previewClass.superclass.remove.call(this);
        }

    });

    // register
    ctx.previewClass = previewClass;

} (jQuery, visualstyle));
