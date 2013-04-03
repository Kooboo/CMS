<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Sites.Models.AssemblyFile>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=  "Plug-ins".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%:"Plug-ins".Localize() %></h3>
    <div class="command clearfix">
        <%: Html.ActionLink("Upload".Localize(), "Upload", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class="button dialog-link",title="Upload" }))%>
        <%: Html.ActionLink("Delete".Localize(), "Delete",ViewContext.RequestContext.AllRouteValues(),  new RouteValueDictionary(new { @class="button deleteCommand" }))%>
    </div>
    <%: Html.GridForModel() %>
    <input type="hidden" id="_pop_config_edit" selector="a.ViewTypes" value="?title=<%="ViewType".Localize() %>&height=600&width=900"
        loadevent="adminJs.global.commomPopFormLoad" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
