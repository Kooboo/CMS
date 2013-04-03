<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Content.Models.SendingSetting>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:"Sending setting".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="command">
        <%: Html.ActionLink("Create".Localize(), "Create", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class="button dialog-link" ,title="Create".Localize() }))%>
        <%: Html.ActionLink("Delete".Localize(), "Delete", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button deleteCommand" }))%>
    </div>
    <%:Html.GridForModel() %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
