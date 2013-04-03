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
                <%:Html.EditorFor(m => m.FolderName, new { @class="long" })%>
                <%:Html.EditorFor(m=>m.SendReceived) %>
                <%:Html.EditorFor(m => m.SendToChildSites)%>
                <%:Html.EditorFor(m => m.ChildLevel)%>
                <%:Html.EditorFor(m => m.KeepStatus)%>
            </tbody>
        </table>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button>
        </p>
        <% } %>
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {
            var keepStatusTr = $('#KeepStatus').parent().parent();
            var childLevel = $('[name=ChildLevel]').parent().parent().parent().parent();
            keepStatusTr.hide();
            childLevel.hide();
            $('#SendToChildSites').change(function () {
                var sendToChildSites = $(this);

                if (sendToChildSites.is(':checked')) {
                    keepStatusTr.show();
                    childLevel.show();
                }
                else {
                    keepStatusTr.hide();
                    childLevel.hide();
                }
            });
        });
    </script>
</asp:Content>
