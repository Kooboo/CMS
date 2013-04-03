<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Web.Models.CopyFormModel>" %>
<div class="common-form" id="copy-form" style="display: none">
    <%using (Html.BeginForm())
      {%>
    <%:Html.Hidden("Site",Kooboo.CMS.Sites.Models.Site.Current) %>
    <fieldset>
        <table>
            <tbody>
                <%:Html.EditorFor(o=>o.DestName) %>
            </tbody>
        </table>
    </fieldset>
    <p class="buttons">
        <button type="submit">
            <%:"Save".Localize()  %></button></p>
    <% } %>
</div>
