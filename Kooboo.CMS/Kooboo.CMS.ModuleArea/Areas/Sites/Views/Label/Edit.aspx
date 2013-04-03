<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.Globalization.Element>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%:"Edit label".Localize() %></h2>
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary(true) %>

        <fieldset>
            <legend></legend>
            <table>
                <tbody>
                    <%:Html.DisplayFor(m => m.Name, new { @class = "medium" })%>
                    <%:Html.HiddenFor(m => m.Name, new { @class = "medium" })%>
                    <%:Html.DisplayFor(m => m.Category, new { @class = "medium" })%>
                    <%:Html.HiddenFor(m => m.Category, new { @class = "medium" })%>
                    <%:Html.DisplayFor(m => m.Culture, new { @class = "medium" })%>
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
