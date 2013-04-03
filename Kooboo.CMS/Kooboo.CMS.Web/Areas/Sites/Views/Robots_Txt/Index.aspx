<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.FileResource>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Robots.txt".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%: "Robots.txt".Localize() %><a href="#" id="insertSampleCode">
            <%:"Sample robots.txt".Localize() %></a></h3>
    <div class="common-form">
        <form method="post" action="">
        <fieldset>
            <legend></legend>
            <table>
                <tbody>
                    <tr>
                        <td>
                            <textarea id="body" class="max" name="body" rows="1" cols="1"><%: Model.Body %></textarea>
                        </td>
                    </tr>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button>
        </p>
        </form>
    </div>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#insertSampleCode").click(function () {
                $("#body").val('#sample robots file \r\nUser-agent: *\r\nDisallow: /cgi-bin/\r\nDisallow: /private/');
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
