<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.Web.Mvc.Grid.GridModel>" %>
<div class="table-container">
    <%
        var ns = (Kooboo.CMS.Sites.Services.Namespace<Kooboo.CMS.Sites.Models.View>)ViewData["NameSpace"]??new Kooboo.CMS.Sites.Services.Namespace<Kooboo.CMS.Sites.Models.View>();
        using (Html.BeginForm())
        {%>
    <table>
        <thead>
            <tr>
                <%if (Model.Checkable)
                  {%>
                <th class="checkbox">
                    <input type="checkbox" class="select-all" title='<%= "Select All".Localize() %>' />
                </th>
                <%} %>
                <% foreach (var column in Model.GridColumns)
                   {  %>
                <th class="<%=column.PropertyName.ToLower() %>">
                    <%:column.GetFormattedHeaderText(ViewContext)%>
                </th>
                <%} %>
                <%foreach (var action in Model.GridActions)
                  {%>
                <th class="action">
                    <%: action.DisplayName.Localize()%>
                </th>
                <%} %>
            </tr>
        </thead>
        <tbody>
            <% 
                  var subNsList = ns.ChildNamespaces;
                  foreach (var s in ns.ChildNamespaces)
                  {
            %>
            <tr>
                <td>
                    <input type="checkbox" value="<%:s.FullName %>" idproperty="FullName" class="namespace"
                        disabled="disabled" />
                </td>
                <td colspan="7">
                    <%:Html.ActionLink(s.Name, "Index", ViewContext.RequestContext.AllRouteValues().Merge("ns", s.FullName), new RouteValueDictionary(new { @class="f-icon folder" }))%>
                </td>
            </tr>
            <%
      }
            %>
            <% 
            foreach (var item in Model.GridItems)
            {
            %>
            <tr <% if(item.IsAlternatingItem) {%>class="even" <%} %>>
                <%if (Model.Checkable)
                  {%>
                <td>
                    <%if (item.GetCheckVisible(ViewContext))
                      {%>
                    <input type="checkbox" name="Selected" class="select view" name="<%= item.Id %>" value="<%= item.Id %>"
                        idproperty="<%=Model.IdPorperty %>" />
                    <%} %>
                </td>
                <%} %>
                <% foreach (var itemValue in item.GetItemValues(ViewContext))
                   {%>
                <td>
                    <%: itemValue.RenderedItemColumnValue %>
                </td>
                <%} %><% foreach (var action in item.GetItemActions(ViewContext))
                         {%>
                <td class="action">
                    <%if (action.Visible)
                      {%>
                    <%: Html.ActionLink(action.DisplayName, action.ActionName, action.RouteValues, new RouteValueDictionary(new
					{
					title = action.Title,
					@class =action.Class+" action-"+ action.ActionName.ToLower(),					
                    id = action.ActionName + item.Id,
                    confirm= action.ConfirmMessage,
                    idvalue=item.Id
					}))%>
                    <%}%>
                </td>
                <%
             } %>
            </tr>
            <%
            }
            if (Model.GridItems.Count() == 0 && ns.ChildNamespaces.Count() == 0)
            {%>
            <tr>
                <td colspan="99" class="text-center">
                    <%:"Empty".Localize() %>
                </td>
            </tr>
            <% }
            %>
        </tbody>
    </table>
    <%} %>
