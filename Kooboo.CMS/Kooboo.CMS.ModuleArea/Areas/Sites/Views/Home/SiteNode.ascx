<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Sites.Models.SiteNode>" %>
<% var nodeId = string.Join("_", Model.Site.RelativePaths);
   var isOnline = Model.IsOnLine;
   var onLineName = (string.IsNullOrEmpty(Model.Site.DisplayName) == true ? Model.Site.Name : Model.Site.DisplayName);
   var offlineName = onLineName + "(Offline)".Localize();
   var displayName = isOnline ? onLineName : offlineName;
%>
<% if (!ServiceFactory.UserManager.Authorize(Model.Site, ViewContext.HttpContext.User.Identity.Name))
   {
       nodeId = null;
%>
<%}
   else
   { %>
<div class="fm-node map-item <%:Model.IsOnLine?"":"unpublished" %>" id="<%: nodeId %>"
    parentid="<%:ViewData["parentId"]%>" online="<%:onLineName %>" offline="<%:offlineName %>">
    <h4 class="dynamic" title="<%:displayName %>">
        <%: Html.ActionLink(displayName, "SiteMap", new { siteName = Model.Site.FullName, repositoryName = Model.Site.Repository })%></h4>
    <div class="action">
        <a href="javascript:;" title="Click to Expand">View</a>
        <ul>
            <li><a href="<%: FrontUrlHelper.Preview(Url,Model.Site,null,null)%>" title="preview"
                target="_blank">
                <%: "Preview".Localize()%></a></li>
            <li>
                <%: Html.ActionLink("Goto".Localize(), "SiteMap", new { siteName = Model.Site.FullName, repositoryName = Model.Site.Repository })%></li>
            <% if (ServiceFactory.UserManager.AllowCreatingSite(Page.User.Identity.Name))
               {%>
            <li>
                <%: Html.ActionLink("Add sub site".Localize(), "Create", "Site", new { siteName = Model.Site.FullName }, new { @class = "icon icon-create site-create dialog-link", title = "Create a sub site".Localize() })%>
            </li>
            <%} %>
            <% if (Kooboo.CMS.Sites.Services.ServiceFactory.UserManager.Authorize(Model.Site, Page.User.Identity.Name, Kooboo.CMS.Account.Models.Permission.Sites_Settings_SystemPermission))
               { %>
            <li class="site-export">
                <%:Html.ActionLink("Export".Localize(), "Export", new { controller = "site", siteName = Model.Site.FullName })%>
            </li>
            <li>
                <%:Html.ActionLink("Delete".Localize(), "Delete", "Site", new { siteName = Model.Site.FullName }, new { @class = "deleteNode", FullName = Model.Site.FullName, title = "Confirm".Localize() })%>
            </li>
            <li>
                <%:Html.ActionLink(Model.IsOnLine ? "Take offline".Localize() : "Take online".Localize(), "SwitchOffOn", "Site", new { siteName = Model.Site.FullName }, new { @class = "switch-off-on", OnLine = "Take online".Localize(), OffLine = "Take offline".Localize(), IsOnline = Model.IsOnLine, FullName = Model.Site.FullName })%>
            </li>
            <%} %>
        </ul>
    </div>
</div>
<%} %>
<% foreach (var item in Model.Children)
   {
       var viewData = new ViewDataDictionary();
       viewData.Add("parentId", nodeId);       
%>
<%: Html.Partial("SiteNode",item, viewData)%>
<%} %>