<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.HtmlBlock>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%if (ViewBag.ExternalCssSetting != null) {%>
    <script type="text/javascript">
        (function () {
            var old = tinymce.getKoobooConfig;
            tinymce.getKoobooConfig = function () {
                var ret = old.apply(this, arguments);
                ret.content_css = '<%=ViewBag.ExternalCssSetting%>';
                return ret;
            };
        } ());
    </script>
    <%}%>
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%--sitename for remote validation--%>
        <%:Html.Hidden("SiteName",ViewContext.RequestContext.GetRequestValue("SiteName")) %>
        <%:Html.ValidationSummary(true) %>
        <fieldset>
            <table>
                <tbody>
                    <%:Html.EditorFor(m => m.Name, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.Body, new { @class = "medium" })%>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button></p>
        <% } %>
        <script type="text/javascript">
            $('.common-form textarea#Body').attr('rows', 26);
        </script>
    </div>
</asp:Content>
