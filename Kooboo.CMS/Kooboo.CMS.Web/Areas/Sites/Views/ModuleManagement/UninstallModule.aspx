<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h3 class="title">
            <%: "Remove files".Localize()%></h3>
        <ul class="step clearfix">
          
            <li class="current">
                <%: "Remove files".Localize()%></li>
            <li>&gt;</li>
            <li>
                <%: "Finish".Localize()%></li>
        </ul>
        <div class="step-content">
            <div class="block">
                <%= "Click <b>Next</b> to remove module files.".Localize() %>
            </div>
            <%using (Html.BeginForm())
              {%>
            <p class="buttons">
                <button type="submit" class="button step-link">
                    <%:"Next".Localize()%>
                    »</button>
            </p>
            <%} %>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
