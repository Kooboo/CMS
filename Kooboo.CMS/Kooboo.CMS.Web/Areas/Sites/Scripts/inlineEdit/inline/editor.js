/*
*   inline editor
*   author: ronglin
*   create date: 2011.02.10
*/

/*
* config parameters:
* el, onSave, onCancel
*/

(function (ctx, $) {

    // page mask
    var enableMask = true, mask;
    var posiMask = enableMask ? function () {
        var docEl = document.documentElement, body = document.body;
        mask.css({
            width: Math.max(docEl.scrollWidth, body.scrollWidth),
            height: Math.max(docEl.scrollHeight, body.scrollHeight)
        });
    } : function () { };
    var hideMask = enableMask ? function () {
        $('.kb-page-mask').remove();
        $(window).unbind('resize', posiMask);
    } : function () { };
    var showMask = enableMask ? function () {
        mask = $('<var class="kb-page-mask"></var>').appendTo(ctx.cacheCon);
        posiMask();
        $(window).bind('resize', posiMask);
        ctx.zTop(mask).animate({ opacity: 0.5 }, { duration: 400 });
    } : function () { };

    // region mask
    var regionMask = true, monitor;
    if (regionMask) {
        hideMask = function (el) {
            monitor.stop();
            mask.remove();
        };
        showMask = function (el) {
            mask = new ctx.widgetMask({
                containerNode: el,
                cssClass: 'kb-page-mask'
            });
            mask.region();
            // monitor
            var oldInfo = { width: 0, height: 0, left: 0, top: 0 };
            monitor = new ctx.monitor({
                scope: this,
                interval: 50,
                tester: function () {
                    var size = { width: el.width(), height: el.height() };
                    if (oldInfo.width != size.width || oldInfo.height != size.height) {
                        oldInfo.width = size.width; oldInfo.height = size.height;
                        return true;
                    }
                    var posi = el.position();
                    if (oldInfo.top != posi.top || oldInfo.left != posi.left) {
                        oldInfo.top = posi.top; oldInfo.left = posi.left;
                        return true;
                    }
                },
                handler: function () {
                    mask.correctPosition();
                }
            });
            monitor.start();
        };
    }

    // fix browser default actions which cause edit problems, and revert them after edit complete.
    // actions attention: As of jQuery 1.6, the .attr() method returns undefined for attributes that have not been set. 
    var unvalidActions = function () {
        var actions = [], find = function (el, fn) {
            var par = el.parent(), tag;
            while (par) {
                tag = par.get(0).tagName;
                if (tag === 'BODY') { break; }
                if (fn(par, tag) === false) { break; }
                par = par.parent();
            }
        };
        return {
            reg: function (act) {
                actions.push(act);
            },
            fix: function (el) {
                this.revert(el);
                find(el, function (obj, tag) {
                    $.each(actions, function () {
                        this.fix.call(el, obj, tag);
                    });
                });
            },
            revert: function (el) {
                find(el, function (obj, tag) {
                    $.each(actions, function () {
                        this.revert.call(el, obj, tag);
                    });
                });
            }
        };
    } ();

    // 1. eidt the content inside a link, that cause some editor selection problem.
    // find all it's parent elements, remove and revert the href default action of links.
    unvalidActions.reg(function () {
        var holder = 'KOOBOOHREF', name = 'href';
        return {
            fix: function (obj, tag) {
                if (tag === 'A' && obj.attr(name) !== undefined) {
                    obj.attr(holder, obj.attr(name));
                    obj.removeAttr(name);
                }
            },
            revert: function (obj, tag) {
                if (tag === 'A' && obj.attr(holder) !== undefined) {
                    obj.attr(name, obj.attr(holder));
                    obj.removeAttr(holder);
                }
            }
        };
    } ());

    // 2. element opacity will cause the editor element be hide under the page mask
    // find it and remove the opacity class when before edit, reset the opacity class after edit.
    unvalidActions.reg(function () {
        var holder = 'KOOBOOCLASS', cls = 'kb-block-unpublish';
        return {
            fix: function (obj, tag) {
                if (obj.hasClass(cls)) {
                    obj.removeClass(cls);
                    obj.attr(holder, cls);
                }
            },
            revert: function (obj, tag) {
                if (obj.attr(holder) !== undefined) {
                    obj.removeAttr(holder);
                    obj.addClass(cls);
                }
            }
        };
    } ());

    // 3. when the editor element's parents have a 'z-index' css and it lower than the editor element 'z-index'
    // the editor element will be masked by the editor masker. fix the 'z-index' css to 'auto' value of parents.
    unvalidActions.reg(function () {
        // Since it use four parts of mask to compose a page masker, the 'z-index' problem is a history. so comment belows out.
        var holder = 'KOOBOOZINDEX', name = 'z-index';
        return {
            fix: function (obj, tag) {
                //var zindex = obj.css(name);
                //if (zindex !== 'auto') {
                //    obj.attr(holder, zindex);
                //    obj.css(name, 'auto');
                //}
            },
            revert: function (obj, tag) {
                //if (obj.attr(holder) !== undefined) {
                //    obj.css(name, obj.attr(holder));
                //    obj.removeAttr(holder);
                //}
            }
        };
    } ());

    // 4. when the editor element inside a label and the label has a "for" tag, 
    // it cause the selection focus to a input element but not the editor element. so fix and revert it.
    unvalidActions.reg(function () {
        var holder = 'KOOBOOFOR', name = 'for';
        return {
            fix: function (obj, tag) {
                if (tag === 'LABEL' && obj.attr(name) !== undefined) {
                    obj.attr(holder, obj.attr(name));
                    obj.removeAttr(name);
                }
            },
            revert: function (obj, tag) {
                if (tag === 'LABEL' && obj.attr(holder) !== undefined) {
                    obj.attr(name, obj.attr(holder));
                    obj.removeAttr(holder);
                }
            }
        };
    } ());

    // 5. the mask(not the composed masker) will cover the editor's parent container element, 
    // and then the background color in the container will be converd too, so it need to copy style.
    if (!regionMask && enableMask) {
        unvalidActions.reg(function () {
            var holder = 'KOOBOOBGCOPIED', copied;
            return {
                fix: function (obj, tag) {
                    if (copied !== true) {
                        var c = ctx.currentStyle(obj.get(0), 'backgroundColor');
                        // chrome use rgba(0, 0, 0, 0) instead of transparent.
                        if (c && (c = c.toLowerCase()) != 'transparent' && c != 'rgba(0, 0, 0, 0)') {
                            this.css('background-color', c);
                            this.attr(holder, 'true');
                            copied = true;
                        }
                    }
                },
                revert: function (obj, tag) {
                    if (copied === true) {
                        copied = undefined;
                        this.removeAttr(holder);
                        this.css('background-color', '');
                    }
                }
            };
        } ());
    }


    var currentEditor;
    var currentEditorToolbar;
    var originalContent;

    var inlineEditor = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    inlineEditor.prototype = {

        el: null, dataType: 'Auto', onSave: null, onCancel: null,

        initialize: function () {
            if (!this.onSave) { this.onSave = function () { }; }
            if (!this.onCancel) { this.onCancel = function () { }; }
        },

        message: function (msg) {
            //if (!currentEditorToolbar) { return; }
            //currentEditorToolbar.message(msg, currentEditorToolbar.components.btnSave.el);
            currentEditor && currentEditor.message(msg, currentEditor.getSaveEl());
        },

        disabled: function (disable) {
            if (!currentEditor) { return; }
            disable = (disable === true);
            //Modified by Raoh in 2013-06-20
            //currentEditor.setEnable(!disable);
            //currentEditorToolbar.disableBtns(disable);
            currentEditor.setEnable(!disable);
        },

        value: function () {
            if (!currentEditor) { return; }
            currentEditor.clearGarbage();
            return currentEditor.getHtml();
        },

        clearDynamicPos: function () {
            if (this.mousemove) {
                $(currentEditor.doc).unbind('mousemove', this.mousemove);
                this.mousemove = null;
            }
            currentEditorToolbar.correctPosition();
            currentEditorToolbar.positionMonitor.start();
            currentEditorToolbar.el.css({ opacity: 1 });
        },

        attachDynamicPos: function (pos1, pos2) {
            // filter
            var bounding = currentEditorToolbar.el.get(0).getBoundingClientRect();
            if (bounding.top > 0) { return; }
            if (!currentEditor.Selection.hasSelection()) { return; }
            if (currentEditor.Selection.isControl()) { return; }
            if (!currentEditor.Selection.getText()) { return; }

            // range
            var minX = Math.min(pos1.left, pos2.left);
            var maxX = Math.max(pos1.left, pos2.left);
            var minY = Math.min(pos1.top, pos2.top);
            var maxY = Math.max(pos1.top, pos2.top);

            // set new position
            var offsetBtm = 20;
            var offsetTop = currentEditorToolbar.el.outerHeight() + offsetBtm;
            currentEditorToolbar.positionMonitor.stop();
            currentEditorToolbar.el.css({
                left: minX,
                top: minY - offsetTop
            });

            // monitor opacity
            if (!this.mousemove) {
                this.mousemove = this.mousemoveHandler(this, offsetTop, offsetBtm, minY, maxY);
                $(currentEditor.doc).bind('mousemove', this.mousemove);
            }
        },

        mousemoveHandler: function (self, startOffsetTop, startOffsetBtm, minY, maxY) {
            return function (ev) {
                if (ctx.modeling) { return; }
                var distance, offset;
                if (ev.pageY < minY) {
                    offset = minY - ev.pageY;
                    if (offset > startOffsetTop) {
                        distance = offset - startOffsetTop;
                    }
                } else if (ev.pageY > maxY) {
                    offset = ev.pageY - maxY;
                    if (offset > startOffsetBtm) {
                        distance = offset - startOffsetBtm;
                    }
                }
                if (distance !== undefined) {
                    if (distance > 100) {
                        self.clearDynamicPos();
                    } else {
                        var opacity = (100 - distance) / 100;
                        currentEditorToolbar.el.css({ opacity: opacity });
                    }
                }
            };
        },

        mouseupHandler: function (sender, ev) {
            // reset
            this.clearDynamicPos();

            // do dynamic
            var self = this;
            setTimeout(function () {
                var pos1 = self.cachedStartPosition;
                var pos2 = { left: ev.pageX, top: ev.pageY };
                self.attachDynamicPos(pos1, pos2);
            }, 100);
        },

        mousedownHandler: function (sender, ev) {
            this.clearDynamicPos();
            this.cachedStartPosition = { left: ev.pageX, top: ev.pageY };
        },

        edit: function () {
            var self = this;
            // unvalids
            unvalidActions.fix(this.el);
            // cancel events
            ctx.eventPropagation.stop(this.el);
            // editor map
            var editorTypes = {
                Auto: ctx.editor,
                RichText: ctx.editor,
                Text: ctx.editorPlain,
                Date: ctx.editorDate
            };
            // new editor
            currentEditor = new (editorTypes[this.dataType])({
                el: this.el,
                onSave:function(){
                    self.onSave();
                },
                onCancel: function () {
                    self.onCancel();
                },
                onInitialized: function () {
                    // cache widget html
                    originalContent = this.getHtml();
                    // current editor anchor
                    //Commented by Raoh in 2013-06-20
                    /*currentEditorToolbar = new ctx.editorAnchor({
                        alignTo: this.el,
                        renderTo: ctx.cacheCon,
                        editor: this,
                        onSave: function () { self.onSave(); },
                        onCancel: function () { self.onCancel(); }
                    });
                    currentEditorToolbar.show();
                    currentEditorToolbar.fix(true);
                    // add mouseup hander
                    this.onMouseup.add(self.mouseupHandler, self);
                    this.onMousedown.add(self.mousedownHandler, self);*/
                }
            });
            // status
            ctx.zTop(this.el);
            ctx.anchor.hideAll();
            ctx.anchor.fixAll(true);
            showMask(this.el);
        },
        cancel: function (revert) {
            if (!currentEditor) { return; }
            //this.clearDynamicPos();//Commented by Raoh in 2013-06-20
            // unvalids
            unvalidActions.revert(this.el);
            // cancel events
            ctx.eventPropagation.release(this.el);
            // remove editor
            currentEditor.setEnable(false);
            if (revert !== false) { currentEditor.setHtml(originalContent, { processScript: false }); }
            currentEditor.remove();
            currentEditor = null;
            //Commented by Raoh in 2013-06-20
            // editor toolbar
            //currentEditorToolbar.remove();
            //currentEditorToolbar = null;
            // reset status
            ctx.zOld(this.el);
            ctx.anchor.overel = false;
            ctx.anchor.fixAll(false);
            originalContent = null;
            hideMask(this.el);
        }
    };

    // register
    ctx.inlineEditor = inlineEditor;

})(yardi, jQuery);
