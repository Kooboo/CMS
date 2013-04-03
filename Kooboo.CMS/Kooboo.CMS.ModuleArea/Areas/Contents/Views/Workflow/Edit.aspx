<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master" Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Content.Models.Workflow>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary(true)%>
        <fieldset>
            <table>
                <tbody>
                    <%:Html.DisplayFor(o=>o.Name) %>
                    <%:Html.HiddenFor(o=>o.Name) %>
                    <%:Html.EditorFor(o=>o.Items) %>
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

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
