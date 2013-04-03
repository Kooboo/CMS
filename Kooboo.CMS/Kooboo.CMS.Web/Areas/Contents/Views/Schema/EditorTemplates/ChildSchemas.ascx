<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<%
    var fullPropertyName = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(".", "_");
    ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
    var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString(); %>
<tr>
    <th>
        <label for="<%: ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName)%>">
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
        <%             
            if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
            {%>
        <a href="javascript:;" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description %>">
        </a>
        <%} %>
    </th>
    <td>
        <div class="clearfix" id="dynamic-container">
        </div>
        <p id="dynamic-template" class="clearfix">
            <%: Html.DropDownList(propertyName, ViewData.ModelMetadata.GetDataSource()
        .GetSelectListItems(ViewContext.RequestContext),
        Html.GetUnobtrusiveValidationAttributes(propertyName, ViewData.ModelMetadata).Merge("class", ViewData["class"]).Merge("Name",null))%>
            <a class="o-icon remove" href="javascript:;">remove</a>
        </p>
        <%: Html.ValidationMessage(ViewData.ModelMetadata, new { name = ViewData["name"] })%>
        <a href="javascript:;" id="dynamic-add" class="o-icon add">Add</a>
    </td>
</tr>
<script type="text/javascript">
    kooboo.cms.ui.dynamicListInstance({
            containerId: 'dynamic-container',
            templateId: 'dynamic-template',
            addButtonId: 'dynamic-add',
            propertyName:'<%:fullPropertyName %>',
            data:<%=Model!=null?Model.ToJSON():"[]" %>
        });
</script>
