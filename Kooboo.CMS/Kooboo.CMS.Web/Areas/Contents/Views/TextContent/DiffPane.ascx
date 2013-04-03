<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IDictionary<string,DiffPlex.DiffBuilder.Model.DiffPaneModel>>" %>
<div class="diffPane">
    <table cellpadding="0" cellspacing="0" class="diffTable">
        <%var lines = 0;
          foreach (var k in Model.Keys)
          {
              foreach (var diffLine in Model[k].Lines)
              {
                  lines = diffLine.Position.HasValue ? lines + diffLine.Position.Value : lines;
        %>
        <tr>
            <td class="lineNumber">
                <%= lines%>
            </td>
            <td>
                <strong>
                    <%:k %>:</strong>
            </td>
        </tr>
        <tr title="<%=k %>">
            <td>
            </td>
            <td class="line <%=diffLine.Type.ToString() %>Line">
                <span class="lineText">
                    <% Html.RenderPartial("DiffLine", diffLine); %>
                </span>
            </td>
        </tr>
        <%}
          } %>
    </table>
</div>
