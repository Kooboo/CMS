<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Sites.Models.Layout>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Layouts".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%="Layouts".Localize() %></h3>
    <div class="command clearfix">
        <%if (Kooboo.CMS.Sites.View.TemplateEngines.Engines.Count() == 1)
          {%>
        <%:Html.ActionLink("Create".Localize(), "Create", ViewContext.RequestContext.AllRouteValues().Merge("EngineName", Kooboo.CMS.Sites.View.TemplateEngines.Engines.First().Name), new RouteValueDictionary(new { @class = "button" }))%>
        <%}
          else
          {%><div class="dropdown-button">
        <span>
            <%="Create New".Localize()%></span>
        <div class="hide">
            <%:Html.Partial("TemplateEngineList") %>
        </div>
    </div>
        <% } %>
        <%: Html.ActionLink("Import".Localize(), "Import", ViewContext.RequestContext.AllRouteValues(),new RouteValueDictionary(new { @class="button", name = "import" }))%>
        <%: Html.Partial("Import", Kooboo.CMS.Web.Areas.Sites.Models.ImportModel.Default)%>
        <%: Html.ActionLink("Export".Localize(), "Export", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class="button exportCommand" }))%>
        <%: Html.ActionLink("Delete".Localize(), "Delete", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class="button deleteCommand" }))%>
    </div>
    <%: Html.GridForModel() %>
    <%:Html.Partial("CopyForm",new Kooboo.CMS.Web.Models.CopyFormModel()) %>
    <script language="javascript" type="text/javascript">
        $(function () {
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
        });
    </script>
</asp:Content>
