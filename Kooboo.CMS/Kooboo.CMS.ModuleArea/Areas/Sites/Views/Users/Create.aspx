<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.User>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary(true) %>
        <%--sitename for remote validation--%>
        <fieldset>            
            <table>
                <tbody>
                    <%:Html.EditorFor(m => m.UserName, new { @class="medium" })%>
                    <%:Html.EditorFor(m => m.Roles, new { @class = "medium" })%>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button>
        </p>
        <% } %>
    </div>
</asp:Content>
