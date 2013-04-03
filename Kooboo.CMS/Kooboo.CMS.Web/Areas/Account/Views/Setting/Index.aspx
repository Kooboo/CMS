<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Account/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Account.Models.Setting>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Settings".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%: "Settings".Localize()%></h3>
    <div class="common-form">
        <% Html.BeginForm(); %>
        <fieldset>
            <table>
                <tbody>
                    <%:Html.EditorFor(m => m.PasswordStrength, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.PasswordInvalidMessage, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.EnableLockout, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.FailedPasswordAttemptCount, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.MinutesToUnlock, new { @class = "medium" })%>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize()%></button>
        </p>
        <% Html.EndForm(); %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
