<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
   var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString();
   var guid = Guid.NewGuid();
%>
<tr>
    <th>
        <label for="<%:ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName)%>">
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
        <%             
            if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
            {%>
        <a href="javascript:;" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description %>">
        </a>
        <%} %>
    </th>
    <td>
        <ul id="container-<%:guid %>" class="clearfix key-value">
            <li id="template-<%:guid %>" class="clearfix">
                <%: Html.DropDownListTree(propertyName, 
        ViewData.ModelMetadata.GetDataSource().GetSelectListItems(ViewContext.RequestContext).OfType<Kooboo.Web.Mvc.SelectListItemTree>()
        ,Html.GetUnobtrusiveValidationAttributes(propertyName, ViewData.ModelMetadata).Merge("class", ViewData["class"]))%>
                <%: Html.ValidationMessage(ViewData.ModelMetadata, new { name = ViewData["name"] })%>
                <a href="javascript:;" class="o-icon remove"></a></li>
        </ul>
        <a href="javascript:;" class="o-icon add" id="add-<%:guid %>"></a>
        <script language="javascript" type="text/javascript">
            $(function () {
                $('#template-<%:guid %> select').addClass('long').removeAttr('name');
                kooboo.cms.ui.dynamicListInstance({
                    addButtonId: 'add-<%:guid %>',
                    templateId: 'template-<%:guid %>',
                    containerId: 'container-<%:guid %>',
                    propertyName: '<%:propertyName %>',
                    data: eval('(<%=Model!=null?Model.ToJSON():"[]" %>)'),
                    onAdd: function (item) {
                        var instance = this;
                        item.find('select').blur(function () {
                            setTimeout(function () {
                                if (!isValidItem(instance.getItems(), item)) {
                                    instance.remove(item);
                                }
                            }, 100);
                        });
                    }
                });
                function isValidItem(items, item) {
                    var result = true;
                    items.each(function () {
                        var it = $(this);
                        if (it.get(0) != item.get(0)) {
                            result = result && ($(this).find('select').val() != item.find('select').val());
                        }
                        if (!result) { return result; }
                    });
                    return result;
                }
            });
        </script>
    </td>
</tr>
