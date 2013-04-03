<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%var guid = Guid.NewGuid();
  var viewEngine = (Kooboo.CMS.Sites.View.ITemplateEngine)ViewData["ViewEngine"];


  var codeHelper = Kooboo.CMS.Sites.Services.ServiceFactory.CodeSnippetManager.All(viewEngine.Name);
%>
<div class="task-block codehelper">
    <h3 class="title">
        <span><%="Code helper".Localize() %></span><span class="arrow"></span></h3>
    <div class="content  block-list  viewTools">
        <%:Html.Partial("CodeHelperItem",codeHelper)%>
    </div>
</div>
