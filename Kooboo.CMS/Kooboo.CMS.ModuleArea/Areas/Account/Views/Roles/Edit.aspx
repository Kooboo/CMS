<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Account/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Account.Models.CreateRoleModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <%:Html.ValidationSummary(true) %>
        <%using (Html.BeginForm())
          { %>
        <fieldset>
            <table>
                <%:Html.DisplayFor(m=>m.Name) %>
            </table>
        </fieldset>
        <input type="hidden" value="<%:Model.Name %>" name="Name"/>
        <%:Html.Partial("Permissions") %>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button>
        </p>
        <%} %>
    </div>
</asp:Content>
