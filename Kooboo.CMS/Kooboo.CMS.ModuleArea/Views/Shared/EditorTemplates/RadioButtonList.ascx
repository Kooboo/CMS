<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
   var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString(); %>
<tr>
	<td>
		<label for="<%:ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName) %>">
			<%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
		<%
			if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
			{%>
		<a href="#" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description.Localize() %>">
		</a>
		<%} %>
	</td>
	<td>
		<%: Html.RadioButtonList(propertyName, ViewData.ModelMetadata.GetDataSource().GetSelectListItems(ViewContext.RequestContext).SetActiveItem(Model))%>
		<%: Html.ValidationMessage(ViewData.ModelMetadata,null) %>
	</td>
</tr>
