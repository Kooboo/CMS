<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Web.Areas.Contents.Models.ImportModel>" %>
<div id="dialog" class="common-form" style="display: none;">
    <% using (this.Html.BeginForm("Import", ViewContext.RequestContext.AllRouteValues()["controller"].ToString(), ViewContext.RequestContext.AllRouteValues(), FormMethod.Post, new RouteValueDictionary(new { enctype = "multipart/form-data" })))
       { %>
    <fieldset>
        <table>
            <tbody>
        <%:Html.EditorFor(m=>m.File) %>
        <%:Html.EditorFor(m=>m.Override) %>
            </tbody>
        </table>
    </fieldset>
    <p class="buttons"><button type="submit"><%:"Import".Localize() %></button></p>
    <%} %>
</div>
