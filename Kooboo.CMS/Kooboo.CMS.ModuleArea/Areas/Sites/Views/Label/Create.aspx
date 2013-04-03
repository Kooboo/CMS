<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.Globalization.Element>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%:"Create Label".Localize() %></h2>
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary(true) %>

        <fieldset>
            <legend></legend>
            <table>
                <tbody>
                    <%:Html.EditorFor(m => m.Name, new { @class="medium" })%>
                    <%:Html.EditorFor(m => m.Category, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.Value, new { @class = "medium" })%>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
        <button type="submit"><%:"Save".Localize() %></button>
        </p>
        <% } %>
    </div>
</asp:Content>
