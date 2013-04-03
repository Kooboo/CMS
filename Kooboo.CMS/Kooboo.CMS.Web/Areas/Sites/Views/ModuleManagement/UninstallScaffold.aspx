<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h3 class="title">
            <%: "Module uninstallation".Localize()%></h3>
        <ul class="step clearfix">
         
            <li>
                <%: "Remove files".Localize()%></li>
            <li>&gt;</li>
            <li>
                <%: "Finish".Localize()%></li>
        </ul>
        <div class="step-content">
            <div class="common-form">
                <%= Model.ResultHtml%>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
