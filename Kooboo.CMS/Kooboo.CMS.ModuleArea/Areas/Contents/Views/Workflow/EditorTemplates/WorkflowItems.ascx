<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<%
    
    ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
    var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString();
    var guid = Guid.NewGuid();
%>
<tr id="tr-<%:guid %>">
    <th>
        <label for="<%:ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName) %>">
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
        <% if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
           {%>
        <a href="javascript:;" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description %>">
        </a>
        <%} %>
    </th>
    <td class="table-container">
        <table style="width: auto">
            <thead>
                <tr>
                    <th class="common">
                        <%:"Step name".Localize() %>
                    </th>
                    <th class="common">
                        <%:"Role name".Localize() %>
                    </th>
                    <th>
                    </th>
                </tr>
            </thead>
            <tbody id="container-<%:guid %>">
                <tr id="template-<%:guid %>">
                    <td>
                        <input type="text" name="DisplayName" class="short valid" data-val="true" data-val-required="Required."
                            tid="DisplayName" />
                    </td>
                    <td>
                        <%:Html.DropDownList("RoleName", Kooboo.CMS.Web.Areas.Account.Models.RoleDataSource.DataSource().GetSelectListItems(ViewContext.RequestContext), new {@class="short rolename" })%>
                    </td>
                    <td class="action">
                        <a class="o-icon remove form-action" title="<%:"remove".Localize() %>" href="javascript:;">
                        </a>
                    </td>
                </tr>
            </tbody>
        </table>
        <br />
        <a href="javascript:;" id="add-<%:guid %>" class="o-icon add form-action">add</a>
    </td>
</tr>
<script language="javascript" type="text/javascript">
    $(function () {

        var selectListItems = [],
         selectClone = $('#RoleName').clone().children().each(function () {
             var handle = $(this);
             selectListItems.push({
                 value: handle.val(),
                 text: handle.text()
             });
         });
        var valueList = [];
        kooboo.cms.ui.dynamicListInstance({
            containerId: 'container-<%:guid %>',
            templateId: 'template-<%:guid %>',
            addButtonId: 'add-<%:guid %>',
            propertyName: '<%=ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName) %>',
            data: eval('(<%=Model!=null?Kooboo.Web.Script.Serialization.JsonHelper.ToJSON(Model):"[{}]" %>)'),
            parentSelector: 'tr:eq(0)',
            beforeAdd: function (item) {
                valueList = getValueList(this);
            },
            onAdd: function (item) {
                var select = item.find('select.rolename');
                if (this.getItems().length - 1 == selectClone.length) {
                    this.remove(item);
                    return false;
                }
                var children = selectClone.filter(getSelector(valueList)).show();
                select.val(children.first().val());
                handleSelect(this);
            },
            onInit: function () {
                handleSelect(this);
            },
            onRemove: function () {
                handleSelect(this);
            }
        });

        function getValueList(instance) {
            var list = [];
            instance.getItems().each(function () {
                var handle = $(this),
                    select = handle.find('select.rolename');
                list.push(select.val());
            });
            return list;
        }

        function getSelector(list) {
            var selector = 'option';
            list.each(function (val, index) {
                selector += '[value!="' + val + '"]'
            });
            return selector;
        }

        function handleSelect(instance) {
            var items = instance.getItems();
            items.each(function () {
                var item = $(this),
                select = item.find('select.rolename'),
                list = getValueList(instance),
                val = select.val(),
                clone = selectClone.clone();
                var selector = getSelector(list.removeElement(select.val()));
                select.children().remove();
                clone.appendTo(select).filter(':not(' + selector + ')').remove();
                clone.filter('[value="' + val + '"]').attr('selected', 'selected');
            });
        }
    });
    

</script>
