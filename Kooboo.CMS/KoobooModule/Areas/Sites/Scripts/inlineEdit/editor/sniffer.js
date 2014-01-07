/*
*   sniffer
*   author: ronglin
*   create date: 2010.06.24
*/

(function (ctx, $) {

    // text resource
    var options = {
        widthFormatError: 'Invalid input width',
        heightFormatError: 'Invalid input height',
        imgSizeConfirm: 'The image size is too big for this layout.\nAre you sure you want to use this size?',
        deleteImgConfirm: 'Are you sure you want to delete this image?',
        unlinkConfirm: 'Are you sure you want to delete the link?'
    };

    // override text resource
    if (window.__inlineEditVars) { $.extend(options, __inlineEditVars.sniffer_js); }

    /*
    * editor sniffer
    */
    var editorSniffer = function (editor) {
        this.editor = editor;
        this.initialize();
    };

    editorSniffer.prototype = {

        editor: null,

        snifferWatcher: null,

        processing: false,

        // static status configs
        newElementFlag: 'yardi_',
        blankHolderName: 'blank_img_holder.gif',
        _editorMousedown: null,

        currentPanel: null,
        currentSniffer: null,
        originalHtml: null,

        constructor: editorSniffer,

        initialize: function () {
            var self = this;
            // watch the specified document
            this.editor.onMousedown.add(this._editorMousedown = function (sender, ev) {
                if (self.currentSniffer != ev.target || ev.which === 3) {
                    if (self.currentSniffer) {
                        self.clearSniffer();
                        return;
                    }
                    if (ev.which !== 1) { return; }
                    // setTimeout to ensure insert sniffer after the document mousedown event(fire twinkle)
                    setTimeout(function () {
                        if (ev.target.nodeName == 'IMG') {
                            var link, elem = ev.target;
                            while (elem) {
                                var n = elem.nodeName;
                                if (n === 'BODY') { break; }
                                if (n === 'A') { link = elem; break; }
                                elem = elem.parentNode;
                            }
                            self.imgSniffer(ev.target, link);
                        } else if (ev.target.nodeName == 'A') {
                            self.linkSniffer(ev.target);
                        }
                        // other nodeTypes here.
                    }, 10);
                }
            });
            // others
            this.snifferWatcher = new ctx.monitor({
                scope: this,
                interval: 100,
                tester: function () {
                    if (this.currentSniffer && this.currentSniffer.parentNode) {
                        return false;
                    } else {
                        return true;
                    }
                },
                handler: function () {
                    this.clearSniffer();
                }
            });
        },

        destroy: function () {
            this.clearSniffer();
            this.snifferWatcher.stop();
            this.editor.onMousedown.remove(this._editorMousedown);
        },

        isNew: function (el) {
            if (el && el.id && el.id.toString().indexOf(this.newElementFlag) == 0) {
                return true;
            } else {
                return false;
            }
        },

        cleanNew: function () {
            var el = this.currentSniffer;
            if (this.isNew(el)) {
                el.id = null;
                el.removeAttribute('id');
            }
        },

        clearSniffer: function () {
            this.snifferWatcher.stop();
            if (this.isNew(this.currentSniffer) && this.originalHtml != null) {
                this.editor.setHtml(this.originalHtml, { processScript: false });
                this.originalHtml = null;
            }
            this.currentSniffer = null;
            if (this.currentPanel) {
                this.currentPanel.remove();
                this.currentPanel = null;
            }
        },

        setSniffer: function (el) {
            this.clearSniffer();
            this.currentSniffer = el;
            this.snifferWatcher.start();
        },

        // for trigger store redo undo list.
        triggerCommend: function (cmd, html) {
            this.editor.onExecCommand.dispatch(this.editor, cmd, html);
        },

        // invoke selection to paste sniffer html
        pasteHtml: function (html) {
            var range = this.editor.Selection.getRange();
            var pelem = this.editor.Selection.getParentElement(range);
            var editorDom = this.editor.el.get(0);
            if (editorDom == pelem || ctx.isAncestor(editorDom, pelem)) {
                if (range.pasteHTML) {
                    range.pasteHTML(html);
                } else {
                    range.deleteContents();
                    range.insertNode($(html).get(0));
                }
            }
        },

        cacheHtml: function () {
            this.originalHtml = this.editor.getHtml();
        },

        getPosition: function (el) {
            var winWidth = $(window).width();
            var selfWdith = this.currentPanel.el.outerWidth();
            var scrollLeft = this.editor.el.get(0).scrollLeft;
            var offset = $(el).offset(), info = {
                x: offset.left,
                y: offset.top,
                width: $(el).outerWidth(),
                height: $(el).outerHeight()
            };
            // parameter
            var left = info.x + (info.width / 2) - (selfWdith / 2) - scrollLeft;
            var top = info.y + info.height;
            var overflow = Math.max(0, info.x - scrollLeft + (info.width / 2) + (selfWdith / 2) - winWidth);
            if (left < 0) { overflow = left; }
            // arrow
            var aLeft = (this.currentPanel.el.width() - this.currentPanel.arrowTarget.width()) / 2 + overflow;
            this.currentPanel.arrowLeft(aLeft);
            // ret
            return { left: left - overflow, top: top };
        },

        // public
        insertNewImg: function () {
            if (this.processing) { return; }
            this.processing = true;
            // cache first
            this.cacheHtml();
            // paste new html
            var holderId = this.newElementFlag + Math.random().toString();
            this.pasteHtml('<img id="' + holderId + '" />');
            // show
            var holderSrc = (ctx.rootPath || '') + 'editor/images/' + this.blankHolderName;
            var self = this, img = this.editor.doc.getElementById(holderId);
            if (img) {
                img.onload = function () {
                    this.onload = null; // set to null or when set a new src the onload event would be fired again.
                    setTimeout(function () {
                        self.imgSniffer(img);
                        self.processing = false;
                    }, 10);
                };
                img.src = holderSrc;
            } else {
                this.processing = false;
            }
        },

        // public
        insertNewLink: function () {
            if (this.processing) { return; }
            this.processing = true;
            // cache first
            this.cacheHtml();
            // paste new html
            var holderId = this.newElementFlag + Math.random().toString();
            var innerText = this.editor.Selection.getText();
            this.pasteHtml('<a id="' + holderId + '" href="javascript:;">' + innerText + '</a>');
            // show
            var self = this, link = this.editor.doc.getElementById(holderId);
            if (link) {
                setTimeout(function () {
                    self.linkSniffer(link);
                    self.processing = false;
                }, 10);
            } else {
                this.processing = false;
            }
        },

        imgSniffer: function (img, link) {
            if (link) {
                img._parentLink = link;
                if (!link.getAttribute('href')) {
                    link.setAttribute('href', '#');
                }
            }
            this.setSniffer(img);
            // create img sniffer
            var self = this;
            var panel = new ctx.imagePanel({
                showArrow: true,
                onOk: function (url, alt, width, height, href, html) {
                    try {
                        if (width) {
                            var w = parseFloat(width);
                            if (!ctx.isNumber(w) || w.toString().length != width.length || w < 0) {
                                alert(options.widthFormatError);
                                return;
                            }
                            if (w > self.editor.el.width() && !confirm(options.imgSizeConfirm)) {
                                return;
                            }
                        }
                        if (height) {
                            var h = parseFloat(height);
                            if (!ctx.isNumber(h) || h.toString().length != height.length || h < 0) {
                                alert(options.heightFormatError);
                                return;
                            }
                            if (h > self.editor.el.height() && !confirm(options.imgSizeConfirm)) {
                                return;
                            }
                        }

                        // src
                        var el = self.currentSniffer;
                        el.src = url;

                        // alt
                        el.removeAttribute('alt');
                        if (alt) { el.alt = alt; }

                        // width
                        el.removeAttribute('width');
                        if (width) { el.width = width; }
                        el.style.width = '';

                        // height
                        el.removeAttribute('height');
                        if (height) { el.height = height; }
                        el.style.height = '';

                        // link
                        var parentLink = el._parentLink;
                        if (href) {
                            if (!parentLink) {
                                parentLink = document.createElement('a');
                                parentLink.setAttribute('target', '_blank');
                                el.parentNode.insertBefore(parentLink, el);
                                parentLink.appendChild(el);
                            }
                            parentLink.setAttribute('href', href);
                        } else if (parentLink) {
                            parentLink.parentNode.insertBefore(el, parentLink);
                            parentLink.parentNode.removeChild(parentLink);
                        }
                    } catch (ex) { }
                    // clean
                    self.cleanNew();
                    self.clearSniffer();
                    self.triggerCommend('insertimage', html);
                },
                onDelete: function () {
                    if (confirm(options.deleteImgConfirm)) {
                        var el = self.currentSniffer;
                        if (!self.isNew(el)) {
                            el.parentNode.removeChild(el);
                            var parentLink = el._parentLink;
                            if (parentLink) { parentLink.parentNode.removeChild(parentLink); }
                        }
                        self.clearSniffer();
                        self.triggerCommend('unimage');
                    }
                },
                onCancel: function () {
                    self.clearSniffer();
                },
                onClose: function () {
                    self.clearSniffer();
                }
            });
            this.currentPanel = panel;
            panel.initialize();
            panel.showPos(this.getPosition(img));
            // init value
            if (img.src.indexOf(this.blankHolderName) == -1) {
                if (img.getAttribute('src')) { panel.setSrc(img.src); } // when src attribute is empty, the src property is link to the current page.
                if (link) { panel.setHref(link.getAttribute('href')); }
                panel.setWidth($(img).width());
                panel.setHeight($(img).height());
                panel.setAlt(img.alt);
            }

            // focus
            setTimeout(function () { panel.focus(); }, 400);
        },

        linkSniffer: function (link) {
            this.setSniffer(link);
            // create link sniffer
            var self = this;
            var panel = new ctx.linkPanel({
                showArrow: true,
                onOk: function (txt, url, title, target, html) {
                    try {
                        if (url) {
                            var urlTrim = url.replace(/(^\s*)|(\s*$)/g, '').toLowerCase();
                            if (urlTrim.indexOf('http://') !== 0 &&
                                urlTrim.indexOf('https://') !== 0 &&
                                urlTrim.indexOf('mailto:') !== 0 &&
                                urlTrim.indexOf('#') !== 0) {
                                //url = 'http://' + url;
                            }
                        }
                        var el = self.currentSniffer;
                        if (ctx.isIE) {
                            // when A tag nested in A tag, innerHTML is unaccessable
                            while (el.childNodes.length) { el.removeChild(el.childNodes[0]); }
                            el.appendChild(document.createTextNode(txt));
                        } else {
                            el.innerHTML = txt;
                        }
                        el.setAttribute('href', url);
                        if (title) {
                            el.title = title;
                        } else {
                            el.removeAttribute('title');
                        }
                        if (target !== undefined) {
                            if (target) {
                                el.target = target;
                            } else {
                                el.removeAttribute('target');
                            }
                        }
                        /*var v = '';
                        panel.linkTypes.each(function () {
                        if ($(this).attr('checked')) {
                        v = $(this).val();
                        return false;
                        }
                        });
                        if (v == 'onlineversion') {
                        $(el).removeAttr('subscription').attr('onlineversion', '');
                        } else {
                        $(el).removeAttr('onlineversion');
                        if (v) {
                        $(el).attr('subscription', v);
                        } else {
                        $(el).removeAttr('subscription');
                        }
                        }*/
                    } catch (ex) { }
                    // clean
                    self.cleanNew();
                    self.clearSniffer();
                    self.triggerCommend('link', html);
                },
                onUnlink: function () {
                    if (confirm(options.unlinkConfirm)) {
                        var el = self.currentSniffer;
                        if (!self.isNew(el)) {
                            var t = $(el).text(), p = el.parentNode;
                            p.insertBefore(document.createTextNode(t), el);
                            p.removeChild(el);
                        }
                        self.clearSniffer();
                        self.triggerCommend('unlink');
                    }
                },
                onCancel: function () {
                    self.clearSniffer();
                },
                onClose: function () {
                    self.clearSniffer();
                }
            });
            this.currentPanel = panel;
            panel.initialize();
            panel.showPos(this.getPosition(link));
            // init value
            var href = link.getAttribute('href');
            if (href) { // when href attribute is empty, the href property is link to the current page.
                if (href.indexOf('#') == 0) {
                    // the anchor use the original value.
                    panel.urlTarget.val(href);
                } else {
                    // common link use absolute url.
                    panel.urlTarget.val(link.href.replace('javascript:;', ''));
                }
            }
            if (link.innerHTML) { panel.textTarget.val(link.innerHTML); }
            if (link.title) { panel.titleTarget.val(link.title); }
            if (link.target) { panel.targetTarget.attr('checked', (link.target == '_blank')); }
            /*var linktype = ($(link).attr('subscription') || '').toLowerCase();
            if ($(link).attr('onlineversion') !== undefined) { linktype = 'onlineversion'; }
            panel.linkTypes.each(function () {
            if ($(this).val() == linktype) {
            $(this).attr('checked', true);
            return false;
            }
            });*/
            setTimeout(function () {
                panel.textTarget.focus();
            }, 400);
        }
    };

    // register
    ctx.editorSniffer = editorSniffer;

})(yardi, jQuery);
