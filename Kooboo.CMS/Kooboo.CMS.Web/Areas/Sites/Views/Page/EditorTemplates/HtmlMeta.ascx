<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Sites.Models.HtmlMeta>" %>
<fieldset>
    <table>
        <tbody>
            <%:Html.EditorFor(m=>m.HtmlTitle, new { @class = "medium" })%>
            <%:Html.EditorFor(m=>m.Canonical, new { @class = "medium" })%>
            <%:Html.EditorFor(m => m.Author, new { @class = "medium" })%>
            <%:Html.EditorFor(m => m.Keywords, new { @class = "medium" })%>
            <%:Html.EditorFor(m => m.Description, new { @class = "medium" })%>
            <%:Html.EditorFor(m=>m.Customs) %>
        </tbody>
    </table>
</fieldset>
