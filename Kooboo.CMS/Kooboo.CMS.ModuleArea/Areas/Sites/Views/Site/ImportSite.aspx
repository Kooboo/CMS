<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Sites.Models.ImportSiteModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary(true) %>
        <fieldset>
            <table>
                <%:Html.EditorFor(o=>o.Name) %>
                <%:Html.EditorFor(o => o.Parent, new {@class="long" })%>
                <%:Html.EditorFor(o => o.Repository, new {@class="long" })%>
                <%:Html.EditorFor(o => o.File, new {@class="long" })%>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button></p>
        <% } %>
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {
            var canSubmit = true;
            kooboo.cms.ui.event.ajaxSubmit(function () {
                return canSubmit;
            });
            $('input:file').change(function () {
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
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
