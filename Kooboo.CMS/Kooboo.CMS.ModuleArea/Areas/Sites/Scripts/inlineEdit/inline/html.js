/*
*
* inline html edit
* author: ronglin
* create date: 2011.02.09
*
*/

(function ($) {

    var htmlClass = function (config) {
        htmlClass.superclass.constructor.call(this, config);
    };

    yardi.extend(htmlClass, yardi.fieldClass, {

        startNode: null, endNode: null, spaceHolder: null,

        initialize: function () {
            if (!this.endNode) {
                var find = yardi.widgetMask.prototype.findEndNode;
                this.endNode = find.call({ startNode: this.startNode });
            }
            htmlClass.superclass.initialize.call(this);
        },

        createMenu: function () {
            var self = this;
            this.menu = new yardi.htmlMenuAnchorBar({
                title: 'Html', //this.params.positionName,
                startNode: this.startNode,
                endNode: this.endNode,
                renderTo: yardi.cacheCon,
                onEdit: function () { self.edit(); }
            });
        },

        getParams: function () {
            return {
                dataType: this.startNode.attr('dataType'),
                positionId: this.startNode.attr('positionId'),
                positionName: this.startNode.attr('positionName')
            };
        },

        ensureEditSpace: function () {
            this.removeEditSpace();
            var empty = true;
            var start = this.startNode.get(0).nextSibling, end = this.endNode.get(0);
            while (start != end) {
                if (start.nodeType !== 8) { // comment node
                    var txt = start.innerText || start.textContent || start.nodeValue || '';
                    if (txt.trim() != '' && txt.length > 0) {
                        empty = false;
                        break;
                    }
                }
                start = start.nextSibling;
            }
            if (empty) {
                var fn = yardi.widgetMask.prototype.genStyle;
                var pos = fn.call({
                    startNode: this.startNode,
                    endNode: this.endNode
                });
                if (!pos.top || pos.originalWidth < 40 || pos.originalHeight < 20) {
                    // add a white space in the holder is necessary, for cause the holder have line-height.
                    this.spaceHolder = $('<var class="kb-field-empty">&nbsp;</var>').css('display', 'inline-block');
                    this.spaceHolder.insertAfter(this.startNode);
                }
            }
        },

        removeEditSpace: function () {
            if (this.spaceHolder) {
                this.spaceHolder.remove();
                this.spaceHolder = null;
            }
        },

        queryCssDisplay: function (node) {
            if (node.nodeType === 8) { return undefined; } // comment node
            var display = null;
            if (node.nodeType === 3) { // text node
                display = 'inline-block';
            } else if (node.nodeType === 1) { // element node
                if (node.tagName == 'BR') {
                    display = 'block';
                } else {
                    display = $(node).css('display');
                }
            }
            return display;
        },

        queryCssFloat: function (node) {
            if (node.nodeType === 1) { // element node
                return $(node).css('float');
            } else {
                return undefined;
            }
        },

        wrap: function () {
            // judge display style
            var displayBlock, firstDisplay, lastDisplay;
            var start = this.startNode.get(0), end = this.endNode.get(0);
            var next = start.nextSibling;
            while (next != end) {
                firstDisplay = this.queryCssDisplay(next);
                if (firstDisplay !== undefined) { break; }
                next = next.nextSibling;
            }
            var prev = end.previousSibling;
            while (prev != start) {
                lastDisplay = this.queryCssDisplay(prev);
                if (lastDisplay !== undefined) { break; }
                prev = prev.previousSibling;
            }
            firstDisplay = (firstDisplay || '').toLowerCase();
            lastDisplay = (lastDisplay || '').toLowerCase();
            displayBlock = (firstDisplay === 'block' || lastDisplay === 'block');
            // judge float style
            var floatValue; next = start.nextSibling;
            while (next != end) {
                floatValue = this.queryCssFloat(next);
                if (!floatValue) { break; }
                next = next.nextSibling;
            }
            // ensure firefox use DIV as editor element, 
            // in firefox there ara some problems in other element types.
            // eg: SPAN within A will cause the content can not be removed.
            var markup = $.browser.mozilla ? '<div></div>' : '<span></span>';
            var wrapper = $(markup).insertAfter(this.startNode);
            if (displayBlock) { wrapper.css('display', 'block'); }
            if (floatValue) { wrapper.css('float', floatValue); }
            // append content
            var wrapperDom = wrapper.get(0);
            while (wrapperDom.nextSibling != end)
                wrapper.append(wrapperDom.nextSibling);
            return wrapper;
        }
    });

    // register
    yardi.htmlClass = htmlClass;

})(jQuery);
