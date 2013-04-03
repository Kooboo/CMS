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
        <%: Html.ActionLink("Import".Localize(), "Import", new { siteName = ViewContext.RequestContext.GetRequestValue("siteName") }, new { @class = "button", name = "import" })%>
        <%: Html.Partial("Import", Kooboo.CMS.Web.Areas.Sites.Models.ImportModel.Default)%>
        <%: Html.ActionLink("Export".Localize(), "Export", new { siteName = ViewContext.RequestContext.GetRequestValue("siteName") }, new { @class = "button" })%>
        <div class="dropdown-button">
            <span>
                <%="Batch actions".Localize()%></span>
            <div class="hide">
                <ul>
                    <li>
                        <%: Html.ActionLink("Delete".Localize(), "Delete", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class="deleteCommand" }))%>
                    </li>
                </ul>
            </div>
        </div>
        <%: Html.Partial("Search") %>
    </div>
    <%: Html.GridFor(typeof(Kooboo.Globalization.Element), Model,"LabelGridTemplate")%>
    <div class="pagination right">
        <%: Html.Pager(Model, "", ViewContext.RequestContext.AllRouteValues(),null)%>
    </div>
    <script language="javascript" type="text/javascript">
        adminJs.label.init();
        $(document).click(function () {
            $('div.dropdown-button').children('div').addClass('hide');
        });
        var dropdown = $('div.dropdown-button').click(function (e) {
            e.stopPropagation();
            var menu = $(this).children('div');
            if (menu.hasClass('hide')) {
                menu.removeClass('hide');
            } else {
                menu.addClass('hide');
            }
        }).children().click(function () {
            $('div.dropdown-button').children('div').addClass('hide');
        });
    </script>
</asp:Content>
