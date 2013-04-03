<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/PageDesign/Design.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Services.Namespace<Kooboo.CMS.Sites.Models.View>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function getDesignerType(types) { return types['view']; }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="view-form">
        <table>
            <tr>
                <td class="view-tree">
                    <h6>
                        <%:"Views:".Localize()%></h6>
                    <ul>
                        <%:Html.Partial("ViewNamespace", Model)%>
                    </ul>
                    <%--<h5 style="margin-left: 5px;">
                        Current view: <span class="current-view"></span>
                    </h5>--%>
                </td>
                <td class="view-parameter">
                    <h6>
                        <%:"Parameters:".Localize()%></h6>
                    <div id="parameter" style="height: 300px;">
                    </div>
                    <h6>
                        <%:"Cache Settings:".Localize()%></h6>
                    <div style="height: 24px;">
                        <label for="enablecache">
                            <%:"Enable:".Localize()%>
                        </label>
                        <input type="checkbox" id="enablecache" /><span id="cache_con" style="margin-left: 10px;"><select
                            id="ExpirationPolicy" name="OutputCache.ExpirationPolicy" style="width: 130px;">
                            <%foreach (var item in (string[])ViewData["Policys"])
                              {%>
                            <option>
                                <%=item%></option>
                            <%}%>
                        </select>
                            <input type="text" name="OutputCache.Duration" style="width: 40px;" />
                            <%:"Second".Localize()%></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
