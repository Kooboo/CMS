<%@ Import Namespace="Kooboo.CMS.Content.Models" %>
<%@ Import Namespace="Kooboo.CMS.Content.Services" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FolderTreeNode<TextFolder>>>" %>
<%if (ViewData["Guid"] == null)
  {%>
<script language="javascript" type="text/javascript">

    $(function () {
        var tree = $("#folder-tree").treeview({
            collapsed: true
        });
        tree.find('div').unbind('click').click(function () {
            $(this).siblings('span').click();
        });


        $(':checkbox.folder-checkbox').change(function () {
            $(':checkbox.folder-checkbox[checked]').attr('checked', false);
            $(this).attr('checked', true);
        });
    });

</script>
<% } %>
<ul id="folder-tree<%:ViewData["Guid"] %>" <%if(ViewData["Guid"]==null){%> class="filetree treeview "
    <%} %>>
    <%
        ViewData["Guid"] = Guid.NewGuid();
        foreach (var m in Model)
        { %>
    <li class="clearfix"><span class="folder">
        <input type="checkbox" value="<%=m.Folder.FullName %>" class="folder folder-checkbox"
            name="DataRule.FolderName" />
        <%:Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(m.Folder).FriendlyText%></span>
        <%if (m.Children != null && m.Children.Count() > 0)
          {%>
        <%:Html.Partial("DataRuleFolderGrid",m.Children,ViewData) %>
        <% } %>
    </li>
    <%} %>
</ul>
