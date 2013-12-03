/*
*
* Yardi javascript.
*
*/

// global
yardi = function () {
    // copy from extjs
    var docMode = document.documentMode,
    ua = navigator.userAgent.toLowerCase(),
    check = function (r) { return r.test(ua); },
    // Opera
    isOpera = check(/opera/),
    isOpera10_5 = isOpera && check(/version\/10\.5/),
    // Chrome
    isChrome = check(/\bchrome\b/),
    // Safari
    isWebKit = check(/webkit/),
    isSafari = !isChrome && check(/safari/),
    isSafari2 = isSafari && check(/applewebkit\/4/),
    isSafari3 = isSafari && check(/version\/3/),
    isSafari4 = isSafari && check(/version\/4/),
    // IE
    isIE = !isOpera && check(/msie/),
    isIE7 = isIE && (check(/msie 7/) || docMode == 7),
    isIE8 = isIE && (check(/msie 8/) && docMode != 7 && docMode != 9 || docMode == 8),
    isIE9 = isIE && (check(/msie 9/) && docMode != 7 && docMode != 8 || docMode == 9),
    isIE6 = isIE && check(/msie 6/),
    //Netscape, Firefox
    isGecko = !isWebKit && check(/gecko/),
    isGecko2 = isGecko && check(/rv:1\.8/),
    isGecko3 = isGecko && check(/rv:1\.9/),
    isGecko4 = isGecko && check(/rv:2\.0/),
    // others
    isWindows = check(/windows|win32/),
    isMac = check(/macintosh|mac os x/),
    isLinux = check(/linux/);

    var OP = Object.prototype;

    var ADD = ["toString", "valueOf"];

    var _hasOwnProperty = (OP.hasOwnProperty) ? function (o, prop) {
        return o && o.hasOwnProperty(prop);
    } : function (o, prop) {
        return !yardi.isUndefined(o[prop]) && o.constructor.prototype[prop] !== o[prop];
    };

    var _IEEnumFix = (isIE) ? function (r, s) {
        var i, fname, f;
        for (i = 0; i < ADD.length; i = i + 1) {
            fname = ADD[i];
            f = s[fname];
            if (yardi.isFunction(f) && f != OP[fname]) {
                r[fname] = f;
            }
        }
    } : function () { };

    // apis
    return {

        isOpera: isOpera,
        isOpera10_5: isOpera10_5,
        isChrome: isChrome,
        isWebKit: isWebKit,
        isSafari: isSafari,
        isSafari2: isSafari2,
        isSafari3: isSafari3,
        isSafari4: isSafari4,
        isIE: isIE,
        isIE7: isIE7,
        isIE8: isIE8,
        isIE9: isIE9,
        isIE6: isIE6,
        isGecko: isGecko,
        isGecko2: isGecko2,
        isGecko3: isGecko3,
        isGecko4: isGecko4,
        isWindows: isWindows,
        isLinux: isLinux,
        isMac: isMac,

        ieEnumFix: _IEEnumFix,

        hasOwnProperty: _hasOwnProperty,

        isUndefined: function (o) {
            return typeof o === 'undefined';
        },

        isArray: function (v) {
            return OP.toString.apply(v) === '[object Array]';
        },

        isFunction: function (o) {
            return OP.toString.apply(o) === '[object Function]';
        },

        isString: function (v) {
            return typeof v === 'string';
        },

        isBoolean: function (v) {
            return typeof v === 'boolean';
        },

        isNumber: function (v) {
            return typeof v === 'number' && isFinite(v);
        },

        setTimeout: function (func, args, time, scope) {
            return window.setTimeout(function () {
                func.apply(scope || window, args || []);
            }, time || 0);
        },

        setInterval: function (func, args, time, scope) {
            return window.setInterval(function () {
                func.apply(scope || window, args || []);
            }, time || 0);
        }
    };

} ();

