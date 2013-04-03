/*
*
* inline html menu anchorbar
* author: ronglin
* create date: 2011.02.09
*
*/


/*
* config parameters:
* title, renderTo, onEdit, startNode, endNode
*/

(function ($) {

    // text resource
    var options = {
        editBtnTitle: 'edit'
    };

    // override text resource
    if (window.__inlineEditVars) { $.extend(options, __inlineEditVars.htmlMenuAnchorBar_js); }

    /*
    * html menu
    */
    var htmlMenuAnchorBar = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    htmlMenuAnchorBar.prototype = {

        title: null, onEdit: null,

        startNode: null, endNode: null, renderTo: null,

        el: null, components: null,

        fixed: false, showed: false, _docMousemoveDelayFn: null,

        initialize: function () {
            // create
            this.el = $(this.buildHtml()).appendTo(this.renderTo);

            // bar events
            var timeoutId, self = this;
            this._docMousemoveDelayFn = function (ev) {
                clearTimeout(timeoutId);
                timeoutId = setTimeout(function () {
                    if (self.fixed === true) { return; }
                    if (yardi.anchorBar.overel === true) { return; }
                    yardi.anchorBar.expire() && self.checkMouseIn(ev);
                }, 50);
            };
            $(document).mousemove(this._docMousemoveDelayFn);

            // hover
            this.el.hover(function () {
                yardi.anchorBar.overel = true;
                self.hoverIn();
            }, function () {
                yardi.anchorBar.overel = false;
                self.hoverOut();
            });

            // components
            this.createComponents();

            // register
            yardi.anchorBar.instances.push(this);
        },

        buildHtml: function () {
            var html = [];
            html.push('<var class="kb-htmlMenuAnchorBar" onselectstart="return false;">');
            html.push('<var class="kb-con"></var>');
            html.push('<var class="kb-name"></var>');
            html.push('</var>');
            return html.join('');
        },

        createComponents: function () {
            this.components = {};
            var self = this, split = '<var class="kb-sep">&nbsp;</var>';
            var btnCon = this.el.find('.kb-con');

            // edit
            this.components.btnEdit = new yardi.imageButton({
                title: options.editBtnTitle,
                renderTo: btnCon,
                imageUrl: 'anchorBar/images/menu_edit.gif',
                onClick: function (ev) { self.onEdit(); }
            });
            btnCon.append(split);

            // fix
            $.each(this.components, function () {
                this.el.removeClass('kb-imagebutton').addClass('kb-btn');
                this.highlightCss = 'kb-hover';
            });

            // set name
            if (this.title) { this.setTitle(this.title); }
        },

        setTitle: function (title) {
            this.el.find('.kb-name').html(title);
        },

        checkMouseIn: function (ev) {
            if (!ev) { return; }
            if (yardi.modeling == true) { return; }
            if (this.isMouseInEl(ev)) {
                this.show(ev);
            } else {
                this.hide();
            }
        },

        isMouseInEl: function (ev) {
            var isIn = false;
            var start = this.startNode.get(0), end = this.endNode.get(0), p = end.parentNode;
            // elemnent node type
            if (yardi.isAncestor(p, ev.target)) {
                var prev = ev.target;
                while (prev) {
                    if (prev == p) { break; }
                    if (prev == end) { break; }
                    if (prev == start) {
                        isIn = true;
                        break;
                    }
                    prev = prev.previousSibling || prev.parentNode;
                }
            }
            // text node type
            if (isIn === false && ev.target == p) {
                var fn = yardi.widgetMask.prototype.genStyle, offset = 0;
                var css = fn.call({ startNode: this.startNode, endNode: this.endNode });
                if (ev.pageX + offset > css.left && ev.pageX - offset < css.left + css.width &&
                    ev.pageY + offset > css.top && ev.pageY - offset < css.top + css.height) {
                    isIn = true;
                }
            }
            // ret
            return isIn;
        },

        getAlignCss: function (ev) {
            return {
                left: ev.pageX + 15,
                top: ev.pageY - 10
            };
        },

        show: function (ev) {
            if (this.fixed != true && this.showed == false) {
                this.el.css(this.getAlignCss(ev));
                this.el.fadeIn('fast');
                this.showed = true;
                yardi.zTop(this.el);
            }
        },

        hide: function (force) {
            if (force === true || (this.fixed != true && this.showed == true)) {
                this.hoverOut();
                if (force === true) {
                    this.el.hide();
                } else {
                    var self = this;
                    this.el.fadeOut('fast', function () {
                        yardi.zOld(self.el);
                    });
                }
                this.showed = false;
            }
        },

        fix: function (isFix) {
            this.fixed = (isFix === true);
        },

        htmlMask: null,

        removeHtmlMask: function () {
            if (!this.htmlMask) { return; }
            this.htmlMask.remove();
            this.htmlMask = null;
        },

        hoverIn: function () {
            if (this.fixed === true) { return; }
            var self = this;
            clearTimeout(this._hoverId);
            this._hoverId = setTimeout(function () {
                // hide others
                yardi.anchorBar.hideAll(self);
                yardi.anchorBar.fixAll(true, self);
                //show block mask
                self.removeHtmlMask();
                self.htmlMask = new yardi.widgetMask({
                    cssClass: 'kb-field-mask',
                    startNode: self.startNode,
                    endNode: self.endNode
                });
                self.htmlMask.mask();
            }, 150);
        },

        hoverOut: function () {
            if (this.fixed === true) { return; }
            clearTimeout(this._hoverId);
            yardi.anchorBar.fixAll(false, this);
            this.removeHtmlMask();
        }
    };

    // register
    yardi.htmlMenuAnchorBar = htmlMenuAnchorBar;

})(jQuery);
