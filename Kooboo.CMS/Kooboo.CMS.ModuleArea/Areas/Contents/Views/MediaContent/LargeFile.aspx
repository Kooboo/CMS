<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= "LargeFile".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptCSS" runat="server">
    <link href="<%=Url.Content("~/Styles/uploadify.css") %>" rel="Stylesheet" />
    <script language="javascript" type="text/javascript" src="<%=Url.Content("~/Scripts/uploadify/swfobject.js") %>"></script>
    <script language="javascript" type="text/javascript" src="<%=Url.Content("~/Scripts/uploadify/jquery.uploadify.v2.1.4.min.js") %>"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            var u = $('#LargeFile').uploadify({
                'uploader': '<%=Url.Content("~/Scripts/uploadify/uploadify.swf") %>',
                'script': '<%=Url.Action("LargeFile",ViewContext.RequestContext.AllRouteValues()) %>',
                'folder': '<%:Request["FolderName"] %>',
                'cancelImg': '<%=Url.Content("~/Scripts/uploadify/cancel.png") %>',
                'multi': true,
                "sizeLimit": 30000000,
                onAllComplete: function (event, data) {
                    top.kooboo.data('large-file-changed', data.filesUploaded > 0);
                },
                scriptData: {
                    folderName: '<%:Request["FolderName"] %>',
                    siteName: '<%:Request["SiteName"] %>'
                }
            });

            $('#upload-btn').click(function () {
                var largeFile = $('#LargeFile');
                largeFile.uploadifySettings("scriptData", {
                    folderName: '<%:Request["FolderName"] %>',
                    siteName: '<%:Request["SiteName"] %>',
                    overrided: $('#Overrided').attr('checked')
                });
                largeFile.uploadifyUpload();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <form action="<%=Url.Action("LargeFile",ViewContext.RequestContext.AllRouteValues()) %>">
    <div style="height: 255px; overflow: scroll; margin-bottom: 10px;">
        <input id="LargeFile" type="file" name="LargeFile" />
    </div>
    <div class="clearfix">
        <input type="checkbox" value="True" name="Overrided" id="Overrided" />
        <label for="Overrided">
            <%:"Overrided".Localize() %></label></div>
    <p class="buttons">
        <a href="javascript:;" id="upload-btn" class="button">Upload</a>
    </p>
    </form>
</asp:Content>
