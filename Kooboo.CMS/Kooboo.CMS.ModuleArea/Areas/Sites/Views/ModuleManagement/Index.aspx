<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Sites.Extension.ModuleArea.ModuleInfo>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Modules".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%: "Modules".Localize()%>
    </h2>
    <div class="command clearfix">
        <%: Html.ActionLink("Install".Localize(), "Install", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button dialog-link", title = "Module installation".Localize() }))%>
    </div>
    <%: Html.GridForModel() %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
