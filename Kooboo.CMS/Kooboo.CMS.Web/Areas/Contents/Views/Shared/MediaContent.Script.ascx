<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script language="javascript" type="text/javascript">


    kooboo.namespace("kooboo.cms.content.fileManager");

    kooboo.cms.content.fileManager.extend({
        enableTextfileEdit: true,
        ready: function (option) {
            var config = {
                saveText: "Save",
                cancelText: "Cancel",
                editText: "Edit"
            },
            host = this;

            $.extend(config, option);

            var ctrlPress = false;
            $(document).keydown(function (event) {
                if (event.keyCode == 17) {
                    ctrlPress = true;
                }
            });

            $(document).keyup(function (event) {
                if (event.keyCode == 17) {
                    ctrlPress = false;
                }
            });

            var eventHandle = null;

            var newFolderBtn = $("a.folder-add");

            (function initAddress() {

                var address = $.request.queryString["FolderName"] || $.request.queryString["folderName"] || '';

                address = address.replace(/~/g, "\\");

                if (address.indexOf('#') >= 0) {
                    address = address.substring(0, address.indexOf('#'));
                }

                var addressInput = $("#addressInput").val(address);


                addressInput.keyup(function (event) {
                    if (event.keyCode == 13) {
                        go.click();
                    }
                });

                var go = $("a.go");
                go.click(function () {
                    var current = $(this);
                    var request = $.request(current.attr('href'));
                    request.queryString['FolderName'] = addressInput.val().replace('\\', '~').replace('/', "~");

                    document.location.href = request.getUrl();
                });

            })();

            (function newFolder() {
                var newFolderStr = '<tr><td></td><td colspan="5"><a href="javascript:;" class="f-icon folder"></a><input type="text" value="new folder" class="editor"/><a href="javascript:;" class="o-icon inline-action save">' + config.saveText + '</a><a href="javascript:;" class="o-icon inline-action cancel">' + config.cancelText + '</a></td></tr>';

                newFolderBtn.click(function () {
                    var tr = $(newFolderStr);

                    tr.find("a.save").click(function () {
                        if (input.val() && input.val().trim().length > 0) {
                            submitFolderName(tr, input);
                        }
                    }).attr("title", config.saveText);

                    tr.find("a.cancel").click(function () {
                        tr.remove();
                    }).attr("title", config.cancelText);

                    tr.attr('id', 'tr' + Math.random().toString().replace('.', "_"));

                    document.location.href = "#" + tr.attr('id');

                    var container = $(".table-container tbody tr").last();

                    if (container.length == 0) {
                        tr.appendTo($(".table-container tbody "));
                    } else {
                        container.after(tr);
                    }

                    var input = tr.find("input").focus();
                    input.get(0).select();
                    input.keydown(function (event) {
                        if (event.keyCode == 13) {
                            return submitFolderName(tr, input);
                        }
                    });

                    return false;
                });
            })();

            (function initDelete() {
                $(".action-delete").click(function () {
                    var current = $(this);

                    var folders = [];
                    var files = [];

                    var trs = [];

                    $(".table-container tbody tr.foldertr input:checkbox[checked]")
                    .each(function () {
                        trs.push($(this).parents('tr'));

                        folders.push($(this).val());
                    });

                    $(".table-container tbody tr.filetr input:checkbox[checked]")
                    .each(function () {
                        trs.push($(this).parents('tr'));
                        files.push($(this).val());
                    });


                    if (files.length == 0 && folders.length == 0) {
                        kooboo.alert(current.attr('alertMsg'));
                        return false;
                    }

                    if (confirm(current.attr('confirmMsg'))) {

                        var href = current.attr('href');

                        kooboo.cms.ui.loading().show();

                        var delForm = kooboo.cms.ui.formHelper
                        .tempForm(folders, href, 'folders', { method: 'post' })
                        .addData(files, 'files').ajaxSubmit({
                            success: function (response) {
                                if (response.Success) {
                                    if (folders.length > 0) {
                                        document.location.reload();
                                    } else {
                                        trs.each(function (value) { value.remove(); });
                                        kooboo.cms.ui.messageBox().hide();
                                    }
                                } else {
                                    kooboo.cms.ui.messageBox().showResponse(response);
                                }
                            }
                        });
                    }

                    return false;
                });
            })();

            (function initSelect() {
                var optionSelector = $('.optional-selector div').click(function (e) {
                    e.stopPropagation();
                    $(this).children('ul').toggleClass("hide");
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
                optionUl.find("li.none").click(function () {
                    $("input:checkbox").attr("checked", false);
                });
                optionUl.find("li.all").click(function () {
                    $("input:checkbox").attr("checked", true);
                });
                optionUl.find("li.docs").click(function () {
                    $("input:checkbox").attr("checked", false);
                    $("input:checkbox.docs").attr("checked", true);
                });
                optionUl.find("li.folders").click(function () {
                    $("input:checkbox").attr("checked", false);
                    $("input:checkbox.folders").attr("checked", true);
                });

                $(document).click(function () {
                    optionUl.addClass('hide');
                });
            })();

            (function initSearch() { })();

            (function initUpload() {
                window.onSuccess = function () {
                    document.location.reload();
                }
                $("input:file.files").change(function () {
                    $(this).parent().submit();
                });
            })();

            (function initLargeFile() {
                $('.dialog-upload').pop({
                    onclose: function () {
                        try {
                            if (top.kooboo.data('large-file-changed')) {
                                document.location.reload(true);
                            }
                        }
                        catch (E) {
                        }

                    },
                    popupOnTop: true,
                    width: 800,
                    height: 580,
                    frameHeight: "100%"
                });
                //$('.muti-upload').pop({
                //    width: 600,
                //    height: 380,
                //    frameHeight: "100%",
                //    onclose: function () {
                //        try {
                //            if (top.kooboo.data('large-file-changed')) {
                //                document.location.reload(true);
                //            }
                //        }
                //        catch (E) {
                //        }

                //    }
                //});
            })();

            (function initImport() {
                var zipForm = $('#zip-form');
                var canSubmit = true;

                kooboo.cms.ui.event.ajaxSubmit(function () {
                    return canSubmit;
                });

                $('#import-zip-btn').pop({
                    useContent: true,
                    contentId: 'import-zip',
                    width: 600,
                    height: 200,
                    onclose: function () { canSubmit = true; zipForm.get(0).reset(); },
                    onload: function () { canSubmit = false; }
                });

                $('input:file.zip').unbind('change').change(function () {
                    var handle = $(this);
                    var span = handle.next().addClass('field-validation-error')
                    var id = handle.attr('id');
                    var exp = /.zip$/gi;
                    if (!exp.test(handle.val())) {
                        span.html('<%:"Please upload zip file".Localize() %>').show();
                        handle.val('');
                        canSubmit = false;
                    } else {
                        span.html('').hide();
                        canSubmit = true;
                    }
                });

            })();

            initGrid();

            (function fileViewer() {
                $('a[filename]').each(function () {
                    var handle = $(this);
                    var quickExp = new RegExp('.jpg$|.png$|.gif$|.bmp$|.jpeg$', 'i');
                    if (quickExp.test($(this).attr('filename'))) {
                        handle.attr('rel', 'imageviewer[image]');
                    }
                });
                $('a[rel^=imageviewer]').prettyPhoto();
            })();

            if (this.enableTextfileEdit) {
                var enabledEditFiles = /html|htm/i;
                $('a[filetype]').each(function () {
                    var handle = $(this),
                    filetype = handle.attr('filetype'),
                    textFileUrl = handle.attr('textfileUrl');
                    if (enabledEditFiles.test(filetype)) {
                        handle.attr('href', textFileUrl);
                        handle.pop({
                            width: 800,
                            height: 580,
                            frameHeight: "100%",
                            popupOnTop: true
                        });
                    }
                });
            }

            ///init  grid
            function initGrid() {
                var selectedClass = "selected";
                var trs = $(".table-container tbody tr").each(function () {
                    var currentCandblclick = true;
                    var current = $(this);
                    var folderLink = current.find("a.folder,a.file");                    
                    var linkText = folderLink.text().trim();
                    folderLink.click(function (event) {
                        event.stopPropagation();
                        current.click();
                        return false;
                    });

                    var edit = $('<a href="javascript:;" class="o-icon edit inline-action hide">' + config.editText + '</a>');
                    folderLink.after(edit);
                    edit.attr("title", config.editText).click(function (event) {
                        event.stopPropagation();
                        edit.addClass("hide");
                        var input = $('<input type="text" class="editor"/> <a href="javascript:;" class="o-icon save inline-action" title="' + config.saveText + '">' + config.saveText + '</a> <a href="javascript:;" class="o-icon cancel inline-action" title="' + config.cancelText + '" >' + config.cancelText + '</a>');

                        input.siblings("a.save").click(function () {
                            if (input.val().toLocaleLowerCase() == linkText.toLocaleLowerCase()) {
                                input.remove();
                                folderLink.text(linkText);
                            } else {
                                submitRename(current, input, linkText, folderLink);
                            }
                        });

                        input.siblings("a.cancel").click(function () {
                            input.remove();
                            folderLink.text(linkText);
                        });

                        input.dblclick(function (event) {
                            event.stopPropagation();
                            return false;
                        }).click(function (event) {
                            event.stopPropagation();
                            return false;
                        }).focus(function () {
                            input[0].select();
                        }).keydown(function (event) {
                            if (event.keyCode == 13) {
                                submitRename(current, input, linkText, folderLink);
                            }
                        }).keyup(function (event) {
                            //jAlert(event.keyCode);
                            if (event.keyCode == 27) {
                                input.val(linkText);
                                //submitRename(current, input, linkText, folderLink);
                            }
                        });
                        setTimeout(function () { input.focus(); }, 100);
                        folderLink.after(input);
                        input.val(folderLink.text().trim());
                        folderLink.text('');
                    });
                    current.hover(function () {
                        if (current.find("input:text").length == 0) {
                            edit.removeClass("hide");
                        }
                    }, function () {
                        edit.addClass("hide");
                    });

                    if (current.find("a.folder").attr('href')) {
                        current.css({ cursor: "pointer" });
                    }
                    current.unbind("click dblclick").click(function () {
                        if (current.find("a.folder").attr('href')) {
                            document.location.href = current.find("a.folder").attr('href');
                        }
                    });

                    current.find("input:checkbox").click(function (event) {
                        event.stopPropagation();
                    }).parent().click(function (event) {
                        event.stopPropagation();
                    }).css({ "cursor": "default" });

                });

                $('a[published]').click(function () {

                    var handle = $(this);

                    var published = handle.hasClass('cross');

                    var confirmMsg = '';

                    if (published) {
                        confirmMsg = handle.attr('confirmMsg');
                    } else {
                        confirmMsg = handle.attr('unConfirmMsg');
                    }

                    kooboo.confirm(confirmMsg, function (r) {
                        if (r) {
                            $.post(handle.attr('href'),
							{ published: published },
							function (response) {
							    if (response.Success) {
							        if (handle.hasClass('tick')) {
							            handle.removeClass('tick').addClass('cross');
							        } else {
							            handle.removeClass('cross').addClass('tick');
							        }
							    } else {
							        kooboo.cms.ui.messageBox().showResponse(response);
							    }
							});
                        }
                    });


                    return false;
                });

                trs.filter('.filetr').find('a.o-icon.link').yardiTip({
                    title: function () {
                        return $(this).attr('href');
                    },
                    trigger: 'show-yarditip'
                }).click(function () {
                    $(this).trigger('show-yarditip');
                    return false;
                });
            };

            function submitFolderName(tr, input) {
                if (!input.val() || input.val().trim().length == 0) {
                    return false;
                }

                var save = tr.find("a.save");

                var cancel = tr.find("a.cancel");

                var source = [];
                var folders = $(".table-container tbody tr").each(function () {
                    source.push($(this).find("td:eq(1) a:visible").text().trim().toLocaleLowerCase());
                });

                var alertMsg = '<%:" existed!".Localize() %> ';

                for (var i = 0; i < source.length; i++) {
                    if (source[i].toLocaleLowerCase() == input.val().toLocaleLowerCase()) {
                        kooboo.cms.ui.messageBox().show(input.val() + alertMsg, 'warning');
                        return false;
                    }
                }
                //tr.find('a.folder').html(input.val());

                $.ajax({
                    url: newFolderBtn.attr('href'),
                    data: { Name: input.val() },
                    type: 'post',
                    beforeSend: function () {
                        kooboo.cms.ui.loading().show();
                    },
                    success: function (response) {
                        if (response.Success) {
                            document.location.reload();
                        } else {
                            kooboo.cms.ui.messageBox().showResponse(response);
                        }
                    }
                });

                //input.attr('disabled', true).hide();
            }

            function submitRename(tr, input, linkText, folderLink) {

                if (!input.val() || input.val().trim().length == 0) {
                    input.val(linkText);
                    return false;
                }

                input.val(input.val().trim());

                if (input.val() == linkText) {
                    folderLink.html(linkText);
                    input.remove();
                    return false;
                }

                var source = [];
                var folders = $(".table-container tbody tr").each(function () {
                    source.push($(this).find("td:eq(1) a.folder").text().trim().toLocaleLowerCase());
                });

                var alertMsg = '<%:" existed!".Localize() %> ';

                for (var i = 0; i < source.length; i++) {
                    if (source[i].toLocaleLowerCase() == input.val().toLocaleLowerCase()) {
                        kooboo.cms.ui.messageBox().show(input.val() + alertMsg, 'warning');
                        return false;
                    }
                }

                $.ajax({
                    url: tr.find("input:hidden[name=editUrl]").val(),
                    data: { Name: input.val() },
                    type: 'post',
                    beforeSend: function () {
                        kooboo.cms.ui.loading().show();
                    },
                    success: function (response) {
                        if (response.Success) {
                            document.location.reload();
                        } else {
                            kooboo.cms.ui.messageBox().showResponse(response);
                        }
                    }
                });
            }

        }
    });
</script>
