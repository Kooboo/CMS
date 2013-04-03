<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.Web.Mvc.Menu.Menu>" %>
<ul>
    <%foreach (var item in Model.Items)
      {%>
    <%: Html.Partial("MenuItem", item)%>
    <%} %>
</ul>
