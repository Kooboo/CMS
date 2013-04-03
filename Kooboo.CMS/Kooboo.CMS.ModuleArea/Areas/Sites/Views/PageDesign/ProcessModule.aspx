<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/PageDesign/Design.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<System.Collections.Specialized.NameValueCollection>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function getDesignerType(types) { return types['module']; }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="clearfix">
        <div class="table-container left" style="width: 320px;">
            <table>
                <caption>
                    <%:"Please select a module".Localize() %></caption>
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
                            <input type="checkbox" name="ModuleName" id="<%=item["ModuleName"]%>" value="<%=item["ModuleName"]%>" />
                            <input type="hidden" id="<%=item["ModuleName"]%>EntryAction" value="<%=item["EntryAction"]%>" />
                            <input type="hidden" id="<%=item["ModuleName"]%>EntryController" value="<%=item["EntryController"]%>" />
                            <input type="hidden" id="<%=item["ModuleName"]%>EntryOptions" value="<%=item["EntryOptions"]%>" />
                        </td>
                        <td>
                            <label for="<%=item["ModuleName"]%>">
                                <%=item["ModuleName"]%></label>
                        </td>
                    </tr>
                    <%}%>
                </tbody>
            </table>
        </div>
        <div class="common-form right">
            <fieldset>
                <table>
                    <tbody>
                        <tr>
                            <th>
                                <label for="exclusive">
                                    <%:"Exclusive".Localize()%></label>
                                <a href="javascript:;" class="tooltip-link" title="<%:"If checked, links within this module will not contains <br />URL parameters of other modules in the same page".Localize() %>">
                                </a>
                            </th>
                            <td>
                                <input type="checkbox" id="exclusive" name="Exclusive" value="True" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <label for="EntryController">
                                    <%:"Entry Options".Localize()%></label>
                            </th>
                            <td>
                                <select id="EntryOptions">
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <label for="EntryController">
                                    <%:"Entry Controller".Localize()%></label>
                                <a href="javascript:;" class="tooltip-link" title="<%:"Initial MVC controller when this module first loaded on a page".Localize() %>">
                                </a>
                            </th>
                            <td>
                                <input type="text" class="medium" id="EntryController" name="Entry.Controller" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <label for="EntryAction">
                                    <%:"Entry Action".Localize()%></label>
                                <a href="javascript:;" class="tooltip-link" title="<%:"Initial MVC action when this module first loaded on a page".Localize() %>">
                                </a>
                            </th>
                            <td>
                                <input type="text" class="medium" id="EntryAction" name="Entry.Action" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </fieldset>
        </div>
    </div>
</asp:Content>
