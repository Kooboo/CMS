<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Sites.Models.CreateSiteModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    CreateSubSite
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary(true) %>
        <fieldset>
            <table>
                <tbody>
                    <%:Html.EditorFor(m => m.Parent, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.Name, new { @class="medium" })%>
                    <%:Html.EditorFor(m => m.Repository, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.DisplayName, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.Culture, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.Theme, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.Domains, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.SitePath, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.Version, new { @class = "medium" })%>
                    <%:Html.EditorFor(m=>m.Mode) %>
                    <%Model.Template = "Pass the validation on the server";%>
                    <%:Html.HiddenFor(m => m.Template) %>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button></p>
        <%}%>
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {
            if ($.request.queryString['siteName']) {
                $("#Parent").val($.request.queryString['siteName']);
            }
            $("#Parent").change(function () {
                $.request.queryString['siteName'] = $(this).val();
                document.location.href = $.request.getUrl();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
