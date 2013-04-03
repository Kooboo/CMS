<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Content.Models.Repository>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%:"Basic".Localize() %> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%:"Repository Basic Setting".Localize() %></h2>
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary() %>
        <fieldset>
            <legend></legend>
            <table>
                <tbody>
                    <%:Html.HiddenFor(m=>m.Name) %>
                    <%: Html.DisplayFor(m=>m.Name) %>
                    <%:Html.EditorFor(m=>m.DisplayName) %>
                    <%:Html.HiddenFor(m=>m.DBProvider) %>
                    <%: Html.DisplayFor(m=>m.DBProvider) %>
                </tbody>
            </table>
        </fieldset>
        <button type="submit">
            <%:"Save".Localize() %></button>
        <% } %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
