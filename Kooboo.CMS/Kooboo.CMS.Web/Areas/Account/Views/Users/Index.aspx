<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Account/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.Web.Mvc.Paging.PagedList<Kooboo.CMS.Account.Models.User>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:"Users".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%:"Users".Localize()%></h3>
    <div class="command clearfix">
        <%: Html.ActionLink("Create".Localize(), "Create", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button dialog-link" , title="Create user".Localize()}))%>
        <%:Html.ActionLink("Delete".Localize(), "Delete", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button delete deleteCommand", errMsg = "Please select schemas!".Localize(), confirmMsg = "Are you sure you want to delete these items?".Localize() }))%>
<%:Html.Partial("Search")%>
    </div>
    <%: Html.GridFor(typeof(Kooboo.CMS.Account.Models.User),Model) %>
    <div class="pagination right">
        <%: Html.Pager(Model, "", ViewContext.RequestContext.AllRouteValues(),null)%>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
