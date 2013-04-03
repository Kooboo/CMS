<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Sites.Models.PagePublishingQueueItem>>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Publishing queue".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <span>
            <%:"Publishing queue".Localize()%></span> <a href="javascript:;" title="<%:"The collection of pages to be published".Localize() %>"
                class="tooltip-link right"></a>
    </h3>
    <div class="command clearfix">
        <%: Html.ActionLink("Delete".Localize(), "Delete",ViewContext.RequestContext.AllRouteValues(),  new RouteValueDictionary(new { @class="button deleteCommand" }))%>
    </div>
    <%: Html.GridForModel() %>
</asp:Content>
