<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
   var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString();
   var guid = Guid.NewGuid();
%>
<tr>
    <th>
        <label for="upload-new-site">
            <%:"Upload new files".Localize() %>
        </label>
                <a href="#" class="tooltip-link" title='<%: "Import from uploaded new template files or existing site templates(under Cms_Data\\ImportedSites).".Localize()%>'>
        </a>
    </th>
    <td>
        <input type="checkbox" checked="checked" value="true" id="upload-new-site" name="UploadNew" />
        <input type="hidden" value="false" name="UploadNew" />
    </td>
</tr>
<tr id="tr-upload-<%:guid %>">
    <th>
        <label for="<%: propertyName%>">
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
    </th>
    <td>
        <input type="file" name="<%: propertyName%>" />
        <% if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
           {%>
        <a href="#" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description %>">
        </a>
        <%} %>
        <%: Html.ValidationMessage(ViewData.ModelMetadata, new { name = ViewData["name"] })%>
    </td>
</tr>
<tr id="tr-select-<%:guid %>">
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
        <%: Html.DropDownList(propertyName, ViewData.ModelMetadata.GetDataSource()
                    .GetSelectListItems(ViewContext.RequestContext, "").SetActiveItem(Model), 
                    Html.GetUnobtrusiveValidationAttributes(propertyName, ViewData.ModelMetadata).Merge("class", ViewData["class"]))%>
        <%: Html.ValidationMessage(ViewData.ModelMetadata, new { name = ViewData["name"] })%>
    </td>
</tr>
<script language="javascript" type="text/javascript">
    $(function () {
        function toggle(r) {
            var trUpload = $('#tr-upload-<%:guid %>');
            var trSelect = $('#tr-select-<%:guid %>');
            if (r) {
                trUpload.show().find('input,select').removeAttr('disabled');
                trSelect.hide().find('input,select').attr('disabled', 'disabled');
            } else {
                trSelect.show().find('input,select').removeAttr('disabled');
                trUpload.hide().find('input,select').attr('disabled', 'disabled');
            }
        }

        var uploadNew = $('#upload-new-site').change(function () {
            var handler = $(this);
            toggle(handler.attr('checked'));
        });

        toggle(uploadNew.attr('checked'));
    });
</script>
