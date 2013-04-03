<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Contents.Models.MediaContentGrid>" %>

<%@ Import Namespace="Kooboo.CMS.Content.Models" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%:Html.Partial("MediaContentGrid",Model) %>
    <p class="buttons">
        <button id="addBtn">
            <%:"Insert".Localize() %></button>
        <button id="cancelBtn">
            <%:"Cancel".Localize()%></button>
    </p>
    <script language="javascript" type="text/javascript">
        $(function () {

            /* inline edit start */
            var doCancel, doSelect;
            var imageType = (location.href.indexOf('&fileType=image') > -1);
            if (imageType) {
                // get content iframe
                var iframe = function () {
                    window.__rid = Math.random().toString();
                    var frm, frms = window.parent.document.getElementsByTagName('iframe');
                    for (var i = 0; i < frms.length; i++) {
                        try {
                            if (frms[i].contentWindow.__rid == window.__rid) { // Permission denied to access property
                                frm = frms[i];
                                break;
                            }
                        } catch (e) { }
                    }
                    return frm;
                } ();
                if (!iframe) { return; }
                // get outer api
                var outerApi = iframe.outerApi;
                if (!outerApi) { return; }
                // set funcs
                doCancel = function () { outerApi.close(); };
                doSelect = function (url, fileName, target) {
                    // request image detail
                    var disable = function (disabled) {
                        $('#addBtn').attr('disabled', disabled);
                        $('#cancelBtn').attr('disabled', disabled);
                    };
                    $.ajax({
                        url: '<%=Url.Action("ImageDetailInfo", "MediaContent", ViewContext.RequestContext.AllRouteValues())%>',
                        data: { fileName: fileName, folderName: $.request.queryString['folderName'] },
                        type: 'post', dataType: 'json', timeout: 5000,
                        beforeSend: function (request) { disable(true); },
                        complete: function (request, state) { disable(false); },
                        success: function (data, state) {
                            if (data.Success) {
                                if (outerApi) {
                                    var imgalt = data.FileName.replace(/\..+/g, '');
                                    outerApi.OnSelect(url, data.Width, data.Height, imgalt);
                                    setTimeout(function () { outerApi.close(); }, 32); // setTimeout use for fix ie9 bug.
                                }
                            } else {
                                alert('Get file failure, maybe the file you selected is not a image type file.');
                            }
                        }
                    });
                };
            }
            /* inline edit end */

            $('tr.filetr').css({
                cursor: 'pointer'
            }).click(function () {
                var tr = $(this);
                var checkbox = tr.find('input:checkbox.docs');
                if (checkbox.attr('checked')) {
                    checkbox.attr('checked', false);
                } else {
                    checkbox.attr('checked', true);
                }
                return false;
            });

            $('tr.filetr a.f-icon.file').each(function () {
                var handle = $(this);
                var url = $(this).attr('href');
                var fileName = $(this).attr('fileName');
                handle.data('url', url);
                //handle.attr('href', 'javascript:;');

                handle.dblclick(function () {
                    if (imageType) {
                        doSelect(url, fileName, handle.text())
                    } else {
                        top.kooboo.data('onFileSelected')(url, handle.text());
                        $.popContext.getCurrent().close();
                    }
                });

                handle.click(function () {
                    //return false;
                });
            });

            $('#addBtn').click(function () {
                var selecteds = $('input:checkbox.docs:checked');
                if (imageType) {
                    selecteds.each(function () {
                        var a = $(this).parents('tr:eq(0)').find('a.f-icon.file');
                        doSelect(a.attr('href'), a.attr('fileName'), a.text());
                        return false;
                    });
                } else {
                    var result = true;
                    if (typeof top.kooboo.data('checkSelect') == 'function') {
                        var selectList = [];
                        selecteds.each(function () {

                            var handle = $(this), a = handle.parents('tr:eq(0)').find('a.f-icon.file');
                            selectList.push({
                                url: a.attr('href'),
                                text: a.text(),
                                el: a,
                                line: handle
                            });
                        });
                        result = top.kooboo.data('checkSelect').call(this, selectList);
                    }
                    if (result != false) {
                        selecteds.each(function () {
                            var a = $(this).parents('tr:eq(0)').find('a.f-icon.file');
                            top.kooboo.data('onFileSelected')(a.attr('href'), a.text());
                        });
                        setTimeout(function () { 
                            $.popContext.getCurrent().close();
                        },100);
                    }

                }
            });
            $('#cancelBtn').click(function () {
                doCancel ? doCancel() : $.popContext.getCurrent().close();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
