<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master" Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Content.Models.MediaFolder>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
           <%:Html.Hidden("success",Request.HttpMethod.ToLower() == "post" && ViewData.ModelState.IsValid) %>
        <%:Html.ValidationSummary(true) %>
        <fieldset>
            <legend><%="Basic Info".Localize() %></legend>
            <table>
                <tbody>
                    <%:Html.EditorFor(m => m.Name, new {@class="width200" })%>
                    <%:Html.EditorFor(m => m.DisplayName, new { @class = "width200" })%>
                </tbody>
            </table>
        </fieldset>
  
        <%:Html.EditorFor(m=>m.AllowedExtensions) %>
        <p class="buttons">
        <button type="submit"><%:"Save".Localize() %></button>
        </p>
        
        <% } %>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
