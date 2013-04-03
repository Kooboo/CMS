<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Sites.Models.ResourceGridModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=  ViewData["Title"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var routes = ViewContext.RequestContext.AllRouteValues();

        string controller = ViewContext.RequestContext.GetRequestValue("controller");

        var directoryPath = ViewContext.RequestContext.GetRequestValue("directoryPath");
        string folderUpUrl = null;
        if (!string.IsNullOrEmpty(directoryPath))
        {
            var parentPathArr = directoryPath.Split(new char[] { '\\' });
            if (parentPathArr.Count() == 1)
            {
                folderUpUrl = Url.Action("Index", ViewContext.RequestContext.AllRouteValues().Merge("directoryPath", null));
            }
            else if (parentPathArr.Count() > 1)
            {
                folderUpUrl = Url.Action("Index", ViewContext.RequestContext.AllRouteValues().Merge("directoryPath", string.Join("\\", parentPathArr.Take(parentPathArr.Count() - 1))));
            }
        }
        
        
    %>
    <h3 class="title">
        <span>
            <%=  ViewData["Title"]%></span>
        <%if (string.Compare(controller, "ThemeResource", true) >= 0)
          {%>
        <a href="javascript:;" title="<%:"Theme contains both CSS and image files. Folders directly under the Theme root will be considered as one theme. ThemeRules.txt is a speical text file used to write your CSS loading rules.".Localize() %>"
            class="tooltip-link left"></a>
        <%} %>
    </h3>
    <input type="hidden" value="<%:Model.Directory is Kooboo.CMS.Sites.Services.DirectoryEntry ? ((Kooboo.CMS.Sites.Services.DirectoryEntry)Model.Directory).RelativePath : "" %>"
        id="baseVirtualPath" />
    <div class="command clearfix">
        <table>
            <tbody>
                <tr>
                    <td>
                        <input type="text" id="addressInput" style="width: 300px;" value="<%:ViewContext.RequestContext.GetRequestValue("directoryPath") %>"
                            readonly="readonly" />
                    </td>
                    <%--                    <td>
                        <a href="<%=this.Url.Action("Index",routes) %>" class="o-icon go" title="<%="Go".Localize() %>">
                            Go</a>
                    </td>--%>
                    <%if (!string.IsNullOrEmpty(folderUpUrl))
                      {%>
                    <td>
                        <a href="<%:folderUpUrl %>" class="o-icon folder-back" title="<%="Back".Localize() %>">
                            Back</a>
                    </td>
                    <% } %>
                    <td>
                        <a href="<%=this.Url.Action("Delete",routes) %>" class="o-icon cross action-delete"
                            title="<%="Delete".Localize() %>">Delete</a>
                    </td>
                    <td>
                        <a href="<%=this.Url.Action("CreateDirectory",routes) %>" class="o-icon folder-add"
                            title="<%="Create folder".Localize() %>">Create folder</a>
                    </td>
                    <td>
                        <a href="<%=this.Url.Action("CreateFile",routes) %>" class="o-icon document file-add"
                            title="<%="Create File".Localize() %>">Create file</a>
                    </td>
                    <td>
                        <div class="file-upload">
                            <form action="<%=this.Url.Action("Upload",routes) %>" enctype="multipart/form-data"
                            method="post">
                            <input type="file" name="files" class="files anything" title="<%="Upload".Localize() %>" />
                            </form>
                        </div>
                    </td>
                    <td>
                        <a href="<%=Url.Action("LargeFile",routes) %>" class="o-icon muti-upload dialog-upload"
                            title="<%:"Large file".Localize() %>">Upload</a>
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
                    <% if (ViewBag.Theme != null && ServiceFactory.HeaderBackgroundManager.IsEanbled(ViewBag.Theme))
                       {  %>
                    <td>
                        <%: Html.ActionLink("Change header background".Localize(), "ChangeHeaderBackground", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class="o-icon header dialog-link",title="Change header background".Localize() }))%>
                    </td>
                    <%} %>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="table-container">
        <table border="0" cellspacing="0" cellpadding="0">
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
                        <%="Name".Localize()%>
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
                    <th class="action">
                        <%:"Link".Localize() %>
                    </th>
                </tr>
            </thead>
            <tbody>
                <%foreach (var folder in Model.Directories)
                  {
                      var currentRoutes = ViewContext.RequestContext.AllRouteValues();
                      currentRoutes["directoryPath"] = folder.RelativePath;         
                %>
                <tr class="foldertr">
                    <td>
                        <input type="checkbox" name="Selected" value="<%=folder.RelativePath %>" class="folder folders select" />
                    </td>
                    <td>
                        <input type="hidden" value="<%=Url.Action("Edit","MediaFolder",currentRoutes) %>"
                            name="editUrl" />
                        <a href="<%:this.Url.Action("Index",currentRoutes) %>" class="f-icon folder">
                            <%=folder.Name %>
                        </a><a class="o-icon edit inline-action" href="<%=this.Url.Action("EditDirectory",currentRoutes) %>">
                            Edit</a>
                    </td>
                    <td class="type">
                        <%="Folder".Localize() %>
                    </td>
                    <td class="date">
                        <%=folder.LastUpdateDate.ToShortDateString() %>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <% } %>
                <%foreach (var content in Model.Files)
                  {
                      var currentRoutes = ViewContext.RequestContext.AllRouteValues();
                      currentRoutes["virtualPath"] = content.RelativePath;
                      var extension = string.IsNullOrWhiteSpace(content.FileExtension) ? "unknown" : content.FileExtension.TrimStart('.');
                      
                %>
                <tr class="filetr <%=extension %>">
                    <td>
                        <input type="checkbox" name="Selected" value="<%=content.RelativePath %>" class="content select docs" />
                    </td>
                    <td>
                        <a href="<%=this.Url.Action("EditFile",currentRoutes) %>" class="f-icon file f-<%=extension %>"
                            target="_blank" rel="viewer[<%=extension.ToLower()%>]">
                            <%=content.Name %>
                        </a>
                    </td>
                    <td class="type">
                        <%=extension.ToUpper()%>
                    </td>
                    <td class="date">
                        <%=content.CreateDate.ToShortDateString() %>
                    </td>
                    <td class="size">
                        <%=content.Site %>
                    </td>
                    <td class="action">
                        <a href="javascript:;" virtualpath="<%=Url.Content( content.VirtualPath) %>" relativepath='<%:content.RelativePath %>'
                            class="o-icon link"></a>
                    </td>
                </tr>
                <%} %>
            </tbody>
        </table>
        <%:Html.Hidden("SortUrl",this.Url.Action("Sort",this.ViewContext.RequestContext.AllRouteValues())) %>
    </div>
    <script language="javascript" type="text/javascript">

        kooboo.namespace("adminJs.resource");
        adminJs.resource.extend({
            //init: function () { var current = this; $(function () { current._init(); }); },
            ready: function (option) {
                var config = {
                    delectConfirm: '<%: "Are you sure you want to delete these items?".Localize()%>',
                    nullSelected: '<%: "Please select items!".Localize()%>'
                };

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

                    $('input:file.zip').change(function () {
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

                (function imageViewer() {
                    $('a[virtualpath].f-icon').each(function () {
                        var handle = $(this);
                        var virtualpath = $(this).attr('virtualpath');
                        var quickExp = new RegExp('.jpg$|.png$|.gif$|.bmp$|.jpeg$', 'i');
                        if (quickExp.test($(this).attr('virtualpath'))) {
                            handle.attr('rel', 'imageviewer[image]');
                            handle.attr('href', virtualpath);
                        } else {
                            handle.click(function () {
                                window.open(virtualpath);
                            });
                        }
                    });
                    $('a[rel^=imageviewer]').prettyPhoto();
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

                //init tools 
                $("a.action-delete").click(function () {
                    var handle = $(this);
                    var selectedFolders = $("input:checkbox.folders[checked]");
                    var selectedFiles = $("input:checkbox.docs[checked]");
                    if (selectedFiles.length == 0 && selectedFolders.length == 0) {
                        alert(config.nullSelected);
                        return false;
                    }

                    if (confirm(config.delectConfirm)) {
                        var filePaths = [];
                        var directoryPaths = [];

                        var trs = [];

                        selectedFiles.each(function () {
                            trs.push($(this).parents('tr'));
                            filePaths.push($(this).val());
                        });

                        selectedFolders.each(function () {
                            trs.push($(this).parents('tr'));
                            directoryPaths.push($(this).val());
                        });

                        var form = kooboo.cms.ui.formHelper.tempForm(filePaths, handle.attr('href'), 'filePaths', { method: "post" });
                        kooboo.cms.ui.formHelper.tempForm(directoryPaths, handle.attr('href'), 'directoryPaths', { method: "post" }, form.form);

                        form.form.ajaxSubmit({
                            beforeSend: function () {
                                kooboo.cms.ui.loading().show();
                            },
                            success: function (response) {
                                if (response.Success) {
                                    if (selectedFolders.length > 0) {
                                        document.location.reload(true);
                                    } else {
                                        trs.each(function (value) {
                                            value.remove();
                                        });
                                        kooboo.cms.ui.loading().hide();
                                    }
                                } else {
                                    kooboo.cms.ui.messageBox().showResponse(response)
                                }
                            },
                            dataType: 'json'
                        });

                        //form.submit();
                    }


                    return false;
                });

                var supportFiles = ['txt', 'js', 'css', 'rule'];

                var supportSelector = '';
                supportFiles.each(function (val) {
                    supportSelector += ('a.' + 'f-' + val + ',');
                });

                supportSelector += 'a.file-add';

                $(supportSelector).pop({
                    width: 850,
                    height: 600,
                    frameHeight: "99%",
                    onload: function (handle, pop, config) {
                        var status = kooboo.cms.ui.status(Math.random().toString()).msg('<%:"Are you sure you want to close this window.".Localize() %>');
                        pop.data('status', status);
                        var directoryExp = /^\w+$/;
                        var existExp = /theme\.rule$/gi;

                        var enableAddRule = '<%:controller %>'.toLowerCase() == 'themeresource' && directoryExp.test('<%:directoryPath %>');

                        $('input:checkbox').each(function () {
                            if (existExp.test($(this).val())) {
                                enableAddRule = false;
                            }
                        });
                        var iframeContent = pop.find('iframe').contents();

                        var fileExtension = iframeContent.find('#FileExtension');


                        if (!enableAddRule) {
                            fileExtension.find('option[value=".rule"]').hide();
                        }
                    }
                });

                //init folder tr
                var folderTrs = $("div.table-container table tr.foldertr").each(function () {
                    var current = $(this);
                    var edit = current.find("a.edit").hide();
                    edit.click(function () {
                        edit.hide();
                        current.data("status", "edit");
                    });
                    current.hover(function () {
                        if (current.data("status") != "edit") {
                            edit.show();
                        }
                    }, function () {
                        edit.hide();
                    });

                    var editor = current.find("td:eq(1) a:first").inlineEditor({
                        triggerSelector: edit,
                        append: "after",
                        dataName: "Name",
                        url: edit.attr("href"),
                        onsave: function (data) {
                            var url = $.request(this.url);
                            url.queryString["directoryPath"] = data.data;
                            this.url = url.getUrl();
                            current.removeData("status");
                            current.find("a").each(function () {
                                var href = $(this).attr("href");
                                href = $.request(href).queryString["directoryPath"] = data.data;
                                $(this).attr("href", href);

                                kooboo.cms.ui.loading().show();
                                document.location.reload();
                            });
                        },
                        oncancel: function () { current.removeData("status"); }
                    });

                }); //end folder.each
                var filetr = $("div.table-container tr.filetr").each(function () {
                    var current = $(this);
                    current.find("a").click(function () { return false; });
                });

                filetr.find('a.o-icon.link').yardiTip({
                    title: function () {
                        return '<b> <%:"VirtualPath:".Localize() %> </b>' + $(this).attr('virtualpath') + '<br/><b> <%:"RelativePath:".Localize() %> </b>' + $(this).attr('relativepath').replace(/\\/g, '/');
                    },
                    trigger: 'show-yarditip'
                }).click(function () {
                    $(this).trigger('show-yarditip');
                });

                $("div.table-container table").tableSorter({
                    cancel: ".foldertr",
                    beforeUp: function (tr) {
                        if (tr.prev() && tr.prev().hasClass("foldertr")) {
                            return false;
                        }
                    },
                    move: function () {
                        var url = $("#SortUrl").val();
                        var filesOrder = [];
                        $("div.table-container table").find("tr.filetr").each(function () {
                            var current = $(this);
                            filesOrder.push(current.find("input:checkbox").val());
                        });

                        var tempForm = kooboo.cms.ui.formHelper.tempForm(filesOrder, url, "filesOrder", { method: "post" });

                        $.post(url, tempForm.form.serialize());

                    },
                    showUp: function (tr) {
                        if (tr.prev() && tr.prev().hasClass("foldertr") || !tr.prev()) {
                            return false;
                        }
                    },
                    showDown: function (tr) {
                        if (tr.next().length > 0) {
                            return true;
                        } else {
                            return false;
                        }
                    }
                });

                (function newFolder() {
                    $("a.folder-add").click(function () {
                        var handle = $(this);
                        var id = Math.random().toString().replace(".", "_");
                        var tr = $('<tr><td></td><td><input type="text" value="newFolder" class="editor" /><a href="javascript:;" class="o-icon save inline-action">Edit</a><a href="javascript:;" class="o-icon cancel inline-action">Cancel</a></td><td></td><td></td></tr>').attr("id", id);
                        var input = tr.find('input');
                        input.keyup(function (e) {
                            if (e.keyCode == 13) {
                                submit();
                            }
                        });
                        var saveBtn = tr.find('a.save');
                        var cancelBtn = tr.find('a.cancel');
                        function cancel() {
                            tr.remove();
                        }

                        saveBtn.click(function () {
                            submit();
                        });

                        cancelBtn.click(function () {
                            cancel();
                        });

                        tr.appendTo("div.table-container table");
                        setTimeout(function () {
                            input.focus();
                        }, 100);

                        var fList = [];
                        folderTrs.each(function () {
                            fList.push($(this).find('a.f-icon.folder').text().trim());
                        });

                        function submit() {

                            if (fList.where(function (o) { return o.toLowerCase() == input.val().toLowerCase(); }).length > 0) {
                                kooboo.cms.ui.messageBox().show('<%:"This folder is existed.".Localize() %>', 'error');
                                return false;
                            } else {
                                kooboo.cms.ui.messageBox().hide();
                            }

                            $.ajax({
                                url: handle.attr('href'),
                                success: function () {
                                    document.location.reload();
                                },
                                data: {
                                    Name: input.val()
                                },
                                type: "POST"
                            });
                        }

                        document.location.href = "#" + id;

                        return false;
                    });
                })();

                (function large_file() {
                    $('a.dialog-upload').pop({
                        onclose: function () {
                            if (top.kooboo.data('large-file-changed')) {
                                document.location.reload(true);
                            }
                        }
                    });
                })();

                $("input:file.anything").change(function () {
                    $(this).parent().submit();
                });
            }

        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
