<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Sites.Models.CustomFile>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Custom Files</h2>
    <div class="command clearfix">
        <%:Html.ActionLink("Create Directory".Localize(), "CreateDirectory", new { siteName = ViewContext.RequestContext.GetRequestValue("siteName"), fullName = Request["fullName"] }, new { @class = "button", rel = "create directory" })%>
        <div class="create_directory" style="display: none;">
            <label title="Create Directory" class="title">
                <% using (this.Html.BeginForm("CreateDirectory", ViewContext.RequestContext.AllRouteValues()["controller"].ToString(), ViewContext.RequestContext.AllRouteValues(), FormMethod.Post))
                    { %>
                <input id="folderName" name="folderName" type="text" />
                <input id="currentFolder" name="currentFolder" type="hidden" />
                <input id="saveDirectory" type="button" value="submit" />
                <input id="cancelDirectory" type="button" value="cancel" />
                <%} %>
        </div>
        <%: Html.ActionLink("Upload".Localize(), "Create", new { siteName = ViewContext.RequestContext.GetRequestValue("siteName"), fullName = Request["fullName"] }, new { @class = "button"})%>
        <%: Html.ActionLink("Import".Localize(), "Import", new { siteName = ViewContext.RequestContext.GetRequestValue("siteName"), fullName = Request["fullName"] }, new { @class = "button", name = "import" })%>
        <%: Html.Partial("Import", Kooboo.CMS.Web.Areas.Sites.Models.ImportModel.Default)%>
        <%: Html.ActionLink("Export".Localize(), "Export", new { siteName = ViewContext.RequestContext.GetRequestValue("siteName"), fullName = Request["fullName"] }, new { @class = "button"})%>
    </div>
    <img id="previewImg" alt="preview" style="display: none;" />
    <%: Html.GridForModel() %>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            adminJs.fileJs.imagePreview();
            adminJs.fileJs.initCreateDirectory();
            adminJs.fileJs.initFolderClick();
            adminJs.fileJs.initUpperLevel('<%:Request["fullName"] %>');
        });
        
    </script>
</asp:Content>
