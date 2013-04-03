<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Sites.Models.Navigation>" %>
<fieldset>
    <table>
        <tbody>
            <%:Html.EditorFor(m=>m.Show) %>
            <%:Html.EditorFor(m => m.DisplayText, new { @class = "medium" })%>
            <%:Html.EditorFor(m => m.Order, new { @class = "medium" })%>
            <%:Html.EditorFor(m => m.ShowInCrumb, new { @class = "medium" })%>
        </tbody>
    </table>
</fieldset>
