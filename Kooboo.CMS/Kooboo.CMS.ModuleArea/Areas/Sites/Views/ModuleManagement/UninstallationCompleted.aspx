<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h3 class="title">
            <%: "Finish".Localize()%></h3>
        <ul class="step clearfix">            
            <li>
                <%: "Remove files".Localize()%></li>
            <li>&gt;</li>
            <li class="current">
                <%: "Finish".Localize()%></li>
        </ul>
        <div class="step-content">
            <div class="block">
                <%= "Uninstallation has been completed. Click on the <b>Finish</b> button to return to the Module page.".Localize() %>
            </div>
            <p class="buttons">
                <button type="submit" class="button step-link" id="btnClose">
                    <%:"Finish".Localize()%></button>
            </p>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#btnClose").click(function () {
                window.top.location.reload(true);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
