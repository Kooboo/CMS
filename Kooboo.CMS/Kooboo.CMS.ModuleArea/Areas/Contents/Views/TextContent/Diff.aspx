<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Contents.Models.TextContentDiffModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Diff
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.ExternalResources("DiffStyles")%>
    <div id="diffBox">
        <div id="leftPane">
            <div class="diffHeader">
                <%= "Version".ToString() %>
                <%=Model.Version1Name %></div>
            <% Html.RenderPartial("DiffPane", Model.Version1); %>
        </div>
        <div id="rightPane">
            <div class="diffHeader">
                <%="Version".Localize()%>
                <%=Model.Version2Name %></div>
            <% Html.RenderPartial("DiffPane", Model.Version2); %>
        </div>
        <div class="clear">
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            InitializeDiffPanes();
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
    <script language="javascript" type="text/javascript" src="<%=Url.Content("~/Areas/Contents/Scripts/Diff.js") %>"></script>
</asp:Content>
