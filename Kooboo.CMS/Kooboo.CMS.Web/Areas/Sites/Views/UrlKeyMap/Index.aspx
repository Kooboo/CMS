<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Sites.Models.UrlKeyMap>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=  "Page mapping".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <span>
            <%:"Page mapping".Localize()%></span> <a href="javascript:;" title="<%:"Map a url key which used in PageUrl to a page".Localize() %>"
                class="tooltip-link"></a>
    </h3>
    <div class="command clearfix">
        <%: Html.ActionLink("Create".Localize(), "Create", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button dialog-link", title = "Create Page mapping" }))%>
        <%: Html.ActionLink("Import".Localize(), "Import", ViewContext.RequestContext.AllRouteValues(),new RouteValueDictionary(new { @class="button", name = "import" }))%>
        <%: Html.Partial("Import", Kooboo.CMS.Web.Areas.Sites.Models.ImportModel.Default)%>
        <%: Html.ActionLink("Export".Localize(), "Export",ViewContext.RequestContext.AllRouteValues(),  new RouteValueDictionary(new { @class="button " }))%>
        <%: Html.ActionLink("Delete".Localize(), "Delete",ViewContext.RequestContext.AllRouteValues(),  new RouteValueDictionary(new { @class="button deleteCommand" }))%>
    </div>
    <%: Html.GridForModel() %>
</asp:Content>
