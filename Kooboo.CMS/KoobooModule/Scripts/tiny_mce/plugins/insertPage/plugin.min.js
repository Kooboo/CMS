/// <reference path="../../tiny_mce_src.js" />

(function () {
    tinymce.create('tinymce.plugins.insertPage', {
        init: function (ed, url) {
            ed.addCommand('insertPage', function () {
                // cache current selection
                var bookmark;
                if (tinymce.isIE) { bookmark = ed.selection.getBookmark(1); }
                    // insert to editor
                    if (tinymce.isIE) { ed.selection.moveToBookmark(bookmark); } // restore cached selection
                    ed.execCommand('mceInsertContent', false, '[[url:pagename|userkey=key1]]', { skip_undo: 1 });
                    ed.undoManager.add();
            });
            ed.addButton('insertPage', {
                cmd: 'insertPage',
                title: ed.getLang('kooboo.insertPage', 'Insert page')
            });
        },
        getInfo: function () {
            return {
                longname: 'Insert page',
                author: 'Kooboo R&D',
                authorurl: 'http://www.kooboo.com',
                infourl: 'http://www.kooboo.com',
                version: tinymce.majorVersion + "." + tinymce.minorVersion
            };
        }
    });
    tinymce.PluginManager.add('insertPage', tinymce.plugins.insertPage);
})();

