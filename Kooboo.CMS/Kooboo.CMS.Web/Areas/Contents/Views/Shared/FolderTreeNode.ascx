<%@ Import Namespace="Kooboo.CMS.Content.Models" %>
<%@ Import Namespace="Kooboo.CMS.Content.Services" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FolderTreeNode<TextFolder>>>" %>
<%
    var values = ViewData["Values"] as List<string> ?? new List<string>();
    if (Model != null && Model.Count() > 0)
  {%>
<ul id="folder-tree<%:ViewData["Guid"] %>" <%if(ViewData["Guid"]==null){%> class="filetree treeview "
    <%}else{%> class="clear" <%} %>>
    <%
      ViewData["Guid"] = Guid.NewGuid();
      foreach (var m in Model)
      {
          var guid = Guid.NewGuid();
    %>
    <li class="clearfix"><span class="folder left">
        <input type="checkbox" id="<%:guid %>" value="<%=m.Folder.FullName %>" class="folder folder-checkbox"
            name="<%:ViewData["InputName"] %>" <%if(values.Contains(m.Folder.FullName)){%> checked <%} %>/>
        &nbsp; </span>
        <label class="right" for="<%:guid %>">
            <%:(Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(m.Folder)??m.Folder).FriendlyName%></label>
        <%if (m.Children != null && m.Children.Count() > 0)
          {%>
        <%:Html.Partial("FolderTreeNode",m.Children,ViewData) %>
        <% } %>
    </li>
    <%} %>
</ul>
<% } %>
