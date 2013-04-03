<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Services.DiagnosisResult>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "System diagnosis".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%: "System diagnosis".Localize() %></h3>
    <div class="common-form diagnosis">
        <fieldset>
            <legend><%="System info".Localize()%></legend>
            <table>
                <tbody>
                    <%:Html.DisplayFor(m=>m.WebApplicationInformation.MachineName) %>
                    <%:Html.DisplayFor(m=>m.WebApplicationInformation.ApplicationVirtualPath) %>
                    <%:Html.DisplayFor(m=>m.WebApplicationInformation.TrustLevel) %>
                    <%:Html.DisplayFor(m=>m.ContentProvider) %>
                </tbody>
            </table>
        </fieldset>
        <fieldset>
            <legend><%="System diagnosis".Localize()%></legend>
            <table>
                <tbody>
                    <%foreach (var item in Model.DiagnosisItems)
                      {%>
                    <tr>
                        <th>
                            <label>
                                <%:item.Name %></label>
                        </th>
                        <td class="icon">
                            <span class="o-icon <% if(item.Result == DiagnosisResultType.Passed){ %>tick<%} else if(item.Result == DiagnosisResultType.Failed){%>cross<%} else{ %>warning<% } %>">
                                <%:item.Result %></span>
                        </td>
                        <td>
                            <%:item.Message %>
                        </td>
                    </tr>
                    <%} %>
                </tbody>
            </table>
        </fieldset>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
