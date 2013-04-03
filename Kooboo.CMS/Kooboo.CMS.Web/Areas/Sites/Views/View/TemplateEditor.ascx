<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Sites.Models.View>" %>
<tr>
    <td>
        <%=Html.TextAreaFor(o=>o.Body, new { rows = 20 ,cols=20 }) %>
    </td>
    <td style="width: 250px;">
        <%=Html.Partial("TaskPanel", Model)%>
    </td>
</tr>
