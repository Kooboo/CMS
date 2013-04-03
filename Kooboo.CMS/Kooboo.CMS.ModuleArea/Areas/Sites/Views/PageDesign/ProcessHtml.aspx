<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/PageDesign/Design.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentFirst" runat="server">
    <script type="text/javascript">
        function getDesignerType(types) { return types['html']; }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var title = "Selected Files".Localize();
        var url = Url.Action("Selection", "MediaContent", ViewContext.RequestContext.AllRouteValues()
            .Merge("area", "contents").Merge("repositoryName", Kooboo.CMS.Sites.Models.Site.Current.Repository));
    %>
    <textarea id="Textarea1" name="Html" style="width: 730px; height: 400px; visibility: hidden;"
        media_library_url="<%=url%>" media_library_title="<%=title%>"></textarea>
</asp:Content>
