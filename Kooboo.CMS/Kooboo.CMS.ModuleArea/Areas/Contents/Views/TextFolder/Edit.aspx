<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Content.Models.TextFolder>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.HiddenFor(m=>m.Name) %>
        <%:Html.ValidationSummary(true) %>
        <fieldset>
            <table>
                <tbody>
                    <%:Html.DisplayFor(m => m.Name, new { @class = "width200" })%>
                    <%:Html.EditorFor(m => m.DisplayName, new { @class = "width200" })%>
                    <%:Html.EditorFor(m => m.SchemaName, new { @class = "width200" })%>
                    <%:Html.HiddenFor(m => m.FullName, new { @class = "width200" })%>
                </tbody>
            </table>
        </fieldset>
        <fieldset>
            <legend class="clickable"><span>
                <%:"Permission Setting".Localize() %></span> </legend>
            <table>
                <tbody>
                    <tr>
                        <th>
                        </th>
                        <td>
                            <input type="radio" name="EnabledWorkflow" value="False" id="EnabledWorkflow_False"
                                checked="checked" <%:!Model.EnabledWorkflow?"checked='checked'":"" %> />
                            <label for="EnabledWorkflow_False">
                                <%:"Role base".Localize() %>&nbsp;&nbsp;&nbsp;&nbsp;</label>
                            <input type="radio" name="EnabledWorkflow" value="True" id="EnabledWorkflow_True"
                                <%:Model.EnabledWorkflow?"checked='checked'":"" %> />
                            <label for="EnabledWorkflow_True">
                                <%:"Workflow".Localize() %>&nbsp;&nbsp;&nbsp;&nbsp;</label>
                        </td>
                    </tr>
                    <%:Html.EditorFor(m=>m.WorkflowName) %>
                    <%:Html.EditorFor(m=>m.Roles) %>
                </tbody>
            </table>
        </fieldset>
        <fieldset>
            <legend class="clickable relation-folder"><span>
                <%:"Relation folders".Localize()%></span> </legend>
            <table>
                <tbody>
                    <%:Html.EditorFor(m => m.Categories, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.EmbeddedFolders, new { @class = "medium" })%>
                </tbody>
            </table>
        </fieldset>
        <fieldset class="orderSetting">
            <legend class="clickable relation-folder"><span>
                <%:"Content management".Localize()%></span><a href="javascript:;" class="tooltip-link"
                    title="<%: "The settings of content management".Localize() %>"> </a></legend>
            <table>
                <tbody>
                    <%:Html.EditorFor(o=>o.OrderSetting)%>
                    <%:Html.EditorFor(m => m.VisibleOnSidebarMenu)%>
                    <%:Html.EditorFor(m => m.PageSize, new { @class = "medium" })%>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button>
        </p>
        <% } %>
    </div>
    <script language="javascript" type="text/javascript">


        $(function () {
            var contentType = $('#SchemaName').change(function () {
                handleContentType($(this));
            });

            function handleContentType(contentType) {
                if (!contentType.val()) {
                    $('#EnabledWorkflow_True').hide().next().hide();
                    $('legend.relation-folder').hide().next().hide();
                } else {
                    $('#EnabledWorkflow_True').show().next().show();
                    $('legend.relation-folder').show();
                }
            }

            handleContentType(contentType);

            function initPermission(radio) {

                var effect, workflowSetting = $('#WorkflowName'),
                roleSetting = $('#Roles');
                if (radio.filter('[checked]').val() == 'True') {
                    workflowSetting.removeAttr('disabled').parents('tr:eq(0)').show(effect);
                    roleSetting.parents('tr:eq(0)').hide(effect).find('select').attr('disabled', 'disabled');
                } else {
                    roleSetting.parents('tr:eq(0)').show(effect).find('select.select-data-field').removeAttr('disabled');
                    workflowSetting.attr('disabled', 'disabled').parents('tr:eq(0)').hide(effect);
                }
            }

            var permissionToggle = $(':radio[name=EnabledWorkflow]').change(function () {
                initPermission(permissionToggle);
            });

            initPermission(permissionToggle);
        });
    </script>
</asp:Content>
