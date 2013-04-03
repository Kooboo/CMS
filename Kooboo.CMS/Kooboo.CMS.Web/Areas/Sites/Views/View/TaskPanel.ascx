<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Sites.Models.View>" %>
<%
    var model = Kooboo.CMS.Web.Models.ModelHelper.ParseViewData<Kooboo.CMS.Sites.Models.View>(Model);
%>
<div class="task-panel">
    <%if (!string.IsNullOrEmpty(Kooboo.CMS.Sites.Models.Site.Current.Repository))
      {%>
    <%:Html.Partial("DataRules",model.DataRules) %>
    <%} %>
    <div class="task-block views">
        <h3 class="title">
            <span><%="Views".Localize() %></span><span class="arrow"></span></h3>
        <div class="content block-list viewTools">
            <%:Html.Partial("ViewList",Kooboo.CMS.Web.Areas.Sites.Models.ViewDataSource.GetNamespace(ViewContext.RequestContext.GetRequestValue("name")))%>
        </div>
    </div>
    <%:Html.EditorFor(o=>o.Parameters) %>
    <%:Html.EditorFor(o=>o.Plugins) %>
    <%:Html.Partial("CodeHelper") %>
</div>
