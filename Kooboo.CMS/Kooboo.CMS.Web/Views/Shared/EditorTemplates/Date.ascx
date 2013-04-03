<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<% 
    ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
    var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString(); %>
<tr>
    <th>
        <label for="<%:ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName) %>">
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
        <% if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
           {%>
        <a href="#" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description.Localize() %>">
        </a>
        <%} %>
    </th>
    <td>
        <%: Html.TextBox(propertyName, Model is DateTime ? ((DateTime)Model).ToLocalTime().ToShortDateString() :Model, Html.GetUnobtrusiveValidationAttributes(propertyName,ViewData.ModelMetadata).Merge("class",ViewData["class"]))%>
        <%: Html.ValidationMessage(ViewData.ModelMetadata, new { name = ViewData["name"] })%>
        <script language="javascript" type="text/javascript">
            $(function () { { $("#<%:ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName)%>").datepicker(); } });
        </script>
    </td>
</tr>
