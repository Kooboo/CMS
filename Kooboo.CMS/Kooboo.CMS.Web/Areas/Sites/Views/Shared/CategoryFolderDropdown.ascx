<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Kooboo.CMS.Content.Models.TextFolder>>" %>
<%@ Import  Namespace="Kooboo.CMS.Content.Models" %>

<select name="DataRule.CategoryFolderName" folderInfoUrl="<%:this.Url.Action("GetFolderInfo",ViewContext.RequestContext.AllRouteValues()) %>" class="medium">
	<option><%:"select a category".Localize() %></option>
	<%foreach (var c in Model) { %>
	<option value="<%:c.FullName %>" ><%:c.Name %> </option>
	<%} %>
</select>


