<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Web.Areas.Sites.Models.ModuleListInSiteModel>>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Modules".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%: "Modules".Localize()%>
    </h3>
    <div class="command clearfix">
        <%: Html.ActionLink("Include".Localize(), "Include", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button command",message="" }))%>
    </div>
    <%: Html.GridForModel() %>
</asp:Content>
