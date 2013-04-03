<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Content.Models.Repository>" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%:"Settings".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%:"Settings".Localize()%></h3>
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary(true) %>
        <fieldset>
            <legend></legend>
            <table>
                <tbody>
                    <%:Html.HiddenFor(m=>m.Name) %>
                    <%: Html.DisplayFor(m => m.Name)%>
                    <%:Html.EditorFor(m => m.DisplayName, new { @class = "medium" })%>
                    <%--<%:Html.HiddenFor(m=>m.DBProvider) %>
                    <%: Html.DisplayFor(m=>m.DBProvider) %>--%>
                    <%:Html.EditorFor(m => m.UserKeyReplacePattern, new { @class="medium" })%>
                    <%:Html.EditorFor(m => m.UserKeyHyphens, new { @class = "medium" })%>
                    <%:Html.EditorFor(m=>m.EnableVersioning) %>
                    <%:Html.EditorFor(m=>m.EnableWorkflow) %>
                    <%:Html.EditorFor(m=>m.EnableBroadcasting) %>
                    <%:Html.EditorFor(m=>m.EnableCustomTemplate) %>
                    <%:Html.EditorFor(m=>m.StrictContentPermission) %>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button>
            <%:Html.ActionLink("Export".Localize(), "Export", "Repository", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary().Merge("class","button"))%>
            <%if (string.IsNullOrWhiteSpace(ViewContext.RequestContext.GetRequestValue("SiteName")))
              {%>
            <%:Html.ActionLink("Delete this repository".Localize(), "Delete","Repository", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary().Merge("id", "deleteRepository").Merge("class","button"))%>
            <%} %>
        </p>
        <% } %>
    </div>
    <script type="text/javascript" language="javascript">
        window.onSuccess = function () {
            //prevent page reload after ajax post success..
        }
        $(function () {
            $("#deleteRepository").click(function () {
                return confirm('<%:"Are you sure you want to delete this repository?".Localize()%>');
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
