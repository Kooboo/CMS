<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Sites.Models.SiteMapNode>" %>
<% if (Model != null)
   {
       var isLocal = Model.Page.IsLocalized(Kooboo.CMS.Sites.Models.Site.Current);
       var isStaticPage = Kooboo.CMS.Sites.Services.ServiceFactory.PageManager.IsStaticPage(Kooboo.CMS.Sites.Models.Site.Current, Model.Page);

       var hasEditPagePermission = Kooboo.CMS.Sites.Services.ServiceFactory.UserManager.Authorize(Kooboo.CMS.Sites.Models.Site.Current,
           Page.User.Identity.Name, Kooboo.CMS.Account.Models.Permission.Sites_Page_EditPermission);

       var hasDraft = ServiceFactory.PageManager.HasDraft(Model.Page);
       
%>
<% var nodeId = ViewData["parentId"] == null ? "root" : "siteMapNode_" + string.Join("_", Model.Page.PageNamePaths);%>
<div class="fm-node map-item <%: isLocal ? "" :"from-parent" %> <%:isStaticPage?"static":"dynamic" %> <%:!Model.Page.Published == true ?"unpublished":"" %>"
    id="<%: nodeId %>" parentid="<%:ViewData!=null? ViewData["parentId"]:""%>">
    <h4 class="dynamic <%:Model.Page.Navigation.Show?" in-nav":"" %>" title="<%:Model.Page.Name%>">
        <%:(Model.Page.Navigation==null || string.IsNullOrEmpty(Model.Page.Navigation.DisplayText))? Model.Page.Name:Model.Page.Navigation.DisplayText%></h4>
    <div class="action">
        <a href="javascript:;" title="Click to Expand">View</a>
        <%
   var currentRoutes = ViewContext.RequestContext.AllRouteValues()
       .Merge("controller", "page")
       .Merge("FullName", Model.Page.FullName)
       .Merge("fromSite", Model.Page.Site.FullName)
       .Merge("ReturnUrl", Request.RawUrl);

   var addPageRoutes = new RouteValueDictionary(currentRoutes);
   if (nodeId != "root")
   {
       addPageRoutes = addPageRoutes.Merge("ParentPage", Model.Page.FullName);
   }

   if (isLocal)
   {  %>
        <ul>
            <%if (isStaticPage)
              { %>
            <li><a href="<%: FrontUrlHelper.Preview(Url,Kooboo.CMS.Sites.Models.Site.Current,Model.Page,null)%>"
                title="preview" target="_blank">
                <%: "Preview".Localize() %></a></li>
            <%} %>
            <%if (hasEditPagePermission)
              {  %>
            <li <%if(hasDraft){%> class="has-sub" <%} %>>
                <%if (!hasDraft)
                  {%>
                <%: Html.ActionLink("Edit".Localize(), "Edit", currentRoutes)%>
                <% }
                  else
                  {%>
                <a href="javascript:;">
                    <%:"Edit".Localize() %></a>
                <ul>
                    <li>
                        <%: Html.ActionLink("Edit".Localize(), "Edit", currentRoutes)%>
                    </li>
                    <li>
                        <%: Html.ActionLink("Draft".Localize(), "Draft", currentRoutes)%>
                    </li>
                </ul>
                <% } %>
            </li>
            <li>
                <%:Html.ActionLink("Copy".Localize(), "CopyPage", "Page", ViewContext.RequestContext.AllRouteValues().Merge("controller", "page").Merge("SourcePage", Model.Page.FullName), new RouteValueDictionary(new { @class = "dialog-link", FullName = Model.Page.FullName, title = "Copy Page".Localize() }))%>
            </li>
            <li class="has-sub"><a href="#">
                <%:"Add sub page".Localize() %>
            </a>
                <%:Html.Partial("LayoutList", addPageRoutes)%>
            </li>
            <%if (ServiceFactory.UserManager.Authorize(Kooboo.CMS.Sites.Models.Site.Current, ViewContext.HttpContext.User.Identity.Name, Kooboo.CMS.Account.Models.Permission.Sites_Page_PublishPermission))
              {%>
            <%if (!Model.Page.HasParentVersion())
              {%>
            <li>
                <%:Html.ActionLink("Delete".Localize(), "Delete", "Page", ViewContext.RequestContext.AllRouteValues().Merge("controller", "page").Merge("FullName", null).Merge("ReturnUrl", Request.RawUrl).Merge("fromSite", Model.Page.Site.Name), new RouteValueDictionary(new { @class = "commandNode", FullName = Model.Page.FullName, confirm = "Are you sure you want to delete this page?".Localize() }))%>
            </li>
            <%}
              else
              {%>
            <li>
             <%:Html.ActionLink("Unlocalize".Localize(), "Unlocalize", "Page", ViewContext.RequestContext.AllRouteValues().Merge("controller", "page").Merge("FullName", Model.Page.FullName).Merge("ReturnUrl", Request.RawUrl).Merge("fromSite", Model.Page.Site.Name), new RouteValueDictionary(new { @class = "commandNode", confirm = "Are you sure you want to unlocalize this item?".Localize() }))%>
            </li>
            <% } %>
            <li><a href="javascript:;" publishurl="<%=Url.Action("Publish","Page",ViewContext.RequestContext.AllRouteValues().Merge("FullName",Model.Page.FullName ).Merge("ReturnUrl", Request.RawUrl)) %>"
                unpublishurl="<%=Url.Action("Unpublish","Page",ViewContext.RequestContext.AllRouteValues().Merge("FullName",Model.Page.FullName )) %>"
                class="publish" publishtext=" <%:"Publish".Localize()%>" unpublishtext=" <%:"Unpublish".Localize()%>">
                <%:Model.Page.Published == true ? "Unpublish".Localize() : "Publish".Localize()%></a></li>
            <li>
                <%:Html.ActionLink("Move To".Localize(), "MovePage", "Page", ViewContext.RequestContext.AllRouteValues().Merge("controller", "page").Merge("FullName", Model.Page.FullName), new RouteValueDictionary(new { @class = "dialog-link", FullName = Model.Page.FullName, title = "Move Page".Localize() }))%>
            </li>
            <%}
              } %>
        </ul>
        <%}
   else
   { %>
        <ul>
            <%if (isStaticPage)
              { %>
            <li><a href="<%: FrontUrlHelper.Preview(Url,Kooboo.CMS.Sites.Models.Site.Current,Model.Page,null)%>"
                title="preview" target="_blank">
                <%: "Preview".Localize() %></a></li>
            <%} %>
            <li><a href="<%:this.Url.Action("Localize","Page",currentRoutes) %>" onclick="return confirm('<%:"Are you sure you want to localize this item?".Localize() %>');">
                <%:"Localize".Localize() %>
            </a></li>
        </ul>
        <%} %>
    </div>
</div>
<% foreach (var item in Model.Children)
   {
       var viewData = new ViewDataDictionary();
       viewData.Add("parentId", nodeId);
       viewData["LayoutList"] = ViewData["LayoutList"];
%>
<%: Html.Partial("SiteMapNode",item, viewData)%>
<%} %>
<%} %>