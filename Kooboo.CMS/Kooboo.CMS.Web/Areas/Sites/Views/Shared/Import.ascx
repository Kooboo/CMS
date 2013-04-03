<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Web.Areas.Sites.Models.ImportModel>" %>
<div id="dialog" class="common-form" style="display: none;">
    <% using (this.Html.BeginForm(Model.Action, ViewContext.RequestContext.AllRouteValues()["controller"].ToString(), ViewContext.RequestContext.AllRouteValues(), FormMethod.Post, new RouteValueDictionary(new { enctype = "multipart/form-data" })))
       { %>
    <fieldset>
        <table>
            <tbody>
                <%:Html.EditorFor(m=>m.File) %>
                <%:Html.EditorFor(m=>m.Override) %>
            </tbody>
        </table>
    </fieldset>
    <p class="buttons">
        <button type="submit">
            <%:"Save".Localize() %></button></p>
    <%} %>
</div>
<script language="javascript" type="text/javascript">
    $(function () {
        var canSubmit = true;
        kooboo.cms.ui.event.ajaxSubmit(function () {
            return canSubmit;
        });
        $('#dialog input:file').change(function () {
            var handle = $(this);
            var span = handle.next().addClass('field-validation-error')
            var id = handle.attr('id');
            var exp = /.zip$/gi;
            if (!exp.test(handle.val())) {
                span.html('<%:"Please upload zip file".Localize() %>').show();
                handle.val('');
                canSubmit = false;
            } else {
                span.html('').hide();
                canSubmit = true;
            }
        });




        //        $('a[name="import"]').click(function () {
        //            var dialog = $("#dialog");
        //            dialog.dialog({
        //                title: $("#dialog .title").attr("title"),
        //                width: 420,
        //                height: 200,
        //                resizable: false,
        //                modal: true
        //            });
        //            return false;
        //        });

    });
</script>
