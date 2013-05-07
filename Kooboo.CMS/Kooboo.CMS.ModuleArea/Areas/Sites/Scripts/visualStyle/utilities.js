/*
*   utilities
*   author: ronglin
*   create date: 2012.3.1
*/

(function ($, ctx) {

    // remove whitespace
    ctx.nospace = function (text, rpl) {
        rpl = (rpl === undefined) ? '' : rpl;
        return (text || '').replace(/\s+/g, rpl).toLowerCase();
    };

    // remove css comments
    var commentRe = new RegExp('/\\*[\\w\\W]*?\\*/', 'ig');
    ctx.noComment = function (text) {
        return (text || '').replace(commentRe, '');
    };

    // parse "font-size" to "fontSize"
    ctx.cameName = function (name) {
        return name.replace(/(-[a-z])/gi, function (m, a) { return a.charAt(1).toUpperCase(); });
    };

    // parse "font size" to "Font Size"
    ctx.capitalizeText = function (text) {
        return text.replace(/(^|\s)[a-z]/g, function (match) { return match.toUpperCase() });
    };

    // parse "rgb(0,0,0)" to "#000000"
    ctx.rgbToHex = function (value) {
        if (value.indexOf('rgb') === -1) { return value; }
        return value.replace(/\brgb\((\d{1,3}),\s*(\d{1,3}),\s*(\d{1,3})\)/gi, function (_, r, g, b) {
            return '#' + ((1 << 24) + (r << 16) + (g << 8) + (b << 0)).toString(16).substr(-6).toUpperCase();
        });
    };

    // test element char size
    ctx.testCharSize = function (target) {
        var test = target.clone().html('a').insertAfter(target);
        test.css({ display: 'inline', height: '', width: '' });
        var lineHeight = parseInt(test.css('line-height').replace(/\D/g, ''));
        var ret = {
            width: test.innerWidth(),
            height: Math.max(test.innerHeight(), lineHeight)
        };
        test.remove();
        return ret;
    };

    // select text(selection)
    ctx.selectText = function (el, start, end) {
        var dom = el.get(0);
        if (dom.setSelectionRange) {
            dom.setSelectionRange(start, end);
        } else if (dom.createTextRange) {
            var range = dom.createTextRange();
            range.moveStart('character', start);
            range.moveEnd('character', end - start);
            range.select();
        }
        if (yardi.isGecko || yardi.isOpera) {
            el.focus();
        }
    };

    // insert text(selection)
    ctx.insertText = function (el, txt) {
        var dom = el.get(0);
        if (window.getSelection) {
            var start = dom.selectionStart, end = dom.selectionEnd, temp = dom.value;
            dom.value = temp.substring(0, start) + txt + temp.substring(end, temp.length);
            if ($.browser.opera) {
                dom.selectionEnd = dom.selectionStart = start + txt.length;
            } else {
                dom.selectionStart = dom.selectionEnd = start + txt.length;
            }
        } else if (document.selection) {
            var range = document.selection.createRange();
            range.text = txt; range.collapse(false); range.select();
        } else {
            dom.value += txt;
        }
    };

    // bind auto complete to input
    ctx.bindAutoComplete = function (input, items, change) {
        // auto complete
        var timeoutId, lastValue = '', measureValue = '', autoComplete = function (val) {
            // do search
            var result, lowerVal = val.toLowerCase(), len = val.length;
            $.each(items, function (key) {
                if (key.toLowerCase().substr(0, len) == lowerVal) {
                    result = key;
                    return false;
                }
            });
            if (result) {
                // set value
                input.val(val + result.substr(len));
                // set selection
                var rlen = result.length;
                if (len < rlen) { ctx.selectText(input, len, rlen); }
            }
        }, selectOption = function (isNext) {
            var val = input.val().toLowerCase()
            var first, last, prev, next, find = false, breaked = false;
            $.each(items, function (key) {
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
                val = isNext ? (next || first) : (prev || last);
            } else if (first) {
                val = first;
            }
            if (val) {
                input.val(val);
                lastValue = val; // set lastValue to prevent fire autoComplete
            }
        };
        // event
        input.keydown(function (ev) {
            if (ev.keyCode === 38) {// UP
                selectOption(false);
            } else if (ev.keyCode === 40) {// DOWN
                selectOption(true);
            }
            clearTimeout(timeoutId);
            timeoutId = setTimeout(function () {
                var val = input.val();
                if (val.length > lastValue.length)
                { autoComplete(val); }
                lastValue = val;
            }, 0);
        }).focus(function () {
            measureValue = input.val();
        }).blur(function () {
            if (measureValue !== input.val()) {
                change && change();
            }
        });
    };

} (jQuery, visualstyle));
