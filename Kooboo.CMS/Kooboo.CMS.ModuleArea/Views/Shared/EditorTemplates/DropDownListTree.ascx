<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
   var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString(); %>
<tr>
	<th>
		<label for="<%: ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName)%>">
			<%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
		<% if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
	 {%>
		<a href="#" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description.Localize() %>">
		</a>
		<%} %>
		<%if (ViewData.ModelMetadata.IsRequired)
	{%><span class="required">*</span><% } %>
	</th>
	<td>
		<%: Html.DropDownListTree(propertyName,
                    ViewData.ModelMetadata.GetDataSource().GetSelectListItems(ViewContext.RequestContext).OfType<Kooboo.Web.Mvc.SelectListItemTree>().SetActiveItem(Model)
        ,Html.GetUnobtrusiveValidationAttributes(propertyName, ViewData.ModelMetadata).Merge("class", ViewData["class"]))%>
		<%: Html.ValidationMessage(ViewData.ModelMetadata, new { name = ViewData["name"] })%>
	</td>
</tr>
