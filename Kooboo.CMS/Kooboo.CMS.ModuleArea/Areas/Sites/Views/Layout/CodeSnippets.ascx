<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% 
    var viewEngine = (Kooboo.CMS.Sites.View.ITemplateEngine)ViewBag.ViewEngine;
%>
<ul class="codeSnippets">
    <li><a href="javascript:;" title="<%:viewEngine.GetCodeHelper().RegisterHtmlMeta() %>">
        <%:"Register HtmlMeta".Localize() %></a> </li>
    <li><a href="javascript:;" title="<%:viewEngine.GetCodeHelper().RegisterStyles() %>">
        <%:"Register Style".Localize()%></a> </li>
    <li><a href="javascript:;" title="<%:viewEngine.GetCodeHelper().RegisterScripts() %>">
        <%:"Register Script".Localize()%>
    </a></li>
    <li><a href="javascript:;" title="<%:viewEngine.GetCodeHelper().RegisterTitle() %>">
        <%:"Register Title".Localize()%></a> </li>
</ul>
