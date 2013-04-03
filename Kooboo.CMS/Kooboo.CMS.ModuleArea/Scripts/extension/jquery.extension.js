/// <reference path="../jquery.js" />
/// <reference path="../jquery-ui.js" />

///jQuery extension
(function ($) {
    var jQuery = $;
    if (!$) {
        return false;
    }

    $.fn.extend({
        isString: function () {
            return typeof (this) == 'string';
        },
        isFunction: function () {
            return typeof (this) == 'function';
        },
        isObject: function () {
            return typeof (this) == 'object';
        }
    }); //end jQuery.fn.extend

    (function popup_ext() {
        ///-------------jQuery.fn.pop------------
        var __apiMap = {};
        var __configMap = {};
        var __divStr = "<div></div>";
        var __iframe = '<iframe  frameborder="0" allowTransparency="true"/>';

        $.fn.pop = function (option) {
            this.each(function () {
                var handle = $(this), cfg = $.extend({}, option);
                if (handle.attr('pop-config')) {
                    var popConfig = eval('(' + handle.attr('pop-config') + ')');
                    $.extend(cfg, popConfig);
                }
                koobooPop.call(handle, option);
            });
            return this;
        }

        var koobooPop = function (option) {
            var config = {
                id: '',
                url: '', // url will being getted from href prior
                title: '', // you dont need to config this if you link element has title attribute
                isIframe: true,
                useUrl: true, // 
                useContent: false,
                contentId: '', // may be a function like function(handle,pop,config){  return contentID ;};
                useClone: false, //this will clone pop content every time

                events: 'click',

                maxAble: true,

                cloneId: 'pop-clone-' + Math.random().toString().replace('.', '-'),

                reload: true, // reload content every time when open

                popupOnTop: false, // show pop on parent page

                openOnclick: true,

                allowTransparency: true,
                frameborder: '0',
                border: '0',

                frameWidth: '100%',
                frameHeight: '100%',
                unbindEvents: 'click',

                width: 780, //jquery dialog option
                height: 560, //jquery dialog option
                show: 'fade', //jquery dialog option
                autoOpen: false, //jquery dialog option
                modal: true, //jquery dialog option
                closeOnEscape: false, //close on esc

                onopen: function (currentHandle, pop, config) { },
                beforeLoad: function (currentHandle, pop, config) { }, // before pop content loaded
                onload: function (currentHandle, pop, config) { }, // on pop content loaded 
                onclose: function (currentHandle, pop, config) { } // on pop closed
            };

            if ($(option).isString()) {
                config.id = option;
            } else if ($(option).isObject()) {
                $.extend(config, option);
            }

            config.id = (config.id != '' ? config.id : Math.random().toString().replace('.', '_'));
            if (__apiMap[config.id]) {
                return __apiMap[config.id];
            }

            var current = this;
            current.data("POPHANDLE", true);
            var pop = $(__divStr);
            config.close = function () {

                config.onclose(current, pop, config);
                pop.find('iframe').attr('src', 'about:blank');
                $.popContext.popList.pop();
            }
            var topJQ = top._jQuery || top.jQuery;
            if (config.popupOnTop && topJQ) {
                pop = topJQ(__divStr).appendTo(top.document.getElementsByTagName("body").item(0));
            } else {
                pop.appendTo("body");
            }

            pop.attr('id', config.id);
            pop.hide();

            if (this.attr('title') && this.attr('title').length > 0) {
                config.title = this.attr('title');
            }
            pop.dialog(config);
            if (config.maxAble) {
                pop.dialogExtend({
                    "maximize": true,
                    "dblclick": "maximize",
                    "icons": {
                        "maximize": "ui-icon-extlink",
                        "restore": "ui-icon-newwin"
                    },
                    events: {
                        restore: function (evt, dlg) {
                            $(dlg).dialog("option", "position", 'center');
                        }
                    }
                });
            }
            if (config.openOnclick) {
                function showPop() {
                    show();
                    $.popContext.popList.push(pop);
                    pop.dialog('open').dialog("option", "position", 'center');
                }
                this.unbind(config.unbindEvents).keydown(function () {
                    showPop();
                    return false;
                }).mousedown(function (e) {
                    if (e.which != 3) {
                        showPop()
                    }
                    return false;
                })
            .bind("contextmenu", function () { return false; }).click(function () {
                showPop();
                return false;
            });
            } else {
                show();
                $.popContext.popList.push(pop);
            }
            function loadContent() {
                config.beforeLoad(current, pop, config);
                if (current.attr('href')) {
                    config.url = current.attr('href');
                }

                current.data('loaded', true);
                if (config.url == '') {
                    if (current.attr('url') && current.attr('url').length > 0) {
                        config.url = current.attr('url');
                    } else if (current.attr('href') && current.attr('href').length > 0) {
                        config.url = current.attr('href');
                    } else if (current.attr('src') && current.attr('src').length > 0) {
                        config.url = current.attr('src');
                    }
                }

                if (config.url != '' && config.useUrl && !config.useContent && !config.useText) {
                    if (!config.isIframe) {
                        $.ajax({
                            url: config.url,
                            success: function (data) {
                                pop.html(data);
                                config.onload(current, pop, config);
                                config.onopen(current, pop, config);
                            },
                            error: function () {
                                pop.html('some errors occur during request!');
                            }
                        });
                    } else {
                        var iframe = $();

                        if (pop.find('iframe').length == 0) {
                            iframe = $(__iframe).appendTo(pop);
                            iframe.css('width', config.frameWidth);
                            iframe.attr('frameborder', config.frameborder);
                            iframe.attr('allowTransparency', config.allowTransparency);

                            iframe.attr('border', config.border);
                            iframe.attr('src', config.url);
                            iframe.attr('id', "pop_iframe" + config.id);

                            if (config.frameConfig) {
                                iframe.attr(config.frameConfig);
                            }

                            if (config.height <= 200) {

                                iframe.css("height", "90%");
                            }

                            if (config.frameHeight == 'auto') {
                                var loadCount = 0;

                                iframe.load(function () {

                                    var height = iframe.contents().height();
                                    iframe.height(height);
                                    loadCount++;
                                });
                            } else {
                                iframe.height(config.frameHeight);
                            }
                            iframe.load(function () {
                                var $fromPop = iframe.contents().find("input:hidden[name=FromPop]");
                                if ($fromPop.length == 0) {
                                    $fromPop = $('<input type="hidden" name="FromPop" value="true"/>').appendTo(iframe.contents().find("form"));
                                }
                                pop.iframe = iframe;
                                config.onload(current, pop, config);
                                config.onopen(current, pop, config);
                            });
                        } else {
                            iframe = pop.children('iframe');
                            iframe.attr('src', config.url);
                            config.onload(current, pop, config);
                            config.onopen(current, pop, config);
                        }
                    }
                } else if (config.useContent) {
                    if (typeof (config.contentId) == 'function') {
                        config.contentId = config.contentId(current, pop, config);
                    }
                    var source = $('#' + config.contentId);
                    if (config.useClone) {
                        source = source.clone();

                        if (typeof (config.cloneId) == 'function') {
                            source.attr('id', config.cloneId(source));
                            source.find('[id]').each(function () {
                                $(this).attr('id', config.cloneId($(this)));
                            });

                        } else {
                            var sid = source.attr('id');
                            source.attr('id', sid + config.cloneId);
                            source.find('[id]').each(function () {
                                var id = $(this).attr('id');
                                $(this).attr('id', id + '-' + config.cloneId);
                            });
                        }
                        source.appendTo(pop).show();
                    } else {
                        if (!current.data('pop-content-loaded')) {
                            source.appendTo(pop).show();
                            current.data('pop-content-loaded', true);
                        }
                    }
                    config.onload(current, pop, config);
                }
            }
            pop.close = close;
            function close() {
                pop.dialog('close');
            }
            function destory() {
                try {
                    CollectGarbage(); //
                } catch (E) {
                }
            }
            pop.destory = function () {
                pop.dialog("destory");
            }
            pop.open = function () {
                pop.dialog('open');
            }
            function show() {

                if (config.reload || !current.data('loaded')) {
                    if (current.attr('href')) {
                        config.url = current.attr('href') || config.url;
                    }
                    loadContent();
                }
            }


            var api = {};
            api.show = show;
            api.reload = loadContent;
            api.close = close;
            api.destory = destory;
            api.getIframe = function () {
                ///return juery object
                return pop.children('iframe');
            }
            api.$ = pop.dialog();
            __apiMap[config.id] = api;
            return api;
        }

        ///TODO new method , the order one is too much complex , i dont like it
        function popDialog(option) {
            if (!(this instanceof popDialog)) {
                return new popDialog(option);
            }

            function init() {

            }

            function create() {

            }

            $.extend(this, {
                close: function () { },
                open: function () { },
                reload: function () { },
                destory: function () { }
            });
        }

        var topJQ = top._jQuery || top.jQuery;
        if (topJQ && topJQ.popContext) {
            $.popContext = topJQ.popContext;
        } else {
            $.popContext = {
                popList: [],
                getCurrent: function () {
                    if (this.popList.length > 0) {
                        return this.popList[this.popList.length - 1];
                    }
                    return null;
                }
            };
        }

        $.pop = function (option) {
            option = option || {};
            option.autoOpen = true;
            var handle = $('<a></a>');
            if (option.url) {
                handle.attr('href', option.url);
            }

            var ret = handle.pop(option);

            setTimeout(function () {
                handle.click();
            }, 10);

            return ret;
        };
        $(function () {
            //regist esc key 
            $(window).keyup(function (e) {
                if (e.keyCode == 27) {
                    if (!$(e.target).is('input,textarea,select')) {
                        var cur = $.popContext.getCurrent();
                        if (cur != null) {
                            cur.close();
                        }
                    }
                }

            });
        });
    })();
    ///end --- jQuery.fn.pop----------------

    $.request = (function () {
        var apiMap = {};

        function request(queryStr) {
            var api = {};

            if (apiMap[queryStr]) {
                return apiMap[queryStr];
            }

            api.queryString = (function () {
                var urlParams = {};
                var e,
                d = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); },
                q = queryStr.substring(queryStr.indexOf('?') + 1),
                r = /([^&=]+)=?([^&]*)/g;

                while (e = r.exec(q))
                    urlParams[d(e[1])] = d(e[2]);


                return urlParams;
            })();

            api.getRequest = function (key) {
                if (!key) {
                    return null;
                }
                for (var q in api.queryString) {
                    if (q.toLowerCase() == key.toLowerCase()) {
                        return api.queryString[q];
                    }
                }
                return null;
            }
            api.setRequest = function (key, value) {
                api.queryString[key] = value;
            }

            api.getUrl = function () {
                var url = queryStr.indexOf('?') > 0 ? queryStr.substring(0, queryStr.indexOf('?') + 1) : queryStr;

                for (var p in api.queryString) {
                    url += p + '=' + api.queryString[p] + "&";
                }

                if (url.lastIndexOf('&') == url.length - 1) {
                    return url.substring(0, url.lastIndexOf('&'));
                }
                return url;
            }

            apiMap[queryStr] = api;
            return api;
        }

        $.extend(request, request(window.location.href));

        return request;

    })();

    $.fn.koobooTab = function (option) {
        var config = {
            containerClass: "tab-content",
            currentClass: "current",
            tabClass: "tab-content",
            event: "click",
            tabContentTag: "<div></div>",
            showTabIndex: 0,
            preventDefault: true
        };

        var tabContents = this.find('.' + config.tabClass).hide();
        var tabMap = [];
        $.extend(config, option);
        var ul = this.children("ul");

        var index = 0;
        ul.find("li a").each(function (val) {
            var current = $(this);
            var tabApi = initTab(current, index);
            index++;
        });

        function initTab(current, index) {
            var href = current.attr("href");
            if (href.indexOf("#") == 0) {
                current.bind(config.event, function (e) {
                    document.location.hash = href.substring(href.indexOf('#'));
                    api.show();
                    if (config.preventDefault) {
                        e.preventDefault();
                    }
                });
            }

            var api = {
                show: function () {
                    var expire = /#\S+$/;
                    var id = expire.exec(href);
                    if (id && id[0]) {
                        tabContents.hide();
                        current.parent().siblings().removeClass(config.currentClass);
                        current.parent().addClass(config.currentClass);
                        $(id[0]).show();
                    }

                }
            };
            tabMap.push({ id: href, handle: current, tab: api, index: index });
            return api;
        }

        var api = {
            showTab: function (tab) {
                tab = tab == undefined ? 0 : tab;
                var tabObj;
                if (typeof (tab) == 'string') {
                    tabObj = tabMap.where(function (o) { return o.id == tab; }).first();
                } else if (!isNaN(tab)) {
                    tabObj = tabMap.where(function (val, index) { return index == tab; }).first();
                }
                tabObj = tabObj ? tabObj : tabMap.first();
                tabObj.tab.show();
            }
        };

        this.data('koobooTab', api);

        function getAddressTab() {
            var url = $.request.getUrl();
            var expire = /#\w+$/;
            var tab = config.showTabIndex;

            if (expire.test(url)) {
                var tabId = expire.exec(url);
                tab = tabId[0];
            }
            return tab;
        }

        $(function () {


            api.showTab(getAddressTab());

            $(window).bind('onhashchange', function () {
                api.showTab(getAddressTab());
            });

        });

        return this;
    }

    $.fn.addHidden = function (name, val, fieldSet) {
        if (this.find('[name="' + name + '"]').length > 0) {
            this.find('[name="' + name + '"]').val(val);
            return this;
        }
        var hiddenInput = $('<input type="hidden"/>').val(val).attr("name", name);

        if (fieldSet) {
            var fieldSet = this.find("fieldset[name=" + fieldSet + "]");
            if (fieldSet.length > 0) {
                fieldSet.append(hiddenInput);
            } else {
                fieldSet = $("<fieldset></fieldset>").attr("name", fieldSet).appendTo(this);
            }
        }
        this.append(hiddenInput);
    }

    $.fn.setFormField = function (name, val) {
        this.find("[name=" + name + "]").val(val);
    }

    $.fn.tableSorter = function (optoin) {

        var config = {
            items: "tr",
            dragable: false,
            cancel: ".none-cancel",
            divInsertAt: 1,
            beforeUp: function () { },
            beforeDown: function () { },
            up: function (handle) { },
            down: function (handle) { },
            move: function (handle) { },
            showUp: function (handle) { },
            showDown: function (handle) { }
        };
        $.extend(config, optoin);
        var sorter = $();
        function createCursorNearDiv(handle, left, cfg) {
            handle = $(handle);
            cfg = config;

            sorter = $('<span><a href="javascript:" class="o-icon move-up inline-action">Move up</a> <a href="javascript:" class="o-icon move-down inline-action">Move up</a>');

            sorter.appendTo(handle.find("td:eq(" + cfg.divInsertAt + ")"));

            var up = sorter.find("a.move-up");
            var down = sorter.find("a.move-down");
            var move = sorter.find("a.move");

            if (cfg.showUp(handle) == false) {
                up.hide();
            }
            if (cfg.showDown(handle) == false) {
                down.hide();
            }
            if (cfg.dragable == false) {
                move.hide();
            }

            up.click(function () {
                if (cfg.beforeUp(handle) == false) {
                    return false;
                }
                var prev = handle.prev();
                if (prev.length > 0) {
                    prev.before(handle);
                } else {
                    //return false;
                }
                cfg.up(handle);
                cfg.move();
                sorter.remove();
            });


            down.click(function () {
                if (cfg.beforeDown(handle) == false) {
                    return false;
                }
                var next = handle.next();
                if (next.length > 0) {
                    next.after(handle);
                } else {
                    //return false;
                }
                cfg.down(handle);
                cfg.move();
                sorter.remove();
            });
        }

        var table = $(this);
        var items = table.find("tr:not(" + config.cancel + ")");
        items.hover(function (e) {
            var handle = $(this);
            createCursorNearDiv(handle, e.clientX + 30, config);
        }, function () {
            sorter.remove();
        });

        function initIndex(items) {
            items.each(function (val, index) {
                $(this).data("tableSortIndex", index);
            });
        }
    };

    $.loadStyle = function (url, attr) {
        $('<link />').attr({
            type: 'text/css',
            rel: 'stylesheet',
            href: url
        }).appendTo('head');
    };

    //#region   dropdownlist
    (function multiple_link_dropdown_list() {
        function linkageSelect(option) {
            if (!(this instanceof linkageSelect)) {
                return new linkageSelect(option);
            }
            var handle = $(option.handle), data = option.data, url = option.url,
            sub = $(option.sub), getSub = function () { return $(option.sub); }, subval = option.subval, _self = this, valueChangEvent = 'value-change', initAfterReady = option.initAfterReady == undefined ? true : option.initAfterReady,
            postName = option.dataname || 'value',
            enabled = true,
            cacheNamespace = function (dataName) { return dataName + '-' + 'data' },
            bindsub = function (datasource) {
                if (datasource instanceof Array) {
                    getSub().dropdownList(datasource, option);
                    _self.subval();
                    getSub().each(function () {
                        var s = $(this);
                        if (s.hasEvent(valueChangEvent)) {
                            s.trigger(valueChangEvent);
                        }
                    });
                }

            };
            handle.bind(valueChangEvent, function () {
                var val = handle.val();
                if (handle.data(cacheNamespace(url + val))) {
                    bindsub(handle.data(cacheNamespace(url + val)));
                } else {
                    var postData = {};
                    postData[postName] = val;
                    $.post(url, postData, function (response) {
                        handle.data(cacheNamespace(url + val), response);
                        bindsub(response);
                    });
                }
            });

            handle.change(function () {
                if (enabled) { $(this).trigger(valueChangEvent); }
            });

            this.change = function (value) {
                handle.val(value || handle.val());
                handle.trigger(valueChangEvent);
                return this;
            };

            this.initSub = function () {
                handle.trigger(valueChangEvent);
                return this;
            }

            this.subval = function (val) {
                subval = val || subval;
                if (subval != undefined) {
                    sub.val(subval);
                }
                return val == undefined ? subval : this;
            };

            this.destory = function () {

            };

            this.disable = function () {
                enabled = false;
            };

            if (initAfterReady) {
                handle.trigger(valueChangEvent);
            }
        }

        $.fn.linkageSelect = function (cfg) {
            this.each(function () {
                var handle = $(this);
                if (!handle.data('linkageSelect')) {
                    var option = $.extend({
                        handle: handle,
                        url: handle.attr('url'),
                        sub: handle.attr('sub'),
                        dataname: handle.attr('dataname')
                    }, cfg);
                    handle.data('linkageSelect', linkageSelect(option));
                }
            });
            return this;
        }

    })();
    (function dropdownListTree() {
        function dropdownList(handle, datasource, option) {
            if (!(this instanceof dropdownList)) {
                return new dropdownList(handle, datasource, option);
            }
            var config = $.extend({ value: 'Value', text: 'Text', items: 'Items', pre: '&nbsp;&nbsp;&nbsp;&nbsp;' }, option),
            optionStr = '<option/>',
            generateOption = function (dataItem, level) {
                if (level == undefined) { level = 0; }
                var option = $(optionStr), item = getDataItem(dataItem);
                option.html(getText(level, config.pre, item.text)).val(item.value);
                handle.append(option);
                if (item.items && item.items.length > 0) {
                    level++;
                    for (var i = 0; i <= item.items.length; i++) {
                        if (item.items[i])
                            generateOption(item.items[i], level);
                    }
                }

            },
            getText = function (level, pre, text) {
                var result = '';
                for (var i = 0; i < level; i++) {
                    result += pre;
                }
                return result + text;
            },
            getDataItem = function (item) {
                return {
                    value: item[config.value],
                    text: item[config.text],
                    items: item[config.items]
                };
            },
            cacheValue = {},
            html;
            $.extend(this, {
                bind: function (datasource) {
                    bind(datasource);
                },
                clear: function () {
                    handle.html('');
                }
            });
            function bind(ds) {
                handle.html('');
                $(ds).each(function (index, item) {
                    generateOption(item, 0);
                });
                if (typeof config.valueProvider == 'function') {
                    handle.val(config.valueProvider(handle));
                } else {
                    handle.val(handle.attr('selectedValue'));
                }
            }
            bind(datasource);
        }
        $.fn.dropdownList = function (datasource, option) {
            this.each(function () {
                var handle = $(this), dataName = 'dropdownListTree';
                if (!handle.data(dataName)) {
                    handle.data(dataName, dropdownList(handle, datasource, option));
                } else {
                    if (datasource instanceof Array) {
                        handle.data(dataName).bind(datasource);
                    } else if (!datasource) {
                        handle.data(dataName).clear();
                    }
                }
            });
            return this;
        }
    })();
    //#endregion

    $.fn.hasEvent = function (event) { var events = this.data("events"); return (events && events[event]) };

    //#region fixedSubmit
    (function () {
        var hiddenList = [];
        function removeHidden() {
            $(hiddenList).each(function (index, value) {
                if (value && value.length) {
                    value.remove();
                }
            });
        }
        function fixsubmit(handle) {
            if (!(this instanceof fixsubmit)) {
                return new fixsubmit(handle);
            }
            (function init(handle) {
                if (handle.is(':submit,.submit')
                && handle.attr('name')
                && !handle.data('fixsubmit.init')) {
                    handle.data('fixsubmit.init', true);
                    var name = handle.attr('name'),
                    value = handle.val() || handle.attr('value'),
                    hiddenInput = $('input:hidden[name="' + name + '"]'),
                    form = handle.parents('form:eq(0)');
                    handle.bind('fixsubmit.click', function () {
                        form.data('fixsubmit.submitbutton', handle);
                        hiddenInput.val(value);
                        removeHidden();
                        hiddenInput = $('<input type="hidden" name="' + name + '" value="' + value + '"/>').appendTo(form);
                        hiddenList.push(hiddenInput);
                        handle.attr('disabled', 'disabled');
                        form.submit();
                    });
                    handle.click(function (e) {
                        handle.trigger('fixsubmit.click');
                        e.preventDefault();
                        return false;
                    }).one('click', function () {
                        form.find('input,select,textarea,:radio').change(function () {
                            handle.removeAttr('disabled');
                        });
                    });
                    var fixRevert = {
                        name: name
                    };
                    handle.data('fixsubmit.revert', fixRevert);
                }
            })(handle);

            this.destory = function () {
                handle.removeData('fixsubmit.init');
                var revert = handle.data('fixsubmit.revert');
                handle.removeData('fixsubmit.revert');
                handle.removeData('fixsubmit');
                handle.attr('name', revert.name);
                handle.unbind('fixsubmit.click');
                removeHidden();
            }
        }

        $.fn.fixsubmit = function (option) {
            this.each(function () {
                var handle = $(this),
                fix = fixsubmit(handle);
                handle.data('fixsubmit', fix);
            });
            return this;
        }
    })();


    //#endregion

    //#region $kooboo
    //#endregion
})(window.jQuery);

