<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Sites.Models.View>" %>
<li class="viewitem">
    <%var parameterTemplateDelayRender = true;%>
    <%var id = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(16);%>
    <input type="checkbox" id="<%=id%>" value="<%=Model.Name%>" name="ViewName" />
    <label class="viewname" for="<%=id%>">
        <%=Model.Name%></label>
    <%if (Model.ParameterTemplate != null) {%>
    <div class="parameterTemplate" style="display: none;">
        <%if (parameterTemplateDelayRender) {%>
        <%=HttpUtility.UrlEncode(Html.Partial(Model.ParameterTemplate, Model).ToHtmlString())%>
        <%} else { %>
        <%=Html.Partial(Model.ParameterTemplate, Model)%>
        <%}%>
    </div>
    <%} else if (Model.Parameters != null && Model.Parameters.Count > 0) {%>
    <div class="definedParameters" style="display: none;">
        <table>
            <%var index = -1;
              foreach (var p in Model.Parameters) {
                  index++;%>
            <tr>
                <th>
                    <%=p.Name%>:
                </th>
                <td>
                    <input type="hidden" no_submit_name="Parameters[<%=index%>].Name" value="<%=p.Name%>" />
                    <input type="hidden" no_submit_name="Parameters[<%=index%>].DataType" value="<%=p.DataType%>" />
                    <%if (p.DataType == Kooboo.Data.DataType.Bool) {
                          var check = (p.Value != null && p.Value.ToString().ToLower() == "true") ? "checked=\"checked\"" : "";
                    %>
                    <input no_submit_name="Parameters[<%=index%>].Value" type="checkbox" value="true" <%=check%> />
                    <%} else {%>
                    <input no_submit_name="Parameters[<%=index%>].Value" type="text" value="<%=p.Value is DateTime? ((DateTime)p.Value).ToLocalTime().ToShortDateString():p.Value%>" />
                    <%}%>
                </td>
            </tr>
            <%}%>
        </table>
    </div>
    <%}%>
</li>
