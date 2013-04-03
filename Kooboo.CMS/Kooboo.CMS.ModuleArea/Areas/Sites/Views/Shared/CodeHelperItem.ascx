<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CodeSnippetGroup>" %>
<%if (Model != null) {%>
<ul>
    <%foreach (var c in Model.CodeSnippets) {%>
    <li><a href="javascript:;" title="<%:c.Code %>">
        <%:c.Name %></a> </li>
    <% } %>
    <%foreach (var g in Model.ChildGroups) {%>
    <li class="<%:g.ChildGroups.Count() > 0 || g.CodeSnippets.Count() > 0 ?"has-sub":"" %>"><a href="javascript:;">
        <%:g.Name %></a>
        <%:Html.Partial("CodeHelperItem",g) %>
    </li>
    <% } %>
</ul>
<% } %>
