<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
	Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Sites.Models.UploadAssemblyViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Upload
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="common-form">
		<form action="<%:this.Url.Action("Upload",ViewContext.RequestContext.AllRouteValues()) %>"
		enctype="multipart/form-data" method="post">
		<%:Html.ValidationSummary(true) %>
		<%:Html.Hidden("success",Request.HttpMethod.ToLower() == "post" && ViewData.ModelState.IsValid) %>
		<fieldset>
			<legend></legend>
			<table>
				<%:Html.EditorFor(m=>m.File) %>
			</table>
		</fieldset>
		<p class="buttons">
			<button type="submit">
				Save</button>
		</p>
		</form>
	</div>
	<script language="javascript" type="text/javascript">

//		function formHandle() {
//			$(function () {
//				var success = $("#success").val();
//				if (success.trim().toString().toLowerCase() == 'true') {
//					top.location.reload();
//				}
//			});
//		}
	</script>
</asp:Content>
