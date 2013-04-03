/*
*   window selection
*   author: ronglin
*   create date: 2010.06.24
*/

(function (ctx) {

    var selectionClass = function (win) {
        this._window = win;
        this.isIE = ctx.isIE;
        this.isOpera = ctx.isOpera;
        this.isSafari = ctx.isSafari;
        this.isSafari2 = ctx.isSafari2;
    };

    selectionClass.prototype = {

        _window: null,

        isIE: false, isOpera: false, isSafari: false, isSafari2: false,

        selectElement: function (el) {
            return selectionClass.utils.selectElement(this._window, el);
        },

        pasteHtml: function (html, select) {
            select = (select == true);
            if (this.isIE) {
                return this._executeIE(html, select);
            } else {
                return this._executeMozilla(html, select);
            }
        },

        //TODO: this is not the newest version.
        _executeIE: function (html, select) {
            var doc = this._window.document;
            var sel = doc.selection;
            if (sel.type.toLowerCase() != "none") {
                sel.createRange().execCommand("Delete");
            }
            if (sel.type.toLowerCase() != "none") {
                doc.execCommand("Delete");
            }
            try {
                //doc.body.setActive();
                //doc.body.firstChild.setActive();
            } catch (ex) { }
            var selRange = sel.createRange();
            if (selRange && selRange.length) {
                var rngItem = selRange.item(0);
                if (rngItem && rngItem.tagName == "BODY") {
                    var el = rngItem.getElementsByTagName("FORM")[0];
                    if (el) {
                        selection.utils.setElementInnerHtml(el, el.innerHTML + html);
                    }
                }
            } else {
                var rng = selRange.duplicate();
                rng.collapse(true);
                //html = selectionClass.utils.getStoredOriginalPathsAndAttributes(html);
                selRange.pasteHTML(html);
                selectionClass.utils.restoreOriginalPathsAndAttributes(selRange.parentElement());
                if (select) {
                    rng.setEndPoint("EndToEnd", selRange);
                    rng.select();
                }
            }
            return true;
        },

        _executeMozilla: function (html, select) {
            var doc = this._window.document;
            var span = doc.createElement("span");
            span.innerHTML = html;
            if (this.isSafari || this.isOpera) {
                var spanId = "radetempnode";
                span.setAttribute("id", spanId);
                if (this.isSafari) {
                    select = true;
                }
                this._insertNodeAtSelection(this._window, span, select);
                var range = doc.createRange();
                var sel = this._window.getSelection();
                var node = doc.getElementById(spanId);
                range.selectNodeContents(node);
                var contents = range.extractContents();
                range.selectNode(node);
                range.insertNode(contents);
                sel.addRange(range);
                if (this.isSafari) {
                    sel.collapseToEnd();
                }
                // ...
                if (span) { span.parentNode.removeChild(span); }
            } else {
                var range = doc.createRange();
                range.selectNodeContents(span);
                var contents = range.extractContents();
                if (contents.childNodes.length == 1) {
                    contents = contents.childNodes[0];
                }
                this._insertNodeAtSelection(this._window, contents, select);
            }
            return true;
        },

        _insertNodeAtSelection: function (win, node, select) {
            var sel = win.getSelection();
            if (sel.rangeCount == 0) {
                win.document.body.appendChild(node);
                return;
            }
            var range = null;
            if (sel.getRangeAt) {
                range = sel.getRangeAt(0);
            } else {
                range = win.document.createRange();
                range.setStart(sel.anchorNode, sel.anchorOffset);
                range.setEnd(sel.focusNode, sel.focusOffset);
            }
            if (sel.removeAllRanges) {
                sel.removeAllRanges();
            }
            range.deleteContents();
            var startNode = this.isSafari2 ? sel.baseNode : range.startContainer;
            var startOffset = this.isSafari2 ? sel.baseOffset : range.startOffset;
            var startNodePrev = startNode.previousSibling;
            if (this.isSafari && null == startNode) {
                startNode = win.document.body;
            }
            range = win.document.createRange();
            if ((node.nodeType == 3) && (startNode.nodeType == 3)) {
                startNode.insertData(startOffset, node.nodeValue);
                range.setEnd(startNode, startOffset + node.length);
                if (select) {
                    range.setStart(startNode, startOffset);
                } else {
                    range.setStart(startNode, startOffset + node.length);
                }
            } else {
                var behindNode;
                var lastNode = null;
                if (startNode.nodeType == 3) {
                    var startNodeRef = startNode;
                    startNode = startNodeRef.parentNode;
                    var nodeVal = startNodeRef.nodeValue;
                    var front = nodeVal.substr(0, startOffset);
                    var behind = nodeVal.substr(startOffset);
                    var frontNode = win.document.createTextNode(front);
                    behindNode = win.document.createTextNode(behind);
                    var flag = false;
                    if (this.isFirefox && node.nodeType != 1) {
                        var nodes = node.childNodes;
                        var len = nodes.length;
                        for (var j = 0; j < len; j++) {
                            if (nodes[j].nodeType != 3) {
                                flag = true;
                                break;
                            }
                        }
                    }
                    startNode.insertBefore(behindNode, startNodeRef);
                    startNode.insertBefore(node, behindNode);
                    try {
                        startNode.insertBefore(frontNode, node);
                    } catch (ex) { }
                    startNode.removeChild(startNodeRef);
                    if (this.isFirefox && flag) {
                        var r = document.createTextNode(front);
                        if (startNodePrev) {
                            startNode.insertBefore(r, startNodePrev.nextSibling);
                        } else {
                            startNode.insertBefore(r, startNode.childNodes[0]);
                        }
                    }
                } else {
                    if (startNode.childNodes.length > 0) {
                        behindNode = startNode.childNodes[startOffset];
                        startNode.insertBefore(node, behindNode);
                    } else {
                        startNode.appendChild(node);
                    }
                }
                if (startNode.tagName == "BODY" && !behindNode) {
                    var nodes = startNode.childNodes;
                    lastNode = nodes[nodes.length - 1];
                }
                try {
                    if (select) {
                        range.setStart(node, 0);
                        range.setEnd(behindNode, 0);
                    } else {
                        if (lastNode) {
                            range.setStartAfter(lastNode);
                            range.setStartAfter(lastNode);
                        } else {
                            range.setStartBefore(behindNode);
                            range.setEndBefore(behindNode);
                        }
                    }
                } catch (ex) { }
            }
            try {
                sel.addRange(range);
            } catch (ex) { }
        },

        selectRange: function (range) {
            if (!range) { return; }
            var win = this._window;
            if (range.select) {
                range.select();
            } else {
                if (win.getSelection) {
                    var sel = win.getSelection();
                    if (sel.removeAllRanges) {
                        sel.removeAllRanges();
                        sel.addRange(range);
                    } else {
                        var startNode = range.baseNode;
                        if (null == startNode) {
                            startNode = win.document.body;
                        }
                        var endNode = range.extentNode;
                        if (null == endNode) {
                            endNode = win.document.body;
                        }
                        sel.setBaseAndExtent(startNode, range.startOffset, endNode, range.endOffset);
                    }
                }
            }
        },

        getBrowserSelection: function () {
            if (!this._window) { return null; }
            if (this._window.document.selection && !window.opera) {
                return this._window.document.selection;
            } else {
                if (this._window.getSelection) {
                    return this._window.getSelection();
                }
            }
            return null;
        },

        getRange: function () {
            if (!this._window) { return null; }
            if (this._window.document.selection && !window.opera) {
                return this._window.document.selection.createRange();
            } else {
                if (this._window.getSelection) {
                    var sel = this._window.getSelection();
                    if (!sel || sel.rangeCount < 1) {
                        return null;
                    }
                    var range = null;
                    if (sel.getRangeAt) {
                        range = sel.getRangeAt(0);
                    } else {
                        range = this._window.document.createRange();
                        range.setStart(sel.anchorNode, sel.anchorOffset);
                        range.setEnd(sel.focusNode, sel.focusOffset);
                    }
                    return range;
                }
            }
        },

        getParentElement: function (range) {
            range = range || this.getRange();
            if (!range) { return null; }
            if (range.commonAncestorContainer) {
                var sel = this._window.getSelection();
                var startNode = range.startContainer ? range.startContainer : sel.baseNode;
                var endNode = range.endContainer ? range.endContainer : sel.extentNode;
                var startOffset = range.startOffset != null ? range.startOffset : sel.baseOffset;
                var endOffset = range.endOffset != null ? range.endOffset : sel.extentOffset;
                if (startNode == endNode && (endOffset - startOffset) == 1 && sel.anchorNode.childNodes[sel.anchorOffset]) {
                    return sel.anchorNode.childNodes[sel.anchorOffset];
                } else {
                    if (this.isSafari) {
                        if (sel.anchorNode.parentNode.tagName == "TH" || sel.anchorNode.parentNode.tagName == "TD") {
                            return sel.anchorNode.parentNode;
                        } else {
                            if (sel.anchorNode.parentNode.tagName == "TR") {
                                return sel.anchorNode;
                            }
                        }
                    }
                    if (!range.commonAncestorContainer.tagName) {
                        if (this._window.document == range.commonAncestorContainer && sel.baseNode) {
                            return sel.baseNode.parentNode;
                        }
                        return range.commonAncestorContainer.parentNode;
                    } else {
                        return range.commonAncestorContainer;
                    }
                }
            } else {
                if (range.length) {
                    return range.item(0);
                } else {
                    if (range.parentElement) {
                        return range.parentElement();
                    } else {
                        return null;
                    }
                }
            }
        },

        isControl: function () {
            if (this._window.document.selection) {
                return (this._window.document.selection.type == "Control");
            } else {
                var sel = this._window.getSelection();
                if (sel.toString() != "") {
                    return false;
                }
                var node = sel.focusNode;
                if (!node || node.nodeType == 1) {
                    return false;
                }
                return (node.tagName == "IMG");
            }
        },

        getText: function () {
            if (this._window.document.selection) {
                var range = this._window.document.selection.createRange();
                if (range.length) {
                    return "";
                } else {
                    if (null != range.text) {
                        return range.text;
                    }
                }
            } else {
                if (this._window.getSelection) {
                    var sel = this._window.getSelection();
                    return sel ? sel.toString() : "";
                } else {
                    return "";
                }
            }
        },

        getHtml: function () {
            var range = null;
            if (this._window.document.selection && !window.opera) {
                range = this._window.document.selection.createRange();
                if (range.length) {
                    return range.item(0).outerHTML;
                } else {
                    if (range.htmlText) {
                        return range.htmlText;
                    } else {
                        return "";
                    }
                }
            } else {
                if (this._window.getSelection) {
                    var sel = this._window.getSelection();
                    if (null == sel) {
                        return "";
                    }
                    if (sel.getRangeAt && typeof (sel.rangeCount) != "undefined" && sel.rangeCount == 0) {
                        return "";
                    }
                    if (sel.getRangeAt) {
                        range = sel.getRangeAt(0);
                        var elem = this._window.document.createElement("div");
                        var contents = range.cloneContents();
                        if (contents) {
                            elem.appendChild(contents);
                            return elem.innerHTML;
                        } else {
                            return "";
                        }
                    } else {
                        return sel;
                    }
                } else {
                    return "";
                }
            }
        },

        moveToElementText: function (el) {
            if (!el) {
                return false;
            }
            var range;
            if (this.isSafari) {
                range = this._window.document.createRange();
                range.selectNodeContents(el);
                this.selectRange(range);
                this.collapse(true);
            } else {
                if (this.isIE) {
                    range = this.getRange();
                    //var a = $telerik.getLocation(el);
                    //range.moveToPoint(a.x, a.y);
                    var pos = $(el).offset();
                    range.moveToPoint(pos.left, pos.top);
                    range.select();
                } else {
                    range = this.getRange();
                    range.selectNodeContents(el);
                    this.selectRange(range);
                    this.collapse(true);
                }
            }
        },

        collapse: function (start) {
            start = (start == true);
            if (this._window.document.selection) {
                var range = this._window.document.selection.createRange();
                if (range.collapse) {
                    range.collapse(start);
                    range.select();
                }
            } else {
                if (this._window.getSelection) {
                    var sel = this._window.getSelection();
                    if (!sel.isCollapsed) {
                        if (start) {
                            sel.collapseToStart();
                        } else {
                            sel.collapseToEnd();
                        }
                    }
                }
            }
        },

        isCollapsed: function (range) {
            if (!range)
                range = this.getRange();

            if (!range || range.item)
                return false;

            var sel = this.getBrowserSelection();
            return !sel || range.boundingWidth == 0 || range.collapsed;
        },

        compareRanges: function (rng1, rng2) {
            if (rng1 && rng2) {
                // Both are control ranges and the selected element matches
                if (rng1.item && rng2.item && rng1.item(0) === rng2.item(0))
                    return 1;

                // Both are text ranges and the range matches
                if (rng1.isEqual && rng2.isEqual && rng2.isEqual(rng1))
                    return 1;
            }

            return 0;
        },

        // copy from google closure-library
        clearSelection: function () {
            var sel = this.getBrowserSelection();
            if (!sel) {
                return;
            }
            if (sel.empty) {
                // We can't just check that the selection is empty, becuase IE
                // sometimes gets confused.
                try {
                    sel.empty();
                } catch (e) {
                    // Emptying an already empty selection throws an exception in IE
                }
            } else {
                sel.removeAllRanges();
            }
        },

        // copy from google closure-library
        hasSelection: function () {
            var sel = this.getBrowserSelection();
            return !!sel && (this.isIE ? sel.type != 'None' : !!sel.rangeCount);
        },

        createBookmark: function () {
            var doc = this._window.document;
            var range = this.getRange();
            var getHtml = function (id) { return '<span id="' + id + '" style="overflow:hidden;line-height:0px;">\uFEFF</span>' };
            // ie standard
            if (this.isIE) {
                if (range.item) {
                    // control selection
                    var item = range.item(0);
                    var name = element.nodeName, index;
                    var elems = doc.getElementsByTagName(name);
                    for (var i = 0; i < elems.length; i++) {
                        if (elems[i] == item) {
                            index = i;
                            break;
                        }
                    }
                    return {
                        name: name,
                        index: index
                    };
                } else {
                    // text selection
                    var bm = { id: '_bookmark_id' };
                    var point = range.duplicate();
                    point.collapse(false);
                    point.pasteHTML(getHtml(bm.id));
                    return bm;
                }
            }
            // w3c standard
            else {
                var insertNode = function (id, cb) {
                    var d = doc.createElement('div');
                    doc.body.appendChild(d);
                    d.innerHTML = getHtml(id);
                    cb(d.firstChild);
                    doc.body.removeChild(d);
                };
                var bm = { id: '_bookmark_id' };
                var point = range.cloneRange();
                point.collapse(false);
                insertNode(bm.id, function (n) { point.insertNode(n); });
                return bm;
            }
        },

        moveToBookmark: function (bm) {
            if (!bm) { return; }
            var doc = this._window.document;
            // ie standard
            if (this.isIE) {
                if (bm.name) {
                    // control selection
                    var r = doc.body.createControlRange();
                    var el = doc.getElementsByTagName(bm.name)[bm.index];
                    if (el) {
                        r.addElement(el);
                        this.selectRange(r);
                    }
                } else {
                    // text selection
                    var point = doc.getElementById(bm.id);
                    if (point) {
                        var r = doc.body.createTextRange();
                        r.moveToElementText(point);
                        r.collapse(true);
                        this.selectRange(r);
                    }
                }
            }
            // w3c standard
            else {
                var point = doc.getElementById(bm.id);
                if (point) {
                    var r = doc.createRange();
                    r.setStartBefore(point);
                    r.setEndBefore(point);
                    r.collapse(true);
                    this.selectRange(r);
                }
            }
        },

        removeBookmark: function (bm) {
            if (!bm || !bm.id) { return; }
            var doc = this._window.document;
            var point = doc.getElementById(bm.id);
            if (point) { point.parentNode.removeChild(point); }
        }
    };

    selectionClass.utils = {

        getStoredOriginalPathsAndAttributes: function (html) {
            var fn = function (l, o, p, f, g, h, i, m, k) {
                if (!f) {
                    f = "";
                    g = g + i;
                    var n = g.search(/(\s|>)/gi);
                    if (n > 0) {
                        i = g.substring(n, g.length);
                        g = g.substring(0, n);
                        if (g == '""') {
                            g = "";
                            f = '"';
                        }
                    } else {
                        return l;
                    }
                }
                return o + " " + p + "=" + f + g + f + ' originalAttribute="' + p + '" originalPath="' + g + '"' + i;
            };
            var exp = new RegExp("(<[^>]*?)\\s(href|src)\\s*=\\s*('|\")?([^>]+?)(\\3)([^>]*?>)", "ig");
            html = html.replace(exp, fn);
            var exp1 = new RegExp('(<!--[^(-->)]+) originalAttribute="(?:href|src)" originalPath="[^"]+"([\\s\\S]*?-->)', "ig");
            var len = html.length + 1;
            while (html.length < len) {
                len = html.length;
                html = html.replace(exp1, "$1$2");
            }
            return html;
        },

        restoreOriginalPathsAndAttributes: function (el) {
            var elems = el.getElementsByTagName("*");
            for (var i = 0; i < elems.length; i++) {
                var item = elems[i];
                var path = item.getAttribute("originalPath");
                var attr = item.getAttribute("originalAttribute");
                if (path != null && attr != null) {
                    item.removeAttribute("originalPath");
                    item.removeAttribute("originalAttribute");
                    if (path.toLowerCase().indexOf("mailto:") == 0) {
                        continue;
                    }
                    path = path.replace(window.location.href + "#", "#");
                    var html = item.innerHTML;
                    item.setAttribute(attr, path, 0);
                    if (html != item.innerHTML) {
                        item.innerHTML = html;
                    }
                }
            }
        },

        setElementInnerHtml: function (el, html) {
            var newHtml = ctx.isIE ? this.getStoredOriginalPathsAndAttributes(html) : html;
            el.innerHTML = "<span>&nbsp;</span>" + newHtml;
            el.removeChild(el.firstChild);
            if (ctx.isIE) {
                this.restoreOriginalPathsAndAttributes(el);
            }
        },

        selectElement: function (win, el) {
            if (!el) {
                return;
            }
            var doc = win.document;
            var range = null;
            if (ctx.isIE) {
                switch (el.tagName) {
                    case "TABLE":
                    case "IMG":
                    case "HR":
                    case "INPUT":
                        range = doc.body.createControlRange();
                        range.add(el);
                        break;

                    case "UL":
                    case "OL":
                        range = doc.body.createTextRange();
                        range.moveToElementText(el);
                        var g = range.parentElement();
                        if (g.tagName != "UL" || g.tagName != "OL") {
                            range.moveEnd("character", -1);
                        }
                        break;

                    default:
                        range = doc.body.createTextRange();
                        range.moveToElementText(el);
                        break;
                }
                if (range) {
                    try {
                        range.select();
                        return true;
                    } catch (d) {
                        return false;
                    }
                }
            } else {
                if (win.getSelection) {
                    range = doc.createRange();
                    range.selectNode(el);
                    if (window.opera) {
                        range.selectNodeContents(el);
                    }
                    var sel = win.getSelection();
                    if (ctx.isSafari) {
                        if (el.tagName != "TD" && el.tagName != "TH") {
                            sel.setBaseAndExtent(range.startContainer, range.startOffset, range.endContainer, range.endOffset);
                        }
                    } else {
                        sel.removeAllRanges();
                        sel.addRange(range);
                    }
                    return true;
                }
            }
            return false;
        }
    };

    // register
    ctx.selectionClass = selectionClass;

})(yardi);
