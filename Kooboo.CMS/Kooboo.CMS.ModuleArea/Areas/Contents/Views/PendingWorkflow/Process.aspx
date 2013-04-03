<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Content.Models.PendingWorkflowItem>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary(true)%>
        <%:Html.HiddenFor(o=>o.WorkflowName) %>
        <%:Html.HiddenFor(o=>o.RoleName) %>
        <%:Html.HiddenFor(o=>o.Name) %>
        <fieldset>
            <table>
                <tbody>
                    <%:Html.DisplayFor(o=>o.WorkflowItemSequence) %>
                    <%:Html.DisplayFor(o=>o.ItemDisplayName) %>
                    <%:Html.DisplayFor(o=>o.WorkflowName) %>
                    <%:Html.DisplayFor(o=>o.RoleName) %>
                    <%:Html.DisplayFor(o=>o.CreationUser) %>
                    <%:Html.DisplayFor(o=>o.CreationUtcDate) %>
                    <%:Html.DisplayFor(o=>o.PreviousComment) %>
                    <tr>
                        <th>
                            <label>
                                <%:"Comment".Localize() %></label>
                        </th>
                        <td>
                            <%:Html.TextArea("Comment", new { rows=5 })%>
                        </td>
                    </tr>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit" id="btn_Passed" name="Passed" value="true">
                <%:"Process".Localize() %>
            </button>
            <%if (Model.WorkflowItemSequence > 1)
              {  %>
            <button type="submit" id="btn_Reject" name="Passed" value="false">
                <%:"Reject".Localize() %>
            </button>
            <%} %>
            <a href="javascript:;" class="dialog-close button">
                <%:"Cancel".Localize() %></a>
        </p>
        <%} %>
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {
            kooboo.cms.ui.event.ajaxSubmit(function () {
                kooboo.data('parent-page-reload', true);
            });

            $("#btn_Passed,#btn_Reject").click(function () {
                return confirm('<%: "Are you sure to treat the workflow item?".Localize() %>');
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
