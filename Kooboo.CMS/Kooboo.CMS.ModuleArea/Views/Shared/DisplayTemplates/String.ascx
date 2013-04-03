<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>


<tr>
	<th>
		<label for="<%: ViewData.ModelMetadata.PropertyName%>">
			<%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
		<% if(!string.IsNullOrEmpty(ViewData.ModelMetadata.Description)) {%>
		<a href="#" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description %>">
		</a>
		<%} %>
	</th>
	<td>
		<%: ViewData.ModelMetadata.Model %>
	</td>
</tr>
