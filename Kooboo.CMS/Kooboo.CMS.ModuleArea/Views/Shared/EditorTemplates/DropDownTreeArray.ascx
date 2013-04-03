<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<%
    var fullPropName = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(".", "_");
    ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
    var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString();
    var guid = Guid.NewGuid();
%>
<tr>
    <th>
        <label for="<%: ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName)%>">
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
        <%             
            if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
            {%>
        <a href="#" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description %>">
        </a>
        <%} %>
    </th>
    <td>
        <div id="container-<%:guid %>">
            <p id="template-<%:guid %>" class="clearfix">
                <%: Html.DropDownListTree(propertyName, ViewData.ModelMetadata.GetDataSource()
                            .GetSelectListItems(ViewContext.RequestContext).OfType<Kooboo.Web.Mvc.SelectListItemTree>())%>
                <a class="o-icon remove form-action" title="remove" href="#"></a>
            </p>
        </div>
        <a href="#" id="add-<%:guid %>" class="o-icon add form-action"></a>
        <%: Html.ValidationMessage(ViewData.ModelMetadata, new { name = ViewData["name"] })%>
    </td>
</tr>
<script language="javascript" type="text/javascript">
    $('#template-<%:guid %>').find('select').removeAttr('name').addClass('<%:ViewData["class"] %>');
    kooboo.cms.ui.dynamicListInstance({
            containerId: 'container-<%:guid %>',
            templateId: 'template-<%:guid %>',
            addButtonId: 'add-<%:guid %>',
            propertyName:'<%=ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName) %>',
            data:<%=Model!=null?Model.ToJSON():"[]" %>,
            onAdd:function(item){
                
                var instance = this,ds = [] , arr = [];
                item.find('select option').each(function(){
                    ds.push($(this).val());
                });

                var itemSelect = item.find('select').blur(function(){
                    var val = $(this).val();
                    arr = [] ;
                    instance.getItems().each(function(){
                        var handle = $(this);
                        arr.push( handle.find('select').val() );
                    });
                    if( arr.where(function(o){ return o == val } ).length > 1){
                        instance.remove(item);
                    };
                }).addClass('select-data-field');

                instance.getItems().each(function(){
                        var handle = $(this);
                        arr.push( handle.find('select').val() );
                    });

                var canSelected = ds.where(function(o){ return arr.indexOf(o) < 0; }).first();

                itemSelect.val(canSelected);

                if(instance.getItems().length > item.find('select option').length){
                    instance.remove(item);
                }
            },
            onInit:function(){
                this.getItems().find('select').addClass('select-data-field');
            }
        });
</script>
