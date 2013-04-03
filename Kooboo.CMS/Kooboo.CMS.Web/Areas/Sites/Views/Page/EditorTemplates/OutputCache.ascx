<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Sites.Models.CacheSettings>" %>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
   var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString();
   var model = Kooboo.CMS.Web.Models.ModelHelper.ParseViewData<Kooboo.CMS.Sites.Models.CacheSettings>(Model);
   var guid = Guid.NewGuid();
%>
<tr>
	<th>
		<label for="<%:propertyName %>">
			<%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
		<% if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
	 {%>
		<a href="javascript:;" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description %>">
		</a>
		<%} %>
	</th>
	<td>
		<input type="checkbox" id="<%:propertyName %>-checkbox" <%if(Model!=null){%> checked
			<%} %> />
	</td>
</tr>
<tr class="tr-<%:guid %>">
	<th>
		<label>
			<%:"Duration (seconds)".Localize()%></label>
	</th>
	<td>
		<%:Html.TextBox(propertyName + ".Duration",Model!=null?Model.Duration:120)%>
	</td>
</tr>
<tr class="tr-<%:guid %>">
	<th>
		<label>
			<%:"Expiration Policy".Localize()%></label>
			<a href="javascript:;" title='<%:"<b>AbsouluteExpiration:</b> a cache entry should be evicted after a specified duration. <br /><b>SlidingExpiration:</b> a cache entry should be evicted if it has not been accessed in a given span of time.".Localize() %>' class="tooltip-link"></a>
	</th>
	<td>
		<%:Html.DropDownList(propertyName + ".ExpirationPolicy", Enum.GetNames(typeof(Kooboo.CMS.Sites.Models.ExpirationPolicy)).Select(o => new System.Web.Mvc.SelectListItem() { Text = o, Value = ((int)Enum.Parse(typeof(Kooboo.CMS.Sites.Models.ExpirationPolicy), o)).ToString(), Selected = o== model.ExpirationPolicy.ToString()}))%>
	</td>
</tr>
<script language="javascript" type="text/javascript">
	$(function () {
		var checkbox = $('#<%:propertyName %>-checkbox').click(function () {
			var handle = $(this);
			initChecked(handle);
		});

		function initChecked(handle) {
			if (handle.attr('checked')) {
				$('.tr-<%:guid %>').find('input,select').attr('disabled', false);
			} else {
				$('.tr-<%:guid %>').find('input,select').attr('disabled', true);
			}
		}

		initChecked(checkbox);
	});
</script>
