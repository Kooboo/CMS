<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Content.Models.Schema>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%="Content Type".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <span>
            <%:"Content type".Localize() %></span> <a class="tooltip-link" href="javascript:;"
                title="The type of information you want to present on the web. For example: articles, news, presentations and products.">
            </a>
    </h3>
    <div class="command clearfix">
        <%: Html.ActionLink("Create".Localize(), "Create", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class="button" }))%>
        <%: Html.ActionLink("Import".Localize(), "Import", ViewContext.RequestContext.AllRouteValues(),new RouteValueDictionary(new { @class="button", name = "import" }))%>
        <%: Html.Partial("Import", Kooboo.CMS.Web.Areas.Contents.Models.ImportModel.Default)%>
        <%:Html.ActionLink("Export".Localize(), "Export", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button export exportCommand", errMsg = "Please select content types!".Localize() }))%>
        <%:Html.ActionLink("Delete".Localize(), "Delete", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button delete deleteCommand", errMsg = "Please select content types!".Localize(), confirm = "Are you sure you want to delete these items?".Localize() }))%>
        <%:Html.ActionLink("Reset templates".Localize(), "ResetTemplates", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button command", errMsg = "Please content types!".Localize(), confirm = "Are you sure you want to reset the templates of this item?".Localize() }))%>
    </div>
    <%: Html.GridForModel() %>
    <%:Html.Partial("CopyForm",new Kooboo.CMS.Web.Models.CopyFormModel()) %>
    <script language="javascript" type="text/javascript">
        //	kooboo.cms.content.schema.initLinkSubmit(".check-command", "#form1");
        //kooboo.cms.content.initRelationPopups(".Action-relations");

        $('.Action-relations').pop({
            width: 600,
            height: 370,
            reload: true,
            isIframe: false
        });

    </script>
</asp:Content>
