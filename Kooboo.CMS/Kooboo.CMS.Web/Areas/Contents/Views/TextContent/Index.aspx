<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Contents.Models.TextContentGrid>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%
        var folder = ViewData["Folder"] as Kooboo.CMS.Content.Models.TextFolder;

        if (folder != null)
        {%>
    <%=folder.FriendlyText %>
    <% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">   
    <ul class="table-desc">
        <li><span class="red"></span>
            <%="Unlocalized item".Localize()%></li>
        <li><span class="blue"></span>
            <%="Workflow item".Localize()%></li>
    </ul>
    <%: Html.Partial(ViewData["Template"].ToString(), Model)%>  
</asp:Content>
