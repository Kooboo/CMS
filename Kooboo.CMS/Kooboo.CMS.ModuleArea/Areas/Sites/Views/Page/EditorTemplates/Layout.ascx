<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
   var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString(); %>
<tr>
	<th>
		<label for="<%:propertyName%>">
			<%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
		<%             
			if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
			{%>
		<a href="javascript:;" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description %>">
		</a>
		<%} %>
	</th>
	<td>
    <input type="hidden" name="<%:propertyName %>" value="<%:Model %>" id="hidden-<%:propertyName %>"/>
		<%: Html.DropDownList(propertyName, ViewData.ModelMetadata.GetDataSource()
                    .GetSelectListItems(ViewContext.RequestContext, "").SetActiveItem(Model), 
                    Html.GetUnobtrusiveValidationAttributes(propertyName, ViewData.ModelMetadata).Merge("class", ViewData["class"]).Merge("disabled","disabled"))%>
		<a href="javascript:;" class="o-icon edit " id="layout-edit">Edit</a> <a href="<%=Url.Action("ChangeLayout",ViewContext.RequestContext.AllRouteValues()) %>" class="o-icon save"
			id="layout-save">Save</a> <a href="javascript:;" class="o-icon cancel" id="layout-cancel">Cancel</a>
		<%: Html.ValidationMessage(ViewData.ModelMetadata, new { name = ViewData["name"] })%>
	</td>
</tr>
<script language="javascript" type="text/javascript">

    $(function () {
        var action = '<%:ViewContext.RequestContext.GetRequestValue("action").ToLower() %>';
        $('#hidden-<%:propertyName %>').removeAttr('disabled');
        var layoutEdit = $('#layout-edit').click(function () {
            layoutEdit.hide();
            $('#Layout').removeAttr('disabled');
            $('#layout-save,#layout-cancel').show();
            $('#hidden-<%:propertyName %>').attr('disabled', 'disabled');
        });
        var layoutSave = $('#layout-save').click(function () {
            var handle = $(this);
            kooboo.confirm('<%:"Are you sure you want to change layout".Localize() %>', function (r) {
                if (r) {
                    //$(layoutSave).parents('form').submit();

                    $.post(handle.attr('href'),
                    { layout: $('#Layout').val() },
                    function (response) {
                        if (response.Success) {
                            $('#page-tab').data('koobooTab').showTab(0);
                            $('#pageDesign').get(0).src = $('#pageDesign').get(0).src;

                            $('#hidden-Layout').val($('#Layout').val());

                            $('#layout-cancel').click();
                            kooboo.cms.ui.loading().hide();
                        } else {
                            kooboo.cms.ui.messageBox().showReponse(response);
                        }
                    });

                    kooboo.cms.ui.loading().show();
                }
            });

            return false;
        }).hide();
        $('#layout-cancel').click(function () {
            
            $('#hidden-<%:propertyName %>').removeAttr('disabled');
            $('#Layout').attr('disabled', 'disabled').val($('#hidden-<%:propertyName %>').val());
            $('#layout-save,#layout-cancel').hide();
            $('#layout-edit').show();
        }).hide();

        $('#Layout').attr('disabled', 'disabled');
        if (action == 'create') {
            layoutEdit.hide();
        }
    });
</script>