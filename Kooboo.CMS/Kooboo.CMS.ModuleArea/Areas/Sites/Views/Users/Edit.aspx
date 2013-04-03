<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.User>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% Html.BeginForm(); %>
        <%:Html.ValidationSummary(true) %>
        <%:Html.Hidden("old_Key", Model.UserName)%>
        <input type="hidden" name="UserName" value="<%:Model.UserName%>" />
        <fieldset>            
            <table>
                <tbody>
                    <%:Html.DisplayFor(m=>m.UserName) %>
                    <%:Html.EditorFor(m => m.Roles, new { @class = "medium" })%>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button></p>
        <% Html.EndForm(); %>
    </div>
</asp:Content>
