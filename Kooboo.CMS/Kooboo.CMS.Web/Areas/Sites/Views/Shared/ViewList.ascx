<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Sites.Services.Namespace<Kooboo.CMS.Sites.Models.View>>" %>
<% 
	var viewEngine = (Kooboo.CMS.Sites.View.ITemplateEngine)ViewData["ViewEngine"];
%>
<ul>
    <% foreach (var e in Model.Entries)
	{%>
    <li title="<%=e.FullName %>"><a href="javascript:;" title='<%=viewEngine.GetCodeHelper().RenderView(e.FullName)%>'>
        <%=e.Name %>
    </a></li>
    <%} %>
    <%foreach (var ns in Model.ChildNamespaces)
   {%>
    <li title="<%=ns.FullName %>" class="has-sub"><a href="javascript:;">
        <%=ns.Name%></a>
        <%:Html.Partial("ViewList",ns) %>
    </li>
    <%} %>
</ul>
