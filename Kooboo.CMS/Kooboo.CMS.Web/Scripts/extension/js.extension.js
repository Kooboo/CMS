(function () {
    /*---
    String Extension
    */
    String.prototype.trim = function () {
        return this.replace(/(^\s*)|(\s*$)/g, "");
    }

    String.prototype.trimAll = function () {
        return this.replace(/\s*/g, '');
    }

    String.prototype.toLower = String.prototype.toLowerCase;

    String.prototype.toUpper = String.prototype.toUpperCase;

    //Object.prototype.trim = String.prototype.trim;

    //Object.prototype.toUpper = String.prototype.toUpper;

    //Object.prototype.toLower = String.prototype.toLower;



    String.prototype.localize = (function () {

        var languageResource = (function getLanguageResource() {
            /*
            DO SOME THING TO GET LANGUAGE Resource
            */
            return new Array();
        })();

        return function () {
            if (languageResource[this]) {
                return languageResource[this]
            }
            return this;
        }
    });
    //end string extension

    /*---
    Array Extension
    */
    Array.prototype.each = function (func) {
        for (var i = 0; i < this.length; i++) {
            if (func(this[i], i) == false) {
                break;
            }
        }
        return this;
    }

    Array.prototype.where = function (func) {
        var results = [];
        this.each(function (value, index) {
            if (func(value, index) == true) {
                results.push(value);
            }
        });
        return results;
    }

    Array.prototype.first = function () {
        if (this.length == 0) {
            return null;
        }
        return this[0];
    }

    Array.prototype.last = function () {
        if (this.length == 0) {
            return null;
        }
        return this[this.length - 1];
    }

    Array.prototype.take = function (num) {
        var result = [];
        for (var i = 0; i < num; i++) {
            result.push(this[i]);
        }
        return result;
    }

    Array.prototype.skip = function (num) {
        var result = [];
        for (var i = num; i < this.length; i++) {
            result.push(this[i]);
        }
        return result;
    }

    Array.prototype.select = function (func) {
        var results = [];
        this.each(function (value, index) {
            results.push(func(value, index));
        });
        return results;
    }
    Array.prototype.remove = function (func) {

        var toRemove = [];

        this.each(function (val, index) {
            if (func(val, index)) { toRemove.push(index); }
        });
        var arr = this;
        toRemove.reverse().each(function (val) { arr.splice(val, 1) });
        return arr;
    }
    Array.prototype.removeElement = function (elment) {
        return this.remove(function (value, index) { return value == elment; });
    }
    Array.prototype.removeAt = function (index) {
        return this.remove(function (value, index) { return index == index; });
    }
    Array.prototype.indexOf = Array.prototype.indexOf || function (element) {
        var i = -1;
        this.each(function (value, index) {
            if (value == element) {
                i = index;
                return false;
            }
        });

        return i;
    }
    Array.prototype.contain = function (element) {
        return this.indexOf(element) >= 0;
    }
    //end Array Extension

    function HashTable() {
        var hash = new Array();

        this.add = function (key, value) {
            hash[key] = value;
            return this;
        }
        this.remove = function (key) {
            hash[key] = null;
            hash.splice(key, 1);
            return this;
        }
    }

    var hash = new HashTable();
    hash.add();
})();


(function () {
    var dateFormat = function () {
        var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
		timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
		timezoneClip = /[^-+\dA-Z]/g,
		pad = function (val, len) {
		    val = String(val);
		    len = len || 2;
		    while (val.length < len) val = "0" + val;
		    return val;
		};

        // Regexes and supporting functions are cached through closure
        return function (date, mask, utc) {
            var dF = dateFormat;

            // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
            if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
                mask = date;
                date = undefined;
            }

            // Passing date through Date applies Date.parse, if necessary
            date = date ? new Date(date) : new Date;
            if (isNaN(date)) throw SyntaxError("invalid date");

            mask = String(dF.masks[mask] || mask || dF.masks["default"]);

            // Allow setting the utc argument via the mask
            if (mask.slice(0, 4) == "UTC:") {
                mask = mask.slice(4);
                utc = true;
            }

            var _ = utc ? "getUTC" : "get",
			d = date[_ + "Date"](),
			D = date[_ + "Day"](),
			m = date[_ + "Month"](),
			y = date[_ + "FullYear"](),
			H = date[_ + "Hours"](),
			M = date[_ + "Minutes"](),
			s = date[_ + "Seconds"](),
			L = date[_ + "Milliseconds"](),
			o = utc ? 0 : date.getTimezoneOffset(),
			flags = {
			    d: d,
			    dd: pad(d),
			    ddd: dF.i18n.dayNames[D],
			    dddd: dF.i18n.dayNames[D + 7],
			    m: m + 1,
			    mm: pad(m + 1),
			    mmm: dF.i18n.monthNames[m],
			    mmmm: dF.i18n.monthNames[m + 12],
			    yy: String(y).slice(2),
			    yyyy: y,
			    h: H % 12 || 12,
			    hh: pad(H % 12 || 12),
			    H: H,
			    HH: pad(H),
			    M: M,
			    MM: pad(M),
			    s: s,
			    ss: pad(s),
			    l: pad(L, 3),
			    L: pad(L > 99 ? Math.round(L / 10) : L),
			    t: H < 12 ? "a" : "p",
			    tt: H < 12 ? "am" : "pm",
			    T: H < 12 ? "A" : "P",
			    TT: H < 12 ? "AM" : "PM",
			    Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
			    o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
			    S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
			};

            return mask.replace(token, function ($0) {
                return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
            });
        };
    } ();

    // Some common format strings
    dateFormat.masks = {
        "default": "ddd mmm dd yyyy HH:MM:ss",
        shortDate: "m/d/yy",
        mediumDate: "mmm d, yyyy",
        longDate: "mmmm d, yyyy",
        fullDate: "dddd, mmmm d, yyyy",
        shortTime: "h:MM TT",
        mediumTime: "h:MM:ss TT",
        longTime: "h:MM:ss TT Z",
        isoDate: "yyyy-mm-dd",
        isoTime: "HH:MM:ss",
        isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
        isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
    };

    // Internationalization strings
    dateFormat.i18n = {
        dayNames: [
		"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
		"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
	],
        monthNames: [
		"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
		"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
	]
    };

    // For convenience...
    Date.prototype.format = function (mask, utc) {
        if ($.datepicker.regional) {
            mask = $.datepicker._defaults.dateFormat;
        }
        return dateFormat(this, mask, utc);
    };
})();