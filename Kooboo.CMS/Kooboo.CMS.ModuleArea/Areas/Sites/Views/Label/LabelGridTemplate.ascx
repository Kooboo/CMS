<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.Web.Mvc.Grid.GridModel>" %>
<div class="table-container">
    <%
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
            <%var categoryList = (IEnumerable<ElementCategory>)ViewData["CategoryList"]; %>
            <%
            if (categoryList != null)
                foreach (var c in categoryList)
                {
                    if (!string.IsNullOrEmpty(c.Category))
                    { %>
            <tr>
                <td>
                    <input type="checkbox" value="<%:c.Category %>" name="Category" class="select category" />
                </td>
                <td colspan="2">
                    <%:Html.ActionLink(c.Category, "Index", ViewContext.RequestContext.AllRouteValues().Merge("Category", c.Category), new RouteValueDictionary(new { @class="f-icon folder" }))%>
                </td>
            </tr>
            <%}
                } %>
            <% 
            foreach (var item in Model.GridItems)
            {
            %>
            <tr <% if(item.IsAlternatingItem) {%>class="even" <%} %>>
                <%if (Model.Checkable)
                  {%>
                <td>
                    <%if (!(item.DataItem is Kooboo.CMS.Sites.Models.IInheritable) || ((Kooboo.CMS.Sites.Models.IInheritable)item.DataItem).Site == Kooboo.CMS.Sites.Models.Site.Current)
                      {%>
                    <input type="checkbox" name="Selected" class="select label" id="<%= item.Id %>" value="<%= item.Id %>"
                        idproperty="<%=Model.IdPorperty %>" />
                    <%} %>
                </td>
                <%} %>
                <% foreach (var itemValue in item.GetItemValues(ViewContext))
                   {%>
                <td>
                    <%: itemValue.RenderedItemColumnValue%>
                </td>
                <%} %><% foreach (var action in item.GetItemActions(ViewContext))
                         {%>
                <td class="action">
                    <%if (action.Visible)
                      {%>
                    <%: Html.ActionLink(action.DisplayName, action.ActionName, action.RouteValues, new RouteValueDictionary(new
					{
					title = action.DisplayName,
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
            if (Model.GridItems.Count() == 0)
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

                            var categorySelect = $("input:checkbox[name=Category][checked]");

                            if (selected.length == 0 && categorySelect.length == 0) {
                                kooboo.alert(grid.messages.empty);
                                return false;
                            }
                            var selectedStr = "";

                            var selectedModel = [];
                            var selectCategory = [];

                            var trs = [];

                            selected.each(function () {
                                var current = $(this);
                                trs.push(current.parents('tr'));
                                var model = {};
                                model[current.attr('IdProperty')] = current.val();
                                selectedModel.push(model);
                            });

                            categorySelect.each(function () {
                                var current = $(this);
                                selectCategory.push({
                                    Category: current.val()
                                });
                            });

                            kooboo.confirm(grid.messages.confirm, function (result) {
                                if (!result) {
                                    return false;
                                }
                                var tempForm = kooboo.cms.ui.formHelper.tempForm(selectedModel, handle.attr("href"), "model", { method: "post" });

                                tempForm.addData(selectCategory, "categoryList");

                                kooboo.cms.ui.loading().show();

                                tempForm.ajaxSubmit({
                                    success: function (response) {
                                        if (response.Success) {
                                            if (categorySelect.length) {
                                                window.location.reload(true);
                                            } else {
                                                trs.each(function (value) {
                                                    value.fadeOut(function () {
                                                        value.remove();
                                                    });

                                                });
                                            }
                                        } else {
                                            kooboo.cms.ui.messageBox().showResponse(response);
                                        }
                                    }
                                });
                            });

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

                    }
                });
            }
        }
    });
	

   
</script>
