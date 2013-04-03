<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<Kooboo.CMS.Search.Models.SearchSetting>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Search settings".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%:"Search settings".Localize() %>
    </h3>
    <div class="command clearfix">
        <a class="button dialog-link" href="<%=Url.Action("Create",ViewContext.RequestContext.AllRouteValues()) %>"
            title="<%:"Create".Localize() %>">
            <%:"Create".Localize() %></a> <a class="button deleteCommand" href="<%=Url.Action("Delete",ViewContext.RequestContext.AllRouteValues()) %>">
                <%:"Delete".Localize() %></a> <%--<a class="button" href="<%=Url.Action("RebuildIndex",ViewContext.RequestContext.AllRouteValues()) %>">
                    <%:"Rebuild index".Localize() %></a>--%>
    </div>
    <%: Html.GridForModel() %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
