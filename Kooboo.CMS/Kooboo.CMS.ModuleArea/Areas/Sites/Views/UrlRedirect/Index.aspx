<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Sites.Models.UrlRedirect>>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Url redirects".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%:"Url redirects".Localize() %></h3>
    <div class="command clearfix">
        <%: Html.ActionLink("Create".Localize(), "Create", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button dialog-link", title = "Create URL redirect".Localize() }))%>
        <%: Html.ActionLink("Import".Localize(), "Import", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button", name = "import" }))%>
        <%: Html.Partial("Import", Kooboo.CMS.Web.Areas.Sites.Models.ImportModel.Default)%>
        <%: Html.ActionLink("Export".Localize(), "Export", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class="button" }))%>
        <%: Html.ActionLink("Delete".Localize(), "Delete",ViewContext.RequestContext.AllRouteValues(),  new RouteValueDictionary(new { @class="button deleteCommand" }))%>
    </div>
    <%: Html.GridForModel() %>
</asp:Content>
