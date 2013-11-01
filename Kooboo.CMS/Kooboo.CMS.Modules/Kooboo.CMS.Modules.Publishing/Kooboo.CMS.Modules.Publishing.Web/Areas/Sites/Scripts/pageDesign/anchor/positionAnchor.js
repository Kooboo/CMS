/*
*
* position anchor
* author: ronglin
* create date: 2010.11.12
*
*/


/*
* config parameters:
* alignTo, renderTo, title
* onAddView, onAddModule, onAddFolder, onAddHtml, onAddHtmlBlock
*/

(function ($) {

    // text resource
    var options = {
        addViewTitle: 'Add a view',
        addModuleTitle: 'Add a module',
        addFolderTitle: 'Add a data folder',
        addHtmlTitle: 'Add HTML',
        addHtmlBlockTitle: 'Add a HTML block'
    };

    // override text resource
    if (window.__designer) { $.extend(options, __designer.positionAnchor_js); }

    /*
    * position anchor
    */
    var positionAnchor = function (config) {
        positionAnchor.superclass.constructor.call(this, config);
    };

    yardi.extend(positionAnchor, yardi.anchor, {

        components: null,

        // btn events
        onAddView: function () { },
        onAddModule: function () { },
        onAddFolder: function () { },
        onAddHtml: function () { },
        onAddHtmlBlock: function () { },
        onAddProxy: function () { },
        title: null,

        buildHtml: function () {
            var html = [];
            html.push('<var class="kb-positionAnchor" onselectstart="return false;">');
            html.push('</var>');
            return html.join('');
        },

        initialize: function () {
            positionAnchor.superclass.initialize.call(this);
            var self = this;

            // render btns
            var tools = (this.alignTo.attr('tools') || '').toLowerCase();
            var split = '<var class="kb-sep">&nbsp;</var>';
            this.components = {};

            // add view
            if (tools.indexOf('view,') !== -1) {
                this.components.btnAddView = new yardi.imageButton({
                    title: options.addViewTitle,
                    renderTo: this.el,
                    imageClass: 'btnAddView',
                    onClick: function (ev) { self.onAddView(); }
                });
                this.el.append(split);
            }

            // add module
            if (tools.indexOf('module,') !== -1) {
                this.components.btnAddModule = new yardi.imageButton({
                    title: options.addModuleTitle,
                    renderTo: this.el,
                    imageClass: 'btnAddModule',
                    onClick: function (ev) { self.onAddModule(); }
                });
                this.el.append(split);
            }

            if (tools.indexOf('htmlblock,') !== -1) {
                // add html block
                this.components.btnAddHtmlBlock = new yardi.imageButton({
                    title: options.addHtmlBlockTitle,
                    renderTo: this.el,
                    imageClass: 'btnAddHtmlBlock',
                    onClick: function (ev) { self.onAddHtmlBlock(); }
                });
                this.el.append(split);
            }

            // add folder
            if (tools.indexOf('folder,') !== -1) {
                this.components.btnAddFolder = new yardi.imageButton({
                    title: options.addFolderTitle,
                    renderTo: this.el,
                    imageClass: 'btnAddFolder',
                    onClick: function (ev) { self.onAddFolder(); }
                });
                this.el.append(split);
            }

            // add html
            if (tools.indexOf('html,') !== -1) {
                this.components.btnAddHtml = new yardi.imageButton({
                    title: options.addHtmlTitle,
                    renderTo: this.el,
                    imageClass: 'btnAddHtml',
                    onClick: function (ev) { self.onAddHtml(); }
                });
                this.el.append(split);
            }

            // add proxy
            if (tools.indexOf('proxy,') !== -1) {
                this.components.btnProxy = new yardi.imageButton({
                    title: options.addProxyTitle,
                    renderTo: this.el,
                    imageClass: 'btnAddProxy',
                    onClick: function (ev) { self.onAddProxy(); }
                });
                this.el.append(split);
            }

            // add name
            this.el.append('<var class="kb-name"></var>');
            this.setTitle(this.title);

            // fix class name
            for (var key in this.components) {
                this.components[key].el.removeClass('kb-imagebutton').addClass('kb-btn');
                this.components[key].highlightCss = 'kb-btnhl';
            }
        },

        setTitle: function (title) {
            this.el.find('.kb-name').html(title);
        },

        lock: function (isLock) {
            this.fixed = (isLock === true);
        },

        getAlignCss: function (ev) {
            var firstChild, left;
            this.alignTo.children().each(function () {
                if ($(this).css('display') != 'none') {
                    firstChild = $(this);
                    return false;
                }
            });
            var pos = this.alignTo.offset();
            var height = this.el.outerHeight();
            if (firstChild) {
                left = firstChild.offset().left;
            } else {
                left = pos.left;
            }
            if (pos.top < height) {
                return {
                    left: left,
                    top: pos.top + this.alignTo.outerHeight()
                };
            } else {
                return {
                    left: left,
                    top: pos.top - height
                };
            }
        },

        beforeShow: function () {
            this.alignTo.addClass('kb-positionAnchor-anchorto');
        },

        beforeHide: function () {
            this.alignTo.removeClass('kb-positionAnchor-anchorto');
        }
    });

    // register
    yardi.positionAnchor = positionAnchor;

})(jQuery);
