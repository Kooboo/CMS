<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Kooboo.Web.Mvc.Grid.PageSizeCommand>>" %>
<%= "Show rows".Localize() %>:
<select>
    <%       
        foreach (var item in Model)
        {
            var current = ViewContext.RequestContext.GetRequestValue(item.PageIndexName);
    %>
    <option value="<%= item.PageIndexValue %>" <%= string.Compare(current,item.PageIndexValue,true)==0? "checked":"" %>
        href="<%: Url.Action(item.ActionName,item.RouteValues) %>">
        <%= item.PageIndexValue %></option>
    <% } %>
</select>
