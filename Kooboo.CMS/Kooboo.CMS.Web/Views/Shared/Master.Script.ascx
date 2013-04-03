<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Kooboo.CMS.Web" %>
<%: Html.IncludeLocalizationScripts() %>
<% var route = ViewContext.RequestContext.AllRouteValues();
%>
<script type="text/javascript">
    kooboo.cms.ui.setting.extend({
        timeoutMessage: '<%:"The bad netword or some unknow reasons cause timeout, do you want to reload the page.".Localize() %>'
    });

    (function ($) {
        $.fn.dialogLink = function () {
            return this.find('a.dialog-link').one('click', function (e) {
                e.preventDefault();
                // show dialog
                var id = new Date().getTime();
                var handle = $(this).pop({
                    id: id,
                    width: 800,
                    height: 580,
                    frameHeight: "100%",
                    popupOnTop: true,
                    onclose: function () {
                        if (top.kooboo.data('parent-page-reload')) {
                            kooboo.cms.ui.getOpener().reload();
                        }
                    }
                }).click();
                // fix zindex
                var topJQ = top.jQuery;
                if (topJQ) {
                    var contentCon = topJQ('#' + id).parent();
                    var contentConMask = contentCon.nextAll('.ui-widget-overlay')
                    if (contentCon.length > 0) {
                        // get zindex
                        var zIndexMax = 0, zindex;
                        topJQ('body > .ui-dialog').each(function () {
                            zindex = parseInt(topJQ(this).css('z-index'));
                            if (zindex > zIndexMax) { zIndexMax = zindex; }
                        });
                        // apply
                        contentCon.css('z-index', zIndexMax + 2);
                        contentConMask.css('z-index', zIndexMax + 1);
                    }
                }
            });
        };

        $.fn.linkPost = function () {
            return this.find('a.boolean-ajax-link').bind('click', function () {
                var handle = $(this), confirmMsg;

                if (handle.hasClass('cross')) {
                    confirmMsg = handle.attr('confirmMsg');
                } else {
                    confirmMsg = handle.attr('unConfirmMsg');
                }

                kooboo.confirm(confirmMsg, function (r) {
                    if (r) {
                        var dataField = handle.attr('data');

                        $.ajax({
                            url: handle.attr('href'),
                            type: 'post',
                            beforeSend: function () {
                                kooboo.cms.ui.loading().show();
                            },
                            success: function (response) {
                                if (response.Success) {
                                    if (handle.hasClass('cross')) {
                                        handle.removeClass('cross');
                                        handle.addClass('tick');
                                    } else {
                                        handle.removeClass('tick');
                                        handle.addClass('cross');
                                    }
                                } else {
                                    kooboo.cms.ui.messageBox().showResponse(response);
                                }
                                kooboo.cms.ui.loading().hide();
                            },
                            error: function () {
                                kooboo.cms.ui.messageBox().show('<%:"There is an error occurs".Localize() %>', 'error');
                            }
                        });
                    }
                });

                return false;
            });
        };
    })(jQuery);
    $(function () {


        //#region Initialize the js culture
        Globalize.culture('<%:System.Globalization.CultureInfo.CurrentCulture.Name %>');
        $.validator.methods.number = function (value, element) {
            return value == "" || !isNaN(Globalize.parseFloat(value));
        }
        //#endregion

        //#region show loading on page unload
        $(window).bind('unload', function () {
            kooboo.cms.ui.loading().show();
        });
        //#endregion

        //#region make first input focus
        $('input.defaultFocus:visible').first().focus();
        //#endregion

        //#region init controls

        /*  init controls   */
        $('input[value-type=float]').numeric('<%= System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator %>');
        $('input[value-type=int32]').numeric(false);

        var hasTinymce = false;
        $('textarea.tinymce').each(function () {

            var textarea = $(this);

            var hasSiteName = null;
            //var hasSiteName = $.request.getRequest('siteName') != null;

            var tinyMCEConfig = $.extend({}, tinymce.getKoobooConfig({ hasSiteName: hasSiteName }), {
                '$textarea': textarea,
                'elements': textarea.attr('id'),
                'media_library_url': '<%=Url.Action("Selection","MediaContent",route.Merge("Area","Contents").Merge("FolderName","")) %>',
                    'media_library_title': '<%:"Select Files".Localize() %>'
                });

                if (hasSiteName) {
                    tinyMCEConfig["page_selector_url"] = '<%=Url.Action("PageSelector","Page",new { Area = "Sites" , siteName=Request["SiteName"] , repositoryName="repositoryName"}) %>';
                tinyMCEConfig["page_selector_title"] = '<%:"Page selector".Localize() %>';
            }

                tinyMCE.init(tinyMCEConfig);

                hasTinymce = true;
            });

        if (hasTinymce) {
            // copy content to texteare onsubmit
            kooboo.cms.ui.event.ajaxSubmit(function (form) {
                tinyMCE.triggerSave();
            });
        }

        //init multiple files upload and edit 
        (function multiple_file_init() {
            //init multiple files upload and edit 
            var dropIndex = function (item) {
                item.find('input').each(function () {
                    var input = $(this);
                    var n = input.attr('name');
                    input.attr('name', n.replace(/\[\d+\]/, ''));
                });
            },
            filetypeMethod = {
                getMethod: function (str) {
                    for (var c in this.checker) {
                        if (this.checker[c].test(str)) {
                            return this.method[c];
                        }
                    }
                    return this.method.others;
                },
                checker: {
                    img: /.jpg$|.png$|.gif$|.jpeg$/i,
                    clientfile: /.clientfile$/i
                },
                method: {
                    img: function (item, src) {
                        if (typeof src != 'string') {
                            return false;
                        }
                        var fileType = src.substring(src.lastIndexOf('.') + 1).toLowerCase();
                        var fileName = src.substring(src.lastIndexOf('/') + 1).toLowerCase();
                        item.removeClass('uploader').find('input:file').hide();
                        if (item.find('img').length) { return false; }
                        item.find('a.remove').addClass('cross').removeClass('remove');
                        var img = $('<img />').attr('src', src).attr('alt', fileName);
                        item.remove('span.preview').append(img);
                        img.wrap('<div class="box-out"/>').wrap('<div class="box-in"/>');
                        item.addClass('img');
                    },
                    others: function (item, src) {
                        if (typeof src != 'string') {
                            return false;
                        }
                        var fileType = src.substring(src.lastIndexOf('.') + 1).toLowerCase();
                        var fileName = src.substring(src.lastIndexOf('/') + 1).toLowerCase();
                        item.removeClass('uploader').find('input:file').hide();
                        //var icon = $('<span/>');
                        item.find('span.preview').addClass('f-icon file').addClass('f-' + fileType).html(fileName);
                    },
                    clientfile: function (item, src) {
                        filetypeMethod.method.others(item, src);
                        return false;
                        if (!filetypeMethod.checker.img.test(src)) {
                            filetypeMethod.method.others(item, src);
                        } else {
                            var fileName = src.substring(src.lastIndexOf('/') + 1).toLowerCase();
                            item.find('input:file').hide();
                            if (item.find('img').length) { return false; }
                            item.find('a.remove').addClass('cross').removeClass('remove');
                            var img = $('<img />').attr('src', '<%=Url.Content("~/Images/Default.jpg") %>').attr('alt', fileName);
                            item.remove('span.preview').append(img);
                            img.wrap('<div class="box-out"/>').wrap('<div class="box-in"/>');
                            item.addClass('img');
                        }
                    }
                }
            },

            initItem = function (item, instance, mutiControl) {
                var data = instance.getItemData(item);
                if (data && (typeof data == 'string') && data.length > 0) {
                    filetypeMethod.getMethod(data)(item, data);
                    var file = item.find('input:file');
                    if (!file.val()) {
                        file.remove();
                    }
                } else {
                    var file = item.find('input:file').change(function () {
                        filetypeMethod.getMethod($(this).val() + ".clientfile")(item, $(this).val());
                    });
                }
            };

            $('input:hidden.multifile').each(function () {
                var handle = $(this),
                    valueStr = handle.val() || '',
                    config = eval('(' + handle.attr('config') + ')'),
                    data = valueStr.split('|');
                var instance = kooboo.cms.ui.dynamicListInstance({
                    containerId: config.containerId,
                    templateId: config.templateId,
                    addButtonId: config.addButtonId,
                    propertyName: config.propertyName,
                    data: data,
                    onInit: function (instance) {
                        var items = this.getItems();
                        items.each(function () {
                            var item = $(this);
                            dropIndex(item);
                            initItem(item, instance, handle);
                        });
                    },
                    onRemove: function () {
                        var instance = this,
                        items = instance.getItems(),
                        valueArr = [];
                        items.each(function () {
                            var item = $(this);
                            var d = instance.getItemData(item);
                            if (d) {
                                valueArr.push(d);
                            }
                            dropIndex(item);
                        });
                        valueStr = valueArr.join('|');
                        if (typeof valueStr == 'string') {
                            handle.val(valueStr);
                        }
                    },
                    onAdd: function (added) {
                        var items = this.getItems();
                        items.each(function (index) {
                            var item = $(this);
                            dropIndex(item);
                        });
                        initItem(added, this, handle);
                    }
                });

                $('#' + config.mediaLinkId).pop({
                    popupOnTop: true,
                    onload: function () {
                        top.kooboo.data('fileSelectPop', $.popContext.getCurrent());
                        top.kooboo.data('onFileSelected', function (url, text) {
                            var oldVal = handle.val() ? handle.val() + '|' : '';
                            handle.val(oldVal + url);
                            var added = instance.add(url);

                        });
                    }
                });

            });

        })();


        //end init controls


        //init image croper

        (function init_image_croper() {
            var tempFileUrl = '<%=Url.Action("TempFile",ViewContext.RequestContext.AllRouteValues()) %>';
            var title = '<%:"Crop image".Localize() %>';
            $('a.image-croper').click(function () {
                var handle = $(this),
                inputId = handle.attr('inputId'),
                input = $('#' + inputId),
                hidden = $('#' + inputId + '-hidden'),
                cropParam = top.kooboo.data('crop-param');
                top.kooboo.data('onCroped', function (param) {
                    cropParam = param;
                    var json = $.toJSON(param);
                    hidden.val(json);
                    input.val(param.Url);
                    param.Model = param.Url;
                    top.kooboo.data('crop-param', param)
                });
                $.pop({
                    width: 700,
                    url: tempFileUrl + "&sourceUrl=" + input.val(),
                    title: title,
                    popupOnTop: true
                });
            });
        })();

        //end init image croper

        //#endregion

        //#region  init dialog 

        //init dialog close
        $('.dialog-close').click(function () {
            if ($.popContext.getCurrent()) {
                $.popContext.getCurrent().close();
            }
        });

        //init import dialog

        $('a[name=import]').one('click', function (e) {
            e.preventDefault();
            var handle = $(this);
            handle.pop({
                useContent: true,
                contentId: 'dialog',
                width: 600,
                height: 200,
                title: '<%:"Import".Localize() %>'
            }).click();
        });

        //        
        //init dialog links 
        /*
        previously the code is $('a.dialog-link').pop()
        if there are 1000 links this will cost 6500 ms

        */
        $(document).dialogLink();
        //init copy dialog 
        var copyDialog = $('#copy-form').dialog({
            width: 600,
            height: 200,
            title: '<%:"Copy".Localize() %>',
            autoOpen: false,
            modal: true,
            close: function () {
                copyDialog.find('form').get(0).reset();
            }
        });

        $('a.common-copy').mousedown(function (e) {
            if (e.which != 3) {
                copyDialog.dialog('open');
                copyDialog.find('form').attr('action', this.href);
            }
            return false;
        })
        .bind("contextmenu", function () { return false; })
        .click(function () { return false; });

        /*end init dialog*/
        //#endregion

        //#region init icon switcher

        $('.icon-switch').click(function () {
            $(this).siblings('.list').slideToggle("normal");
        }).blur(function () {
            $(this).siblings('.list').delay(500).slideUp("normal");
        });
        $('.menu li.has-sub a').click(function () {
            $(this).siblings('span.arrow').click();
        });
        $('.menu span.arrow').click(function () {
            $(this).parent().toggleClass("active");
        });

        //#endregion

        //#region init tooltip 
        $('.tooltip-link').yardiTip({ offsetX: -20 }).click(function (e) {
            return false;
        });
        //#endregion

        //#region codemirror
        if (window.CodeMirror && CodeMirror.defaults) {

            $('textarea.codemirror').each(function () {
                var handle = $(this).codeMirror({
                    mode: { name: "xml", htmlMode: true }
                });

                kooboo.cms.ui.event.ajaxSubmit(function () {
                    if (handle.data('codeMirror') != undefined) {
                        handle.data('codeMirror').save();
                    } else {
                        body.val(body.next().text());
                    }
                });
            });

            CodeMirror.defaults.basePath = '<%=Url.Content("~/Areas/Sites/Scripts/CodeMirror2") %>';
        }
        //#endregion

        //#region ajax post link
        $('.ajax-post-link').click(function (e) {
            e.preventDefault();

            kooboo.cms.ui.loading().show();
            var handle = $(this), url = handle.attr('href'), post = function () {
                $.post(url, function (response) {
                    if (typeof response == 'object') {
                        kooboo.cms.ui.messageBox().showResponse(response);
                    }
                    else {
                        kooboo.cms.ui.messageBox().show(response);
                    }
                    top.kooboo.data('parent-page-reload', true);
                });
            };
            if (handle.attr('confirm')) {
                kooboo.confirm(handle.attr('confirm'), function (r) {
                    if (r) {
                        post();
                    } else {
                        kooboo.cms.ui.loading().hide();
                    }
                });
            } else {
                post();
            }


        });
        //#endregion


        //#region fixed submit button in ff4 there are some problems
        ///
        //$(':submit[name][value]').fixsubmit();
        //#endregion

        //#region silde legend
        function toggleLegend(legend) {
            var handle = legend, next = handle.next();
            if (handle.hasClass('active')) {
                handle.removeClass('active');
                next.fadeOut();
            } else {
                handle.addClass('active');
                next.fadeIn();
            }
        }
        var legend = $('legend.clickable:not(.no-bind)').click(function () {
            toggleLegend($(this));
        }).css({ cursor: 'pointer' }).next().hide();

        toggleLegend(legend);

        //#endregion
    });
</script>
