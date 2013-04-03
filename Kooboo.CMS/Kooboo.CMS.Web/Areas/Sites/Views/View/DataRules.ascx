<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Kooboo.CMS.Sites.Models.DataRuleSetting>>" %>
<div class="task-block datarules">
	<h3 class="title">
		<%="DataRules".Localize() %><span class="arrow"></span></h3>
	<div class="content">
		<p class="buttons clearfix  ">
			<button type="button" id="AddDataRuleBtn" title="<%:"Add DataRule".Localize() %>">
				<%:"Add".Localize() %></button>
		</p>
		<div  id="grid-field-container">
			
		</div>
	</div>
</div>
