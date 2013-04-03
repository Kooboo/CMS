<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<% 
    ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
    var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString(); %>
<tr>
    <td colspan="2">
        <label>
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
        <% if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
           {%>
        <a href="javascript:;" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description.Localize() %>">
        </a>
        <%} %>
        <%:Html.Partial("FolderTree", new Kooboo.CMS.Web.Areas.Contents.Models.FolderTreeModel() { InputName = ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName), IsSingle = true, HtmlAttr = Html.GetUnobtrusiveValidationAttributes(propertyName, ViewData.ModelMetadata).Merge("class", ViewData["class"]), Values = Model != null ? new List<string> { Model.ToString() } : new List<string>(), AllowNull = !ViewData.ModelMetadata.IsRequired})%>
        <%: Html.ValidationMessage(ViewData.ModelMetadata, new { name = ViewData["name"] })%>
    </td>
</tr>
