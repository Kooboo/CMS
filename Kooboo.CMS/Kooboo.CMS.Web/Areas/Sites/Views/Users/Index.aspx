<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Sites.Models.User>>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
<%: "Site users".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%:"Site users".Localize()%></h3>
    <div class="command clearfix">
        <%: Html.ActionLink("Create".Localize(), "Create", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { title="Create user".Localize(), @class = "button dialog-link" }))%>
        <%: Html.ActionLink("Delete".Localize(), "Delete", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class="button deleteCommand" }))%>
    </div>
    <%: Html.GridForModel() %>
</asp:Content>
