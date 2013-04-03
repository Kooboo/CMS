/*
*
* fonts
* author: ronglin
* create date: 2012.3.29
*
*/

(function ($) {

    // custom fonts
    var fonts = [];

    // outer api
    $.each(['strip', 'restore', 'refresh'], function () {
        yardi[this + 'Fonts'] = function (name) {
            return function () {
                var args = arguments;
                $.each(fonts, function () {
                    if (!this.available()) { return; }
                    if (this[name].apply(this, args)) { return false; }
                });
            };
        } (this);
    });

    // register cufon
    // https://github.com/sorccu/cufon
    // http://cufon.shoqolate.com/generate/
    fonts.push({
        available: function () {
            return !!window.Cufon;
        },
        strip: function (el, sender) {
            var cufon = el.find('cufon');
            if (cufon.length > 0) {
                el.attr('hasCufon', 'true');
                cufon.each(function () {
                    var con = this.parentNode;
                    var texts = this.getElementsByTagName('cufontext');
                    for (var n = 0; n < texts.length; n++) {
                        var text = texts[n];
                        while (text.childNodes.length > 0)
                        { con.insertBefore(text.childNodes[0], this); }
                    }
                }).remove();
            }
        },
        restore: function (el, sender) {
            if (el.attr('hasCufon') === 'true') {
                el.removeAttr('hasCufon');
                if ($.browser.msie) {
                    window.Cufon.replace(el.get(0));
                } else {
                    window.Cufon.refresh();
                }
            }
        },
        refresh: function (el, sender) {
            var cufon = el.find('cufon');
            if (cufon.length > 0) {
                if ($.browser.msie) {
                    this.strip(el, sender);
                    window.Cufon.replace(el.get(0));
                } else {
                    window.Cufon.refresh();
                }
            }
        }
    });

    // register typeface
    // http://typeface.neocracy.org/
    fonts.push({
        available: function () {
            return !!window._typeface_js;
        },
        strip: function (el, sender) {
            var con = el.find('.typeface-js-vector-container');
            if (con.length > 0) {
                el.attr('hasTypeface', 'true');
                con.each(function () {
                    var conDom = this, par = conDom.parentNode;
                    $(this).find('.typeface-js-selected-text').each(function () {
                        while (this.childNodes.length > 0)
                        { par.insertBefore(this.childNodes[0], conDom); }
                    });
                }).remove();
                // force multi neighbour text nodes combine to one text node.
                // because typeface create instance for each text node, this will higher performance.
                el.html(el.html());
            }
        },
        restore: function (el, sender) {
            if (el.attr('hasTypeface') === 'true') {
                el.removeAttr('hasTypeface');
                window._typeface_js.replaceText(el.get(0));
            }
        },
        refresh: function (el, sender) {
            var con = el.find('.typeface-js-vector-container');
            if (con.length > 0) {
                this.strip(el, sender);
                window._typeface_js.replaceText(el.get(0));
                //window._typeface_js.renderDocument();
            }
        }
    });

    // register SIFR
    // http://www.sifrgenerator.com/
    fonts.push({
        available: function () {
            return !!window.sIFR;
        },
        strip: function (el, sender) {
            var alternate = el.find('.sIFR-alternate');
            if (alternate.length > 0) {
                el.attr('hasSIFR', 'true');
                alternate.each(function () {
                    var par = this.parentNode;
                    while (this.childNodes.length > 0)
                    { par.insertBefore(this.childNodes[0], this); }
                }).remove();
                el.find('.sIFR-flash').remove();
            }
        },
        restore: function (el, sender) {
            if (el.attr('hasSIFR') === 'true') {
                el.removeAttr('hasSIFR');
                //TODO: do replace
            }
        },
        refresh: function (el, sender) {
            var alternate = el.find('.sIFR-alternate');
            if (alternate.length > 0) {
                //TODO: do replace
            }
        }
    });

})(jQuery);
