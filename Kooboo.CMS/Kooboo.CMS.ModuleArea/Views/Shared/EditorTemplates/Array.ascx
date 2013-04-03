<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object[]>" %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
   var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString();
   var guid = Guid.NewGuid();
    %>    
<tr id="tr-<%:guid %>">
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
        <div id="container-<%:guid %>">
        <p id="template-<%:guid %>" class="clearfix"><input type="text" class="<%:ViewData["class"] %>"/><a  class="o-icon remove form-action" title="remove" href="#"></a></p>
        </div>
        <a href="#" id="add-<%:guid %>" class="o-icon add form-action"></a>
    </td>
</tr>
<script language="javascript" type="text/javascript">
    kooboo.cms.ui.dynamicListInstance({
            containerId: 'container-<%:guid %>',
            templateId: 'template-<%:guid %>',
            addButtonId: 'add-<%:guid %>',
            propertyName:'<%=ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName) %>',
            data:<%=Model!=null?Model.ToJSON():"[]" %> 
        });
</script>
