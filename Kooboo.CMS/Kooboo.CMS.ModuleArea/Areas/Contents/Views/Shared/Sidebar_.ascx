<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Kooboo.CMS.Web.Areas.Contents" %>
<div class="block">
    <h4>
        Content Manager</h4>
    <div class="switch clearfix">
        <% if (Kooboo.CMS.Content.Models.Repository.Current != null)
           { %>
        <span class="title">
            <%=Kooboo.CMS.Content.Models.Repository.Current == null
                                                                ? ViewContext.RequestContext.GetRequestValue("repositoryName")
                                                                                                               : (string.IsNullOrEmpty(Kooboo.CMS.Content.Models.Repository.Current.DisplayName) ?
                                                                                                                                               ViewContext.RequestContext.GetRequestValue("repositoryName") : Kooboo.CMS.Content.Models.Repository.Current.DisplayName)%></span>
        <%}
           else
           { %>
        <span><%:"No database selected".Localize() %></span>
        <%} %>
        <%: Html.ActionLink("Create", "Create", "Repository", null, new { @class = "icon icon-create dialog-link", title = "Create new content repository".Localize() })%>
        <a class="icon icon-switch" href="#" title="<%= "Choose an existing repository".Localize() %>">Switch</a>
        <div class="list">
            <%: Html.Partial("Menu", Kooboo.CMS.Web.Areas.Contents.Menus.RepositoriesMenu.BuildMenu(ViewContext.Controller.ControllerContext))%>
        </div>
    </div>
    <div class="menu">
        <%: Html.Partial("Menu", Kooboo.Web.Mvc.Menu.MenuFactory.BuildMenu(ViewContext.Controller.ControllerContext).SetCurrentRepository(this.ViewContext))%>
    </div>
</div>
