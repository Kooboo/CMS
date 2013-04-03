<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DateTime>" %>
<tr>
    <th>
        <label for="<%: ViewData.ModelMetadata.PropertyName%>">
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
        <% if(!string.IsNullOrEmpty(ViewData.ModelMetadata.Description)) {%>
        <a href="#" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description %>">
        </a>
        <%} %>
    </th>
    <td>    
        <%if (Model.Kind == DateTimeKind.Utc)
      { %>
        <%: Model.ToLocalTime()%>
        <%}
      else { %>
        <%: Model%>
        <%}  %>
    </td>
</tr>
