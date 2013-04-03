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
            var body = $("#Body");

            if (body.val().trim().length == 0) {
                body.val("function functionname(var1,varX){\r\n//some code\r\n}\r\n");
            }

            body.codeMirror({ sizeProvider: false }).data('codeMirror').setOption('mode', 'javascript');

            kooboo.cms.ui.event.ajaxSubmit(function () {
                var codeMirrorAPI = body.data("codeMirror");
                var code = codeMirrorAPI.getCode();
                body.val(code);
            });
        }
    });
</script>
