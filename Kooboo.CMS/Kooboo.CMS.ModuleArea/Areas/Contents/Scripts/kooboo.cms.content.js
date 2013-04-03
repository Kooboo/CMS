/// <reference path="../../../Scripts/jquery-1.4.1-vsdoc.js" />
/// <reference path="../../../Scripts/jQueryUI/jquery-ui-1.8.4.all.min.js" />


/// <reference path="../../../Scripts/kooboo.js" />
/// <reference path="../../../Scripts/kooboo.cms.ui.js" />



kooboo.namespace("kooboo.cms.content");
kooboo.namespace("kooboo.cms.content.folder");

kooboo.namespace("kooboo.cms.content.sendingSetting");
kooboo.cms.content.sendingSetting.extend({

    initAutoComplete: function (repository) {

        $(function () {

            $("#FolderName").autocomplete({

                source: "/Contents/SendingSetting/FolderDataSource/" + repository + "/"

            });

            $("#SchemaName").autocomplete({

                source: "/Contents/SendingSetting/SchemaDataSource/" + repository + "/"

            });

        });


    }

});

kooboo.namespace("kooboo.cms.content.textcontent");
kooboo.cms.content.textcontent.extend({
    initGrid: function () {
        $(function () { kooboo.cms.content.textcontent._initGrid(); });
    },
    _initGrid: function () {
        $(document).click(function () {
            $('div.dropdown-button').children('div').addClass('hide');
        });
        var dropdown = $('div.dropdown-button').click(function (e) {
            e.stopPropagation();
            var menu = $(this).children('div');
            if (menu.hasClass('hide')) {
                menu.removeClass('hide');
            } else {
                menu.addClass('hide');
            }
        }).children().click(function () {
            $('div.dropdown-button').children('div').addClass('hide');
        });

        $('a.delete.button').click(function () {
            var handle = $(this);

            var docArr = [];
            var folderArr = [];
            var removeTrs = [];
            $('input:checkbox:checked.folders').each(function () {
                folderArr.push($(this).val());
            });
            $('input:checkbox:checked.docs').each(function () {
                docArr.push($(this).val());
                removeTrs.push($(this).parents('tr:eq(0)'));
            });

            if (docArr.length == 0 && folderArr.length == 0) {
                alert(handle.attr('alertMsg'));
                return false;
            }


            kooboo.confirm(handle.attr('confirmMsg'), function (result) {
                if (result) {
                    var form = $('<form></form>')
					.attr('action', handle.attr('href'))
					.attr('method', 'post');

                    kooboo.cms.ui.formHelper.createHidden(folderArr, 'folderArr', form);
                    kooboo.cms.ui.formHelper.createHidden(docArr, 'docArr', form);

                    form.ajaxSubmit({
                        beforeSend: function () {
                            kooboo.cms.ui.loading().show();
                        },
                        success: function (response) {
                            if (response.Success) {
                                if (folderArr.length > 0) {
                                    document.location.reload();
                                } else {
                                    var count = removeTrs.length;
                                    removeTrs.each(function (tr, index) {
                                        tr.fadeOut(function () {
                                            tr.remove();
                                            if ((index + 1) == count) {
                                                kooboo.cms.ui.messageBox().hide();
                                            }
                                        });
                                    });
                                }
                            } else {
                                kooboo.cms.ui.messageBox().showResponse(response);
                            }
                            top.kooboo.data("parent-page-reload", true);
                        }
                    });
                }
            });

            return false;

        });

        (function initSelect() {
            var optionSelector = $('.optional-selector div').click(function (e) {
                $(this).children('ul').toggleClass("hide");
                e.stopPropagation();
            }).find("input:checkbox").click(function (e) {
                e.stopPropagation();
            });

            $("input:checkbox.select-all").click(function () {
                if ($(this).attr("checked")) {
                    $("input:checkbox").attr("checked", true);
                } else {
                    $("input:checkbox").attr("checked", false);
                }
            });

            var optionUl = $('.optional-selector div ul');

            optionUl.find("li.none a").click(function (e) {
                $("input:checkbox").attr("checked", false);
                optionUl.addClass("hide");
            });

            optionUl.find("li.all a").click(function (e) {
                $("input:checkbox").attr("checked", true);
                optionUl.addClass("hide");
            });

            optionUl.find("li.docs a").click(function (e) {
                $("input:checkbox").attr("checked", false);
                $("input:checkbox.docs").attr("checked", true);
                optionUl.addClass("hide");
            });

            optionUl.find("li.folders a").click(function (e) {
                $("input:checkbox").attr("checked", false);
                $("input:checkbox.folders").attr("checked", true);
                optionUl.addClass("hide");
            });

            $(document).click(function (e) {
                optionUl.addClass('hide');
            });
        })();
    }
});
