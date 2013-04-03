<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Content.Models.OrderSetting>" %>
<%:Html.EditorFor(o=>o.FieldName)%>
<%:Html.EditorFor(o => o.Direction)%>
<script language="javascript" type="text/javascript">
    $(function () {
        var schemaName = $('#SchemaName');
        schemaName.linkageSelect({
            sub: '#OrderSetting_FieldName',
            url: '<%=Url.Action("GetSchemaFields",ViewContext.RequestContext.AllRouteValues()) %>',
            dataname: 'name',
            subval: '<%:Model!=null?Model.FieldName : "Sequence" %>'
        });

        var trigger = function () {
            if (!schemaName.val()) {
                $('fieldset.orderSetting').hide();
            } else {
                $('fieldset.orderSetting').show();
            }
        }

        trigger();

        schemaName.change(trigger);
    });
</script>
