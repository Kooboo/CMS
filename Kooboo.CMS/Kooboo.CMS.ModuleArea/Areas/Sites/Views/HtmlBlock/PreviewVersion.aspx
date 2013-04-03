<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.HtmlBlock>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <%:Html.ValidationSummary(true) %>
        <fieldset>
            <table>
                <tbody>
                    <%:Html.DisplayFor(m => m.Name, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.Body, new { @class = "medium" })%>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <a href="<%=Url.Action("Revert",ViewContext.RequestContext.AllRouteValues()) %>"
                class="ajax-post-link button" confirm="<%:"Are you sure you want to rever to this version?".Localize()%>">
                <%:"Revert".Localize() %></a> <a href="javascript:;" class="dialog-close button">
                    <%:"Close".Localize() %></a>
        </p>
    </div>
</asp:Content>
