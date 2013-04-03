/*
*
* inline editor
* author: ronglin
* create date: 2011.02.10
*
*/

/*
* config parameters:
* el, onSave, onCancel
*/

(function ($) {

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
        mask = $('<var class="kb-page-mask"></var>').appendTo(yardi.cacheCon);
        posiMask();
        $(window).bind('resize', posiMask);
        yardi.zTop(mask).animate({ opacity: 0.5 }, { duration: 400 });
    } : function () { };

    // region mask
    var regionMask = true, monitor;
    if (regionMask) {
        hideMask = function (el) {
            monitor.stop();
            mask.remove();
        };
        showMask = function (el) {
            mask = new yardi.widgetMask({
                containerNode: el,
                cssClass: 'kb-page-mask'
            });
            mask.region();
            // monitor
            var oldInfo = { width: 0, height: 0, left: 0, top: 0 };
            monitor = new yardi.monitor({
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

    // copy style, this mask will cover the widget container element, 
    // and then the background color in the container will be converd too.
    var copyBackground = function (source, target) {
        if (regionMask) { return; }
        if (!enableMask) { return; }
        var element = source, rootElement = document.body, color;
        while (element != rootElement) {
            var s = yardi.currentStyle(element, 'backgroundColor');
            // chrome use rgba(0, 0, 0, 0) instead of transparent.
            if (s && (s = s.toLowerCase()) != 'transparent' && s != 'rgba(0, 0, 0, 0)') {
                color = s;
                break;
            }
            element = element.parentNode;
        }
        target.style['backgroundColor'] = (color || '#FFFFFF');
        target.setAttribute('bgcopied', '1');
    };

    var deleteBackground = function (element) {
        if (regionMask) { return; }
        if (!enableMask) { return; }
        if (element.getAttribute('bgcopied') == '1') {
            element.removeAttribute('bgcopied');
            element.style['backgroundColor'] = '';
        }
    };

    // 1. eidt the content inside a link, that cause some editor selection problem.
    // this kit remove and revert the link default action.
    // 2. element opacity will cause the editor element be hide under the page mask
    // find it and remove the opacity class when before edit, reset the opacity class after edit.
    // 3. when the editor element's parents have a 'z-index' css and it lower than the editor element 'z-index'
    // the editor element will be masked by the editor masker. fix the 'z-index' css to 'auto' value of parents.
    var unvalidAction = function () {
        var link, href, opacity;
        return {
            find: function (el, filter) {
                var p = el.parent(), tag;
                while (p) {
                    tag = p.get(0).tagName;
                    if (tag === 'BODY') { break; }
                    if (filter(p, tag) === false) { break; }
                    p = p.parent();
                }
            },
            fix: function (el) {
                this.revert(el);
                // link
                this.find(el, function (o, tag) {
                    // link
                    if (!link && tag === 'A') {
                        href = o.attr('href');
                        o.removeAttr('href');
                        link = o;
                    }
                    // class
                    if (!opacity && o.hasClass('kb-block-unpublish')) {
                        o.removeClass('kb-block-unpublish');
                        opacity = o;
                    }
                    // zindex
                    var zindex = o.css('z-index');
                    if (zindex != 'auto') {
                        o.data('oldzindex', zindex);
                        o.css('z-index', 'auto');
                    }
                });
            },
            revert: function (el) {
                // link
                if (link) {
                    link.attr('href', href);
                    link = href = null;
                }
                // class
                if (opacity) {
                    opacity.addClass('kb-block-unpublish');
                    opacity = null;
                }
                this.find(el, function (o, tag) {
                    // zindex
                    var oldzindex = o.data('oldzindex');
                    if (oldzindex != null) {
                        o.removeData('oldzindex');
                        o.css('z-index', oldzindex);
                    }
                });
            }
        };
    } ();

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
            if (!currentEditorToolbar) { return; }
            currentEditorToolbar.message(msg, currentEditorToolbar.components.btnSave.el);
        },

        disabled: function (disable) {
            if (!currentEditor) { return; }
            disable = (disable === true);
            currentEditor.setEnable(!disable);
            currentEditorToolbar.disableBtns(disable);
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
                if (yardi.modeling) { return; }
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
            unvalidAction.fix(this.el);
            // editor map
            var editorTypes = {
                Auto: yardi.editor,
                RichText: yardi.editor,
                Text: yardi.editorPlain,
                Date: yardi.editorDate
            };
            // new editor
            currentEditor = new (editorTypes[this.dataType])({
                el: this.el,
                onInitialized: function () {
                    // cache widget html
                    originalContent = this.getHtml();
                    // copy background
                    copyBackground(this.el.get(0), this.el.get(0));
                    // current editor anchorbar
                    currentEditorToolbar = new yardi.inlineEditorAnchorBar({
                        alignTo: this.el,
                        renderTo: yardi.cacheCon,
                        editor: this,
                        onSave: function () { self.onSave(); },
                        onCancel: function () { self.onCancel(); }
                    });
                    currentEditorToolbar.show();
                    currentEditorToolbar.fix(true);
                    // add mouseup hander
                    this.onMouseup.add(self.mouseupHandler, self);
                    this.onMousedown.add(self.mousedownHandler, self);
                }
            });
            // status
            yardi.zTop(this.el);
            yardi.anchorBar.hideAll();
            yardi.anchorBar.fixAll(true);
            showMask(this.el);
        },

        cancel: function (revert) {
            if (!currentEditor) { return; }
            this.clearDynamicPos();
            // unvalids
            unvalidAction.revert(this.el);
            // delete background
            deleteBackground(currentEditor.el.get(0));
            // remove editor
            currentEditor.setEnable(true);
            (revert !== false) && currentEditor.setHtml(originalContent);
            currentEditor.remove();
            currentEditor = null;
            // editor toolbar
            currentEditorToolbar.remove();
            currentEditorToolbar = null;
            // reset status
            yardi.zOld(this.el);
            yardi.anchorBar.overel = false;
            yardi.anchorBar.fixAll(false);
            originalContent = null;
            hideMask(this.el);
        }
    };

    // register
    yardi.inlineEditor = inlineEditor;

})(jQuery);
