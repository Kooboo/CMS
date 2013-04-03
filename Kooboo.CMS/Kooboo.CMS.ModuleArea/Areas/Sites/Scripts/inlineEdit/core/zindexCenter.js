/*
*   zindexCenter
*   author: ronglin
*   create date: 2012.4.9
*   description: a manager singleton register to top scope for z-index css property
*/

(function ($, undefined) {

    var isWindowExist = function (win) {
        var exist = true;
        try {
            var o = win.document.body.childNodes; // try to access
        } catch (ex) {
            exist = false;
        }
        return exist;
    };

    var zindexCenter = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    zindexCenter.prototype = {

        mapping: null, winUnload: null,

        constructor: zindexCenter,

        initialize: function () {
            var self = this;
            this.mapping = [];
            this.winUnload = function () {
                var win = this;
                setTimeout(function () {
                    self.refresh(win);
                }, 32);
            };
        },

        refresh: function (win) {
            var len = this.mapping.length, item, equal, exist;
            for (var i = len - 1; i > -1; i--) {
                item = this.mapping[i];
                equal = (item.win === win);
                exist = isWindowExist(item.win);
                if (exist && equal) {
                    $(item.win).unbind('unload', this.winUnload);
                }
                if (!exist || equal) {
                    this.mapping.splice(i, 1);
                }
            }
        },

        refreshAll: function () {
            var len = this.mapping.length, item;
            for (var i = len - 1; i > -1; i--) {
                item = this.mapping[i];
                if (isWindowExist(item.win)) {
                    $(item.win).unbind('unload', this.winUnload);
                }
            }
            this.mapping = [];
        },

        queryWinZMax: function (win) {
            var zmax, cur;
            $(win.document.body).find('*').each(function () {
                cur = parseInt($(this).css('z-index'));
                zmax = cur > (zmax || 0) ? cur : zmax;
            });
            return zmax || 0;
        },

        getDomOwner: function (el) {
            // scopes
            var o = $(el), dom = o.get(0), doc = dom.ownerDocument;
            var win = doc.parentWindow || doc.defaultView; // ie and opera has 'parentWindow' others 'defaultView'
            // ret
            return win;
        },

        getMax: function (win, set) {
            // if newest
            set = set || {};
            if (set.newest === true) {
                return this.queryWinZMax(win);
            }
            // query cache
            var ret = undefined;
            $.each(this.mapping, function () {
                if (this.win === win) {
                    ret = this.ret;
                }
            });
            if (ret !== undefined) {
                return ret;
            }
            // query max z-index
            ret = this.queryWinZMax(win);
            // some browser plugin use a z-index:2147483647(max integer number of 32bit platform).
            // when this function increase z-index base on this number, it may cause number overflow issue.
            var threshold = 2147483647 - 10000; // leave 10000 for our component used.
            ret = (ret >= threshold) ? threshold : ret;
            // store cache
            this.mapping.push({
                win: win,
                ret: ret
            });
            // bind refresh
            if (win !== top) {// the singleton is register in top window, so top window don't need to do self refreshing.
                $(win).bind('unload', this.winUnload);
            }
            // ret
            return ret;
        },

        restoreOld: function (el) {
            var o = $(el), zold = o.attr('zold');
            if (!zold) {
                return o;
            } else {
                return o.css('z-index', (parseInt(zold) || 0)).removeAttr('zold');
            }
        },

        setTop: function (el, set) {
            // get
            var win = this.getDomOwner(el);
            var max = this.getMax(win, set);
            // apply
            this.restoreOld(el);
            var o = $(el), old = parseInt(o.css('z-index')) || 0;
            return o.attr('zold', old).css('z-index', old + max);
        },

        remove: function () {
            this.refreshAll();
            this.mapping = undefined;
            this.winUnload = undefined;
        }
    };

    // register
    var singleton = top.zindexCenter;
    if (!singleton) {
        singleton = top.zindexCenter = new zindexCenter();
        $(top).bind('unload', function () { singleton.remove(); });
    }
    if (window.yardi) {
        yardi.zindexCenter = singleton;
        yardi.zTop = function (el) { return singleton.setTop(el); }
        yardi.zOld = function (el) { return singleton.restoreOld(el); }
    }

})(jQuery);
