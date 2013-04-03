<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.UrlRedirect>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%--sitename for remote validation--%>
        <%:Html.Hidden("SiteName",ViewContext.RequestContext.GetRequestValue("SiteName")) %>
        <%:Html.ValidationSummary(true) %>
        <fieldset>            
            <table>
                <tbody>
                    <%:Html.EditorFor(m => m.Regex)%>
                    <%:Html.EditorFor(m => m.InputUrl, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.OutputUrl, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.RedirectType, new { @class = "medium" })%>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button></p>
        <% } %>
    </div>
</asp:Content>
