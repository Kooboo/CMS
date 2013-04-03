<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Sites.Models.CopyPageModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    CopyPage
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% 
            var model = Kooboo.CMS.Web.Models.ModelHelper.ParseViewData<Kooboo.CMS.Web.Areas.Sites.Models.CopyPageModel>(Model);

            var fullName = ViewContext.RequestContext.GetRequestValue("SourcePage");

            var sourceFullName = Kooboo.CMS.Sites.Models.PageHelper.SplitFullName(fullName);

            string name = "CopyOf" + sourceFullName.Last();

            string parentFullName = "";

            if (sourceFullName.Count() > 1)
            {
                var parentPage = sourceFullName.Take(sourceFullName.Count() - 1);
                parentFullName = Kooboo.CMS.Sites.Models.PageHelper.CombineFullName(parentPage);

            }


            using (Html.BeginForm())
            { %>
        <%:Html.Hidden("SiteName", Request["SiteName"])%>
        <fieldset>
            <table id="positionFormTable">
                <%:Html.EditorFor(m=>m.Name) %>
                <%:Html.EditorFor(m => m.ParentPage, new { @class = "long" })%>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize()%></button>
        </p>
        <% } %>
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {
            var option = $('#ParentPage').val('<%:parentFullName %>')//.find('option[value="' + '<%:parentFullName %>' + '"]').attr('selected', true);
            $('#Name').val('<%:name %>');
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
