<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Account/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Account.Models.CreateUserModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Users
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <input type="hidden" value="<%:Model.UserName %>" name="UserName" />
        <fieldset>
            <table>
                <tbody>
                    <%:Html.DisplayFor(m => m.UserName) %>
                    <%:Html.EditorFor(m => m.Email) %>
                    <%:Html.EditorFor(m => m.IsAdministrator)%>
                    <%:Html.EditorFor(m=>m.UICulture) %>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button>
        </p>
        <%} %>
    </div>
</asp:Content>
