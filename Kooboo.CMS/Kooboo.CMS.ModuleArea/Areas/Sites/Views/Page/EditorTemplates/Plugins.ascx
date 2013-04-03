<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<%
    var fullPropName = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(".", "_");
    ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.'); %>
<%if (ViewData.ModelMetadata.GetDataSource().GetSelectListItems(ViewContext.RequestContext).Count() > 0)
  {%>
<%: Html.ValidationMessage(ViewData.ModelMetadata,null) %>
<div class="task-block plugins">
    <div class="command">
        <%: Html.DropDownList(ViewData.ModelMetadata.PropertyName, ViewData.ModelMetadata.GetDataSource().GetSelectListItems(ViewContext.RequestContext).SetActiveItem(Model), new { multiple = "multiple" })%>
        <a class="button add" href="javascript:;" title="<%="Select Plugin".Localize() %>">
            <%="Add".Localize()%></a>
    </div>
    <div class="common-form">
        <div class="table-container">
            <table>
                <thead>
                    <tr>
                        <th>
                            <%:"Name".Localize() %>
                        </th>
                        <th class="action">
                            <%:"Remove".Localize() %>
                        </th>
                    </tr>
                </thead>
                <tbody id="plugin-tbody">
                </tbody>
            </table>
        </div>
    </div>
</div>
<script language="javascript" type="text/javascript">
    $(function initPlugins() {
        var tbody = $("#plugin-tbody");
        var mutipleDropdown = $("#Plugins");

        initDisplayList();
        function initDisplayList() {
            tbody.html('');
            if (mutipleDropdown.val() != null) {
                var valArray = mutipleDropdown.val();
                valArray.each(function (value, index) {
                    var current = mutipleDropdown.children("option[value='" + value + "']");
                    var tr = $('<tr><td class="span"></td><td> <a href="javascript:;" class="o-icon remove right"></a></td></tr>');
                    var remove = tr.find("a.remove").click(function () {
                        current.attr("selected", false);
                        tr.remove();
                    });

                    tr.find("td.span").html(current.text());
                    tr.appendTo(tbody);
                    tr.data("option", current);
                });
            }
        }
        var addBtn = $("div.plugins a.add");

        var select = $("<select></select>");

        select.html(mutipleDropdown.html());

        mutipleDropdown.css({ position: "absolute", left: '-1000px' }).after(select);

        addBtn.click(function () {

            var added = false;

            var selectedOption = mutipleDropdown.children("option[value='" + select.val() + "']");

            tbody.children().each(function () {
                added = added || ($(this).data("option").val() == selectedOption.val());
            });

            if (added) {
                alert('<%:"Item already existed".Localize() %>');
            } else {
                kooboo.cms.ui.status().stop();
                selectedOption.attr("selected", true);

                initDisplayList();
            }


        });

    });
</script>
<% } %>
