<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Contents.Models.CreateRepositoryModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:"Create".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.Hidden("url", ViewData["url"]) %>
        <%:Html.ValidationSummary(true) %>
        <fieldset>
            <table>
                <tbody>
                    <%:Html.EditorFor(m => m.Name)%>
                    <%: Html.EditorFor(m=>m.Template) %>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button></p>
        <% } %>
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {
            $("input:text:first").focus();
            $("input:text").width(150);
            //            var url = $("#url").val();
            //            if (url.length && url.length > 0) {
            //                top.setTimeout(function () {
            //                    for (var i = 0; i < 3; i++) {
            //                        this.parent.document.location.href = url;
            //                    }
            //                }, 10);
            //            }
        });
    </script>
</asp:Content>
