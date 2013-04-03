<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="header" class="clearfix">
    <h1 class="logo">
        <a href="<%=Url.Action("SiteMap","Home",new {area="Sites"}) %>" title="<%="Back to site manager".Localize() %>">
            <img src="<%=Url.Content("~/Images/logo.png") %>" alt="LOGO" /><span>home</span></a></h1>
    <ul class="quicklink">
        <li><strong>
            <%: ViewContext.HttpContext.User.Identity.Name %></strong></li>
        <%if (ServiceFactory.UserManager.IsAdministrator(Page.User.Identity.Name))
          { %>
        <li>
            <%: Html.ActionLink("Modules".Localize(), "Index", new { controller = "ModuleManagement", area = "Sites" })%></li>
        <li>
            <%: Html.ActionLink("Users & Roles".Localize(), "index", new { controller="Users",area="account" }) %></li>
        <%} %>
        <li>
            <%:Html.ActionLink("Change Password".Localize(), "ChangePassword", new { area = "account", controller = "Users" }, new { @class = "dialog-link", title = "Change password".Localize() })%>
        </li>
        <li>
            <% 
                var currentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            %>
            <a class="arrow" href="#">
                <%= currentUICulture.NativeName %></a>
            <ul>
                <%foreach (var item in Kooboo.Globalization.ElementRepository.DefaultRepository.EnabledLanguages())
                  {%>
                <li>
                    <%: Html.ActionLink(@item.NativeName, "ChangeUICulture", "Users", new { area = "account", controller = "Users", UICulture = item.Name,returnUrl = Request.RawUrl }, new { @class = currentUICulture.LCID == item.LCID ? "active" : "" })%></li>
                <% } %>
            </ul>
        </li>
        <li class="last">
            <%: Html.ActionLink("Sign Out".Localize(), "SignOut", new { controller = "Logon", area = "account" })%></li>
    </ul>
</div>