</div>
<script language="javascript" type="text/javascript">
    $(function () {
        if (!kooboo.data('kooboo-gridhand-executed')) {
            kooboo.data('kooboo-gridhand-executed', true);
            if (window.gridHandle) {
                gridHandle();
            } else {

                kooboo.namespace("grid");
                kooboo.namespace("grid.messages");
                grid.messages.extend({
                    confirm: '<%:"Are you sure you want to delete these items?".Localize() %>',
                    empty: '<%:"Please select items".Localize() %>',
                    selectedName: '<%: "Name".Localize()%>'
                });
                grid.extend({
                    ready: function () {

                        $("input:checkbox.select-all").change(function () {
                            var table = $(this).parents("table");

                            if ($(this).attr("checked")) {
                                table.find("input:checkbox.select").attr("checked", true);
                            } else {
                                table.find("input:checkbox.select").attr("checked", false);
                            }
                        });

                        $("a.deleteCommand").click(function () {
                            var handle = $(this);

                            var selected = $("input:checkbox[name=Selected][checked]");
                            if (selected.length == 0) {
                                kooboo.alert(grid.messages.empty);
                                return false;
                            }
                            var selectedStr = "";

                            var selectedModel = [];

                            var removedRows = [];

                            selected.each(function () {
                                var current = $(this);
                                removedRows.push(current.parents('tr'));
                                var model = {};
                                model[current.attr('IdProperty')] = current.val();
                                selectedModel.push(model);
                            });

                            grid.executeCommand(handle.attr("href"), handle.attr("confirm") || grid.messages.confirm, selectedModel, removedRows);

                            return false;
                        });

                        $("a.command").click(function () {
                            var handle = $(this);

                            var selected = $("input:checkbox[name=Selected][checked]");
                            if (selected.length == 0) {
                                kooboo.alert(grid.messages.empty);
                                return false;
                            }
                            var selectedStr = "";

                            var selectedModel = [];

                            var removedRows = [];

                            selected.each(function () {
                                var current = $(this);
                                var model = {};
                                model[current.attr('IdProperty')] = current.val();
                                selectedModel.push(model);
                            });
                            grid.executeCommand(handle.attr("href"), handle.attr("confirm") || handle.attr("message"), selectedModel, removedRows);

                            return false;
                        });

                        $("a.exportCommand").click(function () {
                            setTimeout(function () {
                                kooboo.cms.ui.loading().hide();
                            }, 1000);
                            var handle = $(this);

                            var selected = $("input:checkbox[name=Selected][checked]");
                            if (selected.length == 0) {
                                kooboo.alert(grid.messages.empty);
                                return false;
                            }
                            var selectedStr = "";

                            var selectedModel = [];

                            selected.each(function () {
                                var current = $(this);
                                var model = {};
                                model[current.attr('IdProperty')] = current.val();
                                selectedModel.push(model);
                            });

                            var tempForm = kooboo.cms.ui.formHelper.tempForm(selectedModel, handle.attr("href"), "model", { method: "post" });

                            tempForm.submit();

                            return false;
                        });

                        $('a.actionCommand').click(function () {
                            var handle = $(this);

                            var removedRows = [];

                            removedRows.push(handle.parents('tr'));

                            grid.executeCommand(handle.attr("href"), handle.attr("confirm"), {}, removedRows);

                            return false;
                        });
                    },

                    executeCommand: function (url, confirmMessage, postData, removedRows) {
                        kooboo.confirm(confirmMessage, function (result) {
                            if (!result) {
                                return false;
                            }
                            var tempForm = kooboo.cms.ui.formHelper.tempForm(postData, url, "model", { method: "post" });

                            kooboo.cms.ui.loading().show();

                            $.post(url, tempForm.form.serialize(), function (response) {
                                if (response.Success) {
                                    if (response.RedirectUrl) {
                                        location.href = response.RedirectUrl;
                                        return;
                                    }
                                    if (window.afterRemove) {
                                        afterRemove(response);
                                    } else {
                                        removedRows.each(function (value) {
                                            value.fadeOut(function () {
                                                value.remove();
                                            });

                                        });
                                    }
                                    kooboo.cms.ui.messageBox().hide();

                                } else {
                                    setTimeout(function () {
                                        kooboo.cms.ui.messageBox().hide();
                                    }, 30000);

                                }
                                kooboo.cms.ui.messageBox().showResponse(response);
                            });
                        });
                    }
                });
            }
        }
    });
	

   
</script>
