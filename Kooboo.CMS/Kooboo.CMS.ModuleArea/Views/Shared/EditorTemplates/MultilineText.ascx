<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
   var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString(); %>
<tr>
    <th>
        <label for="<%:ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName)%>">
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
    </th>
    <td>
        <%: Html.TextArea(propertyName, Model == null ? "" : Model.ToString(), Html.GetUnobtrusiveValidationAttributes(propertyName,ViewData.ModelMetadata).Merge("class",ViewData["class"]).Merge("rows", "10"))%>
        <% if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
           {%>
        <a href="#" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description.Localize() %>">
        </a>
        <%} %>
        <%: Html.ValidationMessage(ViewData.ModelMetadata, new { name = ViewData["name"] })%>
    </td>
</tr>
