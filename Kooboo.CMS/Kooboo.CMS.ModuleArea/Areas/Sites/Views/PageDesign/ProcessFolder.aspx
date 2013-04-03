<%@ Import Namespace="Kooboo.CMS.Content.Models" %>
<%@ Import Namespace="Kooboo.CMS.Content.Services" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/PageDesign/Design.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<FolderTreeNode<TextFolder>>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function getDesignerType(types) { return types['folder']; }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h6>
        <%="Select a data folder:".Localize()%></h6>
    <div class="select-tree">
        <%:Html.Partial("DataRuleFolderGrid", Model)%>
    </div>
    <div class="common-form datafolder-form">
        <fieldset>
            <table>
                <tbody>
                    <tr>
                        <th>
                            <label>
                                <%="Result Type".Localize()%></label>
                                <a href="javascript:;" class="tooltip-link" title="<%:"Contents to display <br /> <b>List</b>: A list of content from the folders. <br/><b>TOP</b>: Max number of return content items<br/><b>Detail</b>: One content item based on UserKey condition. <br/>Use {UserKey} to match \"UserKey\" value in the URL query string".Localize() %>">
                                </a>
                        </th>
                        <td>
                            <p class="clearfix">
                                <input id="List" value="List" type="radio" name="Type" /><label for="List">
                                    <%="List".Localize()%></label>
                                <span id="top_con" style="display: none;">
                                    <label for="Top">
                                        <%="Top:".Localize()%></label><input id="Top" name="Top" type="text" class="mini"
                                            value="50" />
                                </span>
                            </p>
                            <p class="clearfix">
                                <input id="Detail" value="Detail" type="radio" name="Type" /><label for="Detail">
                                    <%="Detail".Localize()%></label>
                                <span id="userkey_con" style="display: none;">
                                    <label for="UserKey">
                                        <%="UserKey=".Localize()%></label>
                                    <input id="UserKey" name="UserKey" type="text" value="{UserKey}" class="mini" source="<%=ViewData["UserKeyQuery"]%>"
                                        style="width: 160px;" />
                                </span>
                            </p>
                        </td>
                    </tr>
                </tbody>
            </table>
        </fieldset>
    </div>
</asp:Content>
