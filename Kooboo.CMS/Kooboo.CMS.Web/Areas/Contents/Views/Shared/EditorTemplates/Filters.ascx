<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<Kooboo.CMS.Content.Models.FieldValue>>" %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<%var guid = Guid.NewGuid(); %>
<tr class="flex-list">
    <th>
        <label>
            <span class="arrow active"></span>
            <%="Filters".Localize() %></label>
    </th>
    <td>
        <ul class="key-value" id="container-<%:guid %>">
            <li class="title">
                <label>
                    <%="FieldName".Localize() %></label><label>
                        <%="Value".Localize() %></label></li>
            <li id="template-<%:guid %>">
                <input type="text" name="FieldName" class="textbox" />
                <input type="text" name="Value" class="textbox" /><a class=""></a><a href="javascript:;"
                    class="o-icon remove dynamicIcon">remove</a></li>
        </ul>
        <a href="javascript:;" class="o-icon add" id="add-<%:guid %>"></a>
    </td>
</tr>
<script language="javascript" type="text/javascript">
    $(function () {
        var instance = kooboo.cms.ui.dynamicListInstance({
            containerId: 'container-<%:guid %>',
            templateId: 'template-<%:guid %>',
            addButtonId: 'add-<%:guid %>',
            propertyName: '<%:ViewData.TemplateInfo.HtmlFieldPrefix%>',
            data: eval('(<%=Model!=null?Model.ToJSON():"[{}]" %>)'),
            onAdd: function () {
                this.getItems().show();
                if (!this.getItems().is(':visiable')) {
                    $('.flex-list th label').click();
                }

            }
        });
        $('.flex-list th label').toggle(function () {
            $(this).find('.arrow').removeClass('active');
            instance.getItems().hide();
        }, function () {
            $(this).find('.arrow').addClass('active');
            instance.getItems().show();
        }).css('cursor', 'pointer');
    });

</script>
