<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Sites.Models.CopyPageModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    CopyPage
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% 


            using (Html.BeginForm())
            { %>
        <%:Html.Hidden("SiteName", Request["SiteName"])%>
        <fieldset>
            <table id="positionFormTable">
                <%:Html.DisplayFor(m=>m.SourcePage.FriendlyName) %>
                <%:Html.EditorFor(m => m.ParentPage, new { @class = "long" })%>
                <%:Html.EditorFor(m => m.CreateRedirect, new { @class = "long" })%>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize()%></button>
        </p>
        <% } %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
