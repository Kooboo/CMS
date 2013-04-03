<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.Globalization.Element>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Translate Label</h2>
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary(true) %>
        <fieldset>
            <legend></legend>
            <table>
                <tbody>
                    <%:Html.DisplayFor(m => m.Name)%>
                    <%:Html.HiddenFor(m => m.Name)%>
                    <%:Html.DisplayFor(m=>m.Category) %>
                    <%:Html.HiddenFor(m => m.Category)%>
                    <%:Html.EditorFor(m=>m.Value) %>
                    <%:Html.EditorFor(m=>m.Culture) %>
                </tbody>
            </table>
        </fieldset>
        <button type="submit">Save</button>
        
        <% } %>
    </div>
</asp:Content>
