<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
	Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Sites.Models.CreateSiteModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%:"Create".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="common-form site-form clearfix">
		<div class="left">
			<div class="command clearfix">
				<select class="short">
					<option>unkown</option>
				</select>
				<div class="search-panel">
					<div class="common-search">
						<input type="text" name="key" class="short" />
						<button type="submit">
							Search</button>
					</div>
				</div>
			</div>
			<div class="template clearfix">
				<div class="template-item">
					<label for="template-default">
						<img src="/Images/template/default.png" alt="Default template" />
					</label>
					<p>
						<input type="radio" name="template" id="template-default" />
						<label for="template-default" class="radio-label">
							Default template</label>
					</p>
				</div>
				<div class="template-item">
					<label for="template-1">
						<img src="/Images/template/default.png" alt="Default template" />
					</label>
					<p>
						<input type="radio" name="template" id="template-1" />
						<label for="template-1" class="radio-label">
							Template1</label>
					</p>
				</div>
				<div class="template-item">
					<label for="template-2">
						<img src="/Images/template/default.png" alt="Default template" />
					</label>
					<p>
						<input type="radio" name="template" id="template-2" />
						<label for="template-2" class="radio-label">
							Template2</label>
					</p>
				</div>
				<div class="template-item">
					<label for="template-3">
						<img src="/Images/template/default.png" alt="Default template" />
					</label>
					<p>
						<input type="radio" name="template" id="template-3" />
						<label for="template-3" class="radio-label">
							Template3</label>
					</p>
				</div>
				<div class="template-item">
					<label for="template-4">
						<img src="/Images/template/default.png" alt="Default template" />
					</label>
					<p>
						<input type="radio" name="template" id="template-4" />
						<label for="template-4" class="radio-label">
							Template4</label>
					</p>
				</div>
				<div class="template-item">
					<label for="template-5">
						<img src="/Images/template/default.png" alt="Default template" />
					</label>
					<p>
						<input type="radio" name="template" id="template-5" />
						<label for="template-5" class="radio-label">
							Template5</label>
					</p>
				</div>
			</div>
		</div>
		<div class="right">
			<% using (Html.BeginForm())
	  { %>
			<%:Html.ValidationSummary(true) %>
			<fieldset>
				<legend></legend>
				<table>
					<tbody>
						<tr>
							<td>
								<label>
									Name</label>
								<input type="text" />
							</td>
						</tr>
						<tr>
							<td>
								<label>
									Display name</label>
								<input type="text" />
							</td>
						</tr>
						<tr>
							<td>
								<label>
									Database</label>
								<a class="o-icon add" href="#" title="Create new">Create new</a>
								<select>
									<option>Site1</option>
								</select>
							</td>
						</tr>
					</tbody>
				</table>
			</fieldset>
			<%}%>
		</div>
	</div>
	<p class="buttons">
		<button type="submit">
			<%:"Save".Localize() %></button></p>
	<script language="javascript" type="text/javascript">
		$(function () {
			$('.step-link').click(function () {
				target = $(this).attr('href');
				currentStep = target.replace('#', '.step-');
				$('.step-content').addClass('hide');
				$(target).removeClass('hide');
				$('.step li.current').removeClass('current');
				$(currentStep).addClass('current');
				return false;
			})
		});
	</script>
</asp:Content>
