<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<bool>" %>
<div class="designer">
    <ul class="actions clearfix">
        <li>
            <input class="o-icon input-icon undo" type="button" value="Undo" id="btnUndo" title="Undo"
                disabled="disabled" /></li>
        <li>
            <input class="o-icon input-icon redo" type="button" value="Redo" id="btnRedo" title="Redo"
                disabled="disabled" /></li>
    </ul>
    <div id="container" class="container" style="position: relative;">
        <div id="iframeLoading">
            <%="Loading...".Localize()%>
        </div>
        <div id="iframeMask">
        </div>
        <%
            var siteName = ViewContext.RequestContext.GetRequestValue("siteName");
            var pageName = ViewContext.RequestContext.GetRequestValue("fullName");
            var create = Model;
            if (create) { pageName = null; }
            // ...
            var resources = new StringBuilder();
            resources.AppendFormat("<script type=\"text/javascript\" src=\"{0}\"></script>", Url.Action("Settings", "PageDesign", new { siteName = siteName })).AppendLine();
            resources.Append(Html.ExternalResources("pageDesignJs").ToHtmlString());
            var js = HttpUtility.HtmlEncode(resources.ToString());
            var css = HttpUtility.HtmlEncode(Html.ExternalResources("pageDesignCss").ToHtmlString());
            var url = HttpUtility.HtmlEncode(Url.Action("Design", "PageDesign", new { siteName = siteName
                , pageName = pageName
                , layout = ViewContext.RequestContext.GetRequestValue("layout")
                , draft = ViewContext.RequestContext.RouteData.Values["action"].ToString().ToLower() == "draft"
                ,version = ViewContext.RequestContext.GetRequestValue("version")
            }));
        %>
        <input name="PagePositionsJson" type="hidden" />
        <input id="PageDesignJs" type="hidden" value="<%=js%>" />
        <input id="PageDesignCss" type="hidden" value="<%=css%>" />
        <input id="PageDesignUrl" type="hidden" value="<%=url%>" />
        <script type="text/javascript">
            var __pageDesign = {
                leaveConfirm: '<%="Are you sure you want to leave page? All your changes might be lost.".Localize()%>',
                undoBtnTitle: '<%="Undo".Localize()%>',
                redoBtnTitle: '<%="Redo".Localize()%>'
            };
        </script>
        <script type="text/javascript" src="<%=Url.Content("~/Areas/Sites/Scripts/pageDesign/nest.js")%>"></script>
    </div>
</div>
