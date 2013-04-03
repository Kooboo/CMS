<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<bool?>" %>
<% 
	string prefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
	ViewData.TemplateInfo.HtmlFieldPrefix = prefix;
	var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString();
%>
<tr>
	<th>
		<label class="label-checkbox" for="<%:ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName)%>">
			<%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
		<%
			if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
			{%>
		<a href="javascript:;" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description %>">
		</a>
		<%} %>
	</th>
	<td>
		<%: Html.CheckBox(propertyName, Model.HasValue ? Model.Value : false, Html.GetUnobtrusiveValidationAttributes(propertyName, ViewData.ModelMetadata).Merge("class", ViewData["class"]))%>
	</td>
</tr>
