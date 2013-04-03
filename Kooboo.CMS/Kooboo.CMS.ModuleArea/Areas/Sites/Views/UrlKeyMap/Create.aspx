<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.UrlKeyMap>" %>

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
                    <%:Html.EditorFor(m => m.Key, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.PageFullName, new { @class = "medium" })%>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button></p>
        <% } %>
    </div>
</asp:Content>
