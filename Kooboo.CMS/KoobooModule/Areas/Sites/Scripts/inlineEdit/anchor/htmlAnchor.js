/*
*   htmlAnchor
*   author: ronglin
*   create date: 2011.02.09
*/

/*
* config parameters:
* title, renderTo, onEdit, startNode, endNode
*/

(function (ctx, $) {

    // text resource
    var options = {
        editBtnTitle: 'edit'
    };

    // override text resource
    if (window.__inlineEditVars) { $.extend(options, __inlineEditVars.htmlAnchor_js); }

    /*
    * html menu
    */
    var htmlAnchor = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    htmlAnchor.prototype = {

        el: null, renderTo: null, title: null, onEdit: null,

        startNode: null, endNode: null, components: null, onShow: null,

        isMenu: true, fixed: false, showed: false, _docMousemoveDelayFn: null,

        initialize: function () {
            // create
            this.el = $(this.buildHtml()).appendTo(this.renderTo);

            // bar events
            var timeoutId, self = this;
            $(document).mousemove(this._docMousemoveDelayFn = function (ev) {
                clearTimeout(timeoutId);
                timeoutId = setTimeout(function () {
                    if (self.fixed === true) { return; }
                    if (ctx.anchor.overel === true) { return; }
                    ctx.anchor.expire() && self.checkMouseIn(ev);
                }, 64);
            });

            // hover
            this.el.hover(function () {
                ctx.anchor.overel = true;
                self.hoverIn();
            }, function () {
                ctx.anchor.overel = false;
                self.hoverOut();
            });

            // components
            this.createComponents();

            // register
            this.onShow = new ctx.dispatcher(this);
            if (this.isMenu) { ctx.anchor.regIns(this); }
        },

        buildHtml: function () {
            var html = [];
            html.push('<var class="kb-htmlAnchor" onselectstart="return false;">');
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
            this.components.btnEdit = new ctx.imageButton({
                title: options.editBtnTitle,
                renderTo: btnCon,
                imageClass: 'btnEdit',
                onClick: function (ev) { self.onEdit(); }
            });
            btnCon.append(split);

            // fix
            $.each(this.components, function () {
                this.el.removeClass('kb-imageButton').addClass('kb-btn');
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
            if (ctx.modeling == true) { return; }
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
            if (ctx.isAncestor(p, ev.target)) {
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
                var fn = ctx.widgetMask.prototype.genStyle, offset = 0;
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
                left: ev.pageX + 30,
                top: ev.pageY - 10
            };
        },

        show: function (ev) {
            if (this.fixed != true && this.showed == false) {
                this.el.css(this.getAlignCss(ev));
                this.el.fadeIn('fast');
                this.onShow.dispatch();
                this.showed = true;
                ctx.zTop(this.el);
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
                        ctx.zOld(self.el);
                    });
                }
                this.showed = false;
            }
        },

        fix: function (isFix) {
            this.fixed = (isFix === true);
        },

        remove: function () {
            $(document).unbind('mousemove', this._docMousemoveDelayFn);
            this.el.unbind().remove();
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
                ctx.anchor.hideAll(self);
                ctx.anchor.fixAll(true, self);
                //show block mask
                self.removeHtmlMask();
                self.htmlMask = new ctx.widgetMask({
                    cssClass: 'kb-field-mask',
                    startNode: self.startNode,
                    endNode: self.endNode
                });
                self.htmlMask.mask();
            }, 128);
        },

        hoverOut: function () {
            if (this.fixed === true) { return; }
            clearTimeout(this._hoverId);
            ctx.anchor.fixAll(false, this);
            this.removeHtmlMask();
        }
    };

    // register
    ctx.htmlAnchor = htmlAnchor;

})(yardi, jQuery);
