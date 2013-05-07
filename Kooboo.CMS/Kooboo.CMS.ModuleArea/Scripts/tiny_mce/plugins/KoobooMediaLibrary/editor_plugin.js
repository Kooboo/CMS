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
                        wrapper.append('<img alt="' + text + '" src="' + src + '" />');
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
                    dialogClass: 'iframe-dialog',
                    frameHeight: '100%',
                    beforeLoad: function () {
                        //window.loading.show();
                    },
                    onload: function (handle, pop, config) {
                        top.onFileSelected = onSelected;
                        top.fileSelectPop = pop;
                        //top.kooboo.data('onFileSelected', onSelected);
                        //top.kooboo.data('fileSelectPop', pop);
                    },
                    onclose: function (handle, pop, config) {
                        delete top.onFileSelected;
                        delete top.fileSelectPop;
                        //kooboo.data('onFileSelected', undefined);
                        //kooboo.data('fileSelectPop', undefined);                        
                        //setTimeout(function () {
                        //    if (typeof (pop.destory) == 'function') {
                        //        pop.destory();
                        //    }
                        //    if (typeof (pop.remove) == 'function') {
                        //        pop.remove();
                        //    }                            
                        //}, 10);
                    }
                });
                tinymce.ztopKoobooDialog(id);
            });
            ed.addButton('KoobooMediaLibrary', {
                cmd: 'KoobooMediaLibrary',
                title: ed.getLang('kooboo.mediaContent', 'Media content')
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

