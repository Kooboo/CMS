<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.'); %>
<tr>
    <td>
        <%: Html.TextArea(ViewData.ModelMetadata.PropertyName, Model == null ? "" : Model.ToString(), new { rows = 20 ,cols=20 })%>
        <%: Html.ValidationMessage(ViewData.ModelMetadata,null) %>
    </td>
</tr>
<script language="javascript" type="text/javascript">

    kooboo.namespace("adminJs.scripts");
    adminJs.scripts.extend({
        ready: function () {
            var body = $("#<%: ViewData.TemplateInfo.GetFullHtmlFieldId(ViewData.ModelMetadata.PropertyName) %>");
            body.codeMirror({ sizeProvider: false }).data('codeMirror');

        }
    });
</script>
