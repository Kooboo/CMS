<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Contents.Models.MediaContentGrid>" %>

<%@ Import Namespace="Kooboo.CMS.Content.Models" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var routes = ViewContext.RequestContext.AllRouteValues();

        var fileType = ViewContext.RequestContext.GetRequestValue("fileType");

        var fileTypeDic = new Dictionary<string, List<string>>();
        fileTypeDic.Add("image", new List<string> { "jpg", "png", "gif", "jpeg", "bmp" });

        var displayExtension = string.IsNullOrWhiteSpace(fileType) ? new List<string>() : fileTypeDic.ContainsKey(fileType) ? fileTypeDic[fileType.ToLower()] : new List<string>();

        string action = routes["action"].ToString();
        var fullNameArray = routes["FolderName"] != null ? FolderHelper.SplitFullName(routes["FolderName"].ToString()) : new string[] { };

        var fullNameCount = fullNameArray.Count();

        string folderUpUrl = "";

        if (fullNameCount > 1)
        {
            var parentFolderFullName = FolderHelper.CombineFullName(fullNameArray.Take(fullNameCount - 1));
            var pRoutes = ViewContext.RequestContext.AllRouteValues();
            pRoutes["FolderName"] = parentFolderFullName;
            folderUpUrl = this.Url.Action(action, pRoutes);
        }
        else if (fullNameCount == 1)
        {
            folderUpUrl = Url.Action(action, ViewContext.RequestContext.AllRouteValues().Merge("FolderName", null));
        }
    %>
    <h3 class="title">
        <%= "Media library".Localize() %></h3>
    <div class="command clearfix">
        <table class="left">
            <tbody>
                <tr>
                    <td>
                        <input type="text" id="addressInput" style="width: 300px;" readonly="readonly" />
                    </td>
                    <%if (!string.IsNullOrWhiteSpace(folderUpUrl))
                      {%>
                    <td>
                        <a href="<%=folderUpUrl %>" class="o-icon folder-back" title="<%="Back".Localize() %>">Back</a>
                    </td>
                    <% } %>
                    <td>
                        <a href="<%=this.Url.Action("DeleteSelect",routes) %>" confirmmsg="<%="Are you sure you want to delete these items?".Localize() %>"
                            alertmsg="<%="You have not select any items!".Localize() %>" class="o-icon cross action-delete"
                            title="<%="Delete".Localize() %>">Delete</a>
                    </td>
                    <td>
                        <a href="<%=this.Url.Action("Create","MediaFolder",routes) %>" class="o-icon folder-add"
                            title="<%="Create folder".Localize() %>">Create folder</a>
                    </td>
                    <%if (!string.IsNullOrWhiteSpace(folderUpUrl))
                      {%>
                    <td>
                        <div class="file-upload">
                            <form action="<%=this.Url.Action("Create",routes) %>" enctype="multipart/form-data"
                                method="post">
                                <input type="hidden" value="True" name="Overrided" />
                                <input type="file" name="files" class="files" title="<%="Upload".Localize() %>" />
                            </form>
                        </div>
                    </td>
                    <td>
                        <a href="javascript:;" class="o-icon import" id="import-zip-btn" title="<%:"Import from zip".Localize() %>">
                            <%:"Import".Localize() %></a>
                        <div id="import-zip" style="display: none;" class="common-form">
                            <form action="<%=this.Url.Action("Import",routes) %>" enctype="multipart/form-data"
                                method="post" id="zip-form">
                                <table>
                                    <tr>
                                        <th>
                                            <label>
                                                <%:"File".Localize() %></label>
                                        </th>
                                        <td>
                                            <input type="file" name="files" class="files zip" title="<%="Upload".Localize() %>" /><span></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <label for="zip-overrided">
                                                <%:"Overrided".Localize() %></label>
                                        </th>
                                        <td>
                                            <input type="checkbox" id="zip-overrided" name="Overrided" value="True" />
                                            <%:Html.Hidden("Overrided",false) %>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <p class="buttons">
                                    <button type="submit">
                                        <%:"Upload".Localize() %>
                                    </button>
                                </p>
                            </form>
                        </div>
                    </td>

                    <%} %>
                </tr>
            </tbody>
        </table>
        <%:Html.Partial("Search") %>
    </div>
    <div class="table-container clearfix">
        <table>
            <thead>
                <tr>
                    <th class="checkbox optional-selector ">
                        <div>
                            <input type="checkbox" class="select-all" />
                            <ul class="hide">
                                <li>Select:</li>
                                <li class="all"><a href="javascript:;">
                                    <%="All Elements".Localize() %></a></li>
                                <li class="docs"><a href="javascript:;">
                                    <%="Only Documents".Localize() %></a></li>
                                <li class="folders"><a href="javascript:;">
                                    <%="Only Folders".Localize() %></a></li>
                                <li class="none"><a href="javascript:;">
                                    <%="None".Localize() %></a></li>
                            </ul>
                        </div>
                    </th>
                    <th>
                        <%="Title".Localize()%>
                    </th>
                    <th class="type">
                        <%="Type".Localize() %>
                    </th>
                    <th class="date">
                        <%="Date".Localize() %>
                    </th>
                    <th class="size">
                        <%="Size".Localize() %>
                    </th>
                    <th class="action publish">
                        <%:"Publish".Localize() %>
                    </th>
                    <th class="action">
                        <%:"Link".Localize() %>
                    </th>
                    <th class="action">
                        <%:"Move".Localize() %>
                    </th>
                    <th class="action">
                        <%:"Edit".Localize() %>
                    </th>
                </tr>
            </thead>
            <tbody>
                <%foreach (var folder in Model.ChildFolders)
                  {
                      var currentRoutes = ViewContext.RequestContext.AllRouteValues();
                      currentRoutes["folderName"] = folder.FullName;
                %>
                <tr class="foldertr">
                    <td>
                        <input type="checkbox" name="Selected" value="<%=folder.FullName %>" class="folder folders select" />
                    </td>
                    <td>
                        <input type="hidden" value="<%=folder.FullName %>" name="fullName" />
                        <input type="hidden" value="<%=Url.Action("Edit","MediaFolder",currentRoutes) %>"
                            name="editUrl" />
                        <%:Html.ActionLink(string.IsNullOrEmpty(folder.DisplayName) ? folder.Name : folder.DisplayName, action, currentRoutes, new RouteValueDictionary(new { @class="f-icon folder" }))%>
                    </td>
                    <td class="type">
                        <%="Folder".Localize() %>
                    </td>
                    <td class="date"></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <% } %>
                <%
                    if (Model != null && Model.Contents != null)
                        foreach (var content in Model.Contents)
                        {
                            var currentRoutes = ViewContext.RequestContext.AllRouteValues();
                            currentRoutes["folderName"] = ViewContext.RequestContext.GetRequestValue("folderName");
                            currentRoutes["uuid"] = content.UUID;
                %>
                <%if (displayExtension.Count == 0 || displayExtension.Contains(content.FileType.ToLower()))
                  {%>
                <tr class="filetr">
                    <td>
                        <input type="checkbox" name="Selected" value="<%=content.UUID %>" class="content select docs" />
                    </td>
                    <td>
                        <input name="previewUrl" value="<%=content.Url+"?t="+DateTime.UtcNow.Ticks.ToString() %>" type="hidden" />
                        <input type="hidden" value="<%=Url.Action("RenameFile","MediaContent",currentRoutes) %>"
                            name="editUrl" />
                        <a href="<%=content.Url +"?t="+DateTime.UtcNow.Ticks.ToString() %>" class="f-icon file f-<%=content.FileType.ToLower() %>"
                            target="_blank" filename="<%:content.FileName %>" title="<%:content.FileName %>"
                            textfileurl='<%=Url.Action("TextFile",ViewContext.RequestContext.AllRouteValues().Merge("FileName",content.FileName)) %>'
                            filetype='<%=content.FileType.Localize().ToUpper()%>'
                            selectvalue="<%=content.Url%>"
                            selecttext="<%= (content.Metadata==null || string.IsNullOrEmpty(content.Metadata.AlternateText)) ? content.FileName : content.Metadata.AlternateText %>"><%=  content.FileName %></a>
                    </td>
                    <td class="type">
                        <%=content.FileType.Localize().ToUpper()%>
                    </td>
                    <td class="date">
                        <%= content.UtcCreationDate.ToShortDateString() %>
                    </td>
                    <td class="size">
                        <%=string.Format("{0:0.00}", content.Size/1024) %>
                    kb
                    </td>
                    <td class="action">
                        <a href="<%:this.Url.Action("Publish",ViewContext.RequestContext.AllRouteValues().Merge("uuid",content.UUID)) %>"
                            class="o-icon <%:(content.Published.HasValue && content.Published.Value==true)?"tick":"cross" %>"
                            published="<%:content.Published %>" confirmmsg="<%:"Are you sure you want to publish this item.".Localize() %>"
                            unconfirmmsg="<%:"Are you sure you want to unpublish this item.".Localize() %>">
                            <%:content.Published %></a>
                    </td>
                    <td class="action">
                        <a href="<%=content.Url %>" class="o-icon link"></a>
                    </td>
                    <td class="action">
                        <a href="<%=Url.Action("MoveFile",ViewContext.RequestContext.AllRouteValues().Merge("uuid",content.UUID))%>"
                            class="o-icon move-page dialog-link" title="<%: "Move".Localize() %>"></a>
                    </td>

                    <td class="action">
                        <%if (Kooboo.Drawing.ImageTools.IsImageExtension(System.IO.Path.GetExtension(content.PhysicalPath)))
                          { %>
                        <a href="<%=Url.Action("Edit",ViewContext.RequestContext.AllRouteValues().Merge("uuid",content.UUID).Merge("returnUrl",Request.Url.ToString()))%>"
                            class="o-icon edit dialog-link" title="<%: "Edit".Localize() %>"></a>
                        <% }%>
                    </td>
                </tr>
                <% } %>
                <%} %>
            </tbody>
        </table>
        <div class="pagination">
            <%: Html.Pager(Model.Contents, "", ViewContext.RequestContext.AllRouteValues(),null)%>
        </div>
    </div>
    <%:Html.Partial("MediaContent.Script") %>


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
                }();
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
                var url = $(this).attr('selectValue');
                var fileName = $(this).attr('fileName');
                handle.data('url', url);
                //handle.attr('href', 'javascript:;');

                handle.dblclick(function () {
                    if (imageType) {
                        doSelect(url, fileName, handle.attr('selecttext'))
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
                        doSelect(a.attr('href'), a.attr('fileName'), a.attr('selecttext'));
                        return false;
                    });
                } else {
                    var result = true;
                    if (typeof top.kooboo.data('checkSelect') == 'function') {
                        var selectList = [];
                        selecteds.each(function () {

                            var handle = $(this), a = handle.parents('tr:eq(0)').find('a.f-icon.file');
                            selectList.push({
                                url: a.attr('selectValue'),
                                text: a.attr('selecttext'),
                                el: a,
                                line: handle
                            });
                        });
                        result = top.kooboo.data('checkSelect').call(this, selectList);
                    }
                    if (result != false) {
                        selecteds.each(function () {
                            var a = $(this).parents('tr:eq(0)').find('a.f-icon.file');
                            top.kooboo.data('onFileSelected')(a.attr('selectValue'), a.attr('selecttext'));
                        });
                        setTimeout(function () {
                            $.popContext.getCurrent().close();
                        }, 100);
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
