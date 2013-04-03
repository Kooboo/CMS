<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% 
	var engines = Kooboo.CMS.Sites.View.TemplateEngines.Engines;
%>
<ul>
	<%foreach (var e in engines)
   {%>
	<li>
		<%:Html.ActionLink(e.Name,"Create",ViewContext.RequestContext.AllRouteValues().Merge("EngineName",e.Name)) %>
	</li>
	<% } %>
</ul>
