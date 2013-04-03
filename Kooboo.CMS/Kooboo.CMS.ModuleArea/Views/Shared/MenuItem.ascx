<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.Web.Mvc.Menu.MenuItem>" %>
<% if (Model.Visible)
   { %>
<% var menuItem = Model as Kooboo.Web.Mvc.Menu.MenuItem;
   if (menuItem != null)
   {
       var controller = menuItem.Controller ?? ViewContext.RequestContext.GetRequestValue("controller");
       var folderClass = "";
       var folderControllders = new string[]{
        "TextFolder",
        "MediaContent",
        "ThemeResource",
        "FileResource",
        "View"
       };

       if (folderControllders.Where(o => o.ToLower() == controller.ToLower()).Count() == 1)
       {
           folderClass = "folder";
       }
       var subMenuCount = menuItem.Items == null ? 0 : menuItem.Items.Where(it => it.Visible == true).Count();
%>
<li class="<%=  menuItem.Name %><%: (subMenuCount>0)?" has-sub ":" " %>&nbsp;<%: menuItem.IsActive? " active ":" " %><%=folderClass %>">
    <% if (subMenuCount > 0)
       { %>
    <span class="arrow"></span>
    <% } %>
    <% if (!string.IsNullOrEmpty(menuItem.Action) || !string.IsNullOrEmpty(menuItem.Controller))
       { %>
    <%: Html.ActionLink(menuItem.Localizable ? menuItem.Text.Localize() : menuItem.Text, menuItem.Action, menuItem.Controller, menuItem.RouteValues, menuItem.HtmlAttributes)%>
    <%}  %>
    <%else
       { %>
    <a href="javascript:;">
        <%: menuItem.Text.Localize()%></a>
    <%} %>
    <%} %>
    <% if (Model.Items != null & Model.Items.Count() > 0)
       {  %>
    <ul>
        <%foreach (var item in Model.Items)
          {
        %>
        <%: Html.Partial("MenuItem", item)%>
        <%} %>
    </ul>
    <%} %>
    <% if (menuItem != null)
       {  %>
</li>
<%} %>
<%}  %>