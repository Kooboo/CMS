<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Kooboo.CMS.Sites.Models.Layout>>" %>
<ul>
    <%foreach (var l in Model)
      {%>
    <li>
        <%:Html.ActionLink(l.Name, "Create", "Page", ViewContext.RequestContext.AllRouteValues().Merge("FullName","").Merge("Layout", l.Name).Merge("ReturnUrl", Url.Action("Index", ViewContext.RequestContext.AllRouteValues())).Merge("ParentPage",Request["FullName"]), new RouteValueDictionary())%>
    </li>
    <% } %>
</ul>
