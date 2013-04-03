<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Content.Services.CategoryContents>" %>
<%@ Import Namespace="Kooboo.CMS.Content.Models" %>
<% var categoryFolder = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(Model.CategoryFolder); %>
<tr>
    <td>
        <label>
            <%: string.IsNullOrEmpty(categoryFolder.DisplayName) ? categoryFolder.Name : categoryFolder.DisplayName%>
        </label>
    </td>
    <td>
        <div class="clearfix category-container">
            <div class="category-item-template hide clearfix" style="padding: 2px;">
                <label class="text" style="width: 450px; overflow: hidden; white-space: nowrap">
                </label>
                <a href="javascript:;" class="remove o-icon">remove</a>
            </div>
        </div>
        <%: Html.Hidden("cat_" + Model.CategoryFolder.FullName + "_display", Model.Display, new { @readonly = true })%>
        <%: Html.Hidden("cat_" + Model.CategoryFolder.FullName + "_value", Model.Value)%>
        <%: Html.Hidden("cat_" + Model.CategoryFolder.FullName + "_value_old", Model.Value)%>
        <%: Html.Hidden("cat_" + Model.CategoryFolder.FullName + "_single_choice", Model.SingleChoice)%>
        <a href="<%=Url.Action("SelectCategories",  new { siteName = ViewContext.RequestContext.GetRequestValue("siteName"), repositoryName = ViewContext.RequestContext.GetRequestValue("RepositoryName"), folderName = Model.CategoryFolder.FullName, singleChoice = Model.SingleChoice })%>"
            title="Select <%:categoryFolder.FriendlyText %>" class="categoryButton button">Select...</a>

    </td>
</tr>
