/// <reference path="../../tiny_mce_src.js" />

(function () {
    //tinymce.PluginManager.requireLangPack('KoobooMediaLibrary');
    tinymce.create('tinymce.plugins.KoobooPageSelector',
    {
        init: function (ed, url) {
            var kb = top.kooboo, $textarea = ed.getParam('$textarea'), textareaId = $textarea.attr('id');
            // Register the command so that it can be invoked by using tinyMCE.activeEditor.execCommand('mceExample');
            ed.addCommand('pageSelect', function () {
                top.$.pop({
                    url: ed.getParam('page_selector_url'),
                    title: ed.getParam('page_selector_title'),
                    width: 800,
                    height: 500,
                    frameHeight: '100%',
                    beforeLoad: function () {
                        kooboo.cms.ui.loading().show();
                    },
                    onload: function () {
                        kooboo.cms.ui.loading().hide();
                        kb.data('tinymce.pageselector.selected', $(ed.selection.getNode()).attr('href'));
                    }
                });
            });

            // Register KoobooPageSelector button
            ed.addButton('KoobooPageSelector', {
                title: 'page selector',
                cmd: 'pageSelect'
            });

            function onPageSelect(url, name) {
                var insertType = kb.data('tinymce.pageselector.insertType') || 'link', $node;

                var node = ed.selection.getNode();                
                if (node.tagName == 'A') {
                    $node = $($(node).clone().attr('href', url).removeAttr('_mce_href').wrap('<div/>').parent().html());
                    ed.dom.remove(node);
                } else {
                    $node = $('<a href="' + url + '" title="' + name + '">' + name + '</a>');

                }
                ed.selection.setNode($node.get(0));
            }
            // register nodechange method to global
            kb.data('tinymce.onPageSelect', onPageSelect);
        },
        getInfo: function () {
            return {
                longname: "Kooboo CMS Page's Selector",
                author: 'Kooboo R&D',
                authorurl: 'http://www.kooboo.com',
                infourl: 'http://www.kooboo.com',
                version: tinymce.majorVersion + "." + tinymce.minorVersion
            };
        }
    });

    tinymce.PluginManager.add('KoobooPageSelector', tinymce.plugins.KoobooPageSelector);
})();

