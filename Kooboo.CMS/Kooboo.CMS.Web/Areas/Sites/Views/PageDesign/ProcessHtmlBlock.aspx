<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/PageDesign/Design.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<Kooboo.CMS.Sites.Models.HtmlBlock>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function getDesignerType(types) { return types['htmlBlock']; }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="clearfix">
        <div class="table-container">
            <table>
                <%--<caption>
                    <%:"Select a HTML block:".Localize()%></caption>--%>
                <thead>
                    <tr>
                        <th class="checkbox">
                            <%--<input type="checkbox" disabled="disabled" />--%>
                        </th>
                        <th>
                            <%:"Name".Localize()%>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <%foreach (var item in Model)
                      {%>
                    <tr>
                        <td>
                            <input type="checkbox" name="BlockName" value="<%=item.Name%>" />
                        </td>
                        <td>
                            <label for="<%=item.Name%>">
                                <%=item.Name%></label>
                        </td>
                    </tr>
                    <%}%>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
