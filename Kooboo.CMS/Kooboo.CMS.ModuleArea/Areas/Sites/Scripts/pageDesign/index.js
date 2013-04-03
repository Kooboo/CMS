/*
*
* index
* author: ronglin
* create date: 2010.11.15
*
*/

(function ($) {

    // global setting
    yardi.rootPath = 'Areas/Sites/Scripts/pageDesign/';

    // sync iframe size
    var lockX = false, lockY = false;
    var contentIframe = function (win) {
        var rid = Math.random().toString();
        win[rid] = true;
        var frm, frms = win.parent.document.getElementsByTagName('iframe');
        for (var i = 0; i < frms.length; i++) {
            if (frms[i].contentWindow[rid] === true) {
                frm = frms[i];
                win[rid] = undefined;
                break;
            }
        }
        return frm;
    } (window);
    var applyMaxContentSize = function () { };
    if (contentIframe) {
        // if the <html> and <body> tag set the style 'height:100%' at the same time,
        // there would always cause the page vertical scrollbar, 
        // so getContentSize function would get a larger size each time called.
        // and so as to the style 'width:100%;' cause the page horizontal scrollbar.
        // the toolbar width/height is 10px, all browser are the same surprisingly.
        var isRepeatCss = function () {
            var ohtml = $('html').get(0), obody = $('body').get(0);
            var ohtmlCss = { wcss: null, hcss: null }, obodyCss = { wcss: null, hcss: null };
            try {
                if (yardi.isGecko) { throw new Error(); }
                ohtmlCss.wcss = yardi.currentStyle(ohtml, 'width');
                ohtmlCss.hcss = yardi.currentStyle(ohtml, 'height');
                obodyCss.wcss = yardi.currentStyle(obody, 'width');
                obodyCss.hcss = yardi.currentStyle(obody, 'height');
            } catch (ex) {
                ohtmlCss.wcss = ohtml.style.width;
                ohtmlCss.hcss = ohtml.style.height;
                obodyCss.wcss = obody.style.width;
                obodyCss.hcss = obody.style.height;
            }
            $.each([obodyCss, ohtmlCss], function () {
                var self = this;
                $.each(self, function (key) {
                    if (this) { self[key] = this.toString().toLowerCase(); }
                });
            });
            return {
                w: (ohtmlCss.wcss !== 'auto' && ohtmlCss.wcss !== '' && ohtmlCss.wcss === obodyCss.wcss),
                h: (ohtmlCss.hcss !== 'auto' && ohtmlCss.hcss !== '' && ohtmlCss.hcss === obodyCss.hcss)
            };
        } ();
        var getContentSize = function (fn) {
            var docEl = document.documentElement, body = document.body;
            return {
                w: Math[fn](docEl.scrollWidth, body.scrollWidth),
                h: Math[fn](docEl.scrollHeight, body.scrollHeight)
            };
        },
        oldSize = { w: 0, h: 0 }, granularity = 10, // toolbar width/height is 10px, all browser are the same surprisingly.
        monitor = new yardi.monitor({
            interval: 200,
            tester: function () {
                var size = getContentSize('max');
                if (Math.abs(oldSize.w - size.w) > granularity ||
                    Math.abs(oldSize.h - size.h) > granularity) {
                    oldSize.w = size.w; oldSize.h = size.h;
                    return true;
                }
            },
            handler: function () {
                if (oldSize.w && !lockX) { contentIframe.style.width = oldSize.w + 'px'; }
                if (oldSize.h && !lockY) { contentIframe.style.height = oldSize.h + 'px'; }
            }
        });
        applyMaxContentSize = function () {
            var thesize = getContentSize('max');
            if (thesize.w && !lockX) { contentIframe.style.width = thesize.w + 'px'; }
            if (thesize.h && !lockY) { contentIframe.style.height = (thesize.h + 100) + 'px'; }
        };
        if (false) {// if (isRepeatCss.w || isRepeatCss.h) {
            var setSize = function () {
                applyMaxContentSize();
                setTimeout(function () { $(window).one('resize', setSize); }, 200);
            };
            setTimeout(function () { setSize(); }, 1000);
        } else {
            monitor.start();
        }
    }

    // jquery method wrapper
    $.fn._slideDown = function (callback) {
        this.slideDown('normal', function () {
            $(this).css('display', 'block');
            callback && callback.call(this);
        });
    };
    $.fn._slideUp = function (callback) {
        this.slideUp('normal', function () {
            $(this).css('display', 'none');
            callback && callback.call(this);
        });
    };

    // drag drop history
    var cachedPar, cachedNext,
    nextSibling = function (el) {
        if (!el) { return; }
        var next = el.nextSibling;
        while (next && next.nodeType != 1) {
            next = next.nextSibling;
        }
        return next;
    },
    insertBefore = function (node, refNode) {
        var p = refNode.parentNode;
        p.insertBefore(node, refNode);
    },
    insertAfter = function (node, refNode) {
        var p = refNode.parentNode;
        if (refNode.nextSibling) {
            p.insertBefore(node, refNode.nextSibling);
        } else {
            p.appendChild(node);
        }
    },
    delegate = function (elem, par, next) {
        return function (ev) {
            var component = elem.data('instance');
            component.hide(function () {
                if (next) {
                    insertBefore(elem.get(0), next);
                } else {
                    par.appendChild(elem.get(0));
                }
                component.show(function () { ev.done(); });
            });
        };
    },
    commitHistory = function (elem, cachedNewPar, cachedNewNext) {
        if (cachedNewPar != cachedPar || cachedNewNext != cachedNext) {
            // commit history
            yardi.designCtx.redoundo.commit({
                name: 'Move ' + elem.data('instance').Name,
                undo: delegate(elem, cachedPar, cachedNext),
                redo: delegate(elem, cachedNewPar, cachedNewNext)
            });
        }
    };

    // mouse move direction
    var originalPosition = { left: 0, top: 0 },
    moveDirection = { left: false, top: false, right: false, bottom: false },
    judgeMoveDirection = function (currentPosition) {
        moveDirection.left = (originalPosition.left > currentPosition.left);
        moveDirection.right = !moveDirection.left;
        moveDirection.top = (originalPosition.top > currentPosition.top);
        moveDirection.bottom = !moveDirection.top;
        originalPosition = currentPosition;
    };

    // widget helper
    var placeholder,
    removePlaceholder = function () {
        if (placeholder) {
            placeholder.removeClass('content-place');
            placeholder = null;
        }
    },
    createPlaceholder = function (refEl) {
        removePlaceholder();
        placeholder = refEl.addClass('content-place');
    },
    getPlaceholderCss = function () {
        if (placeholder) {
            var pos = placeholder.offset();
            return {
                width: placeholder.width(),
                top: pos.top - 4,
                left: pos.left - 4
            }
        }
    };

    // position degisn objects
    var positionDesignInstances = [],
    positionAnchorObjsEach = function (lock, hide) {
        $.each(positionDesignInstances, function () {
            this.menuBar.lock(lock);
            hide && this.menuBar.hide(true);
        });
    };

    // designCtx
    yardi.designCtx = {
        // history
        redoundo: new yardi.redoundoCore(),

        // reset css
        resetCss: function (elems) {
            return elems.each(function () {
                $(this).addClass('kb-do-reset');
                if (!$(this).hasClass('kb-htmlcon'))
                    yardi.designCtx.resetCss($(this).children());
            });
        },

        // selection switch
        disableSelection: function (el, disable) {
            if (disable === false) {
                return el.removeAttr('unselectable').css('MozUserSelect', null).unbind('selectstart');
            } else {
                return el.attr('unselectable', 'on').css('MozUserSelect', 'none').bind('selectstart', function () { return false; });
            }
        },

        // is changed
        isChanged: function () {
            var o = yardi.designCtx.redoundo;
            return (o.canUndo() || o.overflowed);
        },

        // show design dialog
        showDialog: function (cfg, onOk, outer) {
            cfg.open = function () { positionAnchorObjsEach(true); }
            cfg.close = function () {
                positionAnchorObjsEach(false);
                yardi.anchorBar.sleep(600);
            };
            window.designCtx && window.designCtx.showDialog(cfg, onOk, outer);
        },

        // scroll to view
        scrollToWidget: function (el, callback) {
            var elemBounding;
            if (el.css('display') == 'none') {
                var temp = $('<var style="width:1px; height:1px;"></var>').insertAfter(el);
                elemBounding = temp.get(0).getBoundingClientRect();
                temp.remove();
            } else {
                elemBounding = el.get(0).getBoundingClientRect();
            }
            var elemHeight = el.height();
            var windowHeight = $(window.parent).height();
            var iframeBouding = contentIframe.getBoundingClientRect();
            var boundingTop = elemBounding.top + iframeBouding.top;
            var scrollOffset = 0, centerOffset = (windowHeight - elemHeight) / 2;
            if (boundingTop < 0) {
                scrollOffset = boundingTop;
                if (centerOffset > 0) { scrollOffset -= centerOffset; }
            } else if (boundingTop + elemHeight > windowHeight) {
                scrollOffset = boundingTop + elemHeight - windowHeight;
                if (centerOffset > 0) { scrollOffset += centerOffset; }
            }
            // do scroll to view
            if (scrollOffset != 0) {
                var scrollEl = (yardi.isSafari || yardi.isChrome) ? window.parent.document.body : window.parent.document.documentElement;
                var scrollTop = $(scrollEl).attr('scrollTop') + scrollOffset;
                $(scrollEl).animate({ scrollTop: scrollTop }, 500, function () { callback(true); });
            } else {
                callback(false);
            }
        },

        // dd setting
        dropSetting: {
            tolerance: 'pointer',
            accept: '.pagedesign-content',
            activate: function (event, ui) { },
            deactivate: function (event, ui) { },
            over: function (event, ui) {
                var self = $(this);
                if (self.hasClass('pagedesign-holder')) {
                    if (self.attr('_ignoreOnce') == '1') {
                        self.removeAttr('_ignoreOnce');
                        return;
                    }
                    var child = null;
                    self.children().each(function () {
                        var item = $(this);
                        if (item.css('display') != 'none' && !item.hasClass('ui-state-disabled')) {
                            child = item;
                            return false;
                        }
                    });
                    if (child && moveDirection.bottom) {
                        insertBefore(placeholder.get(0), child.get(0));
                    } else {
                        this.appendChild(placeholder.get(0));
                    }
                } else {
                    if (moveDirection.bottom) {
                        insertAfter(placeholder.get(0), this);
                    } else {
                        insertBefore(placeholder.get(0), this);
                    }
                }
            },
            out: function (event, ui) { },
            drop: function (event, ui) { }
        },
        dragSetting: {
            refreshPositions: true,
            distance: 1,
            //containment: 'body',
            drag: function (event, ui) {
                // judge move direction
                judgeMoveDirection(ui.position);
            },
            start: function (event, ui) {
                if ($(this).data('canDrag') != true) { return false; }
                // lock iframe dynamic width
                lockX = true;
                // hide anchor bar
                positionAnchorObjsEach(true, true);
                // create holder
                createPlaceholder($(this));
                // mark to ignore once drop of holder.
                $(this).parent().attr('_ignoreOnce', '1');
                // disabled self drop
                $(this).droppable('option', 'disabled', true);
                // cache
                cachedPar = this.parentNode;
                cachedNext = nextSibling(this);
                // disable selection
                yardi.designCtx.disableSelection($('body'));
            },
            stop: function (event, ui) {
                var elem = $(this), cloneHelper = ui.helper.clone().appendTo('body');
                $('.kb-title', cloneHelper).css('width', '');
                cloneHelper.animate(getPlaceholderCss(), {
                    duration: 500,
                    complete: function () {
                        // release lock
                        lockX = false;
                        // release anchor bar
                        positionAnchorObjsEach(false);
                        // replace position holder
                        removePlaceholder();
                        // remove the ignore once mark in kb position.
                        elem.parent().removeAttr('_ignoreOnce');
                        // enabled self drop
                        elem.droppable('option', 'disabled', false);
                        // remove self
                        $(this).remove();
                        // commit history
                        var cachedNewPar = elem.parent().get(0);
                        var cachedNewNext = nextSibling(elem.get(0));
                        commitHistory(elem, cachedNewPar, cachedNewNext);
                        // disable selection
                        yardi.designCtx.disableSelection($('body'), false);
                        // set new order
                        $(cachedPar).data('instance').doOrder();
                        $(cachedNewPar).data('instance').doOrder();
                    }
                });
            },
            helper: function () {
                // create
                var er = $(this).clone();
                er.find('.kb-htmlcon').remove(); // remove before appendTo body, or scripts in htmlcon will be executed.
                er.appendTo('body').find('.kb-title').width(70);
                // width
                var innerWidth = 0;
                er.children().each(function () { innerWidth += $(this).outerWidth(); });
                return yardi.designCtx.resetCss(yardi.zTop(er)).width(innerWidth + 15);
            }
        }
    };

    // register resize iframe event
    //yardi.designCtx.redoundo.onCommit.add(applyMaxContentSize);

    /*
    * jquery entrance
    */
    $.extend($.fn, {
        pageDesign: function (options) {

            // invoke func
            if (typeof (options) === 'string') {
                var result = [], args = Array.prototype.slice.call(arguments, 1)
                this.each(function () {
                    var o = $.data(this, 'instance');
                    if (o && o[options]) { result.push(o[options].apply(o, args)); }
                });
                // ret
                return (result.length === 0) ? undefined : (result.length === 1) ? result[0] : result;
            }

            // closure
            options = options || {};

            // check has elements
            if (!this.length) {
                options.debug && window.console && console.warn('nothing selected');
                return this;
            }

            // element cache container
            if (!options.cacheCon && $('.pagedesign-cache').length == 0)
                options.cacheCon = $('<var class="kb-cache"></var>').appendTo('body');

            // loop
            this.each(function () {

                // get cache
                var o = $.data(this, 'instance');
                if (o) { return o; }

                // new instance
                o = new yardi.positionDesign(options, $(this));
                positionDesignInstances.push(o);

                // set cache
                $.data(this, 'instance', o);

                // reset
                yardi.designCtx.resetCss($(this));

                // ret
                return o;
            });

            // ret
            return this;
        }
    });

    // onload
    $(function () {

        // try to prevent page refresh
        $.each([$('a'), $('[type="submit"]')], function () {
            this.unbind().live('click', function () { return false; });
        });

        var holders = $('.pagedesign-holder');

        // set virtual path
        yardi.rootPath = __designer.virtualPath + yardi.rootPath;

        // bind designer
        holders.pageDesign({
            processViewUrl: __designer.processViewUrl,
            processModuleUrl: __designer.processModuleUrl,
            processFolderUrl: __designer.processFolderUrl,
            processHtmlUrl: __designer.processHtmlUrl,
            processHtmlBlockUrl: __designer.processHtmlBlockUrl
        });

        // inner public
        window.designCtx = {

            getRedoundo: function () {
                return yardi.designCtx.redoundo;
            },

            isChanged: function () {
                var o = this.getRedoundo();
                return (o.canUndo() || o.overflowed);
            },

            getSettings: function () {
                var settings = [];
                holders.each(function (i, o) {
                    settings = settings.concat($(this).data('instance').getSettings());
                });
                return settings;
            },

            getSettingsJson: function () {
                // check, prevent prototype.js pollute the native object.
                var list = [], undef;
                var check = function (obj, key) {
                    if (obj && obj[key]) {
                        list.push({ obj: obj, key: key, val: obj[key] });
                        try {
                            obj[key] = undef;
                            delete obj[key];
                        } catch (ex) { }
                    }
                };
                $.each([
                    Object, Object.prototype,
                    String, String.prototype,
                    Number, Number.prototype,
                    Array, Array.prototype,
                    Date, Date.prototype
                ], function (index, item) {
                    check(this, 'toJSON');
                });
                (typeof (JSON) === 'object') && check(JSON, 'stringify');

                // parse
                var set = this.getSettings();
                var json = $.toJSON(set);

                // reset
                $.each(list, function (index, item) {
                    item.obj[item.key] = item.val;
                });

                // ret
                return json;
            },

            showDialog: function (cfg, onOk, outer) {
                // implement in nested page.
            }
        };

        // report load event.
        window.parent && window.parent.pageDesignLoaded && window.parent.pageDesignLoaded(designCtx, window);

        // report unload event.
        $(window).unload(function () {
            window.parent && window.parent.pageDesignUnload && window.parent.pageDesignUnload();
        });
    });

    // attention:
    // this must be called at the last of component,
    // this file was loaded at last and this function place at the end.
    // resolve the confilict problem, when there ara more then one jquery instance in the page.
    $.noConflict(true);

})(jQuery);
