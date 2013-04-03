<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Contents.Models.IndexSummaryModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Index dashboard".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%:"Index data".Localize()%>
    </h3>
    <%: Html.GridForModel(it=>it.FolderIndexInfoes) %>
    <div class="block queue">
        <h3 class="title">
            <%: "Last actions".Localize()%></h3>
        <ul>
            <%foreach (var item in Model.LastActions)
              {%>
            <li>
                <%if (item.Action != Kooboo.CMS.Content.Models.ContentAction.Delete)
                  {%>
                <%: string.Format("Indexed folder '{0}' - content title '{1}'".Localize(), item.FolderName, item.ContentSummary)%>
                <span><%: item.UtcActionDate.ToLocalTime()%></span>
                <%}
                  else
                  {  %>
                <%: string.Format("Delete index content title '{1}' from folder '{0}'".Localize(), item.FolderName, item.ContentSummary)%>
                <span><%: item.UtcActionDate.ToLocalTime()%></span>
                <%} %></li>
            <%} %>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
