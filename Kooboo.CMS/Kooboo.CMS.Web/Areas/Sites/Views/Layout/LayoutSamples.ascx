<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Kooboo.CMS.Sites.Models.LayoutSample>>" %>
<%@ Import Namespace="Kooboo.CMS.Sites.Models" %>

<ul class="layout-sample">
    <%
        int index = 0;
        foreach (var item in Model)
        { %>
    <li <%=index==Model.Count()-1?"class=\"last\"":"" %>><a href="javascript:;"
        rel="<%:item.Template %>" title="<%=item.Name %>">
        <%=item.Name%></a></li>
    <%
index++;
        } %>
</ul>
