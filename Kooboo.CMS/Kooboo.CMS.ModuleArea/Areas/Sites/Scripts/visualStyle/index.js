/*
*   index
*   author: ronglin
*   create date: 2012.01.30
*/

(function ($, ctx) {

    // register
    ctx.create = function (config) {

        // check
        var to = config.renderTo;
        if (!to) { return undefined; }

        // components
        var editor = new ctx.visualStyleClass({ renderTo: to });
        editor.addComponent('selectors', new ctx.selectorsClass({ renderTo: to }));
        editor.addComponent('editors', new ctx.editorsClass({ renderTo: to }));
        editor.addComponent('ruletext', new ctx.ruletextClass({ renderTo: to }));
        editor.addComponent('preview', new ctx.previewClass({ renderTo: to }));
        editor.addComponent('redoundo', new ctx.redoundoClass({
            btnRedo: config.btnRedo,
            btnUndo: config.btnUndo
        }));
        if (config.effectWin) {
            editor.addComponent('effect', new ctx.effectClass({
                effectWin: config.effectWin
            }));
        }

        // load content
        if (config.url) {
            editor.loadFile(config.url);
        } else if (config.text) {
            editor.loadText(config.text);
        }

        // ret
        return editor;

    };

} (jQuery, visualstyle));
