/*
*
* index
* author: ronglin
* create date: 2011.01.27
*
*/

(function ($) {

    var options = {
        blocks: '[editType="list"]',
        fields: '[editType="field"]',
        labels: '[editType="label"]',
        editHtml: '[editType="html"]',
        htmlBlock: '[editType="htmlBlock"]'
    };

    yardi.rootPath = 'Areas/Sites/Scripts/inlineEdit/';

    yardi.initInlineEditing = function (context) {
        context = context || document.body;
        var create = function (el, key, fn) {
            if (el.data(key)) { return; }
            el.data(key, fn.call(el));
        };

        // field initalization
        $(options.fields, context).each(function () {
            create($(this), 'field', function () {
                return (this.get(0).tagName === 'VAR' && $(this).attr('start') === '') ?
                new yardi.htmlFieldClass({
                    startNode: $(this),
                    updateAction: __inlineEditVars.UpdateContent
                }) :
                new yardi.fieldClass({
                    el: $(this),
                    updateAction: __inlineEditVars.UpdateContent
                });
            });
        });

        // block initalization
        $(options.blocks, context).each(function () {
            $(this).addClass('kb-block');
            create($(this), 'block', function () {
                return new yardi.blockClass({
                    el: $(this),
                    copyAction: __inlineEditVars.CopyContent,
                    updateAction: __inlineEditVars.UpdateContent,
                    deleteAction: __inlineEditVars.DeleteContent
                });
            });
        });

        // label initalization
        $(options.labels, context).each(function () {
            create($(this), 'label', function () {
                return new yardi.labelClass({
                    startNode: $(this),
                    updateAction: __inlineEditVars.UpdateLable
                });
            });
        });

        // html initalization
        $(options.editHtml, context).each(function () {
            create($(this), 'html', function () {
                return new yardi.htmlClass({
                    startNode: $(this),
                    updateAction: __inlineEditVars.UpdateHtml
                });
            });
        });

        // htmlBlock initalization
        $(options.htmlBlock, context).each(function () {
            create($(this), 'htmlBlock', function () {
                return new yardi.htmlBlockClass({
                    startNode: $(this),
                    updateAction: __inlineEditVars.UpdateHtmlBlock
                });
            });
        });
    };

    $(function () {

        // virtual path
        yardi.rootPath = __inlineEditVars.ApplicationPath + yardi.rootPath;

        // create cache container
        yardi.cacheCon = $('<var class="kb-cache"></var>').appendTo('body');

        // initialize
        yardi.initInlineEditing();
    });

    // an custom entrance for iframe environment in dialog. eg:top._jQuery
    if (!window._jQuery) { window._jQuery = $; }
    window._jQueryOfInlineEditing = $;

    // attention:
    // this must be called at the last of component,
    // this file was loaded at last and this function place at the end.
    // resolve the confilict problem, when there ara more then one jquery instance in the page.
    $.noConflict(true);

})(jQuery);
