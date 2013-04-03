<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% 
	 var layoutList = Kooboo.CMS.Web.Models.ModelHelper.ParseViewDataToList<Kooboo.CMS.Sites.Models.Layout>(ViewData["LayoutList"]);

	 var model = (RouteValueDictionary)Model;
	%>
<ul>
	<%if (layoutList.Count() == 0)
	{ %>
	<li><a href="javascript:;">No layout yet</a></li>
	<%} %>
	<%foreach (var l in layoutList) {%>
	<li><%:Html.ActionLink(l.Name,"Create","Page",model.Merge("Layout",l.Name),new RouteValueDictionary()) %> </li>
	<% } %>
</ul>
