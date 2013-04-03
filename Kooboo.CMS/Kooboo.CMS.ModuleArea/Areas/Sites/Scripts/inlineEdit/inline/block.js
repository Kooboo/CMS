/*
*
* block
* author: ronglin
* create date: 2011.01.28
*
*/

(function ($) {

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
            this.menu = new yardi.blockMenuAnchorBar({
                title: this.params.schema,
                alignTo: this.el,
                renderTo: yardi.cacheCon,
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
            yardi.anchorBar.hideAll((all === true) ? undefined : this.menu);
            yardi.anchorBar.fixAll(disabled);
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
            var id = new Date().getTime();
            $.pop({
                id: id,
                url: self.params.editUrl,
                title: self.params.summary,
                width: 800,
                height: 580,
                frameHeight: '100%',
                popupOnTop: true,
                onclose: function () {
                    self.disable(false);
                    if (top.kooboo.data('parent-page-reload')) {
                        //kooboo.cms.ui.getOpener().reload();
                        window.location.reload();
                    }
                }
            });
            var contentCon = $('#' + id);
            if (contentCon.length > 0) {
                var dialogCon = contentCon.parent('.ui-dialog').first();
                var maskCon = dialogCon.next('.ui-widget-overlay').first();
                yardi.zTop(maskCon); yardi.zTop(dialogCon);
            }
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
                yardi.initInlineEditing(clone.wrap('<div style="display:none;"></div>').parent());
                yardi.refreshFonts(clone, self);
                clone.unwrap();
                // show
                clone.slideDown('normal');
                //}, 800);
            });
        },

        doDelete: function () {
            var self = this;
            yardi.anchorBar.fixAll(true);
            if (!confirm(options.confirmDel)) {
                yardi.anchorBar.fixAll(false);
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
    yardi.blockClass = blockClass;

})(jQuery);
