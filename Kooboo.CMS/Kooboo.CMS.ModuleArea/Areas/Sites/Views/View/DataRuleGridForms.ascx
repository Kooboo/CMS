<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Kooboo.CMS.Sites.Models.DataRuleSetting>>" %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<%@ Import Namespace="Kooboo.CMS.Sites.Models" %>
<%var guid = Guid.NewGuid();
  var model = Kooboo.CMS.Web.Models.ModelHelper.ParseViewDataToList<Kooboo.CMS.Sites.Models.DataRuleSetting>(Model)
      .Where(it => it.DataRule.IsValid(Kooboo.CMS.Sites.Models.Site.Current.GetRepository()));
  var viewEngine = (Kooboo.CMS.Sites.View.ITemplateEngine)ViewData["ViewEngine"];
%>
<div class="grid-field">
    <ul class="list">
        <%foreach (var m in model)
          {
              var codeSnippetClean = viewEngine.GetDataRuleCodeSnippet(m.TakeOperation).Generate(Kooboo.CMS.Sites.Models.Site.Current.GetRepository(), m, false);
              var codeSnippetInline = viewEngine.GetDataRuleCodeSnippet(m.TakeOperation).Generate(Kooboo.CMS.Sites.Models.Site.Current.GetRepository(), m, true);
        %>
        <li dataname="<%:m.DataName %>" class="datarule-row"><span class="DataName left">
            <%:m.DataName %>
        </span><a id="remove-<%:m.DataName+guid %>" href="javascript:;" contentid="pop-form-<%:m.DataName %>-<%:guid %>"
            class="o-icon remove pop-remove right" confirmmsg="<%:"Are you sure you want to remove this?".Localize() %>">
        </a><a id="edit-<%:m.DataName+guid %>" title="<%:"Edit".Localize() %>" href="javascript:;"
            contentid="pop-form-<%:m.DataName %>-<%:guid %>" class="o-icon edit inline-action pop-edit right">
        </a><a class="o-icon csharp inline-action right" href="javascript:;" codesnippet="<%: codeSnippetClean%>"
            title="<%: "Html code".Localize() %>">Source</a> <a class="o-icon design inline-action right"
                href="javascript:;" codesnippet="<%:codeSnippetInline %>" title="<%: "Html With Inline editing".Localize() %>">
                Source</a> </li>
        <%} %>
    </ul>
</div>
<div class="form-field">
    <%foreach (var m in model)
      {%>
    <div id="pop-form-<%:m.DataName %>-<%:guid %>" class="pop-form-edit" style="display: none">
        <%:Html.Partial("DataRuleStep2",m) %>
    </div>
    <% } %>
</div>
<script language="javascript" type="text/javascript">
    $(function(){
    $('a[href="javascript:;"]').click(function(){
        return false;
    });
    var textArea = $('textarea');
    var codeMirrorAPI = textArea.data("codeMirror");
    $('a[codesnippet]').click(function(){
        var handle =$(this);
        var text = codeMirrorAPI.getText();

        codeMirrorAPI.insertAtCursor(handle.attr('codesnippet'));

    });
    });
	kooboo.data("datarule-list",<%=model.ToJSON() %>);
</script>