/*
* jQuery JSON Plugin
* version: 2.1 (2009-08-14)
*
* This document is licensed as free software under the terms of the
* MIT License: http://www.opensource.org/licenses/mit-license.php
*
* Brantley Harris wrote this plugin. It is based somewhat on the JSON.org 
* website's http://www.json.org/json2.js, which proclaims:
* "NO WARRANTY EXPRESSED OR IMPLIED. USE AT YOUR OWN RISK.", a sentiment that
* I uphold.
*
* It is also influenced heavily by MochiKit's serializeJSON, which is 
* copyrighted 2005 by Bob Ippolito.
*/

(function ($) {
    /** jQuery.toJSON( json-serializble )
    Converts the given argument into a JSON respresentation.

    If an object has a "toJSON" function, that will be used to get the representation.
    Non-integer/string keys are skipped in the object, as are keys that point to a function.

    json-serializble:
    The *thing* to be converted.
    **/
    $.toJSON = function (o) {
        if (typeof (JSON) == 'object' && JSON.stringify)
            return JSON.stringify(o);

        var type = typeof (o);

        if (o === null)
            return "null";

        if (type == "undefined")
            return undefined;

        if (type == "number" || type == "boolean")
            return o + "";

        if (type == "string")
            return $.quoteString(o);

        if (type == 'object') {
            if (typeof o.toJSON == "function")
                return $.toJSON(o.toJSON());

            if (o.constructor === Date) {
                var month = o.getUTCMonth() + 1;
                if (month < 10) month = '0' + month;

                var day = o.getUTCDate();
                if (day < 10) day = '0' + day;

                var year = o.getUTCFullYear();

                var hours = o.getUTCHours();
                if (hours < 10) hours = '0' + hours;

                var minutes = o.getUTCMinutes();
                if (minutes < 10) minutes = '0' + minutes;

                var seconds = o.getUTCSeconds();
                if (seconds < 10) seconds = '0' + seconds;

                var milli = o.getUTCMilliseconds();
                if (milli < 100) milli = '0' + milli;
                if (milli < 10) milli = '0' + milli;

                return '"' + year + '-' + month + '-' + day + 'T' +
                             hours + ':' + minutes + ':' + seconds +
                             '.' + milli + 'Z"';
            }

            if (o.constructor === Array) {
                var ret = [];
                for (var i = 0; i < o.length; i++)
                    ret.push($.toJSON(o[i]) || "null");

                return "[" + ret.join(",") + "]";
            }

            var pairs = [];
            for (var k in o) {
                var name;
                var type = typeof k;

                if (type == "number")
                    name = '"' + k + '"';
                else if (type == "string")
                    name = $.quoteString(k);
                else
                    continue;  //skip non-string or number keys

                if (typeof o[k] == "function")
                    continue;  //skip pairs where the value is a function.

                var val = $.toJSON(o[k]);

                pairs.push(name + ":" + val);
            }

            return "{" + pairs.join(", ") + "}";
        }
    };

    /** jQuery.evalJSON(src)
    Evaluates a given piece of json source.
    **/
    $.evalJSON = function (src) {
        if (typeof (JSON) == 'object' && JSON.parse)
            return JSON.parse(src);
        return eval("(" + src + ")");
    };

    /** jQuery.secureEvalJSON(src)
    Evals JSON in a way that is *more* secure.
    **/
    $.secureEvalJSON = function (src) {
        if (typeof (JSON) == 'object' && JSON.parse)
            return JSON.parse(src);

        var filtered = src;
        filtered = filtered.replace(/\\["\\\/bfnrtu]/g, '@');
        filtered = filtered.replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']');
        filtered = filtered.replace(/(?:^|:|,)(?:\s*\[)+/g, '');

        if (/^[\],:{}\s]*$/.test(filtered))
            return eval("(" + src + ")");
        else
            throw new SyntaxError("Error parsing JSON, source is not valid.");
    };

    /** jQuery.quoteString(string)
    Returns a string-repr of a string, escaping quotes intelligently.  
    Mostly a support function for toJSON.
    
    Examples:
    >>> jQuery.quoteString("apple")
    "apple"
        
    >>> jQuery.quoteString('"Where are we going?", she asked.')
    "\"Where are we going?\", she asked."
    **/
    $.quoteString = function (string) {
        if (string.match(_escapeable)) {
            return '"' + string.replace(_escapeable, function (a) {
                var c = _meta[a];
                if (typeof c === 'string') return c;
                c = a.charCodeAt();
                return '\\u00' + Math.floor(c / 16).toString(16) + (c % 16).toString(16);
            }) + '"';
        }
        return '"' + string + '"';
    };

    var _escapeable = /["\\\x00-\x1f\x7f-\x9f]/g;

    var _meta = {
        '\b': '\\b',
        '\t': '\\t',
        '\n': '\\n',
        '\f': '\\f',
        '\r': '\\r',
        '"': '\\"',
        '\\': '\\\\'
    };
})(jQuery);


//end jquery.extend
