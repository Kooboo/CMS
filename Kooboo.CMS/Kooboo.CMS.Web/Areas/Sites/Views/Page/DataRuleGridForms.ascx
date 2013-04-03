<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Kooboo.CMS.Sites.Models.DataRuleSetting>>" %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<%@ Import Namespace="Kooboo.CMS.Sites.Models" %>
<%var guid = Guid.NewGuid();
  var model = Kooboo.CMS.Web.Models.ModelHelper.ParseViewDataToList<Kooboo.CMS.Sites.Models.DataRuleSetting>(Model)
            .Where(it => it.DataRule.IsValid(Kooboo.CMS.Sites.Models.Site.Current.GetRepository()));
%>
<div class="grid-field">
    <div class="table-container">
        <table>
            <thead>
                <tr>
                    <th class="auto">
                        <%:"DataName".Localize() %>
                    </th>
                    <th class="action">
                        <%:"Edit".Localize() %>
                    </th>
                    <th class="action">
                        <%:"Delete".Localize() %>
                    </th>
                </tr>
            </thead>
            <tbody>
                <%foreach (var m in model)
                  { %>
                <tr dataname="<%:m.DataName %>" class="datarule-row">
                    <td>
                        <%:m.DataName %>
                    </td>
                    <td class="action">
                        <a id="edit-<%:m.DataName+guid %>" title="<%:"Edit".Localize() %>" href="javascript:;"
                            contentid="pop-form-<%:m.DataName %>-<%:guid %>" class="o-icon edit pop-edit">

                        </a>
                    </td>
                    <td class="action">
                        <a id="remove-<%:m.DataName+guid %>" href="javascript:;" contentid="pop-form-<%:m.DataName %>-<%:guid %>"
                            class="o-icon remove pop-remove" confirmmsg="<%:"Are you sure you want to remove this?".Localize() %>">

                        </a>
                    </td>
                </tr>
                <%} %>
            </tbody>
        </table>
    </div>
</div>
<div class="form-field">
    <%foreach (var m in model)
      {%>
    <div id="pop-form-<%:m.DataName %>-<%:guid %>" class="pop-form-edit" style="display: none">
        <%:Html.Partial("DataRuleStep2",m) %>
    </div>
    <% } %>
</div>
<script language="javascript" type="text/javascript">
	kooboo.data("datarule-list",<%=model.ToJSON() %>);
</script>
