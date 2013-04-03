<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<Kooboo.CMS.Content.Models.Workflow>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <span>
            <%:"Workflow".Localize() %></span>
    </h3>
    <div class="command">
        <%:Html.ActionLink("Create".Localize(), "Create", ViewContext.RequestContext.AllRouteValues(), 
        new RouteValueDictionary(new { 
    title="Create",
    Class="button dialog-link"
}))%>
        <%:Html.ActionLink("Delete".Localize(), "Delete", ViewContext.RequestContext.AllRouteValues(),
        new RouteValueDictionary(new
        {
            title = "Create",
            Class = "deleteCommand button"
        }))%>
    </div>
    <%:Html.GridForModel() %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
