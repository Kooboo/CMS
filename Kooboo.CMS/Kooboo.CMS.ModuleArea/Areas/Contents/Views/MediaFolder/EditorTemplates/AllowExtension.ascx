<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IEnumerable<string>>" %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<%
    var fullPropertyName = ViewData.TemplateInfo.HtmlFieldPrefix;
    var ulId = fullPropertyName.Replace(".", "_");

    ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.'); %>
<fieldset  class=" flex-list">
    <legend class="clearfix"> <span class="arrow"></span><%="AllowExtension".Localize() %>
    </legend>
    <table class="dynamicGroups">
        <tbody id="binaryFolderAllowExtensionContainer">
            <tr id="binaryFolderAllowExtension">
                <td>
                    <input type="text" name="" />
                    <img alt="Remove" class="dynamicIcon del" title="remove" src="/Areas/Contents/Styles/images/remove.png" />
                </td>
            </tr>
        </tbody>
    </table>
    <img id="addAllowExtensionBtn" class="dynamicIcon"
        title="add" src="/Areas/Contents/Styles/images/add.png" alt="Add" />
</fieldset>
<script src="../../../Scripts/kooboo.cms.content.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
        kooboo.cms.ui.dynamicListInstance({
            containerId: 'binaryFolderAllowExtensionContainer',
            templateId: 'binaryFolderAllowExtension',
            addButtonId: 'addAllowExtensionBtn',
            propertyName:'<%=fullPropertyName %>',
            data:<%=Model.ToJSON() %>
        });
</script>
