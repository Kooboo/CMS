<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.Web.Mvc.Paging.PagedList<Kooboo.Globalization.Element>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Labels".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%:"Labels".Localize() %>
        <%=ViewContext.RequestContext.GetRequestValue("category") %></h3>
    <div class="command clearfix">
        <%: Html.ActionLink("Delete".Localize(), "Delete", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class="button deleteCommand" }))%>
<%: Html.Partial("Search") %>
    </div>
    <%: Html.GridFor(typeof(Kooboo.Globalization.Element), Model,"LabelGridTemplate")%>
    <div class="pagination right">
        <%: Html.Pager(Model, "", ViewContext.RequestContext.AllRouteValues(),null)%>
    </div>
    <script language="javascript" type="text/javascript">
        adminJs.label.init();
    </script>
</asp:Content>