yardi = function (yardi, $) {

    // custom html tag
    var defaultCustomTag = 'yardi';
    yardi.get = function (html, customTag) {
        customTag = customTag || defaultCustomTag;
        if (!yardi.isString(html) || html.indexOf('<' + customTag + ' ') === -1) {
            return $(html);
        } else {
            var doc = window.document;
            doc.createElement(customTag);
            var div = doc.createElement('div');
            doc.body.appendChild(div);
            div.innerHTML = html;
            var ret = $(div.childNodes);
            div.parentNode.removeChild(div);
            return ret;
        }
    };

    // amazing implement
    var ie = (function () {
        var undef, v = 3,
        div = document.createElement('div'),
        all = div.getElementsByTagName('i');
        while (div.innerHTML = '<!--[if gt IE ' + (++v) + ']><i></i><![endif]-->', all[0]);
        return v > 4 ? v : undef;
    } ());

    /*
    * extend object inherit function
    */
    yardi.extend = function (subc, superc, overrides) {
        var F = function () { }, i;
        F.prototype = superc.prototype;
        subc.prototype = new F();
        subc.prototype.constructor = subc;
        subc.superclass = superc.prototype;
        if (superc.prototype.constructor == Object.prototype.constructor) {
            superc.prototype.constructor = superc;
        }
        if (overrides) {
            for (i in overrides) {
                if (yardi.hasOwnProperty(overrides, i)) {
                    subc.prototype[i] = overrides[i];
                }
            }
            yardi.ieEnumFix(subc.prototype, overrides);
        }
    };

    /*
    * javascript template small engine.
    */
    var template = function (template, pattern) {
        this.template = String(template);
        this.pattern = pattern || template.pattern;
    };
    template.pattern = /#\{([^}]*)\}/mg;
    template.trim = String.trim || function (str) {
        return str.replace(/^\s+|\s+$/g, '');
    };
    template.prototype = {
        constructor: template,
        compile: function (object) {
            return this.template.replace(this.pattern, function (displace, variable) {
                variable = template.trim(variable);
                return displace = object[variable];
            });
        }
    };
    yardi.template = template;

    /*
    * condition monitor.
    */
    var monitor = function (config) {
        $.extend(this, config);
        this.initialize();
    };
    monitor.prototype = {
        scope: null,
        tester: null,
        handler: null,
        interval: null,
        constructor: monitor,
        initialize: function () {
            if (!this.interval) { this.interval = 50; }
            if (!this.scope) { this.scope = window; }
        },
        test: function () {
            var context = {};
            if (this.tester.call(this.scope, context) === true) {
                this.handler.call(this.scope, context);
            }
        },
        start: function () {
            this.stop();
            var self = this;
            this._id = setInterval(function () {
                self.test();
            }, this.interval);
            return this;
        },
        stop: function () {
            clearInterval(this._id);
            this._id = null;
        }
    };
    yardi.monitor = monitor;

    /*
    * custom event dispatcher
    */
    var dispatcher = function (scope) {
        this.scope = scope || this;
        this.listeners = [];
    };
    dispatcher.prototype = {
        scope: null,
        listeners: null,
        constructor: dispatcher,
        add: function (fn, scope) {
            var item = { fn: fn, scope: scope || this.scope };
            this.listeners.push(item);
            return item;
        },
        addToTop: function (fn, scope) {
            var item = { fn: fn, scope: scope || this.scope };
            this.listeners.unshift(item);
            return item;
        },
        remove: function (fn) {
            var cache;
            for (var i = 0; i < this.listeners.length; i++) {
                var item = this.listeners[i];
                if (fn == item.fn) {
                    cache = item;
                    this.listeners.splice(i, 1);
                }
            }
            return cache;
        },
        dispatch: function () {
            // needs to be a real loop since the listener count might change while looping, and this is also more efficient
            var result;
            for (var i = 0; i < this.listeners.length; i++) {
                var item = this.listeners[i];
                result = item.fn.apply(item.scope, arguments);
                if (result === false)
                    break;
            }
            return result;
        }
    };
    yardi.dispatcher = dispatcher;

    /*
    * get viewport size
    */
    yardi.getViewportSize = function (win) {
        var w = win || window, doc = w.document, docEl = doc.documentElement;
        var mode = doc.compatMode, width = w.self.innerWidth, height = w.self.innerHeight; // Safari, Opera
        // get
        if ((mode || yardi.isIE) && !yardi.isOpera) { // IE, Gecko
            height = (mode == 'CSS1Compat') ?
                        docEl.clientHeight : // Standards
                        doc.body.clientHeight; // Quirks
            width = (mode == 'CSS1Compat') ?
                        docEl.clientWidth : // Standards
                        doc.body.clientWidth; // Quirks
        }
        // ret
        return { width: width, height: height };
    };

    /*
    * get mouse position.
    */
    yardi.getMousePosition = function (e) {
        var result = { x: 0, y: 0 };
        if (!e) e = window.event;
        if (e.pageX || e.pageY) {
            result.x = e.pageX;
            result.y = e.pageY;
        } else if (e.clientX || e.clientY) {
            result.x = e.clientX + (document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft);
            result.y = e.clientY + (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop);
        }
        return result;
    };

    /*
    * parse color value to hex format.
    */
    yardi.colorHex = function (value) {
        if (yardi.isNumber(value)) {
            var c = value.toString(16).toUpperCase();
            switch (c.length) {
                case 1:
                    c = '0' + c + '0000';
                    break;
                case 2:
                    c = c + '0000';
                    break;
                case 3:
                    c = c.substring(1, 3) + '0' + c.substring(0, 1) + '00';
                    break;
                case 4:
                    c = c.substring(2, 4) + c.substring(0, 2) + '00';
                    break;
                case 5:
                    c = c.substring(3, 5) + c.substring(1, 3) + '0' + c.substring(0, 1);
                    break;
                case 6:
                    c = c.substring(4, 6) + c.substring(2, 4) + c.substring(0, 2);
                    break;
                default:
                    c = '';
            }
            return '#' + c;
        }
        if (yardi.isString(value)) {
            var toHex = function (num) {
                if (num == null)
                    return '00';
                num = parseInt(num);
                if (num == 0 || isNaN(num))
                    return '00';
                num = Math.max(0, num);
                num = Math.min(num, 255);
                num = Math.round(num);
                return '0123456789ABCDEF'.charAt((num - num % 16) / 16) + '0123456789ABCDEF'.charAt(num % 16);
            };
            var hex = '#';
            value = (value || '').toUpperCase();
            if (value == 'TRANSPARENT') {
                hex = 'transparent';
            } else if (value.indexOf('RGB') == 0) {
                value.replace(/\d{1,3}(,|\))/g, function (match) {
                    match = match.substr(0, match.length - 1);
                    hex += toHex(parseInt(match, 10));
                });
            } else if (value.indexOf('#') == 0) {
                hex = value;
            } else {
                hex += '000000'; //default
            }
            return hex;
        }
    };

    /*
    * size unit parser, support units: px,pt,em.
    */
    var sizeUnitParser = function (value, type) {
        type = (type || '').toLowerCase();
        this.oldValue = (value.toString() || '').toLowerCase();
        this.isPx = (this.oldValue.indexOf('px') != -1) || (type == 'px');
        this.isPt = (this.oldValue.indexOf('pt') != -1) || (type == 'pt');
        this.isEm = (this.oldValue.indexOf('em') != -1) || (type == 'em');
        this.value = parseFloat(this.oldValue.replace(/\D/, ''));
        this.hasType = (this.isPx || this.isPt || this.isEm);
    };
    sizeUnitParser.prototype = {
        value: 0, oldValue: null, hasType: false,
        isPx: false, isPt: false, isEm: false,
        constructor: sizeUnitParser,
        retNum: function (args) {
            return (args.length > 0 && args[0] === true);
        },
        toPx: function () {
            if (this.hasType === false)
            { throw new Error('parse size error!'); }
            var ret = 0;
            if (this.isPx) {
                ret = this.value;
            } else if (this.isPt) {
                ret = this.value * 4 / 3;
            } else if (this.isEm) {
                ret = this.value * 16;
            }
            return this.retNum(arguments) ? ret : ret + 'px';
        },
        toPt: function () {
            var px = this.toPx(true), ret = px * 3 / 4;
            return this.retNum(arguments) ? ret : ret + 'pt';
        },
        toEm: function () {
            var px = this.toPx(true), ret = px / 16;
            return this.retNum(arguments) ? ret : ret + 'em';
        }
    };
    yardi.sizeUnitParser = sizeUnitParser;

    /*
    * get flat position
    */
    yardi.flatPos = function (el, refEl) {
        var refOffset = refEl.offset();
        var refHeight = refEl.outerHeight(), refWidth = refEl.outerWidth();
        var selHeight = el.outerHeight(), selWidth = el.outerWidth();
        var winHeight = $(window).height(), winWidth = $(window).width();
        var scrollTop = $(window).scrollTop(), scrollLeft = $(window).scrollLeft();
        var left = 0, top = 0;
        if (refOffset.top + refHeight - scrollTop + selHeight > winHeight) {
            top = refOffset.top - selHeight - 1;
        } else {
            top = refOffset.top + refHeight + 1;
        }
        if (refOffset.left + refWidth - scrollLeft + selWidth > winWidth) {
            left = refOffset.left + refWidth - selWidth;
        } else {
            left = refOffset.left;
        }
        return { left: left, top: top };
    };

    /*
    * check if c element is a children of p element. (copy from extjs)
    */
    yardi.isAncestor = function (p, c) {
        var ret = false;
        if (p && c) {
            if (p.contains) {
                return p.contains(c);
            } else if (p.compareDocumentPosition) {
                return !!(p.compareDocumentPosition(c) & 16);
            } else {
                while (c = c.parentNode) {
                    ret = c == p || ret;
                }
            }
        }
        return ret;
    };

    /*
    * get the runtime style.
    */
    yardi.currentStyle = function (el, p) {
        var s = el.currentStyle || el.ownerDocument.defaultView.getComputedStyle(el, null);
        if (p === undefined) {
            return s;
        } else {
            p = p.replace(/(-[a-z])/gi, function (m, a) {
                return a.charAt(1).toUpperCase();
            });
            return s[p];
        }
    };

    /*
    * String extensions
    */
    String.prototype.trim = function () {
        var str = this, whitespace = ' \n\r\t\f\x0b\xa0\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200a\u200b\u2028\u2029\u3000';
        for (var i = 0, len = str.length; i < len; i++) {
            if (whitespace.indexOf(str.charAt(i)) === -1) {
                str = str.substring(i);
                break;
            }
        }
        for (i = str.length - 1; i >= 0; i--) {
            if (whitespace.indexOf(str.charAt(i)) === -1) {
                str = str.substring(0, i + 1);
                break;
            }
        }
        return whitespace.indexOf(str.charAt(0)) === -1 ? str : '';
    };
    if (!String.prototype.quote) {
        var metaObject = {
            '\b': '\\b',
            '\t': '\\t',
            '\n': '\\n',
            '\f': '\\f',
            '\r': '\\r',
            '\\': '\\\\'
        };
        String.prototype.quote = function () {
            str = this.replace(/[\x00-\x1f\\]/g, function (chr) {
                var special = metaObject[chr];
                return special ? special : '\\u' + ('0000' + chr.charCodeAt(0).toString(16)).slice(-4)
            });
            return '"' + str.replace(/"/g, '\\"') + '"';
        };
    }
    String.format = yardi.format = String.format || function (format) {
        var args = [], len = arguments.length;
        for (var i = 1; i < len; i++) {
            args.push(arguments[i]);
        }
        return format.replace(/\{(\d+)\}/g, function (m, i) {
            return args[i];
        });
    };

    /*
    * innerHTML, outerHTML
    */
    var rselfClosing = /^(?:area|br|col|embed|hr|img|input|link|meta|param)$/i;
    var outerHTML = function (el, recursion) {
        switch (el.nodeType + "") {
            case "1":
                var array = [];
                var nodeName = el.nodeName;
                if (recursion && yardi.currentStyle(el, "display") == "block") {
                    array.push("\n")
                }
                array.push("<" + nodeName);
                for (var i = 0, t; t = el.attributes[i++]; ) {
                    array.push(" " + t.name + "=" + (t.value || t.specified + "").quote())
                }
                if (rselfClosing.test(el.nodeName)) {
                    array.push("\/>")
                } else {
                    array.push(">");
                    for (var i = 0, c; c = el.childNodes[i++]; ) {
                        array.push(outerHTML(c, true))
                    }
                    array.push("<\/" + el.nodeName + ">")
                }
                return array.join("");
            case "3":
                return el.nodeValue.trim();
            case "8":
                return "innerHTML" in el ? el.innerHTML : "<!--" + el.nodeValue + "-->"
        }
    };
    var innerHTML = function (el) {
        var array = [];
        for (var i = 0, c; c = el.childNodes[i++]; ) {
            array.push(outerHTML(c, true))
        }
        return array.join("");
    };
    yardi.outerHTML = outerHTML;
    yardi.innerHTML = innerHTML;

    /*
    * resize event
    * fix  resize bug.
    * when scroll bar appeared, the window does not trigger the resize event
    */
    var resizeUtil = function () {
        var resizeCache = {};
        var dataKey = 'resize_datakey';
        return {
            hashCode: function (s) {
                var h = 31, i = 0, l = s.length;
                while (i < l)
                    h ^= (h << 5) + (h >> 2) + s.charCodeAt(i++);
                return h;
            },
            genKey: function (el, fn) {
                var elkey = el.data(dataKey);
                if (!elkey) {
                    elkey = Math.random();
                    el.data(dataKey, elkey);
                }
                var fnkey = fn ? this.hashCode(fn.toString()) : '';
                return elkey + '_' + fnkey;
            },
            register: function (el, fn) {
                if (!fn) { return; }
                var key = this.genKey(el, fn);
                if (resizeCache[key]) { return; }
                var cache = resizeCache[key] = {};
                // do wrap
                cache.wrap = function (o, f, c) {
                    return function () {
                        if (c.monitor) {
                            c.monitor.stop();
                            setTimeout(function () {
                                c.oldSize = { w: o.width(), h: o.height() };
                                c.monitor.start();
                            }, 128);
                        }
                        f.apply(this, arguments);
                    }
                } (el, fn, cache);
                // monitor
                (function (o, c) {
                    c.oldSize = { w: o.width(), h: o.height() };
                    c.monitor = new monitor({
                        scope: o,
                        interval: 32,
                        handler: c.wrap,
                        tester: function () {
                            var size = { w: this.width(), h: this.height() };
                            if (c.oldSize.w != size.w || c.oldSize.h != size.h) {
                                c.oldSize.w = size.w; c.oldSize.h = size.h;
                                return true;
                            }
                        }
                    });
                    c.monitor.start();
                })(el, cache);
            },
            unregister: function (el, fn) {
                var key = this.genKey(el, fn);
                for (var k in resizeCache) {
                    if (k.indexOf(key) !== 0)
                        continue;
                    var cache = resizeCache[k];
                    if (cache) {
                        cache.monitor.stop();
                        delete resizeCache[k];
                    }
                }
            }
        }
    } ();
    $.fn.monitorResize = function (fn) { this.each(function () { resizeUtil.register($(this), fn); }); };
    $.fn.unmonitorResize = function (fn) { this.each(function () { resizeUtil.unregister($(this), fn); }); };

    /*
    * test if page has scroll
    */
    yardi.hasScroll = function (el) {
        // test targets
        var elems = el ? [el] : [document.documentElement, document.body];
        var scrollX = false, scrollY = false;
        for (var i = 0; i < elems.length; i++) {
            var o = elems[i];
            // test horizontal
            var sl = o.scrollLeft;
            o.scrollLeft += (sl > 0) ? -1 : 1;
            o.scrollLeft !== sl && (scrollX = scrollX || true);
            o.scrollLeft = sl;
            // test vertical
            var st = o.scrollTop;
            o.scrollTop += (st > 0) ? -1 : 1;
            o.scrollTop !== st && (scrollY = scrollY || true);
            o.scrollTop = st;
        }
        // ret
        return {
            scrollX: scrollX,
            scrollY: scrollY
        };
    };

    /*
    * test the page scrollbar width
    */
    yardi.scrollBarWidth = function () {
        var helper = document.createElement('div');
        // fix opera bug: put style (position:absolute;top:-99999px;) to hide the test div, or the page would be flashed.
        helper.style.cssText = 'overflow:scroll;width:100px;height:100px;position:absolute;top:-99999px;';
        document.body.appendChild(helper);
        var ret = {
            vertical: helper.offsetWidth - helper.clientWidth,
            horizontal: helper.offsetHeight - helper.clientHeight
        };
        yardi.scrollBarWidth = function () { return ret; }
        document.body.removeChild(helper);
        return ret;
    };

    // detect touch device
    var touchDevice = false;
    $.each(['ontouchstart', 'ontouchend', 'ontouchmove', 'ongesturestart', 'createTouch'], function () {
        if (this in document || this in document.documentElement) {
            touchDevice = true;
            return false;
        }
    });
    if (!touchDevice) {
        // comment out: there were some changes in Chrome 17 and 18 that made this test no exception
        //try {
        //    var ev = document.createEvent('TouchEvent');
        //    if (ev) { touchDevice = true; }
        //} catch (e) { }
    }
    if (!touchDevice) {
        var ua = navigator.userAgent.toLowerCase();
        if (ua.match(/(iphone|ipod|ipad|android|blackberry)/)) { touchDevice = true; }
    }

    // jquery selection extensions
    if (!$.fn.disableSelection) {
        $.fn.disableSelection = function () {
            return this.attr('unselectable', 'on').css({ 'MozUserSelect': 'none', '-moz-user-select': 'none', '-webkit-user-select': 'none' }).bind('selectstart', function () { return false; });
        };
        $.fn.enableSelection = function () {
            return this.removeAttr('unselectable').css({ 'MozUserSelect': '', '-moz-user-select': '', '-webkit-user-select': '' }).unbind('selectstart');
        };
    }

    /*
    * simple drag component
    */
    var dragClass = function (el, config) {
        this.el = el;
        $.extend(this, config);
        this.initialize();
    };
    dragClass.prototype = {
        valid: function (ev) { }, start: function (ev) { }, move: function (ev) { }, end: function (ev) { },
        el: null, enable: false, prefix: '', _elMousedown: null, _docMouseup: null, _docMousemove: null,
        constructor: dragClass,
        initialize: function () {
            var self = this;
            if (touchDevice) { this.prefix = 'v'; }
            this._elMousedown = function (ev) {
                if (self.valid(ev) !== false) {
                    $('body').disableSelection();
                    $(document).bind(self.prefix + 'mouseup', self._docMouseup)
                               .bind(self.prefix + 'mousemove', self._docMousemove);
                    yardi.eventPropagation.subscribe(self.prefix + 'mouseup', self._docMouseup)
                                          .subscribe(self.prefix + 'mousemove', self._docMousemove);
                }
            };
            this._docMouseup = function (ev) {
                if (self.enable === true) {
                    self.enable = false;
                    self.end.call(self.el, ev);
                }
                $('body').enableSelection();
                $(document).unbind(self.prefix + 'mouseup', self._docMouseup)
                           .unbind(self.prefix + 'mousemove', self._docMousemove);
                yardi.eventPropagation.unsubscribe(self.prefix + 'mouseup', self._docMouseup)
                                      .unsubscribe(self.prefix + 'mousemove', self._docMousemove);
            };
            this._docMousemove = function (ev) {
                if (self.enable !== true) {
                    self.enable = true;
                    self.start.call(self.el, ev);
                }
                if (self.enable === true) {
                    self.move.call(self.el, ev);
                    return false;
                }
            };
            this.el.bind(this.prefix + 'mousedown', this._elMousedown);
        },
        destroy: function () {
            $(document).triggerHandler(this.prefix + 'mouseup');
            this.el.unbind(this.prefix + 'mousedown', this._elMousedown);
        }
    };
    yardi.dragClass = dragClass;

    /*
    * lazy functions invoker
    */
    var lazyFunc = function (config) {
        $.extend(this, config);
        this.initialize();
    };
    lazyFunc.prototype = {
        granularity: 1,
        funclist: null,
        executing: false,
        timeoutId: null,
        onComplete: null,
        constructor: lazyFunc,
        initialize: function () {
            this.funclist = [];
            this.onComplete = new dispatcher(this);
            this.granularity = Math.max(0, this.granularity);
        },
        add: function (fn) {
            this.funclist.push(fn);
        },
        clear: function () {
            clearTimeout(this.timeoutId);
            this.funclist = [];
        },
        apply: function () {
            if (this.executing !== true) {
                this.executing = true;
                this.loop(0);
            }
        },
        loop: function (start) {
            var end = Math.min(start + this.granularity, this.funclist.length);
            if (start === end) {
                this.executing = false;
                this.onComplete.dispatch();
                this.destroy();
                return;
            }
            this.timeoutId = setTimeout(function (self, s, e) {
                return function () {
                    for (var i = s; i < e; i++) { self.funclist[i](); }
                    self.loop(e);
                };
            } (this, start, end), 16);
        },
        destroy: function () {
            clearTimeout(this.timeoutId);
            this.funclist = [];
        }
    };
    yardi.lazyFunc = lazyFunc;

    /*
    * parse url
    */
    yardi.parseUrl = function (url) {
        var a = document.createElement('a');
        a.href = url;
        return {
            source: url,
            protocol: a.protocol.replace(':', ''),
            host: a.hostname,
            port: a.port,
            query: a.search,
            params: (function () {
                var ret = {},
                seg = a.search.replace(/^\?/, '').split('&'),
                len = seg.length, i = 0, s;
                for (; i < len; i++) {
                    if (!seg[i]) { continue; }
                    s = seg[i].split('=');
                    ret[s[0]] = s[1];
                }
                return ret;
            })(),
            file: (a.pathname.match(/\/([^\/?#]+)$/i) || [, ''])[1],
            hash: a.hash.replace('#', ''),
            path: a.pathname.replace(/^([^\/])/, '/$1'),
            relative: (a.href.match(/tps?:\/\/[^\/]+(.+)/) || [, ''])[1],
            segments: a.pathname.replace(/^\//, '').split('/')
        };
    };

    /*
    * manager of event propagation on specified types event
    */
    yardi.eventPropagation = function () {
        var types = ['click', 'dblclick', 'contextmenu', 'keydown', 'keypress', 'keyup', 'mousedown', 'mouseup', 'mousemove', 'mouseover', 'mouseout', 'mouseenter', 'mouseleave'];
        var subscribes = {}, slice = Array.prototype.slice, handler = function (ev) {
            ev.stopPropagation();
            var list = subscribes[ev.type];
            if (list) {
                var len = list.length, args = slice.call(arguments, 0);
                for (var i = 0; i < len; i++) { list[i].apply(this, args); }
            }
        };
        var execute = function (el, fn) {
            var len = types.length;
            for (var i = 0; i < len; i++)
            { el[fn](types[i], handler); }
        };
        return {
            stop: function (el) { execute(el, 'bind'); return this; },
            release: function (el) { execute(el, 'unbind'); return this; },
            subscribe: function (type, func) {
                var list = subscribes[type];
                if (!list) { list = subscribes[type] = []; }
                list.push(func); return this;
            },
            unsubscribe: function (type, func) {
                var list = subscribes[type];
                if (list) {
                    for (var i = list.length - 1; i > -1; i--)
                    { if (func == list[i]) { list.splice(i, 1); } }
                }
                return this;
            }
        };
    } ();

    /*
    * get the natural size of specified img
    */
    yardi.imgNaturalSize = function (img, callback) {
        if (img.naturalWidth) {
            callback({
                width: img.naturalWidth,
                height: img.naturalHeight
            });
        } else {
            var run = img.runtimeStyle;
            if (run) {
                var cachew = run.width;
                var cacheh = run.height;
                run.width = 'auto';
                run.height = 'auto';
                var width = img.width;
                var height = img.height;
                run.width = cachew;
                run.height = cacheh;
                callback({
                    width: width,
                    height: height
                });
            } else {
                var testee = new Image();
                testee.style.position = 'absolute';
                testee.style.visibility = 'hidden';
                testee.style.top = '-10000px'; // use negative number because positive number may cause scrollbar eg:opera10
                testee.style.left = '-10000px';
                testee.onload = testee.onerror = function (el, cb) {
                    return function () {
                        var width = el.width;
                        var height = el.height;
                        el.onload = el.onerror = null;
                        document.body.removeChild(el);
                        cb({
                            width: width,
                            height: height
                        });
                    };
                } (testee, callback);
                document.body.appendChild(testee);
                testee.src = img.src;
            }
        }
    };

    /*
    * get or set raw html to element.
    */
    yardi.rawHtml = function (elem, html, set) {
        set = set || {};
        elem = $(elem).get(0);
        if (html === undefined) {
            return elem.innerHTML;
        } else {
            // fix bug: set html to element innerHTML, when style or script tag is at the first position of html.
            // ie6 ie7 ie8 will remove the first style or script tag, so add a temporary span at front of the html.
            if ($.browser.msie && $.browser.version < 9) {
                elem.innerHTML = '<span>temp</span>' + html;
                elem.removeChild(elem.firstChild);
            } else {
                elem.innerHTML = html;
            }
            // process script
            if (set.processScript !== false) {
                yardi.processScript(elem, function () { });
            }
            // ret
            return elem;
        }
    };

    /*
    * process scripts in specified dom context
    */
    yardi.processScript = function (context, callback) {
        // query scripts
        var exeScripts = [], scripts = context.getElementsByTagName('script');
        for (var i = 0; i < scripts.length; i++) {
            // filter marker
            if (!scripts[i].KOOBOOADM_IGNORE) {
                exeScripts.push(scripts[i]);
            }
        }
        if (!exeScripts.length) {
            callback();
            return;
        }
        // marker
        var elem = exeScripts[0];
        elem.KOOBOOADM_IGNORE = true;
        // hacks
        var write = document.write, writeln = document.writeln;
        var parent = elem.parentNode, next = elem.nextSibling;
        var temp = document.createElement('div'); document.body.appendChild(temp);
        temp.style.cssText = 'width:0px;height:0px;position:absolute;top:-99999px;';
        document.writeln = function (text) { document.write(text + ' '); };
        document.write = function (text) {
            temp.innerHTML = text;
            for (var i = 0; i < temp.childNodes.length; i++) {
                if (next) {
                    parent.insertBefore(temp.childNodes[0], next);
                } else {
                    parent.appendChild(temp.childNodes[0]);
                }
            }
        };
        // execute end handler
        var complete = function () {
            // restore hack
            document.write = write;
            document.writeln = writeln;
            document.body.removeChild(temp);
            // do loop. process one by one, and query all script nodes each time.
            // because that each script may create other script nodes when loaded.
            yardi.processScript(context, callback);
        };
        // execute by jQuery
        if (elem.src) {
            $.ajax({ url: elem.src, async: true, dataType: 'script', complete: complete });
        } else {
            $.globalEval(elem.text || elem.textContent || elem.innerHTML || ''); setTimeout(complete, 0);
        }
    };

    // ret
    return yardi;

} (yardi, jQuery);
