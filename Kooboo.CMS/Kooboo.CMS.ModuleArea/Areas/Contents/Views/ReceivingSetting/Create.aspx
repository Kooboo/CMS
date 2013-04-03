<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Content.Models.ReceivingSetting>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary(true) %>
        <table>
            <tr>
                <td style="width:48%">
                    <label><%="Sending folder".Localize()%></label>
                    <%:Html.Partial("RepositoryFolderTree",(new Kooboo.CMS.Web.Areas.Sites.Models.RepositoryDataSource()).GetSelectListItems(ViewContext.RequestContext,"").Select(o=>o.Value)) %>
                </td>
                <td>
                    <label><%="Receving folder".Localize()%></label>
                    <%:Html.Partial("FolderTree", new Kooboo.CMS.Web.Areas.Contents.Models.FolderTreeModel() {
    InputName = "ReceivingFolder",
    AllowNull=false,
    IsSingle=true
})%>
                </td>
            </tr>
        </table>
        <table>
            <tbody>
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
            var repository = $('#SendingRepository').linkageSelect({
                sub: "#SendingFolder",
                url: '<%=Url.Action("GetFolderDataSource",ViewContext.RequestContext.AllRouteValues()) %>',
                dataname: "Repository"
            });
        });
    </script>
</asp:Content>
