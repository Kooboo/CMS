/*
*   index
*   author: ronglin
*   create date: 2011.01.27
*/

(function (ctx, $) {

    var options = {
        blocks: '[editType="list"]',
        fields: '[editType="field"]',
        labels: '[editType="label"]',
        editHtml: '[editType="html"]',
        htmlBlock: '[editType="htmlBlock"]'
    };

    ctx.rootPath = 'Areas/Sites/Scripts/inlineEdit/';

    ctx.initInlineEditing = function (context) {
        context = context || document.body;
        var create = function (el, key, fn) {
            if (el.data(key)) { return; }
            el.data(key, fn.call(el));
        };

        // field initalization
        $(options.fields, context).each(function () {
            create($(this), 'inline_edit_field', function () {
                return (this.get(0).tagName === 'VAR' && $(this).attr('start') === 'true') ?
                new ctx.htmlFieldClass({
                    startNode: $(this),
                    updateAction: __inlineEditVars.UpdateContent
                }) :
                new ctx.fieldClass({
                    el: $(this),
                    updateAction: __inlineEditVars.UpdateContent
                });
            });
        });

        // block initalization
        $(options.blocks, context).each(function () {
            $(this).addClass('kb-block');
            create($(this), 'inline_edit_block', function () {
                return new ctx.blockClass({
                    el: $(this),
                    copyAction: __inlineEditVars.CopyContent,
                    updateAction: __inlineEditVars.UpdateContent,
                    deleteAction: __inlineEditVars.DeleteContent
                });
            });
        });

        // label initalization
        $(options.labels, context).each(function () {
            create($(this), 'inline_edit_label', function () {
                return new ctx.labelClass({
                    startNode: $(this),
                    updateAction: __inlineEditVars.UpdateLable
                });
            });
        });

        // html initalization
        $(options.editHtml, context).each(function () {
            create($(this), 'inline_edit_html', function () {
                return new ctx.htmlClass({
                    startNode: $(this),
                    updateAction: __inlineEditVars.UpdateHtml
                });
            });
        });

        // htmlBlock initalization
        $(options.htmlBlock, context).each(function () {
            create($(this), 'inline_edit_htmlBlock', function () {
                return new ctx.htmlBlockClass({
                    startNode: $(this),
                    updateAction: __inlineEditVars.UpdateHtmlBlock
                });
            });
        });
    };

    $(function () {

        // virtual path
        ctx.rootPath = __inlineEditVars.ApplicationPath + ctx.rootPath;

        // create cache container
        ctx.cacheCon = $('<var class="kb-cache"></var>').appendTo('body');
        ctx.eventPropagation.stop(ctx.cacheCon);

        // initialize
        ctx.initInlineEditing();
    });

    // an custom entrance for iframe environment in dialog. eg:top._jQuery
    if (!window._jQuery) { window._jQuery = $; }
    window._jQueryOfInlineEditing = $;

    // attention:
    // this must be called at the last of component,
    // this file was loaded at last and this function place at the end.
    // resolve the confilict problem, when there ara more then one jquery instance in the page.
    $.noConflict(true);

})(yardi, jQuery);
