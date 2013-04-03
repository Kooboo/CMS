<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<object>" %>

<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%="Start".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            //Flowmap
            $('.fm-board').flowmap({
                typeset: 'fixed'
            });
        })
    </script>
    <h3 class="title">
        <%:"Start".Localize() %></h3>
    <div class="flow-chart">
        <div class="fm-board">
            <div class="step1">
                <h3>
                    <%="1. Create database".Localize() %></h3>
                <div class="description">
                    <%="Create a library to store your contents".Localize() %>
                </div>
            </div>
            <div class="step2">
                <h3>
                    <%="2. Customize setting".Localize()%>
                </h3>
                <div class="description">
                    <%="Create folders or content types with custom fields".Localize() %>
                </div>
            </div>
            <div class="step3">
                <h3>
                    <%="3. Add content ".Localize() %></h3>
                <div class="description">
                    <%="Create text content <br /> or upload media files".Localize() %>
                </div>
            </div>
            <div class="fm-node flow-item" id="Repository" parentid="" xy="20,100" turn="60">
                <a href="#" class="pop" title="<%="Create Database".Localize() %>">
                    <%="Create database".Localize() %></a>
                <div class="shade">
                </div>
            </div>
            <div class="fm-node flow-item" id="ContentType" parentid="Repository" xy="260,20">
                <a href="#">
                    <%="Add content type".Localize() %></a>
                <div class="shade">
                </div>
            </div>
            <div class="fm-node flow-item" id="TextFolder" parentid="Repository,ContentType"
                xy="260,100">
                <a href="#" title="<%:"Add TextFolder".Localize() %>">
                    <%:"Add text folder".Localize() %></a>
                <div class="shade">
                </div>
            </div>
            <div class="fm-node flow-item" id="TextContent" parentid="TextFolder" xy="510,100">
                <a href="#">
                    <%:"Add text content".Localize() %></a>
                <div class="shade">
                </div>
            </div>
            <div class="fm-node flow-item" id="MediaContent" parentid="Repository" xy="510,180">
                <a href="#">
                    <%:"Add media files".Localize() %></a>
                <div class="shade">
                </div>
            </div>
        </div>
        <ul class="flow-description">
            <li><span class="color-block white"></span>
                <%="Available".Localize()%></li>
            <li><span class="color-block grey"></span>
                <%="Not avaliable".Localize() %></li>
            <li><span class="color-block green"></span>
                <%="Completed".Localize() %></li>
        </ul>
    </div>
    <script language="javascript" type="text/javascript">
		//kooboo.cms.content.home.init(<%=Model.ToJSON() %>);

		$(function () {
			var model = <%=Model.ToJSON() %>;
			var statusDic = ["disabled", "enabled", "done"];
			var popConfigDic = { "Repository": { } ,"TextFolder":{}};
			for (var p in model) {
				var step = $("#" + p).addClass(statusDic[model[p].Status]);
				if (model[p].ActionUrl) {
					var a = step.find("a").attr("href", model[p].ActionUrl);
					if (a.hasClass("pop")) {
						a.pop(popConfigDic[p]);
					}
				}

			}
		});

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
