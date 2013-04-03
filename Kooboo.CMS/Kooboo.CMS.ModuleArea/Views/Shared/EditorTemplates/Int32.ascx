<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<int?>" %>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.'); 
    var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString();
    var defaultValue = ViewData.ModelMetadata.AdditionalValues.ContainsKey("DefaultValue") ? ViewData.ModelMetadata.AdditionalValues["DefaultValue"] : 0; %>
<tr>
    <th>
        <label for="<%: ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName)%>">
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
        <% if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
           {%>
        <a href="#" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description.Localize() %>">
        </a>
        <%} %>
    </th>
    <td>
        <%: Html.TextBox(propertyName, Model.HasValue ? Model.Value : defaultValue, Html.GetUnobtrusiveValidationAttributes(propertyName, ViewData.ModelMetadata).Merge("class", ViewData["class"]).Merge("value-type", "int32"))%>
        <%: Html.ValidationMessage(ViewData.ModelMetadata, new { name = ViewData["name"] })%>
    </td>
</tr>
