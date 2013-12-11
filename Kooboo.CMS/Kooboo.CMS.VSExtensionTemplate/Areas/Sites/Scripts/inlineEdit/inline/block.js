/*
*   block
*   author: ronglin
*   create date: 2011.01.28
*/

(function (ctx, $) {

    // text resource
    var options = {
        confirmDel: 'Are you sure you want to delete this item?',
        networkError: 'Network error, the action has been cancelled.',

        copying: 'copying...',
        deleting: 'deleting...',
        publishing: 'publishing...',

        copySuccess: 'copy successfully.',
        deleteSuccess: 'delete successfully.',
        publishSuccess: 'publish successfully.',

        copyFailure: 'The attempt to copy has failed.',
        deleteFailure: 'The attempt to delete has failed.',
        publishFailure: 'The attempt to publish has failed.'
    };

    // override text resource
    if (window.__inlineEditVars) { $.extend(options, __inlineEditVars.block_js); }

    /*
    * block class
    */
    var blockClass = function (config) {
        $.extend(this, config);
        this.initialize();
    };

    blockClass.prototype = {

        el: null,

        params: null, menu: null,

        copyAction: null, deleteAction: null, updateAction: null,

        constructor: blockClass,

        getParams: function () {
            return {
                uuid: this.el.attr('uuid'),
                schema: this.el.attr('schema'),
                folder: this.el.attr('folder'),
                published: this.el.attr('published'),
                publishAvailable: this.el.attr('publishAvailable'),
                editUrl: this.el.attr('editUrl'),
                summary: this.el.attr('summary')
            };
        },

        initialize: function () {
            this.params = this.getParams();
            // bind menu bar
            var self = this;
            this.menu = new ctx.blockAnchor({
                title: this.params.folder,
                alignTo: this.el,
                renderTo: ctx.cacheCon,
                onEdit: function () { self.doEdit(); },
                onCopy: function () { self.doCopy(); },
                onDelete: function () { self.doDelete(); },
                onPublish: function () { self.doPublish(); }
            });
            this.setPublish(this.params.published === 'True');

        },

        setPublish: function (published) {
            if (published) {
                this.el.removeClass('kb-block-unpublish');
            } else {
                this.el.addClass('kb-block-unpublish');
            }
            this.menu.enablePublish(!published && this.params.publishAvailable === 'True');
        },

        remove: function () {
            this.menu.remove();
        },

        disable: function (disabled, all) {
            this.menu.disableBtns(disabled);
            ctx.anchor.hideAll((all === true) ? undefined : this.menu);
            ctx.anchor.fixAll(disabled);
        },

        doPost: function (url, data, error, succsss) {
            var self = this;
            $.ajax({
                url: url, data: data,
                type: 'post', dataType: 'json', timeout: 30000,
                beforeSend: function (jqXHR, settings) { self.disable(true); },
                complete: function (jqXHR, textStatus) { self.disable(false); },
                success: function (data, textStatus, jqXHR) { succsss(data); },
                error: function (jqXHR, textStatus, errorThrown) { error(); }
            });
        },

        doEdit: function () {
            var self = this;
            self.disable(true, true);
            var id = new Date().getTime(), destroyMethod = function () { };
            $.pop({
                id: id,
                url: self.params.editUrl,
                dialogClass: 'iframe-dialog',
                title: self.params.summary,
                width: 800,
                height: 500,                
                popupOnTop: true,
                onclose: function () {
                    self.disable(false);
                    setTimeout(function () { destroyMethod(); }, 16);
                    //if (top.kooboo.data('parent-page-reload')) {
                    //    //kooboo.cms.ui.getOpener().reload();
                    //    window.location.reload();
                    //}
                }
            });
            //var contentCon = $('#' + id);
            //if (contentCon.length > 0) {
            //    var dialogCon = contentCon.parent('.ui-dialog').first();
            //    var maskCon = dialogCon.next('.ui-widget-overlay').first();
            //    ctx.zTop(maskCon); ctx.zTop(dialogCon);
            //    destroyMethod = function () { contentCon.remove(); };
            //}
        },

        doCopy: function () {
            var self = this;
            var message = function (msg) { self.menu.message(msg, self.menu.components.btnCopy.el); };
            //message(options.copying);
            this.doPost(this.copyAction, this.params, function () {
                message(options.copyFailure + '<br/>' + options.networkError);
            }, function (data) {
                if (!data.Success) {
                    message(options.copyFailure);
                    return;
                }
                // successful
                message(); //message(options.copySuccess);
                //setTimeout(function () {
                // copy client content
                var clone = self.el.clone().hide().insertAfter(self.el);
                $.each(data.Model, function (key, val) {
                    clone.attr(key, val);
                });
                // update parameters
                clone.find('[uuid="' + self.params.uuid + '"]').attr('uuid', data.Model.uuid);
                // bind component
                ctx.initInlineEditing(clone.wrap('<div style="display:none;"></div>').parent());
                ctx.refreshFonts(clone, self);
                clone.unwrap();
                // show
                clone.slideDown('normal');
                //}, 800);
            });
        },

        doDelete: function () {
            var self = this;
            ctx.anchor.fixAll(true);
            if (!confirm(options.confirmDel)) {
                ctx.anchor.fixAll(false);
                return;
            }
            var message = function (msg) { self.menu.message(msg, self.menu.components.btnRemove.el); };
            //message(options.deleting);
            this.doPost(this.deleteAction, this.params, function () {
                message(options.deleteFailure + '<br/>' + options.networkError);
            }, function (data) {
                if (!data.Success) {
                    message(options.deleteFailure);
                    return;
                }
                // successful
                message(); //message(options.deleteSuccess);
                //setTimeout(function () {
                // remove component
                self.remove();
                // remove element
                self.el.slideUp('normal', function () {
                    $(this).remove();
                });
                //}, 800);
            });
        },

        doPublish: function () {
            var self = this;
            if (this.params.publishAvailable !== 'True') { return; }
            var message = function (msg) { self.menu.message(msg, self.menu.components.btnPublish.el); };
            //message(options.publishing);
            var postData = $.extend({}, this.params, { fieldName: 'Published', value: 'True' });
            this.doPost(this.updateAction, postData, function () {
                message(options.publishFailure + '<br/>' + options.networkError);
            }, function (data) {
                if (!data.Success) {
                    message(options.publishFailure);
                    return;
                }
                // successful
                message(); //message(options.publishSuccess);
                //setTimeout(function () {
                self.setPublish(true);
                //}, 800);
            });
        }
    };

    // register
    ctx.blockClass = blockClass;

})(yardi, jQuery);
