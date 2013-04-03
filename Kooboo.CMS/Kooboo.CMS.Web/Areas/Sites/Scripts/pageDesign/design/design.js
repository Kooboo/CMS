/*
*
* design
* author: ronglin
* create date: 2010.11.25
*
*/

(function ($) {

    // text resource
    var options = {
        removeConfirm: 'Are you sure you want to remove?',
        // add
        addViewTitle: 'Add a view',
        addHtmlTitle: 'Add HTML',
        addModuleTitle: 'Add a module',
        addFolderTitle: 'Add a data folder',
        addHtmlBlockTitle: 'Add a HTML block',
        // edit
        editViewTitle: 'Edit view',
        editHtmlTitle: 'Edit HTML',
        editModuleTitle: 'Edit module',
        editFolderTitle: 'Edit data folder',
        editHtmlBlockTitle: 'Edit HTML block',
        // btn
        editBtnTitle: 'Edit',
        removeBtnTitle: 'Remove',
        expandBtnTitle: 'Expand',
        closeBtnTitle: 'Close',
        moveBtnTitle: 'Move'
    };

    // override text resource
    if (window.__designer) { $.extend(options, __designer.design_js); }

    /*
    * position design class
    */
    var positionDesign = function (options, el) {
        this.settings = $.extend({}, options);
        this.el = el;
        this.initialize();
    };

    var getParameter = function (el) {
        var ret = {};
        var names = el.attr('ParameterNames').split(',');
        $.each(names, function (i, n) {
            if (!n) { return; }
            var val = el.attr(n);
            if (val != null && val != undefined) { ret[n] = val; }
        });
        return ret;
    };

    var addVariable = {
        view: { urlKey: 'processViewUrl', title: options.addViewTitle },
        html: { urlKey: 'processHtmlUrl', title: options.addHtmlTitle },
        module: { urlKey: 'processModuleUrl', title: options.addModuleTitle },
        folder: { urlKey: 'processFolderUrl', title: options.addFolderTitle },
        htmlBlock: { urlKey: 'processHtmlBlockUrl', title: options.addHtmlBlockTitle }
    };

    var editVariable = {
        view: { urlKey: 'processViewUrl', title: options.editViewTitle },
        html: { urlKey: 'processHtmlUrl', title: options.editHtmlTitle },
        module: { urlKey: 'processModuleUrl', title: options.editModuleTitle },
        folder: { urlKey: 'processFolderUrl', title: options.editFolderTitle },
        htmlBlock: { urlKey: 'processHtmlBlockUrl', title: options.editHtmlBlockTitle }
    };

    positionDesign.prototype = {

        el: null,

        settings: null,

        menuBar: null,

        initialize: function () {
            // reference entrance
            this.el.data('instance', this);

            // create menu bar
            var self = this;
            this.menuBar = new yardi.positionAnchor({
                alignTo: this.el,
                renderTo: this.settings.cacheCon,
                title: this.el.attr('LayoutPositionId'),
                onAddView: function () { self.addComponentDialog('view'); },
                onAddModule: function () { self.addComponentDialog('module'); },
                onAddFolder: function () { self.addComponentDialog('folder'); },
                onAddHtml: function () { self.addComponentDialog('html'); },
                onAddHtmlBlock: function () { self.addComponentDialog('htmlBlock'); }
            });

            // create content component
            this.travelContent(function (i, o) {
                var key = $(o).attr('Type');
                var com = new content.types[key]({ el: $(o) });
            }, false);

            // drop
            this.el.droppable(yardi.designCtx.dropSetting);

            // order
            this.doOrder();

            // watermark
            var watermark = function () { self.doWatermark(); };
            var redoundo = yardi.designCtx.redoundo;
            redoundo.onUndo.add(watermark);
            redoundo.onRedo.add(watermark);
            redoundo.onCommit.add(watermark);
            watermark();

            // show
            this.el.css('display', 'block'); //this.el.show();
        },

        travelContent: function (handler, filter) {
            this.el.find('.pagedesign-content').each(function (i, o) {
                // filter
                if ((this.style.display == 'none' || // fix bug in ff, when the designer tab is hided, $(this).css('display') return a undefined value.
                   $(this).css('display') == 'none') && filter !== false) { return; }
                // handler
                handler && handler.call(this, i, o);
            });
        },

        removeComponent: function (component) {
            if (confirm(options.removeConfirm)) {
                component.hide(function () {
                    // commit history
                    yardi.designCtx.redoundo.commit({
                        name: 'Remove ' + component.Name,
                        undo: function (o) {
                            return function (ev) { o.show(function () { ev.done(); }); };
                        } (component),
                        redo: function (o) {
                            return function (ev) { o.hide(function () { ev.done(); }); }
                        } (component)
                    });
                });
            }
        },

        addComponent: function (html, callback) {
            // insert elem
            var elem = $(html).hide();
            var order = parseInt(elem.attr('Order')) || 0;
            var childs = this.el.children();
            if (childs.length === 0) {
                this.el.append(elem);
            } else {
                for (var i = childs.length - 1; i > -1; i--) {
                    var ord = parseInt($(childs[i]).attr('Order')) || 0;
                    if (order >= ord) {
                        elem.insertAfter(childs[i]);
                        break;
                    }
                }
            }

            // create component
            var key = elem.attr('Type'), self = this;
            var component = new content.types[key]({ el: elem, isNew: true });
            yardi.designCtx.resetCss(elem);

            // callback
            callback(component);
        },

        editComponentDialog: function (item) {
            var self = this, variable = editVariable[item.Type];
            yardi.designCtx.showDialog({
                url: this.settings[variable.urlKey] + '&IsEdit=True',
                title: variable.title
            }, function (html) {
                self.addComponent(html, function (component) {
                    // is changed
                    var changed = false;
                    var set1 = item.getSettings(), set2 = component.getSettings();
                    $.each(set1, function (k, v) {
                        if (set2[k] != v) {
                            changed = true;
                            return false;
                        }
                    });
                    if (!changed) {
                        $.each(set2, function (k, v) {
                            if (set1[k] != v) {
                                changed = true;
                                return false;
                            }
                        });
                    }
                    if (!changed) {
                        component.remove();
                        return;
                    }
                    item.hide('direct');
                    component.show('direct');
                    // commit history
                    var delegate = function (o1, o2) {
                        return function (ev) {
                            yardi.designCtx.scrollToWidget(o1.el, function () {
                                o1.hide('direct');
                                o2.show('direct');
                                ev.done();
                            });
                        };
                    };
                    yardi.designCtx.redoundo.commit({
                        name: 'Edit ' + component.Name,
                        undo: delegate(component, item),
                        redo: delegate(item, component)
                    });
                });
            }, {
                onValid: function (postData) { return true; },
                onGetValue: function () { return item.getSettings(); },
                onGetParent: function () { return $; }
            });
        },

        addComponentDialog: function (type) {
            var self = this, variable = addVariable[type];
            yardi.designCtx.showDialog({
                url: this.settings[variable.urlKey] + '&IsEdit=False',
                title: variable.title
            }, function (html) {
                self.addComponent(html, function (component) {
                    component.show(function () {
                        // order
                        //component.getPosition().doOrder(); // component.show had ordered
                        // commit history
                        yardi.designCtx.redoundo.commit({
                            name: 'Add ' + component.Name,
                            undo: function (o) {
                                return function (ev) { o.hide(function () { ev.done(); }); };
                            } (component),
                            redo: function (o) {
                                return function (ev) { o.show(function () { ev.done(); }); }
                            } (component)
                        });
                    });
                });
            }, {
                onValid: function (postData) { return true; },
                onGetParent: function () { return $; }
            });
        },

        doOrder: function () {
            var order = 0;
            this.travelContent(function () {
                $(this).attr('Order', order++);
            });
        },

        doWatermark: function () {
            var count = 0;
            this.travelContent(function () {
                count++;
            });
            if (count == 0) {
                this.el.addClass('pagedesign-watermark');
            } else {
                this.el.removeClass('pagedesign-watermark');
            }
        },

        getSettings: function (msg) {
            var settings = [];
            var o = getParameter(this.el);
            this.travelContent(function () {
                // new order
                $(this).attr('Order', settings.length);
                // get item setting
                var set = $(this).data('instance').getSettings();
                settings.push($.extend(set, o));
            });
            // ret
            return settings;
        }
    };
    // register
    yardi.positionDesign = positionDesign;


    /*
    * content class
    */
    var content = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    var createBtn = function (cls, title, click) {
        return $('<var class="kb-btn ' + cls + '" title="' + title + '"></var>').click(click).hover(function () {
            $(this).addClass('kb-btnhl');
        }, function () {
            $(this).removeClass('kb-btnhl');
        });
    };

    content.types = {};

    content.register = function (key, type) { content.types[key] = type; };

    content.prototype = {

        el: null, settings: null, canDrag: false, isNew: false,

        btnMove: null, btnEdit: null, btnRemove: null, btnTitle: null,

        Name: null, Type: null,

        initialize: function () {
            // reference entrance
            var self = this;
            this.el.data('instance', this);

            // server parameters
            this.Name = this.el.attr('Name');
            this.Type = this.el.attr('Type');

            var split = '<var class="kb-sep"></var>';

            // btn move
            this.btnMove = $('<var class="kb-movebtn" title="' + options.moveBtnTitle + '"></var>').appendTo(this.el);
            yardi.designCtx.disableSelection(this.btnMove);
            this.el.append(split);

            // btn edit
            this.btnEdit = createBtn('kb-editbtn', options.editBtnTitle, function () {
                self.getPosition().editComponentDialog(self);
            }).appendTo(this.el);
            this.el.append(split);

            // btn remove
            this.btnRemove = createBtn('kb-removebtn', options.removeBtnTitle, function () {
                self.getPosition().removeComponent(self);
            }).appendTo(this.el);
            this.el.append(split);

            // other btns
            this.appendOtherBtn(this.el, split);

            // content name
            this.el.append('<var class="kb-title" title="' + this.Name + '">' + this.Name + '</var>');
            this.btnTitle = $('.kb-title', this.el);

            // drag and drop
            this.el.draggable(yardi.designCtx.dragSetting).droppable(yardi.designCtx.dropSetting);

            // can drag
            this.el.mousedown(function (ev) {
                self.canDrag = $(ev.target).hasClass('kb-movebtn');
                self.el.data('canDrag', self.canDrag);
            });

            // show
            this.el.css('display', 'block'); //this.el.show();
        },

        // virtual method
        appendOtherBtn: function (renderTo, split) { },

        getPosition: function () {
            return this.el.parent().data('instance');
        },

        hide: function (callback) {
            if (callback === 'direct') {
                this.el.hide();
                return;
            }
            var self = this;
            yardi.designCtx.scrollToWidget(this.el, function (scrolled) {
                var fn = function () {
                    self.highlight(function () {
                        self.el._slideUp(function () {
                            self.getPosition().doOrder();
                            callback && callback();
                        });
                    });
                };
                scrolled ? setTimeout(fn, 300) : fn();
            });
        },

        show: function (callback) {
            if (callback === 'direct') {
                this.el.css('display', 'block'); //this.el.show();
                this.highlight();
                this.getPosition().doOrder();
                return;
            }
            var self = this;
            yardi.designCtx.scrollToWidget(this.el, function (scrolled) {
                var fn = function () {
                    self.el._slideDown(function () {
                        self.highlight();
                        self.getPosition().doOrder();
                        callback && callback();
                    });
                };
                scrolled ? setTimeout(fn, 300) : fn();
            });
        },

        remove: function () {
            this.btnMove.remove();
            this.btnEdit.remove();
            this.btnRemove.remove();
            this.btnTitle.remove();
            this.el.remove();
        },

        highlight: function (callback) {
            var self = this;
            var mask = new yardi.widgetMask({
                containerNode: this.el,
                cssClass: 'content-highlight'
            });
            mask.mask();
            setTimeout(function () {
                mask.remove();
                callback && callback.call(self);
            }, 500);
        },

        getSettings: function () {
            return getParameter(this.el);
        }
    };

    /*
    * view content
    */
    var viewContent = function (config) {
        viewContent.superclass.constructor.call(this, config);
    };
    yardi.extend(viewContent, content, {
        initialize: function () {
            viewContent.superclass.initialize.call(this);
        }
    });
    content.register('view', viewContent);

    /*
    * module content
    */
    var moduleContent = function (config) {
        moduleContent.superclass.constructor.call(this, config);
    };
    yardi.extend(moduleContent, content, {
        initialize: function () {
            moduleContent.superclass.initialize.call(this);
        }
    });
    content.register('module', moduleContent);

    /*
    * html content
    */
    var htmlContent = function (config) {
        htmlContent.superclass.constructor.call(this, config);
    };
    yardi.extend(htmlContent, content, {
        btnZoom: null,
        htmlCon: null,
        dynamicName: true, // for htmlBlock config used
        initialize: function () {
            htmlContent.superclass.initialize.call(this);
            this.htmlCon = $('<var class="kb-htmlcon"></var>').appendTo(this.el);
            if (!this.isNew) {
                this.processHtml();
                this.detailStatus(true, false);
            }
        },
        show: function (callback) {
            this.processHtml();
            this.detailStatus(true, false);
            htmlContent.superclass.show.call(this, callback);
        },
        hide: function (callback) {
            if (callback === 'direct') {
                htmlContent.superclass.hide.call(this, callback);
                this.htmlCon.empty();
            } else {
                var self = this;
                htmlContent.superclass.hide.call(this, function () {
                    self.htmlCon.empty();
                    if (callback) { callback.apply(self, arguments); }
                });
            }
        },
        detailStatus: function (show, animate) {
            if (animate) {
                this.htmlCon[show ? '_slideDown' : '_slideUp']();
            } else {
                this.htmlCon.css('display', show ? 'block' : 'none');
            }
            this.btnZoom.removeClass('kb-downbtn kb-upbtn').addClass(show ? 'kb-upbtn' : 'kb-downbtn');
            this.btnZoom.attr('title', show ? options.closeBtnTitle : options.expandBtnTitle);
            if (this.dynamicName) {
                if (yardi.isGecko) {
                    // bug in firefox
                    // move away it and then move it back immediately
                    // this force the dom to rerender the element.
                    this.btnTitle.appendTo('body');
                    this.btnTitle.html(show ? '&nbsp;' : this.Name);
                    this.btnTitle.insertBefore(this.htmlCon);
                } else {
                    this.btnTitle.html(show ? '&nbsp;' : this.Name);
                }
            }
        },
        appendOtherBtn: function (renderTo, split) {
            var self = this;
            this.btnZoom = createBtn('', '', function () {
                self.detailStatus(self.htmlCon.css('display') == 'none', true);
            }).appendTo(renderTo);
            renderTo.append(split);
        },
        processHtml: function () {
            var elem = this.htmlCon.get(0);
            yardi.rawHtml(elem, this.el.attr('Html'));
        }
    });
    content.register('html', htmlContent);

    /*
    * folder content
    */
    var folderContent = function (config) {
        folderContent.superclass.constructor.call(this, config);
    };
    yardi.extend(folderContent, content, {
        initialize: function () {
            folderContent.superclass.initialize.call(this);
        }
    });
    content.register('folder', folderContent);

    /*
    * html block content
    */
    var htmlBlockContent = function (config) {
        htmlBlockContent.superclass.constructor.call(this, config);
    };
    yardi.extend(htmlBlockContent, htmlContent, {
        dynamicName: false,
        initialize: function () {
            htmlBlockContent.superclass.initialize.call(this);
        }
    });
    content.register('htmlBlock', htmlBlockContent);

})(jQuery);