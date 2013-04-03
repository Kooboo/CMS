<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Sites.Models.Smtp>" %>
<fieldset>
    <table>
        <tbody>
            <%:Html.EditorFor(m => m.EnableSsl)%>
            <%:Html.EditorFor(m => m.Port, new { @class = "mini" })%>
            <%:Html.EditorFor(m => m.Host, new { @class = "medium" })%>
            <%:Html.EditorFor(m => m.UserName, new { @class = "medium",autocomplete = "off" })%>
            <%:Html.EditorFor(m => m.Password, new { @class = "medium", autocomplete = "off" })%>
            <%:Html.EditorFor(m => m.From, new { @class = "medium" })%>
            <%:Html.EditorFor(m => m.To, new { @class = "medium" })%>
        </tbody>
    </table>
</fieldset>
