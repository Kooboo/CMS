/// <reference path="../../tiny_mce_src.js" />

(function () {
    //tinymce.PluginManager.requireLangPack('KoobooMediaLibrary');
    tinymce.create('tinymce.plugins.KoobooMediaLibrary', {
        init: function (ed, url) {
            ed.addCommand('KoobooMediaLibrary', function () {
                var $textarea = ed.getParam('$textarea');

                // cache current selection
                var bookmark;
                if (tinymce.isIE) { bookmark = ed.selection.getBookmark(1); }

                /// on selected callback
                function onSelected(src, text, option) {
                    // generate html
                    option = option || {};
                    var wrapper = $('<div></div>');
                    var imgExp = /.jpg$|.png$|.gif$|.bmp$|.jpeg$/i;
                    if (!imgExp.test(src)) {
                        wrapper.append('<a href="' + src + '">' + text + '</a>');
                    } else {
                        wrapper.append('<img alt="IMG" src="' + src + '" />');
                    }
                    wrapper.children().attr(option);
                    // insert to editor
                    if (tinymce.isIE) { ed.selection.moveToBookmark(bookmark); } // restore cached selection
                    ed.execCommand('mceInsertContent', false, wrapper.html(), { skip_undo: 1 });
                    ed.undoManager.add();
                }

                // execute popup
                var topJQ = top._jQuery || top.jQuery;
                var id = new Date().getTime();
                topJQ.pop({
                    id: id,
                    url: ed.getParam('media_library_url'),
                    title: ed.getParam('media_library_title'),
                    width: 900,
                    height: 500,
                    frameHeight: '100%',
                    beforeLoad: function () {
                        kooboo.cms.ui.loading().show();
                    },
                    onload: function (handle, pop, config) {
                        kooboo.cms.ui.messageBox().hide();
                        top.kooboo.data('onFileSelected', onSelected);
                        top.kooboo.data('fileSelectPop', pop);
                    },
                    onclose: function (handle, pop, config) {
                        kooboo.data('onFileSelected', undefined);
                        kooboo.data('fileSelectPop', undefined);
                        setTimeout(function () {
                            pop.destory();
                            pop.remove();
                        }, 10);
                    }
                });
                tinymce.ztopKoobooDialog(id);
            });
            ed.addButton('KoobooMediaLibrary', {
                cmd: 'KoobooMediaLibrary',
                title: 'Media Content'
            });
        },
        getInfo: function () {
            return {
                longname: 'Kooboo CMS Media Library',
                author: 'Kooboo R&D',
                authorurl: 'http://www.kooboo.com',
                infourl: 'http://www.kooboo.com',
                version: tinymce.majorVersion + "." + tinymce.minorVersion
            };
        }
    });
    tinymce.PluginManager.add('KoobooMediaLibrary', tinymce.plugins.KoobooMediaLibrary);
})();

