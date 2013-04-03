<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Sites.Models.PagePermission>" %>
<fieldset>
    <legend>PagePermission</legend>
    <table>
        <tbody>
            <%:Html.EditorFor(m => m.Allowed, new { @class = "medium" })%>
            <%:Html.EditorFor(m => m.Denied, new { @class = "medium" })%>
        </tbody>
    </table>
</fieldset>
