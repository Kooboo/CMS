<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Sites.Models.Security>" %>
<fieldset>
    <table>
        <tbody>
            <%:Html.EditorFor(m => m.TurnOnSubmissionAPI)%>
            <%:Html.EditorFor(m => m.EncryptKey, new { @class = "medium" })%>
        </tbody>
    </table>
</fieldset>
