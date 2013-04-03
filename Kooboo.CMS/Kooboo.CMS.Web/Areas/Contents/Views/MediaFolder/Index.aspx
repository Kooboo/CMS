<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Content.Models.MediaFolder>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= "Media library".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%var routes = ViewContext.RequestContext.AllRouteValues();
      routes.Merge("r", "r");
    %>
    <h3 class="title">
        <%= "Media library".Localize() %></h3>
    <%:Html.Partial("Search") %>
    <div class="command">
        <table>
            <tbody>
                <tr>
                    <td>
                        <input type="text" id="addressInput" style="width: 200px;" />
                    </td>
                    <td>
                        <a href="<%=this.Url.Action("Index",routes) %>" class="o-icon go" title="<%="Go".Localize() %>">Go</a>
                    </td>
                    <% if (Kooboo.CMS.Web.Authorizations.AuthorizationHelpers.Authorize(ViewContext.RequestContext, Kooboo.CMS.Account.Models.Permission.Contents_FolderPermission))
                       {%>
                    <td>
                        <a href="<%=this.Url.Action("Delete",routes) %>" confirmmsg="<%="Are you sure you want to delete these items?".Localize() %>"
                            alertmsg="<%="You have not select any item?".Localize() %>" class="o-icon cross action-delete" title="<%="Delete".Localize() %>">Delete</a>
                    </td>
                    <td>
                        <a href="<%=this.Url.Action("Create","MediaFolder",routes) %>" class="o-icon folder-add" title="<%="Create folder".Localize() %>"></a>
                    </td>
                    <%} %>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="table-container">
        <table>
            <thead>
                <th class="checkbox">
                    <input type="checkbox" class="select-all" />
                </th>
                <th>
                    <%="title".Localize()%>
                </th>
                <th class="type">
                    <%="Type".Localize() %>
                </th>
                <th class="date">
                    <%="Date".Localize() %>
                </th>
            </thead>
            <tbody>
                <%foreach (var folder in Model)
                  {
                      var currentRoutes = ViewContext.RequestContext.AllRouteValues();
                      currentRoutes["folderName"] = folder.FullName;
                      currentRoutes["fullName"] = folder.FullName;
                %>
                <tr class="foldertr">
                    <td>
                        <input type="checkbox" name="Selected" value="<%=folder.FullName %>" class="folder select" />
                    </td>
                    <td>
                        <input type="hidden" value="<%=folder.FullName %>" name="fullName" />
                        <input type="hidden" value="<%=Url.Action("Edit","MediaFolder",currentRoutes) %>"
                            name="editUrl" />
                        <%:Html.ActionLink(string.IsNullOrEmpty(folder.DisplayName) ? folder.Name : folder.DisplayName, "Index","MediaContent", currentRoutes, new RouteValueDictionary(new { @class="f-icon folder" }))%>
                    </td>
                    <td class="type">
                        <%="Folder".Localize() %>
                    </td>
                    <td class="date">
                    </td>
                </tr>
                <% } %>
            </tbody>
        </table>
    </div>
	<%:Html.Partial("MediaContent.Script") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
