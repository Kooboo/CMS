<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<h3 class="title">
	<%:"Choose Folder".Localize() %></h3>
<ul class="step clearfix">
	<li class="current">
		<%:"Choose Folder".Localize() %></li>
	<li>&gt;</li>
	<li>
		<%:"Edit Filters".Localize() %></li>
</ul>

<%:Html.Hidden("FolderInfoUrl",this.Url.Action("GetFolderInfo",ViewContext.RequestContext.AllRouteValues())) %>
<div id="DataRuleFolderGrid" class="select-tree">
	<%:Html.Partial("DataRuleFolderGrid",Model) %>
</div>
<div class="common-form">
	<fieldset>
		<table>
			<tbody>
				<tr>
					<th>
						<label>
							<%:"Query type".Localize() %></label>
					</th>
					<td>
						<p class="clearfix">
							<input type="radio" id="list" name="TakeOperation" value="0" checked dataruletypevalue="0" />
							<label for="list">
							<%:"List of contents in the selected folder".Localize() %>
								</label>
						</p>
						<p class="clearfix">
							<input type="radio" id="First" name="TakeOperation" value="1" dataruletypevalue="0" />
							<label for="First">
								<%:"First or default (one content item)".Localize() %></label>
						</p>
						<%--<p>
							<input type="radio" id="Category" name="TakeOperation" value="0" dataruletypevalue="2" />
							<label for="Category">
								<%:"List of contents in the category folders".Localize() %></label>
						</p>--%>
					</td>
				</tr>
			</tbody>
		</table>
	</fieldset>
</div>
<p class="buttons">
	<button type="submit" id="DataRuleStep1SubmitBtn">
		<%:"Next".Localize() %> »
	</button>
</p>

<script language="javascript" type="text/javascript">
	kooboo.cms.sites.datarule.step1.initStep1();
</script>
