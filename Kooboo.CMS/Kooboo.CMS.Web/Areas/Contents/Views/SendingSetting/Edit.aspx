<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Content.Models.SendingSetting>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:"Index".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <table>
            <tbody>
                <%:Html.HiddenFor(m=>m.Name) %>
                <%:Html.DisplayFor(m => m.FolderName)%>                
                <%--<%:Html.EditorFor(m => m.AcceptAction, new { @class = "" })%>
                <%:Html.EditorFor(m => m.Published, new { @class = "" })%>--%>
            </tbody>
        </table>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button>
        </p>
        <% } %>
    </div>
     
</asp:Content>
